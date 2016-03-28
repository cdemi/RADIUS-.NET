using Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;

namespace Tester
{
    class Program
    {
        //01 00 00 38 0f 40 3f 94 73 97 80 57 bd 83 d5 cb 98 f4 22 7a 01 06 6e 65 6d 6f 02 12 0d be 70 8d 93 d4 13 ce 31 96 e4 3f 78 2a 0a ee 04 06 c0 a8 01 10 05 06 00 00 00 03 
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
