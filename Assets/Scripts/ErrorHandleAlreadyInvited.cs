using Message;
using System;
using Text;
using UnityEngine;

public class ErrorHandleAlreadyInvited : ErrorHandleBase
{
	private bool m_isEnd;

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
	}

	public override void StartErrorHandle()
	{
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Option", "accepted_invite_caption").text;
		string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Option", "accepted_invite_text").text;
		NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
		{
			buttonType = NetworkErrorWindow.ButtonType.Ok,
			anchor_path = string.Empty,
			caption = text,
			message = text2
		});
		this.m_isEnd = false;
	}

	public override void Update()
	{
		if (NetworkErrorWindow.IsOkButtonPressed)
		{
			NetworkErrorWindow.Close();
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
