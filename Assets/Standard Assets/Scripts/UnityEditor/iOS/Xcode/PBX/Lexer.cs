using System;
using System.Linq;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class Lexer
	{
		private string text;

		private int pos;

		private int length;

		private int line;

		public static TokenList Tokenize(string text)
		{
			Lexer lexer = new Lexer();
			lexer.SetText(text);
			return lexer.ScanAll();
		}

		public void SetText(string text)
		{
			this.text = text + "    ";
			this.pos = 0;
			this.length = text.Length;
			this.line = 0;
		}

		public TokenList ScanAll()
		{
			TokenList tokenList = new TokenList();
			Token token;
			do
			{
				token = new Token();
				this.ScanOne(token);
				tokenList.Add(token);
			}
			while (token.type != TokenType.EOF);
			return tokenList;
		}

		private void UpdateNewlineStats(char ch)
		{
			if (ch == '\n')
			{
				this.line++;
			}
		}

		private void ScanOne(Token tok)
		{
			while (this.pos < this.length && char.IsWhiteSpace(this.text[this.pos]))
			{
				this.UpdateNewlineStats(this.text[this.pos]);
				this.pos++;
			}
			if (this.pos >= this.length)
			{
				tok.type = TokenType.EOF;
				return;
			}
			char c = this.text[this.pos];
			char c2 = this.text[this.pos + 1];
			if (c == '"')
			{
				this.ScanQuotedString(tok);
			}
			else if (c == '/' && c2 == '*')
			{
				this.ScanMultilineComment(tok);
			}
			else if (c == '/' && c2 == '/')
			{
				this.ScanComment(tok);
			}
			else if (this.IsOperator(c))
			{
				this.ScanOperator(tok);
			}
			else
			{
				this.ScanString(tok);
			}
		}

		private void ScanString(Token tok)
		{
			tok.type = TokenType.String;
			tok.begin = this.pos;
			while (this.pos < this.length)
			{
				char c = this.text[this.pos];
				char c2 = this.text[this.pos + 1];
				if (char.IsWhiteSpace(c))
				{
					break;
				}
				if (c == '"')
				{
					break;
				}
				if (c == '/' && c2 == '*')
				{
					break;
				}
				if (c == '/' && c2 == '/')
				{
					break;
				}
				if (this.IsOperator(c))
				{
					break;
				}
				this.pos++;
			}
			tok.end = this.pos;
			tok.line = this.line;
		}

		private void ScanQuotedString(Token tok)
		{
			tok.type = TokenType.QuotedString;
			tok.begin = this.pos;
			this.pos++;
			while (this.pos < this.length)
			{
				if (this.text[this.pos] == '\\' && this.text[this.pos + 1] == '"')
				{
					this.pos += 2;
				}
				else
				{
					if (this.text[this.pos] == '"')
					{
						break;
					}
					this.UpdateNewlineStats(this.text[this.pos]);
					this.pos++;
				}
			}
			this.pos++;
			tok.end = this.pos;
			tok.line = this.line;
		}

		private void ScanMultilineComment(Token tok)
		{
			tok.type = TokenType.Comment;
			tok.begin = this.pos;
			this.pos += 2;
			while (this.pos < this.length)
			{
				if (this.text[this.pos] == '*' && this.text[this.pos + 1] == '/')
				{
					break;
				}
				this.UpdateNewlineStats(this.text[this.pos]);
				this.pos++;
			}
			this.pos += 2;
			tok.end = this.pos;
			tok.line = this.line;
		}

		private void ScanComment(Token tok)
		{
			tok.type = TokenType.Comment;
			tok.begin = this.pos;
			this.pos += 2;
			while (this.pos < this.length)
			{
				if (this.text[this.pos] == '\n')
				{
					break;
				}
				this.pos++;
			}
			this.UpdateNewlineStats(this.text[this.pos]);
			this.pos++;
			tok.end = this.pos;
			tok.line = this.line;
		}

		private bool IsOperator(char ch)
		{
			return ";,=(){}".Contains(ch);
		}

		private void ScanOperator(Token tok)
		{
			char c = this.text[this.pos];
			switch (c)
			{
			case '(':
				this.ScanOperatorSpecific(tok, TokenType.LParen);
				return;
			case ')':
				this.ScanOperatorSpecific(tok, TokenType.RParen);
				return;
			case '*':
			case '+':
				IL_2F:
				switch (c)
				{
				case ';':
					this.ScanOperatorSpecific(tok, TokenType.Semicolon);
					return;
				case '<':
					IL_44:
					switch (c)
					{
					case '{':
						this.ScanOperatorSpecific(tok, TokenType.LBrace);
						return;
					case '}':
						this.ScanOperatorSpecific(tok, TokenType.RBrace);
						return;
					}
					return;
				case '=':
					this.ScanOperatorSpecific(tok, TokenType.Eq);
					return;
				}
				goto IL_44;
			case ',':
				this.ScanOperatorSpecific(tok, TokenType.Comma);
				return;
			}
			goto IL_2F;
		}

		private void ScanOperatorSpecific(Token tok, TokenType type)
		{
			tok.type = type;
			tok.begin = this.pos;
			this.pos++;
			tok.end = this.pos;
			tok.line = this.line;
		}
	}
}
