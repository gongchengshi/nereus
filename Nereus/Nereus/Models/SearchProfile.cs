using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nereus.Models
{
   public class SearchProfile
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public SearchPeriod SearchPeriod { get; set; }
      public string ExcludePathsJson { get; set; }
      public string IncludePathsJson { get; set; } // This acts like a custom search engine.
      public string ExcludeQueriesJson { get; set; }
      public string IncludeQueriesJson { get; set; }

      // Exclude Paths
      public IEnumerable<string> GetExcludePaths()
      {
         return string.IsNullOrWhiteSpace(ExcludePathsJson)
                   ? Enumerable.Empty<string>()
                   : JsonConvert.DeserializeObject(ExcludePathsJson) as IEnumerable<string>;
      }

      public void SetExcludePaths(IEnumerable<string> list)
      {
         ExcludePathsJson = JsonConvert.SerializeObject(list);
      }

      // Include Paths
      public IEnumerable<string> GetIncludePaths()
      {
         return string.IsNullOrWhiteSpace(IncludePathsJson)
                   ? Enumerable.Empty<string>()
                   : JsonConvert.DeserializeObject(IncludePathsJson) as IEnumerable<string>;
      }

      public void SetInludePaths(IEnumerable<string> list)
      {
         IncludePathsJson = JsonConvert.SerializeObject(list);
      }

      // Exclude Queries
      public IEnumerable<string> GetExcludeQueries()
      {
         return string.IsNullOrWhiteSpace(ExcludeQueriesJson)
                   ? Enumerable.Empty<string>()
                   : JsonConvert.DeserializeObject(ExcludeQueriesJson) as IEnumerable<string>;
      }

      public void SetExcludeQueriesJson(IEnumerable<string> list)
      {
         ExcludeQueriesJson = JsonConvert.SerializeObject(list);
      }

      // Include Queries
      public IEnumerable<string> GetIncludeQueries()
      {
         return string.IsNullOrWhiteSpace(IncludeQueriesJson)
                   ? Enumerable.Empty<string>()
                   : JsonConvert.DeserializeObject(IncludeQueriesJson) as IEnumerable<string>;
      }

      public void SetIncludeQueriesJson(IEnumerable<string> list)
      {
         IncludeQueriesJson = JsonConvert.SerializeObject(list);
      }
   }
}
