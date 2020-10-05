using System;
using UnityEngine;

public class ServerEventUpdateGameResultsRetry : ServerRetryProcess
{
	private ServerEventGameResults m_results;

	public ServerEventUpdateGameResultsRetry(ServerEventGameResults results, GameObject callbackObject) : base(callbackObject)
	{
		this.m_results = results;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerEventUpdateGameResults(this.m_results, this.m_callbackObject);
		}
	}
}
