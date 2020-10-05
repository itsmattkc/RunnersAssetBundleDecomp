using System;
using System.Text;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class Serializer
	{
		private static string k_Indent = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";

		public static PBXElementDict ParseTreeAST(TreeAST ast, TokenList tokens, string text)
		{
			PBXElementDict pBXElementDict = new PBXElementDict();
			foreach (KeyValueAST current in ast.values)
			{
				PBXElementString pBXElementString = Serializer.ParseIdentifierAST(current.key, tokens, text);
				PBXElement value = Serializer.ParseValueAST(current.value, tokens, text);
				pBXElementDict[pBXElementString.value] = value;
			}
			return pBXElementDict;
		}

		public static PBXElementArray ParseArrayAST(ArrayAST ast, TokenList tokens, string text)
		{
			PBXElementArray pBXElementArray = new PBXElementArray();
			foreach (ValueAST current in ast.values)
			{
				pBXElementArray.values.Add(Serializer.ParseValueAST(current, tokens, text));
			}
			return pBXElementArray;
		}

		public static PBXElement ParseValueAST(ValueAST ast, TokenList tokens, string text)
		{
			if (ast is TreeAST)
			{
				return Serializer.ParseTreeAST((TreeAST)ast, tokens, text);
			}
			if (ast is ArrayAST)
			{
				return Serializer.ParseArrayAST((ArrayAST)ast, tokens, text);
			}
			if (ast is IdentifierAST)
			{
				return Serializer.ParseIdentifierAST((IdentifierAST)ast, tokens, text);
			}
			return null;
		}

		public static PBXElementString ParseIdentifierAST(IdentifierAST ast, TokenList tokens, string text)
		{
			Token token = tokens[ast.value];
			TokenType type = token.type;
			string text2;
			if (type == TokenType.String)
			{
				text2 = text.Substring(token.begin, token.end - token.begin);
				return new PBXElementString(text2);
			}
			if (type != TokenType.QuotedString)
			{
				throw new Exception("Internal parser error");
			}
			text2 = text.Substring(token.begin, token.end - token.begin);
			text2 = PBXStream.UnquoteString(text2);
			return new PBXElementString(text2);
		}

		private static string GetIndent(int indent)
		{
			return Serializer.k_Indent.Substring(0, indent);
		}

		private static void WriteStringImpl(StringBuilder sb, string s, bool comment, GUIDToCommentMap comments)
		{
			if (comment)
			{
				sb.Append(comments.Write(s));
			}
			else
			{
				sb.Append(PBXStream.QuoteStringIfNeeded(s));
			}
		}

		public static void WriteDictKeyValue(StringBuilder sb, string key, PBXElement value, int indent, bool compact, PropertyCommentChecker checker, GUIDToCommentMap comments)
		{
			if (!compact)
			{
				sb.Append("\n");
				sb.Append(Serializer.GetIndent(indent));
			}
			Serializer.WriteStringImpl(sb, key, checker.CheckKeyInDict(key), comments);
			sb.Append(" = ");
			if (value is PBXElementString)
			{
				Serializer.WriteStringImpl(sb, value.AsString(), checker.CheckStringValueInDict(key, value.AsString()), comments);
			}
			else if (value is PBXElementDict)
			{
				Serializer.WriteDict(sb, value.AsDict(), indent, compact, checker.NextLevel(key), comments);
			}
			else if (value is PBXElementArray)
			{
				Serializer.WriteArray(sb, value.AsArray(), indent, compact, checker.NextLevel(key), comments);
			}
			sb.Append(";");
			if (compact)
			{
				sb.Append(" ");
			}
		}

		public static void WriteDict(StringBuilder sb, PBXElementDict el, int indent, bool compact, PropertyCommentChecker checker, GUIDToCommentMap comments)
		{
			sb.Append("{");
			if (el.Contains("isa"))
			{
				Serializer.WriteDictKeyValue(sb, "isa", el["isa"], indent + 1, compact, checker, comments);
			}
			foreach (string current in el.values.Keys)
			{
				if (current != "isa")
				{
					Serializer.WriteDictKeyValue(sb, current, el[current], indent + 1, compact, checker, comments);
				}
			}
			if (!compact)
			{
				sb.Append("\n");
				sb.Append(Serializer.GetIndent(indent));
			}
			sb.Append("}");
		}

		public static void WriteArray(StringBuilder sb, PBXElementArray el, int indent, bool compact, PropertyCommentChecker checker, GUIDToCommentMap comments)
		{
			sb.Append("(");
			foreach (PBXElement current in el.values)
			{
				if (!compact)
				{
					sb.Append("\n");
					sb.Append(Serializer.GetIndent(indent + 1));
				}
				if (current is PBXElementString)
				{
					Serializer.WriteStringImpl(sb, current.AsString(), checker.CheckStringValueInArray(current.AsString()), comments);
				}
				else if (current is PBXElementDict)
				{
					Serializer.WriteDict(sb, current.AsDict(), indent + 1, compact, checker.NextLevel("*"), comments);
				}
				else if (current is PBXElementArray)
				{
					Serializer.WriteArray(sb, current.AsArray(), indent + 1, compact, checker.NextLevel("*"), comments);
				}
				sb.Append(",");
				if (compact)
				{
					sb.Append(" ");
				}
			}
			if (!compact)
			{
				sb.Append("\n");
				sb.Append(Serializer.GetIndent(indent));
			}
			sb.Append(")");
		}
	}
}
