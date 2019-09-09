using System.Collections.Generic;
using System.Linq;

namespace SimpleHttpServer.Models
{
    public class HttpRequest
    {
        public string Method;
        public string Url;
        public string Content;
        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Content) && !Headers.ContainsKey("Content-Length"))
            {
                Headers.Add("Content-Length", Content.Length.ToString());
            }

            return $"{Method} {Url} HTTP/1.0\r\n{string.Join("\r\n", Headers.Select(x => $"{x.Key}: {x.Value}"))}\r\n\r\n{Content}";
        }
    }
}
