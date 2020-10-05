using Message;
using System;
using UnityEngine;

public class FacebookTaskCreateGameData : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskCreateGameData";

	private SocialTaskManager m_manager;

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	private string m_gameId;

	public FacebookTaskCreateGameData()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(string gameId, SocialTaskManager manager, GameObject callbackObject)
	{
		this.m_gameId = gameId;
		this.m_manager = manager;
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		bool flag = true;
		if (!FacebookUtil.IsLoggedIn())
		{
			flag = false;
		}
		if (!flag)
		{
			this.m_isEndProcess = true;
			if (this.m_callbackObject != null)
			{
				MsgSocialNormalResponse value = new MsgSocialNormalResponse();
				this.m_callbackObject.SendMessage("CreateGameDataEndCallback", value, SendMessageOptions.DontRequireReceiver);
			}
			return;
		}
		if (this.m_manager != null)
		{
			FacebookTaskCreateObject facebookTaskCreateObject = new FacebookTaskCreateObject();
			string text = "{\"app_id\":203227836537595,\"type\":\"testrunners:gamedata\",\"url\":\"http://samples.ogp.me/215083468685365\",\"title\":\"Sample GameData\",\"image\":\"https://fbstatic-a.akamaihd.net/images/devsite/attachment_blank.png\",data:{\"game_id\":\"" + this.m_gameId + "\"},\"description\":\"\"}";
			global::Debug.Log(text);
			facebookTaskCreateObject.Request("gamedata", text, new FacebookTaskCreateObject.TaskFinishedCallback(this.CreateObjectEndCallback));
			this.m_manager.RequestProcess(facebookTaskCreateObject);
		}
		this.m_isEndProcess = true;
	}

	protected override void OnUpdate()
	{
	}

	protected override bool OnIsEndProcess()
	{
		return this.m_isEndProcess;
	}

	protected override string OnGetTaskName()
	{
		return this.TaskName;
	}

	private void CreateObjectEndCallback(string idStr)
	{
		bool flag = true;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface == null)
		{
			flag = false;
		}
		if (flag)
		{
			SocialUserData myProfile = socialInterface.MyProfile;
			myProfile.CustomData.ObjectId = idStr;
			if (this.m_manager != null)
			{
				FacebookTaskCreateAction facebookTaskCreateAction = new FacebookTaskCreateAction();
				facebookTaskCreateAction.Request("store", "gamedata", idStr, new FacebookTaskCreateAction.TaskFinishedCallback(this.CreateActionEndCallback));
				this.m_manager.RequestProcess(facebookTaskCreateAction);
			}
		}
		else if (this.m_callbackObject != null)
		{
			MsgSocialNormalResponse value = new MsgSocialNormalResponse();
			this.m_callbackObject.SendMessage("CreateGameDataEndCallback", value, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void CreateActionEndCallback(string idStr)
	{
		bool flag = true;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface == null)
		{
			flag = false;
		}
		if (flag)
		{
			SocialUserData myProfile = socialInterface.MyProfile;
			myProfile.CustomData.ActionId = idStr;
		}
		if (this.m_callbackObject != null)
		{
			MsgSocialNormalResponse value = new MsgSocialNormalResponse();
			this.m_callbackObject.SendMessage("CreateGameDataEndCallback", value, SendMessageOptions.DontRequireReceiver);
		}
	}
}
