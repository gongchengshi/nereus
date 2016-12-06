using System.Web.Mvc;

namespace Nereus.Filters
{
   public class FilterConfig
   {
      public static void RegisterGlobalFilters(GlobalFilterCollection filters)
      {
         filters.Add(new AuthorizeAttribute());
         // This is now set up in NinjectWebCommon
         //filters.Add(new CreateUserAttribute());
         filters.Add(new InvalidCookieValueExceptionFilter());
         filters.Add(new HandleErrorAttribute());
      }
   }
}