using AnimationOrTween;
using Message;
using SaveData;
using System;
using Text;
using UnityEngine;

public class window_performance_setting : WindowBase
{
	public enum State
	{
		EXEC,
		CLOSE
	}

	[SerializeField]
	private UIToggle m_checkBox0;

	[SerializeField]
	private UIToggle m_checkBox1;

	private window_performance_setting.State m_State;

	private bool m_selected;

	private UIPlayAnimation m_uiAnimation;

	private bool m_isToggleLock;

	private bool m_isEnd;

	private bool m_isChangeCheckBox0;

	private bool m_isChangeCheckBox1;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public window_performance_setting.State EndState
	{
		get
		{
			return this.m_State;
		}
	}

	private void Start()
	{
		OptionMenuUtility.TranObj(base.gameObject);
		base.enabled = false;
		this.m_selected = false;
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component;
			this.m_uiAnimation.clipName = "ui_cmn_window_Anim2";
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	public void Setup()
	{
		this.m_selected = false;
		this.m_State = window_performance_setting.State.EXEC;
		this.UpdateButtonImage();
	}

	private void Update()
	{
		if (GeneralWindow.IsCreated("BackTitleSelect") && this.m_selected && GeneralWindow.IsButtonPressed)
		{
			bool flag = this.IsLightMode();
			bool flag2 = this.IsHighTexture();
			if (this.m_isChangeCheckBox0)
			{
				this.SaveSystemData(!flag);
			}
			if (this.m_isChangeCheckBox1)
			{
				this.SaveSystemDataTex(!flag2);
			}
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				instance.SaveSystemData();
			}
			HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.TITLE);
			GeneralWindow.Close();
			base.enabled = false;
			this.PlayCloseAnimation();
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

	private bool IsLightMode()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				return systemdata.lightMode;
			}
		}
		return false;
	}

	private bool IsHighTexture()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				return systemdata.highTexture;
			}
		}
		return false;
	}

	private void UpdateButtonImage()
	{
		bool startsActive = this.IsLightMode();
		bool flag = this.IsHighTexture();
		this.m_isChangeCheckBox0 = false;
		this.m_isChangeCheckBox1 = false;
		this.m_isToggleLock = true;
		if (this.m_checkBox0 != null)
		{
			this.m_checkBox0.startsActive = startsActive;
			this.m_checkBox0.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
		}
		if (this.m_checkBox1 != null)
		{
			this.m_checkBox1.startsActive = !flag;
			this.m_checkBox1.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
		}
		this.m_isToggleLock = false;
	}

	private void SaveSystemData(bool lightModeFlag)
	{
		bool flag = this.IsLightMode();
		if (flag != lightModeFlag)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					systemdata.lightMode = lightModeFlag;
				}
			}
		}
	}

	private void SaveSystemDataTex(bool texModeFlag)
	{
		bool flag = this.IsHighTexture();
		if (flag != texModeFlag)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					systemdata.highTexture = texModeFlag;
				}
			}
		}
	}

	private void ShowBackTileMessage()
	{
		if (this.m_isChangeCheckBox0 || this.m_isChangeCheckBox1)
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "BackTitleSelect",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("MainMenu", "back_title_caption"),
				message = TextUtility.GetCommonText("Option", "weight_saving_back_title_text")
			});
			this.m_selected = true;
			base.enabled = true;
		}
		else
		{
			this.PlayCloseAnimation();
		}
	}

	public void OnChangeCheckBox0()
	{
		if (!this.m_isToggleLock)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_isChangeCheckBox0 = !this.m_isChangeCheckBox0;
		}
	}

	public void OnChangeCheckBox1()
	{
		if (!this.m_isToggleLock)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_isChangeCheckBox1 = !this.m_isChangeCheckBox1;
		}
	}

	private void OnClickOkButton()
	{
		this.m_State = window_performance_setting.State.CLOSE;
		this.m_isToggleLock = false;
		this.ShowBackTileMessage();
	}

	private void OnClickCloseButton()
	{
		this.m_State = window_performance_setting.State.CLOSE;
		this.m_isToggleLock = false;
		this.PlayCloseAnimation();
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
		this.m_selected = false;
	}

	public void PlayOpenWindow()
	{
		this.m_isEnd = false;
		if (this.m_uiAnimation != null)
		{
			this.m_uiAnimation.Play(true);
			base.enabled = true;
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_close");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}
}
