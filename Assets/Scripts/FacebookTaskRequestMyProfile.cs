using Facebook;
using LitJson;
using Message;
using System;
using UnityEngine;

public class FacebookTaskRequestMyProfile : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskRequestMyProfile";

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskRequestMyProfile()
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
		string query = FacebookUtil.FBVersionString + "me?fields=id,picture,name";
		FB.API(query, HttpMethod.GET, new FacebookDelegate(this.RequestMyProfileEndCallback), null);
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

	private void RequestMyProfileEndCallback(FBResult fbResult)
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
		string text = fbResult.Text;
		global::Debug.Log("FacebookTaskRequestMyProfile.responseText = " + text);
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData == null)
		{
			global::Debug.Log("Failed transform plainText to Json");
			return;
		}
		bool flag = false;
		SocialUserData userData = FacebookUtil.GetUserData(jsonData, ref flag);
		if (userData == null)
		{
			return;
		}
		SocialResult socialResult = new SocialResult();
		socialResult.ResultId = 0;
		socialResult.Result = fbResult.Text;
		socialResult.IsError = false;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.MyProfile = userData;
		}
		MsgSocialMyProfileResponse msgSocialMyProfileResponse = new MsgSocialMyProfileResponse();
		msgSocialMyProfileResponse.m_result = socialResult;
		msgSocialMyProfileResponse.m_profile = userData;
		this.m_callbackObject.SendMessage("RequestMyProfileEndCallback", msgSocialMyProfileResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook Request My Profile is finished");
	}
}
