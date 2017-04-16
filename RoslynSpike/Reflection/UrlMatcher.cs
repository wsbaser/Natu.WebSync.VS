using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoslynSpike.Reflection {
    public class MatchUrlResult {
        public string ServiceId;
        public string PageId;

        public MatchUrlResult(string serviceId, string pageId) {
            ServiceId = serviceId;
            PageId = pageId;
        }
    }

    public class UrlMatcher {
        private readonly Assembly _natuAssembly;
        private readonly Assembly _testsAssembly;

        public UrlMatcher(Assembly natuAssembly, Assembly testsAssembly) {
            _natuAssembly = natuAssembly;
            _testsAssembly = testsAssembly;
        }

        public MatchUrlResult Match(string url) {
            var seleniumContextTypes = GetSeleniumContexts();
            foreach (var seleniumContextType in seleniumContextTypes) {
                var seleniumContext = InitSeleniumContext(seleniumContextType);
                return Match(seleniumContextType, seleniumContext, url);
            }
            return null;
        }

        private MatchUrlResult Match(Type seleniumContextType, object seleniumContextInstance, string url) {
            MatchUrlResult result = null;
            var requestDataType = _natuAssembly.GetType("selenium.core.Framework.Service.RequestData");
            var requestData = Activator.CreateInstance(requestDataType, url);

            var webProperty = seleniumContextType.GetProperty("Web");
            var webInstance = webProperty.GetValue(seleniumContextInstance);
            var webType = webProperty.PropertyType;

            var matchServiceMethod = webType.GetMethod("MatchService");
            var matchServiceResult = matchServiceMethod.Invoke(webInstance, new[] {requestData});

            if (matchServiceResult != null) {
                var serviceMatchResultType = _natuAssembly.GetType("selenium.core.Framework.Service.ServiceMatchResult");
                var serviceProperty = serviceMatchResultType.GetProperty("Service");
                var serviceInstance = serviceProperty.GetValue(matchServiceResult);

                var baseUrlInfoProperty = serviceMatchResultType.GetProperty("BaseUrlInfo");
                var baseUrlInfoInstance = baseUrlInfoProperty.GetValue(matchServiceResult);

                var serviceType = _natuAssembly.GetType("selenium.core.Framework.Service.IService");
                var getPageMethod = serviceType.GetMethod("GetPage");
                var pageInstance = getPageMethod.Invoke(serviceInstance, new[] { requestData, baseUrlInfoInstance });
                result = new MatchUrlResult(serviceInstance.GetType().FullName, pageInstance?.GetType().FullName);
            }
            return result;
        }

        private object InitSeleniumContext(Type type) {
            object instance = Activator.CreateInstance(type);
            var initMethod = type.GetMethod("Init");
            initMethod.Invoke(instance, new object[] { });
            return instance;
        }

        private IEnumerable<Type> GetSeleniumContexts() => _testsAssembly.GetTypes().AsEnumerable().Where(t => !t.IsAbstract && t.FullName.Contains("SeleniumContext"));
    }
}