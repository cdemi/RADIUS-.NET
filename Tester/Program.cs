using Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint anySender = new IPEndPoint(IPAddress.Any, 0);

            using (UdpClient udpSocket = new UdpClient(6969))
            {
                while (true)
                {
                    var rawRadiusPacket = udpSocket.Receive(ref anySender);
                    var radiusPacket = new RadiusPacket(rawRadiusPacket);
                    Console.WriteLine(JsonConvert.SerializeObject(radiusPacket));
                    Console.WriteLine();
                }
            }
        }
    }
}
