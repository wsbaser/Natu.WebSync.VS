using System;

namespace selenium.core.Framework.Page {
    public interface IOverlay {
        /// <summary>
        /// Проверяет указанное исключение.
        /// Возвращает true - если при выполнении клика по некоторому элементу был выполнен клик по перекрывающему его оверлею
        /// </summary>
        /// <param name="e">Исключение, отловленное при клике</param>
        bool IsMatch(Exception e);

        void Close();
    }
}