using ArchestrA.GRAccess;
using AutoFixture;
using GCommon.Primitives;
using GServer.Archestra.Extensions;
using GServer.Archestra.Helpers;
using NUnit.Framework;

namespace GServer.Archestra.UnitTests.ExtensionTests
{
    [TestFixture]
    public class MxReferenceTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void GetValue_SingleValue_ReturnsExpectedValue()
        {
            var expected = new Reference {FullReference = "Some.Reference"};
            var mxValue = Mx.Create(expected);
            
            var result = mxValue.GetValue<Reference>();

            Assert.AreEqual(expected.FullReference, result.FullReference);
        }

        [Test]
        public void SetValue_SingleValue_ReturnsExpectedValue()
        {
            var expected = new Reference
            {
                FullReference = "Some.Reference"
            };
            var mxValue = Mx.Create<Reference>();
            
            mxValue.SetValue(expected);

            var result = mxValue.GetValue<Reference>();
            Assert.AreEqual(expected.FullReference, result.FullReference);
        }
    }
}