using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Panel"), ExecuteInEditMode]
public class UIPanel : MonoBehaviour
{
	public enum DebugInfo
	{
		None,
		Gizmos,
		Geometry
	}

	public delegate void OnChangeDelegate();

	public static BetterList<UIPanel> list = new BetterList<UIPanel>();

	public UIPanel.OnChangeDelegate onChange;

	public bool showInPanelTool = true;

	public bool generateNormals;

	public bool widgetsAreStatic;

	public bool cullWhileDragging;

	[HideInInspector]
	public Matrix4x4 worldToLocal = Matrix4x4.identity;

	[HideInInspector, SerializeField]
	private float mAlpha = 1f;

	[HideInInspector, SerializeField]
	private UIDrawCall.Clipping mClipping;

	[HideInInspector, SerializeField]
	private Vector4 mClipRange = Vector4.zero;

	[HideInInspector, SerializeField]
	private Vector2 mClipSoftness = new Vector2(40f, 40f);

	[HideInInspector, SerializeField]
	private int mDepth;

	private static bool mFullRebuild = false;

	private static BetterList<Vector3> mVerts = new BetterList<Vector3>();

	private static BetterList<Vector3> mNorms = new BetterList<Vector3>();

	private static BetterList<Vector4> mTans = new BetterList<Vector4>();

	private static BetterList<Vector2> mUvs = new BetterList<Vector2>();

	private static BetterList<Color32> mCols = new BetterList<Color32>();

	private GameObject mGo;

	private Transform mTrans;

	private Camera mCam;

	private int mLayer = -1;

	private float mCullTime;

	private float mUpdateTime;

	private float mMatrixTime;

	private static float[] mTemp = new float[4];

	private Vector2 mMin = Vector2.zero;

	private Vector2 mMax = Vector2.zero;

	private UIPanel[] mChildPanels;

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

