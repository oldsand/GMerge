using System.Collections.Generic;

namespace GServer.Archestra.Abstractions
{
    public interface IGalaxyExporter
    {
        void ExportPkg(string tagName, string fileName);
        void ExportPkg(IEnumerable<string> tagNames, string fileName);
        void ExportCsv(string tagName, string fileName);
        void ExportCsv(IEnumerable<string> tagNames, string fileName);
        void ExportGraphic(string tagName, string destination);
        void ExportGraphic(IEnumerable<string> tagNames, string destination);
    }
}