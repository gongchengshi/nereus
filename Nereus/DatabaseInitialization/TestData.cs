using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nereus.Models;

namespace DatabaseInitialization
{
   static public class TestData
   {
      static public void Initialize(NereusDb database)
      {
         var yahooGoogle = database.SearchProviders.First(x => x.Name == "Yahoo/Google");

         // Create two users
         var john = new User {FirstName = "John", LastName = "McClane", UserName = "SEL\\johnmccl"};
         database.Users.Add(john);
         var defaultProjectForJohn = new Project { Name = john.UserName, Users = new List<User> { john }, IsPrivate = true, SearchProvider = yahooGoogle };
         database.Projects.Add(defaultProjectForJohn);

         var jeremy = new User { FirstName = "Jeremy", LastName = "McLain", UserName = "SEL\\jeremcla" };
         database.Users.Add(jeremy);
         var defaultProjectForJeremy = new Project { Name = jeremy.UserName, Users = new List<User> { jeremy }, IsPrivate = true, SearchProvider = yahooGoogle };
         database.Projects.Add(defaultProjectForJeremy);

         // Create two projects
         var alligators = new Project { Name = "Alligators", Users = new List<User> { john, jeremy }, SearchProvider = yahooGoogle };
         database.Projects.Add(alligators);

         var crocodiles = new Project { Name = "Crocodiles", IsPrivate = true, Users = new List<User> { john, jeremy }, SearchProvider = yahooGoogle };
         database.Projects.Add(crocodiles);

         var lastYear = new SearchProfile {Name = "Last Year", SearchPeriod = new SearchPeriod { SearchPeriodOption = SearchPeriodOption.PastYear }};
         database.SearchProfiles.Add(lastYear);

         var excludedKeywords = new[]
            {
               "Croc",
               "Salt Water"
            };

         var excludedPaths = new[]
            {
               "http://www.wikipedia.org",
               "http://www.ask.com"
            };

         var noCrocs = new SearchProfile
            {
               Name = "No Crocs",
               ExcludeQueriesJson = JsonConvert.SerializeObject(excludedKeywords),
               ExcludePathsJson = JsonConvert.SerializeObject(excludedPaths),
               SearchPeriod = new SearchPeriod()
            };

         //database.SearchProfiles.Add(noCrocs);

         database.UserSearchProfiles.Add(new UserSearchProfile { User = john, SearchProfile = noCrocs });
         database.UserSearchProfiles.Add(new UserSearchProfile { User = john, SearchProfile = lastYear });

         database.UserSearchSettings.Add(new UserSearchSettings { User = john, NumResultsPerPage = 10 });

         var irrelevant1 = new Url {Path = "http://www.imdb.com/title/tt0080354/"};
         var irrelevant2 = new Url {Path = "http://www.propellerheads.se/products/reason/index.cfm?fuseaction=get_article&article=devices_alligator"};
         var rate0 = new Url { Path = "http://www.example0.com" };
         var rate1 = new Url {Path = "http://www.example1.com"};
         var rate2 = new Url {Path = "http://www.example2.com"};
         var rate3 = new Url {Path = "http://www.example3.com"};
         var hide1 = new Url { Path = "http://en.wikipedia.org/wiki/Alligator" };
         var like1 = new Url {Path = "http://www.boston.com/news/local/breaking_news/2010/09/alligator_captu.html"};

         database.Urls.Add(irrelevant1);
         database.Urls.Add(irrelevant2);
         database.Urls.Add(hide1);
         database.Urls.Add(like1);

         database.UserUrls.Add(new UserUrl { User = john, Url = irrelevant1 });
         database.UserUrls.Add(new UserUrl { User = john, Url = irrelevant2 });
         database.UserUrls.Add(new UserUrl { User = john, Url = hide1 });
         database.UserUrls.Add(new UserUrl { User = john, Url = like1, LastViewed = DateTime.UtcNow });

         var irrelevantProjectUrl1 = new ProjectUrl(alligators, irrelevant1) { Rating = -1 };
         var irrelevantProjectUrl2 = new ProjectUrl(alligators, irrelevant2) { Rating = -1 };
         var rate0ProjectUrl = new ProjectUrl(alligators, rate0) { Rating = 0 };
         var rate1ProjectUrl = new ProjectUrl(alligators, rate1) { Rating = 1 };
         var rate2ProjectUrl = new ProjectUrl(alligators, rate2) { Rating = 2 };
         var rate3ProjectUrl = new ProjectUrl(alligators, rate3) { Rating = 3 };
         var hideProjectUrl1 = new ProjectUrl(alligators, hide1) { Hidden = true };
         var likeProjectUrl1 = new ProjectUrl(alligators, like1) { Rating = 3 };

         database.ProjectUrls.Add(irrelevantProjectUrl1);
         database.ProjectUrls.Add(irrelevantProjectUrl2);
         database.ProjectUrls.Add(rate0ProjectUrl);
         database.ProjectUrls.Add(rate1ProjectUrl);
         database.ProjectUrls.Add(rate2ProjectUrl);
         database.ProjectUrls.Add(rate3ProjectUrl);
         database.ProjectUrls.Add(hideProjectUrl1);
         database.ProjectUrls.Add(likeProjectUrl1);

         database.UserProjectUrls.Add(new UserProjectUrl(irrelevantProjectUrl1, john));
         database.UserProjectUrls.Add(new UserProjectUrl(irrelevantProjectUrl2, john));
         database.UserProjectUrls.Add(new UserProjectUrl(rate0ProjectUrl, john));
         database.UserProjectUrls.Add(new UserProjectUrl(rate1ProjectUrl, john));
         database.UserProjectUrls.Add(new UserProjectUrl(rate2ProjectUrl, john));
         database.UserProjectUrls.Add(new UserProjectUrl(rate3ProjectUrl, john));
         database.UserProjectUrls.Add(new UserProjectUrl(hideProjectUrl1, john));
         database.UserProjectUrls.Add(new UserProjectUrl(likeProjectUrl1, john));

         database.ProjectUrlPatterns.Add(
            new ProjectUrlPattern { Project = alligators, Pattern = @".*http://www\.alligator\.com.*", Irrelevant = true});

         database.ProjectUrlPatterns.Add(
            new ProjectUrlPattern { Project = alligators, Pattern = @".*http://www\.en.wikipedia\.org.*", Hidden = true });

         var alligator1 = new ProjectQuery(alligators, "alligator", new DateTime(2013, 1, 1));
         var alligator2 = new ProjectQuery(alligators,"alligator teeth", new DateTime(2013, 1, 10));
         database.ProjectQueries.Add(alligator1);
         database.ProjectQueries.Add(alligator2);

         database.SaveChanges();
      }
   }
}
