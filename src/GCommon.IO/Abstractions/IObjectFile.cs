using System.Collections.Generic;
using System.Xml.Linq;

namespace GalaxyMerge.IO.Abstractions
{
    public interface IObjectFile : IGalaxyFile
    {
        XElement Root { get; }
        XElement Configuration { get; }
        IEnumerable<XElement> Dependencies { get; }
        void SetGalaxyInfo(XElement element);
        void SetObjectInfo(XElement element);
        void AddAttribute(XElement element);
        void RemoveAttribute(string name);
        void UpdateAttribute(XElement element);
        void AddDependency(XElement element);
        void RemoveDependency(string tagName);
        void UpdateDependency(XElement element);
    }
}