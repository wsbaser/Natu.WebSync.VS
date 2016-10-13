/**
 * Created by VolkovA on 27.02.14.
 * Базовый класс для страниц со статичным Url
 */

using selenium.core.Framework.Service;

namespace selenium.core.Framework.Page {
    public interface ISelfMatchingPage {
        UriMatchResult Match(RequestData requestData, BaseUrlInfo baseUrlInfo);
    }

    public abstract class SelfMatchingPageBase : PageBase, ISelfMatchingPage {
        public abstract string AbsolutePath { get; }

        #region ISelfMatchingPage Members

        public virtual UriMatchResult Match(RequestData requestData, BaseUrlInfo baseUrlInfo) {
            return new UriMatcher(AbsolutePath, Data, Params).Match(requestData.Url, baseUrlInfo.AbsolutePath);
        }

        #endregion

        public virtual RequestData GetRequest(BaseUrlInfo defaultBaseUrlInfo) {
            string url = new UriAssembler(BaseUrlInfo, AbsolutePath, Data, Params).Assemble(defaultBaseUrlInfo);
            return new RequestData(url);
        }
    }
}