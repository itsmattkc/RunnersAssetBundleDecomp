using System;
using UnityEngine;

public class ServerGetLeagueOperatorDataRetry : ServerRetryProcess
{
	public int m_mode;

	public ServerGetLeagueOperatorDataRetry(int mode, GameObject callbackObject) : base(callbackObject)
	{
		this.m_mode = mode;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetLeagueOperatorData(this.m_mode, this.m_callbackObject);
		}
	}
}
