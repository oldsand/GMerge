using System;
using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class WordDiffer : IDifferentiator<string, char?>
    {
        private readonly StringComparison _comparisonType;

        public WordDiffer(StringComparison comparisonType)
        {
            _comparisonType = comparisonType;
        }

        public IEnumerable<Difference<char?>> DifferenceIn(string first, string second, IEqualityComparer<string> comparer = null)
        {
            comparer ??= EqualityComparer<string>.Default;
            var differences = new List<Difference<char?>>();

            if (comparer.Equals(first, second))
                return differences;

            var length = Math.Max(first.Length, second.Length);

            for (var i = 0; i < length; i++)
            {
                char? c1 = i <= first.Length - 1 ? first[i] : null;
                char? c2 = i <= second.Length - 1 ? second[i] : null;

                if (!Equals(c1, c2))
                    differences.Add(Difference<char?>.Create(c1, c2, c => c));
            }

            return differences;
        }
    }
}