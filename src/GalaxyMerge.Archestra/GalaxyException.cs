using System;
using System.Collections.Generic;
using ArchestrA.GRAccess;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GalaxyMerge.Archestra
{
    public class GalaxyException : Exception
    {
        public List<GalaxyCommandResult> Results { get; }

        public GalaxyException(string message, IEnumerable<GalaxyCommandResult> results) : base(message)
        {
            Results = new List<GalaxyCommandResult>(results);
        }
        
        public GalaxyException(string message, GalaxyCommandResult result) : base(message)
        {
            Results = new List<GalaxyCommandResult> {result};
        }
    }
}