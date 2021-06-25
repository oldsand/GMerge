using System.Globalization;
using System.Windows;
using System.Windows.Media;
using GalaxyMerge.Client.Core.Converters;
using GalaxyMerge.Client.Data.Entities;
using NUnit.Framework;

namespace GalaxyMerge.Client.Core.Tests
{
    [TestFixture]
    public class ConverterTest
    {
        [Test]
        public void ColorBrushConverterTest()
        {
            var converter = new ColorToBrushConverter();
            var color = (Color?)ColorConverter.ConvertFromString("#FFF10AB3") ?? Colors.Transparent;
            
            var result = (SolidColorBrush) converter.Convert(color, typeof(Color), null, CultureInfo.CurrentCulture);
            
            var expected = new SolidColorBrush(color);
            Assert.AreEqual(expected.Color, result?.Color);
        }

        [Test]
        public void EnumVisibilityConverter_SameResourceType_ReturnsVisible()
        {
            var converter = new EnumVisibilityConverter();
            var resourceType = ResourceType.Connection;
            
            Assert.NotNull(converter);
            
            var result = converter.Convert(resourceType, typeof(Visibility), "Connection", CultureInfo.CurrentCulture);
            
            Assert.AreEqual(Visibility.Visible, result);
        }
        
        [Test]
        public void EnumVisibilityConverter_DifferentResourceType_ReturnsVisible()
        {
            var converter = new EnumVisibilityConverter();
            var resourceType = ResourceType.Connection;
            
            Assert.NotNull(converter);
            
            var result = converter.Convert(resourceType, typeof(Visibility), "Connection", CultureInfo.CurrentCulture);
            
            Assert.AreEqual(Visibility.Visible, result);
        }
    }
}