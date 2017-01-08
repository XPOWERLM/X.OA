using log4net;
using log4net.Config;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using X.OA.Common.Helper;
using X.OA.IBLL;
using X.OA.Web.App_Start;
using X.OA.Web.Properties;
using static X.OA.Common.Helper.RedisHelper;
using static X.OA.Common.Helper.UnityHelper;

namespace X.OA.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        #region Create required objects
        private static ILog log = null;
        private static IKeywordRankBLL searchBLL = container.Resolve<IKeywordRankBLL>();
        #endregion
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BasicConfigurator.Configure();
            QuartzHelper.StartPolling(() =>
            {
                searchBLL.Statistic();
            });
            log = LogManager.GetLogger("WebLogger");
            ExceptionWatcher(Server);
        }

        /// <summary>
        /// Exception logging polling 
        /// </summary>
        /// <param name="server"></param>
        static void ExceptionWatcher(HttpServerUtility server)
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (redisDB.ListLength(Resources.Redis_ExceptionLog)>0)
                    //if (MyExceptionLogging.exceptionQueue.Count > 0)
                    {
                        //var ex = MyExceptionLogging.exceptionQueue.Dequeue();
                        Exception ex = JsonConvert.DeserializeObject<Exception>(redisDB.ListLeftPop(Resources.Redis_ExceptionLog));
                        if (ex != null)
                        {
                            //string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                            //File.AppendAllText(server.MapPath("log/") + fileName, ex.Message);
                            log.Error(ex);
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
            })
            { IsBackground = true }.Start();
        }


    }
}
