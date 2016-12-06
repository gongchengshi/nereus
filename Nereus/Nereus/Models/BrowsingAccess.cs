using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Nereus.Models
{
   public class Visit
   {
      public string UserName { get; set; }
      public DateTime LastVisit { get; set; }
   }

   public class BrowsingAccess
   {
      private readonly NereusDb _database;
      private readonly User _user;

      public BrowsingAccess(NereusDb database, string userName)
      {
         _database = database;
         try
         {
            _user = _database.GetUser(userName);
         }
         catch (InvalidOperationException)
         {
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
         }
      }

      private int _userID { get { return _user.Id; } }

      public Url GetOrInsertResourceInfo(string url)
      {
         Url resource;

         try
         {
            resource = _database.Urls.First(x => x.Path == url);
         }
         catch (InvalidOperationException)
         {
            resource = new Url { Path = url };
            _database.Urls.Add(resource);
         }

         return resource;
      }

      public void UpdateUserLastViewed(Url resource, DateTime dateTime)
      {
         try
         {
            UserUrl userUrl = _database.UserUrls.First(x => (x.UrlId == resource.Id) && (x.UserId == _userID));
            userUrl.LastViewed = dateTime;
         }
         catch (InvalidOperationException)
         {
            _database.UserUrls.Add(new UserUrl { LastViewed = dateTime, User = _user, Url = resource });
         }
      }

      public void SaveChanges()
      {
         _database.SaveChanges();
      }

      public UserUrl GetUserUrl(string url)
      {
         return _database.UserUrls.First(x => (x.Url.Path == url) && (x.UserId == _userID));
      }

      public DateTime? GetUserPreviouslyVisited(string url)
      {
         try
         {
            var userUrl = GetUserUrl(url);

            return userUrl.PrevLastViewed;
         }
         catch (InvalidOperationException)
         {
            return null;
         }
      }

      public DateTime? GetUserLastVisited(string url)
      {
         try
         {
            var userUrl = GetUserUrl(url);

            return userUrl.LastViewed;
         }
         catch (InvalidOperationException)
         {
            return null;
         }
      }

      public IEnumerable<Visit> GetOtherLastVisited(string url)
      {
         var allVisits = from x in _database.UserUrls
                         where x.Url.Path == url && (x.UserId != _userID) && x.LastViewed != null
                         orderby x.LastViewed
                         select new Visit { UserName = x.User.UserName, LastVisit = x.LastViewed.Value };

         return allVisits;
      }
   }
}