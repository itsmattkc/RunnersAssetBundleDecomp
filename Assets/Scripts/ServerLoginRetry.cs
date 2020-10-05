using System;
using UnityEngine;

public class ServerLoginRetry : ServerRetryProcess
{
	public string m_userId;

	public string m_password;

	public ServerLoginRetry(string userId, string password, GameObject callbackObject) : base(callbackObject)
	{
		this.m_userId = userId;
		this.m_password = password;
	}

	public override void Retry()
	{
		ServerInterface serverInterface = GameObjectUtil.FindGameObjectComponent<ServerInterface>("ServerInterface");
		if (serverInterface != null)
		{
			serverInterface.RequestServerLogin(this.m_userId, this.m_password, this.m_callbackObject);
		}
	}
}
