using System;
using UnityEngine;

public class ServerGetMenuDataRetry : ServerRetryProcess
{
	public ServerGetMenuDataRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetMenuData(this.m_callbackObject);
		}
	}
}
