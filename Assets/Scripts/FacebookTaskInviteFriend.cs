using Facebook;
using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class FacebookTaskInviteFriend : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskInviteFriend";

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskInviteFriend()
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
			return;
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface == null)
		{
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage("InviteFriendEndCallback", null, SendMessageOptions.DontRequireReceiver);
				this.m_callbackObject = null;
			}
			return;
		}
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "SnsFeed", "gw_invite_friend_caption").text;
		string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "SnsFeed", "gw_invite_friend_text").text;
		SocialUserData myProfile = socialInterface.MyProfile;
		FB.AppRequest(text2, null, new List<object>
		{
			"app_non_users"
		}, null, null, myProfile.CustomData.GameId, text, new FacebookDelegate(this.InviteFriendEndCallback));
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

	private void InviteFriendEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (fbResult == null)
		{
			return;
		}
		if (this.m_callbackObject == null)
		{
			return;
		}
		SocialResult socialResult = new SocialResult();
		socialResult.ResultId = 0;
		socialResult.Result = fbResult.Text;
		socialResult.IsError = false;
		MsgSocialNormalResponse msgSocialNormalResponse = new MsgSocialNormalResponse();
		msgSocialNormalResponse.m_result = socialResult;
		this.m_callbackObject.SendMessage("InviteFriendEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook AppRequest is finished");
	}
}
