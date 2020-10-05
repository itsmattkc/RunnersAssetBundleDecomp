using System;
using UnityEngine;

public class ServerUnlockedCharacterRetry : ServerRetryProcess
{
	public CharaType m_charaType;

	public ServerItem m_item;

	public ServerUnlockedCharacterRetry(CharaType charaType, ServerItem serverItem, GameObject callbackObject) : base(callbackObject)
	{
		this.m_charaType = charaType;
		this.m_item = serverItem;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerUnlockedCharacter(this.m_charaType, this.m_item, this.m_callbackObject);
		}
	}
}
