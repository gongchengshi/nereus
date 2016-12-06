using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Nereus.Filters;
using Nereus.Models;
using System.Net;

namespace Nereus
{
   public class MvcApplication : HttpApplication
   {
      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();

         WebApiConfig.Register(GlobalConfiguration.Configuration);
         FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         RouteConfig.RegisterRoutes(RouteTable.Routes);
         BundleConfig.RegisterBundles(BundleTable.Bundles);

         Database.SetInitializer(new DropCreateDatabaseAlways<NereusDb>());
         Database.SetInitializer(new DropCreateDatabaseIfModelChanges<NereusDb>());
      }
   }
}
