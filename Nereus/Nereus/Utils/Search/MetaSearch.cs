namespace Nereus.Utils.Search
{
    public static class MetaSearch
   {
      public static readonly YahooBossWebSearch Yahoo = new YahooBossWebSearch();
      public static readonly GoogleCustomSearch Google = new GoogleCustomSearch();


      //public static SearchResults Search(string q, int start = 0, UserSearchSettings settings = null, int count = 10, SearchProfile profile = null, SearchPeriod searchPeriod = null)
      //{
      //   var yahooResponse = Yahoo.Search(q, start, settings, count, profile, searchPeriod);
      //   var googleResponse = Google.Search(q, start);

      //   return yahooResponse;
      //}
   }
}