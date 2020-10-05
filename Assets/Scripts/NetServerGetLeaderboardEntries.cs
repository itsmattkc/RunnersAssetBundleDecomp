using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetLeaderboardEntries : NetBase
{
	private int _paramMode_k__BackingField;

	private int _paramFirst_k__BackingField;

	private int _paramCount_k__BackingField;

	private int _paramIndex_k__BackingField;

	private int _paramEventId_k__BackingField;

	private int _paramRankingType_k__BackingField;

	private string[] _paramFriendIdList_k__BackingField;

	private int _resultTotalEntries_k__BackingField;

	private ServerLeaderboardEntry _resultMyLeaderboardEntry_k__BackingField;

	private int _resetTime_k__BackingField;

	private int _startTime_k__BackingField;

	private int _startIndex_k__BackingField;

	private ServerLeaderboardEntries _leaderboardEntries_k__BackingField;

	private List<ServerLeaderboardEntry> _resultLeaderboardEntriesList_k__BackingField;

	public int paramMode
	{
		get;
		set;
	}

	public int paramFirst
	{
		get;
		set;
	}

	public int paramCount
	{
		get;
		set;
	}

	public int paramIndex
	{
		get;
		set;
	}

	public int paramEventId
	{
		get;
		set;
	}

	public int paramRankingType
	{
		get;
		set;
	}

	public string[] paramFriendIdList
	{
		get;
		set;
	}

	private int resultTotalEntries
	{
		get;
		set;
	}

	private ServerLeaderboardEntry resultMyLeaderboardEntry
	{
		get;
		set;
	}

	private int resultEntries
	{
		get
		{
			if (this.resultLeaderboardEntriesList != null)
			{
				return this.resultLeaderboardEntriesList.Count;
			}
			return 0;
		}
	}

	private int resetTime
	{
		get;
		set;
	}

	private int startTime
	{
		get;
		set;
	}

	private int startIndex
	{
		get;
		set;
	}

	public ServerLeaderboardEntries leaderboardEntries
	{
		get;
		protected set;
	}

	protected List<ServerLeaderboardEntry> resultLeaderboardEntriesList
	{
		get;
		set;
	}

	public NetServerGetLeaderboardEntries() : this(0, -1, -1, -1, -1, -1, null)
	{
		base.SetSecureFlag(false);
	}

	public NetServerGetLeaderboardEntries(int mode, int first, int count, int index, int rankingType, int eventId, string[] friendIdList)
	{
		this.paramMode = mode;
		this.paramFirst = first;
		this.paramCount = count;
		this.paramIndex = index;
		this.paramEventId = eventId;
		this.paramRankingType = rankingType;
		this.paramFriendIdList = friendIdList;
		base.SetSecureFlag(false);
	}

	protected override void DoRequest()
	{
		base.SetAction("Leaderboard/getWeeklyLeaderboardEntries");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			List<string> list = new List<string>();
			if (this.paramFriendIdList != null)
			{
				string[] paramFriendIdList = this.paramFriendIdList;
				for (int i = 0; i < paramFriendIdList.Length; i++)
				{
					string item = paramFriendIdList[i];
					list.Add(item);
				}
			}
			string getWeeklyLeaderboardEntries = instance.GetGetWeeklyLeaderboardEntries(this.paramMode, this.paramFirst, this.paramCount, this.paramRankingType, list, this.paramEventId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getWeeklyLeaderboardEntries);
		}
	}

	protected void SetParameter_LeaderboardEntries()
	{
		this.SetParameter_Mode();
		this.SetParameter_First();
		this.SetParameter_Count();
		this.SetParameter_RankingType();
		this.SetParameter_FriendIdList();
		this.SetParameter_EventId();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_LeaderboardEntries(jdata);
	}

	protected void GetResponse_LeaderboardEntries(JsonData jdata)
	{
		this.GetResponse_MyEntry(jdata);
		this.GetResponse_EntriesList(jdata);
		this.GetResponse_TotalEntries(jdata);
		this.GetResponse_ResetTime(jdata);
		this.GetResponse_StartTime(jdata);
		this.GetResponse_StartIndex(jdata);
		this.leaderboardEntries = new ServerLeaderboardEntries
		{
			m_myLeaderboardEntry = this.resultMyLeaderboardEntry,
			m_resultTotalEntries = this.resultTotalEntries,
			m_leaderboardEntries = this.resultLeaderboardEntriesList,
			m_resetTime = this.resetTime,
			m_startTime = this.startTime,
			m_startIndex = this.startIndex,
			m_mode = this.paramMode,
			m_first = this.paramFirst,
			m_count = this.paramCount,
			m_index = this.paramIndex,
			m_rankingType = this.paramRankingType,
			m_eventId = this.paramEventId,
			m_friendIdList = this.paramFriendIdList
		};
		this.leaderboardEntries.CopyTo(ServerInterface.LeaderboardEntries);
		if (this.IsRivalHighScore())
		{
			this.leaderboardEntries.CopyTo(ServerInterface.LeaderboardEntriesRivalHighScore);
		}
		if (ServerLeaderboardEntries.IsRivalHighScore(0, this.leaderboardEntries.m_rankingType))
		{
			ServerLeaderboardEntry rankTop = this.leaderboardEntries.GetRankTop();
			if (rankTop != null)
			{
				rankTop.CopyTo(ServerInterface.LeaderboardEntryRivalHighScoreTop);
			}
		}
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultTotalEntries = this.paramCount;
		this.resultLeaderboardEntriesList = new List<ServerLeaderboardEntry>(this.paramCount);
		this.resultMyLeaderboardEntry = new ServerLeaderboardEntry();
		this.resultMyLeaderboardEntry.m_grade = 0;
		int paramFirst = this.paramFirst;
		int num = paramFirst;
		while (num < 125 && num < paramFirst + this.paramCount)
		{
			ServerLeaderboardEntry serverLeaderboardEntry = new ServerLeaderboardEntry();
			serverLeaderboardEntry.m_hspId = "Xeen_" + string.Format("{0:D4}", num);
			serverLeaderboardEntry.m_grade = num + 1;
			serverLeaderboardEntry.m_score = (long)(10000 - num * 100);
			serverLeaderboardEntry.m_name = "Xeen_" + string.Format("{0:D4}", num);
			serverLeaderboardEntry.m_url = string.Empty;
			this.resultLeaderboardEntriesList.Add(serverLeaderboardEntry);
			num++;
		}
	}

	public bool IsRivalHighScore()
	{
		return ServerLeaderboardEntries.IsRivalHighScore(this.paramFirst, this.paramRankingType);
	}

	private void SetParameter_Mode()
	{
		base.WriteActionParamValue("mode", this.paramMode);
	}

	private void SetParameter_First()
	{
		if (-1 < this.paramFirst && -1 < this.paramCount)
		{
			base.WriteActionParamValue("first", this.paramFirst);
		}
	}

	private void SetParameter_Count()
	{
		if (-1 < this.paramFirst && -1 < this.paramCount)
		{
			base.WriteActionParamValue("count", this.paramCount);
		}
	}

	private void SetParameter_RankingType()
	{
		base.WriteActionParamValue("type", this.paramRankingType);
	}

	private void SetParameter_FriendIdList()
	{
		if (this.paramFriendIdList != null && this.paramFriendIdList.Length != 0)
		{
			base.WriteActionParamArray("friendIdList", new List<object>(this.paramFriendIdList));
		}
	}

	private void SetParameter_EventId()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
	}

	public ServerLeaderboardEntry GetResultLeaderboardEntry(int index)
	{
		if (0 <= index && this.resultEntries > index)
		{
			return this.resultLeaderboardEntriesList[index];
		}
		return null;
	}

	private void GetResponse_MyEntry(JsonData jdata)
	{
		this.resultMyLeaderboardEntry = NetUtil.AnalyzeLeaderboardEntryJson(jdata, "playerEntry");
	}

	private void GetResponse_EntriesList(JsonData jdata)
	{
		this.resultLeaderboardEntriesList = new List<ServerLeaderboardEntry>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "entriesList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerLeaderboardEntry item = NetUtil.AnalyzeLeaderboardEntryJson(jdata2, string.Empty);
			this.resultLeaderboardEntriesList.Add(item);
		}
	}

	private void GetResponse_TotalEntries(JsonData jdata)
	{
		this.resultTotalEntries = NetUtil.GetJsonInt(jdata, "totalEntries");
	}

	private void GetResponse_ResetTime(JsonData jdata)
	{
		this.resetTime = NetUtil.GetJsonInt(jdata, "resetTime");
	}

	private void GetResponse_StartTime(JsonData jdata)
	{
		this.startTime = NetUtil.GetJsonInt(jdata, "startTime");
	}

	private void GetResponse_StartIndex(JsonData jdata)
	{
		this.startIndex = NetUtil.GetJsonInt(jdata, "startIndex");
	}
}
