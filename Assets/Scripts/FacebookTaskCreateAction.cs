using Facebook;
using LitJson;
using System;
using System.Collections.Generic;

public class FacebookTaskCreateAction : SocialTaskBase
{
	public delegate void TaskFinishedCallback(string actionId);

	private readonly string TaskName = "FacebookTaskCreateAction";

	private bool m_isEndProcess;

	private string m_actionName;

	private string m_postObjectName;

	private string m_postObjectId;

	private FacebookTaskCreateAction.TaskFinishedCallback m_callback;

	public FacebookTaskCreateAction()
	{
		this.m_isEndProcess = false;
	}

	public void Request(string actionName, string postObjectName, string postObjectId, FacebookTaskCreateAction.TaskFinishedCallback callback)
	{
		this.m_actionName = actionName;
		this.m_postObjectName = postObjectName;
		this.m_postObjectId = postObjectId;
		this.m_callback = callback;
	}

	protected override void OnStartProcess()
	{
		string query = FacebookUtil.FBVersionString + "me/testrunners:" + this.m_actionName;
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{
				this.m_postObjectName,
				this.m_postObjectId
			}
		};
		dictionary.Add("no_feed_story", "1");
		FB.API(query, HttpMethod.POST, new FacebookDelegate(this.CreateActionEndCallback), dictionary);
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

	private void CreateActionEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (fbResult == null)
		{
			return;
		}
		UnityEngine.Debug.Log("Facebook.CreateAction:" + fbResult.Text);
		string text = fbResult.Text;
		if (text == null)
		{
			return;
		}
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData == null)
		{
			UnityEngine.Debug.Log("Failed transform plainText to Json");
			return;
		}
		string jsonString = NetUtil.GetJsonString(jsonData, "id");
		this.m_callback(jsonString);
	}
}
