using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nereus.Models;

namespace Nereus.Controllers
{
   public class PopupViewModel
   {
      public IEnumerable<Project> Projects { get; set; }
      public Project UserProject { get; set; }
   }

   public class PopupController : Controller
   {
      private readonly NereusDb _database;

      public PopupController(NereusDb database)
      {
         _database = database;
      }

      //private BrowsingAccess _browsing_i;
      //private BrowsingAccess _browsing
      //{
      //   get { return _browsing_i = _browsing_i ?? new BrowsingAccess(new NereusDb(), this.User.Identity.Name); }
      //}

      // GET: /Popup/
      public ActionResult Index(string url)
      {
         var user = _database.GetUser(User.Identity.Name);

         var viewModel = new PopupViewModel
            {
               Projects = _database.GetUserProjects(user),
               UserProject = _database.Projects.FirstOrDefault(x => x.Name == user.UserName)
            };

         //if (string.IsNullOrWhiteSpace(url))
         //   return View("Blank");

         //try
         //{
         //   ViewBag.PreviouslyVisited = _browsing.GetUserPreviouslyVisited(url);
         //   ViewBag.GroupLastVisited = _browsing.GetOtherLastVisited(url);

         //}
         //catch (InvalidOperationException)
         //{
         //   ViewBag.PreviouslyVisited = null;
         //   ViewBag.GroupLastVisited = new List<Visit>();
         //}

         return View("Index", viewModel);
      }

   }
}
