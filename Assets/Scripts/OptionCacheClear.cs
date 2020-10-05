using Message;
using System;
using Text;
using UnityEngine;

public class OptionCacheClear : MonoBehaviour
{
	private ui_option_scroll m_ui_option_scroll;

	public void Setup(ui_option_scroll scroll)
	{
		base.enabled = true;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		this.CreateCacheClearWindow();
	}

	private void CreateCacheClearWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "cache_clear",
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_bar"),
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_explanation"),
			anchor_path = "Camera/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.YesNo
		});
	}

	public void Update()
	{
		bool flag = false;
		if (GeneralWindow.IsCreated("cache_clear") && GeneralWindow.IsButtonPressed)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				GeneralUtil.CleanAllCache();
				GeneralWindow.Close();
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "cache_clear_end",
					caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_confirmation_bar"),
					message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_confirmation"),
					anchor_path = "Camera/Anchor_5_MC",
					buttonType = GeneralWindow.ButtonType.Ok
				});
			}
			else
			{
				flag = true;
				GeneralWindow.Close();
			}
		}
		if (GeneralWindow.IsCreated("cache_clear_end") && GeneralWindow.IsOkButtonPressed)
		{
			GeneralWindow.Close();
			HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.TITLE);
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
