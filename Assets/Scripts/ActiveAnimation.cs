using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Active Animation"), RequireComponent(typeof(Animation))]
public class ActiveAnimation : MonoBehaviour
{
	public static ActiveAnimation current;

	public List<EventDelegate> onFinished = new List<EventDelegate>();

	[HideInInspector]
	public GameObject eventReceiver;

	[HideInInspector]
	public string callWhenFinished;

	private Animation mAnim;

	private Direction mLastDirection;

	private Direction mDisableDirection;

	private bool mNotify;

	private bool mRealTime = true;

	public bool isPlaying
	{
		get
		{
			if (this.mAnim == null)
			{
				return false;
			}
			foreach (AnimationState animationState in this.mAnim)
			{
				if (this.mAnim.IsPlaying(animationState.name))
				{
					if (this.mLastDirection == Direction.Forward)
					{
						if (animationState.time < animationState.length)
						{
							bool result = true;
							return result;
						}
					}
					else
					{
						if (this.mLastDirection != Direction.Reverse)
						{
							bool result = true;
							return result;
						}
						if (animationState.time > 0f)
						{
							bool result = true;
							return result;
						}
					}
				}
			}
			return false;
		}
	}

	public void Reset()
	{
		if (this.mAnim != null)
		{
			foreach (AnimationState animationState in this.mAnim)
			{
				if (this.mLastDirection == Direction.Reverse)
				{
					animationState.time = animationState.length;
				}
				else if (this.mLastDirection == Direction.Forward)
				{
					animationState.time = 0f;
				}
			}
		}
	}

	private void Start()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	private void Update()
	{
		float num = (!this.mRealTime) ? Time.deltaTime : RealTime.deltaTime;
		if (num == 0f)
		{
			return;
		}
		if (this.mAnim != null)
		{
			bool flag = false;
			foreach (AnimationState animationState in this.mAnim)
			{
				if (this.mAnim.IsPlaying(animationState.name))
				{
					float num2 = animationState.speed * num;
					animationState.time += num2;
					if (num2 < 0f)
					{
						if (animationState.time > 0f)
						{
							flag = true;
						}
						else
						{
							animationState.time = 0f;
						}
					}
					else if (animationState.time < animationState.length)
					{
						flag = true;
					}
					else
					{
						animationState.time = animationState.length;
					}
				}
			}
			this.mAnim.Sample();
			if (flag)
			{
				return;
			}
			base.enabled = false;
			if (this.mNotify)
			{
				this.mNotify = false;
				ActiveAnimation.current = this;
				EventDelegate.Execute(this.onFinished);
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
				}
				ActiveAnimation.current = null;
				if (this.mDisableDirection != Direction.Toggle && this.mLastDirection == this.mDisableDirection)
				{
					NGUITools.SetActive(base.gameObject, false);
				}
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	private void Play(string clipName, Direction playDirection)
	{
		if (this.mAnim != null)
		{
			base.enabled = true;
			this.mAnim.enabled = false;
			if (playDirection == Direction.Toggle)
			{
				playDirection = ((this.mLastDirection == Direction.Forward) ? Direction.Reverse : Direction.Forward);
			}
			bool flag = string.IsNullOrEmpty(clipName);
			if (flag)
			{
				if (!this.mAnim.isPlaying)
				{
					this.mAnim.Play();
				}
			}
			else if (!this.mAnim.IsPlaying(clipName))
			{
				this.mAnim.Play(clipName);
			}
			foreach (AnimationState animationState in this.mAnim)
			{
				if (string.IsNullOrEmpty(clipName) || animationState.name == clipName)
				{
					float num = Mathf.Abs(animationState.speed);
					animationState.speed = num * (float)playDirection;
					if (playDirection == Direction.Reverse && animationState.time == 0f)
					{
						animationState.time = animationState.length;
					}
					else if (playDirection == Direction.Forward && animationState.time == animationState.length)
					{
						animationState.time = 0f;
					}
				}
			}
			this.mLastDirection = playDirection;
			this.mNotify = true;
			this.mAnim.Sample();
		}
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, enableBeforePlay, disableCondition, true);
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition, bool flagRealTime)
	{
		if (!NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				componentsInChildren[i].Refresh();
				i++;
			}
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mRealTime = flagRealTime;
		activeAnimation.mAnim = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		return activeAnimation;
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, bool flagRealTime)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable, flagRealTime);
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection, bool flagRealTime)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable, flagRealTime);
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable, true);
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable, true);
	}
}
