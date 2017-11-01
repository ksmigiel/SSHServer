using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace SSHServer
{
    public class ClientFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public ClientFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public Client CreateClient(Socket socket)
        {
            var logger = _loggerFactory.CreateLogger($"Client: {socket.RemoteEndPoint}");
            return new Client(socket, logger);
        }
    }
}