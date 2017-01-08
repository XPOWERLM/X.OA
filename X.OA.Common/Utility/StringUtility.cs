using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.Common.Utility
{
    public static class StringUtility
    {
        /// <summary>
        /// Null condition
        /// </summary>
        /// <param name="reverse">reverse condition, False : If parameters contains null,return true. True: If contains not null , return true.</param>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(bool reverse = false, params string[] strs)
        {
            if (strs == null || strs.Length <= 0)
                throw new ArgumentException("Nothing here, check your code");

            return reverse ?
            strs.Any(s => s != null && s?.Length > 0) :
            strs.Any(s => s == null || s?.Length <= 0);
        }
    }
}
