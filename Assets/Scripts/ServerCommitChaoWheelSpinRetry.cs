using System;
using UnityEngine;

public class ServerCommitChaoWheelSpinRetry : ServerRetryProcess
{
	private int m_count = 1;

	public ServerCommitChaoWheelSpinRetry(int count, GameObject callbackObject) : base(callbackObject)
	{
		this.m_count = count;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerCommitChaoWheelSpin(this.m_count, this.m_callbackObject);
		}
	}
}
