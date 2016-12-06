using System.IO;
using System.Linq;

namespace BrowserSearchEngineInstaller
{
    static class Utils
    {
        public static string GetBackupPath(string path)
        {
            var parent = Directory.GetParent(path);
            var matches = parent.GetFiles(Path.GetFileName(path) + "*");

            var proposedNameBase = Path.GetFileName(path) + ".bak";
            var proposedName = proposedNameBase;

            int backupNumber = 1;
            while(matches.Count(x => x.Name == proposedName) > 0)
            {
                proposedName = proposedNameBase + backupNumber++;
            }

            return Path.Combine(parent.FullName, proposedName);
        }
    }
}
