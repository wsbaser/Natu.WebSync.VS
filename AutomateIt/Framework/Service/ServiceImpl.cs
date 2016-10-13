/**
 * Created by VolkovA on 28.02.14.
 */

using System;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.Service {
    public abstract class Service : IService {
        public Service(BaseUrlInfo defaultBaseUrlInfo, BaseUrlPattern baseUrlPattern, Router router) {
            DefaultBaseUrlInfo = defaultBaseUrlInfo;
            BaseUrlPattern = baseUrlPattern;
            Router = router;
        }

        public IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo) {
            return Router.GetPage(requestData, baseUrlInfo);
        }

        public IPage GetEmailPage(Uri uri) {
            return Router.GetEmailPage(uri);
        }

        public RequestData GetRequestData(IPage page) {
            return Router.GetRequest(page, DefaultBaseUrlInfo);
        }

        #region Service Members

        public Router Router { get; private set; }
        public BaseUrlPattern BaseUrlPattern { get; private set; }
        public BaseUrlInfo DefaultBaseUrlInfo { get; private set; }

        #endregion
    }
}