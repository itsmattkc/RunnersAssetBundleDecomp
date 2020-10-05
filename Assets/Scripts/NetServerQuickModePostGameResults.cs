using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerQuickModePostGameResults : NetBase
{
	public List<ServerItemState> m_dailyMissionIncentiveList = new List<ServerItemState>();

	public List<ServerMessageEntry> m_messageEntryList = new List<ServerMessageEntry>();

	public List<ServerOperatorMessageEntry> m_operatorMessageEntryList = new List<ServerOperatorMessageEntry>();

	private ServerQuickModeGameResults _m_paramGameResults_k__BackingField;

	private ServerPlayerState _m_resultPlayerState_k__BackingField;

	private ServerCharacterState[] _m_resultCharacterState_k__BackingField;

	private List<ServerChaoState> _m_resultChaoState_k__BackingField;

	private ServerPlayCharacterState[] _m_resultPlayCharacterState_k__BackingField;

	public ServerQuickModeGameResults m_paramGameResults
	{
		get;
		set;
	}

	public ServerPlayerState m_resultPlayerState
	{
		get;
		private set;
	}

	public ServerCharacterState[] m_resultCharacterState
	{
		get;
		private set;
	}

	public List<ServerChaoState> m_resultChaoState
	{
		get;
		private set;
	}

	public ServerPlayCharacterState[] m_resultPlayCharacterState
	{
		get;
		private set;
	}

	public int totalMessage
	{
		get
		{
			if (this.m_messageEntryList != null)
			{
				return this.m_messageEntryList.Count;
			}
			return 0;
		}
	}

	public int totalOperatorMessage
	{
		get
		{
			if (this.m_operatorMessageEntryList != null)
			{
				return this.m_operatorMessageEntryList.Count;
			}
			return 0;
		}
	}

	public NetServerQuickModePostGameResults(ServerQuickModeGameResults gameResults)
	{
		this.m_paramGameResults = gameResults;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/quickPostGameResults");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string quickModePostGameResultString = instance.GetQuickModePostGameResultString(this.m_paramGameResults);
			UnityEngine.Debug.Log("NetServerQuickModePostGameResults.json = " + quickModePostGameResultString);
			base.WriteJsonString(quickModePostGameResultString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
		this.GetResponse_ChaoState(jdata);
		this.GetResponse_PlayCharacterState(jdata);
		this.GetResponse_DailyMissionIncentives(jdata);
		this.GetResponse_MessageList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Suspended()
	{
		base.WriteActionParamValue("closed", (!this.m_paramGameResults.m_isSuspended) ? 0 : 1);
	}

	private void SetParameter_Score()
	{
		base.WriteActionParamValue("score", this.m_paramGameResults.m_score);
	}

	private void SetParameter_Rings()
	{
		base.WriteActionParamValue("numRings", this.m_paramGameResults.m_numRings);
		base.WriteActionParamValue("numFailureRings", this.m_paramGameResults.m_numFailureRings);
		base.WriteActionParamValue("numRedStarRings", this.m_paramGameResults.m_numRedStarRings);
	}

	private void SetParameter_Distance()
	{
		base.WriteActionParamValue("distance", this.m_paramGameResults.m_distance);
	}

	private void SetParameter_DailyMission()
	{
		base.WriteActionParamValue("dailyChallengeValue", this.m_paramGameResults.m_dailyMissionValue);
		base.WriteActionParamValue("dailyChallengeComplete", (!this.m_paramGameResults.m_dailyMissionComplete) ? 0 : 1);
	}

	private void SetParameter_NumAnimals()
	{
		base.WriteActionParamValue("numAnimals", this.m_paramGameResults.m_numAnimals);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.m_resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_CharacterState(JsonData jdata)
	{
		this.m_resultCharacterState = NetUtil.AnalyzePlayerState_CharactersStates(jdata);
	}

	private void GetResponse_ChaoState(JsonData jdata)
	{
		this.m_resultChaoState = NetUtil.AnalyzePlayerState_ChaoStates(jdata);
	}

	private void GetResponse_PlayCharacterState(JsonData jdata)
	{
		this.m_resultPlayCharacterState = NetUtil.AnalyzePlayerState_PlayCharactersStates(jdata);
	}

	private void GetResponse_DailyMissionIncentives(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "dailyChallengeIncentive");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			ServerItemState item = NetUtil.AnalyzeItemStateJson(jsonArray[i], string.Empty);
			this.m_dailyMissionIncentiveList.Add(item);
		}
	}

	private void GetResponse_MessageList(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "messageList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerMessageEntry item = NetUtil.AnalyzeMessageEntryJson(jdata2, string.Empty);
			this.m_messageEntryList.Add(item);
		}
		JsonData jsonArray2 = NetUtil.GetJsonArray(jdata, "operatorMessageList");
		int count2 = jsonArray2.Count;
		for (int j = 0; j < count2; j++)
		{
			JsonData jdata3 = jsonArray2[j];
			ServerOperatorMessageEntry item2 = NetUtil.AnalyzeOperatorMessageEntryJson(jdata3, string.Empty);
			this.m_operatorMessageEntryList.Add(item2);
		}
	}
}
