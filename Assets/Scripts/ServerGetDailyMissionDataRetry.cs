using System;
using UnityEngine;

public class ServerGetDailyMissionDataRetry : ServerRetryProcess
{
	public ServerGetDailyMissionDataRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetDailyMissionData(this.m_callbackObject);
		}
	}
}
