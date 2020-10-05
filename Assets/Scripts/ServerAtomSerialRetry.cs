using System;
using UnityEngine;

public class ServerAtomSerialRetry : ServerRetryProcess
{
	public string m_campaignId;

	public string m_serial;

	public bool m_new_user;

	public ServerAtomSerialRetry(string campaignId, string serial, bool new_user, GameObject callbackObject) : base(callbackObject)
	{
		this.m_campaignId = campaignId;
		this.m_serial = serial;
		this.m_new_user = new_user;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerAtomSerial(this.m_campaignId, this.m_serial, this.m_new_user, this.m_callbackObject);
		}
	}
}
