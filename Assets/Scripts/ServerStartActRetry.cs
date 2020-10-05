using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerStartActRetry : ServerRetryProcess
{
	public List<ItemType> m_modifiersItem;

	public List<BoostItemType> m_modifiersBoostItem;

	public List<string> m_distanceFriendIdList;

	public bool m_tutorial;

	private int? m_eventId;

	public ServerStartActRetry(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, List<string> distanceFriendIdList, bool tutorial, int? eventId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_modifiersItem = modifiersItem;
		this.m_modifiersBoostItem = modifiersBoostItem;
		this.m_distanceFriendIdList = distanceFriendIdList;
		this.m_tutorial = tutorial;
		this.m_eventId = eventId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerStartAct(this.m_modifiersItem, this.m_modifiersBoostItem, this.m_distanceFriendIdList, this.m_tutorial, this.m_eventId, this.m_callbackObject);
		}
	}
}
