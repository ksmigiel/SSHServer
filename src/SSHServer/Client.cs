using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace SSHServer
{
    public class Client
    {
        private Socket _socket;
        private readonly ILogger _logger;

        public Client(Socket socket, ILogger logger)
        {
            _socket = socket;
            _logger = logger;
        }

        public bool IsConnected { get => _socket != null; }

        public void Poll()
        {

        }

        public void Disconnect()
        {
            _logger.LogDebug("Client disconnected");

            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
                catch (System.Exception)
                {
                    throw;
                }

                _socket = null;
            }
        }
    }
}