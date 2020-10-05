using System;
using UnityEngine;

public class ServerResetDailyBattleMatchingRetry : ServerRetryProcess
{
	private int m_type;

	public ServerResetDailyBattleMatchingRetry(int type, GameObject callbackObject) : base(callbackObject)
	{
		this.m_type = type;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerResetDailyBattleMatching(this.m_type, this.m_callbackObject);
		}
	}
}
