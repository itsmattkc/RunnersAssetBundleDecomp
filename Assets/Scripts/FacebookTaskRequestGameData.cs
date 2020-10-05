using LitJson;
using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookTaskRequestGameData : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskRequestGameData";

	private SocialTaskManager m_manager;

	private GameObject m_callbackObject;

	private SocialUserData m_targetUser;

	private bool m_isEndProcess;

	private string m_userId;

	public FacebookTaskRequestGameData()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(string userId, SocialTaskManager manager, GameObject callbackObject)
	{
		this.m_userId = userId;
		this.m_manager = manager;
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		List<SocialUserData> friendWithMeList = socialInterface.FriendWithMeList;
		foreach (SocialUserData current in friendWithMeList)
		{
			if (current != null)
			{
				if (current.Id == this.m_userId)
				{
					this.m_targetUser = current;
				}
			}
		}
		if (!FacebookUtil.IsLoggedIn())
		{
			this.m_isEndProcess = true;
			if (this.m_callbackObject != null)
			{
				MsgSocialCustomUserDataResponse msgSocialCustomUserDataResponse = new MsgSocialCustomUserDataResponse();
				msgSocialCustomUserDataResponse.m_isCreated = false;
				msgSocialCustomUserDataResponse.m_userData = this.m_targetUser;
				this.m_callbackObject.SendMessage("RequestGameDataEndCallback", msgSocialCustomUserDataResponse, SendMessageOptions.DontRequireReceiver);
			}
			return;
		}
		if (this.m_manager != null)
		{
			FacebookTaskRequestAction facebookTaskRequestAction = new FacebookTaskRequestAction();
			facebookTaskRequestAction.Request(this.m_userId, "testrunners:store", new FacebookTaskRequestAction.TaskFinishedCallback(this.RequestActionEndCallback));
			this.m_manager.RequestProcess(facebookTaskRequestAction);
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

	private void RequestActionEndCallback(string responseText)
	{
		string actionId = FacebookUtil.GetActionId(responseText);
		string objectIdFromAction = FacebookUtil.GetObjectIdFromAction(responseText, "gamedata");
		if (string.IsNullOrEmpty(actionId) || string.IsNullOrEmpty(objectIdFromAction))
		{
			MsgSocialCustomUserDataResponse msgSocialCustomUserDataResponse = new MsgSocialCustomUserDataResponse();
			msgSocialCustomUserDataResponse.m_isCreated = false;
			msgSocialCustomUserDataResponse.m_userData = this.m_targetUser;
			this.m_callbackObject.SendMessage("RequestGameDataEndCallback", msgSocialCustomUserDataResponse, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			this.m_targetUser.CustomData.ActionId = actionId;
			this.m_targetUser.CustomData.ObjectId = objectIdFromAction;
			if (this.m_manager != null)
			{
				FacebookTaskRequestObject facebookTaskRequestObject = new FacebookTaskRequestObject();
				facebookTaskRequestObject.Request(objectIdFromAction, new FacebookTaskRequestObject.TaskFinishedCallback(this.RequestObjectEndCallback));
				this.m_manager.RequestProcess(facebookTaskRequestObject);
			}
		}
	}

	private void RequestObjectEndCallback(string responseText)
	{
		bool flag = true;
		JsonData jsonData = JsonMapper.ToObject(responseText);
		if (jsonData == null)
		{
			global::Debug.Log("Failed transform plainText to Json");
			flag = false;
		}
		if (flag && NetUtil.GetJsonObject(jsonData, "data") == null)
		{
			global::Debug.Log("Not found object in json data");
			flag = false;
		}
		SocialInterface x = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (x == null)
		{
			flag = false;
		}
		if (flag)
		{
			if (this.m_targetUser != null)
			{
			}
			if (this.m_callbackObject != null)
			{
				MsgSocialCustomUserDataResponse msgSocialCustomUserDataResponse = new MsgSocialCustomUserDataResponse();
				msgSocialCustomUserDataResponse.m_isCreated = true;
				msgSocialCustomUserDataResponse.m_userData = this.m_targetUser;
				this.m_callbackObject.SendMessage("RequestGameDataEndCallback", msgSocialCustomUserDataResponse, SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (this.m_callbackObject != null)
		{
			MsgSocialCustomUserDataResponse msgSocialCustomUserDataResponse2 = new MsgSocialCustomUserDataResponse();
			msgSocialCustomUserDataResponse2.m_isCreated = false;
			msgSocialCustomUserDataResponse2.m_userData = this.m_targetUser;
			this.m_callbackObject.SendMessage("RequestGameDataEndCallback", msgSocialCustomUserDataResponse2, SendMessageOptions.DontRequireReceiver);
		}
	}
}
