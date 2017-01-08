using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class CharExt
    {

        /// <summary>
        /// Convert char to Int32
        /// </summary>
        /// <param name="value">The char will be converted</param>
        /// <param name="defalut">If convert failed, return default value</param>
        /// <returns>Char converted to Int32</returns>
        public static int ToInt32(this char str, int defalut = -1)
        {
            int value = 0;
            return int.TryParse(str.ToString(), out value) ? value : defalut;
        }
    }
}