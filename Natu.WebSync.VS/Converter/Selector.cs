namespace RoslynSpike.Converter {
    public class Selector {
        public string XPath { get; }
        public string Css { get; }
        public string Scss { get; }

        public Selector(string scss, string css, string xpath) {
            Scss = scss;
            Css = css;
            XPath = xpath;
        }
    }
}