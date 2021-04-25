using System.Xml.Linq;
using System.Xml.Schema;
using GalaxyMerge.Core.Utilities;

namespace GalaxyMerge.Archestra
{
    internal static class SchemaValidator
    {
        private const string SchemaNameSpace = "Schemas";
        private const string GraphicsSchemaFileName = "aaGraphics.xsd";
        private static readonly EmbeddedResources Resources = new(typeof(SchemaValidator));
        private static readonly XmlSchemaSet SchemaSet = new();
        
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