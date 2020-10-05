using System;
using UnityEngine;

public class ServerGetInformationRetry : ServerRetryProcess
{
	public ServerGetInformationRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetInformation(this.m_callbackObject);
		}
	}
}
