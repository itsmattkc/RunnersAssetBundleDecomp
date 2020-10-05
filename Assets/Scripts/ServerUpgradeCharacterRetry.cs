using System;
using UnityEngine;

public class ServerUpgradeCharacterRetry : ServerRetryProcess
{
	public int m_characterId;

	public int m_abilityId;

	public ServerUpgradeCharacterRetry(int characterId, int abilityId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_characterId = characterId;
		this.m_abilityId = abilityId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerUpgradeCharacter(this.m_characterId, this.m_abilityId, this.m_callbackObject);
		}
	}
}
