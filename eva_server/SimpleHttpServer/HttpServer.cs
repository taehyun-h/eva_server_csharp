using SimpleHttpServer.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleHttpServer
{
    public class HttpServer
    {
        private int _port;
        private TcpListener _listener;
        private HttpProcessor _processor;
        private bool _isActive = true;

        public HttpServer(int port, List<Route> routes)
        {
            _port = port;
            _processor = new HttpProcessor();
            foreach (var route in routes)
            {
                _processor.AddRoute(route);
            }
        }

        public void Listen()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            while (_isActive)
            {
                var tcpClient = _listener.AcceptTcpClient();
                var thread = new Thread(() => { _processor.HandleClient(tcpClient); });
                thread.Start();
                Thread.Sleep(1);
            }
        }
    }
}
