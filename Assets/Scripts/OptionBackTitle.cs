using Message;
using System;
using Text;
using UnityEngine;

public class OptionBackTitle : MonoBehaviour
{
	private ui_option_scroll m_ui_option_scroll;

	public void Setup(ui_option_scroll scroll)
	{
		base.enabled = true;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		this.CreateBackTitleWindow();
	}

	private void CreateBackTitleWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "BackTitle",
			caption = TextUtility.GetCommonText("MainMenu", "back_title_caption"),
			message = TextUtility.GetCommonText("MainMenu", "back_title_text"),
			anchor_path = "Camera/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.YesNo
		});
	}

	public void Update()
	{
		bool flag = false;
		if (GeneralWindow.IsCreated("BackTitle") && GeneralWindow.IsButtonPressed)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				GeneralWindow.Close();
				HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.TITLE);
			}
			else
			{
				flag = true;
				GeneralWindow.Close();
			}
		}
		if (flag)
		{
			if (this.m_ui_option_scroll != null)
			{
				this.m_ui_option_scroll.OnEndChildPage();
			}
			base.enabled = false;
		}
	}
}
