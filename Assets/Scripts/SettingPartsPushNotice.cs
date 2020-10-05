using AnimationOrTween;
using App.Utility;
using SaveData;
using System;
using Text;
using UnityEngine;

public class SettingPartsPushNotice : SettingBase
{
	private enum InputState
	{
		INPUTTING,
		PRESSED_ON,
		PRESSED_OFF,
		CANCELED
	}

	private class InfoButton
	{
		public string Name;

		public string FunctionName;

		public InfoButton(string s1, string s2)
		{
			this.Name = s1;
			this.FunctionName = s2;
		}
	}

	private static GameObject m_prefab;

	private GameObject m_object;

	private UIPlayAnimation m_uiAnimation;

	private SettingPartsPushNotice.InputState m_inputState;

	private string m_anchorPath;

	private readonly string ExcludePathName = "UI Root (2D)/Camera/Anchor_5_MC";

	private bool m_isEnd;

	private bool m_isOverwrite;

	private bool m_isLoaded;

	private bool m_playStartCue;

	private GameObject m_closeBtn;

	private int m_closeBtnEnabled = -1;

	private bool m_LocalPushNoticeFlag;

	private Bitset32 m_infoStateBackup = new Bitset32(0u);

	private UIToggle[] m_InfoStatusToggle = new UIToggle[3];

	private bool m_isWindowOpen;

	private SettingPartsPushNotice.InfoButton[] InfoCheckToggleButton = new SettingPartsPushNotice.InfoButton[]
	{
		new SettingPartsPushNotice.InfoButton("img_check_box_0", "OnClickEventInfoButton"),
		new SettingPartsPushNotice.InfoButton("img_check_box_1", "OnClickChallengeInfoButton"),
		new SettingPartsPushNotice.InfoButton("img_check_box_2", "OnClickFriendInfoButton")
	};

	public bool IsPressedOn
	{
		get
		{
			return this.m_inputState == SettingPartsPushNotice.InputState.PRESSED_ON;
		}
		private set
		{
		}
	}

	public bool IsPressedOff
	{
		get
		{
			return this.m_inputState == SettingPartsPushNotice.InputState.PRESSED_OFF;
		}
		private set
		{
		}
	}

	public bool IsCanceled
	{
		get
		{
			return this.m_inputState == SettingPartsPushNotice.InputState.CANCELED;
		}
		private set
		{
		}
	}

	public bool IsOverwrite
	{
		get
		{
			return this.m_isOverwrite;
		}
		private set
		{
		}
	}

	public bool IsLoaded
	{
		get
		{
			return this.m_isLoaded;
		}
	}

	public void SetWindowActive(bool flag)
	{
		if (this.m_object != null)
		{
			this.m_object.SetActive(flag);
		}
	}

	protected override void OnSetup(string anthorPath)
	{
		if (!this.m_isLoaded)
		{
			this.m_anchorPath = this.ExcludePathName;
		}
	}

