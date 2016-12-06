using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEL.Nereus.Controllers.Helpers;

namespace SEL.Nereus.Tests
{
   [TestClass]
   public class ProjectUrlHelpersTests
   {
      [TestMethod]
      public void TestNormalizePathPattern()
      {
         var result = ProjectUrlHelpers.NormalizePathPattern("http://www.example.com");
         Assert.AreEqual(@"http://www\.example\.com.*", result);

         result = ProjectUrlHelpers.NormalizePathPattern("http://www.example.com/");
         Assert.AreEqual(@"http://www\.example\.com.*", result);
      }
   }
}
