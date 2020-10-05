using System;
using System.Collections.Generic;

public class ServerTickerInfo
{
	private bool m_existNewData;

	private int m_tickerIndex;

	private List<ServerTickerData> m_data = new List<ServerTickerData>();

	public bool ExistNewData
	{
		get
		{
			return this.m_existNewData;
		}
		set
		{
			this.m_existNewData = value;
		}
	}

	public int TickerIndex
	{
		get
		{
			return this.m_tickerIndex;
		}
		set
		{
			if (this.m_tickerIndex != value)
			{
				this.m_existNewData = true;
				this.m_tickerIndex = value;
			}
			else
			{
				this.m_existNewData = false;
			}
		}
	}

	public List<ServerTickerData> Data
	{
		get
		{
			return this.m_data;
		}
		private set
		{
		}
	}

	public void Init(int tickerIndex)
	{
		this.m_existNewData = true;
		this.m_tickerIndex = tickerIndex;
	}
}
