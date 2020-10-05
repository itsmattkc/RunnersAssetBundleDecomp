using System;

public class ServerEventReward : ServerItemState
{
	private int m_rewardId;

	private long m_param;

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

	public ServerEventReward()
	{
		this.m_rewardId = 0;
		this.m_param = 0L;
	}

	public void CopyTo(ServerEventReward to)
	{
		base.CopyTo(to);
		to.m_rewardId = this.m_rewardId;
		to.m_param = this.m_param;
	}
}
