using System;

namespace Experion.Marina.Common
{
    public static class DateTimeUtilities
    {
        /// <summary>
        /// Converts time stamp to date time.
        /// </summary>
        /// <param name="timestamp">The time stamp.</param>
        /// <returns>date time</returns>
        public static DateTime ConvertToDateTime(long timestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dateTime.AddSeconds(timestamp);
        }
    }
}
