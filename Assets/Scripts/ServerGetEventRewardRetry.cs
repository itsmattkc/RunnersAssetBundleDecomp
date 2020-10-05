using System;
using UnityEngine;

public class ServerGetEventRewardRetry : ServerRetryProcess
{
	private int m_eventId;

	public ServerGetEventRewardRetry(int eventId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventReward(this.m_eventId, this.m_callbackObject);
		}
	}
}
