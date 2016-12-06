using System.Web.Mvc;
using System.Web.Routing;

namespace Nereus
{
   public class RouteConfig
   {
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute(
            "IrrelevantPaths",
            "Project/Paths/Irrelevant",
            new {controller = "Project", action = "IrrelevantPaths"});

         routes.MapRoute(
            "HiddenPaths",
            "Project/Paths/Hidden",
            new { controller = "Project", action = "HiddenPaths" });
      
         routes.MapRoute(
             name: "Search",
             url: "Search/{action}",
             defaults: new { controller = "Search", action = "Index" }
         );

         routes.MapRoute(
             name: "DefaultController",
             url: "SearchProfile/{id}",
             defaults: new { controller = "SearchProfile", action = "Index", id = UrlParameter.Optional }
         );

         routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Search", action = "Index", id = UrlParameter.Optional }
         );
      }
   }
}