using System;

public class EventMission
{
	private string m_name = string.Empty;

	private long m_point;

	private int m_reward;

	private int m_index;

	public string name
	{
		get
		{
			return this.m_name;
		}
	}

	public long point
	{
		get
		{
			return this.m_point;
		}
	}

	public int reward
	{
		get
		{
			return this.m_reward;
		}
	}

	public int index
	{
		get
		{
			return this.m_index;
		}
	}

	public EventMission(string name, long point, int reward, int index)
	{
		this.m_name = name;
		this.m_point = point;
		this.m_reward = reward;
		this.m_index = index;
	}

	public bool IsAttainment(long point)
	{
		bool result = false;
		if (point >= this.m_point)
		{
			result = true;
		}
		return result;
	}
}
