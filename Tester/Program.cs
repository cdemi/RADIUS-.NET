using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Tester
{
    class Program
    {
        //01 00 00 38 0f 40 3f 94 73 97 80 57 bd 83 d5 cb 98 f4 22 7a 01 06 6e 65 6d 6f 02 12 0d be 70 8d 93 d4 13 ce 31 96 e4 3f 78 2a 0a ee 04 06 c0 a8 01 10 05 06 00 00 00 03 
        static void Main(string[] args)
        {

            using (UdpClient udpSocket = new UdpClient(6969))
            {
                while (true)
                {
                    IPEndPoint anySender = new IPEndPoint(IPAddress.Any, 0);
                    var rawRadiusRequest = udpSocket.Receive(ref anySender);
                    var radiusRequest = new RadiusPacket(rawRadiusRequest);

                    RadiusPacket radiusResponse = new RadiusPacket(RadiusCode.ACCESS_ACCEPT, radiusRequest.Identifier);

                    udpSocket.Send(radiusResponse.ToRawData(), radiusResponse.ToRawData().Length, anySender);
                    Console.WriteLine(JsonConvert.SerializeObject(radiusRequest, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() }));
                    Console.WriteLine();
                }
            }
        }
    }
}
