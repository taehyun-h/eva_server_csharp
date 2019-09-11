using SimpleHttpServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SimpleHttpServer
{
    public class HttpProcessor
    {
        private readonly List<Route> _routes = new List<Route>();

        public void AddRoute(Route route)
        {
            _routes.Add(route);
        }

        public void HandleClient(TcpClient tcpClient)
        {
            var inputStream = tcpClient.GetStream();
            try
            {
                var request = GetRequest(inputStream);
                var response = RouteRequest(request);
                Console.WriteLine("{0} {1} {2}", DateTime.Now, response.StatusCode, request.Url);

                var outputStream = tcpClient.GetStream();
                SendResponse(outputStream, response);
                outputStream.Flush();
                outputStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} {1}", DateTime.Now, e);
            }

            inputStream.Close();
        }

        private HttpResponse RouteRequest(HttpRequest request)
        {
            var routes = _routes.Where(x => Regex.Match(request.Url, x.UrlRegex).Success).ToList();
            if (!routes.Any()) return HttpDefaultBuilder.NotFound();

            var route = routes.SingleOrDefault(x => x.Method == request.Method);
            if (route == null) return HttpDefaultBuilder.MethodNotAllowed();

            try
            {
                var response = route.Callable(request);
                if (response.Content == null && response.StatusCode != HttpStatusCode.OK)
                {
                    response.ContentUTF8 = $"{response.StatusCode} {request.Url} <p> {response.Reason}";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return HttpDefaultBuilder.InternalServerError();
            }
        }

        private HttpRequest GetRequest(Stream inputStream)
        {
            var (method, url) = ParseRequestFirstLine(inputStream);
            var headers = ParseRequestHeader(inputStream);
            var content = GetContent(inputStream, headers);
            return new HttpRequest
            {
                Method = method,
                Url = url,
                Headers = headers,
                Content = content
            };
        }

        private (string, string) ParseRequestFirstLine(Stream inputStream)
        {
            var firstLine = Readline(inputStream);
            var tokens = firstLine.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception($"Invalid http request first line : {firstLine}");
            }

            return (tokens[0].ToUpper(), tokens[1]);
        }

        private Dictionary<string, string> ParseRequestHeader(Stream inputStream)
        {
            var headers = new Dictionary<string, string>();
            for (;;)
            {
                var line = Readline(inputStream);
                if (string.IsNullOrEmpty(line)) break;

                var separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception($"Invalid http header : {line}");
                }

                var key = line.Substring(0, separator++);
                for (; separator < line.Length && line[separator] == ' '; separator++)
                {
                }

                var value = line.Substring(separator, line.Length - separator);
                headers.Add(key, value);
            }

            return headers;
        }

        private string GetContent(Stream inputStream, Dictionary<string, string> headers)
        {
            if (!headers.TryGetValue("Content-Length", out var contentValue)) return string.Empty;

            var totalBytes = Convert.ToInt32(contentValue);
            var bytesLeft = totalBytes;
            var bytes = new byte[totalBytes];
            while (bytesLeft > 0)
            {
                var buffer = new byte[bytesLeft > 1024 ? 1024 : bytesLeft];
                var n = inputStream.Read(buffer, 0, buffer.Length);
                buffer.CopyTo(bytes, totalBytes - bytesLeft);

                bytesLeft -= n;
            }

            return Encoding.ASCII.GetString(bytes);
        }

        private void SendResponse(Stream stream, HttpResponse response)
        {
            if (response.Content == null)
            {
                response.Content = new byte[] { };
            }

            if (!response.Headers.ContainsKey("Content-Type"))
            {
                response.Headers["Content-Type"] = "text/html";
            }

            response.Headers["Content-Length"] = response.Content.Length.ToString();

            Write(stream, $"HTTP/1.0 {response.StatusCode} {response.Reason}\r\n");
            Write(stream, string.Join("\r\n", response.Headers.Select(x => $"{x.Key}: {x.Value}")));
            Write(stream, "\r\n\r\n");

            stream.Write(response.Content, 0, response.Content.Length);
        }

        #region Stream Util

        private static string Readline(Stream stream)
        {
            var data = string.Empty;
            while (true)
            {
                var nextChar = stream.ReadByte();
                if (nextChar == '\n') break;

                switch (nextChar)
                {
                    case '\r':
                    {
                        break;
                    }
                    case -1:
                    {
                        Thread.Sleep(1);
                        break;
                    }
                    default:
                    {
                        data += Convert.ToChar(nextChar);
                        break;
                    }
                }
            }

            return data;
        }

        private static void Write(Stream stream, string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
        }

        #endregion
    }
}
