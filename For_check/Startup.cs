using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(For_check.Startup))]
namespace For_check
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
