using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MvcTraining.Models;
using SimpleInjector;
using SimpleInjector.Advanced;
using System.Collections.Generic;
using System.Web;

namespace MvcTraining.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static void RegisterDependencies(Container container)
        {
            container.RegisterPerWebRequest(() =>
                                                 AdvancedExtensions.IsVerifying(container)
                                                 ? new OwinContext(new Dictionary<string, object>()).Authentication
                                                 : HttpContext.Current.GetOwinContext().Authentication);
            container.RegisterPerWebRequest<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>());
            container.RegisterSingleton<ILog>(LogManager.GetLogger("RollingLogFileAppender"));
        }
    }
}