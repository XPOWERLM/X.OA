using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.Common.Helper
{
    public static class UnityHelper
    {
        public static IUnityContainer container = CreateConfiguredUnityContainer();

        static UnityHelper()
        {
            UnityServiceLocator locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        private static IUnityContainer CreateConfiguredUnityContainer()
        {
            IUnityContainer container = new UnityContainer();

            //// (optional) provide default mappings programmatically
            //container.RegisterType<ICustomService, CustomServiceImpl2>();

            // (optional) load static config from the *.xml file
            container.LoadConfiguration();

            return container;
        }
    }
}
