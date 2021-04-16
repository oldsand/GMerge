using System.Collections.Generic;
using System.Xml.Linq;

namespace GalaxyMerge.IO.Abstractions
{
    public interface IManifest
    {
        XElement Root { get; }
        XElement ProductVersion { get; }
        IEnumerable<XElement> Templates { get; }
        IEnumerable<XElement> Instances { get; }
        XElement GetByTagName(string tagName);
        XElement GetByObjectId(int objectId);
        IEnumerable<string> GetTextFileNames();
        IEnumerable<XElement> GetByCodebase(string codeBase);
        string GetFileName(string tagName);
        XElement GetObjectPath(string tagName);
        void Save(string destination);
    }
}