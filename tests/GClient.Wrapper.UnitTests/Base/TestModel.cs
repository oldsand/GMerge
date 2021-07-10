using System.Collections.Generic;

namespace GClient.Wrapper.UnitTests.Base
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TestComplexType ComplexType { get; set; }

        public ICollection<TestItem> Items { get; set; }
    }

    public class TestComplexType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestItem
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public double Value { get; set; }
    }
}