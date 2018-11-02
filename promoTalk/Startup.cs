using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(promoTalk.Startup))]
namespace promoTalk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
