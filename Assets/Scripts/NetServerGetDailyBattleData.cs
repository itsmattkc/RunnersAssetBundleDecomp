using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetDailyBattleData : NetBase
{
	private ServerDailyBattleDataPair _battleDataPair_k__BackingField;

	public ServerDailyBattleDataPair battleDataPair
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Battle/getDailyBattleData");
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
		this.battleDataPair = NetUtil.AnalyzeDailyBattleDataPairJson(jdata, "battleData", "rivalBattleData", "startTime", "endTime");
	}
}
