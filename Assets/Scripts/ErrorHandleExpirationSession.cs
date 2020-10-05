using Message;
using System;
using Text;
using UnityEngine;

public class ErrorHandleExpirationSession : ErrorHandleBase
{
	private enum State
	{
		NONE = -1,
		IDLE,
		VALIDATING,
		SUCCESS,
		FAILED,
		COUNT
	}

	private bool m_isEnd;

	private ServerRetryProcess m_retryProcess;

	private ServerSessionWatcher.ValidateType m_validateType;

	private ErrorHandleExpirationSession.State m_state;

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
	}

	public void SetRetryProcess(ServerRetryProcess retryProcess)
	{
		this.m_retryProcess = retryProcess;
	}

	public void SetSessionValidateType(ServerSessionWatcher.ValidateType validateType)
	{
		this.m_validateType = validateType;
	}

	public override void StartErrorHandle()
	{
		if (this.m_state != ErrorHandleExpirationSession.State.IDLE)
		{
			return;
		}
		this.m_state = ErrorHandleExpirationSession.State.VALIDATING;
		ServerSessionWatcher serverSessionWatcher = GameObjectUtil.FindGameObjectComponent<ServerSessionWatcher>("NetMonitor");
		if (serverSessionWatcher != null)
		{
			serverSessionWatcher.ValidateSession(this.m_validateType, new ServerSessionWatcher.ValidateSessionEndCallback(this.ValidateSessionCallback));
		}
	}

	public override void Update()
	{
		if (this.m_state == ErrorHandleExpirationSession.State.FAILED && NetworkErrorWindow.IsOkButtonPressed)
		{
			NetworkErrorWindow.Close();
			HudMenuUtility.GoToTitleScene();
			this.m_isEnd = true;
		}
	}

	public override bool IsEnd()
	{
		return this.m_isEnd;
	}

	public override void EndErrorHandle()
	{
		if (this.m_state == ErrorHandleExpirationSession.State.SUCCESS && this.m_retryProcess != null)
		{
			this.m_retryProcess.Retry();
		}
	}

	private void ValidateSessionCallback(bool isSuccess)
	{
		if (isSuccess)
		{
			this.m_state = ErrorHandleExpirationSession.State.SUCCESS;
			this.m_isEnd = true;
		}
		else
		{
			NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
			{
				buttonType = NetworkErrorWindow.ButtonType.Ok,
				anchor_path = string.Empty,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_session_timeout_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_session_timeout_text").text
			});
			this.m_state = ErrorHandleExpirationSession.State.FAILED;
		}
	}
}
