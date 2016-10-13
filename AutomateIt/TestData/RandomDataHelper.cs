using System;

namespace selenium.core.TestData
{
    public static class RandomDataHelper
    {
        /// <summary>
        /// Сгенерировать числовую последовательность указанной длинны
        /// </summary>
        /// <param name="length">длина последовательности</param>
        public static string Cifers(int length = 10) {
            string s = string.Empty;
            var random = new Random();
            for (int i = 0; i < length; i++)
                s += random.Next(10);
            return s;
        }
    }
}
