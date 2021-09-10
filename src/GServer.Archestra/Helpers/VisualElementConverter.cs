using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ArchestrA.Visualization.GraphicLibrary;
using GCommon.Primitives.Structs;
using GServer.Archestra.Extensions;

[assembly: InternalsVisibleTo("GServer.Archestra.IntegrationTests")]

namespace GServer.Archestra.Helpers
{
    internal static class VisualElementConverter
    {
        public static XDocument Convert(Blob definition)
        {
            var container = new aaSymbolGraphicContainer();
            container.Load(definition.Data);

            return container.GenerateXml();
        }
    }
}