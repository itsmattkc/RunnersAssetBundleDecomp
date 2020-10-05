using System;
using UnityEngine;

public class ServerRetrievePlayerStateRetry : ServerRetryProcess
{
	public ServerRetrievePlayerStateRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerRetrievePlayerState(this.m_callbackObject);
		}
	}
}
