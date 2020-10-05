using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite"), ExecuteInEditMode]
public class UISprite : UIWidget
{
	public enum Type
	{
		Simple,
		Sliced,
		Tiled,
		Filled
	}

	public enum FillDirection
	{
		Horizontal,
		Vertical,
		Radial90,
		Radial180,
		Radial360
	}

	[HideInInspector, SerializeField]
	private UIAtlas mAtlas;

	[HideInInspector, SerializeField]
	private string mSpriteName;

	[HideInInspector, SerializeField]
	private bool mFillCenter = true;

	[HideInInspector, SerializeField]
	private UISprite.Type mType;

	[HideInInspector, SerializeField]
	private UISprite.FillDirection mFillDirection = UISprite.FillDirection.Radial360;

	[HideInInspector, SerializeField]
	private float mFillAmount = 1f;

	[HideInInspector, SerializeField]
	private bool mInvert;

	protected UISpriteData mSprite;

	protected Rect mInnerUV = default(Rect);

	protected Rect mOuterUV = default(Rect);

	private bool mSpriteSet;

	public virtual UISprite.Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	public override Material material
	{
		get
		{
			return (!(this.mAtlas != null)) ? null : this.mAtlas.spriteMaterial;
		}
	}

	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				base.RemoveFromPanel();
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string spriteName = this.mSpriteName;
					this.mSpriteName = string.Empty;
					this.spriteName = spriteName;
					this.MarkAsChanged();
				}
			}
		}
	}

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (string.IsNullOrEmpty(this.mSpriteName))
				{
					return;
				}
				this.mSpriteName = string.Empty;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
			else if (this.mSpriteName != value)
			{
				this.mSpriteName = value;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
		}
	}

	public bool isValid
	{
		get
		{
			return this.GetAtlasSprite() != null;
		}
	}

	public bool fillCenter
	{
		get
		{
			return this.mFillCenter;
		}
		set
		{
			if (this.mFillCenter != value)
			{
				this.mFillCenter = value;
				this.MarkAsChanged();
			}
		}
	}

	public UISprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				this.mChanged = true;
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mFillAmount != num)
			{
				this.mFillAmount = num;
				this.mChanged = true;
			}
		}
	}

	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				this.mChanged = true;
			}
		}
	}

	public override Vector4 border
	{
		get
		{
			if (this.type != UISprite.Type.Sliced)
			{
				return base.border;
			}
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return Vector2.zero;
			}
			return new Vector4((float)atlasSprite.borderLeft, (float)atlasSprite.borderBottom, (float)atlasSprite.borderRight, (float)atlasSprite.borderTop);
		}
	}

	public override int minWidth
	{
		get
		{
			if (this.type == UISprite.Type.Sliced)
			{
				Vector4 border = this.border;
				return Mathf.RoundToInt(border.x + border.z);
			}
			return base.minWidth;
		}
	}

	public override int minHeight
	{
		get
		{
			if (this.type == UISprite.Type.Sliced)
			{
				Vector4 border = this.border;
				return Mathf.RoundToInt(border.y + border.w);
			}
			return base.minHeight;
		}
	}

	private Vector4 drawingDimensions
	{
		get
		{
			if (this.mSprite == null)
			{
				return new Vector4(0f, 0f, (float)this.mWidth, (float)this.mHeight);
			}
			int paddingLeft = this.mSprite.paddingLeft;
			int paddingBottom = this.mSprite.paddingBottom;
			int num = this.mSprite.paddingRight;
			int num2 = this.mSprite.paddingTop;
			Vector2 pivotOffset = base.pivotOffset;
			int num3 = this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight;
			int num4 = this.mSprite.height + this.mSprite.paddingBottom + this.mSprite.paddingTop;
			if ((num3 & 1) == 1)
			{
				num++;
			}
			if ((num4 & 1) == 1)
			{
				num2++;
			}
			float num5 = 1f / (float)num3;
			float num6 = 1f / (float)num4;
			Vector4 result = new Vector4((float)paddingLeft * num5, (float)paddingBottom * num6, (float)(num3 - num) * num5, (float)(num4 - num2) * num6);
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

	public UISpriteData GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UISpriteData uISpriteData = this.mAtlas.spriteList[0];
				if (uISpriteData == null)
				{
					return null;
				}
				this.SetAtlasSprite(uISpriteData);
				if (this.mSprite == null)
				{
					global::Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
		}
		return this.mSprite;
	}

	protected void SetAtlasSprite(UISpriteData sp)
	{
		this.mChanged = true;
		this.mSpriteSet = true;
		if (sp != null)
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
		}
		else
		{
			this.mSpriteName = ((this.mSprite == null) ? string.Empty : this.mSprite.name);
			this.mSprite = sp;
		}
	}

	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			return;
		}
		base.MakePixelPerfect();
		UISprite.Type type = this.type;
		if (type == UISprite.Type.Simple || type == UISprite.Type.Filled)
		{
			Texture mainTexture = this.mainTexture;
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (mainTexture != null && atlasSprite != null)
			{
				int num = Mathf.RoundToInt(this.atlas.pixelSize * (float)(atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
				int num2 = Mathf.RoundToInt(this.atlas.pixelSize * (float)(atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num2 & 1) == 1)
				{
					num2++;
				}
				base.width = num;
				base.height = num2;
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (this.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			this.mChanged = true;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture != null)
		{
			if (this.mSprite == null)
			{
				this.mSprite = this.atlas.GetSprite(this.spriteName);
			}
			if (this.mSprite == null)
			{
				return;
			}
			this.mOuterUV.Set((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			this.mInnerUV.Set((float)(this.mSprite.x + this.mSprite.borderLeft), (float)(this.mSprite.y + this.mSprite.borderTop), (float)(this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float)(this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
			this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, mainTexture.width, mainTexture.height);
			this.mInnerUV = NGUIMath.ConvertToTexCoords(this.mInnerUV, mainTexture.width, mainTexture.height);
		}
		switch (this.type)
		{
		case UISprite.Type.Simple:
			this.SimpleFill(verts, uvs, cols);
			break;
		case UISprite.Type.Sliced:
			this.SlicedFill(verts, uvs, cols);
			break;
		case UISprite.Type.Tiled:
			this.TiledFill(verts, uvs, cols);
			break;
		case UISprite.Type.Filled:
			this.FilledFill(verts, uvs, cols);
			break;
		}
	}

	protected void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector2 item = new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMin);
		Vector2 item2 = new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMax);
		Vector4 drawingDimensions = this.drawingDimensions;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(item);
		uvs.Add(new Vector2(item.x, item2.y));
		uvs.Add(item2);
		uvs.Add(new Vector2(item2.x, item.y));
		Color color = base.color;
		color.a *= this.mPanel.alpha;
		Color32 item3 = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		cols.Add(item3);
		cols.Add(item3);
		cols.Add(item3);
		cols.Add(item3);
	}

	protected void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mSprite == null)
		{
			return;
		}
		if (!this.mSprite.hasBorder)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Vector4 vector = this.border * this.atlas.pixelSize;
		Vector2 pivotOffset = base.pivotOffset;
		float num = 1f / (float)this.mWidth;
		float num2 = 1f / (float)this.mHeight;
		Vector2[] array = new Vector2[]
		{
			new Vector2((float)this.mSprite.paddingLeft * num, (float)this.mSprite.paddingBottom * num2),
			default(Vector2),
			default(Vector2),
			new Vector2(1f - (float)this.mSprite.paddingRight * num, 1f - (float)this.mSprite.paddingTop * num2)
		};
		array[1].x = array[0].x + num * vector.x;
		array[1].y = array[0].y + num2 * vector.y;
		array[2].x = array[3].x - num * vector.z;
		array[2].y = array[3].y - num2 * vector.w;
		for (int i = 0; i < 4; i++)
		{
			Vector2[] expr_171_cp_0 = array;
			int expr_171_cp_1 = i;
			expr_171_cp_0[expr_171_cp_1].x = expr_171_cp_0[expr_171_cp_1].x - pivotOffset.x;
			Vector2[] expr_18D_cp_0 = array;
			int expr_18D_cp_1 = i;
			expr_18D_cp_0[expr_18D_cp_1].y = expr_18D_cp_0[expr_18D_cp_1].y - pivotOffset.y;
			Vector2[] expr_1A9_cp_0 = array;
			int expr_1A9_cp_1 = i;
			expr_1A9_cp_0[expr_1A9_cp_1].x = expr_1A9_cp_0[expr_1A9_cp_1].x * (float)this.mWidth;
			Vector2[] expr_1C5_cp_0 = array;
			int expr_1C5_cp_1 = i;
			expr_1C5_cp_0[expr_1C5_cp_1].y = expr_1C5_cp_0[expr_1C5_cp_1].y * (float)this.mHeight;
		}
		Vector2[] array2 = new Vector2[]
		{
			new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMin),
			new Vector2(this.mInnerUV.xMin, this.mInnerUV.yMin),
			new Vector2(this.mInnerUV.xMax, this.mInnerUV.yMax),
			new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMax)
		};
		Color color = base.color;
		color.a *= this.mPanel.alpha;
		Color32 item = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		for (int j = 0; j < 3; j++)
		{
			int num3 = j + 1;
			for (int k = 0; k < 3; k++)
			{
				if (this.mFillCenter || j != 1 || k != 1)
				{
					int num4 = k + 1;
					verts.Add(new Vector3(array[j].x, array[k].y));
					verts.Add(new Vector3(array[j].x, array[num4].y));
					verts.Add(new Vector3(array[num3].x, array[num4].y));
					verts.Add(new Vector3(array[num3].x, array[k].y));
					uvs.Add(new Vector2(array2[j].x, array2[k].y));
					uvs.Add(new Vector2(array2[j].x, array2[num4].y));
					uvs.Add(new Vector2(array2[num3].x, array2[num4].y));
					uvs.Add(new Vector2(array2[num3].x, array2[k].y));
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
				}
			}
		}
	}

	protected void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.material.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector2 a = new Vector2(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		a *= this.atlas.pixelSize;
		float num = Mathf.Abs(a.x / (float)this.mWidth);
		float num2 = Mathf.Abs(a.y / (float)this.mHeight);
		if (num * num2 < 0.0001f)
		{
			num = 0.01f;
			num2 = 0.01f;
		}
		Color color = base.color;
		color.a *= this.mPanel.alpha;
		Color32 item = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		Vector2 pivotOffset = base.pivotOffset;
		Vector2 vector = new Vector2(this.mInnerUV.xMin, this.mInnerUV.yMin);
		Vector2 vector2 = new Vector2(this.mInnerUV.xMax, this.mInnerUV.yMax);
		Vector2 vector3 = vector2;
		for (float num3 = 0f; num3 < 1f; num3 += num2)
		{
			float num4 = 0f;
			vector3.x = vector2.x;
			float num5 = num3 + num2;
			if (num5 > 1f)
			{
				vector3.y = vector.y + (vector2.y - vector.y) * (1f - num3) / (num5 - num3);
				num5 = 1f;
			}
			while (num4 < 1f)
			{
				float num6 = num4 + num;
				if (num6 > 1f)
				{
					vector3.x = vector.x + (vector2.x - vector.x) * (1f - num4) / (num6 - num4);
					num6 = 1f;
				}
				float x = (num4 - pivotOffset.x) * (float)this.mWidth;
				float x2 = (num6 - pivotOffset.x) * (float)this.mWidth;
				float y = (num3 - pivotOffset.y) * (float)this.mHeight;
				float y2 = (num5 - pivotOffset.y) * (float)this.mHeight;
				verts.Add(new Vector3(x, y));
				verts.Add(new Vector3(x, y2));
				verts.Add(new Vector3(x2, y2));
				verts.Add(new Vector3(x2, y));
				uvs.Add(new Vector2(vector.x, vector.y));
				uvs.Add(new Vector2(vector.x, vector3.y));
				uvs.Add(new Vector2(vector3.x, vector3.y));
				uvs.Add(new Vector2(vector3.x, vector.y));
				cols.Add(item);
				cols.Add(item);
				cols.Add(item);
				cols.Add(item);
				num4 += num;
			}
		}
	}

	protected void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mFillAmount < 0.001f)
		{
			return;
		}
		Color color = base.color;
		color.a *= this.mPanel.alpha;
		Color32 item = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		Vector2[] array = new Vector2[4];
		Vector2[] array2 = new Vector2[4];
		Vector4 drawingDimensions = this.drawingDimensions;
		float num = this.mOuterUV.xMin;
		float num2 = this.mOuterUV.yMin;
		float num3 = this.mOuterUV.xMax;
		float num4 = this.mOuterUV.yMax;
		if (this.mFillDirection == UISprite.FillDirection.Horizontal || this.mFillDirection == UISprite.FillDirection.Vertical)
		{
			if (this.mFillDirection == UISprite.FillDirection.Horizontal)
			{
				float num5 = (num3 - num) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					num = num3 - num5;
				}
				else
				{
					drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					num3 = num + num5;
				}
			}
			else if (this.mFillDirection == UISprite.FillDirection.Vertical)
			{
				float num6 = (num4 - num2) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					num2 = num4 - num6;
				}
				else
				{
					drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					num4 = num2 + num6;
				}
			}
		}
		array[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
		array[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
		array[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
		array[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
		array2[0] = new Vector2(num, num2);
		array2[1] = new Vector2(num, num4);
		array2[2] = new Vector2(num3, num4);
		array2[3] = new Vector2(num3, num2);
		if (this.mFillAmount < 1f)
		{
			if (this.mFillDirection == UISprite.FillDirection.Radial90)
			{
				if (UISprite.RadialCut(array, array2, this.mFillAmount, this.mInvert, 0))
				{
					for (int i = 0; i < 4; i++)
					{
						verts.Add(array[i]);
						uvs.Add(array2[i]);
						cols.Add(item);
					}
				}
				return;
			}
			if (this.mFillDirection == UISprite.FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					float t = 0f;
					float t2 = 1f;
					float t3;
					float t4;
					if (j == 0)
					{
						t3 = 0f;
						t4 = 0.5f;
					}
					else
					{
						t3 = 0.5f;
						t4 = 1f;
					}
					array[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
					array[1].x = array[0].x;
					array[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
					array[3].x = array[2].x;
					array[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t);
					array[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
					array[2].y = array[1].y;
					array[3].y = array[0].y;
					array2[0].x = Mathf.Lerp(num, num3, t3);
					array2[1].x = array2[0].x;
					array2[2].x = Mathf.Lerp(num, num3, t4);
					array2[3].x = array2[2].x;
					array2[0].y = Mathf.Lerp(num2, num4, t);
					array2[1].y = Mathf.Lerp(num2, num4, t2);
					array2[2].y = array2[1].y;
					array2[3].y = array2[0].y;
					float value = this.mInvert ? (this.mFillAmount * 2f - (float)(1 - j)) : (this.fillAmount * 2f - (float)j);
					if (UISprite.RadialCut(array, array2, Mathf.Clamp01(value), !this.mInvert, NGUIMath.RepeatIndex(j + 3, 4)))
					{
						for (int k = 0; k < 4; k++)
						{
							verts.Add(array[k]);
							uvs.Add(array2[k]);
							cols.Add(item);
						}
					}
				}
				return;
			}
			if (this.mFillDirection == UISprite.FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					float t5;
					float t6;
					if (l < 2)
					{
						t5 = 0f;
						t6 = 0.5f;
					}
					else
					{
						t5 = 0.5f;
						t6 = 1f;
					}
					float t7;
					float t8;
					if (l == 0 || l == 3)
					{
						t7 = 0f;
						t8 = 0.5f;
					}
					else
					{
						t7 = 0.5f;
						t8 = 1f;
					}
					array[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
					array[1].x = array[0].x;
					array[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
					array[3].x = array[2].x;
					array[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
					array[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
					array[2].y = array[1].y;
					array[3].y = array[0].y;
					array2[0].x = Mathf.Lerp(num, num3, t5);
					array2[1].x = array2[0].x;
					array2[2].x = Mathf.Lerp(num, num3, t6);
					array2[3].x = array2[2].x;
					array2[0].y = Mathf.Lerp(num2, num4, t7);
					array2[1].y = Mathf.Lerp(num2, num4, t8);
					array2[2].y = array2[1].y;
					array2[3].y = array2[0].y;
					float value2 = (!this.mInvert) ? (this.mFillAmount * 4f - (float)(3 - NGUIMath.RepeatIndex(l + 2, 4))) : (this.mFillAmount * 4f - (float)NGUIMath.RepeatIndex(l + 2, 4));
					if (UISprite.RadialCut(array, array2, Mathf.Clamp01(value2), this.mInvert, NGUIMath.RepeatIndex(l + 2, 4)))
					{
						for (int m = 0; m < 4; m++)
						{
							verts.Add(array[m]);
							uvs.Add(array2[m]);
							cols.Add(item);
						}
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(array[n]);
			uvs.Add(array2[n]);
			cols.Add(item);
		}
	}

	private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if ((corner & 1) == 1)
		{
			invert = !invert;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (invert)
		{
			num = 1f - num;
		}
		num *= 1.57079637f;
		float cos = Mathf.Cos(num);
		float sin = Mathf.Sin(num);
		UISprite.RadialCut(xy, cos, sin, invert, corner);
		UISprite.RadialCut(uv, cos, sin, invert, corner);
		return true;
	}

	private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
	{
		int num = NGUIMath.RepeatIndex(corner + 1, 4);
		int num2 = NGUIMath.RepeatIndex(corner + 2, 4);
		int num3 = NGUIMath.RepeatIndex(corner + 3, 4);
		if ((corner & 1) == 1)
		{
			if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num2].x = xy[num].x;
				}
			}
			else if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num3].y = xy[num2].y;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (!invert)
			{
				xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
			else
			{
				xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
		}
		else
		{
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num2].y = xy[num].y;
				}
			}
			else if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (invert)
			{
				xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
			else
			{
				xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
		}
	}
}
