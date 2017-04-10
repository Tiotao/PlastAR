using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Plastar.Startup))]
namespace Plastar
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
