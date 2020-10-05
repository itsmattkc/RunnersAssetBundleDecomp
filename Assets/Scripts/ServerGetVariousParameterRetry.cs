using System;
using UnityEngine;

public class ServerGetVariousParameterRetry : ServerRetryProcess
{
	public ServerGetVariousParameterRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetVariousParameter(this.m_callbackObject);
		}
	}
}
