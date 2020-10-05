using System;
using System.Collections.Generic;

public class ServerLeaderboardEntries
{
	public ServerLeaderboardEntry m_myLeaderboardEntry;

	public List<ServerLeaderboardEntry> m_leaderboardEntries;

	public int m_resultTotalEntries;

	public int m_resetTime;

	public int m_startTime;

	public int m_startIndex;

	public int m_mode;

	public int m_first;

	public int m_count;

	public int m_index;

	public int m_rankingType;

	public int m_eventId;

	public string[] m_friendIdList;

	public ServerLeaderboardEntries()
	{
		this.m_leaderboardEntries = new List<ServerLeaderboardEntry>();
		this.m_mode = 0;
		this.m_first = -1;
		this.m_count = 0;
		this.m_rankingType = -1;
		this.m_index = 0;
		this.m_eventId = 0;
	}

	public void CopyTo(ServerLeaderboardEntries to)
	{
		if (this.m_myLeaderboardEntry != null)
		{
			if (to.m_myLeaderboardEntry == null)
			{
				to.m_myLeaderboardEntry = new ServerLeaderboardEntry();
			}
			this.m_myLeaderboardEntry.CopyTo(to.m_myLeaderboardEntry);
		}
		else
		{
			to.m_myLeaderboardEntry = null;
		}
		to.m_leaderboardEntries.Clear();
		foreach (ServerLeaderboardEntry current in this.m_leaderboardEntries)
		{
			ServerLeaderboardEntry serverLeaderboardEntry = new ServerLeaderboardEntry();
			current.CopyTo(serverLeaderboardEntry);
			to.m_leaderboardEntries.Add(serverLeaderboardEntry);
		}
		to.m_resultTotalEntries = this.m_resultTotalEntries;
		to.m_resetTime = this.m_resetTime;
		to.m_startTime = this.m_startTime;
		to.m_startIndex = this.m_startIndex;
		to.m_mode = this.m_mode;
		to.m_first = this.m_first;
		to.m_count = this.m_count;
		to.m_index = this.m_index;
		to.m_rankingType = this.m_rankingType;
		to.m_eventId = this.m_eventId;
		if (this.m_friendIdList != null)
		{
			to.m_friendIdList = new string[this.m_friendIdList.Length];
			this.m_friendIdList.CopyTo(to.m_friendIdList, 0);
		}
		else
		{
			to.m_friendIdList = null;
		}
	}

	public bool CompareParam(int mode, int first, int count, int index, int rankingType, int eventId, string[] friendIdList)
	{
		if (mode != this.m_mode || first != this.m_first || count != this.m_count || rankingType != this.m_rankingType || index != this.m_index || eventId != this.m_eventId)
		{
			return false;
		}
		if (friendIdList == null && this.m_friendIdList == null)
		{
			return true;
		}
		if (friendIdList == null || this.m_friendIdList == null || friendIdList.Length != this.m_friendIdList.Length)
		{
			return false;
		}
		for (int i = 0; i < friendIdList.Length; i++)
		{
			if (friendIdList[i] != this.m_friendIdList[i])
			{
				return false;
			}
		}
		return true;
	}

	public bool IsRivalHighScore()
	{
		return ServerLeaderboardEntries.IsRivalHighScore(this.m_first, this.m_rankingType);
	}

	public bool IsRivalRanking()
	{
		return this.m_rankingType == 4 || this.m_rankingType == 5;
	}

	public static bool IsRivalHighScore(int first, int rankingType)
	{
		return first == 0 && rankingType == 4;
	}

	public ServerLeaderboardEntry GetRankTop()
	{
		if (this.m_leaderboardEntries != null && this.m_leaderboardEntries.Count > 0 && this.m_leaderboardEntries[0].m_grade == 1)
		{
			return this.m_leaderboardEntries[0];
		}
		return null;
	}

	public bool IsNext()
	{
		bool result = false;
		if (this.m_leaderboardEntries != null && this.m_count <= this.m_leaderboardEntries.Count)
		{
			result = true;
		}
		return result;
	}

	public bool GetNextRanking(ref int top, ref int count, int margin)
	{
		if (!this.IsNext())
		{
			return false;
		}
		top = this.m_count - margin + 1;
		if (top < 1)
		{
			count = margin + count + top;
			top = 1;
		}
		else
		{
			count = margin + count;
		}
		return true;
	}

	public bool IsPrev()
	{
		bool result = false;
		if (this.m_leaderboardEntries != null && this.m_leaderboardEntries.Count > 0)
		{
			if (this.m_first > 1)
			{
				result = true;
			}
			else if (this.m_first == 0 && this.m_leaderboardEntries[0].m_grade > 1)
			{
				result = true;
			}
		}
		return result;
	}

	public bool GetPrevRanking(ref int top, ref int count, int margin)
	{
		if (!this.IsPrev())
		{
			return false;
		}
		top = this.m_first - count;
		if (top < 1)
		{
			count += top - 1;
			top = 1;
		}
		return true;
	}

	public bool IsReload()
	{
		bool result = false;
		DateTime localDateTime = NetUtil.GetLocalDateTime((long)this.m_resetTime);
		if (localDateTime != default(DateTime) && NetUtil.GetCurrentTime() > localDateTime)
		{
			result = true;
		}
		return result;
	}

	public TimeSpan GetResetTimeSpan()
	{
		TimeSpan result = default(TimeSpan);
		DateTime localDateTime = NetUtil.GetLocalDateTime((long)this.m_resetTime);
		if (localDateTime != default(DateTime))
		{
			return localDateTime - NetUtil.GetCurrentTime();
		}
		return result;
	}

	public bool UpdateSendChallenge(string id)
	{
		bool result = false;
		if (this.m_leaderboardEntries != null && this.m_leaderboardEntries.Count > 0)
		{
			foreach (ServerLeaderboardEntry current in this.m_leaderboardEntries)
			{
				if (id == current.m_hspId)
				{
					current.m_energyFlg = true;
					result = true;
				}
			}
		}
		return result;
	}
}
