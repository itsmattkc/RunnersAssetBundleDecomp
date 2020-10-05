using Message;
using System;
using UnityEngine;

public class ErrorHandleSecurityError : ErrorHandleBase
{
	private bool m_isEnd;

	private ServerRetryProcess m_retryProcess;

	public void SetRetryProcess(ServerRetryProcess retryProcess)
	{
		this.m_retryProcess = retryProcess;
	}

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
	}

	public override void StartErrorHandle()
	{
		ServerSessionWatcher serverSessionWatcher = GameObjectUtil.FindGameObjectComponent<ServerSessionWatcher>("NetMonitor");
		if (serverSessionWatcher != null)
		{
			serverSessionWatcher.InvalidateSession();
			this.m_isEnd = true;
		}
	}

	public override void Update()
	{
	}

	public override bool IsEnd()
	{
		return this.m_isEnd;
	}

	public override void EndErrorHandle()
	{
		if (this.m_retryProcess != null)
		{
			this.m_retryProcess.Retry();
		}
	}

	private void ValidateSessionEndCallback(bool isSuccess)
	{
		this.m_isEnd = true;
	}
}
