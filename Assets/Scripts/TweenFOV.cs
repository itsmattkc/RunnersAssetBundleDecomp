using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Field of View"), RequireComponent(typeof(Camera))]
public class TweenFOV : UITweener
{
	public float from = 45f;

	public float to = 45f;

	private Camera mCam;

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.camera;
			}
			return this.mCam;
		}
	}

	public float fov
	{
		get
		{
			return this.cachedCamera.fieldOfView;
		}
		set
		{
			this.cachedCamera.fieldOfView = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.cachedCamera.fieldOfView = this.from * (1f - factor) + this.to * factor;
	}

	public static TweenFOV Begin(GameObject go, float duration, float to)
	{
		TweenFOV tweenFOV = UITweener.Begin<TweenFOV>(go, duration);
		tweenFOV.from = tweenFOV.fov;
		tweenFOV.to = to;
		if (duration <= 0f)
		{
			tweenFOV.Sample(1f, true);
			tweenFOV.enabled = false;
		}
		return tweenFOV;
	}
}
