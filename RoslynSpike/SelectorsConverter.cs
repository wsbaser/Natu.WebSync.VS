using System;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Scss;

namespace RoslynSpike {
    public class SelectorsConverter {
        private readonly IBrowserConnection _browserConnection;

        public SelectorsConverter(IBrowserConnection browserConnection) {
            _browserConnection = browserConnection;
            _browserConnection.SelectorToConvertReceived += BrowserConnectionSelectorToConvertReceived;
        }

        private void BrowserConnectionSelectorToConvertReceived(object sender, string selector) {
            var result = Convert(selector);
            _browserConnection.SendSelector(result.Item1,result.Item2);
        }

        private Tuple<string, SelectorType> Convert(string selector) {
            try {
                var scss = ScssBuilder.Create(selector);
                if (scss.Css != null)
                {
                    return new Tuple<string, SelectorType>(scss.Css, SelectorType.Css);
                }
                return new Tuple<string, SelectorType>(scss.Xpath, SelectorType.XPath);

            }
            catch (InvalidScssException) {
                return new Tuple<string, SelectorType>(selector, SelectorType.Undefined);
            }
        }
    }

    public enum SelectorType {
        Undefined,
        XPath,
        Css,
        Scss
    }
}