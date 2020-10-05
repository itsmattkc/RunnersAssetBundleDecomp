using System;
using UnityEngine;

public class ServerGetLeaderboardEntriesRetry : ServerRetryProcess
{
	public int m_mode;

	public int m_first;

	public int m_count;

	public int m_index;

	public int m_rankingType;

	public int m_eventId;

	public string[] m_friendIdList;

	public ServerGetLeaderboardEntriesRetry(int mode, int first, int count, int index, int rankingType, int eventId, string[] friendIdList, GameObject callbackObject) : base(callbackObject)
	{
		this.m_mode = mode;
		this.m_first = first;
		this.m_count = count;
		this.m_index = index;
		this.m_rankingType = rankingType;
		this.m_eventId = eventId;
		this.m_friendIdList = friendIdList;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetLeaderboardEntries(this.m_mode, this.m_first, this.m_count, this.m_index, this.m_rankingType, this.m_eventId, this.m_friendIdList, this.m_callbackObject);
		}
	}
}
