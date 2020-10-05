using System;
using UnityEngine;

[AddComponentMenu("SysFont/Text"), ExecuteInEditMode]
public class SysFontText : MonoBehaviour, ISysFontTexturable
{
	public enum PivotAlignment
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	[SerializeField]
	protected SysFontTexture _texture = new SysFontTexture();

	[SerializeField]
	protected Color _fontColor = Color.white;

	[SerializeField]
	protected SysFontText.PivotAlignment _pivot = SysFontText.PivotAlignment.Center;

	protected Color _lastFontColor;

	protected SysFontText.PivotAlignment _lastPivot;

	protected Transform _transform;

	protected Material _createdMaterial;

	protected Material _material;

	protected Vector3[] _vertices;

	protected Vector2[] _uv;

	protected int[] _triangles;

	protected Mesh _mesh;

	protected MeshFilter _filter;

	protected MeshRenderer _renderer;

	protected static Shader _shader;

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

	public Color FontColor
	{
		get
		{
			return this._fontColor;
		}
		set
		{
			if (this._fontColor != value)
			{
				this._fontColor = value;
			}
		}
	}

	public SysFontText.PivotAlignment Pivot
	{
		get
		{
			return this._pivot;
		}
		set
		{
			if (this._pivot != value)
			{
				this._pivot = value;
			}
		}
	}

	protected void UpdateMesh()
	{
		if (this._filter == null)
		{
			this._filter = base.gameObject.GetComponent<MeshFilter>();
			if (this._filter == null)
			{
				this._filter = base.gameObject.AddComponent<MeshFilter>();
				this._filter.hideFlags = HideFlags.HideInInspector;
			}
		}
		if (this._renderer == null)
		{
			this._renderer = base.gameObject.GetComponent<MeshRenderer>();
			if (this._renderer == null)
			{
				this._renderer = base.gameObject.AddComponent<MeshRenderer>();
				this._renderer.hideFlags = HideFlags.HideInInspector;
			}
			if (SysFontText._shader == null)
			{
				SysFontText._shader = Shader.Find("SysFont/Unlit Transparent");
			}
			if (this._createdMaterial == null)
			{
				this._createdMaterial = new Material(SysFontText._shader);
			}
			this._createdMaterial.hideFlags = (HideFlags.HideInInspector | HideFlags.DontSave);
			this._material = this._createdMaterial;
			this._renderer.sharedMaterial = this._material;
		}
		this._material.color = this._fontColor;
		this._lastFontColor = this._fontColor;
		if (this._uv == null)
		{
			this._uv = new Vector2[4];
			this._triangles = new int[]
			{
				0,
				2,
				1,
				2,
				3,
				1
			};
		}
		Vector2 vector = new Vector2((float)this._texture.TextWidthPixels / (float)this._texture.WidthPixels, (float)this._texture.TextHeightPixels / (float)this._texture.HeightPixels);
		this._uv[0] = Vector2.zero;
		this._uv[1] = new Vector2(vector.x, 0f);
		this._uv[2] = new Vector2(0f, vector.y);
		this._uv[3] = vector;
		this.UpdatePivot();
		this.UpdateScale();
	}

	protected void UpdatePivot()
	{
		if (this._vertices == null)
		{
			this._vertices = new Vector3[4];
			this._vertices[0] = Vector3.zero;
			this._vertices[1] = Vector3.zero;
			this._vertices[2] = Vector3.zero;
			this._vertices[3] = Vector3.zero;
		}
		if (this._pivot == SysFontText.PivotAlignment.TopLeft || this._pivot == SysFontText.PivotAlignment.Left || this._pivot == SysFontText.PivotAlignment.BottomLeft)
		{
			this._vertices[0].x = (this._vertices[2].x = 0f);
			this._vertices[1].x = (this._vertices[3].x = 1f);
		}
		else if (this._pivot == SysFontText.PivotAlignment.TopRight || this._pivot == SysFontText.PivotAlignment.Right || this._pivot == SysFontText.PivotAlignment.BottomRight)
		{
			this._vertices[0].x = (this._vertices[2].x = -1f);
			this._vertices[1].x = (this._vertices[3].x = 0f);
		}
		else
		{
			this._vertices[0].x = (this._vertices[2].x = -0.5f);
			this._vertices[1].x = (this._vertices[3].x = 0.5f);
		}
		if (this._pivot == SysFontText.PivotAlignment.TopLeft || this._pivot == SysFontText.PivotAlignment.Top || this._pivot == SysFontText.PivotAlignment.TopRight)
		{
			this._vertices[0].y = (this._vertices[1].y = -1f);
			this._vertices[2].y = (this._vertices[3].y = 0f);
		}
		else if (this._pivot == SysFontText.PivotAlignment.BottomLeft || this._pivot == SysFontText.PivotAlignment.Bottom || this._pivot == SysFontText.PivotAlignment.BottomRight)
		{
			this._vertices[0].y = (this._vertices[1].y = 0f);
			this._vertices[2].y = (this._vertices[3].y = 1f);
		}
		else
		{
			this._vertices[0].y = (this._vertices[1].y = -0.5f);
			this._vertices[2].y = (this._vertices[3].y = 0.5f);
		}
		if (this._mesh == null)
		{
			this._mesh = new Mesh();
			this._mesh.name = "SysFontTextMesh";
			this._mesh.hideFlags = HideFlags.DontSave;
		}
		this._mesh.vertices = this._vertices;
		this._mesh.uv = this._uv;
		this._mesh.triangles = this._triangles;
		this._mesh.RecalculateBounds();
		this._filter.mesh = this._mesh;
		this._lastPivot = this._pivot;
	}

	public void UpdateScale()
	{
		Vector3 localScale = this._transform.localScale;
		localScale.x = (float)this._texture.TextWidthPixels;
		localScale.y = (float)this._texture.TextHeightPixels;
		this._transform.localScale = localScale;
	}

	protected virtual void Awake()
	{
		this._transform = base.transform;
	}

	protected virtual void Update()
	{
		if (this._texture.NeedsRedraw)
		{
			if (!this._texture.Update())
			{
				return;
			}
			this.UpdateMesh();
			this._material.mainTexture = this.Texture;
		}
		if (!this._texture.IsUpdated)
		{
			return;
		}
		if (this._fontColor != this._lastFontColor && this._material != null)
		{
			this._material.color = this._fontColor;
			this._lastFontColor = this._fontColor;
		}
		if (this._lastPivot != this._pivot)
		{
			this.UpdatePivot();
		}
	}

	protected void OnDestroy()
	{
		if (this._texture != null)
		{
			this._texture.Destroy();
			this._texture = null;
		}
		SysFont.SafeDestroy(this._mesh);
		SysFont.SafeDestroy(this._createdMaterial);
		this._createdMaterial = null;
		this._material = null;
		this._vertices = null;
		this._uv = null;
		this._triangles = null;
		this._mesh = null;
		this._filter = null;
		this._renderer = null;
	}
}
