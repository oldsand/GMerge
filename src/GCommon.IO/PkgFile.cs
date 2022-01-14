using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using GalaxyMerge.IO.Abstractions;
using GCommon.Utilities;

namespace GCommon.IO
{
    public class PkgFile : IPkgFile
    {
        private const string TempSubDirectory = @"GalaxyAccess\Pgk\";
        private const string ManifestXmlName = "Manifest.xml";
        private readonly IFileSystem _fileSystem;
        private readonly IFileInfo _file;

        public PkgFile(string fileName) : this(fileName, new FileSystem())
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global // We want internal for unit testing only.
        internal PkgFile(string fileName, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _file = _fileSystem.FileInfo.FromFileName(fileName);

            using var tempDirectory = new TempDirectory(TempSubDirectory);
            Pkg.Unpack(fileName, tempDirectory.FullName);

            var manifestPath = _fileSystem.Path.Combine(tempDirectory.FullName, ManifestXmlName);
            ManifestFile = Manifest.Load(manifestPath);

            TextFiles = new Dictionary<string, string>();

            foreach (var file in tempDirectory.GetFiles("*.txt"))
                TextFiles.Add(file.Name, _fileSystem.File.ReadAllText(file.FullName));
        }

        public string Name => _file.Name;
        public string Extension => _file.Extension;
        public long Size => _file.Length;
        public DateTime CreatedOn => _file.CreationTime;
        public DateTime UpdatedOn => _file.LastWriteTime;
        public IManifest ManifestFile { get; private set; }
        public Dictionary<string, string> TextFiles { get; private set; }

        public IGalaxyFile Load(string fileName)
        {
            return new PkgFile(fileName);
        }

        public void Save(string fileName)
        {
            using var tempDirectory = new TempDirectory(TempSubDirectory);
            ManifestFile.Save(tempDirectory.FullName);
            foreach (var textFile in TextFiles)
            {
                var path = _fileSystem.Path.Combine(tempDirectory.FullName, textFile.Key);
                _fileSystem.File.WriteAllText(path, textFile.Value);
            }
            Pkg.Pack(tempDirectory.FullName, fileName);
        }

        public byte[] GetBinaryData(string tagName)
        {
            var fileName = ManifestFile.GetFileName(tagName);
            var textData = !string.IsNullOrEmpty(fileName)
                ? TextFiles.SingleOrDefault(f => f.Key == fileName).Value
                : null;
            
            return Encoding.Default.GetBytes(textData ?? string.Empty);
        }

        public IEnumerable<byte[]> ReadAllObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}