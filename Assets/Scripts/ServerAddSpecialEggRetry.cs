using System;
using UnityEngine;

public class ServerAddSpecialEggRetry : ServerRetryProcess
{
	public int m_numSpecialEgg;

	public ServerAddSpecialEggRetry(int numSpecialEgg, GameObject callbackObject) : base(callbackObject)
	{
		this.m_numSpecialEgg = numSpecialEgg;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerAddSpecialEgg(this.m_numSpecialEgg, this.m_callbackObject);
		}
	}
}
