using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Scroll Bar")]
public class UIScrollBar : UIWidgetContainer
{
	public enum Direction
	{
		Horizontal,
		Vertical
	}

	public delegate void OnDragFinished();

	public static UIScrollBar current;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	public UIScrollBar.OnDragFinished onDragFinished;

	[HideInInspector, SerializeField]
	private UISprite mBG;

	[HideInInspector, SerializeField]
	private UISprite mFG;

	[HideInInspector, SerializeField]
	private UIScrollBar.Direction mDir;

	[HideInInspector, SerializeField]
	private bool mInverted;

	[HideInInspector, SerializeField]
	private float mScroll;

	[HideInInspector, SerializeField]
	private float mSize = 1f;

	private Transform mTrans;

	private bool mIsDirty;

	private Camera mCam;

	private Vector2 mScreenPos = Vector2.zero;

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

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

	public UISprite background
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	public UISprite foreground
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	public UIScrollBar.Direction direction
	{
		get
		{
			return this.mDir;
		}
		set
		{
			if (this.mDir != value)
			{
				this.mDir = value;
				this.mIsDirty = true;
				if (this.mBG != null)
				{
					int width = this.mBG.width;
					int height = this.mBG.height;
					if ((this.mDir == UIScrollBar.Direction.Vertical && width > height) || (this.mDir == UIScrollBar.Direction.Horizontal && width < height))
					{
						this.mBG.width = height;
						this.mBG.height = width;
						this.ForceUpdate();
						if (this.mBG.collider != null)
						{
							NGUITools.AddWidgetCollider(this.mBG.gameObject);
						}
						if (this.mFG.collider != null)
						{
							NGUITools.AddWidgetCollider(this.mFG.gameObject);
						}
					}
				}
			}
		}
	}

	public bool inverted
	{
		get
		{
			return this.mInverted;
		}
		set
		{
			if (this.mInverted != value)
			{
				this.mInverted = value;
				this.mIsDirty = true;
			}
		}
	}

