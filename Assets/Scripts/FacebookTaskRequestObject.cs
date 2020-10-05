using Facebook;
using System;

public class FacebookTaskRequestObject : SocialTaskBase
{
	public delegate void TaskFinishedCallback(string responseText);

	private readonly string TaskName = "FacebookTaskRequestObject";

	private bool m_isEndProcess;

	private string m_objectId;

	private FacebookTaskRequestObject.TaskFinishedCallback m_callback;

	public FacebookTaskRequestObject()
	{
		this.m_isEndProcess = false;
	}

	public void Request(string objectId, FacebookTaskRequestObject.TaskFinishedCallback callback)
	{
		this.m_objectId = objectId;
		this.m_callback = callback;
	}

	protected override void OnStartProcess()
	{
		string query = FacebookUtil.FBVersionString + this.m_objectId;
		FB.API(query, HttpMethod.GET, new FacebookDelegate(this.RequestObjectEndCallback), null);
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

	private void RequestObjectEndCallback(FBResult fbResult)
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
		UnityEngine.Debug.Log("Facebook.GetAction:" + fbResult.Text);
		this.m_callback(fbResult.Text);
	}
}
