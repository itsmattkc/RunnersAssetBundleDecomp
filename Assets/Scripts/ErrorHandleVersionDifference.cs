using Message;
using System;
using Text;
using UnityEngine;

public class ErrorHandleVersionDifference : ErrorHandleBase
{
	private bool m_isEnd;

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
	}

	public override void StartErrorHandle()
	{
		NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
		{
			buttonType = NetworkErrorWindow.ButtonType.Ok,
			anchor_path = string.Empty,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_update_version_android").text
		});
	}

	public override void Update()
	{
		if (NetworkErrorWindow.IsOkButtonPressed)
		{
			NetworkErrorWindow.Close();
			HudMenuUtility.GoToTitleScene();
			string redirectInstallPageUrl = NetBaseUtil.RedirectInstallPageUrl;
			if (!string.IsNullOrEmpty(redirectInstallPageUrl))
			{
				Application.OpenURL(redirectInstallPageUrl);
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
