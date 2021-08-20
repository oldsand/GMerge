using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.UnitTests.TestClasses
{
    public class Car : IDifferentiable<Car>
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public DateTime Sold { get; set; }
        public int Mileage { get; set; }

        public IEnumerable<Difference> DiffersFrom(Car other)
        {
            var differences = new List<Difference>();

            if (other == null)
            {
                differences.Add(Difference.Create(Make, null, GetType()));
                differences.Add(Difference.Create(Model, null, GetType()));
                differences.Add(Difference.Create(Year, null, Year.GetType(), nameof(Year), GetType()));
                differences.Add(Difference.Create(Sold, null, Sold.GetType(), nameof(Sold), GetType()));
                differences.Add(Difference.Create(Mileage, null, Mileage.GetType(), nameof(Mileage), GetType()));
                return differences;
            }
            
            if (!Equals(Make, other.Make))
                differences.Add(Difference.Create(Make, other.Make, GetType()));
            if (!Equals(Model, other.Model))
                differences.Add(Difference.Create(Model, other.Model, GetType()));
            if (!Equals(Year, other.Year))
                differences.Add(Difference.Create(Year, other.Year, GetType()));
            if (!Equals(Sold, other.Sold))
                differences.Add(Difference.Create(Sold, other.Sold, GetType()));
            if (!Equals(Mileage, other.Mileage))
                differences.Add(Difference.Create(Mileage, other.Mileage, GetType()));
            
            return differences;
        }

        public bool Equals(Car other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Make == other.Make && Model == other.Model && Year == other.Year && Sold.Equals(other.Sold) && Mileage == other.Mileage;
        }
    }

    public class Owner : IDifferentiable<Owner>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public IEnumerable<Car> Cars { get; set; }

        public bool Equals(Owner other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName && Age == other.Age &&
                   Cars.SequenceEqual(other.Cars);
        }

        public IEnumerable<Difference> DiffersFrom(Owner other)
        {
            var differences = new List<Difference>();
            
            if (!Equals(FirstName, other.FirstName))
                differences.Add(Difference.Create(FirstName, other.FirstName, GetType()));
            if (!Equals(LastName, other.LastName))
                differences.Add(Difference.Create(LastName, other.LastName, GetType()));
            if (!Equals(Age, other.Age))
                differences.Add(Difference.Create(Age, other.Age, GetType()));
            if (!Equals(Cars, other.Cars))
                differences.Add(Difference.Create(Cars, other.Cars, GetType()));
            
            return differences;
        }

        public int CompareTo(Owner other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var firstNameComparison = string.Compare(FirstName, other.FirstName, StringComparison.Ordinal);
            if (firstNameComparison != 0) return firstNameComparison;
            var lastNameComparison = string.Compare(LastName, other.LastName, StringComparison.Ordinal);
            if (lastNameComparison != 0) return lastNameComparison;
            return Age.CompareTo(other.Age);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is Owner other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(Owner)}");
        }

        public static bool operator <(Owner left, Owner right)
        {
            return Comparer<Owner>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(Owner left, Owner right)
        {
            return Comparer<Owner>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(Owner left, Owner right)
        {
            return Comparer<Owner>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(Owner left, Owner right)
        {
            return Comparer<Owner>.Default.Compare(left, right) >= 0;
        }
    }

    public class CarComparer : IEqualityComparer<Car>
    {
        public bool Equals(Car x, Car y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Make == y.Make && x.Model == y.Model && x.Year == y.Year && x.Sold.Equals(y.Sold) &&
                   x.Mileage == y.Mileage;
        }

        public int GetHashCode(Car obj)
        {
            return HashCode.Combine(obj.Make, obj.Model, obj.Year, obj.Sold, obj.Mileage);
        }
    }
}