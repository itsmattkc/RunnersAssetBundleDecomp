using System;
using UnityEngine;

public class ServerRingExchangeRetry : ServerRetryProcess
{
	public int m_itemId;

	public int m_itemNum;

	public ServerRingExchangeRetry(int itemId, int itemNum, GameObject callbackObject) : base(callbackObject)
	{
		this.m_itemId = itemId;
		this.m_itemNum = itemNum;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerRingExchange(this.m_itemId, this.m_itemNum, this.m_callbackObject);
		}
	}
}
