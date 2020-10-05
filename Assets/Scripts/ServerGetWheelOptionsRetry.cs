using System;
using UnityEngine;

public class ServerGetWheelOptionsRetry : ServerRetryProcess
{
	public ServerGetWheelOptionsRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetWheelOptions(this.m_callbackObject);
		}
	}
}
