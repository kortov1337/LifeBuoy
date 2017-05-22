using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lifebuoy.Startup))]
namespace Lifebuoy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
