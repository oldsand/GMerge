using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;
using GCommon.Extensions;

namespace GCommon.Differencing
{
    public class Difference
    {
        private Difference(object left, object right, Type propertyType, string propertyName, Type objectType)
        {
            Left = left;
            Right = right;
            PropertyType = propertyType;
            PropertyName = propertyName;
            ObjectType = objectType;
        }

        public object Left { get; }
        public object Right { get; }
        public Type PropertyType { get; }
        public string PropertyName { get; }
        public Type ObjectType { get; }
        public DifferenceType DifferenceType => DetermineDifferenceType(Left, Right);

        public static Difference Create<T>(T left, T right, string propertyName = null, Type objectType = null)
        {
            return new(left, right, typeof(T), propertyName, objectType);
        }

        public static IEnumerable<Difference> Between<T>(T left, T right) =>
            ComputeDifference(left, right, new GenericDiffer<T>());

        public static IEnumerable<Difference> Between<T>(T left, T right, string propertyName, Type objectType) =>
            ComputeDifference(left, right, new GenericDiffer<T>(propertyName, objectType));

        public static IEnumerable<Difference> Between<T>(T left, T right, IDifferentiator<T> differentiator) =>
            ComputeDifference(left, right, differentiator);

        public static IEnumerable<Difference> Between<TSource, TValue>(TSource left, TSource right,
            Expression<Func<TSource, TValue>> propertyExpression) => ComputeDifference(left, right, propertyExpression);
        
        public static IEnumerable<Difference> BetweenSequence<TSource>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right) =>
            ComputeSequentialDifferences(left, right, new GenericDiffer<TSource>());
        
        public static IEnumerable<Difference> BetweenSequence<TSource, TValue>(
            TSource left,
            TSource right,
            Expression<Func<TSource, IEnumerable<TValue>>> propertyExpression) => 
            ComputeSequentialDifferences(left, right, propertyExpression);
        
