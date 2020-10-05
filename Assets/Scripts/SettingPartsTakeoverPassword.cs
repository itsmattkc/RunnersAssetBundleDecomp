using System;
using Text;
using UnityEngine;

public class SettingPartsTakeoverPassword : SettingBase
{
	private enum State
	{
		STATE_NONE = -1,
		STATE_IDLE,
		STATE_LOAD,
		STATE_SETTING,
		STATE_WAIT_END,
		STATE_END
	}

	private enum InputState
	{
		INPUTTING,
		DECIDED,
		CANCELED
	}

	private SettingPartsTakeoverPassword.State m_state;

	private GameObject m_object;

	private UIPlayAnimation m_uiAnimation;

	private UIInput m_input;

	private UILabel m_label;

	private string m_anchorPath;

	private readonly string ExcludePathName = "UI Root (2D)/Camera/Anchor_5_MC";

	private bool m_calcelButtonUseFlag = true;

	private bool m_isLoaded;

	private bool m_playStartCue;

	private SettingPartsTakeoverPassword.InputState m_inputState;

	public string InputText
	{
		get
		{
			if (this.m_input == null)
			{
				return string.Empty;
			}
			return this.m_input.value;
		}
		private set
		{
		}
	}

	public UILabel TextLabel
	{
		get
		{
			return this.m_label;
		}
	}

	public bool IsDecided
	{
		get
		{
			return this.m_inputState == SettingPartsTakeoverPassword.InputState.DECIDED;
		}
		private set
		{
		}
	}

	public bool IsCanceled
	{
		get
		{
			return this.m_inputState == SettingPartsTakeoverPassword.InputState.CANCELED;
		}
		private set
		{
		}
	}

	public void SetCancelButtonUseFlag(bool useFlag)
	{
		this.m_calcelButtonUseFlag = useFlag;
	}

