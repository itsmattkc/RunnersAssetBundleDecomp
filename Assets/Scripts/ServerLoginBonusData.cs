using System;
using System.Collections.Generic;

public class ServerLoginBonusData
{
	public ServerLoginBonusState m_loginBonusState;

	public DateTime m_startTime;

	public DateTime m_endTime;

	public List<ServerLoginBonusReward> m_loginBonusRewardList;

	public List<ServerLoginBonusReward> m_firstLoginBonusRewardList;

	public int m_rewardId;

	public int m_rewardDays;

	public int m_firstRewardDays;

	public ServerLoginBonusReward m_lastBonusReward;

	public ServerLoginBonusReward m_firstLastBonusReward;

	public ServerLoginBonusData()
	{
		this.m_loginBonusState = new ServerLoginBonusState();
		this.m_startTime = DateTime.Now;
		this.m_endTime = DateTime.Now;
		this.m_loginBonusRewardList = new List<ServerLoginBonusReward>();
		this.m_firstLoginBonusRewardList = new List<ServerLoginBonusReward>();
		this.m_rewardId = 0;
		this.m_rewardDays = 0;
		this.m_firstRewardDays = 0;
		this.m_lastBonusReward = null;
		this.m_firstLastBonusReward = null;
	}

	public void CopyTo(ServerLoginBonusData dest)
	{
		this.m_loginBonusState.CopyTo(dest.m_loginBonusState);
		dest.m_startTime = this.m_startTime;
		dest.m_endTime = this.m_endTime;
		foreach (ServerLoginBonusReward current in this.m_loginBonusRewardList)
		{
			dest.m_loginBonusRewardList.Add(current);
		}
		foreach (ServerLoginBonusReward current2 in this.m_firstLoginBonusRewardList)
		{
			dest.m_firstLoginBonusRewardList.Add(current2);
		}
		dest.m_rewardId = this.m_rewardId;
		dest.m_rewardDays = this.m_rewardDays;
		dest.m_firstRewardDays = this.m_firstRewardDays;
	}

	public int CalcTodayCount()
	{
		int result = 0;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			DateTime startTime = this.m_startTime;
			DateTime endTime = this.m_endTime;
			DateTime currentTime = NetUtil.GetCurrentTime();
			if (endTime < currentTime)
			{
				return -1;
			}
			if (startTime < currentTime)
			{
				result = (currentTime - startTime).Days;
			}
			else if (currentTime < startTime)
			{
				return -1;
			}
		}
		return result;
	}

	public int getTotalDays()
	{
		int result = 0;
		DateTime startTime = this.m_startTime;
		DateTime endTime = this.m_endTime;
		if (this.m_startTime < this.m_endTime)
		{
			result = (this.m_endTime - this.m_startTime).Days + 1;
		}
		return result;
	}

	public bool isGetLoginBonusToday()
	{
		DateTime lastBonusTime = this.m_loginBonusState.m_lastBonusTime;
		DateTime currentTime = NetUtil.GetCurrentTime();
		return currentTime < lastBonusTime || (this.m_rewardId == -1 || this.m_rewardDays == -1);
	}

	public void setLoginBonusList(ServerLoginBonusReward reward, ServerLoginBonusReward firstReward)
	{
		this.clearLoginBonusList();
		if (reward != null)
		{
			this.m_lastBonusReward = new ServerLoginBonusReward();
			reward.CopyTo(this.m_lastBonusReward);
		}
		if (firstReward != null)
		{
			this.m_firstLastBonusReward = new ServerLoginBonusReward();
			firstReward.CopyTo(this.m_firstLastBonusReward);
		}
	}

	public void clearLoginBonusList()
	{
		this.m_lastBonusReward = null;
		this.m_firstLastBonusReward = null;
	}

	public void replayTodayBonus()
	{
		int numLogin = this.m_loginBonusState.m_numLogin;
		int numBonus = this.m_loginBonusState.m_numBonus;
		ServerLoginBonusReward reward = null;
		if (numBonus > 0 && this.m_loginBonusRewardList != null && this.m_loginBonusRewardList.Count > 0)
		{
			reward = this.m_loginBonusRewardList[numBonus - 1];
		}
		ServerLoginBonusReward firstReward = null;
		if (numLogin > 0 && this.m_firstLoginBonusRewardList != null && this.m_firstLoginBonusRewardList.Count > 0)
		{
			firstReward = this.m_firstLoginBonusRewardList[numLogin - 1];
		}
		this.setLoginBonusList(reward, firstReward);
	}
}
