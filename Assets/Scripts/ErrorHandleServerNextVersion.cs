using App;
using Message;
using SaveData;
using System;
using Text;
using UnityEngine;

public class ErrorHandleServerNextVersion : ErrorHandleBase
{
	private int m_buyRSRNum;

	private int m_freeRSRNum;

	private string m_userId = string.Empty;

	private string m_userName = string.Empty;

	private bool m_titleBack;

	private bool m_isEnd;

	private bool IsRegionJapan()
	{
		return RegionManager.Instance != null && RegionManager.Instance.IsJapan();
	}

	private string GetReplaceText(string srcText, string tag, string replace)
	{
		if (!string.IsNullOrEmpty(srcText) && !string.IsNullOrEmpty(tag) && !string.IsNullOrEmpty(replace))
		{
			return TextUtility.Replace(srcText, tag, replace);
		}
		return srcText;
	}

	public override void Setup(GameObject callbackObject, string callbackFuncName, MessageBase msg)
	{
	}

	public override void StartErrorHandle()
	{
		NetworkErrorWindow.CInfo info = default(NetworkErrorWindow.CInfo);
		info.anchor_path = string.Empty;
		this.m_titleBack = GameModeTitle.Logined;
		if (this.m_titleBack)
		{
			info.buttonType = NetworkErrorWindow.ButtonType.Ok;
			info.caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_caption");
			info.message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_reset");
			info.message += (-19990).ToString();
		}
		else
		{
			this.m_userId = SystemSaveManager.GetGameID();
			info.caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_notification_caption");
			bool flag = Env.language == Env.Language.JAPANESE;
			string text = (!flag) ? ServerInterface.NextVersionState.m_eMsg : ServerInterface.NextVersionState.m_jMsg;
			if (this.m_userId != "0" && this.IsRegionJapan())
			{
				this.m_buyRSRNum = ServerInterface.NextVersionState.m_buyRSRNum;
				this.m_freeRSRNum = ServerInterface.NextVersionState.m_freeRSRNum;
				this.m_userName = ServerInterface.NextVersionState.m_userName;
				if (string.IsNullOrEmpty(this.m_userName))
				{
					ServerSettingState settingState = ServerInterface.SettingState;
					if (settingState != null)
					{
						this.m_userName = settingState.m_userName;
					}
					if (string.IsNullOrEmpty(this.m_userName))
					{
						this.m_userName = " ";
					}
				}
				info.buttonType = NetworkErrorWindow.ButtonType.Repayment;
				string text2 = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_user_info_text");
				text2 = this.GetReplaceText(text2, "{PARAM1}", this.m_userName);
				text2 = this.GetReplaceText(text2, "{PARAM2}", this.m_userId);
				text2 = this.GetReplaceText(text2, "{PARAM3}", this.m_buyRSRNum.ToString());
				text2 = this.GetReplaceText(text2, "{PARAM4}", this.m_freeRSRNum.ToString());
				text = text + "\n" + text2;
			}
			else
			{
				info.buttonType = NetworkErrorWindow.ButtonType.TextOnly;
			}
			info.message = text;
		}
		NetworkErrorWindow.Create(info);
		this.m_isEnd = false;
	}

	public override void Update()
	{
		if (this.m_titleBack)
		{
			if (NetworkErrorWindow.IsOkButtonPressed)
			{
				NetworkErrorWindow.Close();
				HudMenuUtility.GoToTitleScene();
				this.m_isEnd = true;
			}
		}
		else if (NetworkErrorWindow.IsButtonPressed)
		{
			NetworkErrorWindow.ResetButton();
			string url = ServerInterface.NextVersionState.m_url;
			if (!string.IsNullOrEmpty(url))
			{
				Application.OpenURL(url);
			}
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
