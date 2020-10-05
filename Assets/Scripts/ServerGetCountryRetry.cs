using System;
using UnityEngine;

public class ServerGetCountryRetry : ServerRetryProcess
{
	public ServerGetCountryRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetCountry(this.m_callbackObject);
		}
	}
}
