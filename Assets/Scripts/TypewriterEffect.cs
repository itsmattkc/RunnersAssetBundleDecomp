using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Typewriter Effect"), RequireComponent(typeof(UILabel))]
public class TypewriterEffect : MonoBehaviour
{
	public int charsPerSecond = 40;

	private UILabel mLabel;

	private string mText;

	private int mOffset;

	private float mNextChar;

	private void Update()
	{
		if (this.mLabel == null)
		{
			this.mLabel = base.GetComponent<UILabel>();
			this.mLabel.supportEncoding = false;
			this.mLabel.symbolStyle = UIFont.SymbolStyle.None;
			this.mLabel.font.WrapText(this.mLabel.text, out this.mText, this.mLabel.width, this.mLabel.height, this.mLabel.maxLineCount, false, UIFont.SymbolStyle.None);
		}
		if (this.mOffset < this.mText.Length)
		{
			if (this.mNextChar <= Time.time)
			{
				this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
				float num = 1f / (float)this.charsPerSecond;
				char c = this.mText[this.mOffset];
				if (c == '.' || c == '\n' || c == '!' || c == '?')
				{
					num *= 4f;
				}
				this.mNextChar = Time.time + num;
				this.mLabel.text = this.mText.Substring(0, ++this.mOffset);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}
}
