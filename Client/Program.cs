using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press Enter to connect");
            Console.ReadLine();

            //Connect to:
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 9000);
            var socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipEndPoint);
            var networkStream = new NetworkStream(socket, true);

            var msg = "Hello World";
            var buffer = Encoding.UTF8.GetBytes(msg);
            await networkStream.WriteAsync(buffer, 0, buffer.Length);

            var response = new byte[1024];

            var bytesRead = networkStream.Read(response, 0, response.Length);
            var responseStr = Encoding.UTF8.GetString(response);
            Console.WriteLine($"Received: {responseStr}");

            //var myMessage = new MyMessage
            //{
            //    IntProperty = 404,
            //    StringProperty = "Hello World"
            //};

            //Console.WriteLine("Sending");
            //Print(myMessage);

            //await SendAsync(networkStream, myMessage).ConfigureAwait(false);

            //var responseMsg = await ReceiveAsync<MyMessage>(networkStream).ConfigureAwait(false);

            //Console.WriteLine("Received");
            //Print(responseMsg);

            Console.ReadLine();
        }
    }
}