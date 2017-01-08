using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace X.OA.Common.Helper
{
    public static class JsonHelper
    {
        static JsonNewton serialize;
        static JsonHelper()
        {
            serialize = new JsonNewton();
        }

        private class JsonNewton : Controller
        {
            public ActionResult JsonNT(object obj)
            {
                return Content(JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),"application/json");
            }
        }

        /// <summary>
        /// Serialize object using Json.Net
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ActionResult JsonNT(object obj)
        {
            return serialize.JsonNT(obj);
        }

    }
}
