using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DailyBattleManager : SingletonGameObject<DailyBattleManager>
{
	public enum REQ_TYPE
	{
		GET_STATUS,
		UPD_STATUS,
		POST_RESULT,
		GET_PRIZE,
		GET_DATA,
		GET_DATA_HISTORY,
		RESET_MATCHING,
		NUM
	}

	public enum DATA_TYPE
	{
		STATUS,
		DATA_PAIR,
		DATA_PAIR_LIST,
		PRIZE_LIST,
		END_TIME,
		REWARD_FLAG,
		REWARD_DATA_PAIR,
		NUM
	}

	public delegate void CallbackGetStatus(ServerDailyBattleStatus status, DateTime endTime, bool rewardFlag, ServerDailyBattleDataPair rewardDataPair);

	public delegate void CallbackPostResult(ServerDailyBattleDataPair dataPair, bool rewardFlag, ServerDailyBattleDataPair rewardDataPair);

	public delegate void CallbackGetData(ServerDailyBattleDataPair dataPair);

	public delegate void CallbackGetDataHistory(List<ServerDailyBattleDataPair> dataPairList);

	public delegate void CallbackGetPrize(List<ServerDailyBattlePrizeData> prizeList);

	public delegate void CallbackResetMatching(ServerPlayerState playerStatus, ServerDailyBattleDataPair dataPair, DateTime endTime);

	public delegate void CallbackSetupInfo(ServerDailyBattleStatus status, ServerDailyBattleDataPair dataPair, DateTime endTime, bool rewardFlag, int winFlag);

	public delegate void CallbackSetup();

	private const int REQUEST_LOAD_DELAY = 2;

	private const int REQUEST_RELOAD_DELAY = 60;

	private const int DATA_AUTO_RELOAD_TIME = 600;

	[SerializeField]
	private bool m_showLog;

	private DailyBattleManager.CallbackGetStatus m_callbackGetStatus;

	private DailyBattleManager.CallbackPostResult m_callbackPostResult;

	private DailyBattleManager.CallbackGetData m_callbackGetData;

	private DailyBattleManager.CallbackGetDataHistory m_callbackGetDataHistory;

	private DailyBattleManager.CallbackGetPrize m_callbackGetPrize;

	private DailyBattleManager.CallbackResetMatching m_callbackResetMatching;

	private DailyBattleManager.CallbackSetupInfo m_callbackSetupInfo;

	private DailyBattleManager.CallbackSetup m_callbackSetup;

	private Dictionary<DailyBattleManager.REQ_TYPE, bool> m_isRequestList;

	private Dictionary<DailyBattleManager.REQ_TYPE, DateTime> m_requestTimeList;

	private bool m_isDataInit;

	private bool m_isFirstSetupReq;

	private bool m_isResultSetupReq;

	private bool m_isDispUpdate;

	private DateTime m_dataInitTime;

	private ServerDailyBattleStatus m_currentStatus;

	private ServerDailyBattleDataPair m_currentDataPair;

	private List<ServerDailyBattleDataPair> m_currentDataPairList;

	private List<ServerDailyBattlePrizeData> m_currentPrizeList;

	private DateTime m_currentEndTime;

	private bool m_currentRewardFlag;

	private ServerDailyBattleDataPair m_currentRewardDataPair;

	private Dictionary<int, ServerConsumedCostData> m_resetCostList;

	private Dictionary<DailyBattleManager.REQ_TYPE, int> m_chainRequestList;

	private List<DailyBattleManager.REQ_TYPE> m_chainRequestKeys;

	private Dictionary<DailyBattleManager.DATA_TYPE, DateTime> m_currentDataLastUpdateTimeList;

	public ServerDailyBattleStatus currentStatus
	{
		get
		{
			return this.m_currentStatus;
		}
	}

	public ServerDailyBattleDataPair currentDataPair
	{
		get
		{
			return this.m_currentDataPair;
		}
	}

	public List<ServerDailyBattleDataPair> currentDataPairList
	{
		get
		{
			return this.m_currentDataPairList;
		}
	}

	public List<ServerDailyBattlePrizeData> currentPrizeList
	{
		get
		{
			return this.m_currentPrizeList;
		}
	}

	public DateTime currentEndTime
	{
		get
		{
			return this.m_currentEndTime;
		}
	}

	public bool currentRewardFlag
	{
		get
		{
			return this.m_currentRewardFlag;
		}
	}

	public int currentWinFlag
	{
		get
		{
			int result = 0;
			if (this.m_currentDataPair != null && this.m_currentDataPair.myBattleData != null && !string.IsNullOrEmpty(this.m_currentDataPair.myBattleData.userId))
			{
				if (this.m_currentDataPair.rivalBattleData != null && !string.IsNullOrEmpty(this.m_currentDataPair.rivalBattleData.userId))
				{
					if (this.m_currentDataPair.myBattleData.maxScore > this.m_currentDataPair.rivalBattleData.maxScore)
					{
						result = 3;
					}
					else if (this.m_currentDataPair.myBattleData.maxScore == this.m_currentDataPair.rivalBattleData.maxScore)
					{
						result = 2;
					}
					else
					{
						result = 1;
					}
				}
				else
				{
					result = 4;
				}
			}
			return result;
		}
	}

	public Dictionary<int, ServerConsumedCostData> resetCostList
	{
		get
		{
			if (this.m_resetCostList == null)
			{
				List<ServerConsumedCostData> costList = ServerInterface.CostList;
				if (costList != null)
				{
					this.m_resetCostList = new Dictionary<int, ServerConsumedCostData>();
					foreach (ServerConsumedCostData current in costList)
					{
						switch (current.consumedItemId)
						{
						case 980000:
							if (!this.m_resetCostList.ContainsKey(0))
							{
								this.m_resetCostList.Add(0, current);
							}
							break;
						case 980001:
							if (!this.m_resetCostList.ContainsKey(1))
							{
								this.m_resetCostList.Add(1, current);
							}
							break;
						case 980002:
							if (!this.m_resetCostList.ContainsKey(2))
							{
								this.m_resetCostList.Add(2, current);
							}
							break;
						}
					}
				}
			}
			return this.m_resetCostList;
		}
	}

	public bool isLoading
	{
		get
		{
			return this.m_isFirstSetupReq || this.m_isResultSetupReq;
		}
	}

	public static bool isDailyBattleDispUpdateFlag
	{
		get
		{
			return SingletonGameObject<DailyBattleManager>.Instance != null && SingletonGameObject<DailyBattleManager>.Instance.isDispUpdateFlag;
		}
		set
		{
			if (SingletonGameObject<DailyBattleManager>.Instance != null)
			{
				SingletonGameObject<DailyBattleManager>.Instance.isDispUpdateFlag = value;
			}
		}
	}

	public bool isDispUpdateFlag
	{
		get
		{
			return this.m_isDispUpdate;
		}
		set
		{
			if (!value)
			{
				this.m_isDispUpdate = value;
			}
		}
	}

	public TimeSpan GetLimitTimeSpan()
	{
		return this.m_currentEndTime - NetBase.GetCurrentTime();
	}

	private void Init()
	{
		this.m_dataInitTime = NetBase.GetCurrentTime();
		this.m_isFirstSetupReq = false;
		this.m_isResultSetupReq = false;
		this.m_isDispUpdate = false;
		DateTime currentEndTime = this.m_dataInitTime.AddHours(1.0);
		this.m_currentStatus = null;
		this.m_currentDataPair = null;
		this.m_currentDataPairList = null;
		this.m_currentPrizeList = null;
		this.m_currentEndTime = currentEndTime;
		if (this.m_chainRequestList != null)
		{
			this.m_chainRequestList.Clear();
		}
		if (this.m_chainRequestKeys != null)
		{
			this.m_chainRequestKeys.Clear();
		}
		DateTime time = this.m_dataInitTime.AddSeconds(-606.0);
		for (int i = 0; i < 7; i++)
		{
			this.UpdateDataLastTime((DailyBattleManager.DATA_TYPE)i, time);
		}
		this.m_isDataInit = true;
	}

	public ServerDailyBattleDataPair GetRewardDataPair(bool reset = false)
	{
		ServerDailyBattleDataPair result = null;
		if (this.m_currentRewardFlag && this.m_currentRewardDataPair != null)
		{
			result = new ServerDailyBattleDataPair(this.m_currentRewardDataPair);
			if (reset)
			{
				this.m_currentRewardDataPair = null;
				this.m_currentRewardFlag = false;
			}
		}
		return result;
	}

	public bool RestReward()
	{
		bool result = false;
		if (this.m_currentRewardFlag && this.m_currentRewardDataPair != null)
		{
			result = true;
		}
		this.m_currentRewardDataPair = null;
		this.m_currentRewardFlag = false;
		return result;
	}

	public void FirstSetup(DailyBattleManager.CallbackSetupInfo callback)
	{
		this.Init();
		this.m_isFirstSetupReq = true;
		this.m_isDispUpdate = false;
		this.m_callbackSetup = null;
		this.m_callbackSetupInfo = callback;
		this.SetChainRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS, DailyBattleManager.REQ_TYPE.GET_DATA);
	}

	public void FirstSetup(DailyBattleManager.CallbackSetup callback)
	{
		this.Init();
		this.m_isFirstSetupReq = true;
		this.m_isDispUpdate = false;
		this.m_callbackSetup = callback;
		this.m_callbackSetupInfo = null;
		this.SetChainRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS, DailyBattleManager.REQ_TYPE.GET_DATA);
	}

	public void FirstSetup()
	{
		this.Init();
		this.m_isFirstSetupReq = true;
		this.m_isDispUpdate = false;
		this.m_callbackSetup = null;
		this.m_callbackSetupInfo = null;
		this.SetChainRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS, DailyBattleManager.REQ_TYPE.GET_DATA);
	}

	public void ResultSetup(DailyBattleManager.CallbackSetupInfo callback)
	{
		this.m_isResultSetupReq = true;
		this.m_isDispUpdate = false;
		this.m_callbackSetup = null;
		this.m_callbackSetupInfo = callback;
		this.SetChainRequest(DailyBattleManager.REQ_TYPE.POST_RESULT, DailyBattleManager.REQ_TYPE.GET_STATUS);
	}

	public void ResultSetup(DailyBattleManager.CallbackSetup callback)
	{
		this.m_isResultSetupReq = true;
		this.m_isDispUpdate = false;
		this.m_callbackSetup = callback;
		this.m_callbackSetupInfo = null;
		this.SetChainRequest(DailyBattleManager.REQ_TYPE.POST_RESULT, DailyBattleManager.REQ_TYPE.GET_STATUS);
	}

	public void ResultSetup()
	{
		this.m_isResultSetupReq = true;
		this.m_isDispUpdate = false;
		this.m_callbackSetup = null;
		this.m_callbackSetupInfo = null;
		this.SetChainRequest(DailyBattleManager.REQ_TYPE.POST_RESULT, DailyBattleManager.REQ_TYPE.GET_STATUS);
	}

	private bool CheckChainOrMultiRequestType(DailyBattleManager.REQ_TYPE reqType)
	{
		bool result = true;
		if (reqType == DailyBattleManager.REQ_TYPE.NUM || reqType == DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY || reqType == DailyBattleManager.REQ_TYPE.RESET_MATCHING)
		{
			result = false;
		}
		return result;
	}

	private bool IsChainRequest(DailyBattleManager.REQ_TYPE reqType)
	{
		bool result = false;
		if (this.m_chainRequestKeys != null && this.m_chainRequestList.Count > 0 && this.m_chainRequestList.ContainsKey(reqType) && this.m_chainRequestList[reqType] <= 1)
		{
			result = true;
		}
		return result;
	}

	private bool NextChainRequest()
	{
		bool flag = false;
		if (this.m_chainRequestKeys != null && this.m_chainRequestKeys.Count > 0)
		{
			int num = 0;
			for (int i = 0; i < this.m_chainRequestKeys.Count; i++)
			{
				DailyBattleManager.REQ_TYPE rEQ_TYPE = this.m_chainRequestKeys[i];
				int num2 = this.m_chainRequestList[rEQ_TYPE];
				if (num2 >= 2)
				{
					num++;
				}
				if (num2 <= 0)
				{
					this.m_chainRequestList[rEQ_TYPE] = 1;
					this.Request(rEQ_TYPE);
					flag = true;
					break;
				}
			}
			if (!flag && num >= this.m_chainRequestKeys.Count)
			{
				if (this.m_isFirstSetupReq)
				{
					global::Debug.Log(" FirstSetup end !!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
				if (this.m_isResultSetupReq)
				{
					global::Debug.Log(" ResultSetup end !!!!!!!!!!!!!!!!!!!!!!!!!!!");
					if (this.currentDataPair != null && this.currentDataPair.myBattleData != null && this.currentDataPair.rivalBattleData != null && !string.IsNullOrEmpty(this.currentDataPair.myBattleData.userId))
					{
						global::Debug.Log(" ResultSetup end   starTime:" + this.currentDataPair.starDateString + " endTime:" + this.currentDataPair.endDateString);
					}
				}
				this.m_isDispUpdate = true;
				this.m_isResultSetupReq = false;
				this.m_isFirstSetupReq = false;
				this.m_chainRequestList.Clear();
				this.m_chainRequestKeys.Clear();
				if (this.m_callbackSetupInfo != null)
				{
					this.m_callbackSetupInfo(this.currentStatus, this.currentDataPair, this.currentEndTime, this.currentRewardFlag, this.currentWinFlag);
				}
				if (this.m_callbackSetup != null)
				{
					this.m_callbackSetup();
				}
			}
		}
		return flag;
	}

	private bool SetChainRequest(DailyBattleManager.REQ_TYPE req0, DailyBattleManager.REQ_TYPE req1)
	{
		return this.SetChainRequest(new List<DailyBattleManager.REQ_TYPE>
		{
			req0,
			req1
		});
	}

	private bool SetChainRequest(DailyBattleManager.REQ_TYPE req0, DailyBattleManager.REQ_TYPE req1, DailyBattleManager.REQ_TYPE req2)
	{
		return this.SetChainRequest(new List<DailyBattleManager.REQ_TYPE>
		{
			req0,
			req1,
			req2
		});
	}

	private bool SetChainRequest(List<DailyBattleManager.REQ_TYPE> reqList)
	{
		if (reqList == null)
		{
			return false;
		}
		if (reqList.Count <= 0)
		{
			return false;
		}
		for (int i = 0; i < reqList.Count; i++)
		{
			if (!this.CheckChainOrMultiRequestType(reqList[i]))
			{
				return false;
			}
		}
		bool result = false;
		if (this.m_chainRequestList == null || this.m_chainRequestList.Count <= 0)
		{
			if (this.m_chainRequestList == null)
			{
				this.m_chainRequestList = new Dictionary<DailyBattleManager.REQ_TYPE, int>();
			}
			if (this.m_chainRequestKeys == null)
			{
				this.m_chainRequestKeys = new List<DailyBattleManager.REQ_TYPE>();
			}
			for (int j = 0; j < reqList.Count; j++)
			{
				this.m_chainRequestList.Add(reqList[j], 0);
				this.m_chainRequestKeys.Add(reqList[j]);
			}
			this.m_chainRequestList[this.m_chainRequestKeys[0]] = 1;
			this.Request(this.m_chainRequestKeys[0]);
			result = true;
		}
		return result;
	}

	private void SetCurrentStatus(ServerDailyBattleStatus status)
	{
		if (this.m_currentStatus == null)
		{
			this.m_currentStatus = new ServerDailyBattleStatus();
		}
		status.CopyTo(this.m_currentStatus);
		this.UpdateDataLastTime(DailyBattleManager.DATA_TYPE.STATUS, NetBase.GetCurrentTime());
	}

	private void SetCurrentDataPair(ServerDailyBattleDataPair data)
	{
		if (this.m_currentDataPair == null)
		{
			this.m_currentDataPair = new ServerDailyBattleDataPair();
		}
		data.CopyTo(this.m_currentDataPair);
		this.UpdateDataLastTime(DailyBattleManager.DATA_TYPE.DATA_PAIR, NetBase.GetCurrentTime());
	}

	private void SetCurrentDataPairList(List<ServerDailyBattleDataPair> list)
	{
		if (this.m_currentDataPairList == null)
		{
			this.m_currentDataPairList = new List<ServerDailyBattleDataPair>();
		}
		else
		{
			this.m_currentDataPairList.Clear();
		}
		if (list != null && list.Count > 0)
		{
			DateTime dateTime = NetBase.GetCurrentTime();
			TimeSpan t = list[0].endTime - list[0].starTime;
			for (int i = 0; i < list.Count; i++)
			{
				ServerDailyBattleDataPair serverDailyBattleDataPair = list[i];
				if (!serverDailyBattleDataPair.isToday)
				{
					ServerDailyBattleDataPair serverDailyBattleDataPair2 = new ServerDailyBattleDataPair();
					serverDailyBattleDataPair.CopyTo(serverDailyBattleDataPair2);
					if (dateTime.Ticks > serverDailyBattleDataPair2.endTime.Ticks && t.TotalSeconds > 0.0)
					{
						TimeSpan t2 = dateTime - serverDailyBattleDataPair2.endTime;
						if (t2 >= t)
						{
							global::Debug.Log(string.Concat(new object[]
							{
								string.Empty,
								i,
								" span:",
								t2.TotalHours,
								"h  currentEnd:",
								dateTime.ToString(),
								" end:",
								serverDailyBattleDataPair2.endTime
							}));
							ServerDailyBattleDataPair item = new ServerDailyBattleDataPair(serverDailyBattleDataPair2.endTime, dateTime);
							this.m_currentDataPairList.Add(item);
						}
					}
					this.m_currentDataPairList.Add(serverDailyBattleDataPair2);
				}
				dateTime = serverDailyBattleDataPair.starTime;
			}
		}
		this.UpdateDataLastTime(DailyBattleManager.DATA_TYPE.DATA_PAIR_LIST, NetBase.GetCurrentTime());
	}

	private void SetCurrentPrizeList(List<ServerDailyBattlePrizeData> list)
	{
		if (this.m_currentPrizeList == null)
		{
			this.m_currentPrizeList = new List<ServerDailyBattlePrizeData>();
		}
		else
		{
			this.m_currentPrizeList.Clear();
		}
		if (list != null && list.Count > 0)
		{
			foreach (ServerDailyBattlePrizeData current in list)
			{
				ServerDailyBattlePrizeData serverDailyBattlePrizeData = new ServerDailyBattlePrizeData();
				current.CopyTo(serverDailyBattlePrizeData);
				this.m_currentPrizeList.Add(serverDailyBattlePrizeData);
			}
		}
		this.UpdateDataLastTime(DailyBattleManager.DATA_TYPE.PRIZE_LIST, NetBase.GetCurrentTime());
	}

	private void SetCurrentEndTime(DateTime time)
	{
		this.m_currentEndTime = time;
		this.UpdateDataLastTime(DailyBattleManager.DATA_TYPE.END_TIME, NetBase.GetCurrentTime());
	}

	private void SetCurrentRewardFlag(bool flg)
	{
		if (flg || !this.m_currentRewardFlag)
		{
			this.m_currentRewardFlag = flg;
			this.UpdateDataLastTime(DailyBattleManager.DATA_TYPE.REWARD_FLAG, NetBase.GetCurrentTime());
		}
	}

	private void SetCurrentRewardDataPair(ServerDailyBattleDataPair rewardDataPair)
	{
		if (rewardDataPair != null || this.m_currentRewardDataPair == null)
		{
			this.m_currentRewardDataPair = rewardDataPair;
			this.UpdateDataLastTime(DailyBattleManager.DATA_TYPE.REWARD_DATA_PAIR, NetBase.GetCurrentTime());
		}
	}

	private void Update()
	{
		if (!this.m_isDataInit)
		{
			this.Init();
		}
	}

	private void UpdateDataLastTime(DailyBattleManager.DATA_TYPE type, DateTime time)
	{
		if (this.m_currentDataLastUpdateTimeList == null)
		{
			this.m_currentDataLastUpdateTimeList = new Dictionary<DailyBattleManager.DATA_TYPE, DateTime>();
		}
		if (this.m_currentDataLastUpdateTimeList.ContainsKey(type))
		{
			this.m_currentDataLastUpdateTimeList[type] = time;
		}
		else
		{
			this.m_currentDataLastUpdateTimeList.Add(type, time);
		}
	}

	public bool IsExpirationData(DailyBattleManager.DATA_TYPE type)
	{
		bool result = false;
		if (this.m_currentDataLastUpdateTimeList == null || type == DailyBattleManager.DATA_TYPE.NUM)
		{
			return false;
		}
		if (this.m_currentDataLastUpdateTimeList.ContainsKey(type) && !GeneralUtil.IsOverTimeMinute(this.m_currentDataLastUpdateTimeList[type], 3))
		{
			result = true;
		}
		return result;
	}

	private void StartRequest(DailyBattleManager.REQ_TYPE type)
	{
		if (this.m_isRequestList == null)
		{
			this.m_isRequestList = new Dictionary<DailyBattleManager.REQ_TYPE, bool>();
		}
		if (this.m_requestTimeList == null)
		{
			this.m_requestTimeList = new Dictionary<DailyBattleManager.REQ_TYPE, DateTime>();
		}
		if (type != DailyBattleManager.REQ_TYPE.NUM)
		{
			if (!this.m_isRequestList.ContainsKey(type))
			{
				this.m_isRequestList.Add(type, true);
			}
			else
			{
				this.m_isRequestList[type] = true;
			}
			if (!this.m_requestTimeList.ContainsKey(type))
			{
				this.m_requestTimeList.Add(type, NetBase.GetCurrentTime());
			}
			else
			{
				this.m_requestTimeList[type] = NetBase.GetCurrentTime();
			}
		}
	}

	private void EndRequest(DailyBattleManager.REQ_TYPE type)
	{
		if (this.m_isRequestList == null)
		{
			this.m_isRequestList = new Dictionary<DailyBattleManager.REQ_TYPE, bool>();
		}
		if (this.m_requestTimeList == null)
		{
			this.m_requestTimeList = new Dictionary<DailyBattleManager.REQ_TYPE, DateTime>();
		}
		if (type != DailyBattleManager.REQ_TYPE.NUM)
		{
			if (!this.m_isRequestList.ContainsKey(type))
			{
				this.m_isRequestList.Add(type, false);
			}
			else
			{
				this.m_isRequestList[type] = false;
			}
		}
	}

	private bool IsRequestPossible(DailyBattleManager.REQ_TYPE type)
	{
		bool result = false;
		if (this.m_isRequestList == null || this.m_requestTimeList == null)
		{
			return true;
		}
		if (type != DailyBattleManager.REQ_TYPE.NUM)
		{
			if (this.m_isRequestList.ContainsKey(type) && this.m_requestTimeList.ContainsKey(type))
			{
				if (!this.m_isRequestList[type])
				{
					if (type != DailyBattleManager.REQ_TYPE.POST_RESULT && type != DailyBattleManager.REQ_TYPE.RESET_MATCHING)
					{
						if (GeneralUtil.IsOverTimeSecond(this.m_requestTimeList[type], 2))
						{
							result = true;
						}
					}
					else
					{
						result = true;
					}
				}
				else if (type != DailyBattleManager.REQ_TYPE.POST_RESULT && type != DailyBattleManager.REQ_TYPE.RESET_MATCHING)
				{
					if (GeneralUtil.IsOverTimeSecond(this.m_requestTimeList[type], 60))
					{
						this.EndRequest(type);
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	public bool IsDataReload(DailyBattleManager.DATA_TYPE type)
	{
		bool result = true;
		if (this.m_currentDataLastUpdateTimeList != null && this.m_currentDataLastUpdateTimeList.ContainsKey(type))
		{
			DateTime baseTime = this.m_currentDataLastUpdateTimeList[type];
			if (!GeneralUtil.IsOverTimeSecond(baseTime, 600))
			{
				result = false;
				if (type != DailyBattleManager.DATA_TYPE.END_TIME)
				{
					DateTime currentTime = NetBase.GetCurrentTime();
					DateTime currentEndTime = this.currentEndTime;
					if (currentTime.Ticks > currentEndTime.Ticks)
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	private bool Request(DailyBattleManager.REQ_TYPE type)
	{
		bool result = false;
		if (this.CheckChainOrMultiRequestType(type))
		{
			result = true;
			switch (type)
			{
			case DailyBattleManager.REQ_TYPE.GET_STATUS:
				this.RequestGetStatus(null);
				break;
			case DailyBattleManager.REQ_TYPE.UPD_STATUS:
				this.RequestUpdateStatus(null);
				break;
			case DailyBattleManager.REQ_TYPE.POST_RESULT:
				this.RequestPostResult(null);
				break;
			case DailyBattleManager.REQ_TYPE.GET_PRIZE:
				this.RequestGetPrize(null);
				break;
			case DailyBattleManager.REQ_TYPE.GET_DATA:
				this.RequestGetData(null);
				break;
			default:
				result = false;
				break;
			}
		}
		return result;
	}

	public bool RequestGetStatus(DailyBattleManager.CallbackGetStatus callback)
	{
		if (this.IsRequestGetStatus())
		{
			this.StartRequest(DailyBattleManager.REQ_TYPE.GET_STATUS);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callbackGetStatus = callback;
				loggedInServerInterface.RequestServerGetDailyBattleStatus(base.gameObject);
				return true;
			}
		}
		return false;
	}

	public bool IsRequestGetStatus()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && this.IsRequestPossible(DailyBattleManager.REQ_TYPE.GET_STATUS);
	}

	public bool RequestUpdateStatus(DailyBattleManager.CallbackGetStatus callback)
	{
		if (this.IsRequestUpdateStatus())
		{
			this.StartRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callbackGetStatus = callback;
				loggedInServerInterface.RequestServerUpdateDailyBattleStatus(base.gameObject);
				return true;
			}
		}
		return false;
	}

	public bool IsRequestUpdateStatus()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && this.IsRequestPossible(DailyBattleManager.REQ_TYPE.UPD_STATUS);
	}

	public bool RequestPostResult(DailyBattleManager.CallbackPostResult callback)
	{
		if (this.IsRequestPostResult())
		{
			this.StartRequest(DailyBattleManager.REQ_TYPE.POST_RESULT);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callbackPostResult = callback;
				loggedInServerInterface.RequestServerPostDailyBattleResult(base.gameObject);
				return true;
			}
		}
		return false;
	}

	public bool IsRequestPostResult()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && this.IsRequestPossible(DailyBattleManager.REQ_TYPE.POST_RESULT);
	}

	public bool RequestGetData(DailyBattleManager.CallbackGetData callback)
	{
		if (this.IsRequestGetData())
		{
			this.StartRequest(DailyBattleManager.REQ_TYPE.GET_DATA);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callbackGetData = callback;
				loggedInServerInterface.RequestServerGetDailyBattleData(base.gameObject);
				return true;
			}
		}
		return false;
	}

	public bool IsEndTimeOver()
	{
		return NetBase.GetCurrentTime().Ticks >= this.m_currentEndTime.Ticks;
	}

	public bool IsReload(DailyBattleManager.REQ_TYPE reqType, double waitMinutes = 1f)
	{
		bool result = true;
		if (this.m_requestTimeList != null && this.m_requestTimeList.ContainsKey(reqType))
		{
			DateTime currentTime = NetBase.GetCurrentTime();
			DateTime d = this.m_requestTimeList[reqType];
			if ((currentTime - d).TotalMinutes < waitMinutes)
			{
				result = false;
			}
		}
		return result;
	}

	public bool IsDataInit(DailyBattleManager.REQ_TYPE reqType)
	{
		bool result = false;
		if (this.m_requestTimeList != null && this.m_requestTimeList.ContainsKey(reqType))
		{
			result = true;
		}
		return result;
	}

	public bool IsRequestGetData()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && this.IsRequestPossible(DailyBattleManager.REQ_TYPE.GET_DATA);
	}

	public bool RequestGetPrize(DailyBattleManager.CallbackGetPrize callback)
	{
		if (this.IsRequestGetPrize())
		{
			this.StartRequest(DailyBattleManager.REQ_TYPE.GET_PRIZE);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callbackGetPrize = callback;
				loggedInServerInterface.RequestServerGetPrizeDailyBattle(base.gameObject);
				return true;
			}
		}
		return false;
	}

	public bool IsRequestGetPrize()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && this.IsRequestPossible(DailyBattleManager.REQ_TYPE.GET_PRIZE);
	}

	public bool RequestGetDataHistory(int count, DailyBattleManager.CallbackGetDataHistory callback)
	{
		if (this.IsRequestGetDataHistory())
		{
			this.StartRequest(DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callbackGetDataHistory = callback;
				loggedInServerInterface.RequestServerGetDailyBattleDataHistory(count, base.gameObject);
				return true;
			}
		}
		return false;
	}

	public bool IsRequestGetDataHistory()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && this.IsRequestPossible(DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY);
	}

	public bool RequestResetMatching(int type, DailyBattleManager.CallbackResetMatching callback)
	{
		if (this.IsRequestResetMatching())
		{
			this.StartRequest(DailyBattleManager.REQ_TYPE.RESET_MATCHING);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callbackResetMatching = callback;
				loggedInServerInterface.RequestServerResetDailyBattleMatching(type, base.gameObject);
				return true;
			}
		}
		return false;
	}

	public bool IsRequestResetMatching()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && this.IsRequestPossible(DailyBattleManager.REQ_TYPE.RESET_MATCHING);
	}

	public void Dump()
	{
		global::Debug.Log("DailyBattleManager  Dump ============================================================================================");
		if (this.m_currentStatus != null)
		{
			this.m_currentStatus.Dump();
		}
		else
		{
			global::Debug.Log("ServerDailyBattleStatus  null");
		}
		if (this.m_currentDataPair != null)
		{
			this.m_currentDataPair.Dump();
		}
		else
		{
			global::Debug.Log("ServerDailyBattleDataPair  null");
		}
		if (this.m_currentDataPairList != null)
		{
			global::Debug.Log(string.Format("dataPairList:{0}", this.m_currentDataPairList.Count));
		}
		else
		{
			global::Debug.Log(string.Format("dataPairList:{0}", 0));
		}
		if (this.m_currentPrizeList != null)
		{
			global::Debug.Log(string.Format("prizeList:{0}", this.m_currentPrizeList.Count));
		}
		else
		{
			global::Debug.Log(string.Format("prizeList:{0}", 0));
		}
		global::Debug.Log(string.Format("rewardFlag:{0}", this.m_currentRewardFlag));
		global::Debug.Log(string.Format("endTime:{0}", this.m_currentEndTime.ToString()));
		global::Debug.Log("DailyBattleManager  Dump --------------------------------------------------------------------------------------------");
	}

	private void ServerGetDailyBattleStatus_Succeeded(MsgGetDailyBattleStatusSucceed msg)
	{
		if (msg != null)
		{
			this.SetCurrentStatus(msg.battleStatus);
			this.SetCurrentEndTime(msg.endTime);
			this.EndRequest(DailyBattleManager.REQ_TYPE.GET_STATUS);
			if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.GET_STATUS))
			{
				this.m_chainRequestList[DailyBattleManager.REQ_TYPE.GET_STATUS] = 2;
				this.NextChainRequest();
			}
			if (this.m_callbackGetStatus != null)
			{
				this.m_callbackGetStatus(msg.battleStatus, msg.endTime, false, null);
			}
		}
		this.m_callbackGetStatus = null;
		if (this.m_showLog)
		{
			global::Debug.Log("DailyBattleManager ServerGetDailyBattleStatus_Succeeded !!!!!");
			this.Dump();
		}
	}

	private void ServerGetDailyBattleStatus_Failed(MsgServerConnctFailed msg)
	{
		global::Debug.Log("DailyBattleManager ServerGetDailyBattleStatus_Failed !!!!!");
		this.EndRequest(DailyBattleManager.REQ_TYPE.GET_STATUS);
		if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.GET_STATUS))
		{
			this.m_chainRequestList[DailyBattleManager.REQ_TYPE.GET_STATUS] = 3;
			this.NextChainRequest();
		}
		if (this.m_callbackGetStatus != null)
		{
			this.m_callbackGetStatus(null, NetBase.GetCurrentTime(), false, null);
		}
		this.m_callbackGetStatus = null;
	}

	private void ServerUpdateDailyBattleStatus_Succeeded(MsgUpdateDailyBattleStatusSucceed msg)
	{
		if (msg != null)
		{
			this.SetCurrentStatus(msg.battleStatus);
			this.SetCurrentEndTime(msg.endTime);
			this.SetCurrentRewardFlag(msg.rewardFlag);
			this.SetCurrentRewardDataPair(msg.rewardBattleDataPair);
			this.EndRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS);
			if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS))
			{
				this.m_chainRequestList[DailyBattleManager.REQ_TYPE.UPD_STATUS] = 2;
				this.NextChainRequest();
			}
			if (this.m_callbackGetStatus != null)
			{
				this.m_callbackGetStatus(msg.battleStatus, msg.endTime, msg.rewardFlag, msg.rewardBattleDataPair);
			}
		}
		this.m_callbackGetStatus = null;
		if (this.m_showLog)
		{
			global::Debug.Log("DailyBattleManager ServerUpdateDailyBattleStatus_Succeeded !!!!!");
			this.Dump();
		}
	}

	private void ServerUpdateDailyBattleStatus_Failed(MsgServerConnctFailed msg)
	{
		global::Debug.Log("DailyBattleManager ServerUpdateDailyBattleStatus_Failed !!!!!");
		this.EndRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS);
		if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.UPD_STATUS))
		{
			this.m_chainRequestList[DailyBattleManager.REQ_TYPE.UPD_STATUS] = 3;
			this.NextChainRequest();
		}
		if (this.m_callbackGetStatus != null)
		{
			this.m_callbackGetStatus(null, NetBase.GetCurrentTime(), false, null);
		}
		this.m_callbackGetStatus = null;
	}

	private void ServerPostDailyBattleResult_Succeeded(MsgPostDailyBattleResultSucceed msg)
	{
		if (msg != null)
		{
			this.SetCurrentDataPair(msg.battleDataPair);
			this.SetCurrentRewardFlag(msg.rewardFlag);
			this.SetCurrentRewardDataPair(msg.rewardBattleDataPair);
			this.EndRequest(DailyBattleManager.REQ_TYPE.POST_RESULT);
			if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.POST_RESULT))
			{
				this.m_chainRequestList[DailyBattleManager.REQ_TYPE.POST_RESULT] = 2;
				this.NextChainRequest();
			}
			if (this.m_callbackPostResult != null)
			{
				this.m_callbackPostResult(msg.battleDataPair, msg.rewardFlag, msg.rewardBattleDataPair);
			}
		}
		this.m_callbackPostResult = null;
		if (this.m_showLog)
		{
			global::Debug.Log("DailyBattleManager ServerPostDailyBattleResult_Succeeded !!!!!");
			this.Dump();
		}
	}

	private void ServerPostDailyBattleResult_Failed(MsgServerConnctFailed msg)
	{
		global::Debug.Log("DailyBattleManager ServerPostDailyBattleResult_Failed !!!!!");
		this.EndRequest(DailyBattleManager.REQ_TYPE.POST_RESULT);
		if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.POST_RESULT))
		{
			this.m_chainRequestList[DailyBattleManager.REQ_TYPE.POST_RESULT] = 3;
			this.NextChainRequest();
		}
		if (this.m_callbackPostResult != null)
		{
			this.m_callbackPostResult(null, false, null);
		}
		this.m_callbackPostResult = null;
	}

	private void ServerGetDailyBattleData_Succeeded(MsgGetDailyBattleDataSucceed msg)
	{
		if (msg != null)
		{
			this.SetCurrentDataPair(msg.battleDataPair);
			this.EndRequest(DailyBattleManager.REQ_TYPE.GET_DATA);
			if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.GET_DATA))
			{
				this.m_chainRequestList[DailyBattleManager.REQ_TYPE.GET_DATA] = 2;
				this.NextChainRequest();
			}
			if (this.m_callbackGetData != null)
			{
				this.m_callbackGetData(msg.battleDataPair);
			}
		}
		this.m_callbackGetData = null;
		if (this.m_showLog)
		{
			global::Debug.Log("DailyBattleManager ServerGetDailyBattleData_Succeeded !!!!!");
			this.Dump();
		}
	}

	private void ServerGetDailyBattleData_Failed(MsgServerConnctFailed msg)
	{
		global::Debug.Log("DailyBattleManager ServerGetDailyBattleData_Failed !!!!!");
		this.EndRequest(DailyBattleManager.REQ_TYPE.GET_DATA);
		if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.GET_DATA))
		{
			this.m_chainRequestList[DailyBattleManager.REQ_TYPE.GET_DATA] = 3;
			this.NextChainRequest();
		}
		if (this.m_callbackGetData != null)
		{
			this.m_callbackGetData(null);
		}
		this.m_callbackGetData = null;
	}

	private void ServerGetPrizeDailyBattle_Succeeded(MsgGetPrizeDailyBattleSucceed msg)
	{
		if (msg != null)
		{
			this.SetCurrentPrizeList(msg.battlePrizeDataList);
			this.EndRequest(DailyBattleManager.REQ_TYPE.GET_PRIZE);
			if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.GET_PRIZE))
			{
				this.m_chainRequestList[DailyBattleManager.REQ_TYPE.GET_PRIZE] = 2;
				this.NextChainRequest();
			}
			if (this.m_callbackGetPrize != null)
			{
				this.m_callbackGetPrize(msg.battlePrizeDataList);
			}
		}
		this.m_callbackGetPrize = null;
		if (this.m_showLog)
		{
			global::Debug.Log("DailyBattleManager ServerGetPrizeDailyBattle_Succeeded !!!!!");
			this.Dump();
		}
	}

	private void ServerGetPrizeDailyBattle_Failed(MsgServerConnctFailed msg)
	{
		global::Debug.Log("DailyBattleManager ServerGetPrizeDailyBattle_Failed !!!!!");
		this.EndRequest(DailyBattleManager.REQ_TYPE.GET_PRIZE);
		if (this.IsChainRequest(DailyBattleManager.REQ_TYPE.GET_PRIZE))
		{
			this.m_chainRequestList[DailyBattleManager.REQ_TYPE.GET_PRIZE] = 3;
			this.NextChainRequest();
		}
		if (this.m_callbackGetPrize != null)
		{
			this.m_callbackGetPrize(null);
		}
		this.m_callbackGetPrize = null;
	}

	private void ServerGetDailyBattleDataHistory_Succeeded(MsgGetDailyBattleDataHistorySucceed msg)
	{
		if (msg != null)
		{
			this.SetCurrentDataPairList(msg.battleDataPairList);
			this.EndRequest(DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY);
			if (this.m_callbackGetDataHistory != null)
			{
				this.m_callbackGetDataHistory(msg.battleDataPairList);
			}
		}
		this.m_callbackGetDataHistory = null;
		if (this.m_showLog)
		{
			global::Debug.Log("DailyBattleManager ServerGetDailyBattleDataHistory_Succeeded !!!!!");
			this.Dump();
		}
	}

	private void ServerGetDailyBattleDataHistory_Failed(MsgServerConnctFailed msg)
	{
		global::Debug.Log("DailyBattleManager ServerGetDailyBattleDataHistory_Failed !!!!!");
		this.EndRequest(DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY);
		if (this.m_callbackGetDataHistory != null)
		{
			this.m_callbackGetDataHistory(null);
		}
		this.m_callbackGetDataHistory = null;
	}

	private void ServerResetDailyBattleMatching_Succeeded(MsgResetDailyBattleMatchingSucceed msg)
	{
		if (msg != null)
		{
			this.SetCurrentDataPair(msg.battleDataPair);
			this.SetCurrentEndTime(msg.endTime);
			this.EndRequest(DailyBattleManager.REQ_TYPE.RESET_MATCHING);
			if (this.m_callbackResetMatching != null)
			{
				this.m_callbackResetMatching(msg.playerState, msg.battleDataPair, msg.endTime);
			}
		}
		this.m_callbackResetMatching = null;
		if (this.m_showLog)
		{
			global::Debug.Log("DailyBattleManager ServerResetDailyBattleMatching_Succeeded !!!!!");
			this.Dump();
		}
	}

	private void ServerResetDailyBattleMatching_Failed(MsgServerConnctFailed msg)
	{
		global::Debug.Log("DailyBattleManager ServerResetDailyBattleMatching_Failed !!!!!");
		this.EndRequest(DailyBattleManager.REQ_TYPE.RESET_MATCHING);
		if (this.m_callbackResetMatching != null)
		{
			this.m_callbackResetMatching(null, null, NetBase.GetCurrentTime());
		}
		this.m_callbackResetMatching = null;
	}
}
