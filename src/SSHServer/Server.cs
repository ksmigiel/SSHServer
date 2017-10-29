using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SSHServer.Common;

namespace SSHServer
{
    public class Server
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger _logger;
        private TcpListener _listener;

        public Server(ILoggerFactory loggerFactory, AppConfiguration configuration)
        {
            _configuration = configuration.Configuration;
            _logger = loggerFactory.CreateLogger<Server>();
        }

        public void Start()
        {
            Stop();

            _logger.LogInformation("Starting up...");

            var port = int.Parse(_configuration.GetSection("port").Value);

            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            _logger.LogInformation($"Listening on port: {port}");
        }

        public void Poll()
        {
            while (_listener.Pending())
            {
                var acceptSocket = _listener.AcceptSocket();
                
                _logger.LogDebug($"New client connected: {acceptSocket.RemoteEndPoint.ToString()}");
            }
        }

        public void Stop()
        {
            if (_listener != null)
            {
                _logger.LogInformation("Shutting down...");

                _listener.Stop();
                _listener = null;
                
                _logger.LogInformation("Stopped!");
            }
        }
    }
}