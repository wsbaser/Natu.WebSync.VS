using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using selenium.core.Framework.Browser;

namespace selenium.core.Tests {
    public class TestHelper {
        /// <summary>
        /// Выполняет указанное действие
        /// Любой возникший при этом Exception преобразовывает в AssertException
        /// </summary>
        public static void AssertAction(Action action, string msg, params object[] args) {
            try {
                action.Invoke();
            }
            catch (Exception e) {
                throw new AssertFailedException(string.Format(msg, args), e);
            }
        }

        /// <summary>
        /// Проверяет что в обработчике кнопки есть вызов window.print()
        /// </summary>
        public static void AssertHasPrintAction(ISeleniumContext context, string css) {
            var clickHandlers = context.Browser.Js.GetEventHandlers(css, JsEventType.click);
            Assert.IsTrue(clickHandlers.Contains("window.print()"),
                          "Обработчик клика по элементу '{0}' не содержит вызова window.print()");
        }
    }
}