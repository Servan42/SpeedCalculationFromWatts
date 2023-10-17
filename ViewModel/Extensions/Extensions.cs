using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Extensions
{
    public static class Extensions
    {
        public static string GetHourMinSecString(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours.ToString("00")}h{timeSpan.Minutes.ToString("00")}m{timeSpan.Seconds.ToString("00")}s";
        }
    }
}
