using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Draggable Panel"), ExecuteInEditMode, RequireComponent(typeof(UIPanel))]
public class UIDraggablePanel : MonoBehaviour
{
	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public enum ShowCondition
	{
		Always,
		OnlyIfNeeded,
		WhenDragging
	}

	public delegate void OnDragFinished();

	public UIDraggablePanel.DragEffect dragEffect = UIDraggablePanel.DragEffect.MomentumAndSpring;

	public bool restrictWithinPanel = true;

	public bool disableDragIfFits;

	public bool smoothDragStart = true;

	public bool repositionClipping;

	public bool iOSDragEmulation = true;

	public float scrollWheelFactor;

	public float momentumAmount = 35f;

	public UIScrollBar horizontalScrollBar;

	public UIScrollBar verticalScrollBar;

	public UIDraggablePanel.ShowCondition showScrollBars = UIDraggablePanel.ShowCondition.OnlyIfNeeded;

	public Vector3 scale = new Vector3(1f, 0f, 0f);

	public Vector2 relativePositionOnReset = Vector2.zero;

	public UIDraggablePanel.OnDragFinished onDragFinished;

	private Transform mTrans;

	private UIPanel mPanel;

	private Plane mPlane;

	private Vector3 mLastPos;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private bool mCalculatedBounds;

	private bool mShouldMove;

	private bool mIgnoreCallbacks;

	private int mDragID = -10;

	private Vector2 mDragStartOffset = Vector2.zero;

	private bool mDragStarted;

	public UIPanel panel
	{
		get
		{
			return this.mPanel;
		}
	}

