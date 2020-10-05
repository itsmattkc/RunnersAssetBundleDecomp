using System;
using UnityEngine;

public class ServerCommitWheelSpinGeneralRetry : ServerRetryProcess
{
	private int m_eventId;

	private int m_spinId;

	private int m_spinCostItemId;

	private int m_spinNum;

	public ServerCommitWheelSpinGeneralRetry(int eventId, int spinId, int spinCostItemId, int spinNum, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_spinId = spinId;
		this.m_spinCostItemId = spinCostItemId;
		this.m_spinNum = spinNum;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerCommitWheelSpinGeneral(this.m_eventId, this.m_spinId, this.m_spinCostItemId, this.m_spinNum, this.m_callbackObject);
		}
	}
}
