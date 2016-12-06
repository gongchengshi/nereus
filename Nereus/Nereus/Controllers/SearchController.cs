using System;
using System.Web.Mvc;
using Nereus.Models;
using System.Collections.Generic;
using System.Linq;
using Nereus.ViewModels;
using Gongchengshi.Collections.Generic;
using WebMining.SearchProvidor;
using Gongchengshi.EF;

namespace Nereus.Controllers
{
    public class SearchController : Controller
   {
      private readonly NereusDb _database;

      public SearchController(NereusDb database)
      {
         _database = database;
      }

      public ActionResult Index(string q, int start = 1,
         bool showHidden = false, bool showIrrelevant = false,
         int? totalResults = null, string searchPeriod = null, 
         bool? showDuplicates = null, bool? showGooglebotBlockedOnly = null, string language = null)
      {
         var now = DateTime.UtcNow;
         var user = _database.GetUser(User.Identity.Name);

         var viewModel = new SearchResultsViewModel
            {
               SearchProfiles = from x in _database.UserSearchProfiles
                                where x.UserId == user.Id
                                select x.SearchProfile,
               Projects = _database.GetUserProjects(user),
               SearchSettings = _database.UserSearchSettings.FirstOrNew(x => x.UserId == user.Id),
               ShowIrrelevant = showIrrelevant,
               ShowHidden = showHidden,
               SearchPeriod = (searchPeriod == null) ? null : new SearchPeriod(searchPeriod)
            };

         var tempSearchSettings = new TemporarySearchSettings
         {
            ShowGooglebotBlockedOnly = showGooglebotBlockedOnly?? false,
            Language = language?? "en",
            SearchPeriod = viewModel.SearchPeriod,
            ShowDuplicates = showDuplicates?? false
         };

         var currentProject = Helpers.Common.GetProjectFromCookie(Request.Cookies, _database.Projects);
         viewModel.UserProject = _database.Projects.FirstOrDefault(x => x.Name == user.UserName);
         if (currentProject == null)
         {
            // This should never happen because the user's project gets created the first time they log in.
            if (viewModel.UserProject == null)
            {
               return Redirect("~/Project");
            }
            currentProject = viewModel.UserProject;
         }

         viewModel.RecentQueries = _database.ProjectQueries.
            Where(x => x.ProjectId == currentProject.Id).
            OrderByDescending(x => x.LastUsed).
            Take(10);

         if (string.IsNullOrWhiteSpace(q))
            return View("QueryForm", viewModel);

         var currentSearchProfile = Helpers.Common.GetSearchProfileFromCookie(Request.Cookies, _database.SearchProfiles);

         viewModel.DateConstrained = (currentSearchProfile != null && currentSearchProfile.SearchPeriod != null && currentSearchProfile.SearchPeriod.IsConstrained) ||
                                     (viewModel.SearchPeriod != null && viewModel.SearchPeriod.IsConstrained);

         var currentQuery = _database.ProjectQueries.FirstOrAdd(
            x => x.Text == q && x.ProjectId == currentProject.Id,
           () => new ProjectQuery(currentProject, q, now));

         viewModel.CurrentQueryId = currentQuery.Id;

         viewModel.SearchResults = new SearchResults(q, start, viewModel.SearchSettings.NumResultsPerPage);

         Search(viewModel, user, currentProject, currentSearchProfile, tempSearchSettings);

         if (viewModel.SearchResults.TotalResults == 0 && totalResults != null)
         {
            viewModel.SearchResults.TotalResults = totalResults.Value;
         }

         _database.SaveChanges();

         return View("SearchResults", viewModel);
      }

