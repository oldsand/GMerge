using System.Globalization;
using System.Windows.Controls;
using GClient.Converters;
using GClient.Core.Utilities;
using GClient.Data.Entities;
using NUnit.Framework;

namespace GClient.Converters.UnitTests
{
    [TestFixture]
    public class ResourceTypeIconConverterTests
    {
        [Test]
        public void Convert_Connection_ReturnsConnectionIcon()
        {
            var converter = new ResourceTypeIconConverter();
            var expected = ResourceFinder.Find<ControlTemplate>("Icon.Filled.Plug");

            var result = converter.Convert(ResourceType.Connection, typeof(ControlTemplate), null,
                CultureInfo.CurrentCulture);
            
            Assert.AreEqual(expected, result);
        }
    }
}