using System;

public class ServerEventState
{
	private long m_param;

	private int m_rewardId;

	public long Param
	{
		get
		{
			return this.m_param;
		}
		set
		{
			this.m_param = value;
		}
	}

	public int RewardId
	{
		get
		{
			return this.m_rewardId;
		}
		set
		{
			this.m_rewardId = value;
		}
	}

	public ServerEventState()
	{
		this.m_param = 0L;
		this.m_rewardId = 0;
	}

	public void CopyTo(ServerEventState to)
	{
		to.m_param = this.m_param;
		to.m_rewardId = this.m_rewardId;
	}
}
