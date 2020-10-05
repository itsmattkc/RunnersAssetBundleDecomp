using System;
using UnityEngine;

public class OptionUserResult : MonoBehaviour
{
	private window_user_date m_userData;

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
			if (this.m_userData != null)
			{
				this.m_userData.PlayOpenWindow();
			}
		}
		else
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_user_date", true);
		}
	}

	private void SetUserData()
	{
		if (this.m_gameObject != null && this.m_userData == null)
		{
			this.m_userData = this.m_gameObject.GetComponent<window_user_date>();
		}
	}

	public void Update()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetUserData();
			if (this.m_userData != null)
			{
				this.m_userData.PlayOpenWindow();
			}
		}
		else if (this.m_userData != null && this.m_userData.IsEnd)
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
		}
	}
}
