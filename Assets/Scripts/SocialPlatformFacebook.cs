using System;
using System.Collections.Generic;
using UnityEngine;

public class SocialPlatformFacebook : SocialPlatform
{
	private SocialTaskManager m_manager;

	private GameObject m_callbackObject;

	private void Start()
	{
		this.m_manager = new SocialTaskManager();
	}

	public override void Initialize(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskInitialize facebookTaskInitialize = new FacebookTaskInitialize();
		facebookTaskInitialize.Request(callbackObject);
		this.m_manager.RequestProcess(facebookTaskInitialize);
	}

	public override void Login(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskLogin facebookTaskLogin = new FacebookTaskLogin();
		facebookTaskLogin.Request(callbackObject);
		this.m_manager.RequestProcess(facebookTaskLogin);
	}

	public override void Logout()
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskLogout facebookTaskLogout = new FacebookTaskLogout();
		facebookTaskLogout.Request();
		this.m_manager.RequestProcess(facebookTaskLogout);
	}

	public override void RequestMyProfile(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskRequestMyProfile facebookTaskRequestMyProfile = new FacebookTaskRequestMyProfile();
		facebookTaskRequestMyProfile.Request(callbackObject);
		this.m_manager.RequestProcess(facebookTaskRequestMyProfile);
	}

	public override void RequestFriendList(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskRequestFriendList facebookTaskRequestFriendList = new FacebookTaskRequestFriendList();
		facebookTaskRequestFriendList.Request(callbackObject);
		this.m_manager.RequestProcess(facebookTaskRequestFriendList);
	}

	public override void SetScore(SocialDefine.ScoreType type, int score, GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskSetScore facebookTaskSetScore = new FacebookTaskSetScore();
		facebookTaskSetScore.Request(type, score, callbackObject);
		this.m_manager.RequestProcess(facebookTaskSetScore);
	}

	public override void CreateMyGameData(string gameId, GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskCreateGameData facebookTaskCreateGameData = new FacebookTaskCreateGameData();
		facebookTaskCreateGameData.Request(gameId, this.m_manager, callbackObject);
		this.m_manager.RequestProcess(facebookTaskCreateGameData);
	}

	public override void RequestGameData(string userId, GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskRequestGameData facebookTaskRequestGameData = new FacebookTaskRequestGameData();
		facebookTaskRequestGameData.Request(userId, this.m_manager, callbackObject);
		this.m_manager.RequestProcess(facebookTaskRequestGameData);
	}

	public override void DeleteGameData(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskDeleteGameData facebookTaskDeleteGameData = new FacebookTaskDeleteGameData();
		facebookTaskDeleteGameData.Request(this.m_manager, callbackObject);
		this.m_manager.RequestProcess(facebookTaskDeleteGameData);
	}

	public override void InviteFriend(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskInviteFriend facebookTaskInviteFriend = new FacebookTaskInviteFriend();
		facebookTaskInviteFriend.Request(callbackObject);
		this.m_manager.RequestProcess(facebookTaskInviteFriend);
	}

	public override void ReceiveEnergy(string energyId, GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
	}

	public override void Feed(string feedCaption, string feedText, GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskFeed facebookTaskFeed = new FacebookTaskFeed();
		facebookTaskFeed.Request(feedCaption, feedText, callbackObject);
		this.m_manager.RequestProcess(facebookTaskFeed);
	}

	public override void SendEnergy(SocialUserData userData, GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskSendEnergy facebookTaskSendEnergy = new FacebookTaskSendEnergy();
		facebookTaskSendEnergy.Request(userData, callbackObject);
		this.m_manager.RequestProcess(facebookTaskSendEnergy);
	}

	public override void RequestInvitedFriend(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskRequestInviteList facebookTaskRequestInviteList = new FacebookTaskRequestInviteList();
		facebookTaskRequestInviteList.Request(callbackObject);
		this.m_manager.RequestProcess(facebookTaskRequestInviteList);
	}

	public override void RequestPermission(GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskRequestPermission facebookTaskRequestPermission = new FacebookTaskRequestPermission();
		facebookTaskRequestPermission.Request(callbackObject);
		this.m_manager.RequestProcess(facebookTaskRequestPermission);
	}

	public override void AddPermission(List<SocialInterface.Permission> permissions, GameObject callbackObject)
	{
		if (this.m_manager == null)
		{
			return;
		}
		FacebookTaskAddPermission facebookTaskAddPermission = new FacebookTaskAddPermission();
		facebookTaskAddPermission.Request(permissions, callbackObject);
		this.m_manager.RequestProcess(facebookTaskAddPermission);
	}

	private void Update()
	{
		if (this.m_manager != null)
		{
			this.m_manager.Update();
		}
	}
}
