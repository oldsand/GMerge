using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable MemberCanBePrivate.Global

namespace GalaxyMerge.Primitives.Base
{
    /// <summary>
    /// Abstract enumeration class contains logic for generic type safe enumeration.
    /// </summary>
    public abstract class Enumeration
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        protected Enumeration()
        {
        }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public static T FromId<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "Id", item => item.Id == value);
            return matchingItem;
        }

        public static T FromName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "Name", 
                item => string.Equals(item.Name, displayName, StringComparison.CurrentCultureIgnoreCase));
            return matchingItem;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumeration other))
                return false;

            return GetType() == obj.GetType() && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Enumeration a, Enumeration b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Enumeration a, Enumeration b)
        {
            return !(a == b);
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration) other).Id);

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }
    }
}