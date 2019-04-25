using System;

namespace BsonInspector.Core.Utility
{
    public static class DateTimeHelper
    {
        public static DateTime GetDateTimeFromUnixTicks(long ticks)
        {
            var realTicks = (ticks * 10000) + 621355968000000000; //ticks since the Unix epoch.
            return new DateTime(realTicks);
        }
    }
}
