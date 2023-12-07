using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Helpers
{
    public static class DateTimeExtensions
    {
        public static int GetTotalSeconds(this DateTime dateTime)
        {
            var timespan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            return (int)timespan.TotalSeconds;
        }
        public static int GetTotalSeconds(this DateTime dateTime, DateTimeKind timeKind)
        {
            var timespan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, timeKind);
            return (int)timespan.TotalSeconds;
        }

        public static int? GetTotalSeconds(this DateTime? dateTime)
        {
            if (dateTime == null)
                return null;

            var timespan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            return (int?)timespan.Value.TotalSeconds;
        }
    }
}
