using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using automateit.utils.Extensions;

namespace selenium.core.Framework.Page {
	public class UriMatcher
	{
		private readonly string _pageAbsolutePath;
		private readonly Dictionary<string, string> _pageData;
		private readonly StringDictionary _pageParams;

		public UriMatcher(string pageAbsolutePath, Dictionary<string, string> pageData, StringDictionary pageParams)
		{
			_pageAbsolutePath = pageAbsolutePath;
			_pageData = pageData;
			_pageParams = pageParams;
		}

		public UriMatchResult Match(Uri uri, string siteAbsolutePath)
		{
			var actualData = new Dictionary<string, string>();
			var actualParams = new StringDictionary();

			string realPath = uri.AbsolutePath.Replace(siteAbsolutePath, "");
			if (realPath == "/app/")
			{
				var t = uri.ToString().Split('/');
				foreach (string value in _pageParams.Values)
				{
					if (t.Last() == value)
						return new UriMatchResult(true, _pageData, _pageParams);
				}
			}

			var pageArr = _pageAbsolutePath.Split('/');
			var realArr = realPath.Split('/');
			if (pageArr.Length != realArr.Length)
				return UriMatchResult.Unmatched();

			// Сравнить адрес страницы
			for (int i = 0; i < pageArr.Length; i++)
			{
				string pageArrItem = pageArr[i];
				string realArrItem = realArr[i];
				if (pageArrItem.StartsWith("{") && pageArrItem.EndsWith("}"))
				{
					string paramName = pageArrItem.Substring(1, pageArrItem.Length - 2);
					actualData[paramName] = realArrItem;
				}
				else if (string.Compare(pageArrItem, realArrItem, StringComparison.OrdinalIgnoreCase) != 0)
					return UriMatchResult.Unmatched();
			}

			// Извлечь список параметров
			var queryParamsArr = uri.Query.CutFirst('?').Split('&');
			foreach (var queryParam in queryParamsArr)
			{
				var keyvalue = queryParam.Split('=');
				if (keyvalue.Length < 2)
					continue;
				actualParams.Add(keyvalue[0], keyvalue[1]);
			}

			// Сравнение Data
			if (_pageData!=null)
				foreach (var key in _pageData.Keys)
				{
					if (!actualData.ContainsKey(key) ||
						string.Compare(actualData[key], _pageData[key], StringComparison.OrdinalIgnoreCase) != 0)
						return UriMatchResult.Unmatched();
				}

//			if (!(realPath.IndexOf("Results", StringComparison.OrdinalIgnoreCase) >= 0) && !(realPath.IndexOf("Document", StringComparison.OrdinalIgnoreCase) >= 0))
//			{
//				// Сравнение Params
//				if (actualParams.Count != _pageParams.Count)
//				{
//					return UriMatchResult.Unmatched();
//				}
				foreach (string key in _pageParams.Keys)
				{
					if (!actualParams.ContainsKey(key) ||
						string.Compare(actualParams[key], _pageParams[key], StringComparison.OrdinalIgnoreCase) != 0)
						return UriMatchResult.Unmatched();
				}
//			}

			return new UriMatchResult(true, actualData, actualParams);
		}
		
	}
}