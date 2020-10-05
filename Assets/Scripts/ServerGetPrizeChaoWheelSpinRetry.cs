using System;
using UnityEngine;

public class ServerGetPrizeChaoWheelSpinRetry : ServerRetryProcess
{
	private int m_chaoWheelSpinType;

	public ServerGetPrizeChaoWheelSpinRetry(int chaoWheelSpinType, GameObject callbackObject) : base(callbackObject)
	{
		this.m_chaoWheelSpinType = chaoWheelSpinType;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetPrizeChaoWheelSpin(this.m_chaoWheelSpinType, this.m_callbackObject);
		}
	}
}
