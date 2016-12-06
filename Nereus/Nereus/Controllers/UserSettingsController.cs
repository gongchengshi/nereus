using System.Linq;
using System.Net;
using System.Web.Mvc;
using Nereus.Models;

namespace Nereus.Controllers
{
   public class UserSettingsController : Controller
   {
      // GET: /Settings/
      private readonly NereusDb _database;

      public UserSettingsController(NereusDb database)
      {
         _database = database;
      }

      public ActionResult Index()
      {
         var user = _database.GetUser(User.Identity.Name);
         var setting = _database.UserUISettings.First(x => x.UserId == user.Id);
         return View(setting);
      }

      public ActionResult UserTooltipSetting()
      {
         var user = _database.GetUser(User.Identity.Name);
         var userSetting = _database.UserUISettings.First(x => x.UserId == user.Id);
         var tooltip = userSetting.TooltipsOn.ToString();
         return Content(tooltip);
      }

      [HttpPut]
      public HttpStatusCodeResult UpdateUISettings(UserUISettings value)
      {
         var user = _database.GetUser(User.Identity.Name);
         var setting = _database.UserUISettings.Find(user.Id);

         setting.TooltipsOn = value.TooltipsOn;
         _database.SaveChanges();

         return new HttpStatusCodeResult(HttpStatusCode.OK);
      }

      [HttpPut]
      public HttpStatusCodeResult UpdateSearchSettings(UserSearchSettings value)
      {
         //    var user = _database.GetUser(User.Identity.Name);
         //    var setting = _database.UserSearchSettings.Find(user.Id);

         //    setting.NumResultsPerPage = value.NumResultsPerPage;
         //    _database.SaveChanges();

         return new HttpStatusCodeResult(HttpStatusCode.OK);
      }
   }
}
