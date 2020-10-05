using System;
using UnityEngine;

public class BuyHistory : MonoBehaviour
{
	private window_buying_history m_history;

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
			if (this.m_history != null)
			{
				this.m_history.PlayOpenWindow();
			}
		}
		else
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_buying_history", true);
		}
	}

	private void SetBuyingHistory()
	{
		if (this.m_gameObject != null && this.m_history == null)
		{
			this.m_history = this.m_gameObject.GetComponent<window_buying_history>();
		}
	}

	public void Update()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetBuyingHistory();
			if (this.m_history != null)
			{
				this.m_history.PlayOpenWindow();
			}
		}
		else if (this.m_history != null && this.m_history.IsEnd)
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
