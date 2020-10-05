using System;
using UnityEngine;

public class WeightSaving : MonoBehaviour
{
	private window_performance_setting m_performanceSetting;

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
			if (this.m_performanceSetting != null)
			{
				this.m_performanceSetting.Setup();
				this.m_performanceSetting.PlayOpenWindow();
			}
		}
		else
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_performance_setting", true);
		}
	}

	private void SetEventSetting()
	{
		if (this.m_gameObject != null && this.m_performanceSetting == null)
		{
			this.m_performanceSetting = this.m_gameObject.GetComponent<window_performance_setting>();
		}
	}

	public void Update()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetEventSetting();
			if (this.m_performanceSetting != null)
			{
				this.m_performanceSetting.Setup();
				this.m_performanceSetting.PlayOpenWindow();
			}
		}
		else if (this.m_performanceSetting != null && this.m_performanceSetting.IsEnd)
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
