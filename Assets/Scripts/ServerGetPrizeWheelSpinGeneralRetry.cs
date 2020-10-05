using System;
using UnityEngine;

public class ServerGetPrizeWheelSpinGeneralRetry : ServerRetryProcess
{
	private int m_spinType;

	private int m_eventId;

	public ServerGetPrizeWheelSpinGeneralRetry(int eventId, int spinType, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_spinType = spinType;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetPrizeWheelSpinGeneral(this.m_eventId, this.m_spinType, this.m_callbackObject);
		}
	}
}
