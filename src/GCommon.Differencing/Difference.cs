using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing
{
    public class Difference<T>
    {
        public Difference(T? left, T? right, string propertyName = null, Type objectType = null)
        {
            Left = left;
            Right = right;
            PropertyType = typeof(T);
            PropertyName = propertyName;
            ObjectType = objectType;
        }
        
        public Difference(T? left, string propertyName = null, Type objectType = null)
        {
            Left = left;
            Right = default;
            PropertyType = typeof(T);
            PropertyName = propertyName;
            ObjectType = objectType;
        }

        public T? Left { get; }
        public T? Right { get; }
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

        public static IEnumerable<Difference<T>> Between(T left, T right) =>
            ComputeDifference(left, right, Differentiator<T>.Default);
        
        public static IEnumerable<Difference<T>> Between(T left, T right, IDifferentiator<T, T> differentiator) =>
            ComputeDifference(left, right, differentiator);

        public static IEnumerable<Difference<TDifference>> Between<TSource, TDifference>(TSource left, TSource right,
            IDifferentiator<TSource, TDifference> differentiator) => ComputeDifference(left, right, differentiator);

        private static IEnumerable<Difference<TDifference>> ComputeDifference<TSource, TDifference>(
            TSource left, TSource right, IDifferentiator<TSource, TDifference> differentiator)
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