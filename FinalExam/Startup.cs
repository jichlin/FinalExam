using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalExam.Startup))]
namespace FinalExam
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
