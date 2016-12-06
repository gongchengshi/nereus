using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Nereus.Models;
using Gongchengshi.Web;
using System.Web;

namespace Nereus.Controllers.Helpers
{
    public static class Common
   {
      public static Project GetProjectFromCookie(HttpCookieCollection cookies, DbSet<Project> projects)
      {
         int id;

         var cookie = cookies["Project"];

         var project = (cookie != null) && int.TryParse(cookie.Value, out id)
                          ? projects.Find(id)
                          : null;
         return project;
      }

      public static int GetProjectIdFromCookie(HttpCookieCollection cookies)
      {
         try
         {
            return int.Parse(cookies["Project"].Value);
         }
         catch (Exception ex)
         {
            if (ex is NullReferenceException || ex is FormatException)
            {
               throw new InvalidCookieValue("Project", ex);
            }
            throw;
         }
      }

      public static Project GetProjectFromCookie(HttpRequestHeaders headers, DbSet<Project> projects)
      {
         try
         {
            return projects.Find(GetProjectIdFromCookie(headers));
         }
         catch (Exception)
         {
            return null;
         }
      }

      public static int GetProjectIdFromCookie(HttpRequestHeaders headers)
      {
         try
         {
            return int.Parse(headers.GetCookies("Project").Single()["Project"].Value);
         }
         catch (Exception ex)
         {
            if (ex is InvalidOperationException || ex is NullReferenceException)
            {
               throw new InvalidCookieValue("Project", ex);
            }
            throw;
         }
      }

      public static SearchProfile GetSearchProfileFromCookie(HttpCookieCollection cookies,
                                                             DbSet<SearchProfile> searchProfiles)
      {
         int id;
         var cookie = cookies["SearchProfile"];
         var searchProfile = (cookie != null) && int.TryParse(cookie.Value, out id)
                          ? searchProfiles.Find(id)
                          : null;
         return searchProfile;
      }
   }
}