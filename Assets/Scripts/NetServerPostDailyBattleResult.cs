using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerPostDailyBattleResult : NetBase
{
	private ServerDailyBattleStatus _battleStatus_k__BackingField;

	private ServerDailyBattleDataPair _battleDataPair_k__BackingField;

	private bool _rewardFlag_k__BackingField;

	private ServerDailyBattleDataPair _rewardBattleDataPair_k__BackingField;

	public ServerDailyBattleStatus battleStatus
	{
		get;
		private set;
	}

	public ServerDailyBattleDataPair battleDataPair
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
		base.SetAction("Battle/postDailyBattleResult");
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
		this.battleStatus = NetUtil.AnalyzeDailyBattleStatusJson(jdata, "battleStatus");
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
