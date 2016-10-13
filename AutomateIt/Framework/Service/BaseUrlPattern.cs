using System;
using System.Text.RegularExpressions;

namespace selenium.core.Framework.Service {
    public class BaseUrlPattern {
        private readonly String _regexPattern;

        public BaseUrlPattern(String regexPattern) {
            _regexPattern = regexPattern;
        }

        // Соответствует ли указанный Url шаблону
        public BaseUrlMatchResult Match(String url) {
            var regex = new Regex(_regexPattern);
            Match match = regex.Match(url);
            if (!match.Success)
                return BaseUrlMatchResult.Unmatched();
            string domain = match.Groups["domain"].Value;
            string abspath = hasGroup(match, "abspath") ? match.Groups["abspath"].Value : "/";

            // У сервиса есть жестко заданный поддомен и он совпадает с поддоменом в Url
            if (hasGroup(match, "subdomain")) {
                return new BaseUrlMatchResult(BaseUrlMatchLevel.FullDomain, match.Groups["subdomain"].Value, domain,
                                              abspath);
            }

            String optionalsubdomain = match.Groups["optionalsubdomain"].Value;
            // У сервиса нет жестко заданного поддомена и в Url также нет поддомена
            if (String.IsNullOrEmpty(optionalsubdomain))
                return new BaseUrlMatchResult(BaseUrlMatchLevel.FullDomain, null, domain, abspath);

            // У сервиса нет жестко заданного поддомена, но в Url поддомен имеется
            return new BaseUrlMatchResult(BaseUrlMatchLevel.BaseDomain, optionalsubdomain, domain, abspath);
        }

        // Проверить, имеется ли группа в паттерне
        private bool hasGroup(Match match, string groupName) {
            return !string.IsNullOrEmpty(match.Groups[groupName].Value);
        }
    }
}