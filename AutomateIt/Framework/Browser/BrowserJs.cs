using OpenQA.Selenium;

namespace selenium.core.Framework.Browser {
    public class BrowserJs : DriverFacade {
        public BrowserJs(Browser browser)
            : base(browser) {
        }

        /// <summary>
        /// ��������� Java Script
        /// </summary>
        public T Excecute<T>(string js) {
            return (T) Excecute(js);
        }

        /// <summary>
        /// ��������� Java Script
        /// </summary>
        public object Excecute(string js, params object[] args) {
            var excecutor = Driver as IJavaScriptExecutor;
            js = string.Format(js, args);
            return excecutor.ExecuteScript(js);
        }

        /// <summary>
        /// ���������� ��������� �� Y ����� ������� ���� �����
        /// </summary>
        public void ScrollIntoView(IWebElement element) {
            Excecute("window.scrollTo(0, {0});", element.Location.Y);
        }

        /// <summary>
        /// �������� ����������� ������� ��� ���������� ��������
        /// </summary>
        /// <remarks>������ ��� ������� � JQuery</remarks>
        public string GetEventHandlers(string css, JsEventType eventType) {
            string js = string.Format(@"var handlers= $._data($('{0}').get(0),'events').{1};
                          var s='';
                          for(var i=0;i<handlers.length;i++)
                            s+=handlers[i].handler.toString();
                          return s;", css, eventType);
            return Excecute<string>(js);
        }

        /// <summary>
        /// ��������� �� ����� ��������
        /// </summary>
        public bool IsPageBottom() {
            return Excecute<bool>("return document.body.scrollHeight===" +
                                  "document.body.scrollTop+document.documentElement.clientHeight");
        }

        /// <summary>
        /// ���������� ��������� �� ���� ��������
        /// </summary>
        public void ScrollToBottom() {
            Excecute(@"window.scrollTo(0,
                                       Math.max(document.documentElement.scrollHeight,
                                                document.body.scrollHeight,
                                                document.documentElement.clientHeight));");
        }

        /// <summary>
        /// ���������� ��������� �� ����� ��������
        /// </summary>
        public void ScrollToTop() {
            Excecute(@"window.scrollTo(0,0);");
        }
    }

    /// <summary>
    /// ���� ����������� js �������
    /// </summary>
    public enum JsEventType {
        click
    }
}