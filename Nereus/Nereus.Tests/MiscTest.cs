using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEL.Nereus.Views.Search;

namespace SEL.Nereus.Tests
{
   [TestClass]
   public class MiscTest
   {
      [TestMethod]
      public void CalculatePageNavigationTest()
      {
         Dictionary<string, int> pageNavValues;

         // current page: 0
         pageNavValues = SearchResultsHelpers.CalculatePageNavigation(0, 9, 1001, 10);
         CheckCalculatePageNavigationAssertions(-1, 10, 0, 10, 0, false, true, pageNavValues);

         pageNavValues = SearchResultsHelpers.CalculatePageNavigation(0, 9, 101, 10);
         CheckCalculatePageNavigationAssertions(-1, 10, 0, 10, 0, false, true, pageNavValues);

         pageNavValues = SearchResultsHelpers.CalculatePageNavigation(0, 9, 91, 10);
         CheckCalculatePageNavigationAssertions(-1, 10, 0, 10, 0, false, true, pageNavValues);

         pageNavValues = SearchResultsHelpers.CalculatePageNavigation(0, 9, 89, 10);
         CheckCalculatePageNavigationAssertions(-1, 10, 0, 9, 0, false, true, pageNavValues);

         // current page: 1
         pageNavValues = SearchResultsHelpers.CalculatePageNavigation(10, 19, 89, 10);
         CheckCalculatePageNavigationAssertions(0, 20, 0, 9, 1, true, true, pageNavValues);

         pageNavValues = SearchResultsHelpers.CalculatePageNavigation(60, 69, 89, 10);
         CheckCalculatePageNavigationAssertions(50, 70, 0, 9, 6, true, true, pageNavValues);
      }

      private void CheckCalculatePageNavigationAssertions(
         int prevStart, int nextStart, 
         int pagingStart, int pagingEnd, int currentPage,
         bool showPrev, bool showNext,
         IReadOnlyDictionary<string, int> pageNavValues)
      {
         Assert.AreEqual(prevStart, pageNavValues["prevStart"]);
         Assert.AreEqual(nextStart, pageNavValues["nextStart"]);
         Assert.AreEqual(pagingStart, pageNavValues["pagingStart"]);
         Assert.AreEqual(pagingEnd, pageNavValues["pagingEnd"]);
         Assert.AreEqual(currentPage, pageNavValues["currentPage"]);
         Assert.AreEqual(showPrev, pageNavValues["prevStart"] >= 0);
         Assert.AreEqual(showNext, pageNavValues["nextStart"] >= 0);
      }
   }
}
