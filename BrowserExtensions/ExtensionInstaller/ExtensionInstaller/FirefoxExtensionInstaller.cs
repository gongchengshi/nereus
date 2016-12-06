using System;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace ExtensionInstaller
{
    public class FirefoxExtensionInstaller : IExtensionInstaller
   {
      // On my Windows 7 Machine this directory is located here:
      // C:\Users\jeremcla\AppData\Roaming\Mozilla\Firefox\Profiles\tsngh0i6.default
      public string ProfileDir { get; private set; }

      public string Profile { get; private set; }
      public string TempPath { get; private set; }

      public FirefoxExtensionInstaller(string extensionName, string profile)
      {
         var appData = Environment.GetEnvironmentVariable("APPDATA");

         var profiles = Path.Combine(appData, @"Mozilla\Firefox\Profiles");
         ProfileDir = Directory.GetDirectories(profiles).Where(x => x.EndsWith(".default")).First();

         TempPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(extensionName));
         if (Directory.Exists(TempPath))
         {
            Directory.Delete(TempPath, true);
         }
      }

      public virtual void InstallXpi(string xpiFilePath, bool showOnToolbar)
      {
         var extensionExtractLocation = Path.Combine(ProfileDir, "extensions");

         var destination = Path.Combine(extensionExtractLocation, Path.GetFileName(xpiFilePath));

         if(File.Exists(destination))
         {
            throw new InvalidOperationException("Extension is already installed");
         }

         File.Copy(xpiFilePath, extensionExtractLocation);
      }

      public virtual void Install(string zipFilePath, string extensionDirName, bool showOnToolbar)
      {
         var zipFile = new ZipFile(zipFilePath);
         zipFile.ExtractAll(TempPath, ExtractExistingFileAction.OverwriteSilently);
         var extensionExtractLocation = Path.Combine(ProfileDir, "extensions");
         Utils.CopyDirectory(Path.Combine(TempPath, extensionDirName), extensionExtractLocation, true);

         // Todo: Configure to show/hide on toolbar
      }
   }
}
