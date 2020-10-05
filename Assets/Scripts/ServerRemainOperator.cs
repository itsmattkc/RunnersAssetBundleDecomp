using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerRemainOperator
{
	private int _operatorData_k__BackingField;

	private int _number_k__BackingField;

	private Dictionary<int, ServerItemState> _ItemState_k__BackingField;

	public int operatorData
	{
		get;
		set;
	}

	public int number
	{
		get;
		set;
	}

	public Dictionary<int, ServerItemState> ItemState
	{
		get;
		private set;
	}

	public ServerRemainOperator()
	{
		this.operatorData = 0;
		this.number = 0;
		this.ItemState = new Dictionary<int, ServerItemState>();
	}

	public void CopyTo(ServerRemainOperator to)
	{
		to.operatorData = this.operatorData;
		to.number = this.number;
		to.ItemState.Clear();
		foreach (ServerItemState current in this.ItemState.Values)
		{
			to.ItemState.Add(current.m_itemId, current);
		}
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

	public void Dump()
	{
	}
}
