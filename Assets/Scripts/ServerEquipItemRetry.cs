using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerEquipItemRetry : ServerRetryProcess
{
	public List<ItemType> m_items;

	public ServerEquipItemRetry(List<ItemType> items, GameObject callbackObject) : base(callbackObject)
	{
		this.m_items = items;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerEquipItem(this.m_items, this.m_callbackObject);
		}
	}
}
