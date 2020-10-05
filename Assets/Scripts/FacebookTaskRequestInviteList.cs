using Facebook;
using Message;
using System;
using UnityEngine;

public class FacebookTaskRequestInviteList : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskRequestInviteList";

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskRequestInviteList()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(GameObject callbackObject)
	{
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		if (!FacebookUtil.IsLoggedIn())
		{
			this.m_isEndProcess = true;
		}
		string query = FacebookUtil.FBVersionString + "me/apprequests";
		FB.API(query, HttpMethod.GET, new FacebookDelegate(this.RequestInviteListEndCallback), null);
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

	private void RequestInviteListEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		bool flag = true;
		string text = string.Empty;
		if (fbResult == null)
		{
			flag = false;
		}
		else
		{
			text = fbResult.Text;
		}
		if (flag)
		{
			SocialResult socialResult = new SocialResult();
			socialResult.ResultId = 0;
			socialResult.Result = fbResult.Text;
			socialResult.IsError = false;
			MsgSocialFriendListResponse msgSocialFriendListResponse = new MsgSocialFriendListResponse();
			msgSocialFriendListResponse.m_result = socialResult;
			msgSocialFriendListResponse.m_friends = FacebookUtil.GetInvitedFriendList(text);
			SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface != null)
			{
				socialInterface.InvitedFriendList = msgSocialFriendListResponse.m_friends;
			}
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage("RequestInviteListEndCallback", msgSocialFriendListResponse, SendMessageOptions.DontRequireReceiver);
				this.m_callbackObject = null;
			}
		}
		else if (this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage("RequestInviteListEndCallback", null, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
		}
		global::Debug.Log("Facebook AppRequest is finished");
	}
}
