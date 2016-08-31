using Microsoft.Owin;

[assembly: OwinStartup(typeof(ForestBookstore.Startup))]
namespace ForestBookstore
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
