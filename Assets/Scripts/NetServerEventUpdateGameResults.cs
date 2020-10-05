using LitJson;
using System;
using System.Collections.Generic;

public class NetServerEventUpdateGameResults : NetBase
{
	private ServerEventGameResults m_paramEventGameResults;

	private ServerPlayerState m_playerState;

	private ServerPlayCharacterState[] m_playCharacterState;

	private ServerWheelOptions m_wheelOptions;

	private List<ServerItemState> m_dailyMissionIncentiveList = new List<ServerItemState>();

	private List<ServerMessageEntry> m_messageEntryList = new List<ServerMessageEntry>();

	private List<ServerOperatorMessageEntry> m_operatorMessageEntryList = new List<ServerOperatorMessageEntry>();

	private int m_totalMessage;

	private int m_totalOperatorMessage;

	private List<ServerItemState> m_eventIncentiveList = new List<ServerItemState>();

	private ServerEventState m_eventState;

	private ServerEventRaidBossBonus m_raidBossBonus;

	public ServerPlayerState PlayerState
	{
		get
		{
			return this.m_playerState;
		}
	}

	public ServerPlayCharacterState[] PlayerCharacterState
	{
		get
		{
			return this.m_playCharacterState;
		}
	}

	public ServerWheelOptions WheelOptions
	{
		get
		{
			return this.m_wheelOptions;
		}
	}

	public List<ServerItemState> DailyMissionIncentiveList
	{
		get
		{
			return this.m_dailyMissionIncentiveList;
		}
	}

	public List<ServerMessageEntry> MessageEntryList
	{
		get
		{
			return this.m_messageEntryList;
		}
	}

	public int TotalMessage
	{
		get
		{
			return this.m_totalMessage;
		}
	}

	public List<ServerOperatorMessageEntry> OperatorMessageEntryList
	{
		get
		{
			return this.m_operatorMessageEntryList;
		}
	}

	public int TotalOperatorMessage
	{
		get
		{
			return this.m_totalOperatorMessage;
		}
	}

	public List<ServerItemState> EventIncentiveList
	{
		get
		{
			return this.m_eventIncentiveList;
		}
	}

	public ServerEventState EventState
	{
		get
		{
			return this.m_eventState;
		}
	}

	public ServerEventRaidBossBonus RaidBossBonus
	{
		get
		{
			return this.m_raidBossBonus;
		}
	}

	public NetServerEventUpdateGameResults(ServerEventGameResults eventGameResults)
	{
		this.m_paramEventGameResults = eventGameResults;
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/eventUpdateGameResults");
		this.SetParameter_Rings();
		this.SetParameter_Suspended();
		this.SetParameter_DailyMission();
		this.SetParameter_EventRaidBoss();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_PlayCharacterState(jdata);
		this.GetResponse_WheelOptions(jdata);
		this.GetResponse_DailyMissionIncentives(jdata);
		this.GetResponse_MessageList(jdata);
		this.GetResponse_EventRaidBoss(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Suspended()
	{
		base.WriteActionParamValue("closed", (!this.m_paramEventGameResults.m_isSuspended) ? 0 : 1);
	}

	private void SetParameter_Rings()
	{
		base.WriteActionParamValue("numRings", this.m_paramEventGameResults.m_numRings);
		base.WriteActionParamValue("numRedStarRings", this.m_paramEventGameResults.m_numRedStarRings);
		base.WriteActionParamValue("numFailureRings", this.m_paramEventGameResults.m_numFailureRings);
	}

	private void SetParameter_DailyMission()
	{
		base.WriteActionParamValue("dailyChallengeValue", this.m_paramEventGameResults.m_dailyMissionValue);
		base.WriteActionParamValue("dailyChallengeComplete", (!this.m_paramEventGameResults.m_dailyMissionComplete) ? 0 : 1);
	}

	private void SetParameter_EventRaidBoss()
	{
		base.WriteActionParamValue("eventId", this.m_paramEventGameResults.m_eventId);
		base.WriteActionParamValue("eventValue", this.m_paramEventGameResults.m_eventValue);
		base.WriteActionParamValue("raidbossId", this.m_paramEventGameResults.m_raidBossId);
		base.WriteActionParamValue("raidbossDamage", this.m_paramEventGameResults.m_raidBossDamage);
		base.WriteActionParamValue("raidbossBeatFlg", (!this.m_paramEventGameResults.m_isRaidBossBeat) ? 0 : 1);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.m_playerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_PlayCharacterState(JsonData jdata)
	{
		this.m_playCharacterState = NetUtil.AnalyzePlayerState_PlayCharactersStates(jdata);
	}

	private void GetResponse_WheelOptions(JsonData jdata)
	{
		this.m_wheelOptions = NetUtil.AnalyzeWheelOptionsJson(jdata, "wheelOptions");
		if (this.m_wheelOptions.m_numJackpotRing > 0)
		{
			RouletteManager.numJackpotRing = this.m_wheelOptions.m_numJackpotRing;
			UnityEngine.Debug.Log("numJackpotRing : " + RouletteManager.numJackpotRing);
		}
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
		this.m_totalMessage = NetUtil.GetJsonInt(jdata, "totalMessage");
		this.m_totalOperatorMessage = NetUtil.GetJsonInt(jdata, "totalOperatorMessage");
	}

	private void GetResponse_EventRaidBoss(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "eventIncentiveList");
		if (jsonArray != null)
		{
			int count = jsonArray.Count;
			for (int i = 0; i < count; i++)
			{
				ServerItemState item = NetUtil.AnalyzeItemStateJson(jsonArray[i], string.Empty);
				this.m_eventIncentiveList.Add(item);
			}
		}
		this.m_eventState = NetUtil.AnalyzeEventState(jdata);
		this.m_raidBossBonus = NetUtil.AnalyzeEventRaidBossBonus(jdata);
	}
}
