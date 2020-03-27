using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    public static class HexTo
    {
        public static string HexToFloat(string hexString)
        {
            uint num = uint.Parse(hexString, System.Globalization.NumberStyles.AllowHexSpecifier);
            var number = System.Convert.ToString(num, 10);
            return number;
        }
    }
}
