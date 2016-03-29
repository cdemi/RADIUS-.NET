using Logic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Models
{
    public class RadiusAttribute
    {
        public readonly RadiusAttributeType Type;
        public readonly byte Length;
        public readonly string Value;

        public RadiusAttribute(byte[] rawData)
        {
            Type = (RadiusAttributeType)rawData[0];
            Length = rawData[1];
            Value = getValueFromRawData(rawData);
        }

        public byte[] ToRawData()
        {
            List<byte> rawData = new List<byte>();

            rawData.Add((byte)Type);

            switch (Type)
            {
                default:
                    rawData.AddRange(Encoding.ASCII.GetBytes(Value));
                    break;
            }

            //rawData.Insert(1, ((short)rawData.Count + 1));

            return rawData.ToArray();
        }
        private string getValueFromRawData(byte[] rawData)
        {
            byte[] rawValue = new byte[Length - 2];
            Buffer.BlockCopy(rawData, 2, rawValue, 0, Length - 2);
            switch (Type)
            {
                case RadiusAttributeType.NAS_IP_ADDRESS:
                case RadiusAttributeType.NAS_IPV6_ADDRESS:
                case RadiusAttributeType.FRAMED_IP_ADDRESS:
                case RadiusAttributeType.FRAMED_IP_NETMASK:
                case RadiusAttributeType.LOGIN_IP_HOST:
                case RadiusAttributeType.LOGIN_IPV6_HOST:
                    return new IPAddress(rawValue).ToString();
                case RadiusAttributeType.NAS_PORT:
                    return Helpers.BytesToUShort(rawValue).ToString();
                default:
                    return Encoding.ASCII.GetString(rawData, 2, rawData.Length - 2);
            }
        }
    }
}
