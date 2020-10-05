using System;
using UnityEngine;

public class ServerRequestEnergyRetry : ServerRetryProcess
{
	public string m_friendId;

	public ServerRequestEnergyRetry(string friendId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_friendId = friendId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerRequestEnergy(this.m_friendId, this.m_callbackObject);
		}
	}
}
