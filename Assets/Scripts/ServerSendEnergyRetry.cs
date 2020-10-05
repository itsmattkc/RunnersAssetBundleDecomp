using System;
using UnityEngine;

public class ServerSendEnergyRetry : ServerRetryProcess
{
	public string m_friendId;

	public ServerSendEnergyRetry(string friendId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_friendId = friendId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSendEnergy(this.m_friendId, this.m_callbackObject);
		}
	}
}
