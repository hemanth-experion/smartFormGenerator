using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace Experion.Marina.Common
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Supported hash algorithms
        /// </summary>
        public enum eHashType
        {
            HMACMD5, HMACSHA1, HMACSHA256, HMACSHA384, HMACSHA512,
            MD5, SHA1, SHA256, SHA384, SHA512
        }

        /// <summary>
        /// Defines the different encryption algorithms supported by this class
        /// </summary>
        public enum EncryptionType
        {
            TDES,
            AES
        }

        /// <summary>
        /// Computes the hash of the string using a specified hash algorithm
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <param name="hashType">The hash algorithm to use</param>
        /// <returns>The resulting hash or an empty string on error</returns>
        public static string ComputeHash(this string input, eHashType hashType)
        {
            try
            {
                byte[] hash = GetHash(input, hashType);
                StringBuilder ret = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    ret.Append(hash[i].ToString("x2"));
                }
                return ret.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Decrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string Decrypt(this string input, EncryptionType type)
        {
            if (type == EncryptionType.TDES)
            {
                return TdesDecryption(input);
            }
            return AesDecryption(input);
        }

        /// <summary>
        /// it will create a Deep Clone of Given Object
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="clone">The clone.</param>
        /// <returns></returns>
        public static T DeepClone<T>(this T clone)
        {
            if (clone != null)
            {
                DataContractJsonSerializer formatter = new DataContractJsonSerializer(typeof(T));
                MemoryStream stream = new MemoryStream();

                formatter.WriteObject(stream, clone);
                stream.Seek(0, SeekOrigin.Begin);

                T result = (T)formatter.ReadObject(stream);
                stream.Dispose();
                return result;
            }
            return default(T);
        }

        /// <summary>
        /// Encrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string Encrypt(this string input, EncryptionType type)
        {
            if (type == EncryptionType.TDES)
            {
                return TdesEncryption(input);
            }
            return AesEncryption(input);
        }

        /// <summary>
        /// It will first split the string with the separator and generate a list<string>
        /// After this, it will try to convert List<string> into List<T>
        /// If Conversion fails then return the default value for that item
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="str">String</param>
        /// <param name="separator">Separator to split the String</param>
        /// <param name="defaultValue">set Default Value if conversion fails</param>
        /// <returns>return the List<T> holds converted values or default values</returns>
        public static List<T> ToList<T>(this string str, char separator, T defaultValue) where T : IConvertible
        {
            List<string> lstStr = str.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();

            List<T> lst = new List<T>();
            lstStr.ForEach(s => lst.Add(s.To<T>(defaultValue)));
            return lst;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(this List<string> strings, char separator)
        {
            StringBuilder concatenatedString = new StringBuilder();
            strings.ForEach(x => concatenatedString.Append(x).Append(separator));
            return concatenatedString.ToString();
        }

        /// <summary>
        /// You have to Pass List it will check if an object contains or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T source, params T[] list) => list.Contains(source);

        /// <summary>
        /// Determines whether the specified lower is between.
        /// </summary>
        /// <param name="theNumber">The number.</param>
        /// <param name="lower">The lower.</param>
        /// <param name="higher">The higher.</param>
        /// <returns></returns>
        public static bool IsBetween(this int theNumber, int lower, int higher) => (theNumber >= lower) && (theNumber <= higher);

        /// <summary>
        /// Converts the current object To the json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            if (obj != null)
            {
                return JsonConvert.SerializeObject(obj);
            }
            return null;
        }

        /// <summary>
        /// Convert the JSON string to its corresponding object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T FromJson<T>(this string obj)
        {
            if (obj != null)
            {
                return JsonConvert.DeserializeObject<T>(obj);
            }
            return default(T);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static T GetPropertyValue<T>(this object source, string property)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperties();

            var propertyValue = (from s in sourceProperties
                                 where s.Name.Equals(property)
                                 select s.GetValue(source, null)).FirstOrDefault();

            return propertyValue != null ? (T)propertyValue : default(T);
        }

        /// <summary>
        /// Generates tree of items from item list
        /// </summary>
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        ///
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }

        /// <summary>
        /// To the size of the file.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string ToFileSize(this long size)
        {
            if (size < 1024) { return (size).ToString("F0") + " bytes"; }
            if (size < Math.Pow(1024, 2)) { return (size / 1024).ToString("F0") + "KB"; }
            if (size < Math.Pow(1024, 3)) { return (size / Math.Pow(1024, 2)).ToString("F0") + "MB"; }
            if (size < Math.Pow(1024, 4)) { return (size / Math.Pow(1024, 3)).ToString("F0") + "GB"; }
            if (size < Math.Pow(1024, 5)) { return (size / Math.Pow(1024, 4)).ToString("F0") + "TB"; }
            if (size < Math.Pow(1024, 6)) { return (size / Math.Pow(1024, 5)).ToString("F0") + "PB"; }
            return (size / Math.Pow(1024, 6)).ToString("F0") + "EB";
        }

        /// <summary>
        /// This extension method replaces an item in a collection that implements the IList interface.
        /// </summary>
        /// <typeparam name="T">The type of the field that we are manipulating</typeparam>
        /// <param name="thisList">The input list</param>
        /// <param name="position">The position of the old item</param>
        /// <param name="item">The item we are going to put in it's place</param>
        /// <returns>True in case of a replace, false if failed</returns>
        public static bool Replace<T>(this IList<T> thisList, int position, T item)
        {
            if (position > thisList.Count - 1) // only process if inside the range of this list
            {
                return false;
            }

            thisList.RemoveAt(position); // remove the old item
            thisList.Insert(position, item); // insert the new item at its position
            return true; // return success
        }

        /// <summary>
        /// Determines whether [is multiple of] [the specified parameter].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public static bool IsMultipleOf(this int value, int param) => value % param == 0 ? true : false;

        /// <summary>
        /// Performs the specified action for each of the element in the enumeration list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action.</param>
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// Converts the list to CSV (Comma will be separator).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string ToCsv<T>(this IEnumerable<T> instance) => instance.ToCsv(',');

        /// <summary>
        /// Converts the list to CSV (Character can be specified).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string ToCsv<T>(this IEnumerable<T> instance, char separator)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        /// <summary>
        /// To the printable format.
        ///
        /// TODO - Not fully functional
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="printableString">The printable string.</param>
        /// <returns></returns>
        public static string ToPrintableFormat<T>(this T array, StringBuilder printableString = null)
        {
            if (printableString == null)
            {
                printableString = new StringBuilder();
            }
            if (array.GetType().FullName.Contains("System.Collections.Generic.IEnumerable") || array.GetType().IsArray)
            {
                IEnumerable p = array as IEnumerable;
                if (p != null)
                {
                    foreach (var item in p)
                    {
                        if (item.GetType().FullName.Contains("System.Collections.Generic.IEnumerable") || item.GetType().IsArray)
                        {
                            printableString.Append("[").Append(item.ToPrintableFormat()).Append("]");
                        }
                        else
                        {
                            printableString.Append(item).Append(", ");
                        }
                    }
                }
                if (printableString.Length > 0)
                {
                    printableString = new StringBuilder(printableString.ToString().Trim().TrimSuffix(","));
                }
            }
            else
            {
                printableString.Append(array.ToString());
            }
            return printableString.ToString();
        }

        /// <summary>
        /// Gets the dictionary value if exists.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetDictionaryValueIfExists(this Dictionary<string, string> dictionary, string key)
        {
            if (dictionary != null && dictionary.Any() && dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return string.Empty;
        }

        #region Private Methods

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        private static byte[] GetHash(string input, eHashType hash)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            switch (hash)
            {
                case eHashType.HMACMD5:
                    return new HMACMD5().ComputeHash(inputBytes);

                case eHashType.HMACSHA1:
                    return new HMACSHA1().ComputeHash(inputBytes);

                case eHashType.HMACSHA256:
                    return new HMACSHA256().ComputeHash(inputBytes);

                case eHashType.HMACSHA384:
                    return new HMACSHA384().ComputeHash(inputBytes);

                case eHashType.HMACSHA512:
                    return new HMACSHA512().ComputeHash(inputBytes);

                case eHashType.MD5:
                    return MD5.Create().ComputeHash(inputBytes);

                case eHashType.SHA1:
                    return SHA1.Create().ComputeHash(inputBytes);

                case eHashType.SHA256:
                    return SHA256.Create().ComputeHash(inputBytes);

                case eHashType.SHA384:
                    return SHA384.Create().ComputeHash(inputBytes);

                case eHashType.SHA512:
                    return SHA512.Create().ComputeHash(inputBytes);

                default:
                    return inputBytes;
            }
        }

        /// <summary>
        /// Decrypt the encrypted string using AES algorithm.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>decrypted string</returns>
        private static string AesDecryption(string input)
        {
            string EncryptionKey = "V@nP001_20164C1tYgR0uP";
            byte[] cipherBytes = Convert.FromBase64String(input);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Dispose();
                    }
                    input = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return input;
        }

        /// <summary>
        /// Encrypt the input string using AES algorithm.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>encrypted string</returns>
        private static string AesEncryption(string input)
        {
            string EncryptionKey = "V@nP001_20164C1tYgR0uP";
            byte[] clearBytes = Encoding.Unicode.GetBytes(input);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Dispose();
                    }
                    input = Convert.ToBase64String(ms.ToArray());
                }
            }
            return input;
        }

        /// <summary>
        /// Decrypt the encrypted string using Triple DES algorithm.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>decrypted string</returns>
        private static string TdesDecryption(string password)
        {
            byte[] Results;
            UTF8Encoding UTF8 = new UTF8Encoding();
            string key = "V@nP001_2016";
            String ivString = "C1tYgR0uP";
            byte[] ivBytes = UTF8.GetBytes(ivString);

            MD5 md5 = MD5.Create();

            byte[] keyBytes = md5.ComputeHash(UTF8.GetBytes(key));
            byte[] TDESKey = new byte[24];
            Array.Copy(keyBytes, TDESKey, 16);

            TripleDES des = TripleDES.Create();
            des.Key = TDESKey;
            des.IV = ivBytes;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(password);

            try
            {
                ICryptoTransform Decryptor = des.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                des.Dispose();
                md5.Dispose();
            }
            return UTF8.GetString(Results);
        }

        /// <summary>
        /// Encrypt the input string using Triple DES algorithm.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Encrypted string</returns>
        private static string TdesEncryption(string input)
        {
            byte[] Results;
            UTF8Encoding UTF8 = new UTF8Encoding();
            string key = "V@nP001_2016";
            string ivString = "C1tYgR0uP";
            byte[] ivBytes = UTF8.GetBytes(ivString);

            MD5 md5 = MD5.Create();
            byte[] keyBytes = md5.ComputeHash(UTF8.GetBytes(key));
            byte[] TDESKey = new byte[24];
            Array.Copy(keyBytes, TDESKey, 16);
            TripleDES des = TripleDES.Create();
            des.Key = TDESKey;
            des.IV = ivBytes;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;

            byte[] plainText = UTF8.GetBytes(input);
            try
            {
                ICryptoTransform Encryptor = des.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            finally
            {
                des.Dispose();
                md5.Dispose();
            }
            return Convert.ToBase64String(Results);
        }

        #endregion Private Methods
    }

    /// <summary>
    /// Class to define a tree item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}
