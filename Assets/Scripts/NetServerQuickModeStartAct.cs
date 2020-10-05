using LitJson;
using System;
using System.Collections.Generic;

public class NetServerQuickModeStartAct : NetBase
{
	private ServerPlayerState m_resultPlayerState;

	private List<ItemType> m_paramModifiersItem = new List<ItemType>();

	private List<BoostItemType> m_paramModifiersBoostItem = new List<BoostItemType>();

	private int m_tutorial;

	public ServerPlayerState resultPlayerState
	{
		get
		{
			return this.m_resultPlayerState;
		}
	}

	public List<ItemType> paramModifiersItem
	{
		get
		{
			return this.m_paramModifiersItem;
		}
	}

	public List<BoostItemType> paramModifiersBoostItem
	{
		get
		{
			return this.m_paramModifiersBoostItem;
		}
	}

	public NetServerQuickModeStartAct(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, bool tutorial)
	{
		if (modifiersItem != null)
		{
			for (int i = 0; i < modifiersItem.Count; i++)
			{
				this.m_paramModifiersItem.Add(modifiersItem[i]);
			}
		}
		if (modifiersBoostItem != null)
		{
			for (int j = 0; j < modifiersBoostItem.Count; j++)
			{
				this.m_paramModifiersBoostItem.Add(modifiersBoostItem[j]);
			}
		}
		if (tutorial)
		{
			this.m_tutorial = 1;
		}
		else
		{
			this.m_tutorial = 0;
		}
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/quickActStart");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			List<int> list = new List<int>();
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
			string quickModeActStartString = instance.GetQuickModeActStartString(list, this.m_tutorial);
			UnityEngine.Debug.Log("NetServerQuickModeStartAct.json = " + quickModeActStartString);
			base.WriteJsonString(quickModeActStartString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
		NetUtil.GetResponse_CampaignList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
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

	private void SetParameter_Tutorial()
	{
		base.WriteActionParamValue("tutorial", this.m_tutorial);
	}
}
