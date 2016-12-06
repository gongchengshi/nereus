using System;
using Nereus.Models;
using Nereus.ViewModels;
using WebMining.SearchProvidor;
using Gongchengshi;

namespace Nereus.Utils.Search
{
    public class AtraxSearch : ISearchProvider
   {
      private readonly Atrax.AtraxSearch _search;
      private readonly string _jobName;

      public AtraxSearch(string jobName)
      {
         _jobName = jobName;
         _search = new Atrax.AtraxSearch(jobName);
      }

      public Tuple<string, SearchResults> Search(string q, int start = 0, int count = 10, 
         UserSearchSettings settings = null, TemporarySearchSettings tempSettings = null, SearchProfile profile = null)
      {
         SearchResults searchResults;
         if (tempSettings == null)
         {
            searchResults = _search.Search(q, start, count);
         }
         else
         {
            searchResults = _search.Search(q, start, count,
               googlebotBlockedOnly: tempSettings.ShowGooglebotBlockedOnly, showDuplicates:tempSettings.ShowDuplicates);
         }

         foreach (var searchResult in searchResults)
         {
            searchResult.Url = Globals.MimeoAddress + "/" + _jobName + "/" + UrlUtils.RemoveSchema(searchResult.Url);
         }

         return Tuple.Create(_jobName, searchResults);
      }
   }
}
