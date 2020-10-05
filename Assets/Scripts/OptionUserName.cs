using System;
using UnityEngine;

public class OptionUserName : MonoBehaviour
{
	private const string ATTACH_ANTHOR_NAME = "UI Root (2D)/Camera/Anchor_5_MC";

	private SettingUserName m_settingName;

	private ui_option_scroll m_ui_option_scroll;

	public void Setup(ui_option_scroll scroll)
	{
		if (scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		if (this.m_settingName == null)
		{
			GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
			if (menuAnimUIObject != null)
			{
				this.m_settingName = GameObjectUtil.FindChildGameObjectComponent<SettingUserName>(menuAnimUIObject, "window_name_setting");
			}
		}
		if (this.m_settingName != null)
		{
			this.m_settingName.SetCancelButtonUseFlag(true);
			this.m_settingName.Setup("UI Root (2D)/Camera/Anchor_5_MC");
			this.m_settingName.PlayStart();
		}
		base.enabled = true;
	}

	public void Update()
	{
		if (this.m_settingName != null && this.m_settingName.IsEndPlay())
		{
			if (this.m_ui_option_scroll != null)
			{
				this.m_ui_option_scroll.OnEndChildPage();
			}
			base.enabled = false;
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		}
	}
}
