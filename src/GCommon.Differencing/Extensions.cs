using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;

namespace GCommon.Differencing
{
    public static class Extensions
    {
        public static IEnumerable<Difference<T>> DiffersFrom<T>(this T me, T other) =>
            GetDifferences(me, other, new GenericDiffer<T>());

        public static IEnumerable<Difference<T>> DiffersFrom<T>(this T me, T other,
            IDifferentiator<T, T> differentiator) => GetDifferences(me, other, differentiator);

        public static IEnumerable<Difference<TValue>> DiffersFrom<TSource, TValue>(this TSource me, TSource other,
            Expression<Func<TSource, TValue>> propertyExpression) =>
            GetDifferences(me, other, propertyExpression, EqualityComparer<TValue>.Default);

        public static IEnumerable<Difference<TValue>> DiffersFrom<TSource, TValue>(this TSource me, TSource other,
            Expression<Func<TSource, TValue>> propertyExpression, IEqualityComparer<TValue> comparer) =>
            GetDifferences(me, other, propertyExpression, comparer);

        public static IEnumerable<Difference<TSource>> CollectionDiffersFrom<TSource>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            GetCollectionDifferences(me, other, t => t, CollectionMatchMode.Join, new CollectionDiffer<TSource>());

        public static IEnumerable<Difference<TSource>> CollectionDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other, 
            Func<TSource, TValue> key) =>
            GetCollectionDifferences(me, other, key, CollectionMatchMode.Join, new CollectionDiffer<TSource>());

        public static IEnumerable<Difference<TSource>> CollectionDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> key,
            ICollectionDifferentiator<TSource, TSource> differentiator) =>
            GetCollectionDifferences(me, other, key, CollectionMatchMode.Join, differentiator);

        public static IEnumerable<Difference<TSource>> SequenceDiffersFrom<TSource>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            GetCollectionDifferences(me, other, t => t, CollectionMatchMode.Sort, new CollectionDiffer<TSource>());
        
        public static IEnumerable<Difference<TSource>> SequenceDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> key) =>
            GetCollectionDifferences(me, other, key, CollectionMatchMode.Sort, new CollectionDiffer<TSource>());

        public static IEnumerable<Difference<TSource>> SequenceDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> key,
            ICollectionDifferentiator<TSource, TSource> differentiator) =>
            GetCollectionDifferences(me, other, key, CollectionMatchMode.Sort, differentiator);

        private static IEnumerable<Difference<T>> GetDifferences<T>(this T me, T other,
            IDifferentiator<T, T> differentiator)
        {
            return differentiator.DifferenceIn(me, other);
        }

        private static IEnumerable<Difference<TValue>> GetDifferences<TSource, TValue>(this TSource me, TSource other,
            Expression<Func<TSource, TValue>> propertyExpression, IEqualityComparer<TValue> comparer)
        {
            return Difference<TValue>.Between(me, other, propertyExpression, comparer);
        }

        private static IEnumerable<Difference<TSource>> GetCollectionDifferences<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> key,
            CollectionMatchMode mode,
            ICollectionDifferentiator<TSource, TSource> differentiator)
        {
            return differentiator.DifferenceIn(me, other, key, mode);
        }
    }
}