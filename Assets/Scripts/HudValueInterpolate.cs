using System;

public class HudValueInterpolate
{
	private long m_startValue;

	private long m_endValue;

	private float m_interpolateTime;

	private float m_currentTime;

	private bool m_isEnd;

	private bool m_pauseFlag;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public bool IsPause
	{
		get
		{
			return this.m_pauseFlag;
		}
		set
		{
			this.m_pauseFlag = value;
		}
	}

	public float CurrentTime
	{
		get
		{
			return this.m_currentTime;
		}
	}

	public HudValueInterpolate()
	{
		this.m_startValue = 0L;
		this.m_endValue = 0L;
		this.m_interpolateTime = 0f;
		this.m_currentTime = 0f;
		this.m_isEnd = false;
		this.m_pauseFlag = false;
	}

	public void Setup(long startValue, long endValue, float interpolateTime)
	{
		this.m_startValue = startValue;
		this.m_currentTime = 0f;
		this.m_endValue = endValue;
		this.m_interpolateTime = interpolateTime;
		this.m_isEnd = false;
		this.m_pauseFlag = false;
	}

	public void Reset()
	{
		this.m_isEnd = false;
	}

	public long ForceEnd()
	{
		this.m_isEnd = true;
		this.m_currentTime = this.m_interpolateTime;
		return this.m_endValue;
	}

	public long Update(float deltaTime)
	{
		if (this.m_isEnd)
		{
			return this.m_endValue;
		}
		if (!this.m_pauseFlag)
		{
			this.m_currentTime += deltaTime;
		}
		if (this.m_currentTime >= this.m_interpolateTime)
		{
			this.m_isEnd = true;
			return this.m_endValue;
		}
		long num = this.m_endValue - this.m_startValue;
		float num2 = this.m_currentTime / this.m_interpolateTime;
		return this.m_startValue + (long)((float)num * num2);
	}
}
