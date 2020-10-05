using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerDailyBattlePrizeData
{
	public int operatorData;

	public int number;

	private Dictionary<int, ServerItemState> _ItemState_k__BackingField;

	public Dictionary<int, ServerItemState> ItemState
	{
		get;
		private set;
	}

	public ServerDailyBattlePrizeData()
	{
		this.operatorData = 0;
		this.number = 0;
		this.ItemState = new Dictionary<int, ServerItemState>();
	}

	public void Dump()
	{
	}

	public void AddItemState(ServerItemState itemState)
	{
		if (this.ItemState.ContainsKey(itemState.m_itemId))
		{
			this.ItemState[itemState.m_itemId].m_num += itemState.m_num;
		}
		else
		{
			this.ItemState.Add(itemState.m_itemId, itemState);
		}
	}

	public void CopyTo(ServerDailyBattlePrizeData dest)
	{
		dest.operatorData = this.operatorData;
		dest.number = this.number;
		dest.ItemState.Clear();
		foreach (ServerItemState current in this.ItemState.Values)
		{
			dest.ItemState.Add(current.m_itemId, current);
		}
	}

	public void CopyTo(ServerRemainOperator to)
	{
		to.operatorData = this.operatorData;
		to.number = this.number;
		to.ItemState.Clear();
		to.ItemState.Clear();
		foreach (ServerItemState current in this.ItemState.Values)
		{
			to.ItemState.Add(current.m_itemId, current);
		}
	}

	public static List<ServerRemainOperator> ConvertRemainOperatorList(List<ServerDailyBattlePrizeData> prizeList)
	{
		if (prizeList == null || prizeList.Count <= 0)
		{
			return null;
		}
		List<ServerRemainOperator> list = new List<ServerRemainOperator>();
		foreach (ServerDailyBattlePrizeData current in prizeList)
		{
			ServerRemainOperator serverRemainOperator = new ServerRemainOperator();
			current.CopyTo(serverRemainOperator);
			list.Add(serverRemainOperator);
		}
		return list;
	}
}
