using System.Linq;
using NUnit.Framework;

namespace RoslynSpike.Utilities.Extensions {
	public static class StringExtensions {
		public static string CutFirst(this string s) {
			return s.Substring(1, s.Length - 1);
		}

		public static string CutFirst(this string s, char symbol) {
			return s.StartsWith(symbol.ToString()) ? s.Substring(1, s.Length - 1) : s;
		}

	    public static string CutLast(this string s) {
	        return s.Substring(0, s.Length - 1);
	    }

	    public static string CutLast(this string s, char symbol) {
			return s.EndsWith(symbol.ToString()) ? s.Substring(0, s.Length - 1) : s;
		}

//		public static int AsInt(this string s) {
//			return int.Parse(FindInt(s));
//		}
//
//		public static decimal AsDecimal(this string s) {
//			return decimal.Parse(s.FindNumber());
//		}

//		public static string FindNumber(this string s) {
//			return RegexHelper.GetString(s, "((?:-|)\\d+(?:(?:\\.|,)\\d+)?)");
//		}
//
//		public static string FindInt(this string s) {
//			return RegexHelper.GetString(s, "((?:-|)\\d+)");
//		}
//
//		public static string FindUInt(this string s) {
//			return RegexHelper.GetString(s, "(\\d+)");
//		}

	    public static string RemoveExtension(this string s) {
	        var list = s.Split('.').ToList();
	        if (list.Count > 1 && !string.IsNullOrEmpty(list.Last())) {
	            list.RemoveAt(list.Count - 1);
	            return string.Join(".", list);
	        }
	        return s;
	    }
	}

//	[TestFixture]
//	public class StringExtensionsTest {
//        [TestCase("1.txt","1")]
//        [TestCase(".txt","")]
//        [TestCase("1.1","1")]
//        [TestCase("1.","1.")]
//        [TestCase("1", "1")]
//        [TestCase(".", ".")]
//        public void RemoveExtension(string text, string expected) {
//            Assert.AreEqual(expected,text.RemoveExtension());
//	    }
//
//
//	    [TestCase("-5", "-5")]
//		[TestCase("text -5 text", "-5")]
//		[TestCase("text 1.2 text", "1.2")]
//		[TestCase("text 1,2 text", "1,2")]
//		public void FindNumber(string text, string expected) {
//			Assert.AreEqual(expected, text.FindNumber());
//		}
//
//		[TestCase("-5", -5)]
//		public void AsDecimal(string text, decimal expected) {
//			Assert.AreEqual(expected,text.AsDecimal());
//		}
//
//		[TestCase("1", "1")]
//		[TestCase("text 1 text", "1")]
//		[TestCase("text 1.2 text", "1")]
//		[TestCase("-1", "-1")]
//		[TestCase("text -1 text", "-1")]
//		[TestCase("text -1.2 text", "-1")]
//		public void FindInt(string text, string expected) {
//			Assert.AreEqual(expected, text.FindInt());
//		}
//	}
}
