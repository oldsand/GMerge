using System.Collections.Generic;
using System.Text;

namespace GCommon.Core.Helpers
{
    public static class HexExtensions
    {
        public static string FormString(this IEnumerable<Hex> chars)
        {
            var builder = new StringBuilder();
            
            foreach (var c in chars)
                builder.Append(c.Reverse().ToChar());

            return builder.ToString();
        }
    }
}