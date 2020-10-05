using System;
using UnityEngine;

public class ServerGetDailyBattleStatusRetry : ServerRetryProcess
{
	public ServerGetDailyBattleStatusRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetDailyBattleStatus(this.m_callbackObject);
		}
	}
}
