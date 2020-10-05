using System;
using UnityEngine;

public class ServerGetMessageListRetry : ServerRetryProcess
{
	public ServerGetMessageListRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetMessageList(this.m_callbackObject);
		}
	}
}
