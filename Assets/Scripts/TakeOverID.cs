using System;
using UnityEngine;

public class TakeOverID : MonoBehaviour
{
	private window_takeover_id m_takeOverId;

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
			if (this.m_takeOverId != null)
			{
				this.m_takeOverId.PlayOpenWindow();
			}
		}
		else
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_takeover_id", true);
		}
	}

	private void SetTakeOverId()
	{
		if (this.m_gameObject != null && this.m_takeOverId == null)
		{
			this.m_takeOverId = this.m_gameObject.GetComponent<window_takeover_id>();
		}
	}

	public void Update()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetTakeOverId();
			if (this.m_takeOverId != null)
			{
				this.m_takeOverId.PlayOpenWindow();
			}
		}
		else if (this.m_takeOverId != null && this.m_takeOverId.IsEnd)
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
