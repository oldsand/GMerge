using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Core.Extensions;
using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;

namespace GCommon.Differencing
{
    public class Difference<T>
    {
        public Difference(T left, T right, string propertyName = null, Type objectType = null)
        {
            Left = left;
            Right = right;
            PropertyType = typeof(T);
            PropertyName = propertyName;
            ObjectType = objectType ?? typeof(T);
        }

        public T Left { get; }
        public T Right { get; }
        public Type PropertyType { get; }
        public string PropertyName { get; }
        public Type ObjectType { get; }
        public DifferenceType DifferenceType => DetermineDifferenceType(Left, Right);

        public static IEnumerable<Difference<T>> Between(T left, T right) =>
            ComputeDifference(left, right, new GenericDiffer<T>());
        
        public static IEnumerable<Difference<T>> Between(T left, T right, IEqualityComparer<T> comparer) =>
            ComputeDifference(left, right, new GenericDiffer<T>(comparer));
        
        public static IEnumerable<Difference<T>> Between(T left, T right, IDifferentiator<T, T> differentiator) =>
            ComputeDifference(left, right, differentiator);
        
        public static IEnumerable<Difference<TDifference>> Between<TSource, TDifference>(TSource left, TSource right,
            IDifferentiator<TSource, TDifference> differentiator) => ComputeDifference(left, right, differentiator);
        
        public static IEnumerable<Difference<T>> Between<TSource>(TSource left, TSource right,
            Expression<Func<TSource, T>> propertyExpression) =>
            ComputeDifference(left, right, propertyExpression, EqualityComparer<T>.Default);
        
        public static IEnumerable<Difference<T>> Between<TSource>(TSource left, TSource right,
            Expression<Func<TSource, T>> propertyExpression, IEqualityComparer<T> comparer) =>
            ComputeDifference(left, right, propertyExpression, comparer);

        private static IEnumerable<Difference<TDifference>> ComputeDifference<TSource, TDifference>(
            TSource left, TSource right,  IDifferentiator<TSource, TDifference> differentiator)
        {
            return differentiator.DifferenceIn(left, right);
        }
        
        private static IEnumerable<Difference<TDifference>> ComputeDifference<TSource, TDifference>(TSource left,
            TSource right, Expression<Func<TSource, TDifference>> propertyExpression, IEqualityComparer<TDifference> comparer)
        {
            var differences = new List<Difference<TDifference>>();
            
            comparer ??= EqualityComparer<TDifference>.Default;
            
            var propertyName = GetMemberName(propertyExpression);
            
            var valueGetter = propertyExpression.Compile();
            var leftValue = valueGetter.Invoke(left);
            var rightValue = valueGetter.Invoke(right);

            if (!comparer.Equals(leftValue, rightValue))
            {
                differences.Add(new Difference<TDifference>(leftValue, rightValue, propertyName, typeof(TSource)));
            }
            
            return differences;
        }
        
        private static IEnumerable<Difference<TSource>> ComputeCollectionDifference<TSource, TValue>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            Func<TSource, TValue> key, 
            CollectionMatchMode mode, 
            ICollectionDifferentiator<TSource, TSource> differentiator)
        {
            return differentiator.DifferenceIn(left, right, key, mode);
        }

        private static string GetMemberName<TSource, TValue>(Expression<Func<TSource, TValue>> propertyExpression)
        {
            return propertyExpression.Body is MemberExpression memberExpression
                ? memberExpression.Member.Name
                : string.Empty;
        }

        private static DifferenceType DetermineDifferenceType(T left, T right)
        {
            return left == null && right != null ? DifferenceType.Added
                : left != null && right == null ? DifferenceType.Removed
                : DifferenceType.Changed;
        }
    }
}