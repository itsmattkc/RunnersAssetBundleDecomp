using System;
using System.Runtime.InteropServices;

public struct QuickModePostGameResultNativeParam
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

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] numAnimals;

	public bool closed;

	public int maxComboCount;

	public void Init(ServerQuickModeGameResults resultData)
	{
		if (resultData != null)
		{
			BindingLinkUtility.LongToIntArray(out this.score, resultData.m_score);
			BindingLinkUtility.LongToIntArray(out this.numRings, resultData.m_numRings);
			BindingLinkUtility.LongToIntArray(out this.numFailureRings, resultData.m_numFailureRings);
			BindingLinkUtility.LongToIntArray(out this.numRedStarRings, resultData.m_numRedStarRings);
			BindingLinkUtility.LongToIntArray(out this.distance, resultData.m_distance);
			BindingLinkUtility.LongToIntArray(out this.dailyChallengeValue, resultData.m_dailyMissionValue);
			BindingLinkUtility.LongToIntArray(out this.numAnimals, resultData.m_numAnimals);
			this.maxComboCount = resultData.m_maxComboCount;
			this.closed = resultData.m_isSuspended;
			this.dailyChallengeComplete = resultData.m_dailyMissionComplete;
		}
	}
}
