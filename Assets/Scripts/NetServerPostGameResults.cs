using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerPostGameResults : NetBase
{
	public List<ServerMessageEntry> m_messageEntryList = new List<ServerMessageEntry>();

	public List<ServerOperatorMessageEntry> m_operatorMessageEntryList = new List<ServerOperatorMessageEntry>();

	public int m_totalMessage;

	public int m_totalOperatorMessage;

	public List<ServerItemState> m_resultEventIncentiveList;

	public ServerEventState m_resultEventState;

	private ServerGameResults _m_paramGameResults_k__BackingField;

	private ServerPlayerState _m_resultPlayerState_k__BackingField;

	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	private List<ServerChaoState> _resultChaoState_k__BackingField;

	private ServerPlayCharacterState[] _resultPlayCharacterState_k__BackingField;

	private List<ServerMileageIncentive> _m_resultMileageIncentive_k__BackingField;

	private ServerMileageMapState _m_resultMileageMapState_k__BackingField;

	private List<ServerItemState> _m_resultDailyMissionIncentiveList_k__BackingField;

	public ServerGameResults m_paramGameResults
	{
		get;
		set;
	}

	public ServerPlayerState m_resultPlayerState
	{
		get;
		private set;
	}

	public ServerCharacterState[] resultCharacterState
	{
		get;
		private set;
	}

	public List<ServerChaoState> resultChaoState
	{
		get;
		private set;
	}

	public ServerPlayCharacterState[] resultPlayCharacterState
	{
		get;
		private set;
	}

	public List<ServerMileageIncentive> m_resultMileageIncentive
	{
		get;
		private set;
	}

	public ServerMileageMapState m_resultMileageMapState
	{
		get;
		private set;
	}

	public List<ServerItemState> m_resultDailyMissionIncentiveList
	{
		get;
		set;
	}

	public int resultDailyMissionIncentives
	{
		get
		{
			if (this.m_resultDailyMissionIncentiveList != null)
			{
				return this.m_resultDailyMissionIncentiveList.Count;
			}
			return 0;
		}
	}

	public int resultMileageIncentives
	{
		get
		{
			if (this.m_resultMileageIncentive != null)
			{
				return this.m_resultMileageIncentive.Count;
			}
			return 0;
		}
	}

	public NetServerPostGameResults(ServerGameResults gameResults)
	{
		this.m_paramGameResults = gameResults;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/postGameResults");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string postGameResultString = instance.GetPostGameResultString(this.m_paramGameResults);
			UnityEngine.Debug.Log("NetServerPostGameResults.json = " + postGameResultString);
			base.WriteJsonString(postGameResultString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
		this.GetResponse_ChaoState(jdata);
		this.GetResponse_PlayCharacterState(jdata);
		this.GetResponse_MileageMapState(jdata);
		this.GetResponse_DailyMissionIncentives(jdata);
		this.GetResponse_MileageIncentives(jdata);
		this.GetResponse_MessageList(jdata);
		this.GetResponse_Event(jdata);
		this.GetResponse_WheelOptions(jdata);
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
		base.WriteActionParamValue("stageMaxScore", this.m_paramGameResults.m_maxChapterScore);
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

	private void SetParameter_Mileage()
	{
		base.WriteActionParamValue("reachPoint", this.m_paramGameResults.m_reachPoint);
		base.WriteActionParamValue("chapterClear", (!this.m_paramGameResults.m_clearChapter) ? 0 : 1);
		base.WriteActionParamValue("numBossAttack", this.m_paramGameResults.m_numBossAttack);
	}

	private void SetParameter_ChaoEggPresent()
	{
		base.WriteActionParamValue("getChaoEgg", (!this.m_paramGameResults.m_chaoEggPresent) ? 0 : 1);
	}

	private void SetParameter_BossDestroyed()
	{
		base.WriteActionParamValue("bossDestroyed", (!this.m_paramGameResults.m_isBossDestroyed) ? 0 : 1);
	}

	private void SetParameter_Event()
	{
		int? eventId = this.m_paramGameResults.m_eventId;
		if (eventId.HasValue)
		{
			base.WriteActionParamValue("eventId", this.m_paramGameResults.m_eventId);
			long? eventValue = this.m_paramGameResults.m_eventValue;
			if (eventValue.HasValue)
			{
				base.WriteActionParamValue("eventValue", this.m_paramGameResults.m_eventValue);
			}
		}
	}

	public ServerItemState GetResultDailyMissionIncentive(int index)
	{
		if (0 <= index && this.resultDailyMissionIncentives > index)
		{
			return this.m_resultDailyMissionIncentiveList[index];
		}
		return null;
	}

	public ServerMileageIncentive GetResultMileageIncentive(int index)
	{
		if (0 <= index && this.resultMileageIncentives > index)
		{
			return this.m_resultMileageIncentive[index];
		}
		return null;
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.m_resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_WheelOptions(JsonData jdata)
	{
		ServerWheelOptions serverWheelOptions = NetUtil.AnalyzeWheelOptionsJson(jdata, "wheelOptions");
		if (serverWheelOptions.m_numJackpotRing > 0)
		{
			RouletteManager.numJackpotRing = serverWheelOptions.m_numJackpotRing;
			UnityEngine.Debug.Log("!!!! numJackpotRing : " + RouletteManager.numJackpotRing);
		}
	}

	private void GetResponse_CharacterState(JsonData jdata)
	{
		this.resultCharacterState = NetUtil.AnalyzePlayerState_CharactersStates(jdata);
	}

	private void GetResponse_ChaoState(JsonData jdata)
	{
		this.resultChaoState = NetUtil.AnalyzePlayerState_ChaoStates(jdata);
	}

	private void GetResponse_PlayCharacterState(JsonData jdata)
	{
		this.resultPlayCharacterState = NetUtil.AnalyzePlayerState_PlayCharactersStates(jdata);
	}

	private void GetResponse_MileageMapState(JsonData jdata)
	{
		this.m_resultMileageMapState = NetUtil.AnalyzeMileageMapStateJson(jdata, "mileageMapState");
	}

	private void GetResponse_DailyMissionIncentives(JsonData jdata)
	{
		this.m_resultDailyMissionIncentiveList = new List<ServerItemState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "dailyChallengeIncentive");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			ServerItemState item = NetUtil.AnalyzeItemStateJson(jsonArray[i], string.Empty);
			this.m_resultDailyMissionIncentiveList.Add(item);
		}
	}

	private void GetResponse_MileageIncentives(JsonData jdata)
	{
		this.m_resultMileageIncentive = new List<ServerMileageIncentive>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "mileageIncentiveList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			ServerMileageIncentive item = NetUtil.AnalyzeMileageIncentiveJson(jsonArray[i], string.Empty);
			this.m_resultMileageIncentive.Add(item);
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
		this.m_totalMessage = NetUtil.GetJsonInt(jdata, "totalMessage");
		this.m_totalOperatorMessage = NetUtil.GetJsonInt(jdata, "totalOperatorMessage");
	}

	private void GetResponse_Event(JsonData jdata)
	{
		this.m_resultEventIncentiveList = new List<ServerItemState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "eventIncentiveList");
		if (jsonArray != null)
		{
			int count = jsonArray.Count;
			for (int i = 0; i < count; i++)
			{
				ServerItemState item = NetUtil.AnalyzeItemStateJson(jsonArray[i], string.Empty);
				this.m_resultEventIncentiveList.Add(item);
			}
		}
		this.m_resultEventState = NetUtil.AnalyzeEventState(jdata);
	}
}
