using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Microsoft.Deployment.Compression;
using Microsoft.Deployment.Compression.Cab;
using CompressionLevel = Microsoft.Deployment.Compression.CompressionLevel;

namespace GalaxyMerge.IO
{
    public static class Pkg
    {
        //private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private const string File1Name = "File1.cab";

        public static bool Exists(string path) => File.Exists(path);

        public static void Unpack(string sourceFileName, string destDirectory)
        {
            var file1 = ExtractFile1(sourceFileName, destDirectory);

            if (!file1.Exists)
            {
                //TODO throw custom exception on what comes out of file1 extraction?
                //Logger.Error("File1 does not exists upon unpacking Pgk");
                throw new InvalidOperationException(
                    "We can continue not knowing that File.cab did not come out of this");
            }
            
            ZipFile.ExtractToDirectory(file1.FullName, destDirectory);
            
            File.Delete(file1.FullName);
        }

        public static void Pack(string sourceDirectory, string destFileName)
        {
            ValidatePackage(sourceDirectory);
            
            Directory.CreateDirectory(Path.GetDirectoryName(destFileName)!);
            
            var file1 = GenerateFile1(sourceDirectory);

            var sourceFileNames = new List<string> {file1.FullName};
            var cab = new CabInfo(destFileName);
            cab.PackFiles(null, sourceFileNames, null, CompressionLevel.Max, PgkProgressHandler);

            File.Delete(file1.FullName);
        }

        //TODO: This validation probably should be more extensive. Perhaps create a class to contain logic?
        private static void ValidatePackage(string directoryName)
        {
            var directory = new DirectoryInfo(directoryName);
            
            if (!directory.Exists)
            {
                throw new IOException("Package directory does not exists");
            }

            if (directory.GetFiles("Manifest.xml").Length == 0)
                throw new IOException("");
            
            /*if (directory.GetFiles("*.aaPDF").Length == 0)
                throw new IOException("");*/
        }

        private static FileInfo ExtractFile1(string fileName, string destDirectory)
        {
            ZipFile.ExtractToDirectory(fileName, destDirectory);
            var file1 = Path.Combine(destDirectory, File1Name);
            return new FileInfo(file1);
        }

        private static FileInfo GenerateFile1(string sourceDirectory,
            CompressionLevel compressionLevel = CompressionLevel.Max,
            bool includeSubDirectories = true)
        {
            var file1 = Path.Combine(Path.GetTempPath(), File1Name);
            var cab = new CabInfo(file1);
            cab.Pack(sourceDirectory, includeSubDirectories, compressionLevel, PgkProgressHandler);
            return new FileInfo(file1);
        }

        private static void PgkProgressHandler(object sender, ArchiveProgressEventArgs e)
        {
            //Logger.Trace(e.ProgressType);
            //Logger.Trace(e.FileBytesProcessed);
        }
    }
}