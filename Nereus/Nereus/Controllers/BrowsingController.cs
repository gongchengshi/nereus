using System;
using System.Web.Http;
using Nereus.Controllers.Helpers;
using Nereus.Models;
using System.Net.Http;
using System.Net;

namespace Nereus.Controllers
{
   public class BrowsingController : ApiController
   {
      private BrowsingAccess _browsing_i;
      private BrowsingAccess _browsing
      {
         get { return _browsing_i = _browsing_i ?? new BrowsingAccess(new NereusDb(), User.Identity.Name); }
      }

      [HttpGet]
      public HttpResponseMessage Visit(string url)
      {
         if (!BrowsingHelpers.TrackUrl(url))
         {
            return new HttpResponseMessage(HttpStatusCode.OK);
         }

         Url resource = _browsing.GetOrInsertResourceInfo(url);
         _browsing.UpdateUserLastViewed(resource, DateTime.UtcNow);

         _browsing.SaveChanges();

         return new HttpResponseMessage(HttpStatusCode.OK);
      }

      [HttpGet]
      public HttpResponseMessage LastVisited(string url)
      {
         try
         {
            var lastVisited = _browsing.GetUserLastVisited(url);

            return Request.CreateResponse(HttpStatusCode.OK, lastVisited);
         }
         catch (InvalidOperationException)
         {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
         }
      }

      [HttpGet]
      public HttpResponseMessage AllLastVisited(string url)
      {
         try
         {
            var allVisits = _browsing.GetOtherLastVisited(url);

            return Request.CreateResponse(HttpStatusCode.OK, allVisits);
         }
         catch (InvalidOperationException)
         {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
         }
      }
   }
}