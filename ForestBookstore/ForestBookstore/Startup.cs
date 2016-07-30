using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ForestBookstore.Startup))]
namespace ForestBookstore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
