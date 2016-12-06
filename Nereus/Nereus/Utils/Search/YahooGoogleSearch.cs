using System;
using Nereus.Models;
using Nereus.ViewModels;
using WebMining.SearchProvidor;

namespace Nereus.Utils.Search
{
    public class YahooGoogleSearch : ISearchProvider
   {
      public static readonly YahooBossWebSearch Yahoo = new YahooBossWebSearch();
      public static readonly GoogleCustomSearch Google = new GoogleCustomSearch();

      // In this case count is just a suggested count
      public Tuple<string, SearchResults> Search(string q, int start = 0, int count = 10, UserSearchSettings settings = null, TemporarySearchSettings tempSettings = null, SearchProfile profile = null)
      {
         return (tempSettings != null && tempSettings.SearchPeriod != null && tempSettings.SearchPeriod.IsConstrained)?
            Google.Search(q, start, 10, settings, tempSettings, profile) :
            Yahoo.Search(q, start, count, settings, tempSettings, profile);
      }
   }
}
