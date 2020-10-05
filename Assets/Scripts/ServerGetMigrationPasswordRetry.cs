using System;
using UnityEngine;

public class ServerGetMigrationPasswordRetry : ServerRetryProcess
{
	public string m_userPassword;

	public ServerGetMigrationPasswordRetry(string userPassword, GameObject callbackObject) : base(callbackObject)
	{
		this.m_userPassword = userPassword;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetMigrationPassword(this.m_userPassword, this.m_callbackObject);
		}
	}
}
