using System;
using UnityEngine;

public class ServerGetFirstLaunchChaoRetry : ServerRetryProcess
{
	public ServerGetFirstLaunchChaoRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetFirstLaunchChao(this.m_callbackObject);
		}
	}
}
