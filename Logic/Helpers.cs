using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class Helpers
    {
        public static ushort BytesToUShort(params byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0);
            else
                return BitConverter.ToUInt16(bytes, 0);
        }
    }
}
