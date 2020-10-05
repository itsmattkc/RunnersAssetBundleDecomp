using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Orthographic Size"), RequireComponent(typeof(Camera))]
public class TweenOrthoSize : UITweener
{
	public float from = 1f;

	public float to = 1f;

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

	public float orthoSize
	{
		get
		{
			return this.cachedCamera.orthographicSize;
		}
		set
		{
			this.cachedCamera.orthographicSize = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.cachedCamera.orthographicSize = this.from * (1f - factor) + this.to * factor;
	}

	public static TweenOrthoSize Begin(GameObject go, float duration, float to)
	{
		TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
		tweenOrthoSize.from = tweenOrthoSize.orthoSize;
		tweenOrthoSize.to = to;
		if (duration <= 0f)
		{
			tweenOrthoSize.Sample(1f, true);
			tweenOrthoSize.enabled = false;
		}
		return tweenOrthoSize;
	}
}
