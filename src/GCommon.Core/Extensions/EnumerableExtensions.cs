using System;
using System.Collections.Generic;
using System.Linq;

namespace GCommon.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftItems,
            IEnumerable<TRight> rightItems,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector) {

            return from left in leftItems
                join right in rightItems on leftKeySelector(left) equals rightKeySelector(right) into temp
                from right in temp.DefaultIfEmpty()
                select resultSelector(left, right);
        }

        public static IEnumerable<TResult> RightOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftItems,
            IEnumerable<TRight> rightItems,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector) {

            return from right in rightItems
                join left in leftItems on rightKeySelector(right) equals leftKeySelector(left) into temp
                from left in temp.DefaultIfEmpty()
                select resultSelector(left, right);
        }

        public static IEnumerable<TResult> RightAntiSemiJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftItems,
            IEnumerable<TRight> rightItems,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector) {

            var keys = new HashSet<TKey>(leftItems.Select(leftKeySelector));
            return rightItems.Where(r => !keys.Contains(rightKeySelector(r))).Select(r => resultSelector(default, r));
        }

        public static IEnumerable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftItems,
            IEnumerable<TRight> rightItems,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector)
        {
            var left = leftItems.ToList();
            var right = rightItems.ToList();

            return left.LeftOuterJoin(right, leftKeySelector, rightKeySelector, resultSelector)
                .Concat(left.RightAntiSemiJoin(right, leftKeySelector, rightKeySelector, resultSelector));
        }
    }
}