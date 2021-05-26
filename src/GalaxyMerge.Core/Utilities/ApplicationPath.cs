using System;
using System.IO;

namespace GalaxyMerge.Core.Utilities
{
    public static class ApplicationPath
    {
        private const string PrimaryName = "GalaxyMerge";

        public static readonly string TempExportSubPath = Path.Combine(PrimaryName, @"Exports");
        public static readonly string TempPkgSubPath = Path.Combine(PrimaryName, @"Pkgs");
        public static readonly string TempSymbolSubPath = Path.Combine(PrimaryName, @"Symbols");

        public static readonly string ProgramData =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PrimaryName);

        public static readonly string Archives = Path.Combine(ProgramData, "Archives");
        public static readonly string Logging = Path.Combine(ProgramData, "Logging");
    }
}