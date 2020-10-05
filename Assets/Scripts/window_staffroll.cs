using DataTable;
using System;
using Text;
using UnityEngine;

public class window_staffroll : WindowBase
{
	private const string ATTACH_ANTHOR_NAME = "UI Root (2D)/Camera/menu_Anim/OptionUI/Anchor_5_MC";

	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private UIScrollBar m_scrollBar;

	[SerializeField]
	private UILabel m_headerTextLabel;

	[SerializeField]
	private UILabel m_staffrollLabel;

	private GameObject m_parserObject;

	private string m_creditText = string.Empty;

	private string m_copyrightText = string.Empty;

	private bool m_creditFlag;

	private bool m_isEnd;

	private bool m_init;

	private UIPlayAnimation m_uiAnimation;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
	}

	private void Initialize()
	{
		if (this.m_init)
		{
			return;
		}
		OptionMenuUtility.TranObj(base.gameObject);
		if (this.m_closeBtn != null)
		{
			UIPlayAnimation component = this.m_closeBtn.GetComponent<UIPlayAnimation>();
			if (component != null)
			{
				EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), false);
			}
			UIButtonMessage component2 = this.m_closeBtn.GetComponent<UIButtonMessage>();
			if (component2 == null)
			{
				this.m_closeBtn.AddComponent<UIButtonMessage>();
				component2 = this.m_closeBtn.GetComponent<UIButtonMessage>();
			}
			if (component2 != null)
			{
				component2.enabled = true;
				component2.trigger = UIButtonMessage.Trigger.OnClick;
				component2.target = base.gameObject;
				component2.functionName = "OnClickCloseButton";
			}
		}
		if (this.m_scrollBar != null)
		{
			this.m_scrollBar.value = 0f;
		}
		if (this.m_staffrollLabel != null)
		{
			this.m_staffrollLabel.text = string.Empty;
		}
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component3 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component3;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		this.m_init = true;
	}

	private void Update()
	{
		if (this.m_parserObject != null)
		{
			HtmlParser component = this.m_parserObject.GetComponent<HtmlParser>();
			if (component != null && component.IsEndParse)
			{
				base.enabled = false;
				if (this.m_staffrollLabel != null)
				{
					this.m_staffrollLabel.text = component.ParsedString;
				}
				if (this.m_creditFlag)
				{
					this.m_creditText = component.ParsedString;
				}
				else
				{
					this.m_copyrightText = component.ParsedString;
				}
				UnityEngine.Object.Destroy(this.m_parserObject);
				this.m_parserObject = null;
			}
		}
	}

	public void SetStaffRollText()
	{
		this.Initialize();
		this.m_creditFlag = true;
		TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "staff_credit");
		if (string.IsNullOrEmpty(this.m_creditText))
		{
			if (this.m_staffrollLabel != null)
			{
				this.m_staffrollLabel.text = string.Empty;
			}
			string webPageURL = NetUtil.GetWebPageURL(InformationDataTable.Type.CREDIT);
			this.m_parserObject = HtmlParserFactory.Create(webPageURL, HtmlParser.SyncType.TYPE_ASYNC, HtmlParser.SyncType.TYPE_ASYNC);
			base.enabled = true;
		}
		else if (this.m_staffrollLabel != null)
		{
			this.m_staffrollLabel.text = this.m_creditText;
		}
	}

	public void SetCopyrightText()
	{
		this.Initialize();
		this.m_creditFlag = false;
		TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "copyright");
		if (string.IsNullOrEmpty(this.m_copyrightText))
		{
			if (this.m_staffrollLabel != null)
			{
				this.m_staffrollLabel.text = string.Empty;
			}
			string webPageURL = NetUtil.GetWebPageURL(InformationDataTable.Type.COPYRIGHT);
			this.m_parserObject = HtmlParserFactory.Create(webPageURL, HtmlParser.SyncType.TYPE_ASYNC, HtmlParser.SyncType.TYPE_ASYNC);
			base.enabled = true;
		}
		else if (this.m_staffrollLabel != null)
		{
			this.m_staffrollLabel.text = this.m_copyrightText;
		}
	}

	private void OnClickCloseButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
	}

	public void PlayOpenWindow()
	{
		this.m_isEnd = false;
		if (this.m_scrollBar != null)
		{
			this.m_scrollBar.value = 0f;
		}
		if (this.m_uiAnimation != null)
		{
			this.m_uiAnimation.Play(true);
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		UIButtonMessage component = this.m_closeBtn.GetComponent<UIButtonMessage>();
		if (component != null)
		{
			component.SendMessage("OnClick");
		}
	}
}
