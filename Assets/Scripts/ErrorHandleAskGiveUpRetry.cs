using Message;
using System;
using Text;
using UnityEngine;

public class ErrorHandleAskGiveUpRetry : ErrorHandleBase
{
	private enum State
	{
		NONE = -1,
		INIT,
		ASK_GIVEUP,
		CONFIRMATION,
		END,
		COUNT
	}

	private ErrorHandleAskGiveUpRetry.State m_state = ErrorHandleAskGiveUpRetry.State.NONE;

	private ServerRetryProcess m_retryProcess;

	private GameObject m_callbackObject;

	private string m_callbackFuncName;

	private MessageBase m_msg;

	private bool m_isRetry;

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
			this.m_state = ErrorHandleAskGiveUpRetry.State.END;
			return;
		}
		this.m_isRetry = false;
		this.m_state = ErrorHandleAskGiveUpRetry.State.INIT;
	}

	public override void Update()
	{
		switch (this.m_state)
		{
		case ErrorHandleAskGiveUpRetry.State.INIT:
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
						message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_ask_to_giveup_retry").text;
						goto IL_9E;
					}
					message = string.Empty;
					IL_9E:
					NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
					{
						name = "NetworkErrorRetry",
						buttonType = NetworkErrorWindow.ButtonType.YesNo,
						anchor_path = string.Empty,
						caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
						finishedCloseDelegate = new NetworkErrorWindow.CInfo.FinishedCloseDelegate(this.OnFinishedCloseAnimCallback),
						message = message
					});
				}
				this.m_state = ErrorHandleAskGiveUpRetry.State.ASK_GIVEUP;
			}
			else if (this.m_msg.ID == 61520)
			{
				NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
				{
					name = "NetworkErrorRetry",
					buttonType = NetworkErrorWindow.ButtonType.YesNo,
					anchor_path = string.Empty,
					caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
					message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_ask_to_giveup_retry").text,
					finishedCloseDelegate = new NetworkErrorWindow.CInfo.FinishedCloseDelegate(this.OnFinishedCloseAnimCallback)
				});
				this.m_state = ErrorHandleAskGiveUpRetry.State.ASK_GIVEUP;
			}
			else
			{
				this.m_state = ErrorHandleAskGiveUpRetry.State.END;
			}
			break;
		case ErrorHandleAskGiveUpRetry.State.ASK_GIVEUP:
			if (!NetworkErrorWindow.IsYesButtonPressed)
			{
				if (NetworkErrorWindow.IsNoButtonPressed)
				{
					NetworkErrorWindow.Close();
					NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
					{
						buttonType = NetworkErrorWindow.ButtonType.YesNo,
						anchor_path = string.Empty,
						caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
						message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_giveup_confirmation").text
					});
					this.m_state = ErrorHandleAskGiveUpRetry.State.CONFIRMATION;
				}
			}
			break;
		case ErrorHandleAskGiveUpRetry.State.CONFIRMATION:
			if (NetworkErrorWindow.IsYesButtonPressed)
			{
				NetworkErrorWindow.Close();
				if (this.m_callbackObject != null)
				{
					this.m_callbackObject.SendMessage(this.m_callbackFuncName, this.m_msg, SendMessageOptions.DontRequireReceiver);
				}
				GameObjectUtil.SendMessageFindGameObject("NetMonitor", "OnResetConnectFailedCount", null, SendMessageOptions.DontRequireReceiver);
				HudMenuUtility.GoToTitleScene();
				this.m_state = ErrorHandleAskGiveUpRetry.State.END;
			}
			else if (NetworkErrorWindow.IsNoButtonPressed)
			{
				NetworkErrorWindow.Close();
				this.m_state = ErrorHandleAskGiveUpRetry.State.INIT;
			}
			break;
		}
	}

	public override bool IsEnd()
	{
		return this.m_state == ErrorHandleAskGiveUpRetry.State.END;
	}

	public override void EndErrorHandle()
	{
		NetworkErrorWindow.Close();
		if (this.m_isRetry && this.m_retryProcess != null)
		{
			this.m_retryProcess.Retry();
		}
	}

	private void OnFinishedCloseAnimCallback()
	{
		if (this.m_state == ErrorHandleAskGiveUpRetry.State.ASK_GIVEUP && NetworkErrorWindow.IsYesButtonPressed)
		{
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage(this.m_callbackFuncName, this.m_msg, SendMessageOptions.DontRequireReceiver);
			}
			this.m_isRetry = true;
			this.m_state = ErrorHandleAskGiveUpRetry.State.END;
		}
	}
}
