using System;
using UnityEngine;

public class OptionTutorial : MonoBehaviour
{
	private window_tutorial m_turoialWindow;

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
			if (this.m_turoialWindow != null)
			{
				this.m_turoialWindow.PlayOpenWindow();
			}
		}
		else
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_tutorial", true);
		}
	}

	private void SetTuroialWindow()
	{
		if (this.m_gameObject != null && this.m_turoialWindow == null)
		{
			this.m_turoialWindow = this.m_gameObject.GetComponent<window_tutorial>();
		}
	}

	public void Update()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetTuroialWindow();
			if (this.m_turoialWindow != null)
			{
				this.m_turoialWindow.PlayOpenWindow();
			}
		}
		else if (this.m_turoialWindow != null && this.m_turoialWindow.IsEnd)
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