      private void Search(SearchResultsViewModel viewModel, User user, Project currentProject,
         SearchProfile currentSearchProfile, TemporarySearchSettings tempSearchSettings)
      {
         int queryStart = viewModel.SearchResults.QueryStart;
         int end = queryStart;

         // Always ensure that there are SearchSettings.NumResultsPerPage and retrieve/discard additional results as needed
         // Give the view the correct information to make another request with the correct start index
         while (viewModel.SearchResults.Count < viewModel.SearchSettings.NumResultsPerPage)
         {
            tempSearchSettings.SearchPeriod = (viewModel.SearchPeriod != null && viewModel.SearchPeriod.IsConstrained)
                                  ? viewModel.SearchPeriod
                                  : null;

            _database.Entry(currentProject).Reference(x => x.SearchProvider).Load();
            var searchProvider = currentProject.SearchProvider.GetInstance();

            var searchResultsInfo = searchProvider.Search(
               viewModel.SearchResults.Query, queryStart,
               // Using NumResultsPerPage * 2 here to balance between performance and collisions do to rankings changing between requests.
               viewModel.SearchSettings.NumResultsPerPage * 2,
               viewModel.SearchSettings, tempSearchSettings, currentSearchProfile);

            viewModel.SearchProvidor = searchResultsInfo.Item1;
            var searchResults = searchResultsInfo.Item2;

            viewModel.SearchResults.TotalResults = searchResults.TotalResults;
            viewModel.SearchResults.NumDuplicateResults += searchResults.NumDuplicateResults;

            foreach (var item in searchResults)
            {
               ++end;
               queryStart = end + 1;

               var urlDetails = HandleResult(item, currentProject, viewModel, user);
               if (urlDetails == null)
               {
                  continue;
               }

               viewModel.SearchResults.Add(item);
               viewModel.UrlInfo[item.Url] = urlDetails;

               if (viewModel.SearchResults.Count == viewModel.SearchSettings.NumResultsPerPage)
               {
                  break;
               }
            }

            if (end >= searchResults.TotalResults)
            {
               break;
            }
         }

         viewModel.SearchResults.QueryEnd = end - 1;
      }

      private UrlDetails HandleResult(SearchResult item, Project currentProject, SearchResultsViewModel viewModel, User user)
      {
         var urlModel = _database.Urls.FirstOrAdd(x => x.Path == item.Url, () => new Url { Path = item.Url });

         var projectUrl = _database.ProjectUrls.FirstOrAdd(
            x => (x.UrlId == urlModel.Id) && (x.ProjectId == currentProject.Id),
            () => new ProjectUrl(currentProject, urlModel));

         var urlDetails = new UrlDetails
            {
               ProjectUrl = projectUrl,
               UserUrl = _database.UserUrls.FirstOrDefault(x => (x.UrlId == projectUrl.UrlId) && (x.UserId == user.Id)),
               HasIrrelevantPattern = projectUrl.HasIrrelevantPattern,
               HasHiddenPattern = projectUrl.HasHiddenPattern
            };


         // These are both computed properties so it is a slight performance optimization to just do this once.

         if (projectUrl.Irrelevant || (urlDetails.HasIrrelevantPattern && projectUrl.Rating <= 0))
         {
            ++viewModel.NumIrrelevant;
            if (!viewModel.ShowIrrelevant)
            {
               return null;
            }
         }

         if (projectUrl.Hidden || urlDetails.HasHiddenPattern)
         {
            ++viewModel.NumHidden;
            if (!viewModel.ShowHidden)
            {
               return null;
            }
         }

         urlDetails.SeenBefore = true;

         _database.UserProjectUrls.IfNotContainsAdd(
            x => (x.ProjectUrlId == projectUrl.Id) && (x.UserId == user.Id),
            () =>
            {
               urlDetails.SeenBefore = false;
               return new UserProjectUrl(projectUrl, user);
            });

         _database.SaveChanges(); // Required in order to get the Id for urlModel

         //item.HtmlSnippet = item.HtmlSnippet.Replace("<br>", string.Empty);

         return urlDetails;
      }

      public IEnumerable<UserUrl> GetLastViewed(User user, IEnumerable<Url> resources)
      {
         var userUrls = from x in _database.UserUrls
                        where x.UserId == user.Id && resources.Contains(x.Url)
                        select x;

         return userUrls;
      }
   }
}
