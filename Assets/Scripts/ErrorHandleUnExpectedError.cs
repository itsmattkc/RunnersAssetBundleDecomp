using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ErrorHandleUnExpectedError : ErrorHandleBase
{
	private class TextInfo
	{
		private string m_captionId;

		private string m_messageId;

		public string CaptionId
		{
			get
			{
				return this.m_captionId;
			}
			private set
			{
			}
		}

		public string MessageId
		{
			get
			{
				return this.m_messageId;
			}
			private set
			{
			}
		}

		public TextInfo(string captionId, string messageId)
		{
			this.m_captionId = captionId;
			this.m_messageId = messageId;
		}
	}

	private bool m_isEnd;

	private MessageBase m_msg;

	private static readonly Dictionary<ServerInterface.StatusCode, ErrorHandleUnExpectedError.TextInfo> ErrorTextPair = new Dictionary<ServerInterface.StatusCode, ErrorHandleUnExpectedError.TextInfo>
	{
		{
			ServerInterface.StatusCode.PassWordError,
			new ErrorHandleUnExpectedError.TextInfo("ui_Lbl_caption_local", "ui_Lbl_password_error")
		}
	};

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
		this.m_msg = msg;
	}

	public override void StartErrorHandle()
	{
		string caption = string.Empty;
		string text = string.Empty;
		MsgServerConnctFailed msgServerConnctFailed = this.m_msg as MsgServerConnctFailed;
		if (msgServerConnctFailed != null)
		{
			ErrorHandleUnExpectedError.TextInfo textInfo = null;
			if (ErrorHandleUnExpectedError.ErrorTextPair.TryGetValue(msgServerConnctFailed.m_status, out textInfo))
			{
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", textInfo.CaptionId).text;
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", textInfo.MessageId).text;
			}
			else
			{
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text;
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_reset").text;
				string arg_A6_0 = text;
				int status = (int)msgServerConnctFailed.m_status;
				text = arg_A6_0 + status.ToString();
			}
		}
		NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
		{
			buttonType = NetworkErrorWindow.ButtonType.Ok,
			anchor_path = string.Empty,
			caption = caption,
			message = text
		});
	}

	public override void Update()
	{
		if (NetworkErrorWindow.IsOkButtonPressed)
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
	}
}
