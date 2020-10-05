using AnimationOrTween;
using System;
using Text;
using UnityEngine;

public class SettingTakeoverInput : SettingBase
{
	private enum InputState
	{
		INPUTTING,
		DICIDE,
		CANCELED
	}

	private class textLabelData
	{
		public string label;

		public string text_label;

		public textLabelData(string s1, string s2)
		{
			this.label = s1;
			this.text_label = s2;
		}
	}

	private enum textKind
	{
		HEADER,
		INPUT_ID,
		INPUT_ID_SPACE,
		INPUT_PASS,
		INPUT_PASS_SPACE,
		END
	}

	private static GameObject m_prefab;

	private GameObject m_object;

	private UIPlayAnimation m_uiAnimation;

	private SettingTakeoverInput.InputState m_inputState;

	private string m_anchorPath;

	private readonly string ExcludePathName = "UI Root (2D)/Camera/Anchor_5_MC";

	private bool m_isEnd;

	private bool m_isLoaded;

	private bool m_playStartCue;

	private bool m_isWindowOpen;

	private UIInput m_inputId;

	private UIInput m_inputPass;

	private SettingTakeoverInput.textLabelData[] textParamTable = new SettingTakeoverInput.textLabelData[]
	{
		new SettingTakeoverInput.textLabelData("Lbl_id_info", "takeover_input_header"),
		new SettingTakeoverInput.textLabelData("Lbl_word_id", "takeover_input_id_head"),
		new SettingTakeoverInput.textLabelData("Lbl_input_id", "takeover_input_id_space"),
		new SettingTakeoverInput.textLabelData("Lbl_word_pass", "takeover_input_pass_head"),
		new SettingTakeoverInput.textLabelData("Lbl_input_pass", "takeover_input_pass_space")
	};

	public bool IsDicide
	{
		get
		{
			return this.m_inputState == SettingTakeoverInput.InputState.DICIDE;
		}
		private set
		{
		}
	}

	public bool IsCanceled
	{
		get
		{
			return this.m_inputState == SettingTakeoverInput.InputState.CANCELED;
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

	public string InputIdText
	{
		get
		{
			if (this.m_inputId == null)
			{
				return string.Empty;
			}
			return this.m_inputId.value;
		}
		private set
		{
		}
	}

	public string InputPassText
	{
		get
		{
			if (this.m_inputPass == null)
			{
				return string.Empty;
			}
			return this.m_inputPass.value;
		}
		private set
		{
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
			base.enabled = true;
			if (this.m_uiAnimation != null)
			{
				EventDelegate.Add(this.m_uiAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedOpenAnimationCallback), true);
				this.m_uiAnimation.Play(true);
			}
			this.m_inputState = SettingTakeoverInput.InputState.INPUTTING;
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
	}

	private void SetupWindowData()
	{
		if (SettingTakeoverInput.m_prefab == null)
		{
			SettingTakeoverInput.m_prefab = (Resources.Load("Prefabs/UI/window_takeover_id_input") as GameObject);
		}
		this.m_object = (UnityEngine.Object.Instantiate(SettingTakeoverInput.m_prefab, Vector3.zero, Quaternion.identity) as GameObject);
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
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_ok");
			if (gameObject2 != null)
			{
				UIButtonMessage uIButtonMessage = gameObject2.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = gameObject2.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickOkButton";
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_close");
			if (gameObject3 != null)
			{
				UIButtonMessage uIButtonMessage2 = gameObject3.GetComponent<UIButtonMessage>();
				if (uIButtonMessage2 == null)
				{
					uIButtonMessage2 = gameObject3.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage2.target = base.gameObject;
				uIButtonMessage2.functionName = "OnClickCancelButton";
			}
			TextManager.TextType type = TextManager.TextType.TEXTTYPE_FIXATION_TEXT;
			for (int i = 0; i < 5; i++)
			{
				GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_object, this.textParamTable[i].label);
				if (gameObject4 != null)
				{
					UILabel component = gameObject4.GetComponent<UILabel>();
					if (component != null)
					{
						TextUtility.SetText(component, type, "Title", this.textParamTable[i].text_label);
					}
				}
			}
			this.m_inputId = GameObjectUtil.FindChildGameObjectComponent<UIInput>(this.m_object, "Input_id");
			this.m_inputPass = GameObjectUtil.FindChildGameObjectComponent<UIInput>(this.m_object, "Input_pass");
			this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
			if (this.m_uiAnimation != null)
			{
				Animation component2 = this.m_object.GetComponent<Animation>();
				this.m_uiAnimation.target = component2;
				this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
			}
			this.m_object.SetActive(false);
			global::Debug.Log("SettingTakeoverInput:SetupWindowData End");
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

	private void OnClickCancelButton()
	{
		this.PlayCloseAnimation();
		this.m_inputState = SettingTakeoverInput.InputState.CANCELED;
	}

	private void OnClickOkButton()
	{
		this.PlayCloseAnimation();
		SoundManager.SePlay("sys_menu_decide", "SE");
		this.m_inputState = SettingTakeoverInput.InputState.DICIDE;
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
			this.OnClickCancelButton();
		}
	}
}
