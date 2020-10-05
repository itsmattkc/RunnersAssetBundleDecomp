using System;
using UnityEngine;

public class ServerGetOptionUserResultRetry : ServerRetryProcess
{
	public ServerGetOptionUserResultRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerOptionUserResult(this.m_callbackObject);
		}
	}
}
