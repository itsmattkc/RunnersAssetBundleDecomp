using System;
using UnityEngine;

public class ServerGetCampaignListRetry : ServerRetryProcess
{
	public ServerGetCampaignListRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetCampaignList(this.m_callbackObject);
		}
	}
}
