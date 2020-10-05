using System;
using UnityEngine;

public class ServerSetBirthdayRetry : ServerRetryProcess
{
	public string m_birthday;

	public ServerSetBirthdayRetry(string birthday, GameObject callbackObject) : base(callbackObject)
	{
		this.m_birthday = birthday;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSetBirthday(this.m_birthday, this.m_callbackObject);
		}
	}
}
