using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NetServerGetRentalState : NetBase
{
	private string[] _friendIdList_k__BackingField;

	private int _resultLastOffset_k__BackingField;

	private List<ServerChaoRentalState> _resultChaoRentalStatesList_k__BackingField;

	public string[] friendIdList
	{
		get;
		set;
	}

	public int resultLastOffset
	{
		get;
		private set;
	}

	public int resultStates
	{
		get
		{
			return (this.resultChaoRentalStatesList == null) ? 0 : this.resultChaoRentalStatesList.Count;
		}
	}

	protected List<ServerChaoRentalState> resultChaoRentalStatesList
	{
		get;
		set;
	}

	public NetServerGetRentalState() : this(null)
	{
	}

	public NetServerGetRentalState(string[] friendIdList)
	{
		this.friendIdList = friendIdList;
	}

	protected override void DoRequest()
	{
		base.SetAction("Chao/getRentalState");
		this.SetParameter_FriendId();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_LastOffset(jdata);
		this.GetResponse_ChaoRentalStatesList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultChaoRentalStatesList = new List<ServerChaoRentalState>();
		int num = this.friendIdList.Length;
		for (int i = 0; i < num; i++)
		{
			ServerChaoRentalState serverChaoRentalState = new ServerChaoRentalState();
			serverChaoRentalState.FriendId = UnityEngine.Random.Range(0f, 1E+11f).ToString();
			serverChaoRentalState.Name = "dummy_" + i;
			serverChaoRentalState.RentalState = UnityEngine.Random.Range(0, 1);
			serverChaoRentalState.ChaoData = new ServerChaoData();
			serverChaoRentalState.ChaoData.Id = UnityEngine.Random.Range(400000, 400011);
			serverChaoRentalState.ChaoData.Level = 1;
			serverChaoRentalState.ChaoData.Rarity = 0;
			this.resultChaoRentalStatesList.Add(serverChaoRentalState);
		}
	}

	private void SetParameter_FriendId()
	{
		List<object> list = new List<object>();
		string[] friendIdList = this.friendIdList;
		for (int i = 0; i < friendIdList.Length; i++)
		{
			object item = friendIdList[i];
			list.Add(item);
		}
		base.WriteActionParamArray("friendId", list);
	}

	public ServerChaoRentalState GetResultChaoRentalState(int index)
	{
		if (0 <= index && this.resultStates > index)
		{
			return this.resultChaoRentalStatesList[index];
		}
		return null;
	}

	private void GetResponse_LastOffset(JsonData jdata)
	{
		this.resultLastOffset = NetUtil.GetJsonInt(jdata, "lastOffset");
	}

	private void GetResponse_ChaoRentalStatesList(JsonData jdata)
	{
		this.resultChaoRentalStatesList = new List<ServerChaoRentalState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "chaoRentalState");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerChaoRentalState item = NetUtil.AnalyzeChaoRentalStateJson(jdata2, string.Empty);
			this.resultChaoRentalStatesList.Add(item);
		}
	}
}
