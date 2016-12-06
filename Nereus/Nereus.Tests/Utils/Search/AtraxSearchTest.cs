using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEL.Nereus.Utils.Search;

namespace SEL.Nereus.Tests.Utils.Search
{
   [TestClass]
   public class AtraxSearchTest
   {
      [TestMethod]
      public void TestMethod1()
      {
         var target = new AtraxSearch("siemens20150201");
         target.Search("program");
      }
   }
}
