using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerResetDailyBattleMatching : NetBase
{
	private int _matchingType_k__BackingField;

	private ServerPlayerState _playerState_k__BackingField;

	private ServerDailyBattleDataPair _battleDataPair_k__BackingField;

	private DateTime _endTime_k__BackingField;

	public int matchingType
	{
		private get;
		set;
	}

	public ServerPlayerState playerState
	{
		get;
		private set;
	}

	public ServerDailyBattleDataPair battleDataPair
	{
		get;
		private set;
	}

	public DateTime endTime
	{
		get;
		private set;
	}

	public NetServerResetDailyBattleMatching(int type)
	{
		this.matchingType = type;
	}

	protected override void DoRequest()
	{
		base.SetAction("Battle/resetDailyBattleMatching");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string jsonString = instance.ResetDailyBattleMatchingString(this.matchingType);
			base.WriteJsonString(jsonString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_BattleData(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_MatchingType()
	{
		base.WriteActionParamValue("type", this.matchingType);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.playerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_BattleData(JsonData jdata)
	{
		this.endTime = NetUtil.AnalyzeDateTimeJson(jdata, "endTime");
		this.battleDataPair = NetUtil.AnalyzeDailyBattleDataPairJson(jdata, "battleData", "rivalBattleData", "startTime", "endTime");
	}
}
