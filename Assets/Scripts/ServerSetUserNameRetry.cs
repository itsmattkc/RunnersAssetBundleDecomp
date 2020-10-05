using System;
using UnityEngine;

public class ServerSetUserNameRetry : ServerRetryProcess
{
	public string m_userName;

	public ServerSetUserNameRetry(string userName, GameObject callbackObject) : base(callbackObject)
	{
		this.m_userName = userName;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSetUserName(this.m_userName, this.m_callbackObject);
		}
	}
}
