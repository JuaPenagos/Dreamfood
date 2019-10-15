using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DreamFood.Backend.Startup))]
namespace DreamFood.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
