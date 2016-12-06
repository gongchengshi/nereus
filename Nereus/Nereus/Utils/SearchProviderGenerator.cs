using SEL.Nereus.Models;

namespace SEL.Nereus.Utils
{
   public static class SearchProviderGenerator
   {
      public static SearchProvider Google()
      {
         return new SearchProvider
         {
            ClassName = typeof(Search.GoogleCustomSearch).AssemblyQualifiedName,
            ConstructorParam = null,
            Description = "Google will only return up to 100 results.",
            DisplayName = "Google",
            Name = "Google",
            SupportsDateConstraints = true
         };
      }

      public static SearchProvider Yahoo()
      {
         return new SearchProvider
         {
            ClassName = typeof(Search.YahooBossWebSearch).AssemblyQualifiedName,
            ConstructorParam = null,
            Description = "Yahoo! uses the same index as Bing for organic search results.",
            DisplayName = "Yahoo!",
            Name = "Yahoo",
            SupportsDateConstraints = false
         };
      }

      public static SearchProvider YahooGoogle()
      {
         return new SearchProvider
         {
            ClassName = typeof(Search.YahooGoogleSearch).AssemblyQualifiedName,
            ConstructorParam = null,
            Description =
               "Google is used when the search needs to be date constrained otherwise Yahoo! is used. Only the first 100 search results are returned when Google is being used.",
            DisplayName = "Yahoo! / Google",
            Name = "Yahoo/Google",
            SupportsDateConstraints = true
         };
      }

      public static SearchProvider Atrax(string jobName, string description)
      {
         return new SearchProvider
         {
            ClassName = typeof(Search.AtraxSearch).AssemblyQualifiedName,
            ConstructorParam = jobName,
            Description = description,
            DisplayName = jobName,
            Name = jobName,
            SupportsDateConstraints = false
         };
      }

      public static void Main(string[] args)
      {
         var searchProvider = Atrax("siemens20150201", "All crawled Siemens sites");
      }
   }
}
