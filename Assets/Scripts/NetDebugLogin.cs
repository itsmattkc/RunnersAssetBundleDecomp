using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetDebugLogin : NetBase
{
	private string _paramLineId_k__BackingField;

	private string _paramAltLineId_k__BackingField;

	private string _paramLineAuth_k__BackingField;

	private string _resultSessionId_k__BackingField;

	private long _resultEnergyRefreshTime_k__BackingField;

	private List<ServerRingItemState> _resultRingItemStateList_k__BackingField;

	public string paramLineId
	{
		get;
		set;
	}

	public string paramAltLineId
	{
		get;
		set;
	}

	public string paramLineAuth
	{
		get;
		set;
	}

	public string resultSessionId
	{
		get;
		private set;
	}

	public long resultEnergyRefreshTime
	{
		get;
		private set;
	}

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

	public NetDebugLogin() : this(string.Empty, string.Empty, string.Empty)
	{
	}

	public NetDebugLogin(string lineId, string altLineId, string lineAuth)
	{
		this.paramLineId = lineId;
		this.paramAltLineId = altLineId;
		this.paramLineAuth = lineAuth;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/login");
		this.SetParameter_LineAuth();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_SessionId(jdata);
		this.GetResponse_EnergyRefreshTime(jdata);
		this.GetResponse_RingItemStateList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_LineAuth()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(2);
		dictionary.Add("lineId", this.paramLineId);
		dictionary.Add("altLineId", this.paramAltLineId);
		dictionary.Add("lineAuthToken", this.paramLineAuth);
		base.WriteActionParamObject("lineAuth", dictionary);
		dictionary.Clear();
	}

	public ServerRingItemState GetResultRingItemState(int index)
	{
		if (0 <= index && this.resultRingItemStates > index)
		{
			return this.resultRingItemStateList[index];
		}
		return null;
	}

	private void GetResponse_SessionId(JsonData jdata)
	{
		this.resultSessionId = NetUtil.GetJsonString(jdata, "sessionId");
	}

	private void GetResponse_EnergyRefreshTime(JsonData jdata)
	{
		this.resultEnergyRefreshTime = NetUtil.GetJsonLong(jdata, "energyRecoveryTime");
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
