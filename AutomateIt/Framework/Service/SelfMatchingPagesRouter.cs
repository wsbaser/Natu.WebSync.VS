/**
 * Created by VolkovA on 28.02.14.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.Service {
    public class SelfMatchingPagesRouter : RouterBase {
        private readonly Dictionary<Type, ISelfMatchingPage> _pages;
        private readonly List<IEmailPage> _savedPages;

        public SelfMatchingPagesRouter() {
            _pages = new Dictionary<Type, ISelfMatchingPage>();
            _savedPages = new List<IEmailPage>();
        }

        public override RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo) {
            var selfMatchingPage = page as SelfMatchingPageBase;
            if (selfMatchingPage == null)
                return null;
            return selfMatchingPage.GetRequest(defaultBaseUrlInfo);
        }

        public override IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo) {
            foreach (var dummyPage in _pages.Values)
            {
                UriMatchResult match = dummyPage.Match(requestData, baseUrlInfo);
                if (match.Success) {
                    var instance = (SelfMatchingPageBase) Activator.CreateInstance(dummyPage.GetType());
                    instance.BaseUrlInfo = baseUrlInfo;
                    instance.Data = match.Data;
                    instance.Params = match.Params;
                    instance.Cookies = match.Cookies;
                    return instance;
                }
            }
            return null;
        }

        public override IPage GetEmailPage(Uri uri) {
            return (IPage) _savedPages.FirstOrDefault(p => p.Match(uri));
        }

        public override bool HasPage(IPage page) {
            return _pages.ContainsKey(page.GetType());
        }

        public void RegisterDerivedPages<T>() where T : SelfMatchingPageBase {
            Type superType = typeof (T);
            Assembly assembly = Assembly.GetAssembly(superType);
            IEnumerable<Type> derivedTypes =
                assembly.GetTypes().AsEnumerable().Where(t => !t.IsAbstract && superType.IsAssignableFrom(t));
            foreach (Type derivedType in derivedTypes)
                RegisterPage(derivedType);
        }

        public void RegisterPage<T>() {
            RegisterPage(typeof (T));
        }

        private void RegisterPage(Type pageType) {
            var pageInstance = (ISelfMatchingPage) Activator.CreateInstance(pageType);
            _pages.Add(pageType, pageInstance);
        }

        public void RegisterEmailPage<T>() where T : IEmailPage {
            var pageInstance = (IEmailPage) Activator.CreateInstance(typeof (T));
            _savedPages.Add(pageInstance);
        }
    }
}