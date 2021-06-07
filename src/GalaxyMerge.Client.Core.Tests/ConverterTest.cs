using System.Globalization;
using System.Windows.Media;
using GalaxyMerge.Client.Core.Converters;
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
    }
}