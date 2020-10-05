using System;
using UnityEngine;

public class ServerActRetryFreeRetry : ServerRetryProcess
{
	public ServerActRetryFreeRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerActRetryFree(this.m_callbackObject);
		}
	}
}
