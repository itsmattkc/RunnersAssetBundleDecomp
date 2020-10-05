using DataTable;
using Message;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RankingManager : SingletonGameObject<RankingManager>
{
	private class CallbackData
	{
		private RankingManager.CallbackRankingData _callback_k__BackingField;

		private int _rankingType_k__BackingField;

		private int _getPage_k__BackingField;

		private float _startTime_k__BackingField;

		public RankingManager.CallbackRankingData callback
		{
			get;
			set;
		}

		public int rankingType
		{
			get;
			set;
		}

		public int getPage
		{
			get;
			set;
		}

		public float startTime
		{
			get;
			set;
		}

		public CallbackData(RankingManager.CallbackRankingData target, int ranking, int page)
		{
			this.callback = target;
			this.rankingType = ranking;
			this.getPage = page;
			this.startTime = Time.realtimeSinceStartup;
		}

		public bool Check(int ranking, int page)
		{
			bool result = false;
			if (ranking == this.rankingType && this.getPage == page)
			{
				result = true;
			}
			return result;
		}
	}

	public delegate void CallbackRankingData(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData);

	public const long CHAO_ID_OFFSET = 1000000L;

	public const float CHAO_TEX_LOAD_DELAY = 0.25f;

	public const float AUTO_RELOAD_TIME = 5f;

	private const int CALLBACK_STACK_MAX = 256;

	public const int PAGE0_RANKER_COUNT = 3;

	private const int PAGE_RANKER_COUNT_INIT = 70;

	private const int PAGE_RANKER_COUNT_MARGIN = 20;

	private const int PAGE_RANKER_COUNT_SAME = 100;

	private Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet> m_rankingDataSet;

	private RankingUtil.RankingMode m_mode;

	private RankingUtil.RankingScoreType m_scoreType;

	private RankingUtil.RankingRankerType m_rankerType = RankingUtil.RankingRankerType.ALL;

	private bool m_isLoading;

	private float m_getRankingLastTime;

	private int m_page;

	private int m_eventId;

	private bool m_isSpRankingInit;

	private bool m_isReset = true;

	private bool m_isRankingInit;

	private bool m_isRankingPageCheck;

	private List<RankingManager.CallbackData> m_callbacks = new List<RankingManager.CallbackData>();

	private RankingManager.CallbackRankingData m_callbackBakNormalAll;

	private RankingManager.CallbackRankingData m_callbackBakEventAll;

	private float m_chaoTextureLoadTime = -1f;

	private float m_chaoTextureLoadEndTime = -1f;

	private Dictionary<int, float> m_chaoTextureLoad;

	private Dictionary<int, Texture> m_chaoTextureList;

	private Dictionary<int, List<UITexture>> m_chaoTextureObject;

	private int m_initLoadCount;

	private List<int> m_chainGetRankingCodeList;

	public int eventId
	{
		get
		{
			return this.m_eventId;
		}
	}

	public static RankingUtil.RankingScoreType EndlessRivalRankingScoreType
	{
		get
		{
			RankingUtil.RankingScoreType result = RankingUtil.RankingScoreType.HIGH_SCORE;
			if (SingletonGameObject<RankingManager>.Instance != null)
			{
				RankingUtil.RankingDataSet rankingDataSet = SingletonGameObject<RankingManager>.Instance.GetRankingDataSet(RankingUtil.RankingMode.ENDLESS);
				if (rankingDataSet != null)
				{
					result = rankingDataSet.targetRivalScoreType;
				}
			}
			return result;
		}
	}

	public static RankingUtil.RankingScoreType QuickRivalRankingScoreType
	{
		get
		{
			RankingUtil.RankingScoreType result = RankingUtil.RankingScoreType.HIGH_SCORE;
			if (SingletonGameObject<RankingManager>.Instance != null)
			{
				RankingUtil.RankingDataSet rankingDataSet = SingletonGameObject<RankingManager>.Instance.GetRankingDataSet(RankingUtil.RankingMode.QUICK);
				if (rankingDataSet != null)
				{
					result = rankingDataSet.targetRivalScoreType;
				}
			}
			return result;
		}
	}

	public static RankingUtil.RankingScoreType EndlessSpecialRankingScoreType
	{
		get
		{
			RankingUtil.RankingScoreType result = RankingUtil.RankingScoreType.TOTAL_SCORE;
			if (SingletonGameObject<RankingManager>.Instance != null)
			{
				RankingUtil.RankingDataSet rankingDataSet = SingletonGameObject<RankingManager>.Instance.GetRankingDataSet(RankingUtil.RankingMode.ENDLESS);
				if (rankingDataSet != null)
				{
					result = rankingDataSet.targetSpScoreType;
				}
			}
			return result;
		}
	}

	public RankingUtil.RankingMode mode
	{
		get
		{
			return this.m_mode;
		}
	}

	public RankingUtil.RankingScoreType scoreType
	{
		get
		{
			return this.m_scoreType;
		}
	}

	public RankingUtil.RankingRankerType rankerType
	{
		get
		{
			return this.m_rankerType;
		}
	}

	public bool isSpRankingInit
	{
		get
		{
			return this.m_isSpRankingInit;
		}
	}

	public bool isLoading
	{
		get
		{
			if (this.m_isLoading)
			{
				float num = Mathf.Abs(this.m_getRankingLastTime - Time.realtimeSinceStartup);
				return num > 0.15f;
			}
			return false;
		}
	}

	public bool isChaoTextureLoading
	{
		get
		{
			return this.m_chaoTextureObject != null && this.m_chaoTextureObject.Count > 0;
		}
	}

	public bool isRankingInit
	{
		get
		{
			return this.m_isRankingInit;
		}
	}

	public bool isRankingPageCheck
	{
		get
		{
			return this.m_isRankingPageCheck;
		}
		set
		{
			this.m_isRankingPageCheck = value;
		}
	}

	public bool isReset
	{
		get
		{
			return this.m_isReset;
		}
	}

	public void SetRankingDataSet(ServerWeeklyLeaderboardOptions leaderboardOptions)
	{
		if (this.m_rankingDataSet == null)
		{
			this.m_rankingDataSet = new Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>();
		}
		int num = leaderboardOptions.mode;
		if (num < 0 || num >= 2)
		{
			num = 0;
		}
		if (this.m_rankingDataSet.ContainsKey((RankingUtil.RankingMode)num))
		{
			this.m_rankingDataSet[(RankingUtil.RankingMode)num].Setup(leaderboardOptions);
		}
		else
		{
			RankingUtil.RankingDataSet value = new RankingUtil.RankingDataSet(leaderboardOptions);
			this.m_rankingDataSet.Add((RankingUtil.RankingMode)num, value);
		}
	}

	public RankingUtil.RankingDataSet GetRankingDataSet(RankingUtil.RankingMode rankingMode)
	{
		RankingUtil.RankingDataSet result = null;
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.ContainsKey(rankingMode))
		{
			result = this.m_rankingDataSet[rankingMode];
		}
		return result;
	}

	private RankingDataContainer GetRankingDataContainer(RankingUtil.RankingMode rankingMode)
	{
		RankingDataContainer result = null;
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.ContainsKey(rankingMode))
		{
			result = this.m_rankingDataSet[rankingMode].dataContainer;
		}
		return result;
	}

	public bool Init(RankingManager.CallbackRankingData callbackNormalAll, RankingManager.CallbackRankingData callbackEventAll = null)
	{
		global::Debug.Log("! RankingManager:Init isLoading:" + this.isLoading);
		this.m_initLoadCount = 0;
		if (this.isLoading)
		{
			return false;
		}
		this.m_callbackBakNormalAll = null;
		this.m_callbackBakEventAll = null;
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.Count >= 2)
		{
			return this.RankingInit(callbackNormalAll, callbackEventAll);
		}
		this.m_callbackBakNormalAll = callbackNormalAll;
		this.m_callbackBakEventAll = callbackEventAll;
		ServerInterface.LoggedInServerInterface.RequestServerGetWeeklyLeaderboardOptions(0, base.gameObject);
		return true;
	}

	private void ServerGetWeeklyLeaderboardOptions_Succeeded(MsgGetWeeklyLeaderboardOptions msg)
	{
		global::Debug.Log("RankingManager: ServerGetWeeklyLeaderboardOptions_Succeeded  mode:" + msg.m_weeklyLeaderboardOptions.mode);
		this.m_initLoadCount++;
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.Count >= 2)
		{
			this.m_initLoadCount = 0;
			ServerInterface.LoggedInServerInterface.RequestServerGetLeagueData(0, base.gameObject);
		}
		else if (this.m_initLoadCount > 2)
		{
			global::Debug.Log("RankingManager: ServerGetWeeklyLeaderboardOptions_Succeeded error !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			ServerWeeklyLeaderboardOptions serverWeeklyLeaderboardOptions = new ServerWeeklyLeaderboardOptions();
			msg.m_weeklyLeaderboardOptions.CopyTo(serverWeeklyLeaderboardOptions);
			serverWeeklyLeaderboardOptions.mode = 1;
			this.SetRankingDataSet(serverWeeklyLeaderboardOptions);
			this.m_initLoadCount = 0;
			ServerInterface.LoggedInServerInterface.RequestServerGetLeagueData(0, base.gameObject);
		}
		else
		{
			ServerInterface.LoggedInServerInterface.RequestServerGetWeeklyLeaderboardOptions(msg.m_weeklyLeaderboardOptions.mode + 1, base.gameObject);
		}
	}

	private void ServerGetLeagueData_Succeeded(MsgGetLeagueDataSucceed msg)
	{
		global::Debug.Log("RankingManager: ServerGetLeagueData_Succeeded count:" + this.m_initLoadCount);
		this.m_initLoadCount++;
		int nextLeagueDataMode = this.GetNextLeagueDataMode();
		if (nextLeagueDataMode < 0)
		{
			RankingLeagueTable.SetupRankingLeagueTable();
			this.RankingInit(null, this.m_callbackBakEventAll);
		}
		else if (this.m_initLoadCount > 2)
		{
			global::Debug.Log("RankingManager: ServerGetLeagueData_Succeeded error !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			if (this.m_rankingDataSet != null && this.m_rankingDataSet.Count > 0)
			{
				Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>.KeyCollection keys = this.m_rankingDataSet.Keys;
				foreach (RankingUtil.RankingMode current in keys)
				{
					if (this.m_rankingDataSet[current].leagueData == null)
					{
						this.m_rankingDataSet[current].SetLeagueData(this.m_rankingDataSet[RankingUtil.RankingMode.ENDLESS].leagueData);
					}
				}
			}
			RankingLeagueTable.SetupRankingLeagueTable();
			this.RankingInit(null, this.m_callbackBakEventAll);
		}
		else
		{
			ServerInterface.LoggedInServerInterface.RequestServerGetLeagueData(nextLeagueDataMode, base.gameObject);
		}
	}

	private bool RankingInit(RankingManager.CallbackRankingData callbackNormalAll, RankingManager.CallbackRankingData callbackEventAll)
	{
		global::Debug.Log("! RankingManager:RankingInit isLoading:" + this.isLoading);
		if (this.isLoading)
		{
			return false;
		}
		this.m_isRankingPageCheck = false;
		this.m_page = -1;
		this.m_eventId = 0;
		this.m_getRankingLastTime = 0f;
		EventManager.EventType type = EventManager.Instance.Type;
		this.m_isRankingInit = true;
		this.ResetRankingData(RankingUtil.RankingMode.ENDLESS);
		this.ResetRankingData(RankingUtil.RankingMode.QUICK);
		this.m_isSpRankingInit = false;
		RankingUtil.RankingMode rankingMode = RankingUtil.RankingMode.ENDLESS;
		RankingUtil.RankingScoreType scoreType = RankingUtil.RankingScoreType.HIGH_SCORE;
		this.m_chainGetRankingCodeList = new List<int>();
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.Count > 0)
		{
			int num = 0;
			Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>.KeyCollection keys = this.m_rankingDataSet.Keys;
			foreach (RankingUtil.RankingMode current in keys)
			{
				if (num == 0)
				{
					rankingMode = current;
					scoreType = this.m_rankingDataSet[current].targetRivalScoreType;
				}
				int rankingCode = RankingUtil.GetRankingCode(current, this.m_rankingDataSet[current].targetRivalScoreType, RankingUtil.RankingRankerType.RIVAL);
				if (rankingCode >= 0)
				{
					this.m_chainGetRankingCodeList.Add(rankingCode);
				}
				num++;
			}
		}
		if (callbackNormalAll == null)
		{
			callbackNormalAll = new RankingManager.CallbackRankingData(this.DefaultCallback);
		}
		this.GetRanking(rankingMode, scoreType, RankingUtil.RankingRankerType.RIVAL, 0, callbackNormalAll);
		if (type == EventManager.EventType.SPECIAL_STAGE)
		{
			if (callbackEventAll == null)
			{
				callbackEventAll = new RankingManager.CallbackRankingData(this.EventRankingInitCallback);
			}
			this.GetRanking(RankingUtil.RankingMode.ENDLESS, RankingManager.EndlessSpecialRankingScoreType, RankingUtil.RankingRankerType.SP_ALL, 0, callbackEventAll);
		}
		else if (callbackEventAll != null)
		{
			List<RankingUtil.Ranker> rankerList = new List<RankingUtil.Ranker>();
			callbackEventAll(rankerList, RankingManager.EndlessSpecialRankingScoreType, RankingUtil.RankingRankerType.SP_ALL, 0, false, false, true);
		}
		return true;
	}

	public bool InitNormal(RankingUtil.RankingMode rankingMode, RankingManager.CallbackRankingData callback)
	{
		RankingUtil.RankingScoreType scoreType;
		if (rankingMode == RankingUtil.RankingMode.ENDLESS)
		{
			scoreType = RankingManager.EndlessRivalRankingScoreType;
		}
		else
		{
			scoreType = RankingManager.QuickRivalRankingScoreType;
		}
		global::Debug.Log("! RankingManager:InitNormal isLoading:" + this.isLoading);
		if (this.isLoading)
		{
			return false;
		}
		this.m_isRankingPageCheck = false;
		this.m_page = -1;
		this.m_eventId = 0;
		this.m_getRankingLastTime = 0f;
		this.m_isRankingInit = true;
		this.ResetRankingData(RankingUtil.RankingMode.ENDLESS);
		this.m_isSpRankingInit = false;
		if (callback == null)
		{
			callback = new RankingManager.CallbackRankingData(this.DefaultCallback);
		}
		this.GetRanking(rankingMode, scoreType, RankingUtil.RankingRankerType.RIVAL, 0, callback);
		return true;
	}

	public bool InitSp(RankingManager.CallbackRankingData callback)
	{
		global::Debug.Log("! RankingManager:InitSp isLoading:" + this.isLoading);
		if (this.isLoading)
		{
			return false;
		}
		this.m_page = -1;
		this.m_eventId = 0;
		this.m_getRankingLastTime = 0f;
		EventManager.EventType type = EventManager.Instance.Type;
		this.m_isRankingInit = true;
		this.ResetRankingData(RankingUtil.RankingMode.ENDLESS);
		this.m_isSpRankingInit = false;
		if (type == EventManager.EventType.SPECIAL_STAGE)
		{
			if (callback == null)
			{
				callback = new RankingManager.CallbackRankingData(this.EventRankingInitCallback);
			}
			this.GetRanking(RankingUtil.RankingMode.ENDLESS, RankingManager.EndlessSpecialRankingScoreType, RankingUtil.RankingRankerType.SP_ALL, 0, callback);
		}
		else if (callback != null)
		{
			callback(null, RankingManager.EndlessSpecialRankingScoreType, RankingUtil.RankingRankerType.SP_ALL, 0, false, false, true);
		}
		return true;
	}

	private void Update()
	{
		if (this.m_chaoTextureLoadTime >= 0f)
		{
			float deltaTime = Time.deltaTime;
			if (this.m_chaoTextureObject != null && this.m_chaoTextureObject.Count > 0)
			{
				if (this.m_chaoTextureLoad != null && this.m_chaoTextureLoad.Count > 0)
				{
					int[] array = new int[this.m_chaoTextureLoad.Count];
					this.m_chaoTextureLoad.Keys.CopyTo(array, 0);
					List<int> list = null;
					int[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						int num = array2[i];
						Dictionary<int, float> chaoTextureLoad;
						Dictionary<int, float> expr_8A = chaoTextureLoad = this.m_chaoTextureLoad;
						int key;
						int expr_8E = key = num;
						float num2 = chaoTextureLoad[key];
						expr_8A[expr_8E] = num2 - deltaTime;
						if (this.m_chaoTextureLoad[num] <= 0f && this.m_chaoTextureObject.ContainsKey(num) && this.m_chaoTextureList != null && this.m_chaoTextureList.ContainsKey(num))
						{
							List<UITexture> list2 = this.m_chaoTextureObject[num];
							if (list2 != null && list2.Count > 0)
							{
								foreach (UITexture current in list2)
								{
									current.mainTexture = this.m_chaoTextureList[num];
									current.alpha = 1f;
								}
							}
							if (list == null)
							{
								list = new List<int>();
							}
							list.Add(num);
						}
					}
					if (list != null)
					{
						foreach (int current2 in list)
						{
							this.m_chaoTextureLoad.Remove(current2);
						}
					}
				}
			}
			else
			{
				this.m_chaoTextureLoadEndTime += deltaTime;
			}
			this.m_chaoTextureLoadTime += deltaTime;
			if (this.m_chaoTextureLoadEndTime > 5f && this.m_chaoTextureLoad != null && this.m_chaoTextureLoad.Count <= 0)
			{
				this.m_chaoTextureLoadTime = -1f;
			}
		}
	}

	public ServerLeagueData GetLeagueData(RankingUtil.RankingMode rankingMode)
	{
		ServerLeagueData result = null;
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.Count > 0 && this.m_rankingDataSet.ContainsKey(rankingMode))
		{
			result = this.m_rankingDataSet[rankingMode].leagueData;
		}
		return result;
	}

	public bool SetLeagueData(ServerLeagueData data)
	{
		bool result = false;
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.Count > 0)
		{
			Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>.KeyCollection keys = this.m_rankingDataSet.Keys;
			foreach (RankingUtil.RankingMode current in keys)
			{
				if (current == data.rankinMode)
				{
					this.m_rankingDataSet[current].SetLeagueData(data);
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public int GetNextLeagueDataMode()
	{
		int result = -1;
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.Count > 0)
		{
			Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>.KeyCollection keys = this.m_rankingDataSet.Keys;
			foreach (RankingUtil.RankingMode current in keys)
			{
				if (this.m_rankingDataSet[current].leagueData == null)
				{
					result = (int)current;
					break;
				}
			}
		}
		return result;
	}

	public void Reset(RankingUtil.RankingMode mode, RankingUtil.RankingRankerType type)
	{
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.ContainsKey(mode))
		{
			this.m_rankingDataSet[mode].Reset(type);
		}
	}

	private void ResetRankingData(RankingUtil.RankingMode mode)
	{
		if (this.m_rankingDataSet != null)
		{
			if (this.m_rankingDataSet.ContainsKey(mode))
			{
				this.m_rankingDataSet[mode].Reset();
			}
		}
		else
		{
			this.m_rankingDataSet = new Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>();
		}
		RankingUI.SetLoading();
		if (EventManager.Instance.Type == EventManager.EventType.SPECIAL_STAGE)
		{
			SpecialStageWindow.SetLoading();
		}
		this.m_callbacks = null;
		this.m_callbacks = new List<RankingManager.CallbackData>();
		this.m_isReset = true;
		this.ResetChaoTexture();
	}

	public RankingUtil.RankChange GetRankingRankChange(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
	{
		RankingUtil.RankChange result = RankingUtil.RankChange.NONE;
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer != null)
		{
			result = rankingDataContainer.GetRankChange(scoreType, rankerType);
		}
		return result;
	}

	public RankingUtil.RankChange GetRankingRankChange(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, out int currentRank, out int oldRank)
	{
		RankingUtil.RankChange result = RankingUtil.RankChange.NONE;
		currentRank = -1;
		oldRank = -1;
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer != null)
		{
			result = rankingDataContainer.GetRankChange(scoreType, rankerType, out currentRank, out oldRank);
		}
		return result;
	}

	public RankingUtil.RankChange GetRankingRankChange(RankingUtil.RankingMode rankingMode)
	{
		RankingUtil.RankingDataSet rankingDataSet = this.GetRankingDataSet(rankingMode);
		RankingUtil.RankingRankerType rankerType = RankingUtil.RankingRankerType.RIVAL;
		RankingUtil.RankingScoreType targetRivalScoreType = rankingDataSet.targetRivalScoreType;
		return this.GetRankingRankChange(rankingMode, targetRivalScoreType, rankerType);
	}

	public RankingUtil.RankChange GetRankingRankChange(RankingUtil.RankingMode rankingMode, out int currentRank, out int oldRank)
	{
		RankingUtil.RankingDataSet rankingDataSet = this.GetRankingDataSet(rankingMode);
		RankingUtil.RankingRankerType rankerType = RankingUtil.RankingRankerType.RIVAL;
		RankingUtil.RankingScoreType targetRivalScoreType = rankingDataSet.targetRivalScoreType;
		return this.GetRankingRankChange(rankingMode, targetRivalScoreType, rankerType, out currentRank, out oldRank);
	}

	public void ResetRankingRankChange(RankingUtil.RankingMode rankingMode)
	{
		RankingUtil.RankingDataSet rankingDataSet = this.GetRankingDataSet(rankingMode);
		if (rankingDataSet != null)
		{
			RankingUtil.RankingRankerType rankerType = RankingUtil.RankingRankerType.RIVAL;
			RankingUtil.RankingScoreType targetRivalScoreType = rankingDataSet.targetRivalScoreType;
			rankingDataSet.ResetRankChange(targetRivalScoreType, rankerType);
		}
	}

	public bool GetRanking(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, int page, RankingManager.CallbackRankingData callback)
	{
		if (!this.m_isRankingInit)
		{
			return false;
		}
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		this.m_isReset = false;
		if (rankerType == RankingUtil.RankingRankerType.RIVAL)
		{
			page = 0;
		}
		bool flag = false;
		if (this.isLoading && this.m_scoreType == scoreType && this.m_rankerType == rankerType && (this.m_page == page || page == 0) && this.m_callbacks != null)
		{
			if (this.m_callbacks.Count > 0)
			{
				flag = true;
				foreach (RankingManager.CallbackData current in this.m_callbacks)
				{
					if (current.rankingType == RankingUtil.GetRankingType(scoreType, rankerType) && (current.getPage == page || page == 0))
					{
						flag = false;
						current.getPage = page;
						current.callback = callback;
						return false;
					}
				}
			}
			else
			{
				flag = true;
			}
			if (flag && callback != null)
			{
				List<RankingUtil.Ranker> list = null;
				if (page > 1)
				{
					page--;
				}
				if (this.IsRankingList(rankingMode, scoreType, rankerType, page))
				{
					list = this.GetRankerList(rankingMode, scoreType, rankerType, page);
				}
				if (list != null && rankingDataContainer != null)
				{
					MsgGetLeaderboardEntriesSucceed rankerListOrg = rankingDataContainer.GetRankerListOrg(rankerType, scoreType, page);
					callback(list, scoreType, rankerType, page, rankerListOrg.m_leaderboardEntries.IsNext(), rankerListOrg.m_leaderboardEntries.IsPrev(), true);
					return true;
				}
				return false;
			}
		}
		if (page < 0)
		{
			return false;
		}
		bool flag2 = this.IsRankingList(rankingMode, scoreType, rankerType, page);
		int rankingType = RankingUtil.GetRankingType(scoreType, rankerType);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		bool flag3 = true;
		MsgGetLeaderboardEntriesSucceed msgGetLeaderboardEntriesSucceed = null;
		if (flag2)
		{
			msgGetLeaderboardEntriesSucceed = rankingDataContainer.GetRankerListOrg(rankerType, scoreType, page);
			if (msgGetLeaderboardEntriesSucceed == null)
			{
				flag3 = true;
			}
			else if (page == 0)
			{
				flag3 = rankingDataContainer.IsRankerListReload(rankerType, scoreType);
			}
			else if (page > 0)
			{
				flag3 = true;
			}
		}
		if (page >= 2)
		{
			flag3 = true;
		}
		if (flag3)
		{
			this.m_isLoading = true;
			this.m_scoreType = scoreType;
			this.m_rankerType = rankerType;
			this.m_page = page;
			this.m_eventId = EventManager.Instance.Id;
			if (callback != null && this.m_callbacks != null)
			{
				RankingManager.CallbackData item = new RankingManager.CallbackData(callback, rankingType, page);
				this.m_callbacks.Add(item);
				if (this.m_callbacks.Count > 256)
				{
					this.m_callbacks.RemoveAt(0);
				}
			}
			int rankingTop = this.GetRankingTop(rankingMode, rankerType, scoreType, page);
			int rankingSize = RankingManager.GetRankingSize(rankerType, rankingTop, page);
			string[] friendIdList = RankingUtil.GetFriendIdList();
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetLeaderboardEntries((int)rankingMode, rankingTop, rankingSize, page, rankingType, this.m_eventId, friendIdList, base.gameObject);
			}
		}
		else if (flag2 && callback != null)
		{
			List<RankingUtil.Ranker> rankerList = this.GetRankerList(rankingMode, scoreType, rankerType, page);
			if (rankerList != null)
			{
				callback(rankerList, scoreType, rankerType, page, msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.IsNext(), msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.IsPrev(), true);
				return true;
			}
		}
		return false;
	}

	public bool GetRankingScroll(RankingUtil.RankingMode rankingMode, bool isNext, RankingManager.CallbackRankingData callback)
	{
		if (!this.m_isRankingInit)
		{
			return false;
		}
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		this.m_isReset = false;
		bool result = false;
		if (!this.isLoading && this.m_page > 0)
		{
			RankingUtil.RankingScoreType scoreType = this.m_scoreType;
			RankingUtil.RankingRankerType rankerType = this.m_rankerType;
			if (rankingDataContainer != null)
			{
				bool flag = false;
				int num = 1;
				int num2 = 70;
				int rankingType = RankingUtil.GetRankingType(scoreType, rankerType);
				Dictionary<RankingUtil.RankingScoreType, List<MsgGetLeaderboardEntriesSucceed>> dictionary;
				rankingDataContainer.IsRankerType(rankerType, out dictionary);
				if (dictionary != null && dictionary.Count > 0 && dictionary.ContainsKey(scoreType) && dictionary[scoreType].Count > 1)
				{
					MsgGetLeaderboardEntriesSucceed msgGetLeaderboardEntriesSucceed = dictionary[scoreType][1];
					if (msgGetLeaderboardEntriesSucceed != null && msgGetLeaderboardEntriesSucceed.m_leaderboardEntries != null)
					{
						if (isNext)
						{
							flag = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.IsNext();
							msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.GetNextRanking(ref num, ref num2, 20);
						}
						else
						{
							flag = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.IsPrev();
							msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.GetPrevRanking(ref num, ref num2, 20);
						}
						if (flag)
						{
							if (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count > 30000)
							{
								return false;
							}
							if (num + num2 > 30000)
							{
								int num3 = num + num2 - 30000;
								num2 = num2 - num3 + 2;
							}
						}
					}
				}
				if (flag)
				{
					if (callback != null)
					{
						RankingManager.CallbackData item = new RankingManager.CallbackData(callback, rankingType, 2);
						this.m_callbacks.Add(item);
						if (this.m_callbacks.Count > 256)
						{
							this.m_callbacks.RemoveAt(0);
						}
					}
					result = true;
					this.m_isLoading = true;
					ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
					string[] friendIdList = RankingUtil.GetFriendIdList();
					if (loggedInServerInterface != null)
					{
						global::Debug.Log(string.Concat(new object[]
						{
							"RankingManager:RequestServerGetLeaderboardEntries   rankTop:",
							num,
							"  rankSize:",
							num2,
							"  type:",
							rankingType,
							" eventId:",
							this.m_eventId
						}));
						loggedInServerInterface.RequestServerGetLeaderboardEntries((int)rankingMode, num, num2, 2, rankingType, this.m_eventId, friendIdList, base.gameObject);
					}
				}
			}
		}
		return result;
	}

	public static void SavePlayerRankingData(RankingUtil.RankingMode rankingMode)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			SingletonGameObject<RankingManager>.Instance.SavePlayerRankingDataOrg(rankingMode);
		}
	}

	private void SavePlayerRankingDataOrg(RankingUtil.RankingMode rankingMode)
	{
		if (this.m_rankingDataSet != null)
		{
			Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>.KeyCollection keys = this.m_rankingDataSet.Keys;
			foreach (RankingUtil.RankingMode current in keys)
			{
				if (current == rankingMode)
				{
					this.m_rankingDataSet[current].SaveRanking();
				}
				else
				{
					this.m_rankingDataSet[current].Reset();
				}
			}
		}
	}

	public void SavePlayerRankingDataDummy(RankingUtil.RankingMode rankingMode, RankingUtil.RankingRankerType rankType, RankingUtil.RankingScoreType scoreType, int dammyRank)
	{
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer != null)
		{
			rankingDataContainer.SavePlayerRankingDummy(rankType, scoreType, dammyRank);
		}
	}

	public TimeSpan GetRankigResetTimeSpan(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
	{
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer == null)
		{
			return NetUtil.GetCurrentTime().AddMinutes(1.0) - NetUtil.GetCurrentTime();
		}
		return rankingDataContainer.GetResetTimeSpan(rankerType, scoreType);
	}

	public static int GetRankingMax(RankingUtil.RankingRankerType rankerType, int page = 0)
	{
		return RankingManager.GetRankingSize(rankerType, 1, page) - 1;
	}

	public static RankingUtil.Ranker GetMyRank(RankingUtil.RankingMode rankingMode, RankingUtil.RankingRankerType rankType, RankingUtil.RankingScoreType scoreType)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			return SingletonGameObject<RankingManager>.Instance.GetMyRankOrg(rankingMode, rankType, scoreType);
		}
		return null;
	}

	private RankingUtil.Ranker GetMyRankOrg(RankingUtil.RankingMode rankingMode, RankingUtil.RankingRankerType rankType, RankingUtil.RankingScoreType scoreType)
	{
		RankingUtil.Ranker result = null;
		List<RankingUtil.Ranker> rankerList = this.GetRankerList(rankingMode, scoreType, rankType, 0);
		if (rankerList != null && rankerList.Count > 0)
		{
			RankingUtil.Ranker ranker = rankerList[0];
			if (ranker != null)
			{
				result = ranker;
			}
		}
		return result;
	}

	public static long GetMyHiScore(RankingUtil.RankingMode rankingMode, bool isEvent)
	{
		long result = 0L;
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			RankingUtil.RankingRankerType rankType;
			RankingUtil.RankingScoreType scoreType;
			if (isEvent)
			{
				rankType = RankingUtil.RankingRankerType.SP_ALL;
				scoreType = RankingManager.EndlessSpecialRankingScoreType;
			}
			else
			{
				rankType = RankingUtil.RankingRankerType.RIVAL;
				scoreType = RankingManager.EndlessRivalRankingScoreType;
			}
			RankingUtil.Ranker myRankOrg = SingletonGameObject<RankingManager>.Instance.GetMyRankOrg(rankingMode, rankType, scoreType);
			if (myRankOrg != null)
			{
				result = myRankOrg.hiScore;
			}
		}
		return result;
	}

	public static RankingUtil.RankingScoreType GetCurrentRankingScoreType(RankingUtil.RankingMode rankingMode, bool isEvent)
	{
		RankingUtil.RankingScoreType result = RankingUtil.RankingScoreType.HIGH_SCORE;
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			RankingUtil.RankingDataSet rankingDataSet = SingletonGameObject<RankingManager>.Instance.GetRankingDataSet(rankingMode);
			if (rankingDataSet != null)
			{
				if (isEvent)
				{
					result = rankingDataSet.targetSpScoreType;
				}
				else
				{
					result = rankingDataSet.targetRivalScoreType;
				}
			}
		}
		return result;
	}

	public static int GetCurrentMyLeagueMax(RankingUtil.RankingMode rankingMode)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			return SingletonGameObject<RankingManager>.Instance.GetCurrentMyLeagueMaxOrg(rankingMode);
		}
		return 0;
	}

	private int GetCurrentMyLeagueMaxOrg(RankingUtil.RankingMode rankingMode)
	{
		int result = 0;
		List<RankingUtil.Ranker> rankerList = this.GetRankerList(rankingMode, RankingManager.EndlessRivalRankingScoreType, RankingUtil.RankingRankerType.RIVAL, 0);
		if (rankerList != null)
		{
			result = rankerList.Count - 1;
		}
		return result;
	}

	public static bool GetCurrentRankingStatus(RankingUtil.RankingMode rankingMode, bool isEvent, out long myScore, out long myHiScore, out int myLeague)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			return SingletonGameObject<RankingManager>.Instance.GetCurrentRankingStatusOrg(rankingMode, isEvent, out myScore, out myHiScore, out myLeague);
		}
		myScore = 0L;
		myLeague = 0;
		myHiScore = 0L;
		return false;
	}

	private bool GetCurrentRankingStatusOrg(RankingUtil.RankingMode rankingMode, bool isEvent, out long myScore, out long myHiScore, out int myLeague)
	{
		bool result = false;
		myScore = 0L;
		myHiScore = 0L;
		myLeague = 0;
		List<RankingUtil.Ranker> rankerList;
		if (isEvent)
		{
			rankerList = this.GetRankerList(rankingMode, RankingManager.EndlessSpecialRankingScoreType, RankingUtil.RankingRankerType.SP_ALL, 0);
		}
		else
		{
			rankerList = this.GetRankerList(rankingMode, RankingManager.EndlessRivalRankingScoreType, RankingUtil.RankingRankerType.RIVAL, 0);
		}
		if (rankerList != null && rankerList.Count > 0)
		{
			RankingUtil.Ranker ranker = rankerList[0];
			if (ranker != null)
			{
				myScore = ranker.score;
				myHiScore = ranker.hiScore;
				myLeague = ranker.leagueIndex;
				result = true;
			}
		}
		return result;
	}

	public static int GetCurrentHighScoreRank(RankingUtil.RankingMode rankingMode, bool isEvent, ref long currentScore, out bool isHighScore, out long nextRankScore, out long prveRankScore, out int nextRank)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			return SingletonGameObject<RankingManager>.Instance.GetCurrentHighScoreRankOrg(rankingMode, isEvent, ref currentScore, out isHighScore, out nextRankScore, out prveRankScore, out nextRank);
		}
		isHighScore = false;
		nextRankScore = 0L;
		prveRankScore = 0L;
		nextRank = -1;
		return -1;
	}

	private int GetCurrentHighScoreRankOrg(RankingUtil.RankingMode rankingMode, bool isEvent, ref long currentScore, out bool isHighScore, out long nextRankScore, out long prveRankScore, out int nextRank)
	{
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer != null)
		{
			return rankingDataContainer.GetCurrentHighScoreRank(isEvent, ref currentScore, out isHighScore, out nextRankScore, out prveRankScore, out nextRank);
		}
		isHighScore = false;
		nextRankScore = 0L;
		prveRankScore = 0L;
		nextRank = -1;
		return -1;
	}

	public static bool IsRankingInAggregate(RankingUtil.RankingMode rankingMode, RankingUtil.RankingRankerType rankType, RankingUtil.RankingScoreType scoreType)
	{
		bool result = false;
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			result = SingletonGameObject<RankingManager>.Instance.IsRankingInAggregateOrg(rankingMode, rankType, scoreType);
		}
		return result;
	}

	private bool IsRankingInAggregateOrg(RankingUtil.RankingMode rankingMode, RankingUtil.RankingRankerType rankType, RankingUtil.RankingScoreType scoreType)
	{
		bool result = false;
		if (this.GetRankigResetTimeSpan(rankingMode, scoreType, rankType).Ticks <= 0L)
		{
			result = true;
		}
		return result;
	}

	public static void UpdateSendChallenge(string id)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			SingletonGameObject<RankingManager>.Instance.UpdateSendChallengeOrg(id);
		}
	}

	private void UpdateSendChallengeOrg(string id)
	{
		EventManager.EventType type = EventManager.Instance.Type;
		bool flag = this.UpdateSendChallengeRankingList(RankingUtil.RankingRankerType.RIVAL, id);
		if (flag)
		{
			RankingUI.UpdateSendChallenge(RankingUtil.RankingRankerType.RIVAL, id);
		}
		flag = this.UpdateSendChallengeRankingList(RankingUtil.RankingRankerType.FRIEND, id);
		if (flag)
		{
			RankingUI.UpdateSendChallenge(RankingUtil.RankingRankerType.FRIEND, id);
		}
		flag = this.UpdateSendChallengeRankingList(RankingUtil.RankingRankerType.ALL, id);
		if (flag)
		{
			RankingUI.UpdateSendChallenge(RankingUtil.RankingRankerType.ALL, id);
		}
		flag = this.UpdateSendChallengeRankingList(RankingUtil.RankingRankerType.HISTORY, id);
		if (flag)
		{
			RankingUI.UpdateSendChallenge(RankingUtil.RankingRankerType.HISTORY, id);
		}
		if (type == EventManager.EventType.SPECIAL_STAGE)
		{
			flag = this.UpdateSendChallengeRankingList(RankingUtil.RankingRankerType.SP_FRIEND, id);
			if (flag)
			{
				SpecialStageWindow.UpdateSendChallenge(RankingUtil.RankingRankerType.SP_FRIEND, id);
			}
			flag = this.UpdateSendChallengeRankingList(RankingUtil.RankingRankerType.SP_ALL, id);
			if (flag)
			{
				SpecialStageWindow.UpdateSendChallenge(RankingUtil.RankingRankerType.SP_ALL, id);
			}
		}
	}

	private bool UpdateSendChallengeRankingList(RankingUtil.RankingRankerType type, string id)
	{
		bool result = false;
		if (this.m_rankingDataSet != null)
		{
			Dictionary<RankingUtil.RankingMode, RankingUtil.RankingDataSet>.KeyCollection keys = this.m_rankingDataSet.Keys;
			foreach (RankingUtil.RankingMode current in keys)
			{
				if (this.m_rankingDataSet[current].UpdateSendChallengeList(type, id))
				{
					result = true;
				}
			}
		}
		return result;
	}

	private void DefaultCallback(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		RankingUI.Setup();
	}

	private void EventRankingInitCallback(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		global::Debug.Log(string.Concat(new object[]
		{
			" ! RankingManager:NullCallback   type:",
			type,
			"  page:",
			page,
			"  isNext:",
			isNext,
			"  isPrev:",
			isPrev,
			"  num:",
			rankerList.Count
		}));
		this.m_isSpRankingInit = true;
		SpecialStageWindow.RankingSetup(false);
	}

	public void EventRankingSameRankCallback(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		global::Debug.Log(string.Concat(new object[]
		{
			" ! RankingManager:EventRankingSameRankCallback   type:",
			type,
			"  page:",
			page,
			"  isNext:",
			isNext,
			"  isPrev:",
			isPrev,
			"  num:",
			rankerList.Count
		}));
	}

	public void RankingSameRankCallback(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		global::Debug.Log(string.Concat(new object[]
		{
			" ! RankingManager:RankingSameRankCallback   type:",
			type,
			"  page:",
			page,
			"  isNext:",
			isNext,
			"  isPrev:",
			isPrev,
			"  num:",
			rankerList.Count
		}));
	}

	private void ServerGetLeaderboardEntries_Succeeded(MsgGetLeaderboardEntriesSucceed msg)
	{
		ServerLeaderboardEntries leaderboardEntries = msg.m_leaderboardEntries;
		int num = msg.m_leaderboardEntries.m_mode;
		int rankerPage = this.GetRankerPage(msg);
		int rankingType = leaderboardEntries.m_rankingType;
		if (num < 0 || num >= 2)
		{
			num = 0;
		}
		RankingUtil.RankingMode rankingMode = (RankingUtil.RankingMode)num;
		global::Debug.Log(string.Concat(new object[]
		{
			" RankingManager:ServerGetLeaderboardEntries_Succeeded mode:",
			rankingMode,
			" Count:",
			msg.m_leaderboardEntries.m_leaderboardEntries.Count
		}));
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.ContainsKey(rankingMode))
		{
			this.m_rankingDataSet[rankingMode].AddRankerList(msg);
			if (RankingUtil.IsRankingUserFrontAndBack(this.scoreType, this.rankerType, rankerPage))
			{
				RankingManager.CallbackRankingData callback;
				if (this.rankerType == RankingUtil.RankingRankerType.SP_ALL || this.rankerType == RankingUtil.RankingRankerType.SP_FRIEND)
				{
					callback = new RankingManager.CallbackRankingData(this.EventRankingSameRankCallback);
				}
				else
				{
					callback = new RankingManager.CallbackRankingData(this.RankingSameRankCallback);
				}
				this.GetRanking(rankingMode, this.scoreType, this.rankerType, 3, callback);
			}
			if (this.m_callbacks == null)
			{
				this.m_callbacks = new List<RankingManager.CallbackData>();
			}
			if (this.m_callbacks.Count > 0)
			{
				RankingManager.CallbackData callbackData = null;
				for (int i = 0; i < this.m_callbacks.Count; i++)
				{
					if (this.m_callbacks[i].Check(rankingType, rankerPage))
					{
						callbackData = this.m_callbacks[i];
						break;
					}
				}
				if (callbackData != null && callbackData.callback != null)
				{
					leaderboardEntries = msg.m_leaderboardEntries;
					List<RankingUtil.Ranker> rankerList;
					if (this.rankerType == RankingUtil.RankingRankerType.RIVAL || rankerPage == 0)
					{
						rankerList = this.m_rankingDataSet[rankingMode].GetRankerList(this.rankerType, this.scoreType, 0);
					}
					else
					{
						rankerList = this.m_rankingDataSet[rankingMode].GetRankerList(this.rankerType, this.scoreType, 1);
					}
					callbackData.callback(rankerList, this.scoreType, this.rankerType, rankerPage, leaderboardEntries.IsNext(), leaderboardEntries.IsPrev(), false);
					this.m_callbacks.Remove(callbackData);
				}
			}
			global::Debug.Log(" RankingManager:ServerGetLeaderboardEntries_Succeeded  chainGetRankingCodeList:" + this.m_chainGetRankingCodeList.Count);
			if (this.m_chainGetRankingCodeList != null && this.m_chainGetRankingCodeList.Count > 0)
			{
				RankingUtil.RankingScoreType rankerScoreType = RankingUtil.GetRankerScoreType(rankingType);
				RankingUtil.RankingRankerType rankerType = RankingUtil.GetRankerType(rankingType);
				int rankingCode = RankingUtil.GetRankingCode(rankingMode, rankerScoreType, rankerType);
				if (this.m_chainGetRankingCodeList.Contains(rankingCode))
				{
					this.m_chainGetRankingCodeList.Remove(rankingCode);
					if (this.m_chainGetRankingCodeList.Count > 0)
					{
						this.m_isLoading = false;
						int rankingType2 = this.m_chainGetRankingCodeList[0];
						RankingUtil.RankingMode rankerMode = RankingUtil.GetRankerMode(rankingType2);
						RankingUtil.RankingScoreType rankerScoreType2 = RankingUtil.GetRankerScoreType(rankingType2);
						RankingUtil.RankingRankerType rankerType2 = RankingUtil.GetRankerType(rankingType2);
						this.GetRanking(rankerMode, rankerScoreType2, rankerType2, 0, new RankingManager.CallbackRankingData(this.DefaultCallback));
					}
					else
					{
						global::Debug.Log(" RankingManager:ServerGetLeaderboardEntries_Succeeded  chain end " + (this.m_callbackBakNormalAll != null));
						if (this.m_callbackBakNormalAll != null)
						{
							List<RankingUtil.Ranker> rankerList2;
							if (this.rankerType == RankingUtil.RankingRankerType.RIVAL || rankerPage == 0)
							{
								rankerList2 = this.m_rankingDataSet[rankingMode].GetRankerList(this.rankerType, this.scoreType, 0);
							}
							else
							{
								rankerList2 = this.m_rankingDataSet[rankingMode].GetRankerList(this.rankerType, this.scoreType, 1);
							}
							this.m_callbackBakNormalAll(rankerList2, this.scoreType, this.rankerType, rankerPage, leaderboardEntries.IsNext(), leaderboardEntries.IsPrev(), false);
							this.m_callbackBakNormalAll = null;
						}
						this.m_chainGetRankingCodeList.Clear();
					}
				}
			}
		}
		this.m_getRankingLastTime = Time.realtimeSinceStartup;
		this.m_isLoading = false;
	}

	private void ServerGetLeaderboardEntries_Failed()
	{
		global::Debug.Log(" RankingManager:ServerGetLeaderboardEntries_Failed()");
		this.m_getRankingLastTime = Time.realtimeSinceStartup;
		this.m_isLoading = false;
	}

	private List<RankingUtil.Ranker> GetRankerList(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, int page = 0)
	{
		List<RankingUtil.Ranker> result = null;
		if (rankerType == RankingUtil.RankingRankerType.RIVAL)
		{
			page = 0;
		}
		if (this.m_rankingDataSet != null && this.m_rankingDataSet.ContainsKey(rankingMode))
		{
			result = this.m_rankingDataSet[rankingMode].GetRankerList(rankerType, scoreType, page);
		}
		return result;
	}

	private int GetRankerPage(MsgGetLeaderboardEntriesSucceed msg)
	{
		int result = 0;
		if (msg != null && msg.m_leaderboardEntries != null)
		{
			result = msg.m_leaderboardEntries.m_index;
			if (msg.m_leaderboardEntries.IsRivalRanking())
			{
				result = 0;
			}
		}
		return result;
	}

	private int GetRankingTop(RankingUtil.RankingMode rankingMode, RankingUtil.RankingRankerType rankerType, RankingUtil.RankingScoreType scoreType, int page = 0)
	{
		int num = 1;
		if (page >= 3)
		{
			RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
			if (rankingDataContainer != null)
			{
				Dictionary<RankingUtil.RankingScoreType, List<MsgGetLeaderboardEntriesSucceed>> dictionary;
				rankingDataContainer.IsRankerType(rankerType, out dictionary);
				if (dictionary != null && dictionary.ContainsKey(scoreType))
				{
					List<MsgGetLeaderboardEntriesSucceed> list = dictionary[scoreType];
					if (list != null && list.Count > 0)
					{
						ServerLeaderboardEntry serverLeaderboardEntry = null;
						if (list[0] != null)
						{
							serverLeaderboardEntry = list[0].m_leaderboardEntries.m_myLeaderboardEntry;
						}
						else if (list.Count > 1 && list[1] != null)
						{
							serverLeaderboardEntry = list[1].m_leaderboardEntries.m_myLeaderboardEntry;
						}
						if (serverLeaderboardEntry != null)
						{
							num = serverLeaderboardEntry.m_grade - 50;
							if (num < 1)
							{
								num = 1;
							}
						}
					}
				}
			}
		}
		return num;
	}

	private static int GetRankingSize(RankingUtil.RankingRankerType rankerType, int top, int page)
	{
		if (rankerType == RankingUtil.RankingRankerType.COUNT || page < 0)
		{
			return -1;
		}
		int result = 0;
		switch (rankerType)
		{
		case RankingUtil.RankingRankerType.FRIEND:
		case RankingUtil.RankingRankerType.ALL:
		case RankingUtil.RankingRankerType.HISTORY:
		case RankingUtil.RankingRankerType.SP_ALL:
		case RankingUtil.RankingRankerType.SP_FRIEND:
			result = 4;
			break;
		case RankingUtil.RankingRankerType.RIVAL:
			result = 71;
			break;
		}
		if (page > 0)
		{
			result = 71;
			if (page >= 3)
			{
				result = 100;
			}
		}
		return result;
	}

	public bool IsRankingTop(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
	{
		bool result = false;
		if (scoreType == RankingUtil.RankingScoreType.NONE || rankerType == RankingUtil.RankingRankerType.COUNT)
		{
			return false;
		}
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer != null)
		{
			result = rankingDataContainer.IsRankerAndScoreType(rankerType, scoreType, -1);
		}
		return result;
	}

	private bool IsRankingList(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, int page = 0)
	{
		bool result = false;
		if (scoreType == RankingUtil.RankingScoreType.NONE || rankerType == RankingUtil.RankingRankerType.COUNT || page < 0)
		{
			return false;
		}
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer != null)
		{
			result = rankingDataContainer.IsRankerAndScoreType(rankerType, scoreType, page);
		}
		return result;
	}

	public List<RankingUtil.Ranker> GetCacheRankingList(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
	{
		List<RankingUtil.Ranker> list = null;
		RankingDataContainer rankingDataContainer = this.GetRankingDataContainer(rankingMode);
		if (rankingDataContainer != null)
		{
			if (rankerType == RankingUtil.RankingRankerType.RIVAL)
			{
				list = rankingDataContainer.GetRankerList(rankerType, scoreType, 0);
			}
			else
			{
				list = rankingDataContainer.GetRankerList(rankerType, scoreType, 1);
				List<RankingUtil.Ranker> rankerList = rankingDataContainer.GetRankerList(rankerType, scoreType, 2);
				if (list != null && rankerList != null && rankerList.Count > 1)
				{
					for (int i = 1; i < rankerList.Count; i++)
					{
						list.Add(rankerList[i]);
					}
				}
			}
		}
		return list;
	}

	public Texture GetChaoTexture(int chaoId, UITexture chaoTexture, float delay = 0.2f, bool isDefaultTexture = false)
	{
		Texture result = null;
		if (chaoTexture == null)
		{
			return null;
		}
		if (this.m_chaoTextureList != null && this.m_chaoTextureList.ContainsKey(chaoId))
		{
			result = this.m_chaoTextureList[chaoId];
			chaoTexture.mainTexture = this.m_chaoTextureList[chaoId];
			chaoTexture.alpha = 1f;
		}
		else
		{
			Texture loadedTexture = ChaoTextureManager.Instance.GetLoadedTexture(chaoId);
			if (loadedTexture != null)
			{
				if (this.m_chaoTextureList == null)
				{
					this.m_chaoTextureList = new Dictionary<int, Texture>();
				}
				this.m_chaoTextureList.Add(chaoId, loadedTexture);
				result = this.m_chaoTextureList[chaoId];
				chaoTexture.mainTexture = this.m_chaoTextureList[chaoId];
				chaoTexture.alpha = 1f;
			}
			else
			{
				if (isDefaultTexture || chaoTexture.alpha > 0f)
				{
					chaoTexture.mainTexture = ChaoTextureManager.Instance.m_defaultTexture;
				}
				if (this.m_chaoTextureLoad == null)
				{
					this.m_chaoTextureLoad = new Dictionary<int, float>();
				}
				if (this.m_chaoTextureList == null)
				{
					this.m_chaoTextureList = new Dictionary<int, Texture>();
				}
				if (this.m_chaoTextureObject == null)
				{
					this.m_chaoTextureObject = new Dictionary<int, List<UITexture>>();
				}
				if (!this.m_chaoTextureObject.ContainsKey(chaoId))
				{
					List<UITexture> value = new List<UITexture>();
					this.m_chaoTextureObject.Add(chaoId, value);
				}
				this.m_chaoTextureObject[chaoId].Add(chaoTexture);
				if (this.m_chaoTextureLoad.Count <= 0)
				{
					ChaoTextureManager.CallbackInfo.LoadFinishCallback callback = new ChaoTextureManager.CallbackInfo.LoadFinishCallback(this.ChaoLoadFinishCallback);
					ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(null, callback, false);
					ChaoTextureManager.Instance.GetTexture(chaoId, info);
					this.m_chaoTextureLoadTime = 0f;
				}
				else if (!this.m_chaoTextureLoad.ContainsKey(chaoId))
				{
					this.m_chaoTextureLoad.Add(chaoId, delay);
				}
				else if (delay > 0f && this.m_chaoTextureLoad[chaoId] < delay * 0.15f)
				{
					this.m_chaoTextureLoad[chaoId] = delay * 0.15f;
				}
				this.m_chaoTextureLoadEndTime = 0f;
			}
		}
		return result;
	}

	public void ResetChaoTexture()
	{
		if (this.m_chaoTextureLoad != null)
		{
			this.m_chaoTextureLoad.Clear();
		}
		if (this.m_chaoTextureList != null)
		{
			this.m_chaoTextureList.Clear();
		}
		if (this.m_chaoTextureObject != null)
		{
			this.m_chaoTextureObject.Clear();
		}
		this.m_chaoTextureLoadTime = -1f;
		this.m_chaoTextureLoadEndTime = 0f;
		this.m_chaoTextureLoad = new Dictionary<int, float>();
		this.m_chaoTextureList = new Dictionary<int, Texture>();
		this.m_chaoTextureObject = new Dictionary<int, List<UITexture>>();
	}

	private void ChaoLoadFinishCallback(Texture tex)
	{
		if (tex == null)
		{
			return;
		}
		string[] array = tex.name.Split(new char[]
		{
			'_'
		});
		int num = int.Parse(array[array.Length - 1]);
		if (this.m_chaoTextureObject != null && this.m_chaoTextureLoad != null && this.m_chaoTextureObject.ContainsKey(num))
		{
			bool flag = true;
			if (this.m_chaoTextureLoad.ContainsKey(num))
			{
				if (this.m_chaoTextureLoad[num] > 0f)
				{
					flag = false;
				}
				else
				{
					this.m_chaoTextureLoad.Remove(num);
				}
			}
			if (flag)
			{
				List<UITexture> list = this.m_chaoTextureObject[num];
				if (list != null && list.Count > 0)
				{
					foreach (UITexture current in list)
					{
						if (current != null)
						{
							current.mainTexture = tex;
							current.alpha = 1f;
						}
					}
				}
				this.m_chaoTextureObject.Remove(num);
			}
		}
		if (num >= 0 && this.m_chaoTextureList != null && !this.m_chaoTextureList.ContainsKey(num))
		{
			this.m_chaoTextureList.Add(num, tex);
		}
		int num2 = -1;
		if (this.m_chaoTextureLoad != null && this.m_chaoTextureLoad.Count > 0)
		{
			int[] array2 = new int[this.m_chaoTextureLoad.Count];
			this.m_chaoTextureLoad.Keys.CopyTo(array2, 0);
			int[] array3 = array2;
			for (int i = 0; i < array3.Length; i++)
			{
				int num3 = array3[i];
				if (!this.m_chaoTextureList.ContainsKey(num3))
				{
					num2 = num3;
					break;
				}
			}
			if (num2 >= 0)
			{
				ChaoTextureManager.CallbackInfo.LoadFinishCallback callback = new ChaoTextureManager.CallbackInfo.LoadFinishCallback(this.ChaoLoadFinishCallback);
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(null, callback, false);
				ChaoTextureManager.Instance.GetTexture(num, info);
			}
		}
	}
}
