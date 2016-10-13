using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using automateit.utils.Extensions;
using selenium.core.Framework.Service;

namespace selenium.core.Framework.Page {
    public class UriAssembler {
        private readonly string _absolutePath;
        private readonly BaseUrlInfo _baseUrlInfo;
        private readonly Dictionary<string, string> _data;
        private readonly StringDictionary _params;

        public UriAssembler(BaseUrlInfo baseUrlInfo, string absolutePath, Dictionary<string, string> data,
                            StringDictionary @params) {
            _baseUrlInfo = baseUrlInfo;
            _absolutePath = absolutePath;
            _data = data;
            _params = @params;
        }

        public string Assemble(BaseUrlInfo defaultBaseUrlInfo) {
            string url = String.Format("http://{0}{1}", GetBaseUrl(defaultBaseUrlInfo), GetPath());
            string query = GetQuery();
            if (!string.IsNullOrEmpty(query))
            {
                url += "?" + query;
            }
            return url;
        }

        /// <summary>
        /// —формировать строку с параметрами Url
        /// </summary>
        private string GetQuery() {
            if (_params == null)
                return string.Empty;
            var query = _params.Keys.Cast<string>().Aggregate(string.Empty,
                                                              (current, key) =>
                                                              current + (key + "=" + _params[key] + "&"));
            return query.CutLast('&');
        }

        /// <summary>
        /// ѕодставить параметры из Data в адрес страницы
        /// </summary>
        private string GetPath() {
            if (_data == null)
                return _absolutePath;
            string path = _absolutePath;
            foreach (var key in _data.Keys) {
                string param = "{" + key + "}";
                path = path.Replace(param, _data[key]);
            }
            return path;
        }

        private string GetBaseUrl(BaseUrlInfo defaultBaseUrlInfo) {
            return defaultBaseUrlInfo.ApplyActual(_baseUrlInfo).GetBaseUrl();
        }
    }
}