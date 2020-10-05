using System;
using UnityEngine;

public class ServerSetNoahIdRetry : ServerRetryProcess
{
	public string m_noahId;

	public ServerSetNoahIdRetry(string noahId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_noahId = noahId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSetNoahId(this.m_noahId, this.m_callbackObject);
		}
	}
}
