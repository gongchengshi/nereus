using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Nereus.Models;
using Nereus.ViewModels;
using WebMining.SearchProvidor;
using WebMining.SearchProvidor.Google;

namespace Nereus.Utils.Search
{
    public class GoogleCustomSearch : CustomSearch, ISearchProvider
   {
#if PERF_COUNTERS
      private static readonly PerformanceCounter _numQueries;

      static GoogleCustomSearch()
      {
         const string numQueriesCounterName = "GoogleCseQueries";

         try
         {

            _numQueries = Globals.PerformanceCounters.CounterExists(numQueriesCounterName)
                             ? Globals.PerformanceCounters.GetCounters()
                                      .First(x => x.CounterName == numQueriesCounterName)
                             : new PerformanceCounter(Globals.CounterCategoryName, numQueriesCounterName, false);
            _numQueries.ReadOnly = false;
         }
         catch (InvalidOperationException)
         {
            Debugger.Break();
         }
      }

      private static void IncrementQueryCounter()
      {
         if (_numQueries != null)
         {
            _numQueries.Increment();
         }
      }
#endif

      public GoogleCustomSearch() : base("AIzaSyBpSJoc9SWciC9960FPjFJ3Jzos4CWOdBY", "009465036018407574794:aibhbi8fj30")
      {}

      public Tuple<string, SearchResults> Search(string q, int start = 0, int count = 10, 
         UserSearchSettings settings = null, TemporarySearchSettings tempSettings = null, SearchProfile profile = null)
      {
         if (count > 10)
         {
            count = 10;
         }

         if (start > 100)
         {
            return CreateSearchResultsInfo(new SearchResults(q, 0, start, new List<SearchResult>()));
         }

         if (start + Math.Min(10, count) > 101)
         {
            count = 101 - start;
         }

         var queryBuilder = new StringBuilder(q);

         // As far as I can determine, siteSearch will only take a single site url. In this case we have to use site:
         SearchCommon.AppendIncludeExcludePaths(queryBuilder, profile);

         var request = CreateRequest(queryBuilder.ToString());

         request.Start = start;
         request.Num = count;

         if (tempSettings != null && tempSettings.SearchPeriod != null)
         {
            if (tempSettings.SearchPeriod.StartDate != null)
            {
               int numDays = (DateTime.UtcNow - tempSettings.SearchPeriod.StartDate.Value).Days;
               request.DateRestrict = "d" + numDays;
            }
            else if (tempSettings.SearchPeriod.SearchPeriodOption != SearchPeriodOption.Anytime)
            {
               request.DateRestrict = SearchPeriodToGoogleFormat(tempSettings.SearchPeriod.SearchPeriodOption);
            }
         }
         else if (profile != null && profile.SearchPeriod != null)
         {
            if (profile.SearchPeriod.StartDate != null)
            {
               int numDays = (DateTime.UtcNow - profile.SearchPeriod.StartDate.Value).Days;
               request.DateRestrict = "d" + numDays;
            }
            else if (profile.SearchPeriod.SearchPeriodOption != SearchPeriodOption.Anytime)
            {
               request.DateRestrict = SearchPeriodToGoogleFormat(profile.SearchPeriod.SearchPeriodOption);
            }
         }

         try
         {
#if PERF_COUNTERS
            IncrementQueryCounter();
#endif
            var result = request.Execute();
            return CreateSearchResultsInfo(NormalizeSearchResults.FromGoogleCustomSearch(result));
         }
         catch (Google.GoogleApiException)
         {
            Debugger.Break();
            return CreateSearchResultsInfo(new SearchResults(q, 0, start, new List<SearchResult>()));
         }
      }

      private Tuple<string, SearchResults> CreateSearchResultsInfo(SearchResults searchResults)
      {
         return Tuple.Create("Google", searchResults);
      }

      public static string SearchPeriodToGoogleFormat(SearchPeriodOption searchPeriodOption)
      {
         switch (searchPeriodOption)
         {
            case SearchPeriodOption.Past24Hours:
               return "d1";
            case SearchPeriodOption.PastWeek:
               return "w1";
            case SearchPeriodOption.Past30Days:
               return "m1";
            case SearchPeriodOption.PastYear:
               return "y1";
            case SearchPeriodOption.Past2Years:
               return "y2";
            case SearchPeriodOption.Past5Years:
               return "y5";
            case SearchPeriodOption.Past10Years:
               return "y10";
            default:
               throw new ArgumentException();
         }
      }
   }
}