using ArchestrA.Core;
using FluentAssertions;
using NUnit.Framework;

namespace GServer.Archestra.LibraryTests
{
    [TestFixture]
    public class ReferenceTests
    {
        [Test]
        public void Create_InitializeProperties_ShouldHaveExpected()
        {
            //This class is a part of ArchestrA.Core.
            //Unfortunately Archestra decided to copy code to GRAccess instead of just referencing core.
            var reference = new MxReferenceClass
            {
                FullReferenceString = "Me.Reference",
                AutomationObjectReferenceString = "SomeObject",
                AttributeReferenceString = "Reference"
            };

            reference.Should().NotBeNull();
            reference.FullReferenceString.Should().Be("Me.Reference");
            reference.AutomationObjectReferenceString.Should().Be("SomeObject");
            reference.AttributeReferenceString.Should().Be("Reference");
        }
    }
}