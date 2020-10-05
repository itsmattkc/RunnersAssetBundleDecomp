using System;

public class PanelFade
{
	private const int InterpolateEndValue = 10000;

	private UIPanel m_panel;

	private HudValueInterpolate m_hudInterpolate;

	public void Setup(UIPanel panel)
	{
		this.m_panel = panel;
		this.m_hudInterpolate = new HudValueInterpolate();
	}

	public void PlayStart(float fadeTime, bool isFadeIn)
	{
		if (isFadeIn)
		{
			this.m_hudInterpolate.Setup(0L, 10000L, fadeTime);
		}
		else
		{
			this.m_hudInterpolate.Setup(10000L, 0L, fadeTime);
		}
	}

	public void Update(float deltaTime)
	{
		if (this.m_hudInterpolate == null)
		{
			return;
		}
		if (!this.m_hudInterpolate.IsEnd)
		{
			long num = this.m_hudInterpolate.Update(deltaTime);
			this.m_panel.alpha = (float)(num / 10000L);
		}
	}

	public bool IsEndFade()
	{
		return this.m_hudInterpolate == null || this.m_hudInterpolate.IsEnd;
	}
}
