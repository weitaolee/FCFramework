using System;

namespace FC.Framework
{
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1970, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        /// <summary>
        /// convert datetime to unix timestamp
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int ToUnixTimestamp(this DateTime target)
        {
            return (int)((target.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToLocalDateTime(this int target)
        {
            DateTime dtDateTime = new DateTime(621355968000000000 + (long)target * (long)10000000, DateTimeKind.Utc);

            return dtDateTime.ToLocalTime();
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToUtcDateTime(this int target)
        {
            DateTime dtDateTime = new DateTime(621355968000000000 + (long)target * (long)10000000, DateTimeKind.Utc);

            return dtDateTime;
        }

        /// <summary>
        /// get the first day of month
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime target)
        {
            var firstDayOfMonth = new DateTime(target.Year, target.Month, 1);

            return firstDayOfMonth;
        }

        /// <summary>
        /// get the last day of month
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime target)
        {
            var firstDayOfMonth = new DateTime(target.Year, target.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMilliseconds(-3);

            return lastDayOfMonth;
        }

        /// <summary>
        /// is the first day of month
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsFirstDayOfMonth(this DateTime target)
        {
            var firstDayOfMonth = new DateTime(target.Year, target.Month, 1);

            return firstDayOfMonth== target.Date;
        }

        /// <summary>
        /// is the last day of month
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsLastDayOfMonth(this DateTime target)
        {
            var firstDayOfMonth = new DateTime(target.Year, target.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMilliseconds(-3);

            return lastDayOfMonth == target.Date;
        }
    }
}
