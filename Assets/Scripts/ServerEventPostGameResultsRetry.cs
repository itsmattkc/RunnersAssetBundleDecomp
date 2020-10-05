using System;
using UnityEngine;

public class ServerEventPostGameResultsRetry : ServerRetryProcess
{
	public int m_eventId;

	public int m_numRaidbossRings;

	public ServerEventPostGameResultsRetry(int eventId, int numRaidbossRings, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_numRaidbossRings = numRaidbossRings;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerEventPostGameResults(this.m_eventId, this.m_numRaidbossRings, this.m_callbackObject);
		}
	}
}
