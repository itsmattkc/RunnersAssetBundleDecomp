using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

internal class EasySnsFeedMonoBehaviour : MonoBehaviour
{
	public bool? m_isFeeded;

	public List<ServerPresentState> m_feedIncentiveList;

	public void Init()
	{
		this.m_isFeeded = null;
		this.m_feedIncentiveList = null;
	}

	private void FeedEndCallback(MsgSocialNormalResponse msg)
	{
		this.m_isFeeded = new bool?(!msg.m_result.IsError);
	}

	private void ServerGetFacebookIncentive_Succeeded(MsgGetNormalIncentiveSucceed msg)
	{
		this.m_feedIncentiveList = msg.m_incentive;
	}

	private void ServerGetFacebookIncentive_Failed(MsgServerConnctFailed mag)
	{
		this.m_feedIncentiveList = new List<ServerPresentState>();
	}
}
