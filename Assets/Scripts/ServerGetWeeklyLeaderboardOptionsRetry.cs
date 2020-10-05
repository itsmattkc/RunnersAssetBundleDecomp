using System;
using UnityEngine;

public class ServerGetWeeklyLeaderboardOptionsRetry : ServerRetryProcess
{
	private int m_mode;

	public ServerGetWeeklyLeaderboardOptionsRetry(int mode, GameObject callbackObject) : base(callbackObject)
	{
		this.m_mode = mode;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetWeeklyLeaderboardOptions(this.m_mode, this.m_callbackObject);
		}
	}
}
