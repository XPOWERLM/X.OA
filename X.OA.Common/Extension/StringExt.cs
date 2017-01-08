using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExt
    {

        /// <summary>
        /// Convert string to Int32
        /// </summary>
        /// <param name="value">The string will be converted</param>
        /// <param name="defalut">If convert failed, return default value</param>
        /// <returns>String converted to Int32</returns>
        public static int ToInt32(this string str, int defalut = 0)
        {
            int value = 0;
            return int.TryParse(str, out value) ? value : defalut;
        }
    }
}
