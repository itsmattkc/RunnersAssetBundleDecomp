using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerEquipItem : NetBase
{
	private List<ItemType> _items_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public List<ItemType> items
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerEquipItem() : this(null)
	{
	}

	public NetServerEquipItem(List<ItemType> items)
	{
		this.items = items;
	}

	protected override void DoRequest()
	{
		base.SetAction("Item/equipItem");
		this.SetParameter_EquipItem();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_EquipItem()
	{
		if (this.items == null)
		{
			this.items = new List<ItemType>();
		}
		List<object> list = new List<object>();
		foreach (ItemType current in this.items)
		{
			ServerItem serverItem = new ServerItem(current);
			int id = (int)serverItem.id;
			list.Add(id);
		}
		base.WriteActionParamArray("equipItemList", list);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
