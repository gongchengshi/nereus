using System.Text;
using Nereus.Models;

namespace Nereus.Utils.Search
{
   public static class SearchCommon
   {
      public static void AppendIncludeExcludePaths(StringBuilder queryBuilder, SearchProfile profile)
      {
         if (profile != null)
         {
            var excludePaths = profile.GetExcludePaths();
            if (excludePaths != null)
            {
               foreach (var excludePath in excludePaths)
               {
                  queryBuilder.AppendFormat(" -site:{0}", excludePath);
               }
            }

            var includePaths = profile.GetIncludePaths();
            if (includePaths != null)
            {
               foreach (var includePath in includePaths)
               {
                  queryBuilder.AppendFormat(" site:{0}", includePath);
               }
            }

            var includeQueries = profile.GetIncludeQueries();
            if (includeQueries != null)
            {
               foreach (var includeQuery in includeQueries)
               {
                  queryBuilder.AppendFormat(" {0}", includeQuery);
               }
            }

            var excludeQueries = profile.GetExcludeQueries();
            if (excludeQueries != null)
            {
               foreach (var excludeQuery in excludeQueries)
               {
                  queryBuilder.AppendFormat(" -{0}", excludeQuery);
               }
            }
         }         
      }
   }
}