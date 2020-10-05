using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	public enum Style
	{
		Text,
		Chat
	}

	protected class Paragraph
	{
		public string text;

		public string[] lines;
	}

	public UITextList.Style style;

	public UILabel textLabel;

	public float maxHeight;

	public int maxEntries = 50;

	public bool supportScrollWheel = true;

	protected char[] mSeparator = new char[]
	{
		'\n'
	};

	protected List<UITextList.Paragraph> mParagraphs = new List<UITextList.Paragraph>();

	protected float mScroll;

	protected bool mSelected;

	protected int mTotalLines;

	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	public void Add(string text)
	{
		this.Add(text, true);
	}

	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph paragraph;
		if (this.mParagraphs.Count < this.maxEntries)
		{
			paragraph = new UITextList.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		if (this.textLabel != null && this.textLabel.font != null)
		{
			string text2;
			this.textLabel.font.WrapText(paragraph.text, out text2, this.textLabel.width, 100000, 0, this.textLabel.supportEncoding, this.textLabel.symbolStyle);
			paragraph.lines = text2.Split(this.mSeparator);
			this.mTotalLines = 0;
			int i = 0;
			int count = this.mParagraphs.Count;
			while (i < count)
			{
				this.mTotalLines += this.mParagraphs[i].lines.Length;
				i++;
			}
		}
		if (updateVisible)
		{
			this.UpdateVisibleText();
		}
	}

	private void Awake()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		Collider collider = GetComponent<Collider>();
		if (collider != null && this.maxHeight <= 0f)
		{
			this.maxHeight = collider.bounds.size.y / base.transform.lossyScale.y;
		}
	}

	private void OnSelect(bool selected)
	{
		this.mSelected = selected;
	}

	protected void UpdateVisibleText()
	{
		if (this.textLabel != null)
		{
			UIFont font = this.textLabel.font;
			if (font != null)
			{
				int num = 0;
				int num2 = (this.maxHeight <= 0f) ? 100000 : Mathf.FloorToInt(this.maxHeight / ((float)this.textLabel.font.size * this.textLabel.font.pixelSize));
				int num3 = Mathf.RoundToInt(this.mScroll);
				if (num2 + num3 > this.mTotalLines)
				{
					num3 = Mathf.Max(0, this.mTotalLines - num2);
					this.mScroll = (float)num3;
				}
				if (this.style == UITextList.Style.Chat)
				{
					num3 = Mathf.Max(0, this.mTotalLines - num2 - num3);
				}
				StringBuilder stringBuilder = new StringBuilder();
				int i = 0;
				int count = this.mParagraphs.Count;
				while (i < count)
				{
					UITextList.Paragraph paragraph = this.mParagraphs[i];
					int j = 0;
					int num4 = paragraph.lines.Length;
					while (j < num4)
					{
						string value = paragraph.lines[j];
						if (num3 > 0)
						{
							num3--;
						}
						else
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append("\n");
							}
							stringBuilder.Append(value);
							num++;
							if (num >= num2)
							{
								break;
							}
						}
						j++;
					}
					if (num >= num2)
					{
						break;
					}
					i++;
				}
				this.textLabel.text = stringBuilder.ToString();
			}
		}
	}

	private void OnScroll(float val)
	{
		if (this.mSelected && this.supportScrollWheel)
		{
			val *= ((this.style != UITextList.Style.Chat) ? -10f : 10f);
			this.mScroll = Mathf.Max(0f, this.mScroll + val);
			this.UpdateVisibleText();
		}
	}
}
