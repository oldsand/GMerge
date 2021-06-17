using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrapper.Tests.Model
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TestComplexType ComplexType { get; set; }
    }

    public class TestComplexType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestModelWrapper : ModelWrapperNull<TestModel>
    {
        public TestModelWrapper(TestModel model) : base(model)
        {
        }

        public override void Initialize(TestModel model)
        {
            ComplexType = new TestComplexTypeWrapper(Model.ComplexType);
            RegisterTrackingObject(ComplexType);
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
            get => GetValue<TestComplexTypeWrapper>();
            set => SetValue(value, (m, v) =>
            {
                m.ComplexType = new TestComplexType
                {
                    Id = v.Id,
                    Name = v.Name
                };
            });
        }
    }

    public class TestComplexTypeWrapper : ModelWrapperNull<TestComplexType>
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
    }
}