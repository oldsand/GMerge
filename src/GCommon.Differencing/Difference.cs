#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GCommon.Core.Extensions;

namespace GCommon.Differencing
{
    public class Difference
    {
        private Difference(object? left, object? right, Type propertyType, string propertyName, Type? objectType)
        {
            Left = left;
            Right = right;
            PropertyName = propertyName;
            PropertyType = propertyType;
            ObjectType = objectType;
        }

        public object? Left { get; set; }
        public object? Right { get; set; }
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public Type? ObjectType { get; set; }

        public static Difference Create<T>(T? left, T? right, Type? objectType)
        {
            var propertyType = typeof(T);
            return new Difference(left, right, propertyType, propertyType.Name, objectType);
        }
        
        public static Difference Create(object? left, object? right, Type propertyType, string propertyName, Type? objectType)
        {
            return new(left, right, propertyType, propertyName, objectType);
        }

        public static IEnumerable<Difference> Between<TSource>(TSource me, TSource other) =>
            Between(me, other, EqualityComparer<TSource>.Default);
        
        public static IEnumerable<Difference> BetweenCollection<TSource>(IEnumerable<TSource> me, IEnumerable<TSource> other) =>
            Between(me, other, source => source, EqualityComparer<TSource>.Default);
        
        public static IEnumerable<Difference> Between<TSource, TValue>(IEnumerable<TSource> me, IEnumerable<TSource> other,
            Func<TSource, TValue> sortKey) =>
            Between(me, other, sortKey, EqualityComparer<TSource>.Default);

        public static IEnumerable<Difference> Between<TSource>(TSource me, TSource other, 
            IEqualityComparer<TSource> comparer)
        {
            var type = typeof(TSource);

            if (type.IsEnumerable())
                throw new InvalidOperationException("");
            
            return type.IsSimpleType() ? InSimpleType(me, other, comparer) : InComplexType(me, other, comparer);
        }

        public static IEnumerable<Difference> Between<TSource, TValue>(IEnumerable<TSource> me, 
            IEnumerable<TSource> other, Func<TSource, TValue> sortKey, IEqualityComparer<TSource> comparer)
        {
            var type = typeof(TSource);
            //var sortByName = ExtractPropertyName(sortByProperty);
            
            //todo null and size checks

            if (type.IsSimpleType())
                return InSimpleCollection(me, other, sortKey, comparer);
            
            throw new NotSupportedException("What do you expect from me!");
        }

        private static IEnumerable<Difference> InSimpleType<TSource>(TSource left, TSource right, 
            IEqualityComparer<TSource> comparer)
        {
            var differences = new List<Difference>();
            var propertyType = typeof(TSource);

            if (!propertyType.IsSimpleType())
                throw new InvalidOperationException(
                    "Type is not a simple type. Provide simple type or use InComplexType");

            if (!comparer.Equals(left, right))
                differences.Add(Create(left, right, null));

            return differences;
        }

        private static IEnumerable<Difference> InComplexType<TSource>(TSource left, TSource right,
            IEqualityComparer<TSource> comparer)
        {
            var objectType = typeof(TSource);

            if (objectType.IsSimpleType())
                throw new InvalidOperationException(
                    "Type is a simple type. Provide complex type or use InSimpleType");

            var properties = objectType.GetProperties();

            return (from property in properties
                where property.PropertyType.IsSimpleType() && NotEqual(left, right, property, comparer)
                select new Difference(left, right, property.PropertyType, property.Name, objectType)).ToList();
        }

        private static IEnumerable<Difference> InSimpleCollection<TSource, TValue>(IEnumerable<TSource> me,
            IEnumerable<TSource> other, Func<TSource, TValue> sortByProperty, IEqualityComparer<TSource> comparer)
        {
            if (me == null)
            {
                throw new ArgumentNullException();
            }

            if (other == null)
            {
                throw new ArgumentNullException();
            }
            
            var differences = new List<Difference>();
            
            var orderedMe = me.OrderBy(sortByProperty);
            var orderedOther = other.OrderBy(sortByProperty);

            using var e1 = orderedMe.GetEnumerator();
            using var e2 = orderedOther.GetEnumerator();
            
            while (e1.MoveNext())
            {
                if (!(e2.MoveNext() && comparer.Equals(e1.Current, e2.Current)))
                {
                    differences.Add(Create(e1.Current, e2.Current, null));
                }
            }

            return differences;
        }

        private static bool NotEqual<TSource>(TSource left, TSource right, PropertyInfo propertyInfo, 
            IEqualityComparer<TSource> comparer)
        {
            var l = propertyInfo.GetValue(left);
            var r = propertyInfo.GetValue(right);

            return NotEqual(l, r);
        }

        private static bool NotEqual<T>(T left, T right)
        {
            if (left is IEquatable<T> equatable)
                return !equatable.Equals(right);

            return !Equals(left, right);
        }

        private static string ExtractPropertyName<TSource, TValue>(Expression<Func<TSource, TValue>> propertyExpression)
        {
            if (!(propertyExpression.Body is MemberExpression memberExpression))
                throw new InvalidOperationException("");

            return memberExpression.Member.Name;
        }
        
    }
}