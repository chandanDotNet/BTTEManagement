using System;
using System.IO;
using System.IO.Compression;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CompressUtility'
    public class CompressUtility
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CompressUtility'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CompressUtility.Compress(DirectoryInfo, string)'
        public static void Compress(DirectoryInfo directorySelected, string directoryPath)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CompressUtility.Compress(DirectoryInfo, string)'
        {
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((System.IO.File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        string path = directoryPath + fileToCompress.Name + ".gz";
                        using (FileStream compressedFileStream = System.IO.File.Create(path))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);

                            }
                        }
                    }

                }
            }
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CompressUtility.Decompress(FileInfo)'
        public static void Decompress(FileInfo fileToDecompress)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CompressUtility.Decompress(FileInfo)'
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = System.IO.File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
                    }
                }
            }
        }
    }
}
