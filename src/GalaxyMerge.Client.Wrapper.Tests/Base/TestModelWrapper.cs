using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrapper.Tests.Base
{
    public class TestModelWrapper : ModelWrapper<TestModel>
    {
        private TestComplexTypeWrapper _complexType;

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
            get => _complexType;
            set => SetValue<TestComplexTypeWrapper, TestComplexType>(ref _complexType, value);
        }

        public ChangeTrackingCollection<TestItemWrapper> TestItems { get; private set; }

        protected override void Initialize(TestModel model)
        {
            RequireProperty(nameof(Name));
            
            if (model.ComplexType != null)
                ComplexType = new TestComplexTypeWrapper(model.ComplexType);
            
            if (model.Items != null)
            {
                TestItems = new ChangeTrackingCollection<TestItemWrapper>(model.Items
                    .Select(i => new TestItemWrapper(i)).ToList());
                RegisterCollection(TestItems, model.Items);
            }

            base.Initialize(model);
        }
    }

    public class TestComplexTypeWrapper : ModelWrapper<TestComplexType>
    {
        public TestComplexTypeWrapper(TestComplexType model) : base(model)
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

        protected override void Initialize(TestComplexType model)
        {
            RequireProperty(nameof(Name));
            
            base.Initialize(model);
        }
    }

    public class TestItemWrapper : ModelWrapper<TestItem>
    {
        public TestItemWrapper(TestItem model) : base(model)
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