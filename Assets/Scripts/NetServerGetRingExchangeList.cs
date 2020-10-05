using LitJson;
using System;
using System.Collections.Generic;

public class NetServerGetRingExchangeList : NetBase
{
	public List<ServerRingExchangeList> m_ringExchangeList;

	public int m_totalItems;

	protected override void DoRequest()
	{
		base.SetAction("Store/getRingExchangeList");
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_RingExchangeList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_RingExchangeList(JsonData jdata)
	{
		this.m_ringExchangeList = NetUtil.AnalyzeRingExchangeList(jdata);
		this.m_totalItems = NetUtil.AnalyzeRingExchangeListTotalItems(jdata);
	}
}
