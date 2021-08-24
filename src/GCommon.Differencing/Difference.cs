using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Differencing.Abstractions;

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
            ObjectType = objectType;
        }
        
        public Difference(T left, string propertyName = null, Type objectType = null)
        {
            Left = left;
            Right = default;
            PropertyType = typeof(T);
            PropertyName = propertyName;
            ObjectType = objectType;
        }

        public T Left { get; }
        public T Right { get; }
        public Type PropertyType { get; }
        public string PropertyName { get; }
        public Type ObjectType { get; }
        public DifferenceType DifferenceType
        {
            get
            {
                if (Left != null && Right == null)
                    return DifferenceType.Removed;
                
                if (Left == null && Right != null)
                    return DifferenceType.Added;
                
                return DifferenceType.Changed;
            }
            
        }
        
        public static Difference<T> Create<TSource>(TSource left, TSource right,
            Expression<Func<TSource, T>> propertyExpression)
        {
            var propertyName = GetMemberName(propertyExpression);

            var valueGetter = propertyExpression.Compile();
            
            var leftValue = valueGetter.Invoke(left);
            var rightValue = valueGetter.Invoke(right);

            return new Difference<T>(leftValue, rightValue, propertyName, typeof(TSource));
        }
        
        public static IEnumerable<Difference<T>> Between(T left, T right, IEqualityComparer<T> comparer) =>
            ComputeDifference(left, right, comparer);

        public static IEnumerable<Difference<T>> Between<TSource>(TSource left, TSource right,
            Expression<Func<TSource, T>> propertyExpression) =>
            ComputeDifference(left, right, propertyExpression, EqualityComparer<T>.Default);
        
        public static IEnumerable<Difference<T>> Between<TSource>(TSource left, TSource right,
            Expression<Func<TSource, T>> propertyExpression, IEqualityComparer<T> comparer) =>
            ComputeDifference(left, right, propertyExpression, comparer);

        public static IEnumerable<Difference<T>> Between(T left, T right) =>
            ComputeDifference(left, right, Differentiator<T>.Default);
        
        public static IEnumerable<Difference<T>> Between(T left, T right, IDifferentiator<T, T> differentiator) =>
            ComputeDifference(left, right, differentiator);

        public static IEnumerable<Difference<TDifference>> Between<TSource, TDifference>(TSource left, TSource right,
            IDifferentiator<TSource, TDifference> differentiator) => ComputeDifference(left, right, differentiator);

        private static IEnumerable<Difference<T>> ComputeDifference(T left, T right, IEqualityComparer<T> comparer)
        {
            var differences = new List<Difference<T>>();
            
            comparer ??= EqualityComparer<T>.Default;
            
            if (!comparer.Equals(left, right))
            {
                differences.Add(Create(left, right, x => x));
            }
            
            return differences;
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
        
        private static IEnumerable<Difference<TDifference>> ComputeDifference<TSource, TDifference>(
            TSource left, TSource right,  IDifferentiator<TSource, TDifference> differentiator)
        {
            return differentiator.DifferenceIn(left, right);
        }

        private static string GetMemberName<TSource, TValue>(Expression<Func<TSource, TValue>> propertyExpression)
        {
            return propertyExpression.Body is MemberExpression memberExpression
                ? memberExpression.Member.Name
                : string.Empty;
        }
    }
}