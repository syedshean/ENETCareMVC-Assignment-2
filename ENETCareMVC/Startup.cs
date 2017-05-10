using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ENETCareMVCApp.Startup))]
namespace ENETCareMVCApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
