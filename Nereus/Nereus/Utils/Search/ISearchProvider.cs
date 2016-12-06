using System;
using Nereus.Models;
using Nereus.ViewModels;
using WebMining.SearchProvidor;

namespace Nereus.Utils.Search
{
   public interface ISearchProvider
   {
      Tuple<string, SearchResults> Search(string q, int start = 0, int count = 10, 
         UserSearchSettings settings = null, TemporarySearchSettings tempSettings = null, SearchProfile profile = null);
   }
}
