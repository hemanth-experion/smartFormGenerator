using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Experion.Marina.Common
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Method to convert stream into byte array. Useful when you save images in database.
        /// Database field should be VarBinary(Max).
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// To the stream.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static Stream ToStream(this string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return new MemoryStream(byteArray);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(this Stream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Copy from one stream to another.
        /// Example:
        /// using(var stream = response.GetResponseStream())
        /// using(var ms = new MemoryStream())
        /// {
        ///     stream.CopyTo(ms);
        ///      // Do something with copied data
        /// }
        /// </summary>
        /// <param name="fromStream">From stream.</param>
        /// <param name="toStream">To stream.</param>
        public static void CopyTo(this Stream fromStream, Stream toStream)
        {
            if (fromStream == null)
            {
                throw new ArgumentNullException("fromStream");
            }
            if (toStream == null)
            {
                throw new ArgumentNullException("toStream");
            }
            var bytes = new byte[8092];
            int dataRead;
            while ((dataRead = fromStream.Read(bytes, 0, bytes.Length)) > 0)
            {
                toStream.Write(bytes, 0, dataRead);
            }
        }

        /// <summary>
        /// Saves to file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fullFilePath">The full file path.</param>
        public static void SaveToFile(this Stream stream, string fullFilePath)
        {
            if (stream.Length == 0)
            {
                return;
            }
            using (FileStream fileStream = File.Create(fullFilePath, (int)stream.Length))
            {
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }

        /// <summary>
        /// Downloads the stream to the specified HTTP response message.
        /// </summary>
        /// <param name="stream">The stream representing the file content.</param>
        /// <param name="httpResponseMessage">The HTTP response message.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void Download(this Stream stream, HttpResponseMessage httpResponseMessage, string fileName)
        {
            httpResponseMessage.Content = new StreamContent(stream);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
        }
    }
}
