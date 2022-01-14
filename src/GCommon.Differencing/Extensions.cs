using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;

namespace GCommon.Differencing
{
    public static class Extensions
    {
        public static IEnumerable<Difference> DiffersFrom<T>(this T me, T other) => Difference.Between(me, other);

        public static IEnumerable<Difference> DiffersFrom<T>(this T me, T other, string propertyName, Type objectType)
            => Difference.Between(me, other, propertyName, objectType);

        public static IEnumerable<Difference> DiffersFrom<T>(this T me, T other,
            IDifferentiator<T> differentiator) => Difference.Between(me, other, differentiator);
        
        public static IEnumerable<Difference> DiffersFrom<TSource, TValue>(this TSource me, TSource other,
            Expression<Func<TSource, TValue>> propertySelector) => Difference.Between(me, other, propertySelector);
        
        public static IEnumerable<Difference> SequenceDiffersFrom<TSource>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            Difference.BetweenSequence(me, other, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> SequenceDiffersFrom<TSource>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            IDifferentiator<TSource> differentiator) =>
            Difference.BetweenSequence(me, other, differentiator);

        public static IEnumerable<Difference> SequenceDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> sortKey) =>
            Difference.BetweenSequence(me, other, sortKey, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> SequenceDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> sortKey,
            IDifferentiator<TSource> differentiator) =>
            Difference.BetweenSequence(me, other, sortKey, differentiator);

        public static IEnumerable<Difference> CollectionDiffersFrom<TSource>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other) =>
            Difference.BetweenCollection(me, other, x => x, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> CollectionDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> joinKey) =>
            Difference.BetweenCollection(me, other, joinKey, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> CollectionDiffersFrom<TSource, TValue>(
            this IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> joinKey,
            IDifferentiator<TSource> differentiator) =>
            Difference.BetweenCollection(me, other, joinKey, differentiator);

        
    }
}