using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Experion.Marina.Common
{
    public static class StringExtensions
	{
		/// <summary>
		/// Gets the first x characters.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="x">The x.</param>
		/// <returns></returns>
		public static string GetFirstXCharacters(this string str, int x)
		{
			if (str.Length <= x)
			{
				return str;
			}

			return str.Substring(0, x);
		}

		/// <summary>
		/// Gets the last x characters.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="x">The x.</param>
		/// <returns></returns>
		public static string GetLastXCharacters(this string str, int x)
		{
			if (str.Length <= x)
			{
				return str;
			}

			return str.Substring(str.Length - x, x);
		}

		/// <summary>
		/// Removes the last character.
		/// </summary>
		/// <param name="instr">The instr.</param>
		/// <returns></returns>
		public static string RemoveLastCharacter(this string instr) => instr.Substring(0, instr.Length - 1);

		/// <summary>
		/// Removes the last 'N' characters.
		/// </summary>
		/// <param name="instr">The instr.</param>
		/// <param name="number">The number.</param>
		/// <returns></returns>
		public static string RemoveLast(this string instr, int number) => instr.Substring(0, instr.Length - number);

		/// <summary>
		/// Removes the first character.
		/// </summary>
		/// <param name="instr">The instr.</param>
		/// <returns></returns>
		public static string RemoveFirstCharacter(this string instr) => instr.Substring(1);

		/// <summary>
		/// Removes the first 'N' characters.
		/// </summary>
		/// <param name="instr">The instr.</param>
		/// <param name="number">The number.</param>
		/// <returns></returns>
		public static string RemoveFirst(this string instr, int number) => instr.Substring(number);

		/// <summary>
		/// Reverses the words.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string ReverseWords(this string text)
		{
			var wordsList = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Reverse();
			return string.Join(" ", wordsList);
		}

		/// <summary>
		/// Remove the Consecutive multiple occurrence of chars
		///
		/// </summary>
		/// <param name="Str"></param>
		/// <param name="Occure">the character which consecutively occurs and required Once</param>
		/// <example>"1    2   3  4  5" can be turn into "1 2 3 4 5"
		///            var out = "1    2   3  4  5".RemoveMultipleOccurence(' ');
		/// </example>
		public static string RemoveMultipleOccurence(this string Str, char Occure)
		{
			Regex regex = new Regex(string.Format(@"[{0}]", Occure) + "{2,}", RegexOptions.None);
			return regex.Replace(Str, Occure.ToString());
		}

		/// <summary>
		/// Trims the prefix.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="prefix">The prefix.</param>
		/// <returns></returns>
		public static string TrimPrefix(this string str, string prefix)
		{
			if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(prefix) && str.StartsWith(prefix))
			{
				return str.Substring(prefix.Length);
			}
			return str;
		}

		/// <summary>
		/// Trims the suffix.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="suffix">The suffix.</param>
		/// <returns></returns>
		public static string TrimSuffix(this string str, string suffix)
		{
			if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(suffix) && str.EndsWith(suffix))
			{
				return str.Remove(str.Length - suffix.Length);
			}
			return str;
		}

		/// <summary>
		/// It will convert a string into T
		/// If Fails then Return Default Value given as argument
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="Str">String which will be converted into T</param>
		/// <param name="defaultValue">if Fails then return this</param>
		/// <returns>Return T</returns>
		public static T To<T>(this string Str, T defaultValue) where T : IConvertible
		{
			try
			{
				return (T)Convert.ChangeType(Str, typeof(T));
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		#region String Basic Type Conversion

		/// <summary>
		///  it will Convert a string into int?
		///  if Fails then return null
		/// </summary>
		/// <param name="IntStr">Int Convertible String</param>
		/// <returns>Int ? </returns>
		/// <example > int i = "23".toInt() ?? 0 </example>
		public static int? ToInt(this string IntStr)
		{
			int i;
			return int.TryParse(IntStr, out i) ? (int?)i : null;
		}

		/// <summary>
		///  it will Convert a string into decimal?
		///  if Fails then return null
		/// </summary>
		/// <param name="decimalStr">decimal Convertible String</param>
		/// <returns>decimal ? </returns>
		/// <example > decimal d = "23.2".toDecimal() ?? 0 </example>
		public static decimal? ToDecimal(this string decimalStr)
		{
			decimal i;
			return decimal.TryParse(decimalStr, out i) ? (decimal?)i : null;
		}

		/// <summary>
		///  it will Convert a string into double?
		///  if Fails then return null
		/// </summary>
		/// <param name="dobuleStr">Double Convertible String</param>
		/// <returns>double ? </returns>
		/// <example > double d = "23.2".toDouble() ?? 0.0; </example>
		public static double? ToDouble(this string dobuleStr)
		{
			double i;
			return double.TryParse(dobuleStr, out i) ? (double?)i : null;
		}

		/// <summary>
		///  it will Convert a string into float?
		///  if Fails then return null
		/// </summary>
		/// <param name="floatStr">float Convertible String</param>
		/// <returns> float? </returns>
		/// <example > float f = "23.2".toFloat() ?? 0.0; </example>
		public static float? ToFloat(this string floatStr)
		{
			float i;
			return float.TryParse(floatStr, out i) ? (float?)i : null;
		}

		/// <summary>
		///  it will Convert a string into long?
		///  if Fails then return null
		/// </summary>
		/// <param name="longStr">toLong Convertible String</param>
		/// <returns> long? </returns>
		/// <example > long l = "23".toLong() ?? 0; </example>
		public static long? ToLong(this string longStr)
		{
			long i;
			return long.TryParse(longStr, out i) ? (long?)i : null;
		}

		/// <summary>
		///  it will Convert a string into short?
		///  if Fails then return null
		/// </summary>
		/// <param name="shortStr">short Convertible String</param>
		/// <returns> short? </returns>
		/// <example > short s = "23".toShort() ?? 0; </example>
		public static short? ToShort(this string shortStr)
		{
			short i;
			return short.TryParse(shortStr, out i) ? (short?)i : null;
		}

		/// <summary>
		///  it will Convert a string into bool?
		///  if Fails then return null
		/// </summary>
		/// <param name="BoolStr">boolStr Convertible String</param>
		/// <returns>bool ? </returns>
		/// <example > bool i = "true".toBool() ?? 0 </example>
		public static bool? ToBool(this string boolStr)
		{
			bool i;
			return bool.TryParse(boolStr, out i) ? (bool?)i : null;
		}

		#endregion String Basic Type Conversion

		#region DateTime Conversion

		/// <summary>
		/// It will try to Convert a given string into DateTime
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static DateTime? ToDateTime(this string str)
		{
			DateTime i;
			return DateTime.TryParse(str, out i) ? (DateTime?)i : null;
		}

		/// <summary>
		/// It will Convert the String into the Given Format.
		/// Note : it will Return DateTime.MinValue Back if Unable to Convert due to Exceptions
		/// Date Pattern MM-dd-yyyy / dd-MM-yyyy
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="ShortDatePattern">The short date pattern.</param>
		/// <returns>It will return DateTime in a give Format</returns>
		public static string ToDateFormat(this string str, string ShortDatePattern = "MMM-dd-yyyy")
		{
			DateTime date = DateTime.MinValue;
			if (DateTime.TryParse(str, out date))
			{
				return date.ToString(ShortDatePattern);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// it will compare the given date with DateTime.Now and Return you Difference in Seconds,Minutes,Hours
		/// But Not In Days
		/// </summary>
		/// <param name="date">this will be Compare with Current DateTime.Now</param>
		/// <param name="TimesAgoSuffix">Any Suffix after Difference</param>
		/// <param name="DateFormat">Return String Format</param>
		/// <returns>4 Seconds Ago, 5 Hours Ago</returns>
		public static string GetTimeAgo(this DateTime date, string TimesAgoSuffix = "ago",
			string DateFormat = "dd MMM yyyy")
		{
			TimeSpan ts = DateTime.Now - date;
			if (date.Date == DateTime.Today)
			{
				if (((int)ts.TotalDays) > 0)
				{
					return date.ToDateFormat(DateFormat);
				}
				else if (((int)ts.TotalHours) > 0)
				{
					int h = (int)ts.TotalHours;
					return h + (h == 1 ? "hour" : " hours ") + TimesAgoSuffix;
				}
				else if (((int)ts.TotalMinutes) > 0)
				{
					int m = (int)ts.TotalMinutes;
					return m + (m == 1 ? " minute " : " minutes") + TimesAgoSuffix;
				}
				else if (((int)ts.TotalSeconds) > 0)
				{
					int sec = (int)ts.TotalSeconds;
					return sec + (sec == 1 ? " second " : " seconds ") + TimesAgoSuffix;
				}
			}
			return date.ToDateFormat(DateFormat);
		}

		#endregion DateTime Conversion

		#region Regex Extensions

		/// <summary>
		/// In a specified input string, replaces all strings that match a specified regular
		///     expression with a specified replacement string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="replacement">The replacement.</param>
		/// <returns></returns>
		public static string RegexReplace(this string input, string pattern, string replacement) => Regex.Replace(input, pattern, replacement);

		/// <summary>
		/// Searches the specified input string for all occurrences of a specified regular
		///     expression, using the specified matching options.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="regexOptions">The regex options.</param>
		/// <returns></returns>
		public static MatchCollection RegexMatches(this string input, string pattern, RegexOptions regexOptions = RegexOptions.IgnoreCase) => Regex.Matches(input, pattern, regexOptions);

		/// <summary>
		/// Searches the input string for the first occurrence of the specified regular expression,
		///     using the specified matching options.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="regexOptions">The regex options.</param>
		/// <returns></returns>
		public static Match RegexMatch(this string input, string pattern, RegexOptions regexOptions = RegexOptions.IgnoreCase) => Regex.Match(input, pattern, regexOptions);

		#endregion Regex Extensions

		#region HTML

		/// <summary>
		/// HTMLs the data decode.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string HtmlDataDecode(this string data) => System.Net.WebUtility.HtmlDecode(data);

		/// <summary>
		/// HTMLs the data encode.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string HtmlDataEncode(this string data) => System.Net.WebUtility.HtmlEncode(data);

		/// <summary>
		/// HTMLs the URL encode.
		/// </summary>
		/// <param name="URL">The URL.</param>
		/// <returns></returns>
		public static string HtmlUrlEncode(this string URL) => System.Uri.EscapeUriString(URL);

		#endregion HTML

		#region Boolean Return Functions

		/// <summary>
		/// Function to test for Positive Integers.
		/// Determines whether [is natural number].
		/// </summary>
		/// <param name="strNumber">The string number.</param>
		/// <returns></returns>
		public static bool IsNaturalNumber(this string strNumber)
		{
			Regex objNotNaturalPattern = new Regex("[^0-9]");
			Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");
			return !objNotNaturalPattern.IsMatch(strNumber) &&
			objNaturalPattern.IsMatch(strNumber);
		}

		/// <summary>
		/// Function to test for Positive Integers with zero inclusive
		/// Determines whether [is whole number].
		/// </summary>
		/// <param name="strNumber">The string number.</param>
		/// <returns></returns>
		public static bool IsWholeNumber(this string strNumber)
		{
			Regex objNotWholePattern = new Regex("[^0-9]");
			return !objNotWholePattern.IsMatch(strNumber);
		}

		/// <summary>
		/// Function to Test for Integers both Positive & Negative
		/// Determines whether this instance is integer.
		/// </summary>
		/// <param name="strNumber">The string number.</param>
		/// <returns></returns>
		public static bool IsInteger(this string strNumber)
		{
			Regex objNotIntPattern = new Regex("[^0-9-]");
			Regex objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");
			return !objNotIntPattern.IsMatch(strNumber) && objIntPattern.IsMatch(strNumber);
		}

		/// <summary>
		/// Function to Test for Positive Number both Integer & Real
		/// Determines whether [is positive number].
		/// </summary>
		/// <param name="strNumber">The string number.</param>
		/// <returns></returns>
		public static bool IsPositiveNumber(this string strNumber)
		{
			Regex objNotPositivePattern = new Regex("[^0-9.]");
			Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
			Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
			return !objNotPositivePattern.IsMatch(strNumber) &&
			objPositivePattern.IsMatch(strNumber) &&
			!objTwoDotPattern.IsMatch(strNumber);
		}

		/// <summary>
		/// Function to test whether the string is valid number or not
		/// Determines whether this instance is number.
		/// </summary>
		/// <param name="strNumber">The string number.</param>
		/// <returns></returns>
		public static bool IsNumber(this string strNumber)
		{
			Regex objNotNumberPattern = new Regex("[^0-9.-]");
			Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
			Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
			string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
			string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
			Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
			return !objNotNumberPattern.IsMatch(strNumber) &&
			!objTwoDotPattern.IsMatch(strNumber) &&
			!objTwoMinusPattern.IsMatch(strNumber) &&
			objNumberPattern.IsMatch(strNumber);
		}

		/// <summary>
		/// Function To test for Alphabets.
		/// Determines whether this instance is alpha.
		/// </summary>
		/// <param name="strToCheck">The string to check.</param>
		/// <returns></returns>
		public static bool IsAlpha(this string strToCheck)
		{
			Regex objAlphaPattern = new Regex("[^a-zA-Z]");
			return !objAlphaPattern.IsMatch(strToCheck);
		}

		/// <summary>
		/// Function to Check for AlphaNumeric.
		/// Determines whether [is alpha numeric].
		/// </summary>
		/// <param name="strToCheck">The string to check.</param>
		/// <returns></returns>
		public static bool IsAlphaNumeric(this string strToCheck)
		{
			Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
			return !objAlphaNumericPattern.IsMatch(strToCheck);
		}

		/// <summary>
		/// Determines whether [is valid email] [the specified email].
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public static bool IsValidEmail(this string email)
		{
			Regex objEmailPattern = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
		   + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
		   + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
		   + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$");
			return !objEmailPattern.IsMatch(email);
		}

		#endregion Boolean Return Functions
	}
}
