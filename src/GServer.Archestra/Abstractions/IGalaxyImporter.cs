namespace GServer.Archestra.Abstractions
{
    public interface IGalaxyImporter
    {
        void ImportPkg(string fileName, bool overwrite);
        void ImportCsv(string fileName);
        void ImportGraphic(string fileName, string tagName, bool overwrite);
    }
}