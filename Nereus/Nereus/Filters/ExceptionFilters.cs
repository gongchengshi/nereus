using Gongchengshi.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IExceptionFilter = System.Web.Mvc.IExceptionFilter;

namespace Nereus.Filters
{
    public class InvalidCookieValueExceptionFilter : IExceptionFilter
   {
      public void OnException(ExceptionContext filterContext)
      {
         if (!(filterContext.Exception is InvalidCookieValue)) return;

         var rc = new RequestContext(filterContext.HttpContext, filterContext.RouteData);
         var url = RouteTable.Routes.GetVirtualPath(rc, new RouteValueDictionary(
                                                           new {Controller = "Project", action = "Index"}))
                             .VirtualPath;
         filterContext.HttpContext.Response.Redirect(url, true);
         filterContext.ExceptionHandled = true;
      }

      public bool AllowMultiple { get; private set; }
   }
}