using System;
using UnityEngine;

public class StaffRoll : MonoBehaviour
{
	public enum TextType
	{
		STAFF_ROLL,
		COPYRIGHT
	}

	private window_staffroll m_staffRoll;

	private GameObject m_windoObj;

	private ui_option_scroll m_ui_option_scroll;

	private StaffRoll.TextType m_textType;

	public void SetTextType(StaffRoll.TextType type)
	{
		this.m_textType = type;
	}

	public void Setup(ui_option_scroll scroll)
	{
		base.enabled = true;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		if (this.m_windoObj == null)
		{
			this.m_windoObj = HudMenuUtility.GetLoadMenuChildObject("window_staffroll", false);
		}
		if (this.m_windoObj != null)
		{
			if (this.m_staffRoll == null)
			{
				this.m_staffRoll = this.m_windoObj.GetComponent<window_staffroll>();
			}
			if (this.m_staffRoll != null)
			{
				StaffRoll.TextType textType = this.m_textType;
				if (textType != StaffRoll.TextType.STAFF_ROLL)
				{
					if (textType == StaffRoll.TextType.COPYRIGHT)
					{
						this.m_staffRoll.SetCopyrightText();
					}
				}
				else
				{
					this.m_staffRoll.SetStaffRollText();
				}
				this.m_windoObj.SetActive(true);
				this.m_staffRoll.PlayOpenWindow();
			}
		}
	}

	public void Update()
	{
		if (this.m_staffRoll != null && this.m_staffRoll.IsEnd)
		{
			if (this.m_ui_option_scroll != null)
			{
				this.m_ui_option_scroll.OnEndChildPage();
			}
			base.enabled = false;
			if (this.m_windoObj != null)
			{
				this.m_windoObj.SetActive(false);
			}
		}
	}
}
