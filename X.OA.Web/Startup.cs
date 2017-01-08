using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(X.OA.Web.Startup))]
namespace X.OA.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
