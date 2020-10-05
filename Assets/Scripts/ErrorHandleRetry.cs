using Message;
using System;
using Text;
using UnityEngine;

public class ErrorHandleRetry : ErrorHandleBase
{
	private ServerRetryProcess m_retryProcess;

	private GameObject m_callbackObject;

	private string m_callbackFuncName;

	private MessageBase m_msg;

	private bool m_isEnd;

	public void SetRetryProcess(ServerRetryProcess retryProcess)
	{
		this.m_retryProcess = retryProcess;
	}

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
		this.m_callbackObject = callbackObject;
		this.m_callbackFuncName = callbackFuncName;
		this.m_msg = msg;
	}

	public override void StartErrorHandle()
	{
		if (this.m_msg == null)
		{
			this.m_isEnd = true;
			return;
		}
		if (this.m_msg.ID == 61517)
		{
			MsgServerConnctFailed msgServerConnctFailed = this.m_msg as MsgServerConnctFailed;
			if (msgServerConnctFailed != null)
			{
				string message = string.Empty;
				ServerInterface.StatusCode status = msgServerConnctFailed.m_status;
				switch (status + 10)
				{
				case ServerInterface.StatusCode.Ok:
				case (ServerInterface.StatusCode)3:
					message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_reload").text;
					goto IL_B4;
				case (ServerInterface.StatusCode)1:
				case (ServerInterface.StatusCode)2:
					IL_62:
					if (status != ServerInterface.StatusCode.ExpirationSession)
					{
						message = string.Empty;
						goto IL_B4;
					}
					message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_timeout").text;
					goto IL_B4;
				}
				goto IL_62;
				IL_B4:
				NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
				{
					name = "NetworkErrorReload",
					buttonType = NetworkErrorWindow.ButtonType.Ok,
					anchor_path = string.Empty,
					caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
					message = message,
					finishedCloseDelegate = new NetworkErrorWindow.CInfo.FinishedCloseDelegate(this.OnFinishCloseAnimCallback)
				});
			}
		}
		else if (this.m_msg.ID == 61520)
		{
			NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
			{
				name = "NetworkErrorReload",
				buttonType = NetworkErrorWindow.ButtonType.Ok,
				anchor_path = string.Empty,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_reload").text,
				finishedCloseDelegate = new NetworkErrorWindow.CInfo.FinishedCloseDelegate(this.OnFinishCloseAnimCallback)
			});
		}
		else
		{
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
		NetworkErrorWindow.Close();
		if (this.m_retryProcess != null)
		{
			this.m_retryProcess.Retry();
		}
	}

	private void OnFinishCloseAnimCallback()
	{
		if (this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage(this.m_callbackFuncName, this.m_msg, SendMessageOptions.DontRequireReceiver);
		}
		this.m_isEnd = true;
	}
}
