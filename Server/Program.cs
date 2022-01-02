using Server;

var echo = new EchoServer();
echo.Start();
Console.WriteLine("EchoServer running");
Console.ReadLine();