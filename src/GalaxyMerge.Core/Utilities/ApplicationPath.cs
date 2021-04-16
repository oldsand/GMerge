using System;
using System.IO;

namespace GalaxyMerge.Core.Utilities
{
    public static class ApplicationPath
    {
        private const string PrimaryName = "GalaxyMerge";

        public static readonly string TempExport = Path.Combine(PrimaryName, @"Exports");
        public static readonly string TempPkg = Path.Combine(PrimaryName, @"Pkgs");
        public static readonly string TempSymbol = Path.Combine(PrimaryName, @"Symbols");

        public static readonly string ProgramData =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PrimaryName);

        public static readonly string Archives = Path.Combine(ProgramData, "Archives");
    }
}