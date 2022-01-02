using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class EchoServer
    {
        public void Start(int port = 9000)
        {
            //Where the server is going to listen:
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback, port);

            //Listen on following socket:
            var socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndPoint);
            socket.Listen(128);

            _ = Task.Run(() => DoEcho(socket));
        }

        private async Task DoEcho(Socket socket)
        {
            do
            {
                //Listening on socket for new connections
                //Result of that (BeginAccept / EndAccept) is a new socket (clientSocket)
                var clientSocket = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object?, IAsyncResult>(socket.BeginAccept),
                    new Func<IAsyncResult, Socket>(socket.EndAccept),
                    null).ConfigureAwait(false);

                Console.WriteLine("ECHO SERVER :: CLIENT CONNECTED");

                using var stream = new NetworkStream(clientSocket, true);
                var buffer = new byte[1024]; //1KB

                do
                {
                    int bytesRead = await stream.ReadAsync(buffer).ConfigureAwait(false);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    await stream.WriteAsync(buffer.AsMemory(0, bytesRead)).ConfigureAwait(false);
                } while (true);
            } while (true);
        }
    }
}