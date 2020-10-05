using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerQuickModeStartActRetry : ServerRetryProcess
{
	public List<ItemType> m_modifiersItem;

	public List<BoostItemType> m_modifiersBoostItem;

	public List<string> m_distanceFriendIdList;

	public bool m_tutorial;

	public ServerQuickModeStartActRetry(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, bool tutorial, GameObject callbackObject) : base(callbackObject)
	{
		this.m_modifiersItem = modifiersItem;
		this.m_modifiersBoostItem = modifiersBoostItem;
		this.m_tutorial = tutorial;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerQuickModeStartAct(this.m_modifiersItem, this.m_modifiersBoostItem, this.m_tutorial, this.m_callbackObject);
		}
	}
}
