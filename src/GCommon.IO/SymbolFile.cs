using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Xml.Linq;
using GalaxyMerge.IO.Abstractions;
using GCommon.Extensions;

namespace GCommon.IO
{
    public class SymbolFile : ISymbolFile
    {
        private readonly IFileSystem _fileSystem;
        private readonly XDocument _document;
        private readonly IFileInfo _file;

        public SymbolFile(string fileName) : this(fileName, new FileSystem())
        {
        }

        public SymbolFile(string fileName, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _file = fileSystem.FileInfo.FromFileName(fileName);
            _document = XDocument.Load(fileName);
        }

        public string Name => _file.Name;
        public string Extension => _file.Extension;
        public long Size => _file.Length;
        public DateTime CreatedOn => _file.CreationTime;
        public DateTime UpdatedOn => _file.LastWriteTime;
        public XElement Root => _document.Root;
        public IEnumerable<XElement> CustomProperties => _document.Descendants("CustomProperty");
        public IEnumerable<XElement> WizardOptions => _document.Descendants("WizardOptions");
        public XElement VisualTree => _document.Root?.Element("GraphicElements");
        public IEnumerable<XElement> Scripts => _document.Descendants("NamedScripts");
        
        public IGalaxyFile Load(string fileName)
        {
            return new SymbolFile(fileName);
        }

        public void Save(string fileName)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBinaryData(string tagName)
        {
            return _document.Root.ToByteArray();
        }

        public IEnumerable<string> GetEmbeddedSymbols()
        {
            throw new NotImplementedException();
        }
    }
}