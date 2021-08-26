using System;
using System.Collections.Generic;
using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;

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
        
        //todo have an overload for accepting name and type
        public static IEnumerable<Difference> Between<T>(T left, T right, string propertyName, Type objectType) =>
            ComputeDifference(left, right, new GenericDiffer<T>());

        public static IEnumerable<Difference> Between<T>(T left, T right, IDifferentiator<T> differentiator) => 
            ComputeDifference(left, right, differentiator);

        private static IEnumerable<Difference> ComputeDifference<T>(
            T left, T right, IDifferentiator<T> differentiator)
        {
            var differ = differentiator ?? new GenericDiffer<T>();
            return differ.DifferenceIn(left, right);
        }

        private static DifferenceType DetermineDifferenceType(object left, object right)
        {
            return left == null && right != null ? DifferenceType.Added
                : left != null && right == null ? DifferenceType.Removed
                : DifferenceType.Changed;
        }
    }
}