using System;
using System.Collections.Generic;

public class ServerLoginBonusReward
{
	public List<ServerItemState> m_itemList;

	public ServerLoginBonusReward()
	{
		this.m_itemList = new List<ServerItemState>();
	}

	public void CopyTo(ServerLoginBonusReward dest)
	{
		foreach (ServerItemState current in this.m_itemList)
		{
			dest.m_itemList.Add(current);
		}
	}
}
