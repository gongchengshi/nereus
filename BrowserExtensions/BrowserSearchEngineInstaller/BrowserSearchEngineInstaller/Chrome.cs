using System;
using System.IO;
using System.Data.SQLite;

namespace BrowserSearchEngineInstaller
{
    public class Chrome : ISearchEngineInstall
    {
        private string _domain = "nereus.com";

        // On my Windows 7 Machine this directory is located here:
        // C:\Users\jeremcla\AppData\Local\Google\Chrome\User Data\Default
        private string _profileDir;

        public Chrome()
        {
            var appData = Environment.GetEnvironmentVariable("APPDATA");

            _profileDir = Path.Combine(Directory.GetParent(appData).FullName, @"Local\Google\Chrome\User Data\Default");
        }

        public void Install()
        {
            var webDataPath = Path.Combine(_profileDir, "Web Data");
            var tempPath = Path.Combine(_profileDir, "Web Data.sqlite");
            var backupPath = Utils.GetBackupPath(webDataPath);
            File.Copy(webDataPath, backupPath);
            File.Copy(webDataPath, tempPath, true);

            using (var conn = new SQLiteConnection("Data Source=" + tempPath))
            {
                conn.Open();

                var now = DateTime.Now;

                var sql = string.Format("SELECT short_name FROM keywords WHERE short_name='{0}';", "SEL");
                bool exists = false;
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    exists = cmd.ExecuteReader().HasRows;
                }

                if (!exists)
                {
                    sql = string.Format(
                        "INSERT INTO keywords (short_name,keyword,favicon_url,url,safe_for_autoreplace," +
                            "originating_url,date_created,usage_count,input_encodings,show_in_default_list," +
                            "suggest_url,prepopulate_id,created_by_policy,instant_url,last_modified,sync_guid) " +
                        "VALUES('{0}','{1}','{2}','{3}',{4},'{5}',{6},{7},'{8}',{9},'{10}',{11},{12},'{13}',{14},'{15}');",
                        "SEL",
                        "sel",
                        string.Empty,
                        $"http://{_domain}/search?query={{searchTerms}}",
                        0,
                        string.Empty,
                        now.ToUnixTimestamp(),
                        0,
                        "UTF-8",
                        1,
                        string.Empty,
                        0,
                        0,
                        string.Empty,
                        now.ToUnixTimestamp(),
                        Guid.NewGuid());

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            File.Copy(tempPath, webDataPath, true);
            File.Delete(tempPath);
            File.Delete(backupPath);
        }

        public void SetAsDefault()
        {
            // In C:\Users\jeremcla\AppData\Local\Google\Chrome\User Data\Default\Preferences

            // Add or replace this using the values found in the SQLite database.

            //"default_search_provider": {
            //    "enabled": true,
            //    "encodings": "",
            //    "icon_url": "",
            //    "id": "336",
            //    "instant_url": "",
            //    "keyword": "biu",
            //    "name": "BIU",
            //    "prepopulate_id": "0",
            //    "search_url": $"http://{_domain}/search?query={{searchTerms}}",
            //    "suggest_url": "",
            //    "synced_guid": "EE03B328-700D-470D-96B7-90CFF947B559"
            //},

            throw new NotImplementedException();
        }
    }
}
