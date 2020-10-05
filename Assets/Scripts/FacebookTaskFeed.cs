using DataTable;
using Facebook;
using Message;
using System;
using UnityEngine;

public class FacebookTaskFeed : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskFeed";

	private string m_feedCaption;

	private string m_feedText;

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskFeed()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(string feedCaption, string feedText, GameObject callbackObject)
	{
		this.m_feedCaption = feedCaption;
		this.m_feedText = feedText;
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		if (!FacebookUtil.IsLoggedIn())
		{
			this.m_isEndProcess = true;
			return;
		}
		string picture = string.Empty;
		picture = InformationDataTable.GetUrl(InformationDataTable.Type.FB_FEED_PICTURE_ANDROID);
		FB.Feed(string.Empty, "http://sonicrunners.sega-net.com/upredirect/index.html", "SonicRunners", this.m_feedCaption, this.m_feedText, picture, string.Empty, string.Empty, string.Empty, string.Empty, null, new FacebookDelegate(this.FeedEndCallback));
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

	private void FeedEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (fbResult == null)
		{
			global::Debug.Log("Facebook.Login:fbResult is null");
			return;
		}
		global::Debug.Log("Facebook.Login:response= " + fbResult.Text);
		if (this.m_callbackObject == null)
		{
			return;
		}
		SocialResult socialResult = new SocialResult();
		socialResult.ResultId = 0;
		socialResult.Result = fbResult.Text;
		if (socialResult.Result.IndexOf("cancelled") >= 0)
		{
			socialResult.IsError = true;
		}
		else
		{
			socialResult.IsError = false;
		}
		MsgSocialNormalResponse msgSocialNormalResponse = new MsgSocialNormalResponse();
		msgSocialNormalResponse.m_result = socialResult;
		this.m_callbackObject.SendMessage("FeedEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook Feed is finished");
	}
}
