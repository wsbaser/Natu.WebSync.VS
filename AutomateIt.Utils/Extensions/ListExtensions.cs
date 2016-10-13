using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace automateit.utils.Extensions
{
	public static class ListExtensions
	{

		public static T RandomItem<T>(this List<T> list, params T[] exclude) {
			return list.RandomItem(exclude.ToList());
		}

		public static T RandomItem<T>(this List<T> list, Func<T, bool> excludeCondition) {
			T randomItem;
			var rnd = new Random();
			do {
				if (list.Count == 0)
					throw new Exception("Unable to get random item. There are no available items in list.");
				randomItem = list[rnd.Next(list.Count)];
				if (excludeCondition.Invoke(randomItem)) {
					list.Remove(randomItem);
					continue;
				}
				break;
			} while (true);
			return randomItem;
		}

		public static T RandomItem<T>(this List<T> list, List<T> exclude) {
			return list.RandomItem(exclude.Contains);
		}

		public static List<string> TrimAll(this List<string> list) {
			return list.Select(i => i.Trim()).ToList();
		}

		public static void Shuffle<T>(this List<T> list) {
			var random = new Random();
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = random.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static string AsString(this IEnumerable<string> list, string delimiter = ",") {
			if (!list.Any())
				return string.Empty;
			var s = list.Aggregate(string.Empty, (current, item) => current + (item + delimiter));
			return s.Substring(0, s.Length - 1);
		}
	}

	[TestFixture]
	public class ListExtensionsTest {
		[Test]
		public void RandomItemTest() {
			var list = new List<ClassA> {new ClassA("111"), new ClassA("222"), new ClassA("111")};
			var randomItem = list.RandomItem(new ClassA("111"));
			Assert.AreNotEqual("111", randomItem.Field);
		}

		class ClassA {
			public readonly string Field;

			public ClassA(string field) {
				Field = field;
			}
			public override bool Equals(object obj) {
				return obj is ClassA && (obj as ClassA).Field == Field;
			}
		}
	}
}

