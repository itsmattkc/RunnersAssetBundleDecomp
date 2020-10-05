using Facebook;
using System;

public class FacebookTaskDeleteObject : SocialTaskBase
{
	public delegate void TaskFinishedCallback();

	private readonly string TaskName = "FacebookTaskDeleteObject";

	private bool m_isEndProcess;

	private string m_actionName;

	private string m_postObjectName;

	private string m_postObjectId;

	private FacebookTaskDeleteObject.TaskFinishedCallback m_callback;

	public FacebookTaskDeleteObject()
	{
		this.m_isEndProcess = false;
	}

	public void Request(FacebookTaskDeleteObject.TaskFinishedCallback callback)
	{
		this.m_callback = callback;
	}

	protected override void OnStartProcess()
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface == null)
		{
			return;
		}
		SocialUserData myProfile = socialInterface.MyProfile;
		string objectId = myProfile.CustomData.ObjectId;
		string query = FacebookUtil.FBVersionString + objectId;
		FB.API(query, HttpMethod.DELETE, new FacebookDelegate(this.DeleteObjectEndCallback), null);
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

	private void DeleteObjectEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (fbResult == null)
		{
			return;
		}
		UnityEngine.Debug.Log("Facebook.DeleteObject:" + fbResult.Text);
		this.m_callback();
	}
}
