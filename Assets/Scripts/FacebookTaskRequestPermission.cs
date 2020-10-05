using Facebook;
using Message;
using System;
using UnityEngine;

public class FacebookTaskRequestPermission : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskRequestPermission";

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskRequestPermission()
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
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage("RequestPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
				this.m_callbackObject = null;
			}
			this.m_isEndProcess = true;
			return;
		}
		string query = FacebookUtil.FBVersionString + "me/permissions";
		FB.API(query, HttpMethod.GET, new FacebookDelegate(this.RequestPermissionEndCallback), null);
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

	private void RequestPermissionEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (fbResult == null)
		{
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage("RequestPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
				this.m_callbackObject = null;
			}
			return;
		}
		if (fbResult.Text == null)
		{
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage("RequestPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
				this.m_callbackObject = null;
			}
			return;
		}
		if (this.m_callbackObject == null)
		{
			return;
		}
		string text = fbResult.Text;
		FacebookUtil.UpdatePermissionInfo(text);
		MsgSocialNormalResponse msgSocialNormalResponse = new MsgSocialNormalResponse();
		msgSocialNormalResponse.m_result = new SocialResult
		{
			ResultId = 0,
			Result = fbResult.Text,
			IsError = false
		};
		this.m_callbackObject.SendMessage("RequestPermissionEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook Request Permission is finished");
	}
}
