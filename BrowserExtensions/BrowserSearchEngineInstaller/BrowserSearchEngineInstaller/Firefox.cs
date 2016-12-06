using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BrowserSearchEngineInstaller
{
    public class Firefox : ISearchEngineInstall
    {
        // On my Windows 7 Machine this directory is located here:
        // C:\Users\jeremcla\AppData\Roaming\Mozilla\Firefox\Profiles\tsngh0i6.default
        private string _profileDir;

        public Firefox()
        {
            var appData = Environment.GetEnvironmentVariable("APPDATA");

            var profiles = Path.Combine(appData, @"Mozilla\Firefox\Profiles");
            _profileDir = Directory.GetDirectories(profiles).Where(x => x.EndsWith(".default")).First();
        }

        public void Install()
        {
            var searchPluginsDir = Path.Combine(_profileDir, "searchplugins");
            Directory.CreateDirectory(searchPluginsDir);

            const string searchDef = "sel-search.xml";

            var selSearch = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.Contains(searchDef)).First();

            using (var selSearchStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(selSearch)))
            {
                using (var outFile = new StreamWriter(Path.Combine(searchPluginsDir, searchDef)))
                {
                    outFile.Write(selSearchStream.ReadToEnd());
                }
            }
        }

        public void SetAsDefault()
        {
            // In
            // C:\Users\jeremcla\AppData\Roaming\Mozilla\Firefox\Profiles\e2kbm80f.default\prefs.js
            // replace 
            // user_pref("browser.search.selectedEngine", "Bing");
            // with
            // user_pref("browser.search.selectedEngine", "SEL Search");

            // "Bing" is just a place holder.  If Google is selected, there will be an empty string
            // Firefox must not be running when this runs.  If it is these changes will be overwritten

            // prefs.js can be viewed and edited in Firefox by entering about:config in the address bar

            throw new NotImplementedException();
        }
    }
}
