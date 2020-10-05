using System;
using UnityEngine;

public class ServerSetInviteCodeRetry : ServerRetryProcess
{
	public string m_friendId;

	public ServerSetInviteCodeRetry(string friendId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_friendId = friendId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSetInviteCode(this.m_friendId, this.m_callbackObject);
		}
	}
}
