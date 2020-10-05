using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	[Range(0f, 1f)]
	public float from = 1f;

	[Range(0f, 1f)]
	public float to = 1f;

	private Transform mTrans;

	private UIWidget mWidget;

	private UIPanel mPanel;

	public float alpha
	{
		get
		{
			if (this.mWidget != null)
			{
				return this.mWidget.alpha;
			}
			if (this.mPanel != null)
			{
				return this.mPanel.alpha;
			}
			return 0f;
		}
		set
		{
			if (this.mWidget != null)
			{
				this.mWidget.alpha = value;
			}
			else if (this.mPanel != null)
			{
				this.mPanel.alpha = value;
			}
		}
	}

	private void Awake()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		if (this.mPanel == null)
		{
			this.mWidget = base.GetComponentInChildren<UIWidget>();
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.alpha = Mathf.Lerp(this.from, this.to, factor);
	}

	public static TweenAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
		tweenAlpha.from = tweenAlpha.alpha;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}
}
