using System;
using UnityEngine;

public class ServerUseSubCharacterRetry : ServerRetryProcess
{
	public bool m_useFlag;

	public ServerUseSubCharacterRetry(bool useFlag, GameObject callbackObject) : base(callbackObject)
	{
		this.m_useFlag = useFlag;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerUseSubCharacter(this.m_useFlag, this.m_callbackObject);
		}
	}
}
