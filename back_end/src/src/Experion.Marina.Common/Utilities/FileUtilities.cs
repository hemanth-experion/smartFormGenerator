using System.IO;

namespace Experion.Marina.Common
{
    public static class FileUtilities
    {
        /// <summary>
        /// Deletes the files in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public static void DeleteFilesInDirectory(string directoryPath)
        {
            if (!string.IsNullOrEmpty(directoryPath))
            {
                foreach (string file in Directory.GetFiles(directoryPath))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="FilePath">The file path.</param>
        public static void CreateDirectory(string FilePath)
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
        }

        /// <summary>
        /// Gets the byte array of the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>byte array</returns>
        public static byte[] GetByteArray(string filePath)
        {
            byte[] bytes = null;
            if (File.Exists(filePath))
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    bytes = binaryReader.ReadBytes((int)stream.Length);
                }
            }
            return bytes;
        }
    }
}
