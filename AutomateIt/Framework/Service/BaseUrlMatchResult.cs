using System;

namespace selenium.core.Framework.Service {
    public class BaseUrlMatchResult {
        public String AbsolutePath;
        public String Domain;
        public BaseUrlMatchLevel Level;
        public String SubDomain;

        public BaseUrlMatchResult(BaseUrlMatchLevel level, String subDomain, String domain, String absolutePath) {
            Level = level;
            SubDomain = subDomain;
            Domain = domain;
            AbsolutePath = absolutePath;
        }

        public static BaseUrlMatchResult Unmatched() {
            return new BaseUrlMatchResult(BaseUrlMatchLevel.Unmatched, null, null, null);
        }

        public BaseUrlInfo getBaseUrlInfo() {
            return new BaseUrlInfo(SubDomain, Domain, AbsolutePath);
        }
    }
}