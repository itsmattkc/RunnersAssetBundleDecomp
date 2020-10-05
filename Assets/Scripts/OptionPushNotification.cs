using SaveData;
using System;
using UnityEngine;

public class OptionPushNotification : MonoBehaviour
{
	private const string ATTACH_ANTHOR_NAME = "UI Root (2D)/Camera/Anchor_5_MC";

	private SettingPartsPushNotice m_pushNotice;

	private ui_option_scroll m_ui_option_scroll;

	public void Setup(ui_option_scroll scroll)
	{
		base.enabled = true;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		if (this.m_pushNotice != null)
		{
			this.m_pushNotice.PlayStart();
		}
		else
		{
			this.m_pushNotice = base.gameObject.AddComponent<SettingPartsPushNotice>();
			if (this.m_pushNotice)
			{
				this.m_pushNotice.Setup("UI Root (2D)/Camera/Anchor_5_MC");
				this.m_pushNotice.PlayStart();
			}
		}
	}

	public void Update()
	{
		if (this.m_pushNotice != null && this.m_pushNotice.IsEndPlay())
		{
			if (this.m_ui_option_scroll != null)
			{
				if (this.m_pushNotice.IsOverwrite)
				{
					SystemSaveManager instance = SystemSaveManager.Instance;
					if (instance != null)
					{
						instance.SaveSystemData();
					}
					this.m_ui_option_scroll.ResetSystemSaveFlag();
				}
				this.m_ui_option_scroll.OnEndChildPage();
			}
			base.enabled = false;
			this.SetActivePushNoticeObject(false);
		}
	}

	private void SetActivePushNoticeObject(bool flag)
	{
		if (this.m_pushNotice != null)
		{
			this.m_pushNotice.SetWindowActive(flag);
		}
	}
}
