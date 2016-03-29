using Logic;
using System;
using System.Collections.Generic;

namespace Models
{
    public class RadiusPacket
    {
        public readonly RadiusCode Code;
        public readonly byte Identifier;
        public readonly ushort Length;
        public readonly byte[] Authenticator;
        public readonly List<RadiusAttribute> Attributes = new List<RadiusAttribute>();

        public RadiusPacket(byte[] rawData)
        {
            this.Code = (RadiusCode)rawData[0];
            Identifier = rawData[1];
            Length = Helpers.BytesToUShort(rawData[2], rawData[3]);

            byte[] authenticator = new byte[16];
            Buffer.BlockCopy(rawData, 4, authenticator, 0, 16);
            Authenticator = authenticator;

            parseRadiusAttributes(rawData);
        }

        public RadiusPacket(RadiusCode code, byte identifier, List<RadiusAttribute> attributes)
        {
            Code = code;
            Identifier = identifier;
            Attributes = attributes;
        }

        public byte[] ToRawData()
        {
            List<byte> rawData = new List<byte>();

            rawData.Add((byte)Code);
            rawData.Add(Identifier);

            return rawData.ToArray();
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