using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ActivityManagement.Startup))]
namespace ActivityManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
