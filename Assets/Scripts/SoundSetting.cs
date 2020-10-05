using System;
using UnityEngine;

public class SoundSetting : MonoBehaviour
{
	private window_sound_setiing m_soundSetiing;

	private GameObject m_gameObject;

	private ui_option_scroll m_ui_option_scroll;

	private bool m_initFlag;

	public void Setup(ui_option_scroll scroll)
	{
		base.enabled = true;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		if (this.m_gameObject != null)
		{
			this.m_initFlag = true;
			this.m_gameObject.SetActive(true);
			if (this.m_soundSetiing != null)
			{
				this.m_soundSetiing.PlayOpenWindow();
			}
		}
		else
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_sound_setiing", true);
		}
	}

	private void SetSoundSetting()
	{
		if (this.m_gameObject != null && this.m_soundSetiing == null)
		{
			this.m_soundSetiing = this.m_gameObject.GetComponent<window_sound_setiing>();
		}
	}

	public void Update()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetSoundSetting();
			if (this.m_soundSetiing != null)
			{
				this.m_soundSetiing.PlayOpenWindow();
			}
		}
		else if (this.m_soundSetiing != null && this.m_soundSetiing.IsEnd)
		{
			if (this.m_ui_option_scroll != null)
			{
				if (this.m_soundSetiing.IsOverwrite)
				{
					this.m_ui_option_scroll.SetSystemSaveFlag();
				}
				this.m_ui_option_scroll.OnEndChildPage();
			}
			base.enabled = false;
			if (this.m_gameObject != null)
			{
				this.m_gameObject.SetActive(false);
			}
		}
	}
}
