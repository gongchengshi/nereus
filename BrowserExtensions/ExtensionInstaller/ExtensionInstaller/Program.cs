using System;
using System.IO;

namespace ExtensionInstaller
{
    class Program
   {
      static void Main(string[] args)
      {
         Utils.WaitForProcessesToClose("firefox", "chrome");

         try
         {
            var GhosteryInstaller = new ChromeExtensionInstaller("mlomiejdfkolichcflejclcbmpeaniij", "Profile 2");
            GhosteryInstaller.Install(@"..\..\Input\Ghostery_Chrome\Ghostery_Chrome.zip", "4.1.0_0", false);

            var AdBlockInstaller = new ChromeExtensionInstaller("gighmmpiobklfepjocnamgkkbiglidom", "Profile 2");
            AdBlockInstaller.Install(@"..\..\Input\AdBlock_Chrome\AdBlock_Chrome.zip", "2.5.54_0", true);

            var FirefoxGhosteryInstaller = new GhosteryFirefoxExtensionInstaller("default");
            FirefoxGhosteryInstaller.Install(@"..\..\Input\Ghostery_Firefox\Ghostery_Firefox.zip", "firefox@ghostery.com", true);

            var FirefoxAdblockPlusInstaller = new FirefoxExtensionInstaller("AdblockPlusFirefoxExtension", "default");
            //FirefoxAdblockPlusInstaller.Install(@"..\..\Input\AdblockPlus_Firefox\AdblockPlus_Firefox.zip", "{d10d0bf8-f5b5-c8b4-a8b2-2b9879e08c5d}.xpi", true);
            FirefoxAdblockPlusInstaller.InstallXpi("{d10d0bf8-f5b5-c8b4-a8b2-2b9879e08c5d}.xpi", true);
         }
         catch (Exception ex)
         {
            Console.WriteLine("An error occured while installing extension for Chrome\n" +
                "Most likely, Chrome is not installed. See error:\n\n" + ex);
         }
         finally
         {

         }
      }
   }

   public class GhosteryFirefoxExtensionInstaller : FirefoxExtensionInstaller
   {
      public GhosteryFirefoxExtensionInstaller(string profile)
         : base("GhosteryFirefoxExtension", profile)
      {}

      public override void Install(string zipFilePath, string extensionDirName, bool showOnToolbar)
      {
         base.Install(zipFilePath, extensionDirName, showOnToolbar);

         Utils.CopyDirectory(Path.Combine(TempPath, "ghostery"), ProfileDir, true);
      }
   }
}