	public float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mAlpha != num)
			{
				this.mAlpha = num;
				for (int i = 0; i < UIDrawCall.list.size; i++)
				{
					UIDrawCall uIDrawCall = UIDrawCall.list[i];
					if (uIDrawCall != null && uIDrawCall.panel == this)
					{
						uIDrawCall.isDirty = true;
					}
				}
				for (int j = 0; j < UIWidget.list.size; j++)
				{
					UIWidget uIWidget = UIWidget.list[j];
					if (uIWidget.panel == this)
					{
						uIWidget.MarkAsChangedLite();
					}
				}
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
				UIPanel.mFullRebuild = true;
				for (int i = 0; i < UIDrawCall.list.size; i++)
				{
					UIDrawCall uIDrawCall = UIDrawCall.list[i];
					if (uIDrawCall != null)
					{
						uIDrawCall.isDirty = true;
					}
				}
				for (int j = 0; j < UIWidget.list.size; j++)
				{
					UIWidget.list[j].MarkAsChangedLite();
				}
				UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
			}
		}
	}

	public int drawCallCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < UIDrawCall.list.size; i++)
			{
				UIDrawCall uIDrawCall = UIDrawCall.list[i];
				if (!(uIDrawCall.panel != this))
				{
					num++;
				}
			}
			return num;
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mClipping = value;
				this.mMatrixTime = 0f;
				this.UpdateDrawcalls();
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (this.mClipRange != value)
			{
				this.mCullTime = ((this.mCullTime != 0f) ? (Time.realtimeSinceStartup + 0.15f) : 0.001f);
				this.mClipRange = value;
				this.mMatrixTime = 0f;
				this.UpdateDrawcalls();
			}
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
				this.UpdateDrawcalls();
			}
		}
	}

	public static int CompareFunc(UIPanel a, UIPanel b)
	{
		if (a != null && b != null)
		{
			if (a.mDepth < b.mDepth)
			{
				return -1;
			}
			if (a.mDepth > b.mDepth)
			{
				return 1;
			}
		}
		else
		{
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
		}
		return 0;
	}

	public void SetAlphaRecursive(float val, bool rebuildList)
	{
		if (rebuildList || this.mChildPanels == null)
		{
			this.mChildPanels = base.GetComponentsInChildren<UIPanel>(true);
		}
		int i = 0;
		int num = this.mChildPanels.Length;
		while (i < num)
		{
			this.mChildPanels[i].alpha = val;
			i++;
		}
	}

	private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float num = Mathf.Min(UIPanel.mTemp);
		float num2 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float num3 = Mathf.Min(UIPanel.mTemp);
		float num4 = Mathf.Max(UIPanel.mTemp);
		return num2 >= this.mMin.x && num4 >= this.mMin.y && num <= this.mMax.x && num3 <= this.mMax.y;
	}

	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		this.UpdateTransformMatrix();
		Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
		return vector.x >= this.mMin.x && vector.y >= this.mMin.y && vector.x <= this.mMax.x && vector.y <= this.mMax.y;
	}

	public bool IsVisible(UIWidget w)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (!w.enabled || !NGUITools.GetActive(w.cachedGameObject) || w.alpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		Vector3[] worldCorners = w.worldCorners;
		return this.IsVisible(worldCorners[0], worldCorners[1], worldCorners[2], worldCorners[3]);
	}

	public static void SetDirty()
	{
		UIPanel.mFullRebuild = true;
	}

	private UIDrawCall GetDrawCall(int index, Material mat)
	{
		if (index < UIDrawCall.list.size)
		{
			UIDrawCall uIDrawCall = UIDrawCall.list.buffer[index];
			if (uIDrawCall != null && uIDrawCall.panel == this && uIDrawCall.material == mat && uIDrawCall.mainTexture == mat.mainTexture)
			{
				return uIDrawCall;
			}
			int i = UIDrawCall.list.size;
			while (i > index)
			{
				UIDrawCall dc = UIDrawCall.list.buffer[--i];
				UIPanel.DestroyDrawCall(dc, i);
			}
		}
		GameObject gameObject = new GameObject("_UIDrawCall [" + mat.name + "]");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		gameObject.layer = this.cachedGameObject.layer;
		UIDrawCall uIDrawCall2 = gameObject.AddComponent<UIDrawCall>();
		uIDrawCall2.material = mat;
		uIDrawCall2.renderQueue = UIDrawCall.list.size;
		uIDrawCall2.panel = this;
		UIDrawCall.list.Add(uIDrawCall2);
		return uIDrawCall2;
	}

	private void Awake()
	{
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
	}

	private void Start()
	{
		this.mLayer = this.mGo.layer;
		UICamera uICamera = UICamera.FindCameraForLayer(this.mLayer);
		this.mCam = ((!(uICamera != null)) ? NGUITools.FindCameraForLayer(this.mLayer) : uICamera.cachedCamera);
	}

	private void OnEnable()
	{
		UIPanel.mFullRebuild = true;
		UIPanel.list.Add(this);
		UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
	}

	private void OnDisable()
	{
		int i = UIDrawCall.list.size;
		while (i > 0)
		{
			UIDrawCall uIDrawCall = UIDrawCall.list.buffer[--i];
			if (uIDrawCall != null && uIDrawCall.panel == this)
			{
				UIPanel.DestroyDrawCall(uIDrawCall, i);
			}
		}
		UIPanel.list.Remove(this);
	}

	private void UpdateTransformMatrix()
	{
		if (this.mUpdateTime == 0f || this.mMatrixTime != this.mUpdateTime)
		{
			this.mMatrixTime = this.mUpdateTime;
			this.worldToLocal = this.cachedTransform.worldToLocalMatrix;
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				Vector2 a = new Vector2(this.mClipRange.z, this.mClipRange.w);
				if (a.x == 0f)
				{
					a.x = ((!(this.mCam == null)) ? this.mCam.pixelWidth : ((float)Screen.width));
				}
				if (a.y == 0f)
				{
					a.y = ((!(this.mCam == null)) ? this.mCam.pixelHeight : ((float)Screen.height));
				}
				a *= 0.5f;
				this.mMin.x = this.mClipRange.x - a.x;
				this.mMin.y = this.mClipRange.y - a.y;
				this.mMax.x = this.mClipRange.x + a.x;
				this.mMax.y = this.mClipRange.y + a.y;
			}
		}
	}

	private void UpdateDrawcalls()
	{
		Vector4 zero = Vector4.zero;
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			zero = new Vector4(this.mClipRange.x, this.mClipRange.y, this.mClipRange.z * 0.5f, this.mClipRange.w * 0.5f);
		}
		if (zero.z == 0f)
		{
			zero.z = (float)Screen.width * 0.5f;
		}
		if (zero.w == 0f)
		{
			zero.w = (float)Screen.height * 0.5f;
		}
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsWebPlayer || platform == RuntimePlatform.WindowsEditor)
		{
			zero.x -= 0.5f;
			zero.y += 0.5f;
		}
		Transform cachedTransform = this.cachedTransform;
		int i = 0;
		while (i < UIDrawCall.list.size)
		{
			UIDrawCall uIDrawCall = UIDrawCall.list.buffer[i];
			if (uIDrawCall == null)
			{
				UIDrawCall.list.RemoveAt(i);
			}
			else
			{
				if (uIDrawCall.panel == this)
				{
					uIDrawCall.clipping = this.mClipping;
					uIDrawCall.clipRange = zero;
					uIDrawCall.clipSoftness = this.mClipSoftness;
					Transform transform = uIDrawCall.transform;
					transform.position = cachedTransform.position;
					transform.rotation = cachedTransform.rotation;
					transform.localScale = cachedTransform.lossyScale;
				}
				i++;
			}
		}
	}

	private void LateUpdate()
	{
		if (UIPanel.list[0] != this)
		{
			return;
		}
		for (int i = 0; i < UIPanel.list.size; i++)
		{
			UIPanel uIPanel = UIPanel.list[i];
			uIPanel.mUpdateTime = RealTime.time;
			uIPanel.UpdateTransformMatrix();
			uIPanel.UpdateLayers();
			uIPanel.UpdateWidgets();
		}
		if (UIPanel.mFullRebuild)
		{
			UIWidget.list.Sort(new Comparison<UIWidget>(UIWidget.CompareFunc));
			UIPanel.Fill();
		}
		else
		{
			int j = 0;
			while (j < UIDrawCall.list.size)
			{
				UIDrawCall uIDrawCall = UIDrawCall.list[j];
				if (uIDrawCall.isDirty && !UIPanel.Fill(uIDrawCall))
				{
					UIPanel.DestroyDrawCall(uIDrawCall, j);
				}
				else
				{
					j++;
				}
			}
		}
		for (int k = 0; k < UIPanel.list.size; k++)
		{
			UIPanel uIPanel2 = UIPanel.list[k];
			uIPanel2.UpdateDrawcalls();
		}
		UIPanel.mFullRebuild = false;
	}

	private static void DestroyDrawCall(UIDrawCall dc, int index)
	{
		if (dc != null)
		{
			UIDrawCall.list.RemoveAt(index);
			NGUITools.DestroyImmediate(dc.gameObject);
		}
	}

	private void UpdateLayers()
	{
		if (this.mLayer != this.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			UICamera uICamera = UICamera.FindCameraForLayer(this.mLayer);
			this.mCam = ((!(uICamera != null)) ? NGUITools.FindCameraForLayer(this.mLayer) : uICamera.cachedCamera);
			UIPanel.SetChildLayer(this.cachedTransform, this.mLayer);
			int i = 0;
			int size = UIDrawCall.list.size;
			while (i < size)
			{
				UIDrawCall uIDrawCall = UIDrawCall.list[i];
				if (uIDrawCall != null && uIDrawCall.panel == this)
				{
					uIDrawCall.gameObject.layer = this.mLayer;
				}
				i++;
			}
		}
	}

	private void UpdateWidgets()
	{
		bool forceVisible = !this.cullWhileDragging && (this.clipping == UIDrawCall.Clipping.None || this.mCullTime > this.mUpdateTime);
		bool flag = false;
		int i = 0;
		int size = UIWidget.list.size;
		while (i < size)
		{
			UIWidget uIWidget = UIWidget.list[i];
			if (uIWidget.enabled && uIWidget.panel == this && uIWidget.UpdateGeometry(this, forceVisible))
			{
				flag = true;
				if (!UIPanel.mFullRebuild)
				{
					UIDrawCall drawCall = uIWidget.drawCall;
					if (drawCall != null)
					{
						drawCall.isDirty = true;
					}
					else
					{
						UIPanel.mFullRebuild = true;
					}
				}
			}
			i++;
		}
		if (flag && this.onChange != null)
		{
			this.onChange();
		}
	}

	public void Refresh()
	{
		UIPanel.mFullRebuild = true;
		UIPanel.list[0].LateUpdate();
	}

	public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		float num = this.clipRange.z * 0.5f;
		float num2 = this.clipRange.w * 0.5f;
		Vector2 minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = new Vector2(this.clipRange.x - num, this.clipRange.y - num2);
		Vector2 maxArea = new Vector2(this.clipRange.x + num, this.clipRange.y + num2);
		if (this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += this.clipSoftness.x;
			minArea.y += this.clipSoftness.y;
			maxArea.x -= this.clipSoftness.x;
			maxArea.y -= this.clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 b = this.CalculateConstrainOffset(targetBounds.min, targetBounds.max);
		if (b.magnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += b;
				targetBounds.center += b;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + b, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(this.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref bounds, immediate);
	}

	private static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.GetComponent<UIPanel>() == null)
			{
				if (child.GetComponent<UIWidget>() != null)
				{
					child.gameObject.layer = layer;
				}
				UIPanel.SetChildLayer(child, layer);
			}
		}
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		Transform y = trans;
		UIPanel uIPanel = null;
		while (uIPanel == null && trans != null)
		{
			uIPanel = trans.GetComponent<UIPanel>();
			if (uIPanel != null)
			{
				break;
			}
			if (trans.parent == null)
			{
				break;
			}
			trans = trans.parent;
		}
		if (createIfMissing && uIPanel == null && trans != y)
		{
			UIPanel.mFullRebuild = true;
			uIPanel = trans.gameObject.AddComponent<UIPanel>();
			UIPanel.SetChildLayer(uIPanel.cachedTransform, uIPanel.cachedGameObject.layer);
		}
		return uIPanel;
	}

	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, true);
	}

	private static void Fill()
	{
		int i = UIDrawCall.list.size;
		while (i > 0)
		{
			UIPanel.DestroyDrawCall(UIDrawCall.list[--i], i);
		}
		int num = 0;
		UIPanel uIPanel = null;
		Material material = null;
		UIDrawCall uIDrawCall = null;
		int j = 0;
		while (j < UIWidget.list.size)
		{
			UIWidget uIWidget = UIWidget.list[j];
			if (uIWidget == null)
			{
				UIWidget.list.RemoveAt(j);
			}
			else
			{
				if (uIWidget.isVisible && uIWidget.hasVertices)
				{
					if (uIPanel != uIWidget.panel || material != uIWidget.material)
					{
						if (uIPanel != null && material != null && UIPanel.mVerts.size != 0)
						{
							uIPanel.SubmitDrawCall(uIDrawCall);
							uIDrawCall = null;
						}
						uIPanel = uIWidget.panel;
						material = uIWidget.material;
					}
					if (uIPanel != null && material != null)
					{
						if (uIDrawCall == null)
						{
							uIDrawCall = uIPanel.GetDrawCall(num++, material);
						}
						uIWidget.drawCall = uIDrawCall;
						if (uIPanel.generateNormals)
						{
							uIWidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, UIPanel.mNorms, UIPanel.mTans);
						}
						else
						{
							uIWidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, null, null);
						}
					}
				}
				else
				{
					uIWidget.drawCall = null;
				}
				j++;
			}
		}
		if (UIPanel.mVerts.size != 0)
		{
			uIPanel.SubmitDrawCall(uIDrawCall);
		}
	}

	private void SubmitDrawCall(UIDrawCall dc)
	{
		dc.Set(UIPanel.mVerts, (!this.generateNormals) ? null : UIPanel.mNorms, (!this.generateNormals) ? null : UIPanel.mTans, UIPanel.mUvs, UIPanel.mCols);
		UIPanel.mVerts.Clear();
		UIPanel.mNorms.Clear();
		UIPanel.mTans.Clear();
		UIPanel.mUvs.Clear();
		UIPanel.mCols.Clear();
	}

	private static bool Fill(UIDrawCall dc)
	{
		if (dc != null)
		{
			dc.isDirty = false;
			int i = 0;
			while (i < UIWidget.list.size)
			{
				UIWidget uIWidget = UIWidget.list[i];
				if (uIWidget == null)
				{
					UIWidget.list.RemoveAt(i);
				}
				else
				{
					if (uIWidget.drawCall == dc)
					{
						if (uIWidget.isVisible && uIWidget.hasVertices)
						{
							if (dc.panel.generateNormals)
							{
								uIWidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, UIPanel.mNorms, UIPanel.mTans);
							}
							else
							{
								uIWidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, null, null);
							}
						}
						else
						{
							uIWidget.drawCall = null;
						}
					}
					i++;
				}
			}
			if (UIPanel.mVerts.size != 0)
			{
				dc.Set(UIPanel.mVerts, (!dc.panel.generateNormals) ? null : UIPanel.mNorms, (!dc.panel.generateNormals) ? null : UIPanel.mTans, UIPanel.mUvs, UIPanel.mCols);
				UIPanel.mVerts.Clear();
				UIPanel.mNorms.Clear();
				UIPanel.mTans.Clear();
				UIPanel.mUvs.Clear();
				UIPanel.mCols.Clear();
				return true;
			}
		}
		return false;
	}
}
