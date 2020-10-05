using AnimationOrTween;
using SaveData;
using System;
using Text;
using UnityEngine;

public class window_id_info : WindowBase
{
	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private UILabel m_headerTextLabel;

	[SerializeField]
	private UILabel m_yourIDLabel;

	[SerializeField]
	private UILabel m_idLabel;

	private bool m_isEnd;

	private bool m_isResetPassEnd;

	private UIPlayAnimation m_uiAnimation;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public bool IsPassResetEnd
	{
		get
		{
			return this.m_isResetPassEnd;
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
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_copy");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.enabled = true;
			uIButtonMessage.trigger = UIButtonMessage.Trigger.OnClick;
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnClickClipboard";
		}
		UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_passset");
		if (uIButtonMessage2 != null)
		{
			uIButtonMessage2.enabled = true;
			uIButtonMessage2.trigger = UIButtonMessage.Trigger.OnClick;
			uIButtonMessage2.target = base.gameObject;
			uIButtonMessage2.functionName = "OnClickPassSet";
		}
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component3 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component3;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "id_check");
		TextUtility.SetCommonText(this.m_yourIDLabel, "Option", "your_id");
		if (this.m_idLabel != null)
		{
			string viewUserID = this.GetViewUserID();
			this.m_idLabel.text = viewUserID;
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

	public void PlayOpenWindow()
	{
		this.m_isEnd = false;
		this.m_isResetPassEnd = false;
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_num_takeover");
		if (uILabel != null)
		{
			uILabel.text = SystemSaveManager.GetTakeoverID();
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_num_pass");
		if (uILabel2 != null)
		{
			uILabel2.text = SystemSaveManager.GetTakeoverPassword();
		}
		if (this.m_uiAnimation != null)
		{
			this.m_uiAnimation.Play(true);
		}
	}

	private void PlayCloseAnimation()
	{
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), true);
			}
		}
		SoundManager.SePlay("sys_window_close", "SE");
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

	private void OnClickClipboard()
	{
		string gameID = SystemSaveManager.GetGameID();
		string takeoverID = SystemSaveManager.GetTakeoverID();
		string text = gameID + "\n" + takeoverID;
		Clipboard.text = text;
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickPassSet()
	{
		this.PlayCloseAnimation();
		this.m_isResetPassEnd = true;
	}

	private string GetViewUserID()
	{
		string text = SystemSaveManager.GetGameID();
		if (text.Length > 7)
		{
			text = text.Insert(6, " ");
			text = text.Insert(3, " ");
		}
		return text;
	}
}
