using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SimpleHttpServer.Models
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode;
        public string Reason;
        public byte[] Content;
        public readonly Dictionary<string, string> Headers = new Dictionary<string, string>();

        public string ContentUTF8
        {
            set => Content = Encoding.UTF8.GetBytes(value);
        }

        public override string ToString()
        {
            return $"HTTP status {StatusCode} {Reason}";
        }
    }
}
