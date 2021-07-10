using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using GCommon.Core.Extensions;
using GCommon.Core.Utilities;
using GalaxyMerge.IO.Abstractions;

namespace GalaxyMerge.IO
{
    public class ObjectFile : IObjectFile
    {
        private const string ObjectSchemaFileName = "ObjectFile.xsd";
        private const string SchemaFileNameSpace = "Schemas";
        private static readonly EmbeddedResources Resources = new EmbeddedResources(typeof(ObjectFile));
        private readonly IFileSystem _fileSystem;
        private XDocument _document;
        private IFileInfo _fileInfo;
        private readonly XmlSchemaSet _schemaSet;

        private ObjectFile(XDocument document) : this(document, new FileSystem())
        {
        }

        private ObjectFile(string fileName) : this(fileName, new FileSystem())
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global // We want internal for unit testing only.
        internal ObjectFile(string fileName, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _fileInfo = fileSystem.FileInfo.FromFileName(fileName);
            _document = XDocument.Load(fileName);

            using var schemaStream = Resources.GetStream(ObjectSchemaFileName, SchemaFileNameSpace);
            var schema = XmlSchema.Read(schemaStream, null);
            _schemaSet = new XmlSchemaSet();
            _schemaSet.Add(schema);
        }

        // ReSharper disable once MemberCanBePrivate.Global // We want internal for unit testing only.
        internal ObjectFile(XDocument document, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _document = document;

            using var schemaStream = Resources.GetStream(ObjectSchemaFileName, SchemaFileNameSpace);
            var schema = XmlSchema.Read(schemaStream, null);
            _schemaSet = new XmlSchemaSet();
            _schemaSet.Add(schema);
        }

        public string Name => _fileInfo.Name;
        public string Extension => _fileInfo.Extension;
        public long Size => _fileInfo.Length;
        public DateTime CreatedOn => _fileInfo.CreationTime;
        public DateTime UpdatedOn => _fileInfo.LastWriteTime;
        public XElement Root => _document.Root;
        public XElement Configuration => _document.Root?.Element("Configuration");
        public IEnumerable<XElement> Dependencies => _document.Descendants("Dependency");
        
        public static IObjectFile Create(string galaxyName, string isaVersion, string cdiVersion)
        {
            var document = Empty();
            document.Root?.Add(new XAttribute("OriginatingGalaxy", galaxyName));
            document.Root?.Add(new XAttribute("IsaVersion", isaVersion));
            document.Root?.Add(new XAttribute("CdiVersion", cdiVersion));

            using var schemaStream = Resources.GetStream(ObjectSchemaFileName, SchemaFileNameSpace);
            var schema = XmlSchema.Read(schemaStream, null);
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);
            document.Validate(schemaSet, null);

            return new ObjectFile(document);
        }

        private static XDocument Empty()
        {
            var declaration = new XDeclaration("1.0", "UTF-8", "yes");
            var root = new XElement("ObjectFile");
            root.Add(new XElement("Configuration"));
            root.Add(new XElement("Dependencies"));
            var document = new XDocument(declaration, root);
            return document;
        }

        public IGalaxyFile Load(string fileName)
        {
            return new ObjectFile(fileName);
        }

        public void Save(string fileName)
        {
            _document.Save(fileName);
            _fileInfo = _fileSystem.FileInfo.FromFileName(fileName);
        }

        public byte[] GetBinaryData(string tagName = null)
        {
            return _document.Root.ToByteArray();
        }

        public static XElement NewAttribute(string name, string dataType, string category, string security, bool locked,
            string description, string engUnits, object value)
        {
            var element = new XElement("Attribute");
            element.Add(new XAttribute("Name", name));
            element.Add(new XAttribute("DataType", dataType));
            element.Add(new XAttribute("Category", category));
            element.Add(new XAttribute("Security", security));
            element.Add(new XAttribute("Locked", locked));
            
            element.Add(new XElement("Description") {Value = description});
            element.Add(new XElement("EngUnits") {Value = engUnits});

            var content = new XElement("Value");
            content.Add(new XCData(value.ToString()));
            element.Add(content);

            return element;
        }

        public void SetGalaxyInfo(XElement element)
        {
            var document = new XDocument(_document);
            document.Root?.Element("GalaxyInfo")?.ReplaceWith(element);
            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void SetObjectInfo(XElement element)
        {
            var document = new XDocument(_document);
            document.Root?.Element("ObjectInfo")?.ReplaceWith(element);
            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void AddAttribute(XElement element)
        {
            var document = new XDocument(_document);
            document.Root?.Element("Attributes")?.Add(element);
            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void RemoveAttribute(string name)
        {
            var document = new XDocument(_document);
            document.Descendants("Attribute").SingleOrDefault(a => a.Attribute("Name")?.Value == name)?.RemoveAll();
            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void UpdateAttribute(XElement element)
        {
            var document = new XDocument(_document);
            var attribute = document.Descendants("Attribute")
                .SingleOrDefault(a => a.Attribute("Name")?.Value == element.Attribute("Name")?.Value);

            if (attribute == null)
                AddAttribute(element);
            else
                attribute.ReplaceWith(element);

            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void AddDependency(XElement element)
        {
            var document = new XDocument(_document);
            document.Root?.Element("Dependencies")?.Add(element);
            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void RemoveDependency(string tagName)
        {
            var document = new XDocument(_document);
            document.Descendants("Dependency")
                .SingleOrDefault(a => a.Attribute("TagName")?.Value == tagName)?.RemoveAll();
            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void UpdateDependency(XElement element)
        {
            var document = new XDocument(_document);
            var dependency = document.Descendants("Dependency")
                .SingleOrDefault(a => a.Attribute("ObjectId")?.Value == element.Attribute("ObjectId")?.Value);

            if (dependency == null)
                AddDependency(element);
            else
                dependency.ReplaceWith(element);

            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void SetManifestConfig(XElement element)
        {
            var document = new XDocument(_document);
            document.Root?.Element("ManifestConfig")?.ReplaceWith(element);
            document.Validate(_schemaSet, null);
            _document = document;
        }

        public void SetFileData(XElement element)
        {
            var document = new XDocument(_document);
            document.Root?.Element("FileData")?.ReplaceWith(element);
            document.Validate(_schemaSet, null);
            _document = document;
        }
    }
}