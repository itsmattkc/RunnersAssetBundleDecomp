using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Texture"), ExecuteInEditMode]
public class UITexture : UIWidget
{
	[HideInInspector, SerializeField]
	private Rect mRect = new Rect(0f, 0f, 1f, 1f);

	[HideInInspector, SerializeField]
	private Shader mShader;

	[HideInInspector, SerializeField]
	private Texture mTexture;

	[HideInInspector, SerializeField]
	private Material mMat;

	private bool mCreatingMat;

	private Material mDynamicMat;

	private int mPMA = -1;

	public Rect uvRect
	{
		get
		{
			return this.mRect;
		}
		set
		{
			if (this.mRect != value)
			{
				this.mRect = value;
				this.MarkAsChanged();
			}
		}
	}

	public Shader shader
	{
		get
		{
			if (this.mShader == null)
			{
				Material material = this.material;
				if (material != null)
				{
					this.mShader = material.shader;
				}
				if (this.mShader == null)
				{
					this.mShader = Shader.Find("Unlit/Texture");
				}
			}
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				this.mShader = value;
				Material material = this.material;
				if (material != null)
				{
					material.shader = value;
				}
				this.mPMA = -1;
			}
		}
	}

	public bool hasDynamicMaterial
	{
		get
		{
			return this.mDynamicMat != null;
		}
	}

	public override Material material
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat;
			}
			if (this.mDynamicMat != null)
			{
				return this.mDynamicMat;
			}
			if (!this.mCreatingMat && this.mDynamicMat == null)
			{
				this.mCreatingMat = true;
				if (this.mShader == null)
				{
					this.mShader = Shader.Find("Unlit/Transparent Colored");
				}
				this.Cleanup();
				this.mDynamicMat = new Material(this.mShader);
				this.mDynamicMat.hideFlags = HideFlags.DontSave;
				this.mDynamicMat.mainTexture = this.mTexture;
				this.mPMA = 0;
				this.mCreatingMat = false;
			}
			return this.mDynamicMat;
		}
		set
		{
			if (this.mMat != value)
			{
				this.Cleanup();
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	public bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat.mainTexture;
			}
			if (this.mTexture != null)
			{
				return this.mTexture;
			}
			return null;
		}
		set
		{
			base.RemoveFromPanel();
			Material material = this.material;
			if (material != null)
			{
				this.mPanel = null;
				this.mTexture = value;
				material.mainTexture = value;
				base.MarkAsChangedLite();
				if (base.enabled)
				{
					base.CreatePanel();
				}
				if (this.mPanel != null)
				{
					this.mPanel.Refresh();
				}
			}
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

	public void SetTexture(Texture value)
	{
		Material material = this.material;
		if (material != null)
		{
			this.mTexture = value;
			material.mainTexture = value;
		}
	}

	private void OnDestroy()
	{
		this.Cleanup();
	}

	private void Cleanup()
	{
		if (this.mDynamicMat != null)
		{
			NGUITools.Destroy(this.mDynamicMat);
			this.mDynamicMat = null;
		}
	}

	public override void MakePixelPerfect()
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture != null)
		{
			int num = mainTexture.width;
			if ((num & 1) == 1)
			{
				num++;
			}
			int num2 = mainTexture.height;
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			base.width = num;
			base.height = num2;
		}
		base.MakePixelPerfect();
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Color color = base.color;
		color.a *= this.mPanel.alpha;
		Color32 item = (!this.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		Vector4 drawingDimensions = this.drawingDimensions;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMin));
		uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMax));
		uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMax));
		uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMin));
		cols.Add(item);
		cols.Add(item);
		cols.Add(item);
		cols.Add(item);
	}
}
