using Message;
using System;
using Text;
using UnityEngine;

public class FacebookTaskSendEnergy : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskSendEnergy";

	private SocialUserData m_userData;

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskSendEnergy()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(SocialUserData userData, GameObject callbackObject)
	{
		this.m_userData = userData;
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		bool flag = true;
		if (!FacebookUtil.IsLoggedIn())
		{
			flag = false;
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && !socialInterface.IsGrantedPermission[1])
		{
			flag = false;
		}
		if (!flag)
		{
			this.m_isEndProcess = true;
			MsgSocialNormalResponse msgSocialNormalResponse = new MsgSocialNormalResponse();
			msgSocialNormalResponse.m_result = null;
			this.m_callbackObject.SendMessage("SendEnergyEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
			return;
		}
		if (this.m_userData == null)
		{
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage("SendEnergyEndCallback", null, SendMessageOptions.DontRequireReceiver);
			}
			return;
		}
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "SnsFeed", "gw_send_challenge_caption").text;
		string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "SnsFeed", "gw_send_challenge_text").text;
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

	private void SendEnergyEndCallback(FBResult fbResult)
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
		this.m_callbackObject.SendMessage("SendEnergyEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook sendEnergy is finished");
	}
}
