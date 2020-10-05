using Facebook;
using LitJson;
using System;
using System.Collections.Generic;

public class FacebookTaskCreateObject : SocialTaskBase
{
	public delegate void TaskFinishedCallback(string objectId);

	private readonly string TaskName = "FacebookTaskCreateObject";

	private bool m_isEndProcess;

	private string m_objectName;

	private string m_jSonString;

	private FacebookTaskCreateObject.TaskFinishedCallback m_callback;

	public FacebookTaskCreateObject()
	{
		this.m_isEndProcess = false;
	}

	public void Request(string objectName, string jSonString, FacebookTaskCreateObject.TaskFinishedCallback callback)
	{
		this.m_objectName = objectName;
		this.m_jSonString = jSonString;
		this.m_callback = callback;
	}

	protected override void OnStartProcess()
	{
		string query = FacebookUtil.FBVersionString + "me/objects/testrunners:" + this.m_objectName;
		Dictionary<string, string> formData = new Dictionary<string, string>
		{
			{
				"object",
				this.m_jSonString
			}
		};
		FB.API(query, HttpMethod.POST, new FacebookDelegate(this.CreateObjectEndCallback), formData);
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

	private void CreateObjectEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (fbResult == null)
		{
			this.m_callback(null);
			return;
		}
		if (fbResult.Text == null)
		{
			this.m_callback(null);
			return;
		}
		UnityEngine.Debug.Log("Facebook.CreateObject:" + fbResult.Text);
		string text = fbResult.Text;
		if (text == null)
		{
			return;
		}
		UnityEngine.Debug.Log("Facebook.CreateObject:" + text);
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
