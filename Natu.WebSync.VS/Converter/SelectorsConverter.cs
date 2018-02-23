using automateit.SCSS;
using Moq;
using NUnit.Framework;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Scss;

namespace RoslynSpike.Converter {
    public class SelectorsConverter {
        private readonly IBrowserConnection _browserConnection;

        public SelectorsConverter(IBrowserConnection browserConnection) {
            _browserConnection = browserConnection;
            if (_browserConnection != null)
                _browserConnection.SelectorToConvertReceived += BrowserConnectionSelectorToConvertReceived;
        }

        private void BrowserConnectionSelectorToConvertReceived(object sender, string selector) {
            var result = Convert(selector);
            _browserConnection.SendSelector(result);
        }

        public Selector Convert(string sourceScss) {
            string xpath = string.Empty;
            string css = string.Empty;
            string scss = sourceScss ?? string.Empty;
            while (!string.IsNullOrEmpty(scss)) {
                try {
                    var scssObject = ScssBuilder.Create(scss);
                    return new Selector(scss, scssObject.Css, scssObject.Xpath);
                }
                catch (InvalidScssException) {
                    scss = scss.Substring(0, scss.Length - 1);
                }
            }
            return new Selector(scss, css, xpath);
        }
    }

    [TestFixture]
    public class SelectorsConverterTests {
        [TestCase(null, "", "", "")]
        [TestCase("input.myclass", "input.myclass", "input.myclass", "//input[contains(@class,'myclass')]")]
        [TestCase("input.myclass[__invalid__", "input.myclass", "input.myclass", "//input[contains(@class,'myclass')]")]
        public void ValidConversion(string sourceScss, string scss, string css, string xpath) {
            // .Arrange
            var converter = new SelectorsConverter(null);
            // .Act
            var selector = converter.Convert(sourceScss);
            // .Assert
            Assert.AreEqual(scss, selector.Scss, "Invalid scss");
            Assert.AreEqual(css, selector.Css, "Invalid css");
            Assert.AreEqual(xpath, selector.XPath, "Invalid xpath");
        }
    }
}