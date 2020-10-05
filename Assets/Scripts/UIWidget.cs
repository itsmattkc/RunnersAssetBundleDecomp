using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
	public enum Pivot
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

	public static BetterList<UIWidget> list = new BetterList<UIWidget>();

	[HideInInspector, SerializeField]
	protected Color mColor = Color.white;

	[HideInInspector, SerializeField]
	protected UIWidget.Pivot mPivot = UIWidget.Pivot.Center;

	[HideInInspector, SerializeField]
	protected int mWidth;

	[HideInInspector, SerializeField]
	protected int mHeight;

	[HideInInspector, SerializeField]
	protected int mDepth;

	protected GameObject mGo;

	protected Transform mTrans;

	protected UIPanel mPanel;

	protected bool mChanged = true;

	protected bool mPlayMode = true;

	private bool mStarted;

	private Vector3 mDiffPos;

	private Quaternion mDiffRot;

	private Vector3 mDiffScale;

	private Matrix4x4 mLocalToPanel;

	private bool mVisibleByPanel = true;

	private float mLastAlpha;

	private UIGeometry mGeom = new UIGeometry();

	private Vector3[] mCorners = new Vector3[4];

	private bool mForceVisible;

	private Vector3 mOldV0;

	private Vector3 mOldV1;

	private UIDrawCall _drawCall_k__BackingField;

	public UIDrawCall drawCall
	{
		get;
		set;
	}

	public bool isVisible
	{
		get
		{
			return this.mVisibleByPanel && this.finalAlpha > 0.001f;
		}
	}

	public int width
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			int minWidth = this.minWidth;
			if (value < minWidth)
			{
				value = minWidth;
			}
			if (this.mWidth != value)
			{
				this.mWidth = value;
				this.MarkAsChanged();
			}
		}
	}

	public int height
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			int minHeight = this.minHeight;
			if (value < minHeight)
			{
				value = minHeight;
			}
			if (this.mHeight != value)
			{
				this.mHeight = value;
				this.MarkAsChanged();
			}
		}
	}

	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (!this.mColor.Equals(value))
			{
				this.mColor = value;
				this.mChanged = true;
			}
		}
	}

	public float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			Color color = this.mColor;
			color.a = value;
			this.color = color;
		}
	}

	public float finalAlpha
	{
		get
		{
			if (this.mPanel == null)
			{
				this.CreatePanel();
			}
			return (!(this.mPanel != null)) ? this.mColor.a : (this.mColor.a * this.mPanel.alpha);
		}
	}

	public UIWidget.Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				Vector3 vector = this.worldCorners[0];
				this.mPivot = value;
				this.mChanged = true;
				Vector3 vector2 = this.worldCorners[0];
				Transform cachedTransform = this.cachedTransform;
				Vector3 vector3 = cachedTransform.position;
				float z = cachedTransform.localPosition.z;
				vector3.x += vector.x - vector2.x;
				vector3.y += vector.y - vector2.y;
				this.cachedTransform.position = vector3;
				vector3 = this.cachedTransform.localPosition;
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				vector3.z = z;
				this.cachedTransform.localPosition = vector3;
			}
		}
	}

	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				this.mDepth = value;
				UIPanel.SetDirty();
			}
		}
	}

	public int raycastDepth
	{
		get
		{
			return (!(this.mPanel != null)) ? this.mDepth : (this.mDepth + this.mPanel.depth * 1000);
		}
	}

	public virtual Vector3[] localCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			this.mCorners[0] = new Vector3(num, num2, 0f);
			this.mCorners[1] = new Vector3(num, y, 0f);
			this.mCorners[2] = new Vector3(x, y, 0f);
			this.mCorners[3] = new Vector3(x, num2, 0f);
			return this.mCorners;
		}
	}

	public virtual Vector2 localSize
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return localCorners[2] - localCorners[0];
		}
	}

	public virtual Vector3[] worldCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			Transform cachedTransform = this.cachedTransform;
			this.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
			this.mCorners[1] = cachedTransform.TransformPoint(num, y, 0f);
			this.mCorners[2] = cachedTransform.TransformPoint(x, y, 0f);
			this.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
			return this.mCorners;
		}
	}

	public Vector3[] innerWorldCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			Vector4 border = this.border;
			num += border.x;
			num2 += border.y;
			num3 -= border.z;
			num4 -= border.w;
			Transform cachedTransform = this.cachedTransform;
			this.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
			this.mCorners[1] = cachedTransform.TransformPoint(num, num4, 0f);
			this.mCorners[2] = cachedTransform.TransformPoint(num3, num4, 0f);
			this.mCorners[3] = cachedTransform.TransformPoint(num3, num2, 0f);
			return this.mCorners;
		}
	}

	public bool hasVertices
	{
		get
		{
			return this.mGeom != null && this.mGeom.hasVertices;
		}
	}

	public Vector2 pivotOffset
	{
		get
		{
			return NGUIMath.GetPivotOffset(this.pivot);
		}
	}

	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public virtual Material material
	{
		get
		{
			return null;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no material setter");
		}
	}

	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			return (!(material != null)) ? null : material.mainTexture;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no mainTexture setter");
		}
	}

	public UIPanel panel
	{
		get
		{
			if (this.mPanel == null)
			{
				this.CreatePanel();
			}
			return this.mPanel;
		}
		set
		{
			this.mPanel = value;
		}
	}

	[Obsolete("There is no relative scale anymore. Widgets now have width and height instead")]
	public Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	public virtual int minWidth
	{
		get
		{
			return 4;
		}
	}

	public virtual int minHeight
	{
		get
		{
			return 4;
		}
	}

	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
	}

	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		int num = UIPanel.CompareFunc(left.mPanel, right.mPanel);
		if (num == 0)
		{
			if (left.mDepth < right.mDepth)
			{
				return -1;
			}
			if (left.mDepth > right.mDepth)
			{
				return 1;
			}
		}
		return num;
	}

	public Bounds CalculateBounds()
	{
		return this.CalculateBounds(null);
	}

	public Bounds CalculateBounds(Transform relativeParent)
	{
		if (relativeParent == null)
		{
			Vector3[] localCorners = this.localCorners;
			Bounds result = new Bounds(localCorners[0], Vector3.zero);
			for (int i = 1; i < 4; i++)
			{
				result.Encapsulate(localCorners[i]);
			}
			return result;
		}
		Matrix4x4 worldToLocalMatrix = relativeParent.worldToLocalMatrix;
		Vector3[] worldCorners = this.worldCorners;
		Bounds result2 = new Bounds(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[0]), Vector3.zero);
		for (int j = 1; j < 4; j++)
		{
			result2.Encapsulate(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[j]));
		}
		return result2;
	}

	private void SetDirty()
	{
		if (this.drawCall != null)
		{
			this.drawCall.isDirty = true;
		}
		else if (this.isVisible && this.hasVertices)
		{
			UIPanel.SetDirty();
		}
	}

	protected void RemoveFromPanel()
	{
		if (this.mPanel != null)
		{
			this.drawCall = null;
			this.mPanel = null;
			this.SetDirty();
		}
	}

	public void MarkAsChangedLite()
	{
		this.mChanged = true;
	}

	public virtual void MarkAsChanged()
	{
		this.mChanged = true;
		if (this.mPanel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !Application.isPlaying && this.material != null)
		{
			this.SetDirty();
			this.CheckLayer();
		}
	}

	public void CreatePanel()
	{
		if (this.mPanel == null && base.enabled && NGUITools.GetActive(base.gameObject) && this.material != null)
		{
			this.mPanel = UIPanel.Find(this.cachedTransform, this.mStarted);
			if (this.mPanel != null)
			{
				this.CheckLayer();
				this.mChanged = true;
				UIPanel.SetDirty();
			}
		}
	}

	public void CheckLayer()
	{
		if (this.mPanel != null && this.mPanel.gameObject.layer != base.gameObject.layer)
		{
			global::Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.mPanel.gameObject.layer;
		}
	}

	public void ParentHasChanged()
	{
		if (this.mPanel != null)
		{
			UIPanel y = UIPanel.Find(this.cachedTransform);
			if (this.mPanel != y)
			{
				this.RemoveFromPanel();
				this.CreatePanel();
			}
		}
	}

	protected virtual void Awake()
	{
		this.mGo = base.gameObject;
		this.mPlayMode = Application.isPlaying;
	}

	protected virtual void OnEnable()
	{
		UIWidget.list.Add(this);
		this.mChanged = true;
		this.mPanel = null;
		if (this.mWidth == 0 && this.mHeight == 0)
		{
			this.UpgradeFrom265();
			this.cachedTransform.localScale = Vector3.one;
		}
	}

	protected virtual void UpgradeFrom265()
	{
		Vector3 localScale = this.cachedTransform.localScale;
		this.mWidth = Mathf.Abs(Mathf.RoundToInt(localScale.x));
		this.mHeight = Mathf.Abs(Mathf.RoundToInt(localScale.y));
		if (base.GetComponent<BoxCollider>() != null)
		{
			NGUITools.AddWidgetCollider(base.gameObject, true);
		}
	}

	private void Start()
	{
		this.mStarted = true;
		this.OnStart();
		this.CreatePanel();
	}

	public virtual void Update()
	{
		if (this.mPanel == null)
		{
			this.CreatePanel();
		}
	}

	protected virtual void OnDisable()
	{
		UIWidget.list.Remove(this);
		this.RemoveFromPanel();
	}

	private void OnDestroy()
	{
		this.RemoveFromPanel();
	}

	private bool HasTransformChanged()
	{
		if (this.cachedTransform.hasChanged)
		{
			this.mTrans.hasChanged = false;
			return true;
		}
		return false;
	}

	public bool UpdateGeometry(UIPanel p, bool forceVisible)
	{
		if (this.material != null && p != null)
		{
			this.mPanel = p;
			bool flag = false;
			float finalAlpha = this.finalAlpha;
			bool flag2 = finalAlpha > 0.001f;
			bool flag3 = forceVisible || this.mVisibleByPanel;
			if (this.HasTransformChanged())
			{
				if (!this.mPanel.widgetsAreStatic)
				{
					this.mLocalToPanel = p.worldToLocal * this.cachedTransform.localToWorldMatrix;
					flag = true;
					Vector2 pivotOffset = this.pivotOffset;
					float num = -pivotOffset.x * (float)this.mWidth;
					float num2 = -pivotOffset.y * (float)this.mHeight;
					float x = num + (float)this.mWidth;
					float y = num2 + (float)this.mHeight;
					Transform cachedTransform = this.cachedTransform;
					Vector3 vector = cachedTransform.TransformPoint(num, num2, 0f);
					Vector3 vector2 = cachedTransform.TransformPoint(x, y, 0f);
					vector = p.worldToLocal.MultiplyPoint3x4(vector);
					vector2 = p.worldToLocal.MultiplyPoint3x4(vector2);
					if (Vector3.SqrMagnitude(this.mOldV0 - vector) > 1E-06f || Vector3.SqrMagnitude(this.mOldV1 - vector2) > 1E-06f)
					{
						this.mChanged = true;
						this.mOldV0 = vector;
						this.mOldV1 = vector2;
					}
				}
				if (flag2 || this.mForceVisible != forceVisible)
				{
					this.mForceVisible = forceVisible;
					flag3 = (forceVisible || this.mPanel.IsVisible(this));
				}
			}
			else if (flag2 && this.mForceVisible != forceVisible)
			{
				this.mForceVisible = forceVisible;
				flag3 = this.mPanel.IsVisible(this);
			}
			if (this.mVisibleByPanel != flag3)
			{
				this.mVisibleByPanel = flag3;
				this.mChanged = true;
			}
			if (this.mVisibleByPanel && this.mLastAlpha != finalAlpha)
			{
				this.mChanged = true;
			}
			this.mLastAlpha = finalAlpha;
			if (this.mChanged)
			{
				this.mChanged = false;
				if (this.isVisible)
				{
					bool hasVertices = this.mGeom.hasVertices;
					this.mGeom.Clear();
					this.OnFill(this.mGeom.verts, this.mGeom.uvs, this.mGeom.cols);
					if (this.mGeom.hasVertices)
					{
						if (!flag)
						{
							this.mLocalToPanel = p.worldToLocal * this.cachedTransform.localToWorldMatrix;
						}
						this.mGeom.ApplyTransform(this.mLocalToPanel);
						return true;
					}
					return hasVertices;
				}
				else if (this.mGeom.hasVertices)
				{
					this.mGeom.Clear();
					return true;
				}
			}
		}
		return false;
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		this.mGeom.WriteToBuffers(v, u, c, n, t);
	}

	public int WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t, int point)
	{
		return this.mGeom.WriteToBuffers(v, u, c, n, t, point);
	}

	public virtual void MakePixelPerfect()
	{
		Vector3 localPosition = this.cachedTransform.localPosition;
		localPosition.z = Mathf.Round(localPosition.z);
		localPosition.x = Mathf.Round(localPosition.x);
		localPosition.y = Mathf.Round(localPosition.y);
		this.cachedTransform.localPosition = localPosition;
		Vector3 localScale = this.cachedTransform.localScale;
		this.cachedTransform.localScale = new Vector3(Mathf.Sign(localScale.x), Mathf.Sign(localScale.y), 1f);
	}

	protected virtual void OnStart()
	{
	}

	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}
}
