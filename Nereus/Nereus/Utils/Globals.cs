using System.Text.RegularExpressions;
using Nereus.Models;
using Gongchengshi.Collections.Generic.Concurrent;

namespace Nereus.Utils
{
    public static class Globals
   {
      public static string DefaultSearchProviderName { get; private set; }
#if DEBUG
      //public static string MimeoAddress = "http://localhost:63584";
      public static string MimeoAddress = "https://mimeo.com";
#else
      public static string MimeoAddress = "https://mimeo.com";
#endif

      #region UrlPatterns
      public static ConcurrentDictionaryOfDictionaries<int, string, Regex> CompiledIrrelevantUrlPatterns { get; private set; }
      public static ConcurrentDictionaryOfDictionaries<int, string, Regex> CompiledHiddenUrlPatterns { get; private set; }
      #endregion

#if PERF_COUNTERS
      #region PerformanceCounters
      public const string CounterCategoryName = "Nereus";
      public static readonly PerformanceCounterCategory PerformanceCounters;
      #endregion
#endif

      static Globals()
      {
         var database = new NereusDb();

         DefaultSearchProviderName = "Yahoo/Google";

         #region UrlPatterns
         CompiledIrrelevantUrlPatterns = new ConcurrentDictionaryOfDictionaries<int, string, Regex>();
         CompiledHiddenUrlPatterns = new ConcurrentDictionaryOfDictionaries<int, string, Regex>();

         foreach (var pattern in database.ProjectUrlPatterns)
         {
            var compiledPattern = new Regex(pattern.Pattern, RegexOptions.Compiled);

            if (pattern.Irrelevant)
            {
               CompiledIrrelevantUrlPatterns.Add(pattern.ProjectId, pattern.Pattern, compiledPattern);
            }

            if (pattern.Hidden)
            {
               CompiledHiddenUrlPatterns.Add(pattern.ProjectId, pattern.Pattern, compiledPattern);
            }
         }
         #endregion
#if PERF_COUNTERS
         #region PerformanceCounters
         PerformanceCounters = PerformanceCounterCategory.Exists(CounterCategoryName) ? 
            PerformanceCounterCategory.GetCategories().First(x => x.CategoryName == CounterCategoryName) : 
            new PerformanceCounterCategory(CounterCategoryName);
         #endregion
#endif
      }
   }
}