using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetPrizeDailyBattle : NetBase
{
	private List<ServerDailyBattlePrizeData> _battleDataPrizeList_k__BackingField;

	public List<ServerDailyBattlePrizeData> battleDataPrizeList
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Battle/getPrizeDailyBattle");
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_BattleData(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_BattleData(JsonData jdata)
	{
		this.battleDataPrizeList = NetUtil.AnalyzeDailyBattlePrizeDataJson(jdata, "battlePrizeDataList");
	}
}
