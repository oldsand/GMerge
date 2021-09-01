using System.Collections.Generic;
using ArchestrA.GRAccess;
using ArchestrA.Visualization.GraphicAccess;
using GServer.Archestra.Abstractions;
using GServer.Archestra.Extensions;

namespace GServer.Archestra
{
    public class GalaxyFileManager : IGalaxyExporter, IGalaxyImporter
    {
        private readonly IGalaxy _galaxy;
        private readonly GraphicAccess _graphicAccess;

        public GalaxyFileManager(IGalaxyRepository galaxyRepository)
        {
            var gr = (GalaxyRepository) galaxyRepository;
            _galaxy = gr.Galaxy;
            _graphicAccess = new GraphicAccess();
        }
        
        public void ExportPkg(string tagName, string fileName)
        {
            _galaxy.SynchronizeClient();

            var collection = _galaxy.CreategObjectCollection();
            var item = _galaxy.GetObjectByName(tagName);
            collection.Add(item);

            collection.ExportObjects(EExportType.exportAsPDF, fileName);
            collection.CommandResults.Process();
        }

        public void ExportPkg(IEnumerable<string> tagNames, string fileName)
        {
            _galaxy.SynchronizeClient();

            var collection = _galaxy.GetObjectsByName(tagNames);
            collection.ExportObjects(EExportType.exportAsPDF, fileName);
            collection.CommandResults.Process();
        }

        public void ExportCsv(string tagName, string fileName)
        {
            _galaxy.SynchronizeClient();

            var collection = _galaxy.CreategObjectCollection();
            var item = _galaxy.GetObjectByName(tagName);
            collection.Add(item);

            collection.ExportObjects(EExportType.exportAsCSV, fileName);
            collection.CommandResults.Process();
        }

        public void ExportCsv(IEnumerable<string> tagNames, string fileName)
        {
            _galaxy.SynchronizeClient();

            var collection = _galaxy.GetObjectsByName(tagNames);
            collection.ExportObjects(EExportType.exportAsCSV, fileName);
            collection.CommandResults.Process();
        }

        public void ExportGraphic(string tagName, string fileName)
        {
            _galaxy.SynchronizeClient();

            var result = _graphicAccess.ExportGraphicToXml(_galaxy, tagName, fileName);
            result.Process();
        }

        public void ImportPkg(string fileName, bool overwrite)
        {
            _galaxy.SynchronizeClient();

            _galaxy.ImportObjects(fileName, overwrite);
            _galaxy.CommandResults.Process();
        }

        public void ImportCsv(string fileName)
        {
            _galaxy.SynchronizeClient();

            _galaxy.GRLoad(fileName, GRLoadMode.GRLoadModeUpdate);
            _galaxy.CommandResults.Process();
        }

        public void ImportGraphic(string fileName, string tagName, bool overwrite)
        {
            _galaxy.SynchronizeClient();

            var result = _graphicAccess.ImportGraphicFromXml(_galaxy, tagName, fileName, overwrite);
            result.Process();
        }
    }
}