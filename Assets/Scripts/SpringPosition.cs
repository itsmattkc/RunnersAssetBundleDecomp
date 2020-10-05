using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
	public delegate void OnFinished(SpringPosition spring);

	public Vector3 target = Vector3.zero;

	public float strength = 10f;

	public bool worldSpace;

	public bool ignoreTimeScale;

	public GameObject eventReceiver;

	public string callWhenFinished;

	public SpringPosition.OnFinished onFinished;

	private Transform mTrans;

	private float mThreshold;

	private void Start()
	{
		this.mTrans = base.transform;
	}

	private void Update()
	{
		float deltaTime = (!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime;
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.position).magnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.position).magnitude)
			{
				this.mTrans.position = this.target;
				if (this.onFinished != null)
				{
					this.onFinished(this);
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.localPosition).magnitude * 0.001f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).magnitude)
			{
				this.mTrans.localPosition = this.target;
				if (this.onFinished != null)
				{
					this.onFinished(this);
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				base.enabled = false;
			}
		}
	}

	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.enabled)
		{
			springPosition.mThreshold = 0f;
			springPosition.enabled = true;
		}
		return springPosition;
	}
}
