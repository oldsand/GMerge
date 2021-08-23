using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing
{
    public class Difference
    {
        private Difference(object left, object right, Type propertyType, string propertyName, Type objectType)
        {
            Left = left;
            Right = right;
            PropertyName = propertyName;
            PropertyType = propertyType;
            ObjectType = objectType;
        }

        public string PropertyName { get; }
        public Type PropertyType { get; }
        public object Left { get; }
        public object Right { get; }
        public Type ObjectType { get; }

        public static Difference Create<TSource>(TSource left, TSource right, string propertyName = null,
            Type objectType = null)
        {
            var propertyType = typeof(TSource);
            propertyName ??= propertyType.Name;
            return new Difference(left, right, propertyType, propertyName, objectType);
        }

        public static Difference Create<TSource>(TSource left, string propertyName = null, Type objectType = null)
        {
            var propertyType = typeof(TSource);
            propertyName ??= propertyType.Name;
            return new Difference(left, default, propertyType, propertyName, objectType);
        }

        public static Difference Create<TSource, TValue>(TSource left, TSource right,
            Expression<Func<TSource, TValue>> propertyExpression)
        {
            var propertyName = GetMemberName(propertyExpression);

            var valueGetter = propertyExpression.Compile();
            
            var leftValue = valueGetter.Invoke(left);
            var rightValue = valueGetter.Invoke(right);

            return new Difference(leftValue, rightValue, typeof(TValue), propertyName, typeof(TSource));
        }

        public static IEnumerable<Difference> Between<TSource>(TSource left, TSource right) =>
            ComputeDifference(left, right, Differentiator<TSource>.Default);

        public static IEnumerable<Difference> Between<TSource>(TSource left, TSource right,
            IDifferentiator<TSource> differentiator) => ComputeDifference(left, right, differentiator);

        private static IEnumerable<Difference> ComputeDifference<TSource>(TSource left, TSource right,
            IDifferentiator<TSource> differentiator)
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