using System;
using System.Collections;
using System.Globalization;
using System.Text;

public static class NewJSON
{
	private static bool IsWhitespace(char c)
	{
		return c == ' ' || c == '\b' || c == '\f' || c == '\n' || c == '\r' || c == '\t';
	}

	private static bool AddHexToInt(char c, ref int n, int s)
	{
		switch (c)
		{
		case '0':
			n |= 0 << s;
			return true;
		case '1':
			n |= 1 << s;
			return true;
		case '2':
			n |= 2 << s;
			return true;
		case '3':
			n |= 3 << s;
			return true;
		case '4':
			n |= 4 << s;
			return true;
		case '5':
			n |= 5 << s;
			return true;
		case '6':
			n |= 6 << s;
			return true;
		case '7':
			n |= 7 << s;
			return true;
		case '8':
			n |= 8 << s;
			return true;
		case '9':
			n |= 9 << s;
			return true;
		case ':':
		case ';':
		case '<':
		case '=':
		case '>':
		case '?':
		case '@':
			IL_67:
			switch (c)
			{
			case 'a':
				n |= 10 << s;
				return true;
			case 'b':
				n |= 11 << s;
				return true;
			case 'c':
				n |= 12 << s;
				return true;
			case 'd':
				n |= 13 << s;
				return true;
			case 'e':
				n |= 14 << s;
				return true;
			case 'f':
				n |= 15 << s;
				return true;
			default:
				return false;
			}
			break;
		case 'A':
			n |= 10 << s;
			return true;
		case 'B':
			n |= 11 << s;
			return true;
		case 'C':
			n |= 12 << s;
			return true;
		case 'D':
			n |= 13 << s;
			return true;
		case 'E':
			n |= 14 << s;
			return true;
		case 'F':
			n |= 15 << s;
			return true;
		}
		goto IL_67;
	}

