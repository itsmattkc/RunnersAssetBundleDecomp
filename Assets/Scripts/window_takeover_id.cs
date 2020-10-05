using Message;
using System;
using Text;
using UnityEngine;

public class window_takeover_id : WindowBase
{
	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private UILabel m_headerTextLabel;

	[SerializeField]
	private UILabel m_passwordTextLabel;

	[SerializeField]
	private UILabel m_passwordLabel;

	private bool m_isEnd;

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
		OptionMenuUtility.TranObj(base.gameObject);
		base.enabled = false;
		if (this.m_closeBtn != null)
		{
			UIPlayAnimation component = this.m_closeBtn.GetComponent<UIPlayAnimation>();
			if (component != null)
			{
				EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), false);
			}
			UIButtonMessage uIButtonMessage = this.m_closeBtn.GetComponent<UIButtonMessage>();
			if (uIButtonMessage == null)
			{
				uIButtonMessage = this.m_closeBtn.AddComponent<UIButtonMessage>();
			}
			if (uIButtonMessage != null)
			{
				uIButtonMessage.enabled = true;
				uIButtonMessage.trigger = UIButtonMessage.Trigger.OnClick;
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickCloseButton";
			}
		}
		TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "take_over");
		TextUtility.SetCommonText(this.m_passwordTextLabel, "Option", "password");
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component2 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component2;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		if (this.m_passwordLabel != null)
		{
			this.m_passwordLabel.text = string.Empty;
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	private void OnClickCloseButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
	}

	private void RequestServerGetMigrationPassword()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			if (string.IsNullOrEmpty(ServerInterface.MigrationPassword))
			{
				loggedInServerInterface.RequestServerGetMigrationPassword(null, base.gameObject);
			}
			else if (this.m_passwordLabel != null)
			{
				this.m_passwordLabel.text = ServerInterface.MigrationPassword;
			}
		}
	}

	public void PlayOpenWindow()
	{
		this.RequestServerGetMigrationPassword();
		this.m_isEnd = false;
		if (this.m_uiAnimation != null)
		{
			this.m_uiAnimation.Play(true);
		}
	}

	private void ServerGetMigrationPassword_Succeeded(MsgGetMigrationPasswordSucceed msg)
	{
		if (msg != null && this.m_passwordLabel != null)
		{
			this.m_passwordLabel.text = msg.m_migrationPassword;
		}
	}

	private void ServerGetMigrationPassword_Failed()
	{
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
