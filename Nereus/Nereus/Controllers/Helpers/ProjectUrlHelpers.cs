using System.Text;
using System.Text.RegularExpressions;

namespace Nereus.Controllers.Helpers
{
   public class ProjectUrlHelpers
   {
      public static string NormalizePathPattern(string path)
      {
         // Todo: support putting wildcards anywhere in the path and removing the '/' where appropriate
         var normalizedPath = new StringBuilder(Regex.Escape(path));
         while (normalizedPath[normalizedPath.Length - 1] == '/')
         {
            normalizedPath.Remove(normalizedPath.Length - 1, 1);
         }

         normalizedPath.Insert(0, ".*");
         normalizedPath.Append(".*");

         return normalizedPath.ToString();
      }
   }
}