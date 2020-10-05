using System;
using UnityEngine;

public class ServerSetFacebookScopedIdRetry : ServerRetryProcess
{
	public string m_userId;

	public ServerSetFacebookScopedIdRetry(string userId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_userId = userId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSetFacebookScopedId(this.m_userId, this.m_callbackObject);
		}
	}
}
