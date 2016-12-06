using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nereus.Models;
using Nereus.Utils;

namespace Nereus.Filters
{
   public class CreateUserAttribute : IActionFilter
   {
      private readonly NereusDb _nereusDb;

      public CreateUserAttribute(NereusDb nereusDb)
      {
         _nereusDb = nereusDb;
      }

      public void OnActionExecuting(ActionExecutingContext filterContext)
      {
         var nereusDb = _nereusDb;
         var userIdentity = filterContext.RequestContext.HttpContext.User.Identity;

         if (!userIdentity.IsAuthenticated) return;

         if (!nereusDb.Users.Any(x => x.UserName == userIdentity.Name))
         {
            var newUser = new User {UserName = userIdentity.Name};

            SearchProvider defaultSearchProvider;
            try
            {
               defaultSearchProvider =
                  nereusDb.SearchProviders.First(x => x.Name == Globals.DefaultSearchProviderName);
            }
            catch (ObjectDisposedException)
            {
               // For some reason the filter is used sometimes outside of the context of a request
               // In those cases _nereusDb is already disposed. 
               nereusDb = new NereusDb();
               defaultSearchProvider =
                  nereusDb.SearchProviders.First(x => x.Name == Globals.DefaultSearchProviderName);
            }

            var project = new Project
               {
                  IsPrivate = true,
                  Name = userIdentity.Name,
                  Users = new List<User> {newUser},
                  SearchProvider = defaultSearchProvider
               };

            nereusDb.Projects.Add(project);

            var userSettings = new UserUISettings
            {
                User = newUser
            };
           
            nereusDb.UserUISettings.Add(userSettings);
            nereusDb.SaveChanges();

            var cookie = new HttpCookie("Project", project.Id.ToString());
            HttpContext.Current.Response.Cookies.Add(cookie);
         }
      }

      public void OnActionExecuted(ActionExecutedContext filterContext)
      {
      }
   }
}