	public Bounds bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
			}
			return this.mBounds;
		}
	}

	public bool shouldMoveHorizontally
	{
		get
		{
			float num = this.bounds.size.x;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.x * 2f;
			}
			return num > this.mPanel.clipRange.z;
		}
	}

	public bool shouldMoveVertically
	{
		get
		{
			float num = this.bounds.size.y;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.y * 2f;
			}
			return num > this.mPanel.clipRange.w;
		}
	}

	private bool shouldMove
	{
		get
		{
			if (!this.disableDragIfFits)
			{
				return true;
			}
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			Vector4 clipRange = this.mPanel.clipRange;
			Bounds bounds = this.bounds;
			float num = (clipRange.z != 0f) ? (clipRange.z * 0.5f) : ((float)Screen.width);
			float num2 = (clipRange.w != 0f) ? (clipRange.w * 0.5f) : ((float)Screen.height);
			if (!Mathf.Approximately(this.scale.x, 0f))
			{
				if (bounds.min.x < clipRange.x - num)
				{
					return true;
				}
				if (bounds.max.x > clipRange.x + num)
				{
					return true;
				}
			}
			if (!Mathf.Approximately(this.scale.y, 0f))
			{
				if (bounds.min.y < clipRange.y - num2)
				{
					return true;
				}
				if (bounds.max.y > clipRange.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
			this.mShouldMove = true;
		}
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mPanel = base.GetComponent<UIPanel>();
		if (Application.isPlaying)
		{
			UIPanel expr_28 = this.mPanel;
			expr_28.onChange = (UIPanel.OnChangeDelegate)Delegate.Combine(expr_28.onChange, new UIPanel.OnChangeDelegate(this.OnPanelChange));
		}
	}

	private void OnDestroy()
	{
		if (Application.isPlaying && this.mPanel != null)
		{
			UIPanel expr_21 = this.mPanel;
			expr_21.onChange = (UIPanel.OnChangeDelegate)Delegate.Remove(expr_21.onChange, new UIPanel.OnChangeDelegate(this.OnPanelChange));
		}
	}

	private void OnPanelChange()
	{
		this.UpdateScrollbars(true);
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			this.UpdateScrollbars(true);
			if (this.horizontalScrollBar != null)
			{
				this.horizontalScrollBar.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnHorizontalBar)));
				this.horizontalScrollBar.alpha = ((this.showScrollBars != UIDraggablePanel.ShowCondition.Always && !this.shouldMoveHorizontally) ? 0f : 1f);
			}
			if (this.verticalScrollBar != null)
			{
				this.verticalScrollBar.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnVerticalBar)));
				this.verticalScrollBar.alpha = ((this.showScrollBars != UIDraggablePanel.ShowCondition.Always && !this.shouldMoveVertically) ? 0f : 1f);
			}
		}
	}

	public bool RestrictWithinBounds(bool instant)
	{
		Vector3 vector = this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max);
		if (vector.magnitude > 0.001f)
		{
			if (!instant && this.dragEffect == UIDraggablePanel.DragEffect.MomentumAndSpring)
			{
				SpringPanel.Begin(this.mPanel.gameObject, this.mTrans.localPosition + vector, 13f);
			}
			else
			{
				this.MoveRelative(vector);
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	public void DisableSpring()
	{
		SpringPanel component = base.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void UpdateScrollbars(bool recalculateBounds)
	{
		if (this.mPanel == null)
		{
			return;
		}
		if (this.horizontalScrollBar != null || this.verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				this.mCalculatedBounds = false;
				this.mShouldMove = this.shouldMove;
			}
			Bounds bounds = this.bounds;
			Vector2 a = bounds.min;
			Vector2 a2 = bounds.max;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 clipSoftness = this.mPanel.clipSoftness;
				a -= clipSoftness;
				a2 += clipSoftness;
			}
			if (this.horizontalScrollBar != null && a2.x > a.x)
			{
				Vector4 clipRange = this.mPanel.clipRange;
				float num = clipRange.z * 0.5f;
				float num2 = clipRange.x - num - bounds.min.x;
				float num3 = bounds.max.x - num - clipRange.x;
				float num4 = a2.x - a.x;
				num2 = Mathf.Clamp01(num2 / num4);
				num3 = Mathf.Clamp01(num3 / num4);
				float num5 = num2 + num3;
				this.mIgnoreCallbacks = true;
				this.horizontalScrollBar.barSize = 1f - num5;
				this.horizontalScrollBar.value = ((num5 <= 0.001f) ? 0f : (num2 / num5));
				this.mIgnoreCallbacks = false;
			}
			if (this.verticalScrollBar != null && a2.y > a.y)
			{
				Vector4 clipRange2 = this.mPanel.clipRange;
				float num6 = clipRange2.w * 0.5f;
				float num7 = clipRange2.y - num6 - a.y;
				float num8 = a2.y - num6 - clipRange2.y;
				float num9 = a2.y - a.y;
				num7 = Mathf.Clamp01(num7 / num9);
				num8 = Mathf.Clamp01(num8 / num9);
				float num10 = num7 + num8;
				this.mIgnoreCallbacks = true;
				this.verticalScrollBar.barSize = 1f - num10;
				this.verticalScrollBar.value = ((num10 <= 0.001f) ? 0f : (1f - num7 / num10));
				this.mIgnoreCallbacks = false;
			}
		}
		else if (recalculateBounds)
		{
			this.mCalculatedBounds = false;
		}
	}

	public void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		this.DisableSpring();
		Bounds bounds = this.bounds;
		if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.y)
		{
			return;
		}
		Vector4 clipRange = this.mPanel.clipRange;
		float num = clipRange.z * 0.5f;
		float num2 = clipRange.w * 0.5f;
		float num3 = bounds.min.x + num;
		float num4 = bounds.max.x - num;
		float num5 = bounds.min.y + num2;
		float num6 = bounds.max.y - num2;
		if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= this.mPanel.clipSoftness.x;
			num4 += this.mPanel.clipSoftness.x;
			num5 -= this.mPanel.clipSoftness.y;
			num6 += this.mPanel.clipSoftness.y;
		}
		float num7 = Mathf.Lerp(num3, num4, x);
		float num8 = Mathf.Lerp(num6, num5, y);
		if (!updateScrollbars)
		{
			Vector3 localPosition = this.mTrans.localPosition;
			if (this.scale.x != 0f)
			{
				localPosition.x += clipRange.x - num7;
			}
			if (this.scale.y != 0f)
			{
				localPosition.y += clipRange.y - num8;
			}
			this.mTrans.localPosition = localPosition;
		}
		clipRange.x = num7;
		clipRange.y = num8;
		this.mPanel.clipRange = clipRange;
		if (updateScrollbars)
		{
			this.UpdateScrollbars(false);
		}
	}

	public void ResetPosition()
	{
		this.mCalculatedBounds = false;
		this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, false);
		this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
	}

	private void OnHorizontalBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			float x = (!(this.horizontalScrollBar != null)) ? 0f : this.horizontalScrollBar.value;
			float y = (!(this.verticalScrollBar != null)) ? 0f : this.verticalScrollBar.value;
			this.SetDragAmount(x, y, false);
		}
	}

	private void OnVerticalBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			float x = (!(this.horizontalScrollBar != null)) ? 0f : this.horizontalScrollBar.value;
			float y = (!(this.verticalScrollBar != null)) ? 0f : this.verticalScrollBar.value;
			this.SetDragAmount(x, y, false);
		}
	}

	public void MoveRelative(Vector3 relative)
	{
		this.mTrans.localPosition += relative;
		Vector4 clipRange = this.mPanel.clipRange;
		clipRange.x -= relative.x;
		clipRange.y -= relative.y;
		this.mPanel.clipRange = clipRange;
		this.UpdateScrollbars(false);
	}

	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 a = this.mTrans.InverseTransformPoint(absolute);
		Vector3 b = this.mTrans.InverseTransformPoint(Vector3.zero);
		this.MoveRelative(a - b);
	}

	public void Press(bool pressed)
	{
		if (this.smoothDragStart && pressed)
		{
			this.mDragStarted = false;
			this.mDragStartOffset = Vector2.zero;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (!pressed && this.mDragID == UICamera.currentTouchID)
			{
				this.mDragID = -10;
			}
			this.mCalculatedBounds = false;
			this.mShouldMove = this.shouldMove;
			if (!this.mShouldMove)
			{
				return;
			}
			this.mPressed = pressed;
			if (pressed)
			{
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				this.DisableSpring();
				this.mLastPos = UICamera.lastHit.point;
				this.mPlane = new Plane(this.mTrans.rotation * Vector3.back, this.mLastPos);
			}
			else
			{
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect == UIDraggablePanel.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(false);
				}
				if (this.onDragFinished != null)
				{
					this.onDragFinished();
				}
			}
		}
	}

	public void Drag()
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mShouldMove)
		{
			if (this.mDragID == -10)
			{
				this.mDragID = UICamera.currentTouchID;
			}
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			if (this.smoothDragStart && !this.mDragStarted)
			{
				this.mDragStarted = true;
				this.mDragStartOffset = UICamera.currentTouch.totalDelta;
			}
			Ray ray = (!this.smoothDragStart) ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - this.mDragStartOffset);
			float distance = 0f;
			if (this.mPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.mTrans.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.mTrans.TransformDirection(vector);
				}
				this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				if (!this.iOSDragEmulation)
				{
					this.MoveAbsolute(vector);
				}
				else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude > 0.001f)
				{
					this.MoveAbsolute(vector * 0.5f);
					this.mMomentum *= 0.5f;
				}
				else
				{
					this.MoveAbsolute(vector);
				}
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect != UIDraggablePanel.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(true);
				}
			}
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.scrollWheelFactor != 0f)
		{
			this.DisableSpring();
			this.mShouldMove = this.shouldMove;
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	private void LateUpdate()
	{
		if (this.repositionClipping)
		{
			this.repositionClipping = false;
			this.mCalculatedBounds = false;
			this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
		}
		if (!Application.isPlaying)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		if (this.showScrollBars != UIDraggablePanel.ShowCondition.Always)
		{
			bool flag = false;
			bool flag2 = false;
			if (this.showScrollBars != UIDraggablePanel.ShowCondition.WhenDragging || this.mDragID != -10 || this.mMomentum.magnitude > 0.01f)
			{
				flag = this.shouldMoveVertically;
				flag2 = this.shouldMoveHorizontally;
			}
			if (this.verticalScrollBar)
			{
				float num = this.verticalScrollBar.alpha;
				num += ((!flag) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num = Mathf.Clamp01(num);
				if (this.verticalScrollBar.alpha != num)
				{
					this.verticalScrollBar.alpha = num;
				}
			}
			if (this.horizontalScrollBar)
			{
				float num2 = this.horizontalScrollBar.alpha;
				num2 += ((!flag2) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num2 = Mathf.Clamp01(num2);
				if (this.horizontalScrollBar.alpha != num2)
				{
					this.horizontalScrollBar.alpha = num2;
				}
			}
		}
		if (this.mShouldMove && !this.mPressed)
		{
			this.mMomentum -= this.scale * (this.mScroll * 0.05f);
			if (this.mMomentum.magnitude > 0.0001f)
			{
				this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
				Vector3 absolute = NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
				this.MoveAbsolute(absolute);
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					this.RestrictWithinBounds(false);
				}
				if (this.mMomentum.magnitude < 0.0001f && this.onDragFinished != null)
				{
					this.onDragFinished();
				}
				return;
			}
			this.mScroll = 0f;
			this.mMomentum = Vector3.zero;
		}
		else
		{
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}
}
