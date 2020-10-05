using SaveData;
using System;
using Text;
using UnityEngine;

public class CheckID : MonoBehaviour
{
	private enum State
	{
		INIT,
		FIRST_CAUTION,
		CHECK_PASSWORD,
		INPUT_USERPASS,
		GET_MOVING_PASSWORD,
		IDLE,
		CLOSE
	}

	private const string ATTACH_ANTHOR_NAME = "UI Root (2D)/Camera/Anchor_5_MC";

	private window_id_info m_idInfo;

	private GameObject m_gameObject;

	private ui_option_scroll m_ui_option_scroll;

	private SettingTakeoverPassword m_settingPassword;

	private bool m_initFlag;

	private CheckID.State m_State;

	public void Setup(ui_option_scroll scroll)
	{
		base.enabled = true;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		if (this.m_gameObject == null)
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_id_info", true);
		}
		this.m_State = CheckID.State.INIT;
	}

	private void SetIdInfo()
	{
		if (this.m_gameObject != null && this.m_idInfo == null)
		{
			this.m_idInfo = this.m_gameObject.GetComponent<window_id_info>();
		}
	}

	public void Update()
	{
		switch (this.m_State)
		{
		case CheckID.State.INIT:
			this.CreateFirstCautionWindow();
			break;
		case CheckID.State.FIRST_CAUTION:
			if (GeneralWindow.IsCreated("FirstCaution") && GeneralWindow.IsButtonPressed)
			{
				if (this.m_gameObject != null)
				{
					this.m_gameObject.SetActive(false);
				}
				GeneralWindow.Close();
				this.m_State = CheckID.State.CHECK_PASSWORD;
			}
			break;
		case CheckID.State.CHECK_PASSWORD:
		{
			string takeoverID = SystemSaveManager.GetTakeoverID();
			if (string.IsNullOrEmpty(takeoverID))
			{
				this.CreateUserPassWindow();
			}
			else
			{
				this.SetupInfoWindow();
			}
			break;
		}
		case CheckID.State.INPUT_USERPASS:
			if (this.m_settingPassword != null && this.m_settingPassword.IsEndPlay())
			{
				if (this.m_settingPassword.isCancel)
				{
					this.CloseFunction();
				}
				else
				{
					this.SetupInfoWindow();
				}
			}
			break;
		case CheckID.State.IDLE:
			if (this.m_idInfo != null && this.m_idInfo.IsEnd)
			{
				if (this.m_idInfo.IsPassResetEnd)
				{
					if (this.m_gameObject != null)
					{
						this.m_gameObject.SetActive(false);
					}
					this.CreateUserPassWindow();
				}
				else
				{
					this.CloseFunction();
				}
			}
			break;
		}
	}

	private void CreateFirstCautionWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "FirstCaution",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextUtility.GetCommonText("Option", "take_over_attention"),
			message = TextUtility.GetCommonText("Option", "take_over_attention_text")
		});
		this.m_State = CheckID.State.FIRST_CAUTION;
	}

	private void CreateUserPassWindow()
	{
		if (this.m_settingPassword == null)
		{
			this.m_settingPassword = base.gameObject.AddComponent<SettingTakeoverPassword>();
		}
		if (this.m_settingPassword != null)
		{
			this.m_settingPassword.SetCancelButtonUseFlag(true);
			this.m_settingPassword.Setup("UI Root (2D)/Camera/Anchor_5_MC");
			this.m_settingPassword.PlayStart();
		}
		this.m_State = CheckID.State.INPUT_USERPASS;
	}

	private void SetupInfoWindow()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetIdInfo();
		}
		this.m_gameObject.SetActive(true);
		if (this.m_idInfo != null)
		{
			this.m_idInfo.PlayOpenWindow();
		}
		this.m_State = CheckID.State.IDLE;
	}

	private void CloseFunction()
	{
		if (this.m_ui_option_scroll != null)
		{
			this.m_ui_option_scroll.OnEndChildPage();
		}
		base.enabled = false;
		if (this.m_gameObject != null)
		{
			this.m_gameObject.SetActive(false);
		}
		this.m_State = CheckID.State.CLOSE;
	}
}
