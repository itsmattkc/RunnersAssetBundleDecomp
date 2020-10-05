using System;

namespace Text
{
	public class TextObject
	{
		private string m_text = string.Empty;

		public string text
		{
			get
			{
				return this.m_text;
			}
			set
			{
				this.m_text = value;
			}
		}

		public TextObject(string text)
		{
			this.m_text = text;
		}

		public void ReplaceTag(string tagString, string replaceString)
		{
			if (tagString == null)
			{
				return;
			}
			if (replaceString == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.m_text))
			{
				return;
			}
			this.m_text = this.m_text.Replace(tagString, replaceString);
		}
	}
}
