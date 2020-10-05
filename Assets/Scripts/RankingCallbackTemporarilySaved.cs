using System;
using System.Collections.Generic;

public class RankingCallbackTemporarilySaved
{
	private List<RankingUtil.Ranker> m_rankerList;

	private RankingUtil.RankingScoreType m_score;

	private RankingUtil.RankingRankerType m_type = RankingUtil.RankingRankerType.RIVAL;

	private int m_page;

	private bool m_isNext;

	private bool m_isPrev;

	private bool m_isCashData;

	private RankingManager.CallbackRankingData m_callback;

	public RankingCallbackTemporarilySaved(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData, RankingManager.CallbackRankingData callback)
	{
		this.m_rankerList = rankerList;
		this.m_score = score;
		this.m_type = type;
		this.m_page = page;
		this.m_isNext = isNext;
		this.m_isPrev = isPrev;
		this.m_isCashData = isCashData;
		this.m_callback = callback;
	}

	public void SendCallback()
	{
		if (this.m_callback != null)
		{
			this.m_callback(this.m_rankerList, this.m_score, this.m_type, this.m_page, this.m_isNext, this.m_isPrev, this.m_isCashData);
		}
	}
}
