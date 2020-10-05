using Facebook;
using Message;
using System;
using UnityEngine;

public class FacebookTaskInitialize : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskInitialize";

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	private static bool m_isAlreadyInit;

	public FacebookTaskInitialize()
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
		if (FacebookTaskInitialize.m_isAlreadyInit)
		{
			this.InitEndCallback();
			return;
		}
		FB.Init(new InitDelegate(this.InitEndCallback), null, null);
		FacebookTaskInitialize.m_isAlreadyInit = true;
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

	private void InitEndCallback()
	{
		this.m_isEndProcess = true;
		global::Debug.Log("FacebookInitialize:Facebook login is " + FB.IsLoggedIn.ToString());
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.IsLoggedIn = FB.IsLoggedIn;
			socialInterface.IsInitialized = true;
		}
		if (this.m_callbackObject == null)
		{
			return;
		}
		SocialResult socialResult = new SocialResult();
		socialResult.IsError = false;
		socialResult.ResultId = 0;
		socialResult.Result = string.Empty;
		MsgSocialNormalResponse msgSocialNormalResponse = new MsgSocialNormalResponse();
		msgSocialNormalResponse.m_result = socialResult;
		this.m_callbackObject.SendMessage("InitEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
	}
}
