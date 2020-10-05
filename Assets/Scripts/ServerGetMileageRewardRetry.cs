using System;
using UnityEngine;

public class ServerGetMileageRewardRetry : ServerRetryProcess
{
	private int m_episode;

	private int m_chapter;

	public ServerGetMileageRewardRetry(int episode, int chapter, GameObject callbackObject) : base(callbackObject)
	{
		this.m_episode = episode;
		this.m_chapter = chapter;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetMileageReward(this.m_episode, this.m_chapter, this.m_callbackObject);
		}
	}
}
