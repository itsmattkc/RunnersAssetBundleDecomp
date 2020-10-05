using System;
using UnityEngine;

public class ServerPostGameResultsRetry : ServerRetryProcess
{
	private ServerGameResults m_results;

	public ServerPostGameResultsRetry(ServerGameResults results, GameObject callbackObject) : base(callbackObject)
	{
		this.m_results = results;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerPostGameResults(this.m_results, this.m_callbackObject);
		}
	}
}
