using System;
using System.Collections.Generic;

public class ServerLoginBonusRewardList
{
	public List<ServerLoginBonusReward> m_selectRewardList;

	public ServerLoginBonusRewardList()
	{
		this.m_selectRewardList = new List<ServerLoginBonusReward>();
	}

	public void CopyTo(ServerLoginBonusRewardList dest)
	{
		foreach (ServerLoginBonusReward current in this.m_selectRewardList)
		{
			dest.m_selectRewardList.Add(current);
		}
	}
}
