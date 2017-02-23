using Ionic.Zip;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Experion.Marina.Common
{
    public static class CommonUtilities
    {
        /// <summary>
        /// Compresses the byte array.
        /// </summary>
        /// <param name="rawBinaryData">The raw binary data.</param>
        /// <returns></returns>
        public static byte[] CompressByteArray(byte[] rawBinaryData)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(rawBinaryData, 0, rawBinaryData.Length);
                }
                return memory.ToArray();
            }
        }

        /// <summary>
        /// Zips the directory.
        /// </summary>
        /// <param name="inputDirectory">The input directory.</param>
        /// <param name="zipFilePath">The zip file path.</param>
        public static void ZipDirectory(string inputDirectory, string zipFilePath)
        {
            using (var zip = new ZipFile())
            {
                zip.AddDirectory(inputDirectory);
                zip.Save(zipFilePath);
            }
        }

        /// <summary>
        /// Unzip file to a directory.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="outputDirectory">The output directory.</param>
        public static void UnZipFile(string zipFilePath, string outputDirectory)
        {
            ZipFile zip = ZipFile.Read(zipFilePath);
            Directory.CreateDirectory(outputDirectory);
            zip.ExtractAll(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
        }

        /// <summary>
        /// Creates the random text.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string CreateRandomText(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            Random randNum = new Random();
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[(int)(allowedChars.Length * randNum.NextDouble())];
            }

            return new string(chars).ToLower();
        }

        /// <summary>
        /// Creates the thumbnail of the specified input file and save it to the target file path.
        /// </summary>
        /// <param name="inputFile">The input file.</param>
        /// <param name="targetFilePath">The target file path.</param>
        public static void CreateThumbnail(string inputFile, string Filename, string targetFilePath)
        {
            var filename = "";
            byte[] compressedData = null;
            if (!string.IsNullOrEmpty(inputFile))
            {
                compressedData = FileUtilities.GetByteArray(targetFilePath);
                byte[] bytes = Convert.FromBase64String(inputFile);
                compressedData = bytes;
                filename = Filename;
            }
            bool isFileExists = File.Exists(Path.Combine(targetFilePath, filename));
            if (isFileExists)
            {
                filename = GetNewFilename(targetFilePath, filename);
            }

            targetFilePath += "\\" + filename;
            Stream fs = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Write(compressedData, 0, compressedData.Length);
            fs.Dispose();
        }

        /// <summary>
        /// Creates the thumbnail.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void CreateThumbnail(string imagePath, string destinationPath, int width, int height)
        {
            var fileInfo = new FileInfo(imagePath);
            Image.GetThumbnailImageAbort callback = () => true;
            Image image = new Bitmap(imagePath);

            Image pThumbnail = image.GetThumbnailImage(width, height, callback, new IntPtr());
            // 0x0112 is the EXIF byte address for the orientation tag
            if (pThumbnail.PropertyIdList.Contains(0x0112))
            {
                // get the first byte from the orientation tag and convert it to an integer
                var orientationNumber = pThumbnail.GetPropertyItem(0x0112).Value[0];

                switch (orientationNumber)
                {
                    // up is pointing to the right
                    case 8:
                        pThumbnail.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    // up is pointing to the bottom (image is upside-down)
                    case 3:
                        pThumbnail.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    // up is pointing to the left
                    case 6:
                        pThumbnail.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    // up is pointing up (correct orientation)
                    case 1:
                        break;
                }
            }
            pThumbnail.Save(destinationPath + "/" + fileInfo.Name);
            image.Dispose();
        }

        /// <summary>
        /// Gets the new filename.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="filename">Name of the file.</param>
        /// <returns></returns>
        private static string GetNewFilename(string rootPath, string filename)
        {
            string extension = Path.GetExtension(filename);
            if (!(File.Exists(Path.Combine(rootPath, filename))))
            {
                return filename;
            }
            filename = Path.GetFileNameWithoutExtension(filename) + "1" + extension;
            return GetNewFilename(rootPath, filename);
        }

        /// <summary>
        /// To the URL slug.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToUrlSlug(string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        /// <summary>
        /// Servers the name of the host.
        /// </summary>
        /// <returns></returns>
        public static string ServerHostName()
        {
            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
            {
                port = "";
            }
            else
            {
                port = ":" + port;
            }

            string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
            {
                protocol = "http://";
            }
            else
            {
                protocol = "https://";
            }

            return protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port;
        }

        /// <summary>
        /// Gets the request base URL.
        /// </summary>
        /// <returns></returns>
        public static string GetRequestBaseUrl()
        {
            var request = HttpContext.Current.Request;
            string baseUrl = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/";
            return baseUrl;
        }
    }
}
