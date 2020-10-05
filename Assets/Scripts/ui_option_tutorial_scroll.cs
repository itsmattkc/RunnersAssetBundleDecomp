using Message;
using System;
using Text;
using UnityEngine;

public class ui_option_tutorial_scroll : MonoBehaviour
{
	[SerializeField]
	private UILabel m_textLabel;

	[SerializeField]
	private UILabel m_shadowTextLabel;

	[SerializeField]
	private GameObject m_btnObj;

	private window_tutorial_other_character m_window;

	private window_tutorial.ScrollInfo m_scrollInfo;

	private bool m_openWindow;

	public bool OpenWindow
	{
		get
		{
			return this.m_openWindow;
		}
	}

	private void Start()
	{
		base.enabled = false;
		if (this.m_btnObj != null)
		{
			UIButtonMessage component = this.m_btnObj.GetComponent<UIButtonMessage>();
			if (component != null)
			{
				component.enabled = true;
				component.trigger = UIButtonMessage.Trigger.OnClick;
				component.target = base.gameObject;
				component.functionName = "OnClickOptionTutorialScroll";
			}
		}
	}

	public void Update()
	{
		if (this.m_window != null && this.m_window.IsEnd)
		{
			this.m_openWindow = false;
			this.m_window.gameObject.SetActive(false);
			base.enabled = false;
			if (this.m_scrollInfo != null && this.m_scrollInfo.Parent != null)
			{
				this.m_scrollInfo.Parent.SetCloseBtnColliderTrigger(false);
			}
		}
	}

	public void UpdateView(window_tutorial.ScrollInfo info)
	{
		this.m_scrollInfo = info;
		this.SetText();
	}

	public void SetText()
	{
		if (this.m_scrollInfo != null)
		{
			string text = null;
			switch (this.m_scrollInfo.DispType)
			{
			case window_tutorial.DisplayType.TUTORIAL:
				text = TextUtility.GetCommonText("Option", "tutorial");
				break;
			case window_tutorial.DisplayType.QUICK:
				text = TextUtility.GetCommonText("Tutorial", "caption_quickmode_tutorial");
				break;
			case window_tutorial.DisplayType.CHARA:
				text = TextUtility.GetCommonText("CharaName", CharaName.Name[(int)this.m_scrollInfo.Chara]);
				break;
			case window_tutorial.DisplayType.BOSS_MAP_1:
				text = BossTypeUtil.GetTextCommonBossName(BossType.MAP1);
				break;
			case window_tutorial.DisplayType.BOSS_MAP_2:
				text = BossTypeUtil.GetTextCommonBossName(BossType.MAP2);
				break;
			case window_tutorial.DisplayType.BOSS_MAP_3:
				text = BossTypeUtil.GetTextCommonBossName(BossType.MAP3);
				break;
			}
			if (text != null)
			{
				if (this.m_textLabel != null)
				{
					this.m_textLabel.text = text;
				}
				if (this.m_shadowTextLabel != null)
				{
					this.m_shadowTextLabel.text = text;
				}
			}
		}
	}

	private void OnClickOptionTutorialScroll()
	{
		if (this.m_scrollInfo != null)
		{
			if (this.m_scrollInfo.DispType == window_tutorial.DisplayType.TUTORIAL)
			{
				HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.STAGE);
			}
			else
			{
				if (this.m_window == null)
				{
					GameObject loadMenuChildObject = HudMenuUtility.GetLoadMenuChildObject("window_tutorial_other_character", true);
					if (loadMenuChildObject != null)
					{
						this.m_window = loadMenuChildObject.GetComponent<window_tutorial_other_character>();
					}
				}
				if (this.m_window != null)
				{
					this.m_window.SetScrollInfo(this.m_scrollInfo);
					this.m_window.PlayOpenWindow();
					this.m_openWindow = true;
					base.enabled = true;
					if (this.m_scrollInfo.Parent != null)
					{
						this.m_scrollInfo.Parent.SetCloseBtnColliderTrigger(true);
					}
				}
			}
		}
	}
}
