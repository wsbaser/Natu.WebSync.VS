/**
 * Created by VolkovA on 27.02.14.
 */

using System;
using System.Collections.Generic;
using System.Web;
using OpenQA.Selenium;

namespace selenium.core.Framework.Service {
    public class RequestData {
        public RequestData(String url)
            : this(url, new List<Cookie>()) {
        }

        public RequestData(String url, List<Cookie> cookies) {
            Url = new Uri(url);
            Cookies = cookies;
        }

        public Uri Url { get; private set; }
        public List<Cookie> Cookies { get; private set; }
        public string BasicAuthLogin { get; set; }
        public string BasicAuthPassword { get; set; }

        public bool HasBasicAuth() {
            return !string.IsNullOrEmpty(BasicAuthLogin);
        }
    }
}