using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/SysFont Label"), ExecuteInEditMode]
public class UISysFontLabel : UIWidget, ISysFontTexturable
{
	[SerializeField]
	protected SysFontTexture _texture = new SysFontTexture();

	protected static Shader _shader;

	protected Material _createdMaterial;

	protected Vector3[] _vertices;

	protected Vector2 _uv;

	public string Text
	{
		get
		{
			return this._texture.Text;
		}
		set
		{
			this._texture.Text = value;
		}
	}

	public string AppleFontName
	{
		get
		{
			return this._texture.AppleFontName;
		}
		set
		{
			this._texture.AppleFontName = value;
		}
	}

	public string AndroidFontName
	{
		get
		{
			return this._texture.AndroidFontName;
		}
		set
		{
			this._texture.AndroidFontName = value;
		}
	}

	public string FontName
	{
		get
		{
			return this._texture.FontName;
		}
		set
		{
			this._texture.FontName = value;
		}
	}

	public int FontSize
	{
		get
		{
			return this._texture.FontSize;
		}
		set
		{
			this._texture.FontSize = value;
		}
	}

	public bool IsBold
	{
		get
		{
			return this._texture.IsBold;
		}
		set
		{
			this._texture.IsBold = value;
		}
	}

	public bool IsItalic
	{
		get
		{
			return this._texture.IsItalic;
		}
		set
		{
			this._texture.IsItalic = value;
		}
	}

	public SysFont.Alignment Alignment
	{
		get
		{
			return this._texture.Alignment;
		}
		set
		{
			this._texture.Alignment = value;
		}
	}

	public bool IsMultiLine
	{
		get
		{
			return this._texture.IsMultiLine;
		}
		set
		{
			this._texture.IsMultiLine = value;
		}
	}

	public int MaxWidthPixels
	{
		get
		{
			return this._texture.MaxWidthPixels;
		}
		set
		{
			this._texture.MaxWidthPixels = value;
		}
	}

	public int MaxHeightPixels
	{
		get
		{
			return this._texture.MaxHeightPixels;
		}
		set
		{
			this._texture.MaxHeightPixels = value;
		}
	}

	public int WidthPixels
	{
		get
		{
			return this._texture.WidthPixels;
		}
	}

	public int HeightPixels
	{
		get
		{
			return this._texture.HeightPixels;
		}
	}

	public int TextWidthPixels
	{
		get
		{
			return this._texture.TextWidthPixels;
		}
	}

	public int TextHeightPixels
	{
		get
		{
			return this._texture.TextHeightPixels;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return this._texture.Texture;
		}
	}

	public override Material material
	{
		get
		{
			return this._createdMaterial;
		}
		set
		{
		}
	}

	public override Texture mainTexture
	{
		get
		{
			return (!(this.material != null)) ? null : this.material.mainTexture;
		}
		set
		{
		}
	}

	private Vector4 drawingDimensions
	{
		get
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			Texture mainTexture = this.mainTexture;
			Rect rect = (!(mainTexture != null)) ? new Rect(0f, 0f, (float)this.mWidth, (float)this.mHeight) : new Rect(0f, 0f, (float)mainTexture.width, (float)mainTexture.height);
			Vector2 pivotOffset = base.pivotOffset;
			int num5 = Mathf.RoundToInt(rect.width);
			int num6 = Mathf.RoundToInt(rect.height);
			float num7 = (float)(((num5 & 1) != 0) ? (num5 + 1) : num5);
			float num8 = (float)(((num6 & 1) != 0) ? (num6 + 1) : num6);
			Vector4 result = new Vector4(num / num7, num2 / num8, ((float)num5 - num3) / num7, ((float)num6 - num4) / num8);
			result.x -= pivotOffset.x;
			result.y -= pivotOffset.y;
			result.z -= pivotOffset.x;
			result.w -= pivotOffset.y;
			result.x *= (float)this.mWidth;
			result.y *= (float)this.mHeight;
			result.z *= (float)this.mWidth;
			result.w *= (float)this.mHeight;
			return result;
		}
	}

	protected new virtual void Update()
	{
		base.Update();
		this.MarkAsChanged();
		if (this._texture.NeedsRedraw)
		{
			this._texture.Update();
			this._uv = new Vector2((float)this._texture.TextWidthPixels / (float)this._texture.WidthPixels, (float)this._texture.TextHeightPixels / (float)this._texture.HeightPixels);
			global::Debug.Log("_texture.TextWidthPixels = " + this._texture.TextWidthPixels);
			global::Debug.Log("_texture.WidthPixels = " + this._texture.WidthPixels);
			global::Debug.Log("_texture.TextHeightPixels = " + this._texture.TextHeightPixels);
			global::Debug.Log("_texture.HeightPixels = " + this._texture.HeightPixels);
			global::Debug.Log("uv.u = " + this._uv.x.ToString());
			global::Debug.Log("uv.v = " + this._uv.y.ToString());
			NGUITools.Destroy(this._createdMaterial);
			this._createdMaterial = new Material(UISysFontLabel._shader);
			this._createdMaterial.hideFlags = HideFlags.DontSave;
			this._createdMaterial.mainTexture = this._texture.Texture;
			this._createdMaterial.color = base.color;
		}
	}

	public override void MakePixelPerfect()
	{
		Vector3 localScale = base.cachedTransform.localScale;
		localScale.x = (float)this._texture.TextWidthPixels;
		localScale.y = (float)this._texture.TextHeightPixels;
		base.cachedTransform.localScale = localScale;
		base.MakePixelPerfect();
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 drawingDimensions = this.drawingDimensions;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(Vector2.zero);
		uvs.Add(new Vector2(0f, this._uv.y));
		uvs.Add(this._uv);
		uvs.Add(new Vector2(this._uv.x, 0f));
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		this.MakePixelPerfect();
		if (this.material.mainTexture != this._texture.Texture)
		{
			this.material.mainTexture = this._texture.Texture;
		}
	}

	protected new void OnEnable()
	{
		base.OnEnable();
		if (UISysFontLabel._shader == null)
		{
			UISysFontLabel._shader = Shader.Find("SysFont/Unlit Transparent");
		}
	}

	protected void OnDestroy()
	{
		this.material = null;
		SysFont.SafeDestroy(this._createdMaterial);
		if (this._texture != null)
		{
			this._texture.Destroy();
			this._texture = null;
		}
	}
}
