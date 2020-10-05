using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerUpdateMessageRetry : ServerRetryProcess
{
	public List<int> m_messageIdList;

	public List<int> m_operatorMessageIdList;

	public ServerUpdateMessageRetry(List<int> messageIdList, List<int> operatorMessageIdList, GameObject callbackObject) : base(callbackObject)
	{
		this.m_messageIdList = messageIdList;
		this.m_operatorMessageIdList = operatorMessageIdList;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerUpdateMessage(this.m_messageIdList, this.m_operatorMessageIdList, this.m_callbackObject);
		}
	}
}
