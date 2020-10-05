using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerChaoSpinResult
{
	private ServerChaoData _AcquiredChaoData_k__BackingField;

	private bool _IsRequiredChao_k__BackingField;

	private int _NumRequiredSpEggs_k__BackingField;

	private Dictionary<int, ServerItemState> _ItemState_k__BackingField;

	private int _ItemWon_k__BackingField;

	private bool _IsGotAlreadyChaoLevelMax_k__BackingField;

	public ServerChaoData AcquiredChaoData
	{
		get;
		set;
	}

	public bool IsRequiredChao
	{
		get;
		set;
	}

	public int NumRequiredSpEggs
	{
		get;
		set;
	}

	public bool IsRequiredSpEgg
	{
		get
		{
			return 0 < this.NumRequiredSpEggs;
		}
	}

	public Dictionary<int, ServerItemState> ItemState
	{
		get;
		private set;
	}

	public int ItemWon
	{
		get;
		set;
	}

	public bool IsGotAlreadyChaoLevelMax
	{
		get;
		set;
	}

	public ServerChaoSpinResult()
	{
		this.AcquiredChaoData = null;
		this.IsRequiredChao = true;
		this.NumRequiredSpEggs = 0;
		this.ItemState = new Dictionary<int, ServerItemState>();
		this.ItemWon = 0;
		this.IsGotAlreadyChaoLevelMax = false;
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

	public void CopyTo(ServerChaoSpinResult to)
	{
		to.AcquiredChaoData = this.AcquiredChaoData;
		to.IsRequiredChao = this.IsRequiredChao;
		to.NumRequiredSpEggs = this.NumRequiredSpEggs;
		to.ItemState.Clear();
		foreach (ServerItemState current in this.ItemState.Values)
		{
			to.ItemState.Add(current.m_itemId, current);
		}
		to.ItemWon = this.ItemWon;
		to.IsGotAlreadyChaoLevelMax = this.IsGotAlreadyChaoLevelMax;
	}

	public void Dump()
	{
	}
}
