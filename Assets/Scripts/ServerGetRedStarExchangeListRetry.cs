using System;
using UnityEngine;

public class ServerGetRedStarExchangeListRetry : ServerRetryProcess
{
	public int m_itemType;

	public ServerGetRedStarExchangeListRetry(int itemType, GameObject callbackObject) : base(callbackObject)
	{
		this.m_itemType = itemType;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetRedStarExchangeList(this.m_itemType, this.m_callbackObject);
		}
	}
}
