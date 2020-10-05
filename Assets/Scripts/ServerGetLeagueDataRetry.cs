using System;
using UnityEngine;

public class ServerGetLeagueDataRetry : ServerRetryProcess
{
	public int m_mode;

	public ServerGetLeagueDataRetry(int mode, GameObject callbackObject) : base(callbackObject)
	{
		this.m_mode = mode;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetLeagueData(this.m_mode, this.m_callbackObject);
		}
	}
}
