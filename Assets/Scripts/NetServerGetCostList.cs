using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetCostList : NetBase
{
	private List<ServerConsumedCostData> _resultCostList_k__BackingField;

	public List<ServerConsumedCostData> resultCostList
	{
		get;
		set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/getCostList");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_CostList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_CostList(JsonData jdata)
	{
		this.resultCostList = NetUtil.AnalyzeConsumedCostDataList(jdata);
	}
}
