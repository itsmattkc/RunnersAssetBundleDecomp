using Message;
using System;
using Text;
using UnityEngine;

public class ErrorHandleMaintenance : ErrorHandleBase
{
	private bool m_isEnd;

	private bool m_isExistMaintenancePage;

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
	}

	public override void StartErrorHandle()
	{
		this.m_isExistMaintenancePage = false;
		NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
		{
			buttonType = ((!this.m_isExistMaintenancePage) ? NetworkErrorWindow.ButtonType.Ok : NetworkErrorWindow.ButtonType.HomePage),
			anchor_path = string.Empty,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_maintenance").text
		});
	}

	public override void Update()
	{
		if (this.m_isExistMaintenancePage)
		{
			if (NetworkErrorWindow.IsYesButtonPressed)
			{
				NetworkErrorWindow.Close();
				HudMenuUtility.GoToTitleScene();
				string maintenancePageURL = ErrorHandleMaintenanceUtil.GetMaintenancePageURL();
				Application.OpenURL(maintenancePageURL);
				this.m_isEnd = true;
			}
			else if (NetworkErrorWindow.IsNoButtonPressed)
			{
				NetworkErrorWindow.Close();
				HudMenuUtility.GoToTitleScene();
				this.m_isEnd = true;
			}
		}
		else if (NetworkErrorWindow.IsOkButtonPressed)
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
