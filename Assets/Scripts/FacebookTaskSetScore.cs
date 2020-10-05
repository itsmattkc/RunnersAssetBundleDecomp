using Facebook;
using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookTaskSetScore : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskSetScore";

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	private int m_score;

	public FacebookTaskSetScore()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(SocialDefine.ScoreType type, int score, GameObject callbackObject)
	{
		this.m_score = score;
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		if (!FacebookUtil.IsLoggedIn())
		{
			this.m_isEndProcess = true;
			return;
		}
		string query = FacebookUtil.FBVersionString + "me/scores)";
		Dictionary<string, string> formData = new Dictionary<string, string>
		{
			{
				"score",
				this.m_score.ToString()
			}
		};
		FB.API(query, HttpMethod.POST, new FacebookDelegate(this.SetScoreEndCallback), formData);
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

	private void SetScoreEndCallback(FBResult fbResult)
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
		this.m_callbackObject.SendMessage("SetScoreEndCallback", msgSocialNormalResponse, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
	}
}
