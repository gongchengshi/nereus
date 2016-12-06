using System;
using System.Text;
using Nereus.Models;
using Nereus.ViewModels;
using WebMining.SearchProvidor.Yahoo.Boss;
using WebMining.SearchProvidor;

namespace Nereus.Utils.Search
{
   public static class YahooBossKeys
   {
      public const string ConsumerKey =
         "dj0yJmk9elNMYWxxVk5SUjFOJmQ9WVdrOWFGSkdVM0ZqTlRJbWNHbzlNakl3TlRnMk5EWXkmcz1jb25zdW1lcnNlY3JldCZ4PTJi";

      public const string ConsumerSecret = "28c9bd39eea879a626ee2e6f75ffcf7419f48721";
   }

   public class YahooBossAlsoTryQueries : AlsoTryQueries
   {
      public YahooBossAlsoTryQueries() : base(YahooBossKeys.ConsumerKey, YahooBossKeys.ConsumerSecret)
      {}
   }

   public class YahooBossWebSearch : WebSearch, ISearchProvider
   {
      public YahooBossWebSearch() : base(YahooBossKeys.ConsumerKey, YahooBossKeys.ConsumerSecret)
      {}

      public Tuple<string, SearchResults> Search(string q, int start = 0, int count = 10, 
         UserSearchSettings settings = null, TemporarySearchSettings tempSettings = null, SearchProfile profile = null)
      {
         var queryBuilder = new StringBuilder(q);

         SearchCommon.AppendIncludeExcludePaths(queryBuilder, profile);

         var yahooResponse = Search(queryBuilder.ToString(), start, count, true, true);

         return Tuple.Create("Yahoo", NormalizeSearchResults.FromYahooBoss(yahooResponse, q));
      }
   }
}