	public void SetOkButtonEnabled(bool enabled)
	{
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_object, "Btn_ok");
		if (uIImageButton != null)
		{
			uIImageButton.isEnabled = enabled;
		}
	}

	private void OnDestroy()
	{
		if (this.m_object != null)
		{
			UnityEngine.Object.Destroy(this.m_object);
		}
	}

	protected override void OnSetup(string anchorPath)
	{
		if (!this.m_isLoaded)
		{
			this.m_anchorPath = this.ExcludePathName;
			this.m_state = SettingPartsTakeoverPassword.State.STATE_LOAD;
		}
	}

	private void SetupWindowData()
	{
		this.m_object = HudMenuUtility.GetLoadMenuChildObject("window_password_setting", true);
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
			this.m_input = GameObjectUtil.FindChildGameObjectComponent<UIInput>(this.m_object, "Input_password");
			EventDelegate item = new EventDelegate(new EventDelegate.Callback(this.OnFinishedInput));
			this.m_input.onSubmit.Add(item);
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_ok");
			if (gameObject2 != null)
			{
				UIButtonMessage uIButtonMessage = gameObject2.AddComponent<UIButtonMessage>();
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickOkButton";
				UIPlayAnimation component = gameObject2.GetComponent<UIPlayAnimation>();
				if (component != null)
				{
					EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedAnimation), false);
				}
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_close");
			if (gameObject3 != null)
			{
				if (!this.m_calcelButtonUseFlag)
				{
					gameObject3.SetActive(false);
				}
				else
				{
					gameObject3.SetActive(true);
					UIButtonMessage uIButtonMessage2 = gameObject3.AddComponent<UIButtonMessage>();
					uIButtonMessage2.target = base.gameObject;
					uIButtonMessage2.functionName = "OnClickCancelButton";
					window_name_setting component2 = this.m_object.GetComponent<window_name_setting>();
					if (component2 == null)
					{
						this.m_object.AddComponent<window_name_setting>();
					}
				}
				UIPlayAnimation component3 = gameObject3.GetComponent<UIPlayAnimation>();
				if (component3 != null)
				{
					EventDelegate.Add(component3.onFinished, new EventDelegate.Callback(this.OnFinishedAnimation), false);
				}
			}
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_name_setting");
			if (gameObject4 != null)
			{
				UILabel component4 = gameObject4.GetComponent<UILabel>();
				if (component4 != null)
				{
					TextUtility.SetCommonText(component4, "Option", "take_over_password_setting");
				}
			}
			GameObject gameObject5 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_name_setting_sub");
			if (gameObject5 != null)
			{
				UILabel component5 = gameObject5.GetComponent<UILabel>();
				if (component5 != null)
				{
					TextUtility.SetCommonText(component5, "Option", "take_over_password_setting_info");
				}
			}
			GameObject gameObject6 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_input_password");
			if (gameObject6 != null)
			{
				this.m_label = gameObject6.GetComponent<UILabel>();
				if (this.m_label != null)
				{
					TextUtility.SetCommonText(this.m_label, "Option", "take_over_password_input");
				}
			}
			this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
			if (this.m_uiAnimation != null)
			{
				Animation component6 = this.m_object.GetComponent<Animation>();
				this.m_uiAnimation.target = component6;
				this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
			}
		}
	}

	protected override void OnPlayStart()
	{
		if (this.m_isLoaded)
		{
			this.m_playStartCue = false;
			if (this.m_object != null)
			{
				this.m_object.SetActive(true);
			}
			if (this.m_uiAnimation != null)
			{
				this.m_uiAnimation.Play(true);
			}
			this.m_inputState = SettingPartsTakeoverPassword.InputState.INPUTTING;
			this.m_state = SettingPartsTakeoverPassword.State.STATE_SETTING;
			UIInput uIInput = GameObjectUtil.FindChildGameObjectComponent<UIInput>(this.m_object, "Input_password");
			if (uIInput != null)
			{
				uIInput.value = string.Empty;
			}
			if (this.m_label != null)
			{
				TextUtility.SetCommonText(this.m_label, "Option", "take_over_password_input");
			}
			SoundManager.SePlay("sys_window_open", "SE");
		}
		else
		{
			this.m_playStartCue = true;
		}
	}

	protected override bool OnIsEndPlay()
	{
		return this.m_state == SettingPartsTakeoverPassword.State.STATE_END;
	}

	protected override void OnUpdate()
	{
		switch (this.m_state)
		{
		case SettingPartsTakeoverPassword.State.STATE_LOAD:
			this.m_isLoaded = true;
			this.SetupWindowData();
			if (this.m_playStartCue)
			{
				this.OnPlayStart();
			}
			break;
		case SettingPartsTakeoverPassword.State.STATE_SETTING:
			if (this.m_input.selected)
			{
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Option", "take_over_password_input").text;
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_object, "Lbl_input_password");
				UIInput uIInput = GameObjectUtil.FindChildGameObjectComponent<UIInput>(this.m_object, "Input_password");
				if (text != null && uIInput != null && uILabel != null)
				{
					string value = uIInput.value;
					if (value.IndexOf(text) >= 0)
					{
						uIInput.value = string.Empty;
						uILabel.text = string.Empty;
					}
				}
			}
			if (this.m_inputState == SettingPartsTakeoverPassword.InputState.DECIDED || this.m_inputState == SettingPartsTakeoverPassword.InputState.CANCELED)
			{
				this.m_state = SettingPartsTakeoverPassword.State.STATE_WAIT_END;
			}
			break;
		}
	}

	private void OnFinishedAnimation()
	{
		if (this.m_object != null)
		{
			this.m_object.SetActive(false);
		}
		this.m_state = SettingPartsTakeoverPassword.State.STATE_END;
	}

	private void OnClickOkButton()
	{
		this.m_inputState = SettingPartsTakeoverPassword.InputState.DECIDED;
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickCancelButton()
	{
		this.m_inputState = SettingPartsTakeoverPassword.InputState.CANCELED;
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnFinishedInput()
	{
		global::Debug.Log("Input Finished! Input Text is" + this.m_input.value);
	}
}
