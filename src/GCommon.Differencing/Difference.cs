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
            PropertyName = propertyName;
            PropertyType = propertyType;
            ObjectType = objectType;
        }

        public object Left { get; set; }
        public object Right { get; set; }
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public Type ObjectType { get; set; }
        
        public static Difference Create(object left, object right, Type propertyType, string propertyName, Type objectType)
        {
            return new(left, right, propertyType, propertyName, objectType);
        }
        
        public static Difference Create<T>(T left, T right, Type objectType)
        {
            var propertyType = typeof(T);
            return new Difference(left, right, propertyType, propertyType.Name, objectType);
        }
        
        public static IEnumerable<Difference> Between<TSource>(TSource me, TSource other) =>
            Compute(me, other, Differentiator<TSource>.Default);
        
        public static IEnumerable<Difference> Between<TSource>(TSource me, TSource other, 
            IDifferentiator<TSource> differentiator) => Compute(me, other, differentiator);

        private static IEnumerable<Difference> Compute<TSource>(TSource me, TSource other, 
            IDifferentiator<TSource> differentiator)
        {
            return differentiator.DifferenceIn(me, other);
        }
    }
}