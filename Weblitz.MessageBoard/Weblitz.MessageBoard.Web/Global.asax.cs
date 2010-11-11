using System.Web.Mvc;
using System.Web.Routing;
using Weblitz.MessageBoard.Web.Controllers.Factories;
using Weblitz.MessageBoard.Web.Models.Binders;

namespace Weblitz.MessageBoard.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Forum", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());

            ModelBinders.Binders.DefaultBinder = new EntityModelBinder();
        }
    }
}