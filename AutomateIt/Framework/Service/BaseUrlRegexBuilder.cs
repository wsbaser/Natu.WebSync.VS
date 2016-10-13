/**
 * Created by VolkovA on 28.02.14.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace selenium.core.Framework.Service {
    public class BaseUrlRegexBuilder {
        private readonly String _domainPattern;
        private String _absolutePathPattern = "";
        private String _subDomainPattern = "((?<optionalsubdomain>[^\\.]+)\\.)?";

        public BaseUrlRegexBuilder(String domain)
            : this(new List<String> {domain}) {
        }

        public BaseUrlRegexBuilder(List<String> domains) {
            _domainPattern = GenerateDomainsPattern(domains);
        }

        private String GenerateDomainsPattern(List<String> domains) {
            String s = domains.Aggregate("", (current, domain) => current + (domain + "|"));
            s = s.Substring(0, s.Length - 1);
            s = s.Replace(".", "\\.");
            return String.Format("(?<domain>({0}))", s);
        }

        public void SetSubDomain(String value) {
            _subDomainPattern = string.Format("(?<subdomain>{0})\\.", value);
        }

        public void SetAbsolutePathPattern(String pattern) {
            _absolutePathPattern = String.Format("(?<abspath>\\/{0})", pattern);
        }

        // Сформировать Regex паттерн для BaseUrl сервиса
        public String Build() {
            return "(http(|s)://|)(www.|)" +
                   _subDomainPattern +
                   _domainPattern +
                   _absolutePathPattern +
                   ".*";
        }
    }
}