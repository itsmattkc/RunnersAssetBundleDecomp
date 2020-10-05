using LitJson;
using System;
using System.Collections.Generic;

public class NetServerGetTicker : NetBase
{
	private List<ServerTickerData> m_tickerData;

	public NetServerGetTicker()
	{
		this.m_tickerData = new List<ServerTickerData>();
	}

	protected override void DoRequest()
	{
		base.SetAction("login/getTicker");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_tickerData.Clear();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "tickerList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			long jsonLong = NetUtil.GetJsonLong(jdata2, "id");
			long jsonLong2 = NetUtil.GetJsonLong(jdata2, "start");
			long jsonLong3 = NetUtil.GetJsonLong(jdata2, "end");
			string jsonString = NetUtil.GetJsonString(jdata2, "param");
			this.AddInfo(jsonLong, jsonLong2, jsonLong3, jsonString);
		}
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	public ServerTickerData GetInfo(int index)
	{
		if (index < this.m_tickerData.Count)
		{
			return this.m_tickerData[index];
		}
		return null;
	}

	public int GetInfoCount()
	{
		return this.m_tickerData.Count;
	}

	private void AddInfo(long id, long start, long end, string param)
	{
		ServerTickerData serverTickerData = new ServerTickerData();
		serverTickerData.Init(id, start, end, param);
		this.m_tickerData.Add(serverTickerData);
	}
}
