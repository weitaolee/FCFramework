using System;
using System.Diagnostics;

namespace FC.Framework
{
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1970, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        /// <summary>
        /// convert datetime to unix timestamp
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static int ToUnixTimestamp(this DateTime target)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToUniversalTime(new System.DateTime(1970, 1, 1));
            intResult = (int)(target - startTime).TotalSeconds;
            return intResult;
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DateTime ToLocalDateTime(this int target)
        {
            var time = DateTime.MinValue;

            System.DateTime startTime = new System.DateTime(1970, 1, 1).ToUniversalTime();
            time = startTime.AddSeconds(target);
           
            return time;
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DateTime ToUtcDateTime(this int target)
        {
            var time = DateTime.MinValue;

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(target);
            return time;
        }
    }
}
