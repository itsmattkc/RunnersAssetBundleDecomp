using System;
using UnityEngine;

public class ServerGetEventRaidBossUserListRetry : ServerRetryProcess
{
	private int m_eventId;

	private long m_raidBossId;

	public ServerGetEventRaidBossUserListRetry(int eventId, long raidBossId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_raidBossId = raidBossId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventRaidBossUserList(this.m_eventId, this.m_raidBossId, this.m_callbackObject);
		}
	}
}
