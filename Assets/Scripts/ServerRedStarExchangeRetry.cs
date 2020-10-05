using System;
using UnityEngine;

public class ServerRedStarExchangeRetry : ServerRetryProcess
{
	public int m_storeItemId;

	public ServerRedStarExchangeRetry(int storeItemId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_storeItemId = storeItemId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerRedStarExchange(this.m_storeItemId, this.m_callbackObject);
		}
	}
}
