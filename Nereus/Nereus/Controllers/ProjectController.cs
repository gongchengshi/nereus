using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Nereus.Models;
using System.Data.Entity;
using Nereus.Utils;
using Nereus.ViewModels;

namespace Nereus.Controllers
{
   public class ProjectController : Controller
   {
      private readonly NereusDb _database;

      public ProjectController(NereusDb database)
      {
         _database = database;
      }

      // GET: /Project/
      public ViewResult Index()
      {
         var user = _database.GetUser(User.Identity.Name);
         return View("Index", _database.GetUserProjects(user).ToList());
      }

      public ViewResult All()
      {
         return View("Index", _database.Projects.ToList());
      }

      // GET: /Project/Details/5
      public ViewResult Details(int id)
      {
         Project project = _database.Projects.Find(id);
         _database.Entry(project).Collection(p => p.Users).Load();
         return View(project);
      }

      // GET: /Project/Create
      public ActionResult Create()
      {
         var project = new Project();
         return View(new CreateOrEditProjectViewModel(project, _database.SearchProviders));
      }

      // POST: /Project/Create
      [HttpPost]
      public ActionResult Create(Project project)
      {
         if (_database.Projects.Any(x => x.Name == project.Name))
         {
            // A project with that name already exists.
            // Todo: return appropriate error here
            return RedirectToAction("Index");
         }

         if (ModelState.IsValid)
         {
            if (project.IsPrivate)
            {
               project.Users.Add(_database.GetUser(User.Identity.Name));
            }
            else
            {
               project.Users.AddRange(_database.Users);
            }

            _database.Projects.Add(project);
            _database.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(new CreateOrEditProjectViewModel(project, _database.SearchProviders));
      }

      [HttpGet]
      public int UserProjectId()
      {
         return _database.Projects.First(x => x.Name == User.Identity.Name).Id;
      }

      [HttpPost]
      public int CreateProject(string name, bool isPrivate = false)
      {
         if (_database.Projects.Any(x => x.Name == name))
         {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return -1;
         }

         var project = new Project
         {
            Name = name,
            Users = new List<User> { _database.GetUser(User.Identity.Name) },
            IsPrivate = isPrivate,
            SearchProvider = _database.SearchProviders.First(x => x.Name == Globals.DefaultSearchProviderName)
         };
         
         _database.Projects.Add(project);
         _database.SaveChanges();
         return project.Id;
      }

      // GET: /Project/Edit/5
      public ActionResult Edit(int id)
      {
         Project project = _database.Projects.Single(x => x.Id == id);
         return View(new CreateOrEditProjectViewModel(project, _database.SearchProviders));
      }

      // POST: /Project/Edit/5
      [HttpPost]
      public ActionResult Edit(Project project)
      {
         if (ModelState.IsValid)
         {
            _database.Entry(project).State = EntityState.Modified;
            _database.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(new CreateOrEditProjectViewModel(project, _database.SearchProviders));
      }

      private int GetProjectId()
      {
         return Helpers.Common.GetProjectIdFromCookie(Request.Cookies);
      }

      public ActionResult IrrelevantPaths()
      {
         var projectId = GetProjectId();
         var project = _database.Projects.Find(projectId);

         var viewModel = new PathsViewModel(PathType.Irrelevant)
            {
               ProjectName = project.Name,
               Patterns = _database.ProjectUrlPatterns.Where(x => x.ProjectId == projectId && x.Irrelevant).OrderBy(x => x.Pattern)
            };

         return View("Paths", viewModel);
      }

      public ActionResult HiddenPaths()
      {
         var projectId = GetProjectId();
         var project = _database.Projects.Find(projectId);

         var viewModel = new PathsViewModel(PathType.Hidden)
            {
               ProjectName = project.Name,
               Patterns = _database.ProjectUrlPatterns.Where(x => x.ProjectId == projectId && x.Hidden).OrderBy(x => x.Pattern)
            };

         return View("Paths", viewModel);
      }

      public ViewResult Queries()
      {
         var projectId = GetProjectId();
         var project = _database.Projects.Find(projectId);
         var viewModel = new QueriesViewModel
         {
            ProjectName = project.Name,
            Queries = _database.ProjectQueries.Where(x => x.ProjectId == projectId)
         };
         return View("Queries", viewModel);
      }

      public ActionResult Documents(bool hdn = false, int r = 3)
      {
         var projectId = GetProjectId();
         var project = _database.Projects.Find(projectId);

         if (project == null)
         {
            return RedirectToAction("Index");
         }

         IEnumerable<ProjectUrl> urls;

         var queryableUrls = _database.ProjectUrls.Include(x => x.Url).Where(x => x.ProjectId == projectId);

         if (hdn)
         {
            urls = queryableUrls.ToArray().Where(x => (x.Hidden || x.HasHiddenPattern) && x.Rating == r);

            if (r == -1)
            {
               urls = urls.Where(x => (x.Rating == -1 || x.HasIrrelevantPattern));
            }
         }
         else if(r == -1)
         {
            urls = queryableUrls.ToArray().Where(x => (x.Rating == -1 || x.HasIrrelevantPattern) && !x.HasHiddenPattern);
         }
         else
         {
            urls = queryableUrls.Where(x => (!x.Hidden) && (x.Rating == r)).ToArray().Where(x => !x.HasIrrelevantPattern && !x.HasHiddenPattern);
         }

         var viewModel = new UrlsViewModel
            {
               ProjectName = project.Name,
               Urls = urls.OrderBy(x => x.Url.Path),
               Hidden = hdn,
               Rating = r
            };

         return View("Documents", viewModel);
      }

      //public ViewResult DocumentsOrFilter(bool? hdn = null, string r = null)
      //{
      //   var projectId = GetProjectId();
      //   var project = _database.Projects.Find(projectId);

      //   var urls = _database.ProjectUrls.Include(x => x.Url).Where(x => x.ProjectId == projectId);

      //   var ratings = Enumerable.Empty<int>();

      //   if (!string.IsNullOrWhiteSpace(r))
      //   {
      //      ratings = r.Split(',').Select(x => Convert.ToInt32(x));
      //      urls = urls.Where(x => ratings.Contains(x.Rating));
      //   }

      //   if (hdn != null)
      //   {
      //      urls = urls.Where(x => x.Hidden == hdn);
      //   }

      //   var viewModel = new UrlsOrFilterViewModel
      //   {
      //      ProjectName = project.Name,
      //      Urls = urls,
      //      Hdn = hdn,
      //      Ratings = ratings
      //   };

      //   return View("Documents", viewModel);
      //}

      [HttpDelete]
      public void Delete(int id)
      {
         _database.Projects.Remove(_database.Projects.Find(id));
         _database.SaveChanges();
      }

      [HttpPut]
      public HttpStatusCodeResult UpdatePrivate(Project value)
      {
         var project = _database.Projects.Find(value.Id);
         _database.Entry(project).Collection(x => x.Users).Load();

         if (value.IsPrivate && project.Users.Count > 1)
         {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden,
                                             "You may only mark a project private if you are the only user in the project.");
         }

         project.IsPrivate = value.IsPrivate;
         _database.SaveChanges();

         return new HttpStatusCodeResult(HttpStatusCode.OK);
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            _database.Dispose();
         }
         base.Dispose(disposing);
      }

      public PartialViewResult Selector()
      {   
          var user = _database.GetUser(User.Identity.Name);
          var model = new SearchResultsViewModel();
          model.Projects = user.Projects;
          model.UserProject.Id = user.Id;          

          return PartialView("ProjectSelector", model);
      }
   }
}