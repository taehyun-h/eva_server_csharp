using System;

namespace SimpleHttpServer.Models
{
    public class Route
    {
        public string Name;
        public string UrlRegex;
        public string Method;
        public Func<HttpRequest, HttpResponse> Callable;
    }
}
