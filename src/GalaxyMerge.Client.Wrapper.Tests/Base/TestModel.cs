using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrapper.Tests.Base
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

    public class TestModelWrapper : ModelWrapper<TestModel>
    {
        public TestModelWrapper(TestModel model) : base(model)
        {
        }

        public int Id
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Description
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TestComplexTypeWrapper ComplexType
        {
            get => new TestComplexTypeWrapper(Model.ComplexType);
            set => SetValue<TestComplexTypeWrapper, TestComplexType>(value);
        }

        public ChangeTrackingCollection<TestItemWrapper> TestItems { get; private set; }

        protected override void Initialize(TestModel model)
        {
            if (model.Items != null)
            {
                TestItems = new ChangeTrackingCollection<TestItemWrapper>(model.Items.Select(i => new TestItemWrapper(i)).ToList());
                RegisterCollection(TestItems, model.Items);
            }
            
            base.Initialize(model);
        }
    }

    public class TestComplexTypeWrapper : ModelWrapper<TestComplexType>
    {
        public TestComplexTypeWrapper(TestComplexType model) : base(model, false)
        {
        }

        public int Id
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
    
    public class TestItemWrapper : ModelWrapper<TestItem>
    {
        public TestItemWrapper(TestItem model) : base(model, false)
        {
        }

        public int Id
        {
            get => GetValue<int>();
            set => SetValue(value, (m, v) => m.Id = v);
        }

        public string Item  
        {
            get => GetValue<string>();
            set => SetValue(value, (m, v) => m.Item = v);
        }

        public double Value
        {
            get => GetValue<double>();
            set => SetValue(value, (m, v) => m.Value = v);
        }
    }
}