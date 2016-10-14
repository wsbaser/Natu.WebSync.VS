using System;
using System.Reflection;

namespace RoslynSpike.Utilities.Extensions {
	public static class EnumExtensions {
		public static string StringValue(this Enum value) {
			string output = null;
			Type type = value.GetType();
			FieldInfo fi = type.GetField(value.ToString());
			var attrs = fi.GetCustomAttributes(typeof (StringValueAttribute), false) as StringValueAttribute[];
			if (attrs != null && attrs.Length > 0)
				output = attrs[0].Value;
			return output;
		}
	}

	public class StringValueAttribute : Attribute {
		private readonly string _value;

		public StringValueAttribute(string value) {
			_value = value;
		}

		public string Value {
			get { return _value; }
		}
	}
}