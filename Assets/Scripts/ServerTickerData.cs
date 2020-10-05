using System;

public class ServerTickerData
{
	private long m_id;

	private long m_start;

	private long m_end;

	private string m_param;

	public long Id
	{
		get
		{
			return this.m_id;
		}
	}

	public long Start
	{
		get
		{
			return this.m_start;
		}
	}

	public long End
	{
		get
		{
			return this.m_end;
		}
	}

	public string Param
	{
		get
		{
			return this.m_param;
		}
	}

	public void Init(long id, long start, long end, string param)
	{
		this.m_id = id;
		this.m_start = start;
		this.m_end = end;
		this.m_param = param;
	}

	public void CopyTo(ServerTickerData to)
	{
		to.m_id = this.m_id;
		to.m_start = this.m_start;
		to.m_end = this.m_end;
		to.m_param = this.m_param;
	}
}
