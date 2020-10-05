using Message;
using System;
using Text;
using UnityEngine;

public class window_user_date : WindowBase
{
	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private UIScrollBar m_scrollBar;

	[SerializeField]
	private UIRectItemStorage m_itemStorage;

	[SerializeField]
	private UILabel m_headerTextLabel;

	private ServerOptionUserResult m_serverOptionUserResult = new ServerOptionUserResult();

	private UIPlayAnimation m_uiAnimation;

	private bool m_isEnd;

	private bool m_init;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public ServerOptionUserResult OptionUserResult
	{
		get
		{
			return this.m_serverOptionUserResult;
		}
	}

	private void Start()
	{
		OptionMenuUtility.TranObj(base.gameObject);
		if (this.m_closeBtn != null)
		{
			UIButtonMessage component = this.m_closeBtn.GetComponent<UIButtonMessage>();
			if (component == null)
			{
				this.m_closeBtn.AddComponent<UIButtonMessage>();
				component = this.m_closeBtn.GetComponent<UIButtonMessage>();
			}
			if (component != null)
			{
				component.enabled = true;
				component.trigger = UIButtonMessage.Trigger.OnClick;
				component.target = base.gameObject;
				component.functionName = "OnClickCloseButton";
			}
			UIPlayAnimation component2 = this.m_closeBtn.GetComponent<UIPlayAnimation>();
			if (component2 != null)
			{
				EventDelegate.Add(component2.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), false);
			}
		}
		if (this.m_scrollBar != null)
		{
			this.m_scrollBar.value = 0f;
		}
		if (this.m_headerTextLabel != null)
		{
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Option", "users_results");
			if (text != null)
			{
				this.m_headerTextLabel.text = text.text;
			}
		}
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component3 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component3;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerOptionUserResult(base.gameObject);
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	private void Update()
	{
		if (!this.m_init)
		{
			base.enabled = false;
			this.m_init = true;
			this.SetItemStorage();
		}
	}

	private void SetItemStorage()
	{
		if (this.m_itemStorage != null)
		{
			this.m_itemStorage.maxItemCount = 14;
			this.m_itemStorage.maxRows = 14;
			this.m_itemStorage.Restart();
			this.UpdateViewItemStorage();
		}
	}

	private void UpdateViewItemStorage()
	{
		if (this.m_itemStorage != null)
		{
			ui_option_window_user_date_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_option_window_user_date_scroll>(true);
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				if (i < 14)
				{
					ui_option_window_user_date_scroll.ResultType type = (ui_option_window_user_date_scroll.ResultType)i;
					componentsInChildren[i].UpdateView(type, this.m_serverOptionUserResult);
				}
			}
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

	private void ServerGetOptionUserResult_Succeeded(MsgGetOptionUserResultSucceed msg)
	{
		if (msg != null && msg.m_serverOptionUserResult != null)
		{
			msg.m_serverOptionUserResult.CopyTo(this.m_serverOptionUserResult);
		}
		this.UpdateViewItemStorage();
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

	private void ServerGetOptionUserResult_Failed()
	{
	}

	public void PlayOpenWindow()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerOptionUserResult(base.gameObject);
		}
		this.m_isEnd = false;
		if (this.m_uiAnimation != null)
		{
			this.m_uiAnimation.Play(true);
		}
	}
}
