using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Nereus.Models;
using Nereus.Utils;
using Gongchengshi.EF;

namespace Nereus.Controllers
{
    public class IrrelevantProjectUrlPatternController : ApiController
   {
      private readonly NereusDb _database;

      public IrrelevantProjectUrlPatternController(NereusDb database)
      {
         _database = database;
      }

      [HttpPost]
      public HttpResponseMessage Create([FromBody] string path)
      {
         int projectId = Helpers.Common.GetProjectIdFromCookie(Request.Headers);

         var pathPattern = Helpers.ProjectUrlHelpers.NormalizePathPattern(path);

         var newPath = false;
         var pattern = _database.ProjectUrlPatterns.FirstOrAdd(
            x => x.ProjectId == projectId && x.Pattern == pathPattern,
            () =>
               {
                  newPath = true;
                  return new ProjectUrlPattern(projectId, pathPattern);
               });

         pattern.Irrelevant = true;

         if (newPath)
         {
            Globals.CompiledIrrelevantUrlPatterns.Add(projectId, pathPattern,
                                                      new Regex(pathPattern, RegexOptions.Compiled));
         }

         _database.SaveChanges();

         return new HttpResponseMessage(HttpStatusCode.OK);
      }

      [HttpDelete]
      public void Delete(int id)
      {
         var urlPattern = _database.ProjectUrlPatterns.Find(id);
         if (!urlPattern.Irrelevant) return;

         var set = Globals.CompiledIrrelevantUrlPatterns[urlPattern.ProjectId];
         Regex dummy;
         set.TryRemove(urlPattern.Pattern, out dummy);

         // Only remove the row if it is neither irrelevant nor hidden
         if (urlPattern.Hidden)
         {
            urlPattern.Irrelevant = false;
         }
         else
         {
            _database.ProjectUrlPatterns.Remove(urlPattern);
         }
         _database.SaveChanges();
      }
   }

   public class HiddenProjectUrlPatternController : ApiController
   {
      private readonly NereusDb _database;
      
      public HiddenProjectUrlPatternController(NereusDb database)
      {
         _database = database;
      }

      [HttpPost]
      public HttpResponseMessage Create([FromBody] string path)
      {
         int projectId = Helpers.Common.GetProjectIdFromCookie(Request.Headers);

         var pathPattern = Helpers.ProjectUrlHelpers.NormalizePathPattern(path);

         var newPath = false;
         var pattern = _database.ProjectUrlPatterns.FirstOrAdd(
            x => x.ProjectId == projectId && x.Pattern == pathPattern,
            () =>
            {
               newPath = true;
               return new ProjectUrlPattern(projectId, pathPattern);
            });

         pattern.Hidden = true;

         if (newPath)
         {
            Globals.CompiledHiddenUrlPatterns.Add(projectId, pathPattern, new Regex(pathPattern, RegexOptions.Compiled));
         }

         _database.SaveChanges();

         return new HttpResponseMessage(HttpStatusCode.OK);
      }

      [HttpDelete]
      public void Delete(int id)
      {
         var urlPattern = _database.ProjectUrlPatterns.Find(id);
         if (!urlPattern.Hidden) return;

         var set = Globals.CompiledHiddenUrlPatterns[urlPattern.ProjectId];
         Regex dummy;
         set.TryRemove(urlPattern.Pattern, out dummy);

         // Only remove the row if it is neither irrelevant nor hidden
         if (urlPattern.Irrelevant)
         {
            urlPattern.Hidden = false;
         }
         else
         {
            _database.ProjectUrlPatterns.Remove(urlPattern);
         }
         _database.SaveChanges();
      }
   }
}
