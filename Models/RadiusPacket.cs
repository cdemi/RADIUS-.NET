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
                return (ushort)((rawData[2] << 8) + rawData[3]);
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
        public RadiusPacket(byte[] rawData)
        {
            this.rawData = rawData;
        }
    }
}