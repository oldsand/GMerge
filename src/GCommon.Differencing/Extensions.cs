using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Core.Extensions;
using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;

namespace GCommon.Differencing
{
    public static class Extensions
    {
        public static IEnumerable<Difference> DiffersFrom<T>(this T me, T other) =>
            GetDifferences(me, other, new GenericDiffer<T>());

        public static IEnumerable<Difference> DiffersFrom<T>(this T me, T other,
            IDifferentiator<T> differentiator) => GetDifferences(me, other, differentiator);

        public static IEnumerable<Difference> CollectionDiffersFrom<TSource>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            JoinedCollectionDifferences(me, other, x => x, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> CollectionDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other, 
            Func<TSource, TValue> joinKey) =>
            JoinedCollectionDifferences(me, other, joinKey, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> CollectionDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> joinKey,
            IDifferentiator<TSource> differentiator) =>
            JoinedCollectionDifferences(me, other, joinKey, differentiator);

        public static IEnumerable<Difference> SequenceDiffersFrom<TSource>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            SequenceDifferences(me, other, new GenericDiffer<TSource>());
        
        public static IEnumerable<Difference> SequenceDiffersFrom<TSource, TReturn>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            IDifferentiator<TSource> differentiator) =>
            SequenceDifferences(me, other, differentiator);
        
        public static IEnumerable<Difference> SequenceDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> sortKey) =>
            SortedSequenceDifferences(me, other, sortKey, new GenericDiffer<TSource>());
        
        public static IEnumerable<Difference> SequenceDiffersFrom<TSource, TValue, TReturn>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> sortKey,
            IDifferentiator<TSource> differentiator) =>
            SortedSequenceDifferences(me, other, sortKey, differentiator);

        private static IEnumerable<Difference> JoinedCollectionDifferences<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> joinKey,
            IDifferentiator<TSource> differentiator)
        {
            var joined = me.FullOuterJoin(other, joinKey, joinKey, (first, second) => new
            {
                First = first,
                Second = second
            });

            return joined.SelectMany(x => GetDifferences(x.First, x.Second, differentiator));
        }
        
        private static IEnumerable<Difference> SortedSequenceDifferences<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> sortKey,
            IDifferentiator<TSource> differentiator)
        {
            var sortedLeft = me.OrderBy(sortKey);
            var sortedRight = other.OrderBy(sortKey);

            return SequenceDifferences(sortedLeft, sortedRight, differentiator);
        }

        private static IEnumerable<Difference> SequenceDifferences<TSource>(
            this IEnumerable<TSource> me, 
            IEnumerable<TSource> other,
            IDifferentiator<TSource> differentiator)
        {
            var differences = new List<Difference>();
            
            using var first = me.GetEnumerator();
            using var second = other.GetEnumerator();

            while (first.MoveNext())
            {
                if (second.MoveNext())
                {
                    if (first.Current != null)
                    {
                        differences.AddRange(GetDifferences(first.Current, second.Current, differentiator));
                    }
                }
                else
                {
                    differences.Add(Difference.Create(first.Current, default));
                }
            }

            while (second.MoveNext())
                differences.Add(Difference.Create(default, second.Current));

            return differences;
        }

        private static IEnumerable<Difference> GetDifferences<T>(T me, T other, IDifferentiator<T> differentiator)
        {
            return Difference.Between(me, other, differentiator);
        }
    }
}