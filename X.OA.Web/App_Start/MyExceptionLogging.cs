using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.OA.Web.Properties;
using static X.OA.Common.Helper.RedisHelper;

namespace X.OA.Web.App_Start
{
    public class MyExceptionLogging : HandleErrorAttribute
    {
        //public static Queue<Exception> exceptionQueue = new Queue<Exception>();

        public override void OnException(ExceptionContext filterContext)
        {
            //exceptionQueue.Enqueue(filterContext.Exception);
            redisDB.ListLeftPush(Resources.Redis_ExceptionLog, JsonConvert.SerializeObject(filterContext.Exception));
            filterContext.HttpContext.Response.Redirect("/Error");
            base.OnException(filterContext);
        }
    }
}