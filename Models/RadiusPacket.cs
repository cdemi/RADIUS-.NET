using Logic;
using System;
using System.Collections.Generic;

namespace Models
{
    public class RadiusPacket
    {
        private byte[] rawData;


        public RadiusCode Code
        {
            get
            {
                return (RadiusCode)rawData[0];
            }
        }

        public byte Identifier
        {
            get
            {
                return rawData[1];
            }
        }

        public ushort Length
        {
            get
            {
                return Helpers.BytesToUShort(rawData[2], rawData[3]);
            }
        }

        public byte[] Authenticator
        {
            get
            {
                byte[] authenticator = new byte[16];
                Buffer.BlockCopy(rawData, 4, authenticator, 0, 16);
                return authenticator;
            }
        }
        public List<RadiusAttribute> Attributes = new List<RadiusAttribute>();
        public RadiusPacket(byte[] rawData)
        {
            this.rawData = rawData;
            parseRadiusAttributes();
        }

        private void parseRadiusAttributes()
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