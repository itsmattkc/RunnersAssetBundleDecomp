using System;
using UnityEngine;

public class ServerBuyIosRetry : ServerRetryProcess
{
	public string m_receiptData;

	public ServerBuyIosRetry(string receiptData, GameObject callbackObject) : base(callbackObject)
	{
		this.m_receiptData = receiptData;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerBuyIos(this.m_receiptData, this.m_callbackObject);
		}
	}
}
