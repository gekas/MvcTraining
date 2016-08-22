using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcTraining.Startup))]
namespace MvcTraining
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
