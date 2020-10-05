using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : MonoBehaviour
{
	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public Transform target;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public bool restrictWithinPanel;

	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	private Plane mPlane;

	private Vector3 mLastPos;

	private UIPanel mPanel;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private int mTouchID;

	private bool mStarted;

	private void FindPanel()
	{
		this.mPanel = ((!(this.target != null)) ? null : UIPanel.Find(this.target.transform, false));
		if (this.mPanel == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mMomentum = Vector3.zero;
					this.mPressed = true;
					this.mStarted = false;
					this.mScroll = 0f;
					if (this.restrictWithinPanel && this.mPanel == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, this.target);
					}
					SpringPosition component = this.target.GetComponent<SpringPosition>();
					if (component != null)
					{
						component.enabled = false;
					}
					Transform transform = UICamera.currentCamera.transform;
					this.mPlane = new Plane(((!(this.mPanel != null)) ? transform.rotation : this.mPanel.cachedTransform.rotation) * Vector3.back, UICamera.lastHit.point);
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
				{
					this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, false);
				}
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float distance = 0f;
			if (this.mPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector = Vector3.zero;
				}
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.target.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.target.TransformDirection(vector);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				if (this.restrictWithinPanel)
				{
					Vector3 localPosition = this.target.localPosition;
					this.target.position += vector;
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - localPosition);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.mPanel.clipping != UIDrawCall.Clipping.None && this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.mMomentum = Vector3.zero;
						this.mScroll = 0f;
					}
				}
				else
				{
					this.target.position += vector;
				}
			}
		}
	}

	private void LateUpdate()
	{
		float deltaTime = RealTime.deltaTime;
		if (this.target == null)
		{
			return;
		}
		if (this.mPressed)
		{
			SpringPosition component = this.target.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
			this.mScroll = 0f;
		}
		else
		{
			this.mMomentum += this.scale * (-this.mScroll * 0.05f);
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
			if (this.mMomentum.magnitude > 0.0001f)
			{
				if (this.mPanel == null)
				{
					this.FindPanel();
				}
				if (this.mPanel != null)
				{
					this.target.position += NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
					if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
					{
						this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, this.target);
						if (!this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
						{
							SpringPosition component2 = this.target.GetComponent<SpringPosition>();
							if (component2 != null)
							{
								component2.enabled = false;
							}
						}
					}
					return;
				}
			}
			else
			{
				this.mScroll = 0f;
			}
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}
}
