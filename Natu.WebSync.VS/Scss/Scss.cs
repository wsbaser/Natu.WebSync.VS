using System;
using System.Linq;
using automateit.SCSS;
using NUnit.Framework;

namespace RoslynSpike.Scss {
    public class Scss
    {
        public readonly string Css;
        public readonly string Xpath;
        public bool CombineWithRoot;

        public Scss(string xpath, string css, bool combineWithRoot = false)
        {
            Css = css;
            Xpath = xpath;
            CombineWithRoot = combineWithRoot;
        }

//        public By By {
//            get { return string.IsNullOrEmpty(Css) ? By.XPath(Xpath) : By.CssSelector(Css); }
//        }

        public string Value
        {
            get { return string.IsNullOrEmpty(Css) ? Xpath : Css; }
        }

        public static string Concat(string scssSelector1, string scssSelector2)
        {
            return ScssBuilder.Concat(scssSelector1, scssSelector2).Value;
        }

        public Scss Concat(Scss scss2)
        {
            string resultXpath = XPathBuilder.Concat(Xpath, scss2.Xpath);
            string resultCss = string.IsNullOrEmpty(Css) || string.IsNullOrEmpty(scss2.Css)
                ? string.Empty
                : CssBuilder.Concat(Css, scss2.Css);
            return new Scss(resultXpath, resultCss);
        }

//        public static By GetBy(string scssSelector1, string scssSelector2) {
//            return ScssBuilder.Concat(scssSelector1, scssSelector2).By;
//        }
//
//        public static By GetBy(string scssSelector) {
//            return ScssBuilder.CreateBy(scssSelector);
//        }
        public static bool Equals(Scss x, Scss y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            return x != null && y != null &&
                   string.Equals(x.Css, y.Css) && string.Equals(x.Xpath, y.Xpath) &&
                   x.CombineWithRoot == y.CombineWithRoot;
        }
    }

    public class CssBuilder {
        private const char CSS_PARTS_DELIMITER = ',';

        public static string Concat(string rootCss, string relativeCss) {
            if (string.IsNullOrWhiteSpace(relativeCss))
                return rootCss;
            if (string.IsNullOrEmpty(rootCss))
                return relativeCss;
            var roots = rootCss.Split(CSS_PARTS_DELIMITER);
            if (roots.Length == 1)
                // Выход из рекурсии
                return string.Format("{0} {1}", rootCss, relativeCss);
            string s = roots.Aggregate(string.Empty,
                                       (current, rootXpath) => current + (Concat(rootXpath.Trim(), relativeCss) + ","));
            return s.Substring(0, s.Length - 1);
        }
    }

    [TestFixture]
    public class ScssTests {
        [TestCase("div", "div", "//div/descendant::div", "div div")]
        public void Run(string scssSelector1, string scssSelector2, string resultXpath, string resultCss) {
            Scss scss1 = ScssBuilder.Create(scssSelector1);
            Scss scss2 = ScssBuilder.Create(scssSelector2);
            Scss resultScss = scss1.Concat(scss2);
            Assert.AreEqual(resultXpath, resultScss.Xpath);
            Assert.AreEqual(resultCss, resultScss.Css);
        }
    }
}