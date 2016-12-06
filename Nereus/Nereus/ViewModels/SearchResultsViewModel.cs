using System.Collections.Generic;
using Nereus.Models;
using WebMining.SearchProvidor;

namespace Nereus.ViewModels
{
    public class TemporarySearchSettings
   {
      public bool ShowGooglebotBlockedOnly { get; set; }
      public string Language { get; set; }
      public SearchPeriod SearchPeriod { get; set; }
      public bool ShowDuplicates { get; set; }
   }

   public class SearchViewModel
   {
      public IEnumerable<SearchProfile> SearchProfiles { get; set; }
      public IEnumerable<Project> Projects { get; set; }
      public UserSearchSettings SearchSettings { get; set; } 
      public SearchPeriod SearchPeriod { get; set; }
      public IEnumerable<ProjectQuery> RecentQueries { get; set; }
      public long CurrentQueryId { get; set; }
      public Project UserProject { get; set; }
   }

   public class SearchResultsViewModel : SearchViewModel
   {
      public SearchResults SearchResults 
      { 
         get { return _searchResults; }
         set
         {
            _searchResults = value;
            UrlInfo = new Dictionary<string, UrlDetails>(_searchResults.Count);
         }
      }
      private SearchResults _searchResults ;
      public Dictionary<string, UrlDetails> UrlInfo { get; set; }
      public bool ShowIrrelevant { get; set; }
      public bool ShowHidden { get; set; }
      public int NumIrrelevant { get; set; }
      public int NumHidden { get; set; }
      public bool ContainsIrrelevant { get { return NumIrrelevant > 0; } }
      public bool ContainsHidden { get { return NumHidden > 0; } }
      public bool DateConstrained { get; set; }
      public string SearchProvidor { get; set; }
   }

   public class UrlDetails
   {
      public bool SeenBefore { get; set; }
      public ProjectUrl ProjectUrl { get; set; }
      public UserUrl UserUrl { get; set; }
      public bool HasIrrelevantPattern { get; set; }
      public bool HasHiddenPattern { get; set; }
   }
}
