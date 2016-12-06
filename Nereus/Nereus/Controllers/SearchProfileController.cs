using System.Web.Mvc;
using Nereus.Models;

namespace Nereus.Controllers
{
   public class SearchProfileController : Controller
   {
      private readonly NereusDb _database;

      public SearchProfileController(NereusDb database)
      {
         _database = database;
      }

      public ActionResult Index(int id)
      {
         //var searchProfile = _database.SearchProfiles.Find(id);
         var searchProfile = new SearchProfile();
         return View("SearchProfile", searchProfile);
      }
   }
}
