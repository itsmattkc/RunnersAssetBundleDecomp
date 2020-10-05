using System;
using UnityEngine;

public class ServerSendApolloRetry : ServerRetryProcess
{
	public int m_type;

	public string[] m_value;

	public ServerSendApolloRetry(int type, string[] value, GameObject callbackObject) : base(callbackObject)
	{
		this.m_type = type;
		this.m_value = value;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSendApollo(this.m_type, this.m_value, this.m_callbackObject);
		}
	}
}
