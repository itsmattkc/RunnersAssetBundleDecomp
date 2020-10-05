using Message;
using System;
using UnityEngine;

public class FacebookTaskDeleteGameData : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskDeleteGameData";

	private SocialTaskManager m_manager;

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskDeleteGameData()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(SocialTaskManager manager, GameObject callbackObject)
	{
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
				this.m_callbackObject.SendMessage("DeleteGameDataEndCallback", value, SendMessageOptions.DontRequireReceiver);
			}
			return;
		}
		if (this.m_manager != null)
		{
			FacebookTaskDeleteObject facebookTaskDeleteObject = new FacebookTaskDeleteObject();
			facebookTaskDeleteObject.Request(new FacebookTaskDeleteObject.TaskFinishedCallback(this.DeleteObjectEndCallback));
			this.m_manager.RequestProcess(facebookTaskDeleteObject);
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

	private void DeleteObjectEndCallback()
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
			myProfile.CustomData.ObjectId = string.Empty;
			myProfile.CustomData.ActionId = string.Empty;
			if (this.m_callbackObject != null)
			{
				MsgSocialNormalResponse value = new MsgSocialNormalResponse();
				this.m_callbackObject.SendMessage("DeleteGameDataEndCallback", value, SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (this.m_callbackObject != null)
		{
			MsgSocialNormalResponse value2 = new MsgSocialNormalResponse();
			this.m_callbackObject.SendMessage("DeleteGameDataEndCallback", value2, SendMessageOptions.DontRequireReceiver);
		}
	}
}
