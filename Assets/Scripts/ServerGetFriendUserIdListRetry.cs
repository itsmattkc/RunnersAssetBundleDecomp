using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerGetFriendUserIdListRetry : ServerRetryProcess
{
	public List<string> m_friendFBIdList;

	public ServerGetFriendUserIdListRetry(List<string> friendFBIdList, GameObject callbackObject) : base(callbackObject)
	{
		this.m_friendFBIdList = friendFBIdList;
	}

	public override void Retry()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetFriendUserIdList(this.m_friendFBIdList, this.m_callbackObject);
		}
	}
}
