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

            if (!Equals(Make, other.Make))
                differences.Add(Difference.Create(this, other, c => c.Make));
            if (!Equals(Model, other.Model))
                differences.Add(Difference.Create(this, other, c => c.Model));
            if (!Equals(Year, other.Year))
                differences.Add(Difference.Create(this, other, c => c.Year));
            if (!Equals(Sold, other.Sold))
                differences.Add(Difference.Create(this, other, c => c.Sold));
            if (!Equals(Mileage, other.Mileage))
                differences.Add(Difference.Create(this, other, c => c.Mileage));
            
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
                differences.Add(Difference.Create(this, other, c => c.FirstName));
            if (!Equals(LastName, other.LastName))
                differences.Add(Difference.Create(this, other, c => c.LastName));
            if (!Equals(Age, other.Age))
                differences.Add(Difference.Create(this, other, c => c.Age));
            if (!Equals(Cars, other.Cars))
                differences.AddRange(Cars.SequenceDiffersBy(other.Cars));
            
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