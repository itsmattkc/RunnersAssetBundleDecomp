using System;
using UnityEngine;

public class ServerGetEventUserRaidBossStateRetry : ServerRetryProcess
{
	private int m_eventId;

	public ServerGetEventUserRaidBossStateRetry(int eventId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventUserRaidBossState(this.m_eventId, this.m_callbackObject);
		}
	}
}
