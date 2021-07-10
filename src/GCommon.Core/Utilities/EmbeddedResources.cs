using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GCommon.Core.Utilities
{
    public class EmbeddedResources
    {
        private readonly Assembly _assembly;

        public EmbeddedResources(Type type)
        {
            _assembly = type.Assembly;

            if (_assembly == null)
                throw new InvalidOperationException("Assembly Type Not Initialized");
        }

        public List<string> Names => _assembly.GetManifestResourceNames().ToList();
        public string AssemblyName => _assembly.FullName?.Substring(0, _assembly.FullName.IndexOf(','));

        public Stream GetStream(string fileName, string nameSpace)
        {
            return _assembly.GetManifestResourceStream($"{AssemblyName}.{nameSpace}.{fileName}");
        }

        public string ReadFile(string fileName, string nameSpace)
        {
            var resourceStream = _assembly.GetManifestResourceStream($"{AssemblyName}.{nameSpace}.{fileName}");
            if (resourceStream == null) return "";
            var resourceText = new StreamReader(resourceStream).ReadToEnd();
            return resourceText;
        }
    }
}