using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.UnitTests.TestClasses
{
    public class Car : IDifferentiable<Car>
    {
        public int VinNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public DateTime Sold { get; set; }
        public int Mileage { get; set; }

        public IEnumerable<Difference> DiffersFrom(Car other)
        {
            var differences = new List<Difference>();
            
            differences.AddRange(Difference.Between(this, other, x => x.VinNumber));
            differences.AddRange(Difference.Between(this, other, x => x.Make));
            differences.AddRange(Difference.Between(this, other, x => x.Model));
            differences.AddRange(Difference.Between(this, other, x => x.Year));
            differences.AddRange(Difference.Between(this, other, x => x.Sold));
            differences.AddRange(Difference.Between(this, other, x => x.Mileage));

            return differences;
        }

        public bool Equals(Car other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return VinNumber == other.VinNumber && Make == other.Make && Model == other.Model && Year == other.Year &&
                   Sold.Equals(other.Sold) && Mileage == other.Mileage;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Car);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Make, Model, Year, Sold, Mileage);
        }

        public static bool operator ==(Car left, Car right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Car left, Car right)
        {
            return !Equals(left, right);
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
            
            differences.AddRange(Difference.Between(this, other, x => x.FirstName));
            differences.AddRange(Difference.Between(this, other, x => x.LastName));
            differences.AddRange(Difference.Between(this, other, x => x.Age));
            differences.AddRange(Difference.BetweenCollection(this, other, x => x.Cars, c => c.VinNumber));

            return differences;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Owner);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, Age, Cars);
        }

        public static bool operator ==(Owner left, Owner right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Owner left, Owner right)
        {
            return !Equals(left, right);
        }
    }

    public class CarTitle
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Car Car { get; set; }
        public Owner Owner { get; set; }
    } 
}