	public float value
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mScroll != num)
			{
				this.mScroll = num;
				this.mIsDirty = true;
				if (this.onChange != null)
				{
					UIScrollBar.current = this;
					EventDelegate.Execute(this.onChange);
					UIScrollBar.current = null;
				}
			}
		}
	}

	[Obsolete("Use 'value' instead")]
	public float scrollValue
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	public float barSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mSize != num)
			{
				this.mSize = num;
				this.mIsDirty = true;
				if (this.onChange != null)
				{
					UIScrollBar.current = this;
					EventDelegate.Execute(this.onChange);
					UIScrollBar.current = null;
				}
			}
		}
	}

	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG != null)
			{
				return this.mBG.alpha;
			}
			return 0f;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				NGUITools.SetActiveSelf(this.mFG.gameObject, this.mFG.alpha > 0.001f);
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				NGUITools.SetActiveSelf(this.mBG.gameObject, this.mBG.alpha > 0.001f);
			}
		}
	}

	private void CenterOnPos(Vector2 localPos)
	{
		if (this.mBG == null || this.mFG == null)
		{
			return;
		}
		Bounds bounds = NGUIMath.CalculateRelativeInnerBounds(this.cachedTransform, this.mBG);
		Bounds bounds2 = NGUIMath.CalculateRelativeInnerBounds(this.cachedTransform, this.mFG);
		if (this.mDir == UIScrollBar.Direction.Horizontal)
		{
			float num = bounds.size.x - bounds2.size.x;
			float num2 = num * 0.5f;
			float num3 = bounds.center.x - num2;
			float num4 = (num <= 0f) ? 0f : ((localPos.x - num3) / num);
			this.value = ((!this.mInverted) ? num4 : (1f - num4));
		}
		else
		{
			float num5 = bounds.size.y - bounds2.size.y;
			float num6 = num5 * 0.5f;
			float num7 = bounds.center.y - num6;
			float num8 = (num5 <= 0f) ? 0f : (1f - (localPos.y - num7) / num5);
			this.value = ((!this.mInverted) ? num8 : (1f - num8));
		}
	}

	private void Reposition(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		float distance;
		if (!plane.Raycast(ray, out distance))
		{
			return;
		}
		this.CenterOnPos(cachedTransform.InverseTransformPoint(ray.GetPoint(distance)));
	}

	private void OnPressBackground(GameObject go, bool isPressed)
	{
		this.mCam = UICamera.currentCamera;
		this.Reposition(UICamera.lastTouchPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	private void OnDragBackground(GameObject go, Vector2 delta)
	{
		this.mCam = UICamera.currentCamera;
		this.Reposition(UICamera.lastTouchPosition);
	}

	private void OnPressForeground(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.mCam = UICamera.currentCamera;
			Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.mFG.cachedTransform);
			this.mScreenPos = this.mCam.WorldToScreenPoint(bounds.center);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	private void OnDragForeground(GameObject go, Vector2 delta)
	{
		this.mCam = UICamera.currentCamera;
		this.Reposition(this.mScreenPos + UICamera.currentTouch.totalDelta);
	}

	private void Start()
	{
		if (this.background != null && this.background.collider != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(this.background.gameObject);
			UIEventListener expr_39 = uIEventListener;
			expr_39.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_39.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
			UIEventListener expr_5B = uIEventListener;
			expr_5B.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_5B.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		}
		if (this.foreground != null && this.foreground.collider != null)
		{
			UIEventListener uIEventListener2 = UIEventListener.Get(this.foreground.gameObject);
			UIEventListener expr_B5 = uIEventListener2;
			expr_B5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_B5.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener expr_D7 = uIEventListener2;
			expr_D7.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_D7.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
		if (this.onChange != null)
		{
			UIScrollBar.current = this;
			EventDelegate.Execute(this.onChange);
			UIScrollBar.current = null;
		}
		this.ForceUpdate();
	}

	private void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	public void ForceUpdate()
	{
		this.mIsDirty = false;
		if (this.mBG != null && this.mFG != null)
		{
			this.mSize = Mathf.Clamp01(this.mSize);
			this.mScroll = Mathf.Clamp01(this.mScroll);
			Vector4 border = this.mBG.border;
			Vector4 border2 = this.mFG.border;
			Vector2 vector = new Vector2(Mathf.Max(0f, (float)this.mBG.width - border.x - border.z), Mathf.Max(0f, (float)this.mBG.height - border.y - border.w));
			float num = (!this.mInverted) ? this.mScroll : (1f - this.mScroll);
			if (this.mDir == UIScrollBar.Direction.Horizontal)
			{
				Vector2 vector2 = new Vector2(vector.x * this.mSize, vector.y);
				this.mFG.pivot = UIWidget.Pivot.Left;
				this.mBG.pivot = UIWidget.Pivot.Left;
				this.mBG.cachedTransform.localPosition = Vector3.zero;
				this.mFG.cachedTransform.localPosition = new Vector3(border.x - border2.x + (vector.x - vector2.x) * num, 0f, 0f);
				this.mFG.width = Mathf.RoundToInt(vector2.x + border2.x + border2.z);
				this.mFG.height = Mathf.RoundToInt(vector2.y + border2.y + border2.w);
				if (num < 0.999f && num > 0.001f)
				{
					this.mFG.MakePixelPerfect();
				}
				if (this.mFG.collider != null)
				{
					NGUITools.AddWidgetCollider(this.mFG.gameObject);
				}
			}
			else
			{
				Vector2 vector3 = new Vector2(vector.x, vector.y * this.mSize);
				this.mFG.pivot = UIWidget.Pivot.Top;
				this.mBG.pivot = UIWidget.Pivot.Top;
				this.mBG.cachedTransform.localPosition = Vector3.zero;
				this.mFG.cachedTransform.localPosition = new Vector3(0f, -border.y + border2.y - (vector.y - vector3.y) * num, 0f);
				this.mFG.width = Mathf.RoundToInt(vector3.x + border2.x + border2.z);
				this.mFG.height = Mathf.RoundToInt(vector3.y + border2.y + border2.w);
				if (num < 0.999f && num > 0.001f)
				{
					this.mFG.MakePixelPerfect();
				}
				if (this.mFG.collider != null)
				{
					NGUITools.AddWidgetCollider(this.mFG.gameObject);
				}
			}
		}
	}
}
