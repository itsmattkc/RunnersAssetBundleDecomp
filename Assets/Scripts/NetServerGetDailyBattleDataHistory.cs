using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetDailyBattleDataHistory : NetBase
{
	private int _historyCount_k__BackingField;

	private List<ServerDailyBattleDataPair> _battleDataPairList_k__BackingField;

	public int historyCount
	{
		private get;
		set;
	}

	public List<ServerDailyBattleDataPair> battleDataPairList
	{
		get;
		private set;
	}

	public NetServerGetDailyBattleDataHistory(int count)
	{
		this.historyCount = count;
	}

	protected override void DoRequest()
	{
		base.SetAction("Battle/getDailyBattleDataHistory");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string dailyBattleDataHistoryString = instance.GetDailyBattleDataHistoryString(this.historyCount);
			base.WriteJsonString(dailyBattleDataHistoryString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_BattleData(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_HistoryCount()
	{
		base.WriteActionParamValue("count", this.historyCount);
	}

	private void GetResponse_BattleData(JsonData jdata)
	{
		this.battleDataPairList = NetUtil.AnalyzeDailyBattleDataPairListJson(jdata, "battleDataList");
	}
}
