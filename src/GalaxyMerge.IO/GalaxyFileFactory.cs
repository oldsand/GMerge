using System.IO.Abstractions;
using GalaxyMerge.IO.Abstractions;

namespace GalaxyMerge.IO
{
    public class GalaxyFileFactory : IGalaxyFileFactory
    {
        private readonly IFileSystem _fileSystem;

        public GalaxyFileFactory() : this(new FileSystem())
        {
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        internal GalaxyFileFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IGalaxyFile FromFile(string fileName)
        {
            var fileInfo = _fileSystem.FileInfo.FromFileName(fileName);
            
            //TODO: Probably do file validation here right?
            if (fileInfo.Extension == "aaPKG")
                return new PkgFile(fileName);
            
            if (fileInfo.Extension == "xml")
                return new SymbolFile(fileName);

            return null;
        }
    }
}