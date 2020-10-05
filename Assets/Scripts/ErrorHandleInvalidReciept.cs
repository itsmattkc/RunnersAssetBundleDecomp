using Message;
using System;
using Text;
using UnityEngine;

public class ErrorHandleInvalidReciept : ErrorHandleBase
{
	private bool m_isEnd;

	private GameObject m_callbackObject;

	private string m_callbackFuncName;

	private MessageBase m_msg;

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
		this.m_callbackObject = callbackObject;
		this.m_callbackFuncName = callbackFuncName;
		this.m_msg = msg;
	}

	public override void StartErrorHandle()
	{
		NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
		{
			buttonType = NetworkErrorWindow.ButtonType.Ok,
			anchor_path = string.Empty,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_invalid_receipt").text
		});
	}

	public override void Update()
	{
		if (NetworkErrorWindow.IsOkButtonPressed)
		{
			NetworkErrorWindow.Close();
			if (this.m_callbackObject != null)
			{
				this.m_callbackObject.SendMessage(this.m_callbackFuncName, this.m_msg, SendMessageOptions.DontRequireReceiver);
			}
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
