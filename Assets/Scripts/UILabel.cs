using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Label"), ExecuteInEditMode]
public class UILabel : UIWidget
{
	public enum Effect
	{
		None,
		Shadow,
		Outline
	}

	public enum Overflow
	{
		ShrinkContent,
		ClampContent,
		ResizeFreely,
		ResizeHeight
	}

	[HideInInspector, SerializeField]
	private UIFont mFont;

	[HideInInspector, SerializeField]
	private string mText = string.Empty;

	[HideInInspector, SerializeField]
	private bool mEncoding = true;

	[HideInInspector, SerializeField]
	private int mMaxLineCount;

	[HideInInspector, SerializeField]
	private bool mPassword;

	[HideInInspector, SerializeField]
	private bool mShowLastChar;

	[HideInInspector, SerializeField]
	private UILabel.Effect mEffectStyle;

	[HideInInspector, SerializeField]
	private Color mEffectColor = Color.black;

	[HideInInspector, SerializeField]
	private UIFont.SymbolStyle mSymbols = UIFont.SymbolStyle.Uncolored;

	[HideInInspector, SerializeField]
	private Vector2 mEffectDistance = Vector2.one;

	[HideInInspector, SerializeField]
	private UILabel.Overflow mOverflow;

	[HideInInspector, SerializeField]
	private bool mShrinkToFit;

	[HideInInspector, SerializeField]
	private int mMaxLineWidth;

	[HideInInspector, SerializeField]
	private int mMaxLineHeight;

	[HideInInspector, SerializeField]
	private float mLineWidth;

	[HideInInspector, SerializeField]
	private bool mMultiline = true;

	private bool mShouldBeProcessed = true;

	private string mProcessedText;

	private bool mPremultiply;

	private Vector2 mSize = Vector2.zero;

	private float mScale = 1f;

	private int mLastWidth;

	private int mLastHeight;

