using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Experion.Marina.Common
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Betweens the specified range beg.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="rangeBeg">The range beg.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <returns></returns>
        public static bool Between(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        /// <summary>
        /// Calculates the age.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now < dateTime.AddYears(age))
                age--;
            return age;
        }

        /// <summary>
        /// DateDiff in SQL style.
        /// Datepart implemented:
        ///     "year" (abbr. "yy", "yyyy"),
        ///     "quarter" (abbr. "qq", "q"),
        ///     "month" (abbr. "mm", "m"),
        ///     "day" (abbr. "dd", "d"),
        ///     "week" (abbr. "wk", "ww"),
        ///     "hour" (abbr. "hh"),
        ///     "minute" (abbr. "mi", "n"),
        ///     "second" (abbr. "ss", "s"),
        ///     "millisecond" (abbr. "ms").
        /// </summary>
        /// <param name="datePart"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static Int64 DateDiff(this DateTime startDate, String datePart, DateTime endDate)
        {
            Int64 dateDiffVal = 0;
            Calendar cal = new GregorianCalendar();
            TimeSpan ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (datePart.ToLower().Trim())
            {
                #region year

                case "year":
                case "yy":
                case "yyyy":
                    dateDiffVal = (Int64)(cal.GetYear(endDate) - cal.GetYear(startDate));
                    break;

                #endregion year

                #region quarter

                case "quarter":
                case "qq":
                case "q":
                    dateDiffVal = (Int64)((((cal.GetYear(endDate)
                                        - cal.GetYear(startDate)) * 4)
                                        + ((cal.GetMonth(endDate) - 1) / 3))
                                        - ((cal.GetMonth(startDate) - 1) / 3));
                    break;

                #endregion quarter

                #region month

                case "month":
                case "mm":
                case "m":
                    dateDiffVal = (Int64)(((cal.GetYear(endDate)
                                        - cal.GetYear(startDate)) * 12
                                        + cal.GetMonth(endDate))
                                        - cal.GetMonth(startDate));
                    break;

                #endregion month

                #region day

                case "day":
                case "d":
                case "dd":
                    dateDiffVal = (Int64)ts.TotalDays;
                    break;

                #endregion day

                #region week

                case "week":
                case "wk":
                case "ww":
                    dateDiffVal = (Int64)(ts.TotalDays / 7);
                    break;

                #endregion week

                #region hour

                case "hour":
                case "hh":
                    dateDiffVal = (Int64)ts.TotalHours;
                    break;

                #endregion hour

                #region minute

                case "minute":
                case "mi":
                case "n":
                    dateDiffVal = (Int64)ts.TotalMinutes;
                    break;

                #endregion minute

                #region second

                case "second":
                case "ss":
                case "s":
                    dateDiffVal = (Int64)ts.TotalSeconds;
                    break;

                #endregion second

                #region millisecond

                case "millisecond":
                case "ms":
                    dateDiffVal = (Int64)ts.TotalMilliseconds;
                    break;

                #endregion millisecond

                default:
                    throw new Exception(String.Format("DatePart \"{0}\" is unknown", datePart));
            }
            return dateDiffVal;
        }

        /// <summary>
        /// Returns the number of days in the month of the given date.
        /// </summary>
        public static int DaysInMonth(this DateTime date)
        {
            int result = 31;
            switch (date.Month)
            {
                case 2:
                    result = date.IsLeapYear() ? 29 : 28;
                    break;

                case 4:
                case 6:
                case 9:
                case 11:
                    result = 30;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Gets the day of week from input date string.
        /// Input string should be in valid date format like "M/d/yyyy","dd-MM-yyyy","dd/MM/yyyy","M/d/yyyy h:mm tt".
        /// </summary>
        /// <param name="dateTime">The date.</param>
        /// <returns>day of week</returns>
        public static DayOfWeek GetDayOfWeek(this DateTime dateTime)
        {
            DayOfWeek dayOfWeek;
            dayOfWeek = dateTime.DayOfWeek;
            return dayOfWeek;
        }

        /// <summary>
        /// Gets the last day of month.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Returns true if given date time falls in the morning (AM).
        /// </summary>
        public static bool IsAm(this DateTime time)
        {
            return time.TimeOfDay < new DateTime(2000, 1, 1, 12, 0, 0).TimeOfDay;
        }

        /// <summary>
        /// Returns true if the given date falls in a leap year.
        /// </summary>
        public static bool IsLeapYear(this DateTime date)
        {
            int year = date.Year;
            return ((year.IsMultipleOf(4) && !year.IsMultipleOf(100)) || year.IsMultipleOf(400));
        }

        /// <summary>
        /// Returns true if given date time falls in the afternoon (PM).
        /// </summary>
        public static bool IsPm(this DateTime time)
        {
            return time.TimeOfDay >= new DateTime(2000, 1, 1, 12, 0, 0).TimeOfDay;
        }

        /// <summary>
        /// Determines whether the given date is a weekend (Sunday or Saturday).
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Provides the next date on which the same week day repeats.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns></returns>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;
            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }
            DateTime result = current.AddDays(offsetDays);
            return result;
        }

        /// <summary>
        /// Get the Next the working day of a given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime NextWorkday(this DateTime date)
        {
            var nextDay = date;
            while (!nextDay.WorkingDay())
            {
                nextDay = nextDay.AddDays(1);
            }
            return nextDay;
        }

        /// <summary>
        /// Converts the date time in to specified format.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="shortDatePattern">The short date pattern.</param>
        /// <returns></returns>
        public static string ToDateFormat(this DateTime date, string shortDatePattern = "MMM-dd-yyyy")
        {
            try
            {
                return date.ToString(shortDatePattern, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the date string specified in the given input format to the specified output format .
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="inputFormat">The input format.</param>
        /// <param name="outputFormat">The output format.</param>
        /// <returns>converted date</returns>
        public static string ToDateFormat(this string dateString, string inputFormat, string outputFormat)
        {
            string convertedDate = string.Empty;
            DateTime dateTime;
            string[] dateFormats = { inputFormat, outputFormat };
            if (dateString != null &&
                DateTime.TryParseExact(dateString, dateFormats, new CultureInfo("en-US"),
                DateTimeStyles.None, out dateTime))
            {
                convertedDate = dateTime.ToString(outputFormat);
            }
            return convertedDate;
        }

        /// <summary>
        /// To the readable time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToReadableTime(this DateTime value)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - value.Ticks);
            double delta = ts.TotalSeconds;
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }

        /// <summary>
        /// Returns the quarter of the year within which the given date falls:
        /// Jan, Feb, Mar = 1
        /// ...
        /// Oct, Nov, Dec = 4
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int Quarter(this DateTime date)
        {
            return (date.Month + 2) / 3;
        }

        /// <summary>
        /// To the UNIX time stamp.
        /// </summary>
        /// <param name="dateTimeUtc">The date time UTC.</param>
        /// <returns></returns>
        public static long ToUnixTimeStamp(this DateTime dateTimeUtc)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan timeSpan = dateTimeUtc - epoch;
            return Convert.ToInt64(timeSpan.TotalSeconds);
        }

        /// <summary>
        /// Verifies whether the given date is a working day (other than Sunday or Saturday)
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static bool WorkingDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Converts time to 12 HR format
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static string To12Hr(this string time)
        {
            var timeFromInput = DateTime.ParseExact(time, "HH:mm", null, DateTimeStyles.None);
            if (Thread.CurrentThread.CurrentCulture.Name == "ar")
            {
                CultureInfo culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
                culture.DateTimeFormat.Calendar = new HijriCalendar();
                return timeFromInput.ToString("hh:mm tt", culture);
            }

            return timeFromInput.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts time to 24 HR format
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static string To24Hr(this string time)
        {
            string timeOutput = time;
            DateTime timeFromInput;
            if (DateTime.TryParseExact(time, "hh:mm tt", null, DateTimeStyles.None, out timeFromInput) ||
                DateTime.TryParseExact(time, "h:mm tt", null, DateTimeStyles.None, out timeFromInput) ||
                DateTime.TryParseExact(time, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeFromInput))
            {
                timeOutput = timeFromInput.ToString("HH:mm", CultureInfo.InvariantCulture);
            }
            return timeOutput;
        }

        /// <summary>
        /// Converts time string to minutes
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static int ToMinutes(this string time)
        {
            var timeFromInput = DateTime.ParseExact(time, "HH:mm", null, DateTimeStyles.None);
            return timeFromInput.Hour * 60 + timeFromInput.Minute;
        }

        /// <summary>
        /// Converts the minutes provided into a 24 hr time string of format HH:mm
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static string To24HrTimeString(this int minutes)
        {
            var today = DateTime.Today.AddMinutes(minutes);
            return today.ToString("HH:mm");
        }

        /// <summary>
        /// Converts the minutes provided into a 12 hr time string of format hh:mm tt
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static string To12HrTimeString(this int minutes)
        {
            var today = DateTime.Today.AddMinutes(minutes);
            return today.ToString("hh:mm tt");
        }

        /// <summary>
        /// Converts the minutes to the given application time format
        /// Format can be either 12 or 24.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ToApplicationTimeFormat(this int minutes, string format)
        {
            if (format.Equals("12"))
            {
                return minutes.To12HrTimeString();
            }
            return minutes.To24HrTimeString();
        }

        /// <summary>
        /// To the time zone.
        /// </summary>
        /// <param name="utcDate">The UTC date.</param>
        /// <param name="timeZoneId">The time zone identifier.</param>
        /// <returns></returns>
        public static DateTime ToTimeZone(this DateTime utcDate, string timeZoneId)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, cstZone);
        }

        /// <summary>
        /// Adds the distinct days.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public static List<DateTime> AddDistinctDays(this List<DateTime> days, DateTime startDate, DateTime endDate)
        {
            if (days == null)
            {
                return days;
            }

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!days.Contains(date))
                {
                    days.Add(date);
                }
            }

            return days;
        }

        public static DateTime ToUtcDate(this DateTime date)
        {
            DateTime utcDate = TimeZoneInfo.ConvertTimeToUtc(date);
            return utcDate;
        }

        ///// <summary>
        ///// To the time band string.
        ///// </summary>
        ///// <param name="minutes">The minutes.</param>
        ///// <returns></returns>
        public static string To24HrTimeBandString(this int minutes)
        {
            var band = minutes / 60;
            return band.ToString("D2") + ":" + (band + 1).ToString("D2");
        }

        /// <summary>
        /// To12s the hr time band string.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static string To12HrTimeBandString(this int minutes)
        {
            var fromTimeSpan = new DateTime().AddMinutes(minutes);
            var toTimeSpan = fromTimeSpan.AddHours(1);
            return fromTimeSpan.TimeOfDay.Hours.ToString("D2") + " " + fromTimeSpan.ToString("tt") + " - "
                    + toTimeSpan.TimeOfDay.Hours.ToString("D2") + " " + toTimeSpan.ToString("tt");
        }
    }
}
