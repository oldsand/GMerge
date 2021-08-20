using System;
using System.Collections.Generic;
using GCommon.Core.Extensions;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing
{
    public static class Extensions
    {
        public static IEnumerable<Difference> DiffersFrom<T>(this T me, T other) =>
            GetDifferences(me, other, EqualityComparer<T>.Default);

        public static IEnumerable<Difference> DiffersFrom<T>(this T me, T other, IEqualityComparer<T> comparer) =>
            GetDifferences(me, other, comparer);

        public static IEnumerable<Difference> SequenceDiffersBy<TSource>(this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            GetSequentialDifferences(me, other, s => s, EqualityComparer<TSource>.Default);
        
        public static IEnumerable<Difference> SequenceDiffersBy<TSource, TValue>(this IEnumerable<TSource> me, 
            IEnumerable<TSource> other, Func<TSource, TValue> sortKey) =>
            GetSequentialDifferences(me, other, sortKey, EqualityComparer<TSource>.Default);
        
        public static IEnumerable<Difference> SequenceDiffersBy<TSource, TValue>(this IEnumerable<TSource> me, 
            IEnumerable<TSource> other, Func<TSource, TValue> sortKey, IEqualityComparer<TSource> comparer) =>
            GetSequentialDifferences(me, other, sortKey, comparer);
        
        private static IEnumerable<Difference> GetDifferences<T>(this T me, T other, IEqualityComparer<T> comparer)
        {
            if (me is IDifferentiable<T> differentiable)
            {
                return differentiable.DiffersFrom(other);
            }
            
            var differences = new List<Difference>();
            
            if (!comparer.Equals(me, other))
            {
                differences.Add(Difference.Create(me, other, typeof(T)));
            }

            return differences;
        }
        
        private static IEnumerable<Difference> GetSequentialDifferences<TSource, TValue>(this IEnumerable<TSource> me,
            IEnumerable<TSource> other, Func<TSource, TValue> key, IEqualityComparer<TSource> comparer)
        {
            var differences = new List<Difference>();

            var joined = me.FullOuterJoin(other, key, key, (first, second) => new
            {
                //If the join produced null for the 'first' object, used the second in place to avoid null reference exception
                First = first != null ? first : second,
                Second = first != null ? second : default
            });

            foreach (var pair in joined)
            {
                differences.AddRange(pair.First.GetDifferences(pair.Second, comparer));
            }

            return differences;
        }
    }
}