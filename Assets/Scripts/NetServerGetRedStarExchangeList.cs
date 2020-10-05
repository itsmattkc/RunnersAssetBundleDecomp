using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetRedStarExchangeList : NetBase
{
	private int mParamItemType;

	public string resultBirthDay;

	public int resultMonthPurchase;

	private int _resultTotalItems_k__BackingField;

	private List<ServerRedStarItemState> _resultRedStarItemStateList_k__BackingField;

	public int paramItemType
	{
		get
		{
			return this.mParamItemType;
		}
		set
		{
			this.mParamItemType = value;
		}
	}

	public int resultTotalItems
	{
		get;
		private set;
	}

	public int resultItems
	{
		get
		{
			if (this.resultRedStarItemStateList != null)
			{
				return this.resultRedStarItemStateList.Count;
			}
			return 0;
		}
	}

	private List<ServerRedStarItemState> resultRedStarItemStateList
	{
		get;
		set;
	}

	public NetServerGetRedStarExchangeList() : this(0)
	{
	}

	public NetServerGetRedStarExchangeList(int itemType)
	{
		this.paramItemType = itemType;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/getRedstarExchangeList");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getRedStarExchangeListString = instance.GetGetRedStarExchangeListString(this.paramItemType);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getRedStarExchangeListString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_RedStarItemStateList(jdata);
		this.GetResponse_TotalItems(jdata);
		this.GetResponse_BirthDay(jdata);
		this.GetResponse_MonthPurchase(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_ItemType()
	{
		base.WriteActionParamValue("itemType", this.paramItemType);
	}

	public ServerRedStarItemState GetResultRedStarItemState(int index)
	{
		if (0 <= index && this.resultItems > index)
		{
			return this.resultRedStarItemStateList[index];
		}
		return null;
	}

	private void GetResponse_RedStarItemStateList(JsonData jdata)
	{
		this.resultRedStarItemStateList = new List<ServerRedStarItemState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "itemList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			ServerRedStarItemState item = NetUtil.AnalyzeRedStarItemStateJson(jsonArray[i], string.Empty);
			this.resultRedStarItemStateList.Add(item);
		}
	}

	private void GetResponse_TotalItems(JsonData jdata)
	{
		this.resultTotalItems = NetUtil.GetJsonInt(jdata, "totalItems");
	}

	private void GetResponse_BirthDay(JsonData jdata)
	{
		this.resultBirthDay = NetUtil.GetJsonString(jdata, "birthday");
	}

	private void GetResponse_MonthPurchase(JsonData jdata)
	{
		this.resultMonthPurchase = NetUtil.GetJsonInt(jdata, "monthPurchase");
	}
}
