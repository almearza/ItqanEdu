using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using QuranEducation.Models;

[assembly: OwinStartupAttribute(typeof(QuranEducation.Startup))]
namespace QuranEducation
{
    public partial class Startup
    {
        

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
      
    }
}
