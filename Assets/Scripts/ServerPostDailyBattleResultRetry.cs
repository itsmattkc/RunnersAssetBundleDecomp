using System;
using UnityEngine;

public class ServerPostDailyBattleResultRetry : ServerRetryProcess
{
	public ServerPostDailyBattleResultRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerPostDailyBattleResult(this.m_callbackObject);
		}
	}
}
