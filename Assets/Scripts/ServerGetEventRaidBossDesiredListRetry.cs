using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerGetEventRaidBossDesiredListRetry : ServerRetryProcess
{
	public int m_eventId;

	public long m_raidBossId;

	public List<string> m_friendIdList;

	public ServerGetEventRaidBossDesiredListRetry(int eventId, long raidBossId, List<string> friendIdList, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_raidBossId = raidBossId;
		this.m_friendIdList = friendIdList;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventRaidBossDesiredList(this.m_eventId, this.m_raidBossId, this.m_friendIdList, this.m_callbackObject);
		}
	}
}
