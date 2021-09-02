using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Schema;
using GCommon.Core.Utilities;

[assembly: InternalsVisibleTo("GServer.Archestra.UnitTests")]

namespace GServer.Archestra.Helpers
{
    internal static class SchemaValidator
    {
        private const string SchemaNameSpace = "Schemas";
        private const string GraphicsSchemaFileName = "aaGraphics.xsd";
        private static readonly EmbeddedResources Resources = new EmbeddedResources(typeof(SchemaValidator));
        private static readonly XmlSchemaSet SchemaSet = new XmlSchemaSet();
        
        public static void ValidateSymbol(XDocument symbol)
        {
            //Only if not already loaded?
            LoadGraphicsSchema();
            
            symbol.Validate(SchemaSet, null);
        }

        private static void LoadGraphicsSchema()
        {
            using var stream = Resources.GetStream(GraphicsSchemaFileName, SchemaNameSpace);
            var schema = XmlSchema.Read(stream, null);
            SchemaSet.Add(schema);
        }
    }
}