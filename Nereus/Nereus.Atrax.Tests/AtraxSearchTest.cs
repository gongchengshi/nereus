using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nereus.Utils.Search;

namespace Nereus.Atrax.Tests
{
   [TestClass]
   public class AtraxSearchTest
   {
      [TestMethod]
      public void TestMethod1()
      {
         var target = new AtraxSearch("");
         target.Search("progra");
      }
   }
}
