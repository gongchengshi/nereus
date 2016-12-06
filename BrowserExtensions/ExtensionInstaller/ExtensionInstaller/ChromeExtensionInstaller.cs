using System;
using System.IO;
using Ionic.Zip;
using Newtonsoft.Json.Linq;

namespace ExtensionInstaller
{
    // Installs the given extension into the specified profile
    public class ChromeExtensionInstaller : IExtensionInstaller
   {
      // On my Windows 7 Machine this directory is located here:
      // C:\Users\jeremcla\AppData\Local\Google\Chrome\User Data\Default
      private string _profileDir;
      private string _extensionDir { get { return Path.Combine(_profileDir, "Extensions"); } }
      private string _preferencesPath { get { return Path.Combine(_profileDir, "Preferences"); } }

      public string Profile 
      {
         get { return _Profile ?? "Default"; }
         set { _Profile = value; }
      }
      private string _Profile = null;

      public ChromeExtensionInstaller(string id, string profile)
      {
         Profile = profile;
         ID = id;
         var appData = Environment.GetEnvironmentVariable("APPDATA");

         _profileDir = Path.Combine(Directory.GetParent(appData).FullName, @"Local\Google\Chrome\User Data", Profile);
      }

      public string ID { get; private set; }

      public static class Helpers
      {
         static public string UpdatePreferences(string input, string id, string settingsIn, bool show)
         {
            dynamic root = JObject.Parse(input);
            var extensions = (JObject)root.GetValue("extensions");
            var settings = (JObject)extensions.GetValue("settings");
            var manifestInSettings = (JObject)settings.GetValue(id);

            if (manifestInSettings == null)
            {
               var manifestJObject = JObject.Parse(settingsIn);
               settings.Add(id, manifestJObject);
            }

            if (show)
            {
               var toolbar = (JArray)settings.GetValue("toolbar");
               if (toolbar == null)
               {
                  toolbar = new JArray();
                  settings.Add("toolbar", toolbar);
               }
               toolbar.Add(id);
            }

            return root.ToString();
         }
      }

      public void Install(string zipFilePath, string extensionDirName, bool showOnToolbar)
      {
         var installDir = Path.Combine(_extensionDir, ID);

         if (Directory.Exists(installDir))
         {
            throw new InvalidOperationException("Extension is already installed");
         }

         var tempPath = Path.Combine(Path.GetTempPath(), ID);
         if (Directory.Exists(tempPath))
         {
            Directory.Delete(tempPath, true);
         }
         Directory.CreateDirectory(tempPath);
         var zipFile = new ZipFile(zipFilePath);
         zipFile.ExtractAll(tempPath, ExtractExistingFileAction.OverwriteSilently);
         Directory.CreateDirectory(installDir);
         var extensionExtractLocation = Path.Combine(installDir, extensionDirName);
         Utils.CopyDirectory(Path.Combine(tempPath, extensionDirName), extensionExtractLocation, true);

         var localStorageInstallDir = Path.Combine(_profileDir, "Local Storage");
         if(!Directory.Exists(localStorageInstallDir))
         {
            Directory.CreateDirectory(localStorageInstallDir);
         }

         var localStorageFileName = string.Format("chrome-extension_{0}_0.localstorage", ID);
         var localStoragePath = Path.Combine(tempPath, localStorageFileName);
         if (File.Exists(localStoragePath))
         {
            File.Copy(localStoragePath, Path.Combine(localStorageInstallDir, localStorageFileName), true);
         }

         var preferenceSettingsPath = Path.Combine(tempPath, "PreferenceSettings");
         if (File.Exists(preferenceSettingsPath))
         {
            var settings = File.ReadAllText(preferenceSettingsPath);
            var currentPreferences = File.ReadAllText(_preferencesPath);

            using (var writer = new StreamWriter(_preferencesPath))
            {
               writer.Write(Helpers.UpdatePreferences(currentPreferences, ID, settings, showOnToolbar));
            }
         }
      }
   }
}
