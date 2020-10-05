using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetItemStockNum : NetBase
{
	public int paramEventId;

	public List<int> paramItemId;

	private List<ServerItemState> _m_itemStockList_k__BackingField;

	public List<ServerItemState> m_itemStockList
	{
		get;
		set;
	}

	public NetServerGetItemStockNum(int eventId, List<int> itemId)
	{
		this.paramEventId = eventId;
		this.paramItemId = itemId;
	}

	protected override void DoRequest()
	{
		base.SetAction("RaidbossSpin/getItemStockNum");
		int eventId = this.paramEventId;
		int[] itemIdList = this.paramItemId.ToArray();
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getItemStockNumString = instance.GetGetItemStockNumString(eventId, itemIdList);
			UnityEngine.Debug.Log("NetServerGetItemStockNum.json = " + getItemStockNumString);
			base.WriteJsonString(getItemStockNumString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_itemStock(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
		List<object> list = new List<object>();
		foreach (object item in this.paramItemId)
		{
			list.Add(item);
		}
		base.WriteActionParamArray("itemIdList", list);
	}

	private void GetResponse_itemStock(JsonData jdata)
	{
		this.m_itemStockList = new List<ServerItemState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "itemStockList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			ServerItemState item = NetUtil.AnalyzeItemStateJson(jsonArray[i], string.Empty);
			this.m_itemStockList.Add(item);
		}
	}
}
