using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MCU.Startup))]
namespace MCU
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
