using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerEventStartActRetry : ServerRetryProcess
{
	public int m_eventId;

	public int m_energyExpend;

	public long m_raidBossId;

	public List<ItemType> m_modifiersItem;

	public List<BoostItemType> m_modifiersBoostItem;

	public ServerEventStartActRetry(int eventId, int energyExpend, long raidBossId, List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, GameObject callbackObject) : base(callbackObject)
	{
		this.m_eventId = eventId;
		this.m_energyExpend = energyExpend;
		this.m_raidBossId = raidBossId;
		this.m_modifiersItem = modifiersItem;
		this.m_modifiersBoostItem = modifiersBoostItem;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerEventStartAct(this.m_eventId, this.m_energyExpend, this.m_raidBossId, this.m_modifiersItem, this.m_modifiersBoostItem, this.m_callbackObject);
		}
	}
}
