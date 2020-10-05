using System;
using UnityEngine;

public class ServerBuyAndroidRetry : ServerRetryProcess
{
	public string m_receiptData;

	public string m_signature;

	public ServerBuyAndroidRetry(string receiptData, string signature, GameObject callbackObject) : base(callbackObject)
	{
		this.m_receiptData = receiptData;
		this.m_signature = signature;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerBuyAndroid(this.m_receiptData, this.m_signature, this.m_callbackObject);
		}
	}
}
