namespace Nereus.Controllers.Helpers
{
   static public class BrowsingHelpers
   {
      static public bool TrackUrl(string url)
      {
         return !url.Contains("localhost") && !url.StartsWith("https://www.google.com/search");
      }
   }
}
