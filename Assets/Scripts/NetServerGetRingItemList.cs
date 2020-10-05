using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetRingItemList : NetBase
{
	private List<ServerRingItemState> _resultRingItemStateList_k__BackingField;

	public int resultRingItemStates
	{
		get
		{
			if (this.resultRingItemStateList != null)
			{
				return this.resultRingItemStateList.Count;
			}
			return 0;
		}
	}

	private List<ServerRingItemState> resultRingItemStateList
	{
		get;
		set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/getRingItemList");
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_RingItemStateList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	public ServerRingItemState GetResultRingItemState(int index)
	{
		if (0 <= index && this.resultRingItemStates > index)
		{
			return this.resultRingItemStateList[index];
		}
		return null;
	}

	private void GetResponse_RingItemStateList(JsonData jdata)
	{
		this.resultRingItemStateList = new List<ServerRingItemState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "ringItemList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerRingItemState item = NetUtil.AnalyzeRingItemStateJson(jdata2, string.Empty);
			this.resultRingItemStateList.Add(item);
		}
	}
}
