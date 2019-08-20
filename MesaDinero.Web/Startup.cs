using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MesaDinero.Web.Startup))]
namespace MesaDinero.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
