using System.Linq;
using ArchestrA.GRAccess;
using AutoFixture;
using GCommon.Primitives;
using GServer.Archestra.Extensions;
using GServer.Archestra.Helpers;
using NUnit.Framework;

namespace GServer.Archestra.UnitTests.ExtensionTests
{
    [TestFixture]
    public class MxTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }
        
        [Test]
        public void CreateBoolean_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = Mx.Create<bool>();

            var type = mxValue.GetDataType();
            
            Assert.AreEqual(MxDataType.MxBoolean, type);
        }
        
        [Test]
        public void CreateBoolean_ProvideValue_ReturnsExpectedDataType()
        {
            var mxValue = Mx.Create(_fixture.Create<bool>());

            var type = mxValue.GetDataType();
            
            Assert.AreEqual(MxDataType.MxBoolean, type);
        }
        
        [Test]
        public void CreateBooleanArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = Mx.Create<bool[]>();

            var type = mxValue.GetDataType();
            
            Assert.AreEqual(MxDataType.MxBoolean, type);
            Assert.True(mxValue.IsArray());
        }
        
        [Test]
        public void CreateBooleanArray_ProvideValue_ReturnsExpectedDataType()
        {
            var mxValue = Mx.Create(_fixture.CreateMany<bool>().ToArray());

            var type = mxValue.GetDataType();
            
            Assert.AreEqual(MxDataType.MxBoolean, type);
            Assert.True(mxValue.IsArray());
        }
        
        [Test]
        public void CreateIntArray_ProvideValue_ReturnsExpectedDataType()
        {
            var mxValue = Mx.Create(_fixture.CreateMany<int>());

            var type = mxValue.GetDataType();
            
            Assert.AreEqual(MxDataType.MxInteger, type);
            Assert.True(mxValue.IsArray());
        }

        [Test]
        public void CreateReference_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = Mx.Create<Reference>();

            var type = mxValue.GetDataType();

            Assert.AreEqual(MxDataType.MxReferenceType, type);
        }

        [Test]
        public void CreateReferenceArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = Mx.Create<Reference[]>();

            var type = mxValue.GetDataType();

            Assert.AreEqual(MxDataType.MxReferenceType, type);
            Assert.True(mxValue.IsArray());
        }
        
        [Test]
        public void CreateReference_ProvidedValue_ReturnsExpectedDataTypeAndValue()
        {
            var expected = new Reference {FullReference = "TestReference"};
            var mxValue = Mx.Create(expected);

            var type = mxValue.GetDataType();
            Assert.AreEqual(MxDataType.MxReferenceType, type);

            var result = mxValue.GetValue<Reference>();
            Assert.AreEqual(expected.FullReference, result.FullReference);
        }
    }
}