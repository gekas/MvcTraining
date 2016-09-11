using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MvcTraining.Models;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace MvcTraining.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static void RegisterDependencies(Container container)
        {
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterSingleton<ILog>(LogManager.GetLogger("RollingFileAppender"));
            container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()));
            container.RegisterPerWebRequest(() =>
                                                 AdvancedExtensions.IsVerifying(container)
                                                 ? new OwinContext(new Dictionary<string, object>()).Authentication
                                                 : HttpContext.Current.GetOwinContext().Authentication);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly()); 
        }
    }
}