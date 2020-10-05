using System;
using UnityEngine;

public class ServerChangeCharacterRetry : ServerRetryProcess
{
	public int m_mainCharaId;

	public int m_subCharaId;

	public ServerChangeCharacterRetry(int mainCharaId, int subCharaId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_mainCharaId = mainCharaId;
		this.m_subCharaId = subCharaId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerChangeCharacter(this.m_mainCharaId, this.m_subCharaId, this.m_callbackObject);
		}
	}
}
