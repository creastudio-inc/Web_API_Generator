using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_API_Generator.Startup))]
namespace Web_API_Generator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
