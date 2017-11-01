using System.Collections.Generic;
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
        private readonly ClientFactory _clientFactory;
        private readonly List<Client> _clients = new List<Client>();
        private TcpListener _listener;

        public Server(ILoggerFactory loggerFactory, AppConfiguration configuration, ClientFactory clientFactory)
        {
            _configuration = configuration.Configuration;
            _logger = loggerFactory.CreateLogger<Server>();
            _clientFactory = clientFactory;
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
                var socket = _listener.AcceptSocket();

                _logger.LogDebug($"New client connected: {socket.RemoteEndPoint.ToString()}");

                _clients.Add(_clientFactory.CreateClient(socket));
                _clients.ForEach(c => c.Poll());
                _clients.RemoveAll(c => !c.IsConnected);
            }
        }

        public void Stop()
        {
            if (_listener != null)
            {
                _logger.LogInformation("Shutting down...");

                _clients.ForEach(c => c.Disconnect());
                _clients.Clear();

                _listener.Stop();
                _listener = null;

                _logger.LogInformation("Stopped!");
            }
        }
    }
}