using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerUpdateDailyBattleStatus : NetBase
{
	private ServerDailyBattleStatus _battleDataStatus_k__BackingField;

	private DateTime _endTime_k__BackingField;

	private bool _rewardFlag_k__BackingField;

	private ServerDailyBattleDataPair _rewardBattleDataPair_k__BackingField;

	public ServerDailyBattleStatus battleDataStatus
	{
		get;
		private set;
	}

	public DateTime endTime
	{
		get;
		private set;
	}

	public bool rewardFlag
	{
		get;
		private set;
	}

	public ServerDailyBattleDataPair rewardBattleDataPair
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Battle/updateDailyBattleStatus");
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
		this.battleDataStatus = NetUtil.AnalyzeDailyBattleStatusJson(jdata, "battleStatus");
		bool jsonBoolean = NetUtil.GetJsonBoolean(jdata, "rewardFlag");
		if (jsonBoolean)
		{
			this.rewardFlag = true;
			this.rewardBattleDataPair = NetUtil.AnalyzeDailyBattleDataPairJson(jdata, "rewardBattleData", "rewardRivalBattleData", "rewardStartTime", "rewardEndTime");
		}
		else
		{
			this.rewardFlag = false;
			this.rewardBattleDataPair = null;
		}
	}
}
