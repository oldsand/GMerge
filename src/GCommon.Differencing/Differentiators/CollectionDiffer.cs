using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Core.Extensions;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class CollectionDiffer<T> : ICollectionDifferentiator<T, T>
    {
        private readonly IEqualityComparer<T> _customComparer;

        public CollectionDiffer(IEqualityComparer<T> customComparer = null)
        {
            _customComparer = customComparer;
        }
        
        public bool Equals(T x, T y)
        {
            var comparer = _customComparer ?? EqualityComparer<T>.Default;
            return comparer.Equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public IEnumerable<Difference<T>> DifferenceIn<TValue>(IEnumerable<T> first, IEnumerable<T> second, 
            Func<T, TValue> key, CollectionMatchMode mode)
        {
            var differences = new List<Difference<T>>();
            
            switch (mode)
            {
                case CollectionMatchMode.Join:
                    differences.AddRange(JoinedCollectionDifferences(first, second, key));
                    break;
                case CollectionMatchMode.Sort:
                    differences.AddRange(SortedCollectionDifferences(first, second, key));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            return differences;
        }
        
        private static IEnumerable<Difference<TSource>> JoinedCollectionDifferences<TSource, TValue>(
            IEnumerable<TSource> left, IEnumerable<TSource> right, Func<TSource, TValue> key)
        {
            var joined = left.FullOuterJoin(right, key, key, (first, second) => new
            {
                First = first,
                Second = second
            });

            return (from pair in joined
                where !Equals(pair.First, pair.Second)
                select new Difference<TSource>(pair.First, pair.Second)).ToList();
        }
        
        private static IEnumerable<Difference<TSource>> SortedCollectionDifferences<TSource, TValue>(
            IEnumerable<TSource> left, IEnumerable<TSource> right, Func<TSource, TValue> key)
        {
            var differences = new List<Difference<TSource>>();
            
            var sortedLeft = left.OrderBy(key);
            var sortedRight = right.OrderBy(key);

            using var e1 = sortedLeft.GetEnumerator();
            using var e2 = sortedRight.GetEnumerator();

            while (e1.MoveNext())
            {
                if (e2.MoveNext())
                {
                    if (!Equals(e1.Current, e2.Current))
                    {
                        differences.Add(new Difference<TSource>(e1.Current, e2.Current));
                    }
                }
                else
                {
                    differences.Add(new Difference<TSource>(e1.Current, right: default));
                }
            }

            while (e2.MoveNext())
            {
                differences.Add(new Difference<TSource>(default, e2.Current));
            }

            return differences;
        } 
    }
}