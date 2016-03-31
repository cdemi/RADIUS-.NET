using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Models
{
    public class RadiusAttribute
    {
        public readonly RadiusAttributeType Type;
        public readonly byte Length;
        public readonly byte[] Value;
        public string ReadableValue
        {
            get
            {
                switch (Type)
                {
                    case RadiusAttributeType.NAS_IP_ADDRESS:
                    case RadiusAttributeType.NAS_IPV6_ADDRESS:
                    case RadiusAttributeType.FRAMED_IP_ADDRESS:
                    case RadiusAttributeType.FRAMED_IP_NETMASK:
                    case RadiusAttributeType.LOGIN_IP_HOST:
                    case RadiusAttributeType.LOGIN_IPV6_HOST:
                        return new IPAddress(Value).ToString();
                    case RadiusAttributeType.NAS_PORT:
                        return Helpers.BytesToUShort(Value).ToString();
                    default:
                        return Encoding.ASCII.GetString(Value);
                }
            }
        }

        public RadiusAttribute(byte[] rawData)
        {
            Type = (RadiusAttributeType)rawData[0];
            Length = rawData[1];
            Value = new byte[Length - 2];
            Buffer.BlockCopy(rawData, 2, Value, 0, Length - 2);
        }

        public RadiusAttribute(RadiusAttributeType type, IPAddress ipAddress)
            : this(type, ipAddress.GetAddressBytes())
        {
        }

        public RadiusAttribute(RadiusAttributeType type, byte[] value)
        {
            Type = type;
            Value = value;

            switch (Type)
            {
                case RadiusAttributeType.LOGIN_IP_HOST:
                    break;
                default:
                    Array.Resize(ref Value, 4);
                    Value = Value.Reverse().ToArray();
                    break;
            }
        }

        public byte[] ToRawData()
        {
            List<byte> rawData = new List<byte>();

            rawData.Add((byte)Type);
            rawData.AddRange(Value);
            rawData.Insert(1, (byte)(rawData.Count + 1));

            return rawData.ToArray();
        }
    }
}
