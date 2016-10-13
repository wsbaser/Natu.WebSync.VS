namespace selenium.core.SCSS {
    internal class ScssAttribute {
        public string Name;
        public string Value;
        public AttributeMatchStyle MatchStyle;

        public ScssAttribute(string name, string value, AttributeMatchStyle matchStyle) {
            Name = name;
            Value = value;
            MatchStyle = matchStyle;
        }
    }
}