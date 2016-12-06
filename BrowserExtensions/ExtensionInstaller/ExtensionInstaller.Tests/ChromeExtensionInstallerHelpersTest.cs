using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtensionInstaller.Tests
{
   [TestClass]
   public class ChromeExtensionInstallerHelpersTest
   {
      [TestMethod]
      public void UpdatePreferences()
      {
         var results = ChromeExtensionInstaller.Helpers.UpdatePreferences(
            File.ReadAllText(@"..\..\Input\StartingPreferences"),
            "mlomiejdfkolichcflejclcbmpeaniij",
            File.ReadAllText(@"..\..\Input\PreferenceSettings"),
            true);

         var outputDir = @"..\..\Output";

         Directory.CreateDirectory(outputDir);

         var outputFile = Path.Combine(outputDir, "ResultingPreferences.json");

         File.Delete(outputFile);

         File.WriteAllText(outputFile, results);
      }
   }
}
