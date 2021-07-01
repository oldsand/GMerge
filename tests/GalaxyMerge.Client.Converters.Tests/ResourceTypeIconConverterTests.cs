using System.Globalization;
using System.Windows.Controls;
using GalaxyMerge.Client.Core.Utilities;
using GalaxyMerge.Client.Data.Entities;
using NUnit.Framework;

namespace GalaxyMerge.Client.Converters.Tests
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