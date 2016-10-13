/**
 * Created by VolkovA on 27.02.14.
 */

using System;
using OpenQA.Selenium;

namespace selenium.core.Logging {
    public interface TestLogger {
        /// <summary>
        /// Залогировать действие
        /// </summary>
        void Action(String msg, params Object[] args);

        /// <summary>
        /// Залогировать информационное сообщение
        /// </summary>
        void Info(String msg, params Object[] args);

        /// <summary>
        /// Залогировать критическуюж ошибку
        /// </summary>
        void FatalError(String s, Exception e);

        /// <summary>
        /// Сохранить значение в лог
        /// </summary>
        void WriteValue(string key, object value);

        /// <summary>
        /// Прочитать сохраненное ранее значение из логи
        /// </summary>
        T GetValue<T>(string key);

		T GetValue<T>(object key);

		/// <summary>
		/// Залогировать селектор
		/// </summary>
		void Selector(By by);
        /// <summary>
        /// Залогировать исключение
        /// </summary>
        void Exception(Exception exception);
    }
}