using System;

public class GameResultScoreInterporate
{
	private UILabel m_label;

	private HudValueInterpolate m_interpolate;

	private long m_score;

	public bool IsEnd
	{
		get
		{
			return this.m_interpolate.IsEnd;
		}
	}

	public bool IsPause
	{
		get
		{
			return this.m_interpolate.IsPause;
		}
		set
		{
			this.m_interpolate.IsPause = value;
		}
	}

	public float CurrentTime
	{
		get
		{
			return this.m_interpolate.CurrentTime;
		}
	}

	public GameResultScoreInterporate()
	{
		this.m_label = null;
		this.m_interpolate = null;
	}

	public void Setup(UILabel label)
	{
		this.m_label = label;
		this.m_interpolate = new HudValueInterpolate();
		this.m_score = 0L;
	}

	public void AddScore(long addScore)
	{
		this.m_score += addScore;
		this.SetLabelStartValue(this.m_score);
	}

	public void SetLabelStartValue(long startValue)
	{
		if (this.m_label == null)
		{
			return;
		}
		this.m_label.text = GameResultUtility.GetScoreFormat(startValue);
	}

	public void PlayStart(long startValue, long endValue, float interpolateTime)
	{
		if (this.m_interpolate == null)
		{
			return;
		}
		this.m_interpolate.Setup(startValue, endValue, interpolateTime);
		this.m_interpolate.Reset();
		if (startValue == endValue)
		{
			this.PlaySkip();
		}
	}

	public void PlaySkip()
	{
		if (this.m_interpolate == null)
		{
			return;
		}
		long val = this.m_interpolate.ForceEnd();
		this.m_label.text = GameResultUtility.GetScoreFormat(val);
	}

	public long Update(float deltaTime)
	{
		if (this.m_interpolate == null)
		{
			return 0L;
		}
		if (this.m_label == null)
		{
			return 0L;
		}
		long num = this.m_interpolate.Update(deltaTime);
		this.m_label.text = GameResultUtility.GetScoreFormat(num);
		return num;
	}
}
