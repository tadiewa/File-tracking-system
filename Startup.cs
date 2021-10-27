using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(File_tracking_system.Startup))]
namespace File_tracking_system
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
