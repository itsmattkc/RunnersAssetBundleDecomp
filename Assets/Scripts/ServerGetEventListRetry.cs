using System;
using UnityEngine;

public class ServerGetEventListRetry : ServerRetryProcess
{
	public ServerGetEventListRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventList(this.m_callbackObject);
		}
	}
}
