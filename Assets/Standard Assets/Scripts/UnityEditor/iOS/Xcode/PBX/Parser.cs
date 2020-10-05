using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class Parser
	{
		private TokenList tokens;

		private int currPos;

		public Parser(TokenList tokens)
		{
			this.tokens = tokens;
			this.currPos = this.SkipComments(0);
		}

		private int SkipComments(int pos)
		{
			while (pos < this.tokens.Count && this.tokens[pos].type == TokenType.Comment)
			{
				pos++;
			}
			return pos;
		}

		private int IncInternal(int pos)
		{
			if (pos >= this.tokens.Count)
			{
				return pos;
			}
			pos++;
			return this.SkipComments(pos);
		}

		private int Inc()
		{
			int result = this.currPos;
			this.currPos = this.IncInternal(this.currPos);
			return result;
		}

		private TokenType Tok()
		{
			if (this.currPos >= this.tokens.Count)
			{
				return TokenType.EOF;
			}
			return this.tokens[this.currPos].type;
		}

		private void SkipIf(TokenType type)
		{
			if (this.Tok() == type)
			{
				this.Inc();
			}
		}

		private string GetErrorMsg()
		{
			return "Invalid PBX project (parsing line " + this.tokens[this.currPos].line + ")";
		}

		public IdentifierAST ParseIdentifier()
		{
			if (this.Tok() != TokenType.String && this.Tok() != TokenType.QuotedString)
			{
				throw new Exception(this.GetErrorMsg());
			}
			return new IdentifierAST
			{
				value = this.Inc()
			};
		}

		public TreeAST ParseTree()
		{
			if (this.Tok() != TokenType.LBrace)
			{
				throw new Exception(this.GetErrorMsg());
			}
			this.Inc();
			TreeAST treeAST = new TreeAST();
			while (this.Tok() != TokenType.RBrace && this.Tok() != TokenType.EOF)
			{
				treeAST.values.Add(this.ParseKeyValue());
			}
			this.SkipIf(TokenType.RBrace);
			return treeAST;
		}

		public ArrayAST ParseList()
		{
			if (this.Tok() != TokenType.LParen)
			{
				throw new Exception(this.GetErrorMsg());
			}
			this.Inc();
			ArrayAST arrayAST = new ArrayAST();
			while (this.Tok() != TokenType.RParen && this.Tok() != TokenType.EOF)
			{
				arrayAST.values.Add(this.ParseValue());
				this.SkipIf(TokenType.Comma);
			}
			this.SkipIf(TokenType.RParen);
			return arrayAST;
		}

		public KeyValueAST ParseKeyValue()
		{
			KeyValueAST keyValueAST = new KeyValueAST();
			keyValueAST.key = this.ParseIdentifier();
			if (this.Tok() != TokenType.Eq)
			{
				throw new Exception(this.GetErrorMsg());
			}
			this.Inc();
			keyValueAST.value = this.ParseValue();
			this.SkipIf(TokenType.Semicolon);
			return keyValueAST;
		}

		public ValueAST ParseValue()
		{
			if (this.Tok() == TokenType.String || this.Tok() == TokenType.QuotedString)
			{
				return this.ParseIdentifier();
			}
			if (this.Tok() == TokenType.LBrace)
			{
				return this.ParseTree();
			}
			if (this.Tok() == TokenType.LParen)
			{
				return this.ParseList();
			}
			throw new Exception(this.GetErrorMsg());
		}
	}
}
