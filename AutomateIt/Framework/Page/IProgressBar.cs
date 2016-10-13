using OpenQA.Selenium;
using selenium.core.Logging;

namespace selenium.core.Framework.Page {
    public interface IProgressBar : IComponent {
        void WaitWhileVisible();
    }

    public class WebLoader : ComponentBase, IProgressBar {
        private readonly By _loaderSelector;

        public WebLoader(IPage parent, By loaderSelector)
            : base(parent) {
            _loaderSelector = loaderSelector;
        }

        public override bool IsVisible() {
            return Is.Visible(_loaderSelector);
        }

        public void WaitWhileVisible() {
            Wait.WhileElementVisible(_loaderSelector);
        }
    }
}