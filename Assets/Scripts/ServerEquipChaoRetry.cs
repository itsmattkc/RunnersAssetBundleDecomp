using System;
using UnityEngine;

public class ServerEquipChaoRetry : ServerRetryProcess
{
	public int m_mainChaoId;

	public int m_subChaoId;

	public ServerEquipChaoRetry(int mainChaoId, int subChaoId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_mainChaoId = mainChaoId;
		this.m_subChaoId = subChaoId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerEquipChao(this.m_mainChaoId, this.m_subChaoId, this.m_callbackObject);
		}
	}
}
