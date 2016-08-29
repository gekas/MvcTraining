using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcTraining.Models;
using SimpleInjector;
using System.Web;

namespace MvcTraining.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static void RegisterDependencies(Container container)
        {
            container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()));
            container.RegisterPerWebRequest(() => HttpContext.Current.GetOwinContext().Authentication);
            container.RegisterSingleton<ILog>(LogManager.GetLogger("RollingLogFileAppender"));
        }
    }
}