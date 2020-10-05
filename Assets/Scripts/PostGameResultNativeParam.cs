using System;
using System.Runtime.InteropServices;

public struct PostGameResultNativeParam
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] score;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] distance;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] numRings;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] numFailureRings;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] numRedStarRings;

	public bool dailyChallengeComplete;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] dailyChallengeValue;

	public bool closed;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] numAnimals;

	public int reachPoint;

	public bool chapterClear;

	public int numBossAttack;

	public bool getChaoEgg;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] stageMaxScore;

	public int eventId;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] eventValue;

	public bool bossDestroyed;

	public int maxComboCount;

	public void Init(ServerGameResults resultData)
	{
		if (resultData != null)
		{
			this.closed = resultData.m_isSuspended;
			BindingLinkUtility.LongToIntArray(out this.score, resultData.m_score);
			BindingLinkUtility.LongToIntArray(out this.stageMaxScore, resultData.m_maxChapterScore);
			BindingLinkUtility.LongToIntArray(out this.numRings, resultData.m_numRings);
			BindingLinkUtility.LongToIntArray(out this.numFailureRings, resultData.m_numFailureRings);
			BindingLinkUtility.LongToIntArray(out this.numRedStarRings, resultData.m_numRedStarRings);
			BindingLinkUtility.LongToIntArray(out this.distance, resultData.m_distance);
			BindingLinkUtility.LongToIntArray(out this.dailyChallengeValue, resultData.m_dailyMissionValue);
			this.dailyChallengeComplete = resultData.m_dailyMissionComplete;
			BindingLinkUtility.LongToIntArray(out this.numAnimals, resultData.m_numAnimals);
			this.reachPoint = resultData.m_reachPoint;
			this.chapterClear = resultData.m_clearChapter;
			this.numBossAttack = resultData.m_numBossAttack;
			this.getChaoEgg = resultData.m_chaoEggPresent;
			int? num = resultData.m_eventId;
			if (num.HasValue)
			{
				this.eventId = resultData.m_eventId.Value;
				BindingLinkUtility.LongToIntArray(out this.eventValue, resultData.m_eventValue.Value);
			}
			else
			{
				this.eventId = -1;
				BindingLinkUtility.LongToIntArray(out this.eventValue, -1L);
			}
			this.bossDestroyed = resultData.m_isBossDestroyed;
			this.maxComboCount = resultData.m_maxComboCount;
		}
	}
}
