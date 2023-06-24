using System.Net;
using System.Net.Sockets;
using System.Text;

IPAddress localAddress = IPAddress.Loopback;
Console.Write("Input name: ");
string name = Console.ReadLine();
Console.Write("Input local port: ");
int portLocal = Int32.Parse(Console.ReadLine());
Console.Write("Input remote port: ");
int portRemote = Int32.Parse(Console.ReadLine());

Task.Run(ReceiveMessageAsync);
await SendMessageAsync();

async Task SendMessageAsync()
{
    //using Socket socketSender = new Socket(AddressFamily.InterNetwork, 
    //                                       SocketType.Dgram, 
    //                                       ProtocolType.Udp);
    using UdpClient clientSender = new UdpClient();

    Console.WriteLine("Input message and press Enter");
    while(true)
    {
        string message = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(message)) break;
        message = $"{name}: {message}";
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        //await socketSender.SendToAsync(buffer, new IPEndPoint(localAddress, portRemote));

        await clientSender.SendAsync(buffer, new IPEndPoint(localAddress, portRemote));
    }
}

async Task ReceiveMessageAsync()
{
    byte[] buffer = new byte[(int)Math.Pow(2, 16) - 1];
    //using Socket socketReciver = new Socket(AddressFamily.InterNetwork,
    //                                 SocketType.Dgram,
    //                                 ProtocolType.Udp);

    using UdpClient clientReceiver = new UdpClient(portLocal);

    //socketReciver.Bind(new IPEndPoint(localAddress, portLocal));

    while(true)
    {
        //var messageByte = await socketReciver.ReceiveFromAsync(buffer, new IPEndPoint(IPAddress.Any, 0));
        var messageByte = await clientReceiver.ReceiveAsync();

        //string message = Encoding.UTF8.GetString(buffer, 0, messageByte.ReceivedBytes);
        string message = Encoding.UTF8.GetString(buffer);
        Console.WriteLine(message);
    }
}