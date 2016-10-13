using System;
using selenium.core.Framework.Page;
using selenium.core.Logging;

namespace selenium.core.Framework.Service {
    public abstract class RouterBase : Router {
        public abstract RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);
        public abstract IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);
        public abstract IPage GetEmailPage(Uri uri);
        public abstract bool HasPage(IPage page);
    }
}