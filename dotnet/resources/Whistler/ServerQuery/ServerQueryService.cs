using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Whistler.Helpers;

namespace Whistler.ServerQuery
{
    class ServerQueryService
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ServerQueryService));
        private static Commands _commands = new Commands();
        private static TcpListener _listener;
        private static bool _loop = true;
        public ServerQueryService()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Parse(Main.ServerConfig.Query.IpAdress), Main.ServerConfig.Query.Port);
                _listener.Start(10);
                _logger.WriteWarning("Пиздос");
                ListenBegine();
            }
            catch (Exception e)
            {
                _loop = false;
                _listener.Stop();
                _logger.WriteError($"Service not created: {e}");
            }
            
        }

        private async void ListenBegine()
        {
            while (_loop)
            {
                try
                {
                    byte[] data = new byte[256];
                    var request = new StringBuilder();
                    var client = await _listener.AcceptTcpClientAsync();
                    var stream = client.GetStream();
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        request.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    data = Encoding.UTF8.GetBytes(_commands.Call(request.ToString()));
                    stream.Write(data, 0, data.Length);

                    stream.Close();
                    client.Close();

                }
                catch (Exception e)
                {
                    _logger.WriteError(e.ToString());
                }
            }
        }
    }
}
