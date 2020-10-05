using System;
using UnityEngine;

public abstract class ServerRetryProcess
{
	protected GameObject m_callbackObject;

	public ServerRetryProcess(GameObject callbackObject)
	{
		this.m_callbackObject = callbackObject;
	}

	public abstract void Retry();
}
