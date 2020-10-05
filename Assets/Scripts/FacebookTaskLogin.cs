using Facebook;
using Message;
using System;
using UnityEngine;

public class FacebookTaskLogin : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskLogin";

	private GameObject m_callbackObject;

	private FBResult m_fbResult;

	private bool m_isEndProcess;

	public FacebookTaskLogin()
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
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && !socialInterface.IsInitialized)
		{
			this.m_callbackObject.SendMessage("LoginEndCallback", null, SendMessageOptions.DontRequireReceiver);
			return;
		}
		string text = FacebookUtil.PermissionString[0];
		text += ",";
		text += FacebookUtil.PermissionString[1];
		FB.Login(text, new FacebookDelegate(this.LoginEndCallback));
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

	private void LoginEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		this.m_fbResult = fbResult;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.IsLoggedIn = FB.IsLoggedIn;
			if (socialInterface.IsLoggedIn)
			{
				FoxManager.SendLtvPoint(FoxLtvType.FacebookLogIn);
			}
		}
		global::Debug.Log("Facebook Access Token: " + FB.AccessToken);
		if (this.m_callbackObject == null)
		{
			return;
		}
		if (fbResult == null)
		{
			this.m_callbackObject.SendMessage("LoginEndCallback", null, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
			return;
		}
		if (fbResult.Text == null)
		{
			this.m_callbackObject.SendMessage("LoginEndCallback", null, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
			return;
		}
		MsgSocialNormalResponse msgSocialNormalResponse = new MsgSocialNormalResponse();
		msgSocialNormalResponse.m_result = new SocialResult
		{
			IsError = false,
			ResultId = 0,
			Result = this.m_fbResult.Error
		};
		this.m_callbackObject.SendMessage("LoginEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook Login is finished");
	}
}