	private static object DecodeString(char[] buffer, ref int p, int length)
	{
		int num = 0;
		int num2 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		while (p < length)
		{
			char c = buffer[p++];
			int num3 = num;
			switch (num3)
			{
			case 10:
				if (!NewJSON.AddHexToInt(c, ref num2, 12))
				{
					return false;
				}
				num = 11;
				break;
			case 11:
				if (!NewJSON.AddHexToInt(c, ref num2, 8))
				{
					return false;
				}
				num = 12;
				break;
			case 12:
				if (!NewJSON.AddHexToInt(c, ref num2, 4))
				{
					return false;
				}
				num = 13;
				break;
			case 13:
				if (!NewJSON.AddHexToInt(c, ref num2, 0))
				{
					return false;
				}
				stringBuilder.Append((char)num2);
				num = 0;
				break;
			default:
				if (num3 != 0)
				{
					if (num3 != 1)
					{
						return null;
					}
					if (c == '"' || c == '\\' || c == '/')
					{
						stringBuilder.Append(c);
						num = 0;
					}
					else if (c == 'b')
					{
						stringBuilder.Append('\b');
						num = 0;
					}
					else if (c == 'f')
					{
						stringBuilder.Append('\f');
						num = 0;
					}
					else if (c == 'n')
					{
						stringBuilder.Append('\n');
						num = 0;
					}
					else if (c == 'r')
					{
						stringBuilder.Append('\r');
						num = 0;
					}
					else if (c == 't')
					{
						stringBuilder.Append('\t');
						num = 0;
					}
					else if (c == 'u')
					{
						num2 = 0;
						num = 10;
					}
				}
				else
				{
					if (c == '"')
					{
						return stringBuilder.ToString();
					}
					if (c == '\\')
					{
						num = 1;
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				break;
			}
		}
		return null;
	}

	private static object DecodeNumber(char[] buffer, ref int p, int length)
	{
		int num = 0;
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder();
		while (p < length)
		{
			char c = buffer[p++];
			stringBuilder.Append(c);
			switch (num)
			{
			case 0:
				if (c == '0')
				{
					num = 2;
				}
				else if (c >= '1' && c <= '9')
				{
					num = 4;
				}
				else
				{
					if (c != '-')
					{
						return null;
					}
					num = 1;
				}
				continue;
			case 1:
				if (c == '0')
				{
					num = 2;
				}
				else
				{
					if (c < '1' || c > '9')
					{
						return null;
					}
					num = 4;
				}
				continue;
			case 2:
				if (c == '.')
				{
					num = 8;
					continue;
				}
				break;
			case 3:
				if (c >= '0' && c <= '9')
				{
					num = 3;
				}
				else
				{
					if (c != 'e' && c != 'E')
					{
						break;
					}
					num = 5;
				}
				continue;
			case 4:
				if (c == '.')
				{
					num = 8;
				}
				else if (c >= '0' && c <= '9')
				{
					num = 4;
				}
				else
				{
					if (c != 'e' && c != 'E')
					{
						break;
					}
					num = 5;
				}
				continue;
			case 5:
				if (c >= '0' && c <= '9')
				{
					num = 7;
				}
				else
				{
					if (c != '+' && c != '-')
					{
						return null;
					}
					num = 6;
				}
				continue;
			case 6:
				if (c >= '0' && c <= '9')
				{
					num = 7;
					continue;
				}
				return null;
			case 7:
				if (c >= '0' && c <= '9')
				{
					num = 7;
					continue;
				}
				break;
			case 8:
				if (c >= '0' && c <= '9')
				{
					num = 3;
					continue;
				}
				return null;
			default:
				continue;
			}
			IL_1F5:
			if (!flag)
			{
				p--;
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			double num2;
			if (!double.TryParse(stringBuilder.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out num2))
			{
				return null;
			}
			return num2;
		}
		flag = true;
		goto IL_1F5;
	}

	private static object DecodeObject(char[] buffer, ref int p, int length)
	{
		int num = 0;
		object key = null;
		Hashtable hashtable = new Hashtable();
		while (p < length)
		{
			char c = buffer[p++];
			switch (num)
			{
			case 0:
				if (c == '"')
				{
					if ((key = NewJSON.DecodeString(buffer, ref p, length)) == null)
					{
						return null;
					}
					num = 1;
				}
				else
				{
					if (c == '}')
					{
						return hashtable;
					}
					if (!NewJSON.IsWhitespace(c))
					{
						return null;
					}
				}
				break;
			case 1:
				if (c == ':')
				{
					num = 2;
				}
				else if (!NewJSON.IsWhitespace(c))
				{
					return null;
				}
				break;
			case 2:
				p--;
				hashtable.Add(key, NewJSON.DecodeValue(buffer, ref p, length));
				num = 3;
				break;
			case 3:
				if (c == ',')
				{
					num = 0;
				}
				else
				{
					if (c == '}')
					{
						return hashtable;
					}
					if (!NewJSON.IsWhitespace(c))
					{
						return null;
					}
				}
				break;
			default:
				return null;
			}
		}
		return null;
	}

	private static object DecodeArray(char[] buffer, ref int p, int length)
	{
		int num = 0;
		ArrayList arrayList = new ArrayList();
		while (p < length)
		{
			char c = buffer[p++];
			int num2 = num;
			if (num2 != 0)
			{
				if (num2 != 1)
				{
					return null;
				}
				if (c == ',')
				{
					num = 0;
				}
				else
				{
					if (c == ']')
					{
						return arrayList;
					}
					if (!NewJSON.IsWhitespace(c))
					{
						return null;
					}
				}
			}
			else
			{
				if (c == ']')
				{
					return arrayList;
				}
				if (!NewJSON.IsWhitespace(c))
				{
					p--;
					arrayList.Add(NewJSON.DecodeValue(buffer, ref p, length));
					num = 1;
				}
			}
		}
		return null;
	}

	private static object DecodeValue(char[] buffer, ref int p, int length)
	{
		int num = 0;
		while (p < length)
		{
			char c = buffer[p++];
			int num2 = num;
			switch (num2)
			{
			case 10:
				if (c == 'r')
				{
					num = 11;
					continue;
				}
				return null;
			case 11:
				if (c == 'u')
				{
					num = 12;
					continue;
				}
				return null;
			case 12:
				if (c == 'e')
				{
					return true;
				}
				return null;
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
				IL_56:
				switch (num2)
				{
				case 30:
					if (c == 'u')
					{
						num = 31;
						continue;
					}
					return null;
				case 31:
					if (c == 'l')
					{
						num = 32;
						continue;
					}
					return null;
				case 32:
					if (c == 'l')
					{
						return null;
					}
					return null;
				default:
					if (num2 != 0)
					{
						return null;
					}
					if (c == '"')
					{
						return NewJSON.DecodeString(buffer, ref p, length);
					}
					if (c == '-' || (c >= '0' && c <= '9'))
					{
						p--;
						return NewJSON.DecodeNumber(buffer, ref p, length);
					}
					if (c == '{')
					{
						return NewJSON.DecodeObject(buffer, ref p, length);
					}
					if (c == '[')
					{
						return NewJSON.DecodeArray(buffer, ref p, length);
					}
					if (c == 't')
					{
						num = 10;
					}
					else if (c == 'f')
					{
						num = 20;
					}
					else if (c == 'n')
					{
						num = 30;
					}
					else if (!NewJSON.IsWhitespace(c))
					{
						return null;
					}
					continue;
				}
				break;
			case 20:
				if (c == 'a')
				{
					num = 21;
					continue;
				}
				return null;
			case 21:
				if (c == 'l')
				{
					num = 22;
					continue;
				}
				return null;
			case 22:
				if (c == 's')
				{
					num = 23;
					continue;
				}
				return null;
			case 23:
				if (c == 'e')
				{
					return false;
				}
				return null;
			}
			goto IL_56;
		}
		return null;
	}

	private static void EncodeString(StringBuilder encodedString, string s)
	{
		encodedString.Append('"');
		for (int i = 0; i < s.Length; i++)
		{
			char c = s[i];
			if (c == '"')
			{
				encodedString.Append("\\\"");
			}
			else if (c == '\\')
			{
				encodedString.Append("\\\\");
			}
			else if (c == '/')
			{
				encodedString.Append("\\/");
			}
			else if (c == '\b')
			{
				encodedString.Append("\\b");
			}
			else if (c == '\f')
			{
				encodedString.Append("\\f");
			}
			else if (c == '\n')
			{
				encodedString.Append("\\n");
			}
			else if (c == '\r')
			{
				encodedString.Append("\\r");
			}
			else if (c == '\t')
			{
				encodedString.Append("\\t");
			}
			else if (c > '\u007f')
			{
				encodedString.Append("\\u");
				int num = (int)c;
				encodedString.Append(num.ToString("x4"));
			}
			else
			{
				encodedString.Append(c);
			}
		}
		encodedString.Append('"');
	}

	private static void EncodeNumber(StringBuilder encodedString, double number)
	{
		encodedString.Append(number);
	}

	private static void EncodeObject(StringBuilder encodedString, Hashtable collection)
	{
		bool flag = true;
		encodedString.Append('{');
		foreach (object current in collection.Keys)
		{
			if (!flag)
			{
				encodedString.Append(',');
			}
			NewJSON.EncodeString(encodedString, current.ToString());
			encodedString.Append(':');
			NewJSON.EncodeValue(encodedString, collection[current]);
			flag = false;
		}
		encodedString.Append('}');
	}

	private static void EncodeArray(StringBuilder encodedString, IEnumerable array)
	{
		bool flag = true;
		encodedString.Append('[');
		foreach (object current in array)
		{
			if (!flag)
			{
				encodedString.Append(',');
			}
			NewJSON.EncodeValue(encodedString, current);
			flag = false;
		}
		encodedString.Append(']');
	}

	private static void EncodeValue(StringBuilder encodedString, object JsonObject)
	{
		if (JsonObject == null)
		{
			encodedString.Append("null");
		}
		else if (JsonObject.GetType().IsArray)
		{
			NewJSON.EncodeArray(encodedString, (IEnumerable)JsonObject);
		}
		else if (JsonObject is ArrayList)
		{
			NewJSON.EncodeArray(encodedString, (IEnumerable)JsonObject);
		}
		else if (JsonObject is string)
		{
			NewJSON.EncodeString(encodedString, (string)JsonObject);
		}
		else if (JsonObject is Hashtable)
		{
			NewJSON.EncodeObject(encodedString, (Hashtable)JsonObject);
		}
		else if (JsonObject is bool)
		{
			if ((bool)JsonObject)
			{
				encodedString.Append("true");
			}
			else
			{
				encodedString.Append("false");
			}
		}
		else if (JsonObject.GetType().IsPrimitive)
		{
			NewJSON.EncodeNumber(encodedString, Convert.ToDouble(JsonObject));
		}
		else
		{
			encodedString.Append("null");
		}
	}

	public static string JsonEncode(object JsonObject)
	{
		StringBuilder stringBuilder = new StringBuilder();
		NewJSON.EncodeValue(stringBuilder, JsonObject);
		return stringBuilder.ToString();
	}

	public static object JsonDecode(string JsonString)
	{
		int num = 0;
		char[] array = JsonString.ToCharArray();
		return NewJSON.DecodeValue(array, ref num, array.Length);
	}
}
