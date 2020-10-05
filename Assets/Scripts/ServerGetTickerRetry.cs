using System;
using UnityEngine;

public class ServerGetTickerRetry : ServerRetryProcess
{
	public ServerGetTickerRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetTicker(this.m_callbackObject);
		}
	}
}
