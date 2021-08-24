using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Core.Extensions;

namespace GCommon.Differencing
{
    public static class Extensions
    {
        public static IEnumerable<Difference<T>> DiffersFrom<T>(this T me, T other) =>
            GetDifferences(me, other, EqualityComparer<T>.Default);

        public static IEnumerable<Difference<T>> DiffersFrom<T>(this T me, T other, IEqualityComparer<T> comparer) =>
            GetDifferences(me, other, comparer);
        
        public static IEnumerable<Difference<TValue>> DiffersFrom<TSource, TValue>(this TSource me, TSource other,
            Expression<Func<TSource, TValue>> propertyExpression) =>
            GetDifferences(me, other, propertyExpression, EqualityComparer<TValue>.Default);
        
        public static IEnumerable<Difference<TValue>> DiffersFrom<TSource, TValue>(this TSource me, TSource other,
            Expression<Func<TSource, TValue>> propertyExpression, IEqualityComparer<TValue> comparer) =>
            GetDifferences(me, other, propertyExpression, comparer);

        public static IEnumerable<Difference<TSource>> CollectionDiffersFrom<TSource>(this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            GetCollectionDifferences(me, other, s => s, EqualityComparer<TSource>.Default);
        
        public static IEnumerable<Difference<TSource>> CollectionDiffersFrom<TSource, TValue>(this IEnumerable<TSource> me, 
            IEnumerable<TSource> other, Func<TSource, TValue> sortKey) =>
            GetCollectionDifferences(me, other, sortKey, EqualityComparer<TSource>.Default);
        
        public static IEnumerable<Difference<TSource>> CollectionDiffersFrom<TSource, TValue>(this IEnumerable<TSource> me, 
            IEnumerable<TSource> other, Func<TSource, TValue> sortKey, IEqualityComparer<TSource> comparer) =>
            GetCollectionDifferences(me, other, sortKey, comparer);
        
        public static IEnumerable<Difference<TSource>> SequenceDiffersFrom<TSource>(this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            GetSequenceDifferences(me, other, EqualityComparer<TSource>.Default);
        
        public static IEnumerable<Difference<TSource>> SequenceDiffersFrom<TSource>(this IEnumerable<TSource> me,
            IEnumerable<TSource> other, IEqualityComparer<TSource> comparer) =>
            GetSequenceDifferences(me, other, comparer);
        
        private static IEnumerable<Difference<T>> GetDifferences<T>(this T me, T other, IEqualityComparer<T> comparer)
        {
            return Difference<T>.Between(me, other, comparer);
        }
        
        private static IEnumerable<Difference<TValue>> GetDifferences<TSource, TValue>(this TSource me, TSource other,
            Expression<Func<TSource, TValue>> propertyExpression, IEqualityComparer<TValue> comparer)
        {
            return Difference<TValue>.Between(me, other, propertyExpression, comparer);
        }
        
        private static IEnumerable<Difference<TSource>> GetCollectionDifferences<TSource, TValue>(this IEnumerable<TSource> me,
            IEnumerable<TSource> other, Func<TSource, TValue> key, IEqualityComparer<TSource> comparer)
        {
            var differences = new List<Difference<TSource>>();

            var joined = me.FullOuterJoin(other, key, key, (first, second) => new
            {
                //If the join produced null for the 'first' object, used the second in place to avoid null reference exception
                First = first != null ? first : second,
                Second = first != null ? second : default
            });

            foreach (var pair in joined)
            {
                differences.AddRange(Difference<TSource>.Between(pair.First, pair.Second, comparer));
            }

            return differences;
        }
        
        private static IEnumerable<Difference<TSource>> GetSequenceDifferences<TSource>(this IEnumerable<TSource> me,
            IEnumerable<TSource> other, IEqualityComparer<TSource> comparer)
        {
            var differences = new List<Difference<TSource>>();

            using var e1 = me.GetEnumerator();
            using var e2 = other.GetEnumerator();

            while (e1.MoveNext())
            {
                if (e2.MoveNext())
                {
                    differences.AddRange(e1.Current.GetDifferences(e2.Current, comparer));
                }
                else
                {
                    differences.Add(new Difference<TSource>(e1.Current));
                }
            }

            while (e2.MoveNext())
            {
                differences.Add(new Difference<TSource>(e2.Current));
            }

            return differences;
        }
    }
}