using System;
using UnityEngine;

[Serializable]
public class SysFontTexture : ISysFontTexturable
{
	[SerializeField]
	protected string _text = string.Empty;

	[SerializeField]
	protected string _appleFontName = string.Empty;

	[SerializeField]
	protected string _androidFontName = string.Empty;

	[SerializeField]
	protected int _fontSize;

	[SerializeField]
	protected bool _isBold;

	[SerializeField]
	protected bool _isItalic;

	[SerializeField]
	protected SysFont.Alignment _alignment;

	[SerializeField]
	protected bool _isMultiLine = true;

	[SerializeField]
	protected int _maxWidthPixels = 2048;

	[SerializeField]
	protected int _maxHeightPixels = 2048;

	protected string _lastText;

	protected string _lastFontName;

	protected int _lastFontSize;

	protected bool _lastIsBold;

	protected bool _lastIsItalic;

	protected SysFont.Alignment _lastAlignment;

	protected bool _lastIsMultiLine;

	protected int _lastMaxWidthPixels;

	protected int _lastMaxHeightPixels;

	protected int _widthPixels = 1;

	protected int _heightPixels = 1;

	protected int _textWidthPixels;

	protected int _textHeightPixels;

	protected int _textureId;

	protected Texture2D _texture;

	public string Text
	{
		get
		{
			return this._text;
		}
		set
		{
			if (this._text != value)
			{
				this._text = value;
			}
		}
	}

	public string AppleFontName
	{
		get
		{
			return this._appleFontName;
		}
		set
		{
			if (this._appleFontName != value)
			{
				this._appleFontName = value;
			}
		}
	}

	public string AndroidFontName
	{
		get
		{
			return this._androidFontName;
		}
		set
		{
			if (this._androidFontName != value)
			{
				this._androidFontName = value;
			}
		}
	}

	public string FontName
	{
		get
		{
			return this.AndroidFontName;
		}
		set
		{
			this.AndroidFontName = value;
		}
	}

	public int FontSize
	{
		get
		{
			return this._fontSize;
		}
		set
		{
			if (this._fontSize != value)
			{
				this._fontSize = value;
			}
		}
	}

	public bool IsBold
	{
		get
		{
			return this._isBold;
		}
		set
		{
			if (this._isBold != value)
			{
				this._isBold = value;
			}
		}
	}

	public bool IsItalic
	{
		get
		{
			return this._isItalic;
		}
		set
		{
			if (this._isItalic != value)
			{
				this._isItalic = value;
			}
		}
	}

	public SysFont.Alignment Alignment
	{
		get
		{
			return this._alignment;
		}
		set
		{
			if (this._alignment != value)
			{
				this._alignment = value;
			}
		}
	}

	public bool IsMultiLine
	{
		get
		{
			return this._isMultiLine;
		}
		set
		{
			if (this._isMultiLine != value)
			{
				this._isMultiLine = value;
			}
		}
	}

	public int MaxWidthPixels
	{
		get
		{
			return this._maxWidthPixels;
		}
		set
		{
			if (this._maxWidthPixels != value)
			{
				this._maxWidthPixels = value;
			}
		}
	}

	public int MaxHeightPixels
	{
		get
		{
			return this._maxHeightPixels;
		}
		set
		{
			if (this._maxHeightPixels != value)
			{
				this._maxHeightPixels = value;
			}
		}
	}

	public int WidthPixels
	{
		get
		{
			return this._widthPixels;
		}
	}

	public int HeightPixels
	{
		get
		{
			return this._heightPixels;
		}
	}

	public int TextWidthPixels
	{
		get
		{
			return this._textWidthPixels;
		}
	}

	public int TextHeightPixels
	{
		get
		{
			return this._textHeightPixels;
		}
	}

	public bool IsUpdated
	{
		get
		{
			return this._texture != null && this._textureId != 0;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return this._texture;
		}
	}

	public bool NeedsRedraw
	{
		get
		{
			return this._text != this._lastText || this.FontName != this._lastFontName || this._fontSize != this._lastFontSize || this._isBold != this._lastIsBold || this._isItalic != this._lastIsItalic || this._alignment != this._lastAlignment || this._isMultiLine != this._lastIsMultiLine || this._maxWidthPixels != this._lastMaxWidthPixels || this._maxHeightPixels != this._lastMaxHeightPixels;
		}
	}

	public bool Update()
	{
		if (this._texture == null)
		{
			this._texture = new Texture2D(1, 1, TextureFormat.Alpha8, false);
			this._texture.hideFlags = (HideFlags.HideInInspector | HideFlags.DontSave);
			this._texture.filterMode = FilterMode.Point;
			this._texture.wrapMode = TextureWrapMode.Clamp;
			this._texture.Apply(false, true);
			this._textureId = 0;
		}
		if (this._textureId == 0)
		{
			this._textureId = this._texture.GetNativeTextureID();
			if (this._textureId == 0)
			{
				return false;
			}
		}
		SysFont.QueueTexture(this._text, this.FontName, this._fontSize, this._isBold, this._isItalic, this._alignment, this._isMultiLine, this._maxWidthPixels, this._maxHeightPixels, this._textureId);
		this._textWidthPixels = SysFont.GetTextWidth(this._textureId);
		this._textHeightPixels = SysFont.GetTextHeight(this._textureId);
		this._widthPixels = SysFont.GetTextureWidth(this._textureId);
		this._heightPixels = SysFont.GetTextureHeight(this._textureId);
		SysFont.UpdateQueuedTexture(this._textureId);
		this._lastText = this._text;
		this._lastFontName = this.FontName;
		this._lastFontSize = this._fontSize;
		this._lastIsBold = this._isBold;
		this._lastIsItalic = this._isItalic;
		this._lastAlignment = this._alignment;
		this._lastIsMultiLine = this._isMultiLine;
		this._lastMaxWidthPixels = this._maxWidthPixels;
		this._lastMaxHeightPixels = this._maxHeightPixels;
		return true;
	}

	public void Destroy()
	{
		if (this._texture != null)
		{
			if (this._textureId != 0)
			{
				SysFont.DequeueTexture(this._textureId);
				this._textureId = 0;
			}
			SysFont.SafeDestroy(this._texture);
			this._texture = null;
		}
	}
}
