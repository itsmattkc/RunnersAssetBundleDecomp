using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	public static UIButton current;

	public Color disabledColor = Color.grey;

	public List<EventDelegate> onClick = new List<EventDelegate>();

	public bool isEnabled
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			Collider collider = base.collider;
			return collider && collider.enabled;
		}
		set
		{
			Collider collider = base.collider;
			if (collider != null)
			{
				collider.enabled = value;
			}
			base.enabled = value;
		}
	}

	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mStarted)
			{
				if (this.mHighlighted)
				{
					base.OnEnable();
				}
				else
				{
					this.UpdateColor(true, false);
				}
			}
		}
		else
		{
			this.UpdateColor(false, true);
		}
	}

	protected override void OnDisable()
	{
		if (this.mStarted)
		{
			this.UpdateColor(false, false);
		}
	}

	public override void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			base.OnHover(isOver);
		}
	}

	public override void OnPress(bool isPressed)
	{
		if (this.isEnabled)
		{
			base.OnPress(isPressed);
		}
	}

	private void OnClick()
	{
		if (this.isEnabled)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	public void UpdateColor(bool shouldBeEnabled, bool immediate)
	{
		if (this.tweenTarget)
		{
			if (!this.mStarted)
			{
				this.mStarted = true;
				base.Init();
			}
			Color color = (!shouldBeEnabled) ? this.disabledColor : base.defaultColor;
			TweenColor tweenColor = TweenColor.Begin(this.tweenTarget, 0.15f, color);
			if (immediate)
			{
				tweenColor.color = color;
				tweenColor.enabled = false;
			}
		}
	}
}
