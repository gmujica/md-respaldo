using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MesaDinero.Admin.Startup))]
namespace MesaDinero.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
