using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using GCommon.Core.Utilities;
using GCommon.Primitives.Enumerations;

[assembly: InternalsVisibleTo("GCommon.Primitives.UnitTests")]

namespace GCommon.Primitives.Helpers
{
    internal static class PrimitiveLoader
    {
        private const string NameSpace = "Resources";
        private const string FileName = "TemplatePrimitives.xml";
        private const string TemplateElement = "Template";
        private const string NameElement = "Name";
        private const string DataTypeElement = "DataType";
        private const string CategoryElement = "Category";
        private const string SecurityElement = "Security";
        private const string LockedElement = "Locked";
        private const string ValueElement = "Value";
        private static readonly EmbeddedResources Resources = new EmbeddedResources(typeof(PrimitiveLoader));

        public static IEnumerable<ArchestraAttribute> Load(Template template)
        {
            using var stream = Resources.GetStream(FileName, NameSpace);
            if (stream == null) return Enumerable.Empty<ArchestraAttribute>(); //todo or throw exception?

            var rows = XDocument.Load(stream).Descendants("row")
                .Where(e => e.Element(TemplateElement)?.Value == template.Name);

            return (from row in rows
                let name = row.Element(NameElement)?.Value
                let dataType = DataType.FromValue(Convert.ToInt32(row.Element(DataTypeElement)?.Value))
                let category = AttributeCategory.FromValue(Convert.ToInt32(row.Element(CategoryElement)?.Value))
                let security = SecurityClassification.FromValue(Convert.ToInt32(row.Element(SecurityElement)?.Value))
                let locked = Convert.ToBoolean(row.Element(LockedElement)?.Value) ? LockType.InMe : LockType.Unlocked
                let parser = new HexParser(row.Element(ValueElement)?.Value)
                let value = dataType.Parse((Hex)row.Element(ValueElement)?.Value)
                let arrayCount = parser.ArrayLength
                select new ArchestraAttribute(name, dataType, category, security, locked, value, arrayCount)).ToList();
        }
    }
}