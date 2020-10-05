using System;
using UnityEngine;

public class ServerSetInviteHistoryRetry : ServerRetryProcess
{
	private string m_facebookIdHash;

	public ServerSetInviteHistoryRetry(string facebookIdHash, GameObject callbackObject) : base(callbackObject)
	{
		this.m_facebookIdHash = facebookIdHash;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSetInviteHistory(this.m_facebookIdHash, this.m_callbackObject);
		}
	}
}
