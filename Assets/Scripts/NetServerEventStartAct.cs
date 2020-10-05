using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerEventStartAct : NetBase
{
	private int _paramEventId_k__BackingField;

	private int _paramEnergyExpend_k__BackingField;

	private long _paramRaidBossId_k__BackingField;

	private List<ItemType> _paramModifiersItem_k__BackingField;

	private List<BoostItemType> _paramModifiersBoostItem_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private ServerEventUserRaidBossState _userRaidBossState_k__BackingField;

	public int paramEventId
	{
		get;
		set;
	}

	public int paramEnergyExpend
	{
		get;
		set;
	}

	public long paramRaidBossId
	{
		get;
		set;
	}

	public List<ItemType> paramModifiersItem
	{
		get;
		set;
	}

	public List<BoostItemType> paramModifiersBoostItem
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public ServerEventUserRaidBossState userRaidBossState
	{
		get;
		private set;
	}

	public NetServerEventStartAct() : this(-1, -1, -1L, null, null)
	{
	}

	public NetServerEventStartAct(int eventId, int energyExpend, long raidBossId, List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem)
	{
		this.paramEventId = eventId;
		this.paramEnergyExpend = energyExpend;
		this.paramRaidBossId = raidBossId;
		this.paramModifiersItem = new List<ItemType>();
		if (modifiersItem != null)
		{
			for (int i = 0; i < modifiersItem.Count; i++)
			{
				this.paramModifiersItem.Add(modifiersItem[i]);
			}
		}
		this.paramModifiersBoostItem = new List<BoostItemType>();
		if (modifiersBoostItem != null)
		{
			for (int j = 0; j < modifiersBoostItem.Count; j++)
			{
				this.paramModifiersBoostItem.Add(modifiersBoostItem[j]);
			}
		}
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/eventActStart");
		this.SetParameter_EventId();
		this.SetParameter_EnergyExpend();
		this.SetParameter_RaidBossId();
		this.SetParameter_Modifiers();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
		NetUtil.GetResponse_CampaignList(jdata);
		this.userRaidBossState = NetUtil.AnalyzeEventUserRaidBossState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_EventId()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
	}

	private void SetParameter_EnergyExpend()
	{
		base.WriteActionParamValue("energyExpend", this.paramEnergyExpend);
	}

	private void SetParameter_RaidBossId()
	{
		base.WriteActionParamValue("raidbossId", this.paramRaidBossId);
	}

	private void SetParameter_Modifiers()
	{
		List<object> list = new List<object>();
		for (int i = 0; i < this.paramModifiersItem.Count; i++)
		{
			ServerItem serverItem = new ServerItem(this.paramModifiersItem[i]);
			ServerItem.Id id = serverItem.id;
			list.Add((int)id);
		}
		for (int j = 0; j < this.paramModifiersBoostItem.Count; j++)
		{
			ServerItem serverItem2 = new ServerItem(this.paramModifiersBoostItem[j]);
			ServerItem.Id id2 = serverItem2.id;
			list.Add((int)id2);
		}
		base.WriteActionParamArray("modifire", list);
		list.Clear();
	}
}
