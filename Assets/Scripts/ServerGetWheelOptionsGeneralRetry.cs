using System;
using UnityEngine;

public class ServerGetWheelOptionsGeneralRetry : ServerRetryProcess
{
	private int m_spinId;

	private int m_eventId;

	public ServerGetWheelOptionsGeneralRetry(int eventId, int spinId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_spinId = spinId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetWheelOptionsGeneral(this.m_eventId, this.m_spinId, this.m_callbackObject);
		}
	}
}
