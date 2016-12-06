using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserSearchEngineInstaller
{
    static public class DateTimeExtensions
    {
        static public int ToUnixTimestamp(this DateTime _this)
        {
            TimeSpan diff = _this - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (int)diff.TotalSeconds;
        }
    }
}
