using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace GCommon.Core.Utilities
{
    public class TempDirectory : IDisposable
    {
        private readonly string _subDirectory;
        private readonly IFileSystem _fileSystem;
        private IDirectoryInfo _directory;

        public TempDirectory(string subDirectory = null) : this(new FileSystem(), subDirectory)
        {
        }

        internal TempDirectory(IFileSystem fileSystem, string subDirectory = null)
        {
            _fileSystem = fileSystem;
            _subDirectory = subDirectory;
            Initialize();
        }

        public string FullName => _directory.FullName;
        public string Parent => _directory.Parent.Name;

        public void Dispose()
        {
            _directory.Delete(true); 
        }

        public IEnumerable<IFileInfo> GetFiles()
        {
            return _directory.GetFiles();
        }
        
        public IEnumerable<IFileInfo> GetFiles(string searchPattern)
        {
            return _directory.GetFiles(searchPattern);
        }

        private void Initialize()
        {
            _directory = _fileSystem.DirectoryInfo.FromDirectoryName(CalculatePath());
            
            if (_directory.Exists)
                _directory.Delete();
            
            _directory.Create();
        }

        private string CalculatePath()
        {
            return !string.IsNullOrEmpty(_subDirectory)
                ? _fileSystem.Path.Combine(_fileSystem.Path.GetTempPath(), _subDirectory, Guid.NewGuid().ToString())
                : _fileSystem.Path.Combine(_fileSystem.Path.GetTempPath(), Guid.NewGuid().ToString());
        }
    }
}