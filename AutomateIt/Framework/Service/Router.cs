/**
 * Created by VolkovA on 27.02.14.
 */

using System;
using selenium.core.Framework.Page;
using selenium.core.Logging;

namespace selenium.core.Framework.Service {
    public interface Router {
        RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);
        IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);
        IPage GetEmailPage(Uri uri);
        bool HasPage(IPage page);
    }
}