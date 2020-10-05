using System;
using UnityEngine;

public class ServerGetRingItemListRetry : ServerRetryProcess
{
	public ServerGetRingItemListRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetRingItemList(this.m_callbackObject);
		}
	}
}
