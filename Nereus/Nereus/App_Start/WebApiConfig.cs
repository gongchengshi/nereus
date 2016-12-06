using System.Web.Http;
using System.Web.Mvc;

namespace Nereus
{
   public static class WebApiConfig
   {
      public static void Register(HttpConfiguration config)
      {
         config.Routes.MapHttpRoute(
            name: "Mark Url Pattern Irrelevant",
            routeTemplate: "api/Project/UrlPattern/Irrelevant/{action}/{id}",
            defaults: new { controller = "IrrelevantProjectUrlPattern", action = UrlParameter.Optional, id = UrlParameter.Optional }
            );

         config.Routes.MapHttpRoute(
            name: "Hide Url Pattern",
            routeTemplate: "api/Project/UrlPattern/Hidden/{action}/{id}",
            defaults: new { controller = "HiddenProjectUrlPattern", action = UrlParameter.Optional, id = UrlParameter.Optional}
            );

         config.Routes.MapHttpRoute(
            name: "ActionOnly",
            routeTemplate: "api/{controller}/{action}"
            );

         config.Routes.MapHttpRoute(
             name: "DefaultRestApi",
             routeTemplate: "api/{controller}/{id}",
             defaults: new { id = RouteParameter.Optional }
         );

         config.Routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { id = UrlParameter.Optional }
         );
      }
   }
}
