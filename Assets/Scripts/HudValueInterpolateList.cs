using System;
using System.Collections.Generic;

public class HudValueInterpolateList
{
	private List<HudValueInterpolate> m_interpolateList;

	private bool m_isEnd;

	private long m_lastValue;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public HudValueInterpolateList()
	{
		this.m_interpolateList = new List<HudValueInterpolate>();
		this.m_lastValue = 0L;
	}

	public void Add(int startValue, int endValue, float interpolateTime)
	{
		HudValueInterpolate hudValueInterpolate = new HudValueInterpolate();
		hudValueInterpolate.Setup((long)startValue, (long)endValue, interpolateTime);
		this.m_interpolateList.Add(hudValueInterpolate);
		this.m_isEnd = false;
	}

	public void Reset()
	{
		this.m_isEnd = false;
		if (this.m_interpolateList == null)
		{
			return;
		}
		foreach (HudValueInterpolate current in this.m_interpolateList)
		{
			if (current != null)
			{
				current.Reset();
			}
		}
	}

	public void Clear()
	{
		if (this.m_interpolateList == null)
		{
			return;
		}
		this.m_interpolateList.Clear();
	}

	public void ForceEnd()
	{
		if (this.m_interpolateList == null)
		{
			return;
		}
		int count = this.m_interpolateList.Count;
		if (count == 0)
		{
			return;
		}
		HudValueInterpolate hudValueInterpolate = this.m_interpolateList[count - 1];
		this.m_lastValue = hudValueInterpolate.ForceEnd();
		this.m_interpolateList.Clear();
	}

	public long Update(float deltaTime)
	{
		if (this.m_isEnd)
		{
			return this.m_lastValue;
		}
		HudValueInterpolate hudValueInterpolate = this.m_interpolateList[0];
		long lastValue = hudValueInterpolate.Update(deltaTime);
		if (hudValueInterpolate.IsEnd)
		{
			this.m_interpolateList.Remove(hudValueInterpolate);
			if (this.m_interpolateList.Count == 0)
			{
				this.m_isEnd = true;
			}
		}
		this.m_lastValue = lastValue;
		return this.m_lastValue;
	}
}
