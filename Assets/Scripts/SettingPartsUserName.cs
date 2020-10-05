using System;
using Text;
using UnityEngine;

public class SettingPartsUserName : SettingBase
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

	private SettingPartsUserName.State m_state;

	private GameObject m_object;

	private UIPlayAnimation m_uiAnimation;

	private UIInput m_input;

	private UILabel m_label;

	private string m_anchorPath;

	private readonly string ExcludePathName = "UI Root (2D)/Camera/Anchor_5_MC";

	private bool m_calcelButtonUseFlag = true;

	private bool m_isLoaded;

	private bool m_playStartCue;

	private SettingPartsUserName.InputState m_inputState;

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
			return this.m_inputState == SettingPartsUserName.InputState.DECIDED;
		}
		private set
		{
		}
	}

	public bool IsCanceled
	{
		get
		{
			return this.m_inputState == SettingPartsUserName.InputState.CANCELED;
		}
		private set
		{
		}
	}

	public void SetCancelButtonUseFlag(bool useFlag)
	{
		this.m_calcelButtonUseFlag = useFlag;
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
			this.m_state = SettingPartsUserName.State.STATE_LOAD;
		}
	}

	private void SetupWindowData()
	{
		this.m_object = base.gameObject;
		if (this.m_object != null)
		{
			this.m_input = GameObjectUtil.FindChildGameObjectComponent<UIInput>(this.m_object, "Input_name");
			EventDelegate item = new EventDelegate(new EventDelegate.Callback(this.OnFinishedInput));
			this.m_input.onSubmit.Add(item);
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_ok");
			if (gameObject != null)
			{
				UIButtonMessage uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickOkButton";
				UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
				if (component != null)
				{
					EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedAnimation), false);
				}
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_close");
			if (gameObject2 != null)
			{
				UIButtonMessage uIButtonMessage2 = gameObject2.AddComponent<UIButtonMessage>();
				uIButtonMessage2.target = base.gameObject;
				uIButtonMessage2.functionName = "OnClickCancelButton";
				window_name_setting component2 = this.m_object.GetComponent<window_name_setting>();
				if (component2 == null)
				{
					this.m_object.AddComponent<window_name_setting>();
				}
				UIPlayAnimation component3 = gameObject2.GetComponent<UIPlayAnimation>();
				if (component3 != null)
				{
					EventDelegate.Add(component3.onFinished, new EventDelegate.Callback(this.OnFinishedAnimation), false);
				}
				gameObject2.SetActive(this.m_calcelButtonUseFlag);
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_name_setting");
			if (gameObject3 != null)
			{
				UILabel component4 = gameObject3.GetComponent<UILabel>();
				if (component4 != null)
				{
					TextUtility.SetCommonText(component4, "UserName", "name_setting");
				}
			}
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_name_setting_sub");
			if (gameObject4 != null)
			{
				UILabel component5 = gameObject4.GetComponent<UILabel>();
				if (component5 != null)
				{
					TextUtility.SetCommonText(component5, "UserName", "name_setting_info");
				}
			}
			GameObject gameObject5 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_input_name");
			if (gameObject5 != null)
			{
				this.m_label = gameObject5.GetComponent<UILabel>();
				if (this.m_label != null)
				{
					string text = null;
					ServerSettingState settingState = ServerInterface.SettingState;
					if (settingState != null)
					{
						text = settingState.m_userName;
					}
					if (string.IsNullOrEmpty(text))
					{
						TextUtility.SetCommonText(this.m_label, "UserName", "input_name");
					}
					else
					{
						this.m_label.text = text;
					}
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
				GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_close");
				if (gameObject != null)
				{
					gameObject.SetActive(this.m_calcelButtonUseFlag);
				}
			}
			if (this.m_uiAnimation != null)
			{
				this.m_uiAnimation.Play(true);
			}
			this.m_inputState = SettingPartsUserName.InputState.INPUTTING;
			this.m_state = SettingPartsUserName.State.STATE_SETTING;
			SoundManager.SePlay("sys_window_open", "SE");
		}
		else
		{
			this.m_playStartCue = true;
		}
	}

	protected override bool OnIsEndPlay()
	{
		return this.m_state == SettingPartsUserName.State.STATE_END;
	}

	protected override void OnUpdate()
	{
		switch (this.m_state)
		{
		case SettingPartsUserName.State.STATE_LOAD:
			this.m_isLoaded = true;
			this.SetupWindowData();
			if (this.m_playStartCue)
			{
				this.OnPlayStart();
			}
			break;
		case SettingPartsUserName.State.STATE_SETTING:
			if (this.m_input.selected)
			{
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "UserName", "input_name").text;
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_object, "Lbl_input_name");
				UIInput uIInput = GameObjectUtil.FindChildGameObjectComponent<UIInput>(this.m_object, "Input_name");
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
			if (this.m_inputState == SettingPartsUserName.InputState.DECIDED || this.m_inputState == SettingPartsUserName.InputState.CANCELED)
			{
				this.m_state = SettingPartsUserName.State.STATE_WAIT_END;
			}
			break;
		}
	}

	private void OnFinishedAnimation()
	{
		this.m_state = SettingPartsUserName.State.STATE_END;
	}

	private void OnClickOkButton()
	{
		if (this.m_inputState == SettingPartsUserName.InputState.DECIDED)
		{
			return;
		}
		this.m_inputState = SettingPartsUserName.InputState.DECIDED;
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickCancelButton()
	{
		if (this.m_inputState == SettingPartsUserName.InputState.CANCELED)
		{
			return;
		}
		this.m_input.value = ServerInterface.SettingState.m_userName;
		this.m_inputState = SettingPartsUserName.InputState.CANCELED;
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnFinishedInput()
	{
		global::Debug.Log("Input Finished! Input Text is" + this.m_input.value);
	}
}
