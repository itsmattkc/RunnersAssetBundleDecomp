using System;
using UnityEngine;

public class ServerGetDailyBattleDataHistoryRetry : ServerRetryProcess
{
	private int m_count;

	public ServerGetDailyBattleDataHistoryRetry(int count, GameObject callbackObject) : base(callbackObject)
	{
		this.m_count = count;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetDailyBattleDataHistory(this.m_count, this.m_callbackObject);
		}
	}
}
