using System;
using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class StringDiffer : IDifferentiator<string>
    {
        private readonly StringComparison _comparisonType;

        public StringDiffer(StringComparison comparisonType)
        {
            _comparisonType = comparisonType;
        }
        
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, _comparisonType);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }

        public IEnumerable<Difference> DifferenceIn(string first, string second)
        {
            var differences = new List<Difference>();
            
            if (!Equals(first, second))
            {
                differences.Add(Difference.Create(first, second, typeof(string)));
            }

            return differences;
        }
    }
}