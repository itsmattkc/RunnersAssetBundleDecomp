using System;
using UnityEngine;

public class ServerLoginBonusSelectRetry : ServerRetryProcess
{
	private int m_rewardId;

	private int m_rewardDays;

	private int m_rewardSelect;

	private int m_firstRewardDays;

	private int m_firstRewardSelect;

	public ServerLoginBonusSelectRetry(int rewardId, int rewardDays, int rewardSelect, int firstRewardDays, int firstRewardSelect, GameObject callbackObject) : base(callbackObject)
	{
		this.m_rewardId = rewardId;
		this.m_rewardDays = rewardDays;
		this.m_rewardSelect = rewardSelect;
		this.m_firstRewardDays = firstRewardDays;
		this.m_firstRewardSelect = firstRewardSelect;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerLoginBonusSelect(this.m_rewardId, this.m_rewardDays, this.m_rewardSelect, this.m_firstRewardDays, this.m_firstRewardSelect, this.m_callbackObject);
		}
	}
}