        public static IEnumerable<Difference> BetweenSequence<TSource>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            IDifferentiator<TSource> differentiator) =>
            ComputeSequentialDifferences(left, right, differentiator);
        
        public static IEnumerable<Difference> BetweenSequence<TSource, TValue>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            Func<TSource, TValue> sortKey) =>
            ComputeSortedSequentialDifferences(left, right, sortKey, new GenericDiffer<TSource>());
        
        public static IEnumerable<Difference> BetweenSequence<TSource, TCollection, TValue>(
            TSource left,
            TSource right,
            Expression<Func<TSource, IEnumerable<TCollection>>> propertyExpression,
            Func<TCollection, TValue> sortKey) => 
            ComputeSortedSequentialDifferences(left, right, propertyExpression, sortKey);

        public static IEnumerable<Difference> BetweenSequence<TSource, TValue>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            Func<TSource, TValue> sortKey,
            IDifferentiator<TSource> differentiator) =>
            ComputeSortedSequentialDifferences(left, right, sortKey, differentiator);
        
        public static IEnumerable<Difference> BetweenCollection<TSource>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right) =>
            ComputeJoinedCollectionDifferences(left, right, x => x, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> BetweenCollection<TSource, TValue>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            Func<TSource, TValue> joinKey) =>
            ComputeJoinedCollectionDifferences(left, right, joinKey, new GenericDiffer<TSource>());

        public static IEnumerable<Difference> BetweenCollection<TSource, TValue>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            Func<TSource, TValue> joinKey,
            IDifferentiator<TSource> differentiator) =>
            ComputeJoinedCollectionDifferences(left, right, joinKey, differentiator);
        
        public static IEnumerable<Difference> BetweenCollection<TSource, TDifference, TValue>(
            TSource left, 
            TSource right,
            Expression<Func<TSource, IEnumerable<TDifference>>> propertyExpression,
            Func<TDifference, TValue> joinKey) => 
            ComputeJoinedCollectionDifferences(left, right, propertyExpression, joinKey);
        
        private static IEnumerable<Difference> ComputeDifference<T>(T left, T right, IDifferentiator<T> differentiator)
        {
            var differ = differentiator ?? new GenericDiffer<T>();
            return differ.DifferenceIn(left, right);
        }

        private static IEnumerable<Difference> ComputeDifference<TSource, TDifference>(TSource left, TSource right,
            Expression<Func<TSource, TDifference>> propertyExpression)
        {
            var propertyName = GetMemberName(propertyExpression);
            var valueGetter = propertyExpression.Compile();
            
            var leftValue = valueGetter.Invoke(left);
            var rightValue = valueGetter.Invoke(right);

            var differ = new GenericDiffer<TDifference>(propertyName, typeof(TSource));
            
            return ComputeDifference(leftValue, rightValue, differ);
        }

        private static IEnumerable<Difference> ComputeSequentialDifferences<TSource>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            IDifferentiator<TSource> differentiator)
        {
            var differences = new List<Difference>();

            if (left == null && right == null)
            {
                return differences;
            }

            if (left == null)
            {
                return right.Select(item => Create(default, item));
            }

            if (right == null)
            {
                return left.Select(item => Create(item, default));
            }

            using var first = left.GetEnumerator();
            using var second = right.GetEnumerator();

            while (first.MoveNext())
            {
                if (second.MoveNext())
                {
                    differences.AddRange(Between(first.Current, second.Current, differentiator));
                }
                else
                {
                    differences.Add(Create(first.Current, default));
                }
            }

            while (second.MoveNext())
            {
                differences.Add(Create(default, second.Current));
            }

            return differences;
        }
        
        private static IEnumerable<Difference> ComputeSequentialDifferences<TSource, TDifference>(TSource left, TSource right,
            Expression<Func<TSource, IEnumerable<TDifference>>> propertyExpression)
        {
            var propertyName = GetMemberName(propertyExpression);
            var valueGetter = propertyExpression.Compile();
            
            var leftValue = valueGetter.Invoke(left);
            var rightValue = valueGetter.Invoke(right);

            var differ = new GenericDiffer<TDifference>(propertyName, typeof(TSource));
            
            return ComputeSequentialDifferences(leftValue, rightValue, differ);
        }
        
        private static IEnumerable<Difference> ComputeSortedSequentialDifferences<TSource, TValue>(
            IEnumerable<TSource> me,
            IEnumerable<TSource> other,
            Func<TSource, TValue> sortKey,
            IDifferentiator<TSource> differentiator)
        {
            var sortedLeft = me?.OrderBy(sortKey);
            var sortedRight = other?.OrderBy(sortKey);

            return ComputeSequentialDifferences(sortedLeft, sortedRight, differentiator);
        }
        
        private static IEnumerable<Difference> ComputeSortedSequentialDifferences<TSource, TDifference, TValue>(
            TSource left,
            TSource right,
            Expression<Func<TSource, IEnumerable<TDifference>>> propertyExpression,
            Func<TDifference, TValue> sortKey)
        {
            var propertyName = GetMemberName(propertyExpression);
            var valueGetter = propertyExpression.Compile();
            
            var leftValue = valueGetter.Invoke(left);
            var rightValue = valueGetter.Invoke(right);

            var differ = new GenericDiffer<TDifference>(propertyName, typeof(TSource));
            
            return ComputeSortedSequentialDifferences(leftValue, rightValue, sortKey, differ);
        }

        private static IEnumerable<Difference> ComputeJoinedCollectionDifferences<TSource, TValue>(
            IEnumerable<TSource> left,
            IEnumerable<TSource> right,
            Func<TSource, TValue> joinKey,
            IDifferentiator<TSource> differentiator)
        {
            var joined = left?.FullOuterJoin(right, joinKey, joinKey, (first, second) => new
            {
                First = first,
                Second = second
            });

            return joined?.SelectMany(x => Between(x.First, x.Second, differentiator));
        }
        
        private static IEnumerable<Difference> ComputeJoinedCollectionDifferences<TSource, TDifference, TValue>(
            TSource left,
            TSource right,
            Expression<Func<TSource, IEnumerable<TDifference>>> propertyExpression,
            Func<TDifference, TValue> joinKey)
        {
            var propertyName = GetMemberName(propertyExpression);
            var valueGetter = propertyExpression.Compile();
            
            var leftValue = valueGetter.Invoke(left);
            var rightValue = valueGetter.Invoke(right);

            var differ = new GenericDiffer<TDifference>(propertyName, typeof(TSource));
            
            return ComputeJoinedCollectionDifferences(leftValue, rightValue, joinKey, differ);
        }
        
        private static string GetMemberName<TSource, TDifference>(
            Expression<Func<TSource, TDifference>> propertyExpression)
        {
            return propertyExpression.Body is MemberExpression memberExpression
                ? memberExpression.Member.Name
                : string.Empty;
        }

        private static DifferenceType DetermineDifferenceType(object left, object right)
        {
            return left == null && right != null ? DifferenceType.Added
                : left != null && right == null ? DifferenceType.Removed
                : DifferenceType.Changed;
        }
    }
}