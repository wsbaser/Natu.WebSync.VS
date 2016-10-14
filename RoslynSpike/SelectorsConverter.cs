using System;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Scss;

namespace RoslynSpike {
    public class SelectorsConverter {
        private readonly IBrowserConnection _browserConnection;

        public SelectorsConverter(IBrowserConnection browserConnection) {
            _browserConnection = browserConnection;
            _browserConnection.SelectorReceived += _browserConnection_SelectorReceived;
        }

        private void _browserConnection_SelectorReceived(object sender, string selector) {
            var result = Convert(selector);
            _browserConnection.SendSelector(result.Item1,result.Item2);
        }

        private Tuple<string, SelectorType> Convert(string selector) {
            var scss = ScssBuilder.Create(selector);
            if (scss.Css != null) {
                return new Tuple<string, SelectorType>(scss.Css, SelectorType.Css);
            }
            return new Tuple<string, SelectorType>(scss.Xpath, SelectorType.XPath);
        }
    }

    public enum SelectorType {
        XPath,
        Css,
        Scss
    }
}