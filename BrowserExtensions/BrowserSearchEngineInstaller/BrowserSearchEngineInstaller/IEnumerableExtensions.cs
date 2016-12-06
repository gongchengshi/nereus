using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserSearchEngineInstaller
{
    static class IEnumerableExtensions
    {
        static T FirstOrThrow<T>(this IEnumerable<T> _this, Exception ex)
        {
            var found = _this.FirstOrDefault();

            if (default(T).Equals(found))
            {
                throw ex;
            }

            return found;
        }
    }
}
