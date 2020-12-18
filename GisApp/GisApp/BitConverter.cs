using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GisApp
{
    public static class BitConverter
    {

        public static double ConvertToDouble(byte[] input, ByteTypes order = ByteTypes.LittleEndian)
        {

            if (order == ByteTypes.LittleEndian)
            {
                if (System.BitConverter.IsLittleEndian)
                {
                    return System.BitConverter.ToDouble(input);
                }
                else
                {
                    var temp = input.Reverse().ToArray();
                    return System.BitConverter.ToDouble(temp);
                }
            }
            else if (order == ByteTypes.BigEndian)
            {
                if (System.BitConverter.IsLittleEndian)
                {
                    var temp = input.Reverse().ToArray();
                    return System.BitConverter.ToDouble(temp);
                }
                else
                {
                    return System.BitConverter.ToDouble(input);
                }
            }
            else
            {
                return System.BitConverter.ToDouble(input);
            }
        }
        public static int ConvertToInt32(byte[] input, ByteTypes order = ByteTypes.LittleEndian)
        {
            if (order == ByteTypes.LittleEndian)
            {
                if (System.BitConverter.IsLittleEndian)
                {
                    return System.BitConverter.ToInt32(input);
                }
                else
                {
                    var temp = input.Reverse().ToArray();
                    return System.BitConverter.ToInt32(temp);
                }
            }
            else if (order == ByteTypes.BigEndian)
            {
                if (System.BitConverter.IsLittleEndian)
                {
                    var temp = input.Reverse().ToArray();
                    return System.BitConverter.ToInt32(temp);
                }
                else
                {
                    return System.BitConverter.ToInt32(input);
                }
            }
            else
            {
                return System.BitConverter.ToInt32(input);
            }

        }
    }
}
