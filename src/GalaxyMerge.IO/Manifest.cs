using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using GCommon.Core.Extensions;
using GalaxyMerge.IO.Abstractions;

[assembly: InternalsVisibleTo("GalaxyAccess.IO.IntegrationTests")]
namespace GalaxyMerge.IO
{
    public class Manifest : IManifest
    {
        private readonly XDocument _document;

        private Manifest(XDocument document)
        {
            _document = document;
        }

        public static IManifest Load(string fileName)
        {
            return new Manifest(XDocument.Load(fileName));
        }

        public static IManifest Parse(string text)
        {
            return new Manifest(XDocument.Parse(text));
        }

        public void Save(string destination)
        {
            var fileName = Path.Combine(destination, "Manifest.xml");
            _document.Save(fileName);
        }

        public static IManifest Create(string isaVersion, string cdiVersion)
        {
            var declaration = new XDeclaration("1.0", "UTF-8", "yes");
            var root = new XElement("archestra_pdf_content_version_2_2");
            
            var productVersion = new XElement("product_version",
                new XAttribute("iasversion", isaVersion), new XAttribute("cdiversion", cdiVersion));

            root.Add(productVersion);
            root.Add(new XElement("IODeviceMap", new XAttribute("filename", "IOBindingExport.xml")));
            root.Add(new XElement("TotalObjectCount", new XAttribute("objectcount", "0")));
            
            var document = new XDocument(declaration, root);
            return new Manifest(document);
        }

        public XElement Root => _document.Root;
        public XElement ProductVersion => _document.Root?.Element("product_version");
        public IEnumerable<XElement> Templates => _document.Descendants("template");
        public IEnumerable<XElement> Instances => _document.Descendants("instance");

        public XElement GetByTagName(string tagName)
        {
            var target = _document.Descendants().FirstOrDefault(t => t.Attribute("tag_name")?.Value == tagName);
            target?.RemoveNodes();
            return target;
        }

        public XElement GetByObjectId(int objectId)
        {
            var target = _document.Descendants()
                .FirstOrDefault(t => t.Attribute("gobjectid")?.Value == objectId.ToString());
            target?.RemoveNodes();
            return target;
        }

        public IEnumerable<XElement> GetByCodebase(string codeBase)
        {
            return _document.Descendants().Where(e => e.Attribute("codebase")?.Value == codeBase);
        }
        
        public IEnumerable<string> GetTextFileNames()
        {
            var fileNames = _document.Descendants()
                .Where(x => x.HasAttributes && x.Attributes("file_name").Any())
                .Attributes("file_name").Select(x => x.Value);
            return fileNames.Where(f => f.Contains(".txt"));
        }

        public string GetFileName(string tagName)
        {
            return _document.Descendants().FirstOrDefault(x => x.Attribute("tag_name")?.Value == tagName)
                ?.Attribute("file_name")?.Value.ToString();
        }

        public XElement GetObjectPath(string tagName)
        {
            var target = _document.Descendants().FirstOrDefault(x => x.Attribute("tag_name")?.Value == tagName);
            return target?.PathAndShallowSelf();
        }
    }
}