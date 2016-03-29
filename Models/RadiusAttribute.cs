using Logic;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Models
{
    public class RadiusAttribute
    {
        private byte[] rawData;

        public RadiusAttributeType Type
        {
            get
            {
                return (RadiusAttributeType)rawData[0];
            }
        }

        public byte Length
        {
            get
            {
                return rawData[1];
            }
        }

        public string Value
        {
            get
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

        public RadiusAttribute(byte[] rawData)
        {
            this.rawData = rawData;
        }
    }
}
