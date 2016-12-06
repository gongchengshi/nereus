using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEL.Nereus.Utils.Search;

namespace SEL.Nereus.Tests.Utils.Search
{
   [TestClass]
   public class GoogleCustomSearchUnitTest
   {
      [TestMethod]
      public void GetTypes()
      {
         var t = typeof (GoogleCustomSearch);
         var className = t.AssemblyQualifiedName;

         t = typeof(YahooBossWebSearch);
         className = t.AssemblyQualifiedName;

         t = typeof(SolrSearch);
         className = t.AssemblyQualifiedName;

         t = typeof (YahooGoogleSearch);
         className = t.AssemblyQualifiedName;
      }

      [TestMethod]
      public void Instantiate()
      {

      }
   }
}
