using System;
using UnityEngine;

public class ServerActRetryRetry : ServerRetryProcess
{
	public ServerActRetryRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerActRetry(this.m_callbackObject);
		}
	}
}
