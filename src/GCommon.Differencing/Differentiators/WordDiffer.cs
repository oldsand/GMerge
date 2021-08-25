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

        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, _comparisonType);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }

        public IEnumerable<Difference<char?>> DifferenceIn(string first, string second)
        {
            var differences = new List<Difference<char?>>();

            if (Equals(first, second))
                return differences;

            var length = Math.Max(first.Length, second.Length);

            for (var i = 0; i < length; i++)
            {
                char? c1 = i <= first.Length - 1 ? first[i] : null;
                char? c2 = i <= second.Length - 1 ? second[i] : null;

                if (!Equals(c1, c2))
                    differences.Add(new Difference<char?>(c1, c2));
            }

            return differences;
        }

        public IEnumerable<Difference<char?>> DifferenceIn(IEnumerable<string> first, IEnumerable<string> second)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Levenshtein distance between 2 strings. 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public int Distance(string first, string second) =>
            Distance(first, first.Length, second, second.Length);

        private static int Distance(string text1, int len1, string text2, int len2)
        {
            if (len1 == 0)
            {
                return len2;
            }

            if (len2 == 0)
            {
                return len1;
            }

            var substitutionCost = 0;

            if (text1[len1 - 1] != text2[len2 - 1])
            {
                substitutionCost = 1;
            }

            var deletion = Distance(text1, len1 - 1, text2, len2) + 1;
            var insertion = Distance(text1, len1, text2, len2 - 1) + 1;
            var substitution = Distance(text1, len1 - 1, text2, len2 - 1) + substitutionCost;

            return Minimum(deletion, insertion, substitution);
        }

        private static int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;
    }
}