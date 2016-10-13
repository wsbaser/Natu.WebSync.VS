using System;
using NUnit.Framework;

namespace automateit.utils.Extensions {
	public static class UrlExtentions {
		public static string CutBaseUrl(this string s) {
			var uri = new Uri(s);
			return uri.AbsolutePath + uri.Query + uri.Fragment;
		}
	}

	[TestFixture]
	public class UrlExtentionsTest {
		[TestCase("http://moskva.dr-bee.ru/customers/products/?param1=value1", "/customers/products/?param1=value1")]
		[TestCase("http://moskva.dr-bee.ru/customers/products/?param1=value1#tag", "/customers/products/?param1=value1#tag")]
		public void CutBaseUrl(string s, string expected) {
			Assert.AreEqual(expected, s.CutBaseUrl());
		}
	}
}