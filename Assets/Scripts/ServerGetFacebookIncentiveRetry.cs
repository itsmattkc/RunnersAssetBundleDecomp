using System;
using UnityEngine;

public class ServerGetFacebookIncentiveRetry : ServerRetryProcess
{
	public int m_incentiveType;

	public int m_achievementCount;

	public ServerGetFacebookIncentiveRetry(int incentiveType, int achievementCount, GameObject callbackObject) : base(callbackObject)
	{
		this.m_incentiveType = incentiveType;
		this.m_achievementCount = achievementCount;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetFacebookIncentive(this.m_incentiveType, this.m_achievementCount, this.m_callbackObject);
		}
	}
}
