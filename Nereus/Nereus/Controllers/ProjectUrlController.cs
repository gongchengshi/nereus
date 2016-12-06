using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nereus.Exceptions;
using Nereus.Models;
using System.Data.Entity;
using Nereus.Utils;

namespace Nereus.Controllers
{
   public class ProjectUrlController : ApiController
   {
      private readonly NereusDb _database;

      public ProjectUrlController(NereusDb database)
      {
         _database = database;
      }

      private ProjectUrl GetProjectUrlFromUrlId(long urlId)
      {
         try
         {
            var projectId = Helpers.Common.GetProjectIdFromCookie(Request.Headers);
            var projectUrl = _database.ProjectUrls.First(x => (x.UrlId == urlId) && (x.ProjectId == projectId));
            return projectUrl;
         }
         catch (InvalidOperationException)
         {
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
               {
                  ReasonPhrase = "The URL is not part of this project."
               });
         }
         catch (NereusException ex)
         {
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
               {
                  ReasonPhrase = ex.Message
               });
         }
      }

      [HttpPut]
      public HttpResponseMessage Hide(int id)
      {
         _database.ProjectUrls.Find(id).Hidden = true;
         _database.SaveChanges();

         return new HttpResponseMessage(HttpStatusCode.OK);
      }

      [HttpPut]
      public HttpResponseMessage Show(int id)
      {
         _database.ProjectUrls.Find(id).Hidden = false;
         _database.SaveChanges();

         return new HttpResponseMessage(HttpStatusCode.OK);
      }

      [HttpPut]
      public HttpResponseMessage Rate(int id, [FromBody] int rating)
      {
         _database.ProjectUrls.Find(id).Rating = rating;
         _database.SaveChanges();

         return new HttpResponseMessage(HttpStatusCode.OK);
      }

      [HttpGet]
      public string Details(long id)
      {
         // All properties in ProjectUrl
         var projectUrl = _database.ProjectUrls.Include(x => x.Url).Include(x => x.Project).First(x => x.Id == id);
         var user = _database.Users.First(x => x.UserName == User.Identity.Name);

         // Get all Irrelevant Url Patterns that match it
         var irrelevantUrlPatterns =
            Globals.CompiledIrrelevantUrlPatterns[projectUrl.ProjectId].Where(x => x.Value.IsMatch(projectUrl.Url.Path));

         // Get all Hidden Url Patterns that match it
         var hiddenUrlPatterns =
            Globals.CompiledHiddenUrlPatterns[projectUrl.ProjectId].Where(x => x.Value.IsMatch(projectUrl.Url.Path));

         // User viewed datetimes
         var userUrl = _database.UserUrls.First(x => (x.UrlId == projectUrl.UrlId) && (x.UserId == user.Id));

         // Group viewed datetimes
         _database.Entry(projectUrl).Reference(x => x.Project).Load(); // I think this is necessary

         var groupUrls = projectUrl.Project.Users.Select(
            groupUser => _database.UserUrls.First(x => x.UrlId == projectUrl.UrlId && x.UserId == groupUser.Id)).ToList();

         // Todo: Status in web archives

         // Todo: Is watched?

         // Todo: Last changed

         // Todo: Versioned datetimes

         // Todo: Blocked by Web Washer
         return null;
      }
   }
}
