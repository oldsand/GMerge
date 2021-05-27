using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GalaxyMerge.Archestra.Exceptions
{
    public class GalaxyException : Exception
    {
        public GalaxyException(string message) : base(message)
        {
        }
    }
}