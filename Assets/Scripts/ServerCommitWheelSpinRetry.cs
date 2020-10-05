using System;
using UnityEngine;

public class ServerCommitWheelSpinRetry : ServerRetryProcess
{
	private int m_count = 1;

	public ServerCommitWheelSpinRetry(int count, GameObject callbackObject) : base(callbackObject)
	{
		this.m_count = count;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerCommitWheelSpin(this.m_count, this.m_callbackObject);
		}
	}
}
