using System;
using UnityEngine;

public class ServerGetChaoRentalStatesRetry : ServerRetryProcess
{
	public string[] m_friendId;

	public ServerGetChaoRentalStatesRetry(string[] friendId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_friendId = friendId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetChaoRentalStates(this.m_friendId, this.m_callbackObject);
		}
	}
}
