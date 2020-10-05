using Facebook;
using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookTaskAddPermission : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskAddPermission";

	private GameObject m_callbackObject;

	private List<SocialInterface.Permission> m_permissions;

	private FBResult m_fbResult;

	private bool m_isEndProcess;

	public FacebookTaskAddPermission()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(List<SocialInterface.Permission> permissions, GameObject callbackObject)
	{
		this.m_permissions = permissions;
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		if (!FacebookUtil.IsLoggedIn())
		{
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage("AddPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
				this.m_callbackObject = null;
			}
			this.m_isEndProcess = true;
			return;
		}
		if (this.m_permissions == null && this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage("AddPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
		}
		int count = this.m_permissions.Count;
		if (count <= 0 && this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage("AddPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
		}
		string text = string.Empty;
		for (int i = 0; i < count; i++)
		{
			if (i > 0)
			{
				text += ",";
			}
			SocialInterface.Permission permission = this.m_permissions[i];
			text += FacebookUtil.PermissionString[(int)permission];
		}
		FB.Login(text, new FacebookDelegate(this.AddPermissionEndCallback));
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

	private void AddPermissionEndCallback(FBResult fbResult)
	{
		this.m_fbResult = fbResult;
		string query = FacebookUtil.FBVersionString + "me/permissions";
		FB.API(query, HttpMethod.GET, new FacebookDelegate(this.RequestPermissionEndCallback), null);
	}

	private void RequestPermissionEndCallback(FBResult fbResult)
	{
		if (this.m_callbackObject == null)
		{
			return;
		}
		if (fbResult == null)
		{
			this.m_callbackObject.SendMessage("AddPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
			return;
		}
		if (fbResult.Text == null)
		{
			this.m_callbackObject.SendMessage("AddPermissionEndCallback", null, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
			return;
		}
		string text = fbResult.Text;
		FacebookUtil.UpdatePermissionInfo(text);
		this.m_isEndProcess = true;
		MsgSocialNormalResponse msgSocialNormalResponse = new MsgSocialNormalResponse();
		msgSocialNormalResponse.m_result = new SocialResult
		{
			ResultId = 0,
			Result = this.m_fbResult.Text,
			IsError = false
		};
		this.m_callbackObject.SendMessage("AddPermissionEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook Add Permission is finished");
	}
}
