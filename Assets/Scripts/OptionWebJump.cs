using DataTable;
using System;
using UnityEngine;

public class OptionWebJump : MonoBehaviour
{
	public enum WebType
	{
		HELP,
		TERMS_OF_SERVICE,
		NUM
	}

	private ui_option_scroll m_ui_option_scroll;

	private OptionWebJump.WebType m_webType;

	private bool m_setFlag;

	private void Start()
	{
	}

	public void Setup(ui_option_scroll scroll, OptionWebJump.WebType type)
	{
		base.enabled = true;
		this.m_webType = type;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		this.m_setFlag = true;
	}

	public void Update()
	{
		if (this.m_setFlag)
		{
			string urlText = this.GetUrlText();
			if (urlText != null)
			{
				Application.OpenURL(urlText);
			}
			base.enabled = false;
			this.m_setFlag = false;
			if (this.m_ui_option_scroll != null)
			{
				this.m_ui_option_scroll.OnEndChildPage();
			}
		}
	}

	private string GetUrlText()
	{
		if (this.m_webType == OptionWebJump.WebType.HELP)
		{
			return InformationDataTable.GetUrl(InformationDataTable.Type.HELP);
		}
		if (this.m_webType == OptionWebJump.WebType.TERMS_OF_SERVICE)
		{
			return NetBaseUtil.RedirectTrmsOfServicePageUrl;
		}
		return null;
	}
}
