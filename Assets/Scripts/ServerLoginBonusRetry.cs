using System;
using UnityEngine;

public class ServerLoginBonusRetry : ServerRetryProcess
{
	public ServerLoginBonusRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerLoginBonus(this.m_callbackObject);
		}
	}
}
