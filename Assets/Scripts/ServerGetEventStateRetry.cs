using System;
using UnityEngine;

public class ServerGetEventStateRetry : ServerRetryProcess
{
	private int m_eventId;

	public ServerGetEventStateRetry(int eventId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventState(this.m_eventId, this.m_callbackObject);
		}
	}
}
