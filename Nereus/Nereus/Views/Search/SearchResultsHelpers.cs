using System;
using System.Collections.Generic;

namespace Nereus.Views.Search
{
   static public class SearchResultsHelpers
   {
      // This is not 100% yet. It may need a second look if I can figure out how 
      // do paging to previous with hidden/irrelevant results in the set.
      public static Dictionary<string, int> CalculatePageNavigation(
         int startIndex,
         int endIndex,
         int maxResults,
         int numResultsPerPage)
      {
         var prevStart = (startIndex >= numResultsPerPage) ? startIndex - numResultsPerPage : -1;

         // Limit the available results to 1000
         maxResults = maxResults > 1000 ? 1000 : maxResults;

         var nextStart = endIndex + 1;
         nextStart = nextStart > maxResults ? -1 : nextStart;

         var numPages = (int) Math.Ceiling((double) maxResults/numResultsPerPage);

         var currentPage = startIndex/numResultsPerPage;

         var pagingStart = currentPage - 10;
         pagingStart = pagingStart < 0 ? 0 : pagingStart;

         var pagingEnd = pagingStart + 10;
         pagingEnd = pagingEnd > numPages ? numPages : pagingEnd;

         return new Dictionary<string, int>
            {
               {"prevStart", prevStart},
               {"nextStart", nextStart},
               {"pagingStart", pagingStart},
               {"pagingEnd", pagingEnd},
               {"currentPage", currentPage}
            };
      }
   }
}
