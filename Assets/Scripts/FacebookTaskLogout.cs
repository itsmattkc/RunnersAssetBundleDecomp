using System;

public class FacebookTaskLogout : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskLogout";

	private bool m_isEndProcess;

	public FacebookTaskLogout()
	{
		this.m_isEndProcess = false;
	}

	public void Request()
	{
	}

	protected override void OnStartProcess()
	{
		if (!FacebookUtil.IsLoggedIn())
		{
			this.m_isEndProcess = true;
			return;
		}
		FB.Logout();
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.IsLoggedIn = false;
		}
		this.m_isEndProcess = true;
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
}
