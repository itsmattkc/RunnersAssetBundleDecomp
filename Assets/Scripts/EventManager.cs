using SaveData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public enum EventType
	{
		SPECIAL_STAGE,
		RAID_BOSS,
		COLLECT_OBJECT,
		GACHA,
		ADVERT,
		QUICK,
		BGM,
		NUM,
		UNKNOWN = -1
	}

	public enum CollectEventType
	{
		GET_ANIMALS,
		GET_RING,
		RUN_DISTANCE,
		NUM,
		UNKNOWN = -1
	}

	public enum AdvertEventType
	{
		ROULETTE,
		CHARACTER,
		SHOP,
		NUM,
		UNKNOWN = -1
	}

	[Serializable]
	public class DebugRaidBoss
	{
		[SerializeField]
		public int m_id;

		[SerializeField]
		public int m_level;

		[SerializeField]
		public int m_rarity;

		[SerializeField]
		public int m_hp = 1;

		[SerializeField]
		public int m_hpMax = 1;

		[SerializeField]
		public string m_discovererName;

		[SerializeField]
		public int m_endTimeMinutes = 60;

		[SerializeField]
		public int m_state;

		[SerializeField]
		public bool m_findMyself = true;

		[SerializeField]
		public bool m_validFlag;
	}

	[Serializable]
	public class DebugRaidBossInfo
	{
		[Header("レイドボスの基本データ"), SerializeField]
		public int m_raidBossRingNum;

		[SerializeField]
		public int m_raidBossKillNum;

		[SerializeField]
		public int m_raidBossChallengeNum;

		[Header("レイドボスに挑むときのチャレンジ使用数"), SerializeField]
		public int m_raidBossUseChallengeNum = 1;

		[Header("レイドボス情報リスト(データが足りないようなら、クラスの変数を足してください)"), SerializeField]
		public EventManager.DebugRaidBoss[] m_debugRaidBossDatas = new EventManager.DebugRaidBoss[6];

		[Header("レイドボス情報リストの指定番号を使用して、レイドボス戦(レイドボスステージ用)"), SerializeField]
		public int m_debugCurrentRaidBossDataIndex;

		[Header("レイドボスの襲来フラグ(通常ステージ用)"), SerializeField]
		public bool m_debugRaidBossDescentFlag;
	}

	private const int EVENT_TYPE_COFFI = 100000000;

	private const int SPECIFIC_TYPE_COFFI = 10000;

	private const int NUMBER_OF_TIMES_COFFI = 100;

	private const int DEBUG_RAID_BOSS_COUNT = 6;

	private static readonly string[] EventTypeName = new string[]
	{
		"SpecialStage",
		"RaidBoss",
		"CollectObject",
		"Gacha",
		"Advert",
		"Quick",
		"BGM"
	};

	private static readonly int[] COLLECT_EVENT_SPECIFIC_ID = new int[]
	{
		1,
		2,
		3
	};

	private static EventManager instance = null;

	[Header("debugFlag にチェックを入れると、指定した時間で始められます"), SerializeField]
	private bool m_debugFlag;

	[Header("eventId の詳細はwiki[イベントIDルール]を見てください。"), SerializeField]
	private int m_eventId = -1;

	[Header("イベントスタートまでの時間を設定"), SerializeField]
	private int m_startTimeHours;

	[SerializeField]
	private int m_startTimeMinutes;

	[SerializeField]
	private int m_startTimeSeconds;

	[Header("イベントの残り時間を設定"), SerializeField]
	private int m_endTimeHours;

	[SerializeField]
	private int m_endTimeMinutes;

	[SerializeField]
	private int m_endTimeSeconds;

	[Header("イベントのクローズまでの時間を設定"), SerializeField]
	private int m_closeTimeHours;

	[SerializeField]
	private int m_closeTimeMinutes;

	[SerializeField]
	private int m_closeTimeSeconds;

	[Header("イベントのステージプレイ猶予時間を設定"), SerializeField]
	private int m_endPlayingTimeMinutes = 25;

	[Header("イベントのステージリザルト猶予時間を設定"), SerializeField]
	private int m_endResultTimeMinutes = 30;

	[Header("Debug レイドボスインフォ(データが足りないようなら、変数を足してください)"), SerializeField]
	private EventManager.DebugRaidBossInfo m_debugRaidBossInfo = new EventManager.DebugRaidBossInfo();

	private int m_specificId = -1;

	private int m_numberOfTimes = -1;

	private int m_reservedId;

	private int m_useRaidBossEnergy;

	private long m_collectCount;

	private EventManager.EventType m_eventType = EventManager.EventType.UNKNOWN;

	private EventManager.EventType m_standbyType = EventManager.EventType.UNKNOWN;

	private EventManager.CollectEventType m_collectType = EventManager.CollectEventType.UNKNOWN;

	private EventManager.AdvertEventType m_advertType = EventManager.AdvertEventType.UNKNOWN;

	private bool m_eventStage;

	private bool m_setEventInfo;

	private bool m_appearRaidBoss;

	private bool m_synchFlag;

	private List<EventMenuData> m_datas;

	private List<RaidBossAttackRateTable> m_raidBossAttackRateList;

	private SpecialStageInfo m_specialStageInfo;

	private RaidBossInfo m_raidBossInfo;

	private EtcEventInfo m_etcEventInfo;

	private DateTime m_startTime = DateTime.MinValue;

	private DateTime m_endTime = DateTime.MinValue;

	private DateTime m_endPlayTime = DateTime.MinValue;

	private DateTime m_endResultTime = DateTime.MinValue;

	private DateTime m_closeTime = DateTime.MinValue;

	private List<ServerEventEntry> m_entryList = new List<ServerEventEntry>();

	private List<ServerEventReward> m_rewardList = new List<ServerEventReward>();

	private List<ServerEventRaidBossState> m_userRaidBossList = new List<ServerEventRaidBossState>();

	private ServerEventState m_state = new ServerEventState();

	private ServerEventUserRaidBossState m_raidState = new ServerEventUserRaidBossState();

	private ServerEventRaidBossBonus m_raidBossBonus = new ServerEventRaidBossBonus();

	public static EventManager Instance
	{
		get
		{
			return EventManager.instance;
		}
	}

	public ServerEventState State
	{
		get
		{
			return this.m_state;
		}
	}

	public List<ServerEventReward> RewardList
	{
		get
		{
			return this.m_rewardList;
		}
	}

	public DateTime EventEndTime
	{
		get
		{
			return this.m_endTime;
		}
	}

	public List<ServerEventRaidBossState> UserRaidBossList
	{
		get
		{
			return this.m_userRaidBossList;
		}
	}

	public bool AppearRaidBoss
	{
		get
		{
			return this.m_appearRaidBoss;
		}
		set
		{
			this.m_appearRaidBoss = value;
		}
	}

	public ServerEventRaidBossBonus RaidBossBonus
	{
		get
		{
			return this.m_raidBossBonus;
		}
		set
		{
			this.m_raidBossBonus = value;
		}
	}

	public DateTime EventCloseTime
	{
		get
		{
			return this.m_closeTime;
		}
	}

	public int Id
	{
		get
		{
			return this.m_eventId;
		}
		set
		{
			this.m_eventId = value;
		}
	}

	public EventManager.EventType Type
	{
		get
		{
			return this.m_eventType;
		}
	}

	public EventManager.EventType StandbyType
	{
		get
		{
			return this.m_standbyType;
		}
	}

	public EventManager.EventType TypeInTime
	{
		get
		{
			if (this.m_eventType != EventManager.EventType.UNKNOWN && this.IsInEvent())
			{
				return this.m_eventType;
			}
			return EventManager.EventType.UNKNOWN;
		}
	}

	public EventManager.AdvertEventType AdvertType
	{
		get
		{
			return this.m_advertType;
		}
	}

	public int NumberOfTimes
	{
		get
		{
			return this.m_numberOfTimes;
		}
	}

	public int ReservedId
	{
		get
		{
			return this.m_reservedId;
		}
	}

	public EventManager.CollectEventType CollectType
	{
		get
		{
			return this.m_collectType;
		}
	}

	public long CollectCount
	{
		get
		{
			return this.m_collectCount;
		}
		set
		{
			this.m_collectCount = value;
		}
	}

	public bool EventStage
	{
		get
		{
			return this.m_eventStage;
		}
		set
		{
			this.m_eventStage = value;
		}
	}

	public SpecialStageInfo SpecialStageInfo
	{
		get
		{
			return this.m_specialStageInfo;
		}
	}

	public RaidBossInfo RaidBossInfo
	{
		get
		{
			return this.m_raidBossInfo;
		}
	}

	public EtcEventInfo EtcEventInfo
	{
		get
		{
			return this.m_etcEventInfo;
		}
	}

	public bool IsSetEventStateInfo
	{
		get
		{
			return this.m_setEventInfo;
		}
	}

	public List<ServerChaoData> RecommendedChaos
	{
		get
		{
			return null;
		}
	}

	public ServerEventUserRaidBossState RaidBossState
	{
		get
		{
			return this.m_raidState;
		}
	}

	public int RaidbossChallengeCount
	{
		get
		{
			if (this.m_raidState != null && this.m_raidState.RaidBossEnergyCount >= 0)
			{
				return this.m_raidState.RaidBossEnergyCount;
			}
			return 0;
		}
	}

	public int UseRaidbossChallengeCount
	{
		get
		{
			return this.m_useRaidBossEnergy;
		}
		set
		{
			this.m_useRaidBossEnergy = value;
		}
	}

	public bool IsStandby()
	{
		return this.m_standbyType != EventManager.EventType.UNKNOWN;
	}

	public bool IsInEvent()
	{
		return this.CheckCloseTime();
	}

	public bool IsChallengeEvent()
	{
		return this.IsInEvent() && this.CheckEndTime();
	}

	public bool IsPlayEventForStage()
	{
		return this.CheckPlayingTime();
	}

	public bool IsResultEvent()
	{
		return this.CheckResultTime();
	}

	public bool IsCautionPlayEvent()
	{
		return this.IsChallengeEvent() && (this.m_endTime - NetBase.GetCurrentTime()).TotalSeconds < 1800.0;
	}

	public static EventManager.EventType GetType(int id)
	{
		if (id > 0)
		{
			switch (id / 100000000)
			{
			case 1:
				return EventManager.EventType.SPECIAL_STAGE;
			case 2:
				return EventManager.EventType.RAID_BOSS;
			case 3:
				return EventManager.EventType.COLLECT_OBJECT;
			case 4:
				return EventManager.EventType.GACHA;
			case 5:
				return EventManager.EventType.ADVERT;
			case 6:
				return EventManager.EventType.QUICK;
			case 7:
				return EventManager.EventType.BGM;
			}
		}
		return EventManager.EventType.UNKNOWN;
	}

	public static EventManager.CollectEventType GetCollectEventType(int id)
	{
		EventManager.EventType type = EventManager.GetType(id);
		if (type == EventManager.EventType.COLLECT_OBJECT)
		{
			int num = id % 100000000;
			num /= 10000;
			for (int i = 0; i < 3; i++)
			{
				if (num == EventManager.COLLECT_EVENT_SPECIFIC_ID[i])
				{
					return (EventManager.CollectEventType)i;
				}
			}
		}
		return EventManager.CollectEventType.UNKNOWN;
	}

	public static bool IsVaildEvent(int id)
	{
		if (id > 0)
		{
			int num = id / 10000;
			if (EventManager.instance != null)
			{
				int num2 = EventManager.instance.Id / 10000;
				return num2 == num;
			}
		}
		return false;
	}

	public static int GetSpecificId()
	{
		if (EventManager.instance != null)
		{
			return EventManager.instance.Id / 10000;
		}
		return -1;
	}

	public static int GetSpecificId(int eventId)
	{
		return eventId / 10000;
	}

	public static string GetResourceName()
	{
		int specificId = EventManager.GetSpecificId();
		if (EventManager.instance != null)
		{
			return EventManager.instance.GetEventTypeName() + "_" + specificId.ToString();
		}
		return string.Empty;
	}

	public bool IsQuickEvent()
	{
		return this.m_eventType == EventManager.EventType.QUICK && this.IsInEvent();
	}

	public bool IsBGMEvent()
	{
		return this.m_eventType == EventManager.EventType.BGM && this.IsInEvent();
	}

	public bool IsSpecialStage()
	{
		return this.m_eventStage && this.m_eventType == EventManager.EventType.SPECIAL_STAGE;
	}

	public bool IsRaidBossStage()
	{
		return this.m_eventStage && this.m_eventType == EventManager.EventType.RAID_BOSS;
	}

	public bool IsCollectEvent()
	{
		return this.m_eventStage && this.m_eventType == EventManager.EventType.COLLECT_OBJECT;
	}

	public bool IsGetAnimalStage()
	{
		return this.IsCollectEvent() && this.m_collectType == EventManager.CollectEventType.GET_ANIMALS;
	}

	public bool IsEncounterRaidBoss()
	{
		foreach (ServerEventRaidBossState current in this.m_userRaidBossList)
		{
			if (current.Encounter && current.GetStatusType() != ServerEventRaidBossState.StatusType.PROCESS_END)
			{
				return true;
			}
		}
		return false;
	}

	public string GetEventTypeName()
	{
		return EventManager.GetEventTypeName(this.m_eventType);
	}

	public static string GetEventTypeName(EventManager.EventType type)
	{
		if (type < EventManager.EventType.NUM)
		{
			return EventManager.EventTypeName[(int)type];
		}
		return string.Empty;
	}

	public int GetCollectEventSpecificId(EventManager.CollectEventType type)
	{
		if (type < EventManager.CollectEventType.NUM)
		{
			return EventManager.COLLECT_EVENT_SPECIFIC_ID[(int)type];
		}
		return -1;
	}

	public WindowEventData GetWindowEvenData(int texWindowId)
	{
		if (this.m_datas != null && this.m_datas.Count > 0)
		{
			WindowEventData[] window_data = this.m_datas[0].window_data;
			for (int i = 0; i < window_data.Length; i++)
			{
				WindowEventData windowEventData = window_data[i];
				if (windowEventData.id == texWindowId)
				{
					return windowEventData;
				}
			}
		}
		return null;
	}

	public EventStageData GetStageData()
	{
		if (this.m_datas != null && this.m_datas.Count > 0)
		{
			return this.m_datas[0].stage_data;
		}
		return null;
	}

	public EyeCatcherChaoData[] GetEyeCatcherChaoDatas()
	{
		if (this.m_datas != null && this.m_datas.Count > 0 && this.m_datas[0].chao_data != null)
		{
			return this.m_datas[0].chao_data.eyeCatchers;
		}
		return null;
	}

	public RewardChaoData GetRewardChaoData()
	{
		if (this.m_datas != null && this.m_datas.Count > 0 && this.m_datas[0].chao_data != null && this.m_datas[0].chao_data.rewards != null && this.m_datas[0].chao_data.rewards.Length > 0)
		{
			return this.m_datas[0].chao_data.rewards[0];
		}
		return null;
	}

	public EyeCatcherCharaData[] GetEyeCatcherCharaDatas()
	{
		if (this.m_datas != null && this.m_datas.Count > 0 && this.m_datas[0].chao_data != null)
		{
			return this.m_datas[0].chao_data.charaEyeCatchers;
		}
		return null;
	}

	public EventAvertData GetAvertData()
	{
		if (this.m_datas != null && this.m_datas.Count > 0)
		{
			return this.m_datas[0].advert_data;
		}
		return null;
	}

	public float GetRaidAttackRate(int useChallengeCount)
	{
		if (useChallengeCount > 0)
		{
			int num = useChallengeCount - 1;
			if (this.m_raidBossAttackRateList != null && this.m_raidBossAttackRateList[0].attackRate != null && num < this.m_raidBossAttackRateList[0].attackRate.Length)
			{
				return this.m_raidBossAttackRateList[0].attackRate[num];
			}
		}
		return 1f;
	}

	public void SetEventMenuData(TextAsset xml_data)
	{
		if (this.m_datas == null)
		{
			this.m_datas = new List<EventMenuData>();
		}
		else
		{
			this.m_datas.Clear();
		}
		string s = AESCrypt.Decrypt(xml_data.text);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(EventMenuData[]));
		StringReader textReader = new StringReader(s);
		EventMenuData[] array = (EventMenuData[])xmlSerializer.Deserialize(textReader);
		if (array != null && array.Length > 0 && this.m_datas != null)
		{
			this.m_datas.Add(array[0]);
		}
	}

	public void SetRaidBossAttacRate(TextAsset xml_data)
	{
		if (this.m_raidBossAttackRateList == null)
		{
			this.m_raidBossAttackRateList = new List<RaidBossAttackRateTable>();
		}
		else
		{
			this.m_raidBossAttackRateList.Clear();
		}
		string s = AESCrypt.Decrypt(xml_data.text);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(RaidBossAttackRateTable[]));
		StringReader textReader = new StringReader(s);
		RaidBossAttackRateTable[] array = (RaidBossAttackRateTable[])xmlSerializer.Deserialize(textReader);
		if (array != null && array.Length > 0 && this.m_raidBossAttackRateList != null)
		{
			this.m_raidBossAttackRateList.Add(array[0]);
		}
	}

	public void SetParameter()
	{
		this.m_eventType = EventManager.EventType.UNKNOWN;
		this.m_standbyType = EventManager.EventType.UNKNOWN;
		this.SetCurrentEvent();
		this.CalcParameter();
		this.m_synchFlag = true;
	}

	public void SetState(ServerEventState state)
	{
		if (state != null)
		{
			state.CopyTo(this.m_state);
			this.m_collectCount = this.m_state.Param;
			this.m_setEventInfo = true;
		}
	}

	public void ReCalcEndPlayTime()
	{
		if (this.m_eventType == EventManager.EventType.RAID_BOSS)
		{
			this.m_endPlayingTimeMinutes = UnityEngine.Random.Range(4, 9);
			this.m_endPlayTime = this.m_endTime + new TimeSpan(0, this.m_endPlayingTimeMinutes, 0);
		}
		else
		{
			this.m_endPlayingTimeMinutes = UnityEngine.Random.Range(24, 29);
			this.m_endPlayTime = this.m_endTime + new TimeSpan(0, this.m_endPlayingTimeMinutes, 0);
		}
	}

	public void SetDebugParameter()
	{
		this.m_endTimeHours = 72;
		this.m_debugFlag = true;
		this.SetParameter();
	}

	public void SetEventInfo()
	{
		switch (this.m_eventType)
		{
		case EventManager.EventType.SPECIAL_STAGE:
			if (this.m_debugFlag)
			{
				if (this.m_specialStageInfo == null)
				{
					this.m_specialStageInfo = SpecialStageInfo.CreateDummyData();
				}
			}
			else
			{
				this.m_specialStageInfo = SpecialStageInfo.CreateData();
			}
			break;
		case EventManager.EventType.RAID_BOSS:
			if (this.m_debugFlag)
			{
				if (this.m_raidBossInfo == null)
				{
					this.DebugSetRaidBossData();
				}
			}
			else
			{
				this.SetRaidBossData();
			}
			break;
		case EventManager.EventType.COLLECT_OBJECT:
			if (this.m_debugFlag)
			{
				if (this.m_etcEventInfo == null)
				{
					this.m_etcEventInfo = EtcEventInfo.CreateDummyData();
				}
			}
			else
			{
				this.m_etcEventInfo = EtcEventInfo.CreateData();
			}
			break;
		}
	}

	public void CheckEvent()
	{
		if (this.IsStandby())
		{
			if (this.IsInEvent())
			{
				this.SetParameter();
			}
			else if (this.m_closeTime < NetBase.GetCurrentTime())
			{
				this.ResetParameter();
				this.SetParameter();
			}
		}
		else if (this.m_eventType != EventManager.EventType.UNKNOWN)
		{
			if (this.IsInEvent())
			{
				if (this.m_eventType == EventManager.EventType.ADVERT && this.IsStartOtherEvent())
				{
					this.ResetParameter();
					this.SetParameter();
				}
			}
			else
			{
				this.ResetParameter();
				this.SetParameter();
			}
		}
	}

	public void ResetData()
	{
		if (this.m_datas != null)
		{
			this.m_datas.Clear();
			this.m_datas = null;
		}
		if (this.m_raidBossInfo != null)
		{
			this.m_raidBossInfo = null;
		}
		if (this.m_etcEventInfo != null)
		{
			this.m_etcEventInfo = null;
		}
		if (this.m_specialStageInfo != null)
		{
			this.m_specialStageInfo = null;
		}
		this.m_setEventInfo = false;
		this.m_eventStage = false;
		this.m_appearRaidBoss = false;
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		base.enabled = false;
		this.SynchServerEntryList();
	}

	private void OnDestroy()
	{
		if (EventManager.instance == this)
		{
			EventManager.instance = null;
		}
	}

	public void SynchServerEntryList()
	{
		this.m_synchFlag = false;
		this.m_entryList.Clear();
		List<ServerEventEntry> eventEntryList = ServerInterface.EventEntryList;
		if (eventEntryList != null)
		{
			int count = eventEntryList.Count;
			for (int i = 0; i < count; i++)
			{
				this.m_entryList.Add(eventEntryList[i]);
			}
		}
		this.SetParameter();
	}

	public void SynchServerEventState()
	{
		ServerEventState eventState = ServerInterface.EventState;
		if (eventState != null)
		{
			eventState.CopyTo(this.m_state);
			this.m_collectCount = this.m_state.Param;
			this.m_setEventInfo = true;
		}
	}

	public void SynchServerRewardList()
	{
		this.m_rewardList.Clear();
		List<ServerEventReward> eventRewardList = ServerInterface.EventRewardList;
		if (eventRewardList != null)
		{
			int count = eventRewardList.Count;
			for (int i = 0; i < count; i++)
			{
				this.m_rewardList.Add(eventRewardList[i]);
			}
		}
	}

	public void SynchServerEventRaidBossList(List<ServerEventRaidBossState> raidBossList)
	{
		this.m_userRaidBossList.Clear();
		if (raidBossList != null)
		{
			int count = raidBossList.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				this.m_userRaidBossList.Add(raidBossList[i]);
				EventUtility.SetRaidbossEntry(raidBossList[i]);
				if (raidBossList[i] != null && RaidBossInfo.currentRaidData != null && RaidBossInfo.currentRaidData.id == raidBossList[i].Id)
				{
					RaidBossInfo.currentRaidData = new RaidBossData(raidBossList[i]);
					flag = true;
				}
			}
			if (!flag && RaidBossInfo.currentRaidData != null)
			{
				RaidBossInfo.currentRaidData = null;
			}
		}
		this.SetRaidBossData();
	}

	public void SynchServerEventRaidBossList(ServerEventRaidBossState raidBossState)
	{
		if (raidBossState != null)
		{
			bool flag = true;
			foreach (ServerEventRaidBossState current in this.m_userRaidBossList)
			{
				if (current.Id == raidBossState.Id)
				{
					raidBossState.CopyTo(current);
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.m_userRaidBossList.Add(raidBossState);
				EventUtility.SetRaidbossEntry(raidBossState);
			}
		}
		this.SetRaidBossData();
	}

	public void SynchServerEventUserRaidBossState(ServerEventUserRaidBossState state)
	{
		if (state != null)
		{
			state.CopyTo(this.m_raidState);
		}
		if (this.m_raidBossInfo != null)
		{
			this.m_raidBossInfo.raidRing = (long)this.m_raidState.NumRaidbossRings;
			this.m_raidBossInfo.totalDestroyCount = (long)this.m_raidState.NumBeatedEnterprise;
		}
	}

	public void SynchServerEventRaidBossUserList(List<ServerEventRaidBossUserState> userList, long raidBossId, ServerEventRaidBossBonus bonus)
	{
		RaidBossData raidBossData = null;
		if (this.m_raidBossInfo != null)
		{
			List<RaidBossData> raidData = this.m_raidBossInfo.raidData;
			if (raidData != null)
			{
				foreach (RaidBossData current in raidData)
				{
					if (current.id == raidBossId)
					{
						current.SetUserList(userList);
						current.SetReward(bonus);
						raidBossData = current;
						break;
					}
				}
			}
		}
		if (raidBossData != null && RaidBossInfo.currentRaidData != null && RaidBossInfo.currentRaidData.id == raidBossId)
		{
			RaidBossInfo.currentRaidData = raidBossData;
		}
	}

	private void SetInstance()
	{
		if (EventManager.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			EventManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void CalcParameter()
	{
		if (this.m_eventType != EventManager.EventType.UNKNOWN && this.m_eventType < EventManager.EventType.NUM)
		{
			this.m_specificId = this.m_eventId % 100000000;
			this.m_specificId /= 10000;
			this.m_numberOfTimes = this.m_eventId % 10000;
			this.m_numberOfTimes /= 100;
			this.m_reservedId = this.m_eventId % 100;
			if (this.m_eventType == EventManager.EventType.COLLECT_OBJECT)
			{
				this.SetCollectEventType();
			}
			else if (this.m_eventType == EventManager.EventType.SPECIAL_STAGE || this.m_eventType == EventManager.EventType.RAID_BOSS)
			{
				if (SystemSaveManager.Instance != null)
				{
					SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
					if (systemdata != null && systemdata.pictureShowEventId != this.m_eventId)
					{
						systemdata.pictureShowEventId = this.m_eventId;
						systemdata.pictureShowProgress = -1;
						systemdata.pictureShowEmergeRaidBossProgress = -1;
						systemdata.pictureShowRaidBossFirstBattle = -1;
						SystemSaveManager.Instance.SaveSystemData();
					}
				}
			}
			else if (this.m_eventType == EventManager.EventType.ADVERT)
			{
				this.SetAdvertEventType();
			}
		}
	}

	private void SetCurrentEvent()
	{
		ServerEventEntry serverEventEntry = null;
		ServerEventEntry serverEventEntry2 = null;
		DateTime currentTime = NetBase.GetCurrentTime();
		foreach (ServerEventEntry current in this.m_entryList)
		{
			DateTime eventCloseTime = current.EventCloseTime;
			if (eventCloseTime > currentTime)
			{
				EventManager.EventType type = EventManager.GetType(current.EventId);
				if (type == EventManager.EventType.ADVERT)
				{
					if (serverEventEntry2 == null)
					{
						serverEventEntry2 = current;
					}
					else if (current.EventStartTime < serverEventEntry2.EventStartTime)
					{
						serverEventEntry2 = current;
					}
				}
				else if (serverEventEntry == null)
				{
					serverEventEntry = current;
				}
				else if (current.EventStartTime < serverEventEntry.EventStartTime)
				{
					serverEventEntry = current;
				}
			}
		}
		bool flag = false;
		if (serverEventEntry != null && serverEventEntry2 != null)
		{
			if (serverEventEntry2.EventStartTime < serverEventEntry.EventStartTime)
			{
				DateTime eventStartTime = serverEventEntry.EventStartTime;
				if (eventStartTime > currentTime)
				{
					flag = true;
				}
			}
		}
		else if (serverEventEntry == null && serverEventEntry2 != null)
		{
			flag = true;
		}
		ServerEventEntry serverEventEntry3 = (!flag) ? serverEventEntry : serverEventEntry2;
		if (serverEventEntry3 != null)
		{
			this.m_startTime = serverEventEntry3.EventStartTime;
			this.m_endTime = serverEventEntry3.EventEndTime;
			this.m_closeTime = serverEventEntry3.EventCloseTime;
			this.m_eventId = serverEventEntry3.EventId;
			if (this.IsInEvent())
			{
				this.m_eventType = EventManager.GetType(this.m_eventId);
				this.ReCalcEndPlayTime();
				if (this.m_eventType == EventManager.EventType.RAID_BOSS)
				{
					this.m_endResultTimeMinutes = 10;
					this.m_endResultTime = this.m_endTime + new TimeSpan(0, this.m_endResultTimeMinutes, 0);
				}
				else
				{
					this.m_endResultTimeMinutes = 30;
					this.m_endResultTime = this.m_endTime + new TimeSpan(0, this.m_endResultTimeMinutes, 0);
				}
			}
			else
			{
				this.m_standbyType = EventManager.GetType(this.m_eventId);
			}
		}
	}

	private bool IsStartOtherEvent()
	{
		ServerEventEntry serverEventEntry = null;
		DateTime currentTime = NetBase.GetCurrentTime();
		foreach (ServerEventEntry current in this.m_entryList)
		{
			if (EventManager.GetType(current.EventId) != EventManager.EventType.ADVERT)
			{
				DateTime eventEndTime = current.EventEndTime;
				if (eventEndTime > currentTime)
				{
					if (serverEventEntry == null)
					{
						serverEventEntry = current;
					}
					else if (current.EventStartTime < serverEventEntry.EventStartTime)
					{
						serverEventEntry = current;
					}
				}
			}
		}
		if (serverEventEntry != null)
		{
			DateTime eventStartTime = serverEventEntry.EventStartTime;
			if (eventStartTime <= currentTime)
			{
				return true;
			}
		}
		return false;
	}

	private void DebugSetCurrentEvent()
	{
		if (this.m_debugFlag && this.m_eventId > 0)
		{
			if (!this.m_synchFlag && this.m_endTimeHours + this.m_endTimeMinutes + this.m_endTimeSeconds > 0)
			{
				TimeSpan timeSpan = new TimeSpan(this.m_endTimeHours, this.m_endTimeMinutes, this.m_endTimeSeconds);
				TimeSpan timeSpan2 = new TimeSpan(this.m_closeTimeHours, this.m_closeTimeMinutes, this.m_closeTimeSeconds);
				if (timeSpan > timeSpan2)
				{
					timeSpan2 = timeSpan;
				}
				TimeSpan t = new TimeSpan(this.m_startTimeHours, this.m_startTimeMinutes, this.m_startTimeSeconds);
				this.m_startTime = NetBase.GetCurrentTime() + t;
				this.m_endTime = this.m_startTime + timeSpan;
				this.m_closeTime = this.m_startTime + timeSpan2;
				this.m_endPlayTime = this.m_endTime + new TimeSpan(0, this.m_endPlayingTimeMinutes, 0);
				this.m_endResultTime = this.m_endTime + new TimeSpan(0, this.m_endResultTimeMinutes, 0);
			}
			if (this.IsInEvent())
			{
				this.m_eventType = EventManager.GetType(this.m_eventId);
			}
			else
			{
				this.m_standbyType = EventManager.GetType(this.m_eventId);
			}
		}
	}

	private void SetRaidBossData()
	{
		if (this.m_eventType != EventManager.EventType.RAID_BOSS)
		{
			return;
		}
		if (this.m_raidBossInfo != null)
		{
			if (this.m_raidBossInfo.raidData != null)
			{
				this.m_raidBossInfo.raidData.Clear();
				foreach (ServerEventRaidBossState current in this.m_userRaidBossList)
				{
					this.m_raidBossInfo.raidData.Add(new RaidBossData(current));
				}
			}
		}
		else
		{
			List<RaidBossData> list = new List<RaidBossData>();
			foreach (ServerEventRaidBossState current2 in this.m_userRaidBossList)
			{
				list.Add(new RaidBossData(current2));
			}
			this.m_raidBossInfo = RaidBossInfo.CreateData(list);
		}
		if (this.m_raidBossInfo != null)
		{
			this.m_raidBossInfo.raidRing = (long)this.m_raidState.NumRaidbossRings;
			this.m_raidBossInfo.totalDestroyCount = (long)this.m_raidState.NumBeatedEnterprise;
		}
		this.m_setEventInfo = true;
	}

	private void DebugSetRaidBossData()
	{
		if (this.m_eventType != EventManager.EventType.RAID_BOSS)
		{
			return;
		}
		List<RaidBossData> list = new List<RaidBossData>();
		EventManager.DebugRaidBoss[] debugRaidBossDatas = this.m_debugRaidBossInfo.m_debugRaidBossDatas;
		for (int i = 0; i < debugRaidBossDatas.Length; i++)
		{
			EventManager.DebugRaidBoss debugRaidBoss = debugRaidBossDatas[i];
			if (debugRaidBoss.m_validFlag)
			{
				ServerEventRaidBossState serverEventRaidBossState = new ServerEventRaidBossState();
				DateTime escapeAt = NetBase.GetCurrentTime() + new TimeSpan(0, debugRaidBoss.m_endTimeMinutes, 0);
				serverEventRaidBossState.Id = (long)debugRaidBoss.m_id;
				serverEventRaidBossState.Rarity = debugRaidBoss.m_rarity;
				serverEventRaidBossState.Level = debugRaidBoss.m_level;
				serverEventRaidBossState.EncounterName = debugRaidBoss.m_discovererName;
				serverEventRaidBossState.Encounter = debugRaidBoss.m_findMyself;
				serverEventRaidBossState.Status = debugRaidBoss.m_state;
				serverEventRaidBossState.EscapeAt = escapeAt;
				serverEventRaidBossState.HitPoint = debugRaidBoss.m_hp;
				serverEventRaidBossState.MaxHitPoint = debugRaidBoss.m_hpMax;
				list.Add(new RaidBossData(serverEventRaidBossState));
			}
		}
		this.m_setEventInfo = true;
		this.m_raidBossInfo = RaidBossInfo.CreateDataForDebugData(list);
		if (this.m_raidBossInfo != null)
		{
			int debugCurrentRaidBossDataIndex = this.m_debugRaidBossInfo.m_debugCurrentRaidBossDataIndex;
			if (this.m_raidBossInfo.raidData.Count > debugCurrentRaidBossDataIndex && debugCurrentRaidBossDataIndex >= 0)
			{
				RaidBossInfo.currentRaidData = this.m_raidBossInfo.raidData[debugCurrentRaidBossDataIndex];
			}
			this.m_appearRaidBoss = this.m_debugRaidBossInfo.m_debugRaidBossDescentFlag;
			this.m_raidBossInfo.raidRing = (long)this.m_debugRaidBossInfo.m_raidBossRingNum;
			this.m_raidBossInfo.totalDestroyCount = (long)this.m_debugRaidBossInfo.m_raidBossKillNum;
		}
		if (this.m_raidState != null)
		{
			this.m_raidState.RaidBossEnergy = 20;
		}
	}

	private void ResetParameter()
	{
		this.ResetData();
		this.m_eventType = EventManager.EventType.UNKNOWN;
		this.m_standbyType = EventManager.EventType.UNKNOWN;
		this.m_collectType = EventManager.CollectEventType.UNKNOWN;
		this.m_advertType = EventManager.AdvertEventType.UNKNOWN;
		this.m_eventId = -1;
		this.m_specificId = -1;
		this.m_numberOfTimes = -1;
		this.m_reservedId = -1;
		this.m_useRaidBossEnergy = 0;
		this.m_startTime = DateTime.MinValue;
		this.m_endTime = DateTime.MinValue;
		this.m_closeTime = DateTime.MinValue;
		this.m_endPlayTime = DateTime.MinValue;
		this.m_endResultTime = DateTime.MinValue;
	}

	private bool CheckCloseTime()
	{
		DateTime currentTime = NetBase.GetCurrentTime();
		return currentTime >= this.m_startTime && this.m_closeTime > currentTime;
	}

	private bool CheckEndTime()
	{
		DateTime currentTime = NetBase.GetCurrentTime();
		return currentTime >= this.m_startTime && this.m_endTime > currentTime;
	}

	private bool CheckPlayingTime()
	{
		if (this.m_eventType != EventManager.EventType.UNKNOWN)
		{
			DateTime currentTime = NetBase.GetCurrentTime();
			if (currentTime >= this.m_startTime)
			{
				return this.m_endPlayTime > currentTime;
			}
		}
		return false;
	}

	private bool CheckResultTime()
	{
		if (this.m_eventType != EventManager.EventType.UNKNOWN)
		{
			DateTime currentTime = NetBase.GetCurrentTime();
			if (currentTime >= this.m_startTime)
			{
				return this.m_endResultTime > currentTime;
			}
		}
		return false;
	}

	private void SetCollectEventType()
	{
		this.m_collectType = EventManager.CollectEventType.GET_ANIMALS;
		for (int i = 0; i < 3; i++)
		{
			if (EventManager.COLLECT_EVENT_SPECIFIC_ID[i] == this.m_specificId)
			{
				this.m_collectType = (EventManager.CollectEventType)i;
				break;
			}
		}
	}

	private void SetAdvertEventType()
	{
		this.m_advertType = EventManager.AdvertEventType.UNKNOWN;
		if (this.m_specificId < 1000)
		{
			this.m_advertType = EventManager.AdvertEventType.ROULETTE;
		}
		else if (this.m_specificId < 2000)
		{
			this.m_advertType = EventManager.AdvertEventType.CHARACTER;
		}
		else if (this.m_specificId < 3000)
		{
			this.m_advertType = EventManager.AdvertEventType.SHOP;
		}
	}

	public EventProductionData GetPuductionData()
	{
		if (this.m_datas != null && this.m_datas.Count > 0)
		{
			return this.m_datas[0].puduction_data;
		}
		return null;
	}

	public EventRaidProductionData GetRaidProductionData()
	{
		if (this.m_datas != null && this.m_datas.Count > 0)
		{
			return this.m_datas[0].raid_data;
		}
		return null;
	}
}
