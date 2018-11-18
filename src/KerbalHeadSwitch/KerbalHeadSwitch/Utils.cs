using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalHeadSwitch
{
    static class Utils
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
