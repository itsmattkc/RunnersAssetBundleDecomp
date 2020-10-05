using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggle"), ExecuteInEditMode]
public class UIToggle : UIWidgetContainer
{
	public static BetterList<UIToggle> list = new BetterList<UIToggle>();

	public static UIToggle current;

	public int group;

	public UIWidget activeSprite;

	public Animation activeAnimation;

	public bool startsActive;

	public bool instantTween;

	public bool optionCanBeNone;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	[HideInInspector, SerializeField]
	private Transform radioButtonRoot;

	[HideInInspector, SerializeField]
	private bool startsChecked;

	[HideInInspector, SerializeField]
	private UISprite checkSprite;

	[HideInInspector, SerializeField]
	private Animation checkAnimation;

	[HideInInspector, SerializeField]
	private GameObject eventReceiver;

	[HideInInspector, SerializeField]
	private string functionName = "OnActivate";

	private bool mIsActive = true;

	private bool mStarted;

	public bool value
	{
		get
		{
			return this.mIsActive;
		}
		set
		{
			if (this.group == 0 || value || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

	[Obsolete("Use 'value' instead")]
	public bool isChecked
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

	private void OnEnable()
	{
		UIToggle.list.Add(this);
	}

	private void OnDisable()
	{
		UIToggle.list.Remove(this);
	}

	private void Start()
	{
		this.mIsActive = !this.startsActive;
		this.mStarted = true;
		this.Set(this.startsActive);
	}

	private void OnClick()
	{
		if (base.enabled)
		{
			this.value = !this.value;
		}
	}

	private void Set(bool state)
	{
		if (!this.mStarted)
		{
			this.mIsActive = state;
			this.startsActive = state;
			if (this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!state) ? 0f : 1f);
			}
		}
		else if (this.mIsActive != state)
		{
			if (this.group != 0 && state)
			{
				int i = 0;
				int size = UIToggle.list.size;
				while (i < size)
				{
					UIToggle uIToggle = UIToggle.list[i];
					if (uIToggle != this && uIToggle.group == this.group)
					{
						uIToggle.Set(false);
					}
					i++;
				}
			}
			this.mIsActive = state;
			if (this.activeSprite != null)
			{
				if (this.instantTween)
				{
					this.activeSprite.alpha = ((!this.mIsActive) ? 0f : 1f);
				}
				else
				{
					TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, (!this.mIsActive) ? 0f : 1f);
				}
			}
			UIToggle.current = this;
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mIsActive, SendMessageOptions.DontRequireReceiver);
			}
			UIToggle.current = null;
			if (this.activeAnimation != null)
			{
				ActiveAnimation.Play(this.activeAnimation, (!state) ? Direction.Reverse : Direction.Forward);
			}
		}
	}
}
