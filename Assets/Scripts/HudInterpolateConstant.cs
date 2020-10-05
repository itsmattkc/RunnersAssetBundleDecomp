using System;

public class HudInterpolateConstant
{
	private int m_startValue;

	private int m_endValue;

	private float m_addValuePerSec;

	private float m_currentValue;

	private float m_prevValue;

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

	public int CurrentValue
	{
		get
		{
			return (int)this.m_currentValue;
		}
	}

	public int PrevValue
	{
		get
		{
			return (int)this.m_prevValue;
		}
	}

	public HudInterpolateConstant()
	{
		this.m_startValue = 0;
		this.m_endValue = 0;
		this.m_addValuePerSec = 0f;
		this.m_currentValue = 0f;
		this.m_isEnd = false;
		this.m_pauseFlag = false;
	}

	public void Setup(int startValue, int endValue, float addValuePerSec)
	{
		this.m_startValue = startValue;
		this.m_currentValue = (float)this.m_startValue;
		this.m_prevValue = this.m_currentValue;
		this.m_endValue = endValue;
		this.m_addValuePerSec = addValuePerSec;
		this.m_isEnd = false;
		this.m_pauseFlag = false;
	}

	public void Reset()
	{
		this.m_isEnd = false;
	}

	public int ForceEnd()
	{
		this.m_isEnd = true;
		this.m_currentValue = (float)this.m_endValue;
		return this.m_endValue;
	}

	public int SetForceValue(int value)
	{
		this.m_prevValue = this.m_currentValue;
		if (value < this.m_endValue)
		{
			this.m_currentValue = (float)value;
		}
		else
		{
			this.m_currentValue = (float)this.m_endValue;
			this.m_isEnd = true;
		}
		return (int)this.m_currentValue;
	}

	public int Update(float deltaTime)
	{
		if (this.m_isEnd)
		{
			return this.m_endValue;
		}
		if (!this.m_pauseFlag)
		{
			this.m_prevValue = this.m_currentValue;
			float num = this.m_addValuePerSec * deltaTime;
			this.m_currentValue += num;
		}
		if (this.m_currentValue >= (float)this.m_endValue)
		{
			this.m_currentValue = (float)this.m_endValue;
			this.m_isEnd = true;
			return this.m_endValue;
		}
		return (int)this.m_currentValue;
	}
}
