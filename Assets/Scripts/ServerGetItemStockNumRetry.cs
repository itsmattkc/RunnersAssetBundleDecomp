using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerGetItemStockNumRetry : ServerRetryProcess
{
	private int m_eventId;

	private List<int> m_itemId;

	public ServerGetItemStockNumRetry(int eventId, List<int> itemId, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_itemId = itemId;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetItemStockNum(this.m_eventId, this.m_itemId, this.m_callbackObject);
		}
	}
}
