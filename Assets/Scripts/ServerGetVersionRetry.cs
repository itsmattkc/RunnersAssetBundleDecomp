using System;
using UnityEngine;

public class ServerGetVersionRetry : ServerRetryProcess
{
	public ServerGetVersionRetry(GameObject callbackObject) : base(callbackObject)
	{
	}

	public override void Retry()
	{
		GameObject gameObject = GameObject.Find("ServerInterface");
		if (gameObject != null)
		{
			ServerInterface component = gameObject.GetComponent<ServerInterface>();
			if (component != null)
			{
				component.RequestServerGetVersion(this.m_callbackObject);
			}
		}
	}
}
