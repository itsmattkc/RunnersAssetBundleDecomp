using System;
using UnityEngine;

public class ServerGetMileageDataRetry : ServerRetryProcess
{
	public string[] m_distanceFriendList;

	public ServerGetMileageDataRetry(string[] distanceFriendList, GameObject callbackObject) : base(callbackObject)
	{
		this.m_distanceFriendList = distanceFriendList;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetMileageData(this.m_distanceFriendList, this.m_callbackObject);
		}
	}
}
