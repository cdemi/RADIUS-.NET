using Logic;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Models
{
    public class RadiusPacket
    {
        public readonly RadiusCode Code;
        public readonly byte Identifier;
        public readonly ushort Length;
        public readonly string SharedSecret;
        public readonly byte[] RequestAuthenticator;
        public readonly List<RadiusAttribute> Attributes = new List<RadiusAttribute>();

        public RadiusPacket(byte[] rawData)
        {
            this.Code = (RadiusCode)rawData[0];
            Identifier = rawData[1];
            Length = Helpers.BytesToUShort(rawData[2], rawData[3]);

            byte[] authenticator = new byte[16];
            Buffer.BlockCopy(rawData, 4, authenticator, 0, 16);
            RequestAuthenticator = authenticator;

            parseRadiusAttributes(rawData);
        }

        public RadiusPacket(RadiusCode code, byte identifier, List<RadiusAttribute> attributes, byte[] requestAuthenticator, string sharedSecret)
        {
            Code = code;
            Identifier = identifier;
            Attributes = attributes;
            RequestAuthenticator = requestAuthenticator;
            SharedSecret = sharedSecret;
        }

        public byte[] ToRawData()
        {
            List<byte> rawData = new List<byte>();

            rawData.Add((byte)Code);
            rawData.Add(Identifier);
            foreach (var attribute in Attributes)
            {
                rawData.AddRange(attribute.ToRawData());
            }
            rawData.InsertRange(2, Helpers.ShortToBytes((short)(rawData.Count + 2)));
            rawData.InsertRange(4, buildResponseAuthenticator());

            return rawData.ToArray();
        }

        private byte[] buildResponseAuthenticator()
        {
            List<byte> rawData = new List<byte>();

            rawData.Add((byte)Code);
            rawData.Add(Identifier);
            rawData.AddRange(RequestAuthenticator);
            foreach (var attribute in Attributes)
            {
                rawData.AddRange(attribute.ToRawData());
            }
            rawData.InsertRange(2, Helpers.ShortToBytes((short)(rawData.Count + 2)));
            rawData.AddRange(System.Text.Encoding.ASCII.GetBytes(SharedSecret));

            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(rawData.ToArray());
            return md5.Hash;
        }


        private void parseRadiusAttributes(byte[] rawData)
        {
            int i = 20;
            while (i < rawData.Length)
            {
                var currentLength = rawData[i + 1];
                byte[] rawAttributeData = new byte[currentLength];
                Buffer.BlockCopy(rawData, i, rawAttributeData, 0, currentLength);
                Attributes.Add(new RadiusAttribute(rawAttributeData));
                i = i + currentLength;
            }
        }
    }
}