	protected override void OnPlayStart()
	{
		this.m_isEnd = false;
		this.m_playStartCue = false;
		this.m_isWindowOpen = false;
		if (this.m_isLoaded)
		{
			this.SetWindowActive(true);
			if (this.m_uiAnimation != null)
			{
				EventDelegate.Add(this.m_uiAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedOpenAnimationCallback), true);
				this.m_uiAnimation.Play(true);
			}
			this.m_inputState = SettingPartsPushNotice.InputState.INPUTTING;
			this.m_isOverwrite = false;
			SoundManager.SePlay("sys_window_open", "SE");
			BackKeyManager.AddWindowCallBack(base.gameObject);
		}
		else
		{
			this.m_playStartCue = true;
		}
	}

	protected override bool OnIsEndPlay()
	{
		return this.m_isEnd;
	}

	protected override void OnUpdate()
	{
		if (!this.m_isLoaded)
		{
			this.m_isLoaded = true;
			base.enabled = false;
			this.SetupWindowData();
			if (this.m_playStartCue)
			{
				this.OnPlayStart();
			}
		}
		if (this.m_closeBtnEnabled != -1 && this.m_closeBtn != null)
		{
			if (this.m_closeBtnEnabled == 1)
			{
				this.m_closeBtn.SetActive(true);
			}
			else
			{
				this.m_closeBtn.SetActive(false);
			}
			this.m_closeBtnEnabled = -1;
		}
	}

	private void SetupWindowData()
	{
		if (SettingPartsPushNotice.m_prefab == null)
		{
			SettingPartsPushNotice.m_prefab = (Resources.Load("Prefabs/UI/window_pushinfo_setting2") as GameObject);
		}
		this.m_object = (UnityEngine.Object.Instantiate(SettingPartsPushNotice.m_prefab, Vector3.zero, Quaternion.identity) as GameObject);
		if (this.m_object != null)
		{
			GameObject gameObject = GameObject.Find(this.m_anchorPath);
			if (gameObject != null)
			{
				Vector3 localPosition = new Vector3(0f, 0f, 0f);
				Vector3 localScale = this.m_object.transform.localScale;
				this.m_object.transform.parent = gameObject.transform;
				this.m_object.transform.localPosition = localPosition;
				this.m_object.transform.localScale = localScale;
			}
			window_pushinfo_setting component = this.m_object.GetComponent<window_pushinfo_setting>();
			if (component == null)
			{
				this.m_object.AddComponent<window_pushinfo_setting>();
			}
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					this.m_infoStateBackup = new Bitset32(systemdata.pushNoticeFlags);
				}
				this.m_LocalPushNoticeFlag = this.IsPushNotice();
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_ok");
			if (gameObject2 != null)
			{
				this.m_closeBtn = gameObject2;
				UIButtonMessage uIButtonMessage = gameObject2.AddComponent<UIButtonMessage>();
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickOkButton";
			}
			for (int i = 0; i < 3; i++)
			{
				if (i != 2)
				{
					GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_object, this.InfoCheckToggleButton[i].Name);
					if (gameObject3 != null)
					{
						UIButtonMessage uIButtonMessage2 = gameObject3.AddComponent<UIButtonMessage>();
						uIButtonMessage2.target = base.gameObject;
						uIButtonMessage2.functionName = this.InfoCheckToggleButton[i].FunctionName;
						UIToggle component2 = gameObject3.GetComponent<UIToggle>();
						if (component2 != null)
						{
							component2.value = this.IsPushNoticeFlagStatus((SystemData.PushNoticeFlagStatus)i);
							this.m_InfoStatusToggle[i] = component2;
						}
					}
				}
			}
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_on");
			if (gameObject4 != null)
			{
				UIButtonMessage uIButtonMessage3 = gameObject4.AddComponent<UIButtonMessage>();
				uIButtonMessage3.target = base.gameObject;
				uIButtonMessage3.functionName = "OnClickOnButton";
				UIToggle component3 = gameObject4.GetComponent<UIToggle>();
				if (component3 != null)
				{
					component3.value = this.m_LocalPushNoticeFlag;
				}
			}
			GameObject gameObject5 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_off");
			if (gameObject5 != null)
			{
				UIButtonMessage uIButtonMessage4 = gameObject5.AddComponent<UIButtonMessage>();
				uIButtonMessage4.target = base.gameObject;
				uIButtonMessage4.functionName = "OnClickOffButton";
				UIToggle component4 = gameObject5.GetComponent<UIToggle>();
				if (component4 != null)
				{
					component4.value = !this.m_LocalPushNoticeFlag;
				}
			}
			TextManager.TextType type = TextManager.TextType.TEXTTYPE_FIXATION_TEXT;
			GameObject gameObject6 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_pushinfo_setting");
			if (gameObject6 != null)
			{
				UILabel component5 = gameObject6.GetComponent<UILabel>();
				if (component5 != null)
				{
					TextUtility.SetText(component5, type, "Option", "push_notification");
				}
			}
			GameObject gameObject7 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_pushinfo_setting_sub");
			if (gameObject7 != null)
			{
				UILabel component6 = gameObject7.GetComponent<UILabel>();
				if (component6 != null)
				{
					TextUtility.SetText(component6, type, "Option", "push_notification_info");
				}
			}
			this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
			if (this.m_uiAnimation != null)
			{
				Animation component7 = this.m_object.GetComponent<Animation>();
				this.m_uiAnimation.target = component7;
				this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
			}
			this.m_object.SetActive(false);
			global::Debug.Log("SettingPartsPushNotice:SetupWindowData End");
		}
	}

	private void PlayCloseAnimation()
	{
		if (this.m_object != null)
		{
			Animation component = this.m_object.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Reverse);
				if (activeAnimation != null)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), true);
					this.m_isWindowOpen = false;
				}
			}
		}
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OverwriteSystemData()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				if (systemdata.pushNotice != this.m_LocalPushNoticeFlag)
				{
					systemdata.pushNotice = this.m_LocalPushNoticeFlag;
					LocalNotification.EnableNotification(this.m_LocalPushNoticeFlag);
					this.m_isOverwrite = true;
				}
				PnoteNotification.RegistTagsPnote(systemdata.pushNoticeFlags);
				if (systemdata.pushNoticeFlags != this.m_infoStateBackup)
				{
					this.m_isOverwrite = true;
				}
			}
		}
	}

	private bool IsPushNotice()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				return systemdata.pushNotice;
			}
		}
		return false;
	}

	private bool IsPushNoticeFlagStatus(SystemData.PushNoticeFlagStatus state)
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				return systemdata.IsFlagStatus(state);
			}
		}
		return false;
	}

	private void SetPushNoticeFlagStatus(SystemData.PushNoticeFlagStatus state, bool flag)
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				systemdata.SetFlagStatus(state, flag);
			}
		}
	}

	public void SetCloseButtonEnabled(bool enabled)
	{
		if (enabled)
		{
			this.m_closeBtnEnabled = 1;
		}
		else
		{
			this.m_closeBtnEnabled = 1;
			enabled = true;
		}
		if (this.m_closeBtn != null)
		{
			this.m_closeBtn.SetActive(enabled);
			this.m_closeBtnEnabled = -1;
		}
	}

	private void OnClickCancelButton()
	{
		this.PlayCloseAnimation();
		this.m_inputState = SettingPartsPushNotice.InputState.CANCELED;
	}

	private void OnClickOnButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		this.m_inputState = SettingPartsPushNotice.InputState.PRESSED_ON;
		this.m_LocalPushNoticeFlag = true;
	}

	private void OnClickOffButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		this.m_inputState = SettingPartsPushNotice.InputState.PRESSED_OFF;
		this.m_LocalPushNoticeFlag = false;
	}

	private void OnClickOkButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		this.PlayCloseAnimation();
		this.OverwriteSystemData();
	}

	private void OnClickEventInfoButton()
	{
		this.InfoCheckBoxCommon(SystemData.PushNoticeFlagStatus.EVENT_INFO);
	}

	private void OnClickChallengeInfoButton()
	{
		this.InfoCheckBoxCommon(SystemData.PushNoticeFlagStatus.CHALLENGE_INFO);
	}

	private void OnClickFriendInfoButton()
	{
		this.InfoCheckBoxCommon(SystemData.PushNoticeFlagStatus.FRIEND_INFO);
	}

	private void InfoCheckBoxCommon(SystemData.PushNoticeFlagStatus status)
	{
		UIToggle uIToggle = this.m_InfoStatusToggle[(int)status];
		bool flag = false;
		if (uIToggle != null)
		{
			flag = uIToggle.value;
			this.SetPushNoticeFlagStatus(status, uIToggle.value);
		}
		if (flag)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
		else
		{
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void OnFinishedOpenAnimationCallback()
	{
		if (!this.m_isWindowOpen)
		{
			this.m_isWindowOpen = true;
		}
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
		if (this.m_object != null)
		{
			this.m_object.SetActive(false);
			BackKeyManager.RemoveWindowCallBack(base.gameObject);
		}
	}

	public void OnClickPlatformBackButton()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (this.m_isWindowOpen)
		{
			this.OnClickOkButton();
		}
	}
}
