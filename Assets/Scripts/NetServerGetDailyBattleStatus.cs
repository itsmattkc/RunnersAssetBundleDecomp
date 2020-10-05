using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetDailyBattleStatus : NetBase
{
	private ServerDailyBattleStatus _battleStatus_k__BackingField;

	private DateTime _endTime_k__BackingField;

	public ServerDailyBattleStatus battleStatus
	{
		get;
		private set;
	}

	public DateTime endTime
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Battle/getDailyBattleStatus");
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
		this.endTime = NetUtil.AnalyzeDateTimeJson(jdata, "endTime");
		this.battleStatus = NetUtil.AnalyzeDailyBattleStatusJson(jdata, "battleStatus");
	}
}