	private bool hasChanged
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
			}
			else
			{
				this.mShouldBeProcessed = false;
			}
		}
	}

	public override Material material
	{
		get
		{
			return (!(this.mFont != null)) ? null : this.mFont.material;
		}
	}

	public UIFont font
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				if (this.mFont != null && this.mFont.dynamicFont != null)
				{
					Font expr_43 = this.mFont.dynamicFont;
					expr_43.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Remove(expr_43.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
				}
				base.RemoveFromPanel();
				this.mFont = value;
				this.hasChanged = true;
				if (this.mFont != null && this.mFont.dynamicFont != null)
				{
					Font expr_AB = this.mFont.dynamicFont;
					expr_AB.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Combine(expr_AB.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
					this.mFont.Request(this.mText);
				}
				this.MarkAsChanged();
			}
		}
	}

	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = string.Empty;
				}
				this.hasChanged = true;
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.hasChanged = true;
				if (this.mFont != null)
				{
					this.mFont.Request(value);
				}
			}
		}
	}

	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.hasChanged = true;
				if (value)
				{
					this.mPassword = false;
				}
			}
		}
	}

	public UIFont.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.hasChanged = true;
			}
		}
	}

	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.hasChanged = true;
			}
		}
	}

	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = ((!value) ? 1 : 0);
				this.hasChanged = true;
				if (value)
				{
					this.mPassword = false;
				}
			}
		}
	}

	public override Vector3[] localCorners
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return base.localCorners;
		}
	}

	public override Vector3[] worldCorners
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return base.worldCorners;
		}
	}

	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				if (value != 1)
				{
					this.mPassword = false;
				}
				this.hasChanged = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	public bool password
	{
		get
		{
			return this.mPassword;
		}
		set
		{
			if (this.mPassword != value)
			{
				if (value)
				{
					this.mMaxLineCount = 1;
					this.mEncoding = false;
				}
				this.mPassword = value;
				this.hasChanged = true;
			}
		}
	}

	public bool showLastPasswordChar
	{
		get
		{
			return this.mShowLastChar;
		}
		set
		{
			if (this.mShowLastChar != value)
			{
				this.mShowLastChar = value;
				this.hasChanged = true;
			}
		}
	}

	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.hasChanged = true;
			}
		}
	}

	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (!this.mEffectColor.Equals(value))
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.hasChanged = true;
				}
			}
		}
	}

	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.hasChanged = true;
			}
		}
	}

	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	public Vector2 printedSize
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return this.mSize;
		}
	}

	public override Vector2 localSize
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return base.localSize;
		}
	}

	protected override void OnEnable()
	{
		if (this.mFont != null && this.mFont.dynamicFont != null)
		{
			Font expr_32 = this.mFont.dynamicFont;
			expr_32.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Combine(expr_32.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
		}
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		if (this.mFont != null && this.mFont.dynamicFont != null)
		{
			Font expr_32 = this.mFont.dynamicFont;
			expr_32.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Remove(expr_32.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
		}
		base.OnDisable();
	}

	protected override void UpgradeFrom265()
	{
		this.ProcessText(true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth != 0)
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = ((this.mMaxLineCount <= 0) ? UILabel.Overflow.ShrinkContent : UILabel.Overflow.ResizeHeight);
		}
		else
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int num = Mathf.RoundToInt((float)this.mFont.size * this.mFont.pixelSize);
			if (base.height < num)
			{
				base.height = num;
			}
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		if (base.GetComponent<BoxCollider>() != null)
		{
			NGUITools.AddWidgetCollider(base.gameObject, true);
		}
	}

	protected override void OnStart()
	{
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = (this.font != null && this.font.material != null && this.font.material.shader.name.Contains("Premultiplied"));
		if (this.mFont != null)
		{
			this.mFont.Request(this.mText);
		}
	}

	public override void MarkAsChanged()
	{
		this.hasChanged = true;
		base.MarkAsChanged();
	}

	private void ProcessText()
	{
		this.ProcessText(false);
	}

	private void ProcessText(bool legacyMode)
	{
		if (this.mFont == null)
		{
			return;
		}
		this.mChanged = true;
		this.hasChanged = false;
		float num = 1f / this.mFont.pixelSize;
		float num2 = Mathf.Abs((!legacyMode) ? ((float)this.mFont.size) : base.cachedTransform.localScale.x);
		float num3 = (!legacyMode) ? ((float)base.width * num) : ((this.mMaxLineWidth == 0) ? 1000000f : ((float)this.mMaxLineWidth * num));
		float num4 = (!legacyMode) ? ((float)base.height * num) : ((this.mMaxLineHeight == 0) ? 1000000f : ((float)this.mMaxLineHeight * num));
		if (num2 > 0f)
		{
			while (true)
			{
				this.mScale = num2 / (float)this.mFont.size;
				bool flag = true;
				int width = (this.mOverflow != UILabel.Overflow.ResizeFreely) ? Mathf.RoundToInt(num3 / this.mScale) : 100000;
				int height = (this.mOverflow != UILabel.Overflow.ResizeFreely && this.mOverflow != UILabel.Overflow.ResizeHeight) ? Mathf.RoundToInt(num4 / this.mScale) : 100000;
				if (this.mPassword)
				{
					this.mProcessedText = string.Empty;
					if (this.mShowLastChar)
					{
						int i = 0;
						int num5 = this.mText.Length - 1;
						while (i < num5)
						{
							this.mProcessedText += "*";
							i++;
						}
						if (this.mText.Length > 0)
						{
							this.mProcessedText += this.mText[this.mText.Length - 1];
						}
					}
					else
					{
						int j = 0;
						int length = this.mText.Length;
						while (j < length)
						{
							this.mProcessedText += "*";
							j++;
						}
					}
					flag = this.mFont.WrapText(this.mProcessedText, out this.mProcessedText, width, height, this.mMaxLineCount, false, UIFont.SymbolStyle.None);
				}
				else if (num3 > 0f || num4 > 0f)
				{
					flag = this.mFont.WrapText(this.mText, out this.mProcessedText, width, height, this.mMaxLineCount, this.mEncoding, this.mSymbols);
				}
				else
				{
					this.mProcessedText = this.mText;
				}
				this.mSize = (string.IsNullOrEmpty(this.mProcessedText) ? Vector2.zero : this.mFont.CalculatePrintedSize(this.mProcessedText, this.mEncoding, this.mSymbols));
				if (this.mOverflow == UILabel.Overflow.ResizeFreely)
				{
					break;
				}
				if (this.mOverflow == UILabel.Overflow.ResizeHeight)
				{
					goto Block_17;
				}
				if (this.mOverflow != UILabel.Overflow.ShrinkContent || flag)
				{
					goto IL_395;
				}
				num2 = Mathf.Round(num2 - 1f);
				if (num2 <= 1f)
				{
					goto IL_395;
				}
			}
			this.mWidth = Mathf.RoundToInt(this.mSize.x * this.mFont.pixelSize);
			this.mHeight = Mathf.RoundToInt(this.mSize.y * this.mFont.pixelSize);
			goto IL_395;
			Block_17:
			this.mHeight = Mathf.RoundToInt(this.mSize.y * this.mFont.pixelSize);
			IL_395:
			if (legacyMode)
			{
				base.width = Mathf.RoundToInt(this.mSize.x * this.mFont.pixelSize);
				base.height = Mathf.RoundToInt(this.mSize.y * this.mFont.pixelSize);
				base.cachedTransform.localScale = Vector3.one;
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = string.Empty;
			this.mScale = 1f;
		}
	}

	public override void MakePixelPerfect()
	{
		if (this.font != null)
		{
			float pixelSize = this.font.pixelSize;
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = Vector3.one;
			if (this.mOverflow == UILabel.Overflow.ResizeFreely)
			{
				this.AssumeNaturalSize();
			}
			else
			{
				UILabel.Overflow overflow = this.mOverflow;
				this.mOverflow = UILabel.Overflow.ShrinkContent;
				this.ProcessText(false);
				this.mOverflow = overflow;
				int num = Mathf.RoundToInt(this.mSize.x * pixelSize);
				int num2 = Mathf.RoundToInt(this.mSize.y * pixelSize);
				if (base.width < num)
				{
					base.width = num;
				}
				if (base.height < num2)
				{
					base.height = num2;
				}
			}
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	public void AssumeNaturalSize()
	{
		if (this.font != null)
		{
			this.ProcessText(false);
			float pixelSize = this.font.pixelSize;
			int num = Mathf.RoundToInt(this.mSize.x * pixelSize);
			int num2 = Mathf.RoundToInt(this.mSize.y * pixelSize);
			if (base.width < num)
			{
				base.width = num;
			}
			if (base.height < num2)
			{
				base.height = num2;
			}
		}
	}

	private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = this.mEffectColor;
		color.a *= base.alpha * this.mPanel.alpha;
		Color32 color2 = (!this.font.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			cols.buffer[i] = color2;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mFont == null)
		{
			return;
		}
		UIWidget.Pivot pivot = base.pivot;
		int start = verts.size;
		Color c = base.color;
		c.a *= this.mPanel.alpha;
		if (this.font.premultipliedAlpha)
		{
			c = NGUITools.ApplyPMA(c);
		}
		string processedText = this.processedText;
		float num = this.mScale * this.mFont.pixelSize;
		int lineWidth = Mathf.RoundToInt((float)base.width / num);
		int size = verts.size;
		if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
		{
			this.mFont.Print(processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Left, lineWidth, this.mPremultiply);
		}
		else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
		{
			this.mFont.Print(processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Right, lineWidth, this.mPremultiply);
		}
		else
		{
			this.mFont.Print(processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Center, lineWidth, this.mPremultiply);
		}
		Vector2 pivotOffset = base.pivotOffset;
		float num2 = Mathf.Lerp(0f, (float)(-(float)this.mWidth), pivotOffset.x);
		float num3 = Mathf.Lerp((float)this.mHeight, 0f, pivotOffset.y);
		num3 += Mathf.Lerp(this.mSize.y * num - (float)this.mHeight, 0f, pivotOffset.y);
		if (num == 1f)
		{
			for (int i = size; i < verts.size; i++)
			{
				Vector3[] expr_1C7_cp_0 = verts.buffer;
				int expr_1C7_cp_1 = i;
				expr_1C7_cp_0[expr_1C7_cp_1].x = expr_1C7_cp_0[expr_1C7_cp_1].x + num2;
				Vector3[] expr_1E2_cp_0 = verts.buffer;
				int expr_1E2_cp_1 = i;
				expr_1E2_cp_0[expr_1E2_cp_1].y = expr_1E2_cp_0[expr_1E2_cp_1].y + num3;
			}
		}
		else
		{
			for (int j = size; j < verts.size; j++)
			{
				verts.buffer[j].x = num2 + verts.buffer[j].x * num;
				verts.buffer[j].y = num3 + verts.buffer[j].y * num;
			}
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int size2 = verts.size;
			float pixelSize = this.mFont.pixelSize;
			num2 = pixelSize * this.mEffectDistance.x;
			num3 = pixelSize * this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, start, size2, num2, -num3);
			if (this.effectStyle == UILabel.Effect.Outline)
			{
				start = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, start, size2, -num2, num3);
				start = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, start, size2, num2, num3);
				start = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, start, size2, -num2, -num3);
			}
		}
	}
}
