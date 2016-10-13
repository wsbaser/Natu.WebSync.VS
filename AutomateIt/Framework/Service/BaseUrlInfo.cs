/**
 * Created by VolkovA on 03.03.14.
 */

using System;

namespace selenium.core.Framework.Service {
    public class BaseUrlInfo {
        public BaseUrlInfo(String domain) : this(null, domain, null) {
        }

        public BaseUrlInfo(String domain, String absolutePath) : this(null, domain, absolutePath) {
        }

        public BaseUrlInfo(String subDomain, String domain, String absolutePath) {
            SubDomain = subDomain;
            Domain = domain;
            AbsolutePath = absolutePath;
        }

        public string SubDomain { get; private set; }
        public string Domain { get; set; }
        public string AbsolutePath { get; private set; }

        public BaseUrlInfo ApplyActual(BaseUrlInfo baseUrlInfo) {
            string subDomain = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.SubDomain)
                                   ? SubDomain
                                   : baseUrlInfo.SubDomain;
            string domain = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.Domain)
                                ? Domain
                                : baseUrlInfo.Domain;
            string absolutePath = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.AbsolutePath)
                                      ? AbsolutePath
                                      : baseUrlInfo.AbsolutePath;
            return new BaseUrlInfo(subDomain, domain, absolutePath);
        }

        // Сформировать BaseUrl
        public String GetBaseUrl() {
            String s = Domain;
            if (!string.IsNullOrEmpty(AbsolutePath))
                s += AbsolutePath;
            if (!String.IsNullOrEmpty(SubDomain))
                s = SubDomain + "." + s;
            return s;
        }
    }
}