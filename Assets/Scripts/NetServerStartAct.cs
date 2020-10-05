using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerStartAct : NetBase
{
	private List<ItemType> _paramModifiersItem_k__BackingField;

	private List<BoostItemType> _paramModifiersBoostItem_k__BackingField;

	private List<string> _paramFriendIdList_k__BackingField;

	private bool _paramTutorial_k__BackingField;

	private int? _paramEventId_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private List<ServerDistanceFriendEntry> _resultDistanceFriendEntry_k__BackingField;

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

	public List<string> paramFriendIdList
	{
		get;
		set;
	}

	public bool paramTutorial
	{
		get;
		set;
	}

	public int? paramEventId
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public List<ServerDistanceFriendEntry> resultDistanceFriendEntry
	{
		get;
		private set;
	}

	public NetServerStartAct() : this(null, null, null, false, null)
	{
	}

	public NetServerStartAct(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, List<string> distanceFriendList, bool tutorial, int? eventId)
	{
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
		this.paramFriendIdList = distanceFriendList;
		this.paramTutorial = tutorial;
		this.paramEventId = eventId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/actStart");
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
			int eventId = (!this.paramEventId.HasValue) ? (-1) : this.paramEventId.Value;
			string actStartString = instance.GetActStartString(list, this.paramFriendIdList, this.paramTutorial, eventId);
			UnityEngine.Debug.Log("NetServerPostGameResults.json = " + actStartString);
			base.WriteJsonString(actStartString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_MileageBonus(jdata);
		NetUtil.GetResponse_CampaignList(jdata);
		this.GetResponse_DistanceFriendList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		if (this.resultPlayerState.m_numEnergy < 1)
		{
			base.resultStCd = ServerInterface.StatusCode.NotEnoughEnergy;
			return;
		}
		this.resultPlayerState.m_numEnergy--;
		this.resultPlayerState.m_numContinuesUsed = 0;
		DateTime now = DateTime.Now;
		if (this.resultPlayerState.m_energyRenewsAt <= now)
		{
			this.resultPlayerState.m_energyRenewsAt = now + new TimeSpan(0, 0, (int)ServerInterface.SettingState.m_energyRefreshTime);
		}
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

	private void SetParameter_FriendIdList()
	{
		List<object> list = new List<object>();
		for (int i = 0; i < this.paramFriendIdList.Count; i++)
		{
			list.Add(this.paramFriendIdList[i]);
		}
		base.WriteActionParamArray("distanceFriendList", list);
		list.Clear();
	}

	private void SetParameter_Tutorial()
	{
		base.WriteActionParamValue("tutorial", (!this.paramTutorial) ? 0 : 1);
	}

	private void SetParameter_EventId()
	{
		if (this.paramEventId.HasValue)
		{
			base.WriteActionParamValue("eventId", this.paramEventId);
		}
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_MileageBonus(JsonData jdata)
	{
	}

	private void GetResponse_DistanceFriendList(JsonData jdata)
	{
		this.resultDistanceFriendEntry = new List<ServerDistanceFriendEntry>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "distanceFriendList");
		if (jsonArray != null)
		{
			int count = jsonArray.Count;
			for (int i = 0; i < count; i++)
			{
				ServerDistanceFriendEntry serverDistanceFriendEntry = new ServerDistanceFriendEntry();
				serverDistanceFriendEntry.m_friendId = NetUtil.GetJsonString(jsonArray[i], "friendId");
				serverDistanceFriendEntry.m_distance = NetUtil.GetJsonInt(jsonArray[i], "distance");
				this.resultDistanceFriendEntry.Add(serverDistanceFriendEntry);
			}
		}
	}
}
