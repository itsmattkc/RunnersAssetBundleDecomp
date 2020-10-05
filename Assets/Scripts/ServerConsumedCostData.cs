using System;
using System.Runtime.CompilerServices;

public class ServerConsumedCostData
{
	private int _consumedItemId_k__BackingField;

	private int _itemId_k__BackingField;

	private int _numItem_k__BackingField;

	public int consumedItemId
	{
		get;
		set;
	}

	public int itemId
	{
		get;
		set;
	}

	public int numItem
	{
		get;
		set;
	}

	public ServerConsumedCostData()
	{
		this.consumedItemId = 0;
		this.itemId = 0;
		this.numItem = 0;
	}

	public void CopyTo(ServerConsumedCostData dest)
	{
		dest.consumedItemId = this.consumedItemId;
		dest.itemId = this.itemId;
		dest.numItem = this.numItem;
	}
}
