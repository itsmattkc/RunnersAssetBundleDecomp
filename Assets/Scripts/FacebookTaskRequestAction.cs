using Facebook;
using System;

public class FacebookTaskRequestAction : SocialTaskBase
{
	public delegate void TaskFinishedCallback(string responseText);

	private readonly string TaskName = "FacebookTaskRequestAction";

	private bool m_isEndProcess;

	private string m_userId;

	private string m_actionName;

	private FacebookTaskRequestAction.TaskFinishedCallback m_callback;

	public FacebookTaskRequestAction()
	{
		this.m_isEndProcess = false;
	}

	public void Request(string userId, string actionName, FacebookTaskRequestAction.TaskFinishedCallback callback)
	{
		this.m_userId = userId;
		this.m_actionName = actionName;
		this.m_callback = callback;
	}

	protected override void OnStartProcess()
	{
		if (!FacebookUtil.IsLoggedIn())
		{
			this.m_isEndProcess = true;
			return;
		}
		string query = FacebookUtil.FBVersionString + this.m_userId + "/" + this.m_actionName;
		FB.API(query, HttpMethod.GET, new FacebookDelegate(this.RequestActionEndCallback), null);
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

	private void RequestActionEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (fbResult == null)
		{
			return;
		}
		UnityEngine.Debug.Log("Facebook.Object: " + fbResult.Text);
		this.m_callback(fbResult.Text);
	}
}
