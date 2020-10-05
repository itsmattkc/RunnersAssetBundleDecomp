using System;
using UnityEngine;

public class ServerGetEventUserRaidBossListRetry : ServerRetryProcess
{
	private int m_eventId;

	public ServerGetEventUserRaidBossListRetry(int eventId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventUserRaidBossList(this.m_eventId, this.m_callbackObject);
		}
	}
}
