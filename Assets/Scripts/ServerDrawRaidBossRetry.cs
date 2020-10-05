using System;
using UnityEngine;

public class ServerDrawRaidBossRetry : ServerRetryProcess
{
	private int m_eventId;

	private long m_score;

	public ServerDrawRaidBossRetry(int eventId, long score, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_score = score;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerDrawRaidBoss(this.m_eventId, this.m_score, this.m_callbackObject);
		}
	}
}
