using AnimationOrTween;
using Message;
using SaveData;
using System;
using Text;
using UnityEngine;

public class window_event_setting : WindowBase
{
	public enum TextType
	{
		WEIGHT_SAVING,
		FACEBOOK_ACCESS
	}

	public enum State
	{
		EXEC,
		PRESS_LOGIN,
		PRESS_LOGOUT,
		CLOSE
	}

	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private GameObject m_liteButton;

	[SerializeField]
	private GameObject m_textureButton;

	[SerializeField]
	private UIImageButton m_onButtonLite;

	[SerializeField]
	private UIImageButton m_offButtonLite;

	[SerializeField]
	private UIImageButton m_onButtonTex;

	[SerializeField]
	private UIImageButton m_offButtonTex;

	private window_event_setting.TextType m_textType;

	private window_event_setting.State m_State;

	[SerializeField]
	private UILabel m_headerTextLabel;

	[SerializeField]
	private UILabel m_headerSubTextLabel;

	[SerializeField]
	private UILabel m_ButtonOnTextLabel;

	[SerializeField]
	private UILabel m_ButtonOnSubTextLabel;

	[SerializeField]
	private UILabel m_ButtonOffTextLabel;

	[SerializeField]
	private UILabel m_ButtonOffSubTextLabel;

	private UIPlayAnimation m_uiAnimation;

	private bool m_isEnd;

	private bool m_isOverwrite;

	private float m_initY = -3000f;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public bool IsOverwrite
	{
		get
		{
			return this.m_isOverwrite;
		}
	}

	public window_event_setting.State EndState
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
		}
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component2 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component2;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	public void Setup(window_event_setting.TextType textType)
	{
		this.m_State = window_event_setting.State.EXEC;
		this.m_textType = textType;
		window_event_setting.TextType textType2 = this.m_textType;
		if (textType2 != window_event_setting.TextType.WEIGHT_SAVING)
		{
			if (textType2 == window_event_setting.TextType.FACEBOOK_ACCESS)
			{
				bool lightValue = this.IsLogin();
				this.UpdateButtonImage(lightValue, false);
				TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "facebook_access");
				TextUtility.SetCommonText(this.m_headerSubTextLabel, "Option", "facebook_access_info");
				TextUtility.SetCommonText(this.m_ButtonOnTextLabel, "Option", "login");
				TextUtility.SetCommonText(this.m_ButtonOnSubTextLabel, "Option", "login");
				TextUtility.SetCommonText(this.m_ButtonOffTextLabel, "Option", "logout");
				TextUtility.SetCommonText(this.m_ButtonOffSubTextLabel, "Option", "logout");
			}
		}
		else
		{
			bool lightValue2 = this.IsLightMode();
			bool texValue = this.IsHighTexture();
			this.UpdateButtonImage(lightValue2, texValue);
			TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "weight_saving");
			TextUtility.SetCommonText(this.m_headerSubTextLabel, "Option", "weight_saving_info");
			TextUtility.SetCommonText(this.m_ButtonOnTextLabel, "Option", "button_on");
			TextUtility.SetCommonText(this.m_ButtonOnSubTextLabel, "Option", "button_on");
			TextUtility.SetCommonText(this.m_ButtonOffTextLabel, "Option", "button_off");
			TextUtility.SetCommonText(this.m_ButtonOffSubTextLabel, "Option", "button_off");
		}
	}

	private void Update()
	{
		if (GeneralWindow.IsCreated("BackTitleSelect") && GeneralWindow.IsButtonPressed)
		{
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

	private bool IsLogin()
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		return socialInterface != null && socialInterface.IsLoggedIn;
	}

	private void UpdatePosition()
	{
		if (this.m_liteButton == null || this.m_textureButton == null)
		{
			return;
		}
		if (this.m_textType == window_event_setting.TextType.WEIGHT_SAVING)
		{
			if (this.m_initY == -3000f)
			{
				this.m_initY = this.m_liteButton.transform.localPosition.y;
			}
			this.m_liteButton.transform.localPosition = new Vector3(this.m_liteButton.transform.localPosition.x, this.m_initY, this.m_liteButton.transform.localPosition.x);
			this.m_textureButton.SetActive(true);
		}
		else
		{
			if (this.m_initY == -3000f)
			{
				this.m_initY = this.m_liteButton.transform.localPosition.y;
			}
			float y = this.m_initY + (this.m_textureButton.transform.localPosition.y - this.m_initY) * 0.5f;
			this.m_liteButton.transform.localPosition = new Vector3(this.m_liteButton.transform.localPosition.x, y, this.m_liteButton.transform.localPosition.x);
			this.m_textureButton.SetActive(false);
		}
	}

	private void UpdateButtonImage(bool lightValue, bool texValue)
	{
		this.UpdatePosition();
		window_event_setting.TextType textType = this.m_textType;
		if (textType != window_event_setting.TextType.WEIGHT_SAVING)
		{
			if (textType == window_event_setting.TextType.FACEBOOK_ACCESS)
			{
				if (this.m_onButtonLite != null)
				{
					if (lightValue)
					{
						this.m_onButtonLite.gameObject.SetActive(false);
					}
					else
					{
						this.m_onButtonLite.gameObject.SetActive(true);
					}
				}
				if (this.m_offButtonLite != null)
				{
					if (!lightValue)
					{
						this.m_offButtonLite.gameObject.SetActive(false);
					}
					else
					{
						this.m_offButtonLite.gameObject.SetActive(true);
					}
				}
			}
		}
		else
		{
			if (this.m_onButtonTex != null)
			{
				if (texValue)
				{
					this.m_onButtonTex.gameObject.SetActive(false);
				}
				else
				{
					this.m_onButtonTex.gameObject.SetActive(true);
				}
			}
			if (this.m_offButtonTex != null)
			{
				if (!texValue)
				{
					this.m_offButtonTex.gameObject.SetActive(false);
				}
				else
				{
					this.m_offButtonTex.gameObject.SetActive(true);
				}
			}
			if (this.m_onButtonLite != null)
			{
				if (lightValue)
				{
					this.m_onButtonLite.gameObject.SetActive(false);
				}
				else
				{
					this.m_onButtonLite.gameObject.SetActive(true);
				}
			}
			if (this.m_offButtonLite != null)
			{
				if (!lightValue)
				{
					this.m_offButtonLite.gameObject.SetActive(false);
				}
				else
				{
					this.m_offButtonLite.gameObject.SetActive(true);
				}
			}
		}
	}

	private void SaveSystemData(bool lightModeFlag)
	{
		bool flag = this.IsLightMode();
		if (flag != lightModeFlag)
		{
			this.m_isOverwrite = true;
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
			this.m_isOverwrite = true;
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

	private void OnClickOnButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		window_event_setting.TextType textType = this.m_textType;
		if (textType != window_event_setting.TextType.WEIGHT_SAVING)
		{
			if (textType == window_event_setting.TextType.FACEBOOK_ACCESS)
			{
				this.m_State = window_event_setting.State.PRESS_LOGIN;
				this.PlayCloseAnimation();
			}
		}
		else
		{
			this.SaveSystemData(true);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "BackTitleSelect",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("MainMenu", "back_title_caption"),
				message = TextUtility.GetCommonText("Option", "weight_saving_back_title_text")
			});
			base.enabled = true;
		}
	}

	private void OnClickOffButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		window_event_setting.TextType textType = this.m_textType;
		if (textType != window_event_setting.TextType.WEIGHT_SAVING)
		{
			if (textType == window_event_setting.TextType.FACEBOOK_ACCESS)
			{
				this.m_State = window_event_setting.State.PRESS_LOGOUT;
				this.PlayCloseAnimation();
			}
		}
		else
		{
			this.SaveSystemData(false);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "BackTitleSelect",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("MainMenu", "back_title_caption"),
				message = TextUtility.GetCommonText("Option", "weight_saving_back_title_text")
			});
			base.enabled = true;
		}
	}

	private void OnClickTexOnButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		window_event_setting.TextType textType = this.m_textType;
		if (textType != window_event_setting.TextType.WEIGHT_SAVING)
		{
			if (textType == window_event_setting.TextType.FACEBOOK_ACCESS)
			{
				this.PlayCloseAnimation();
			}
		}
		else
		{
			this.SaveSystemDataTex(true);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "BackTitleSelect",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("MainMenu", "back_title_caption"),
				message = TextUtility.GetCommonText("Option", "weight_saving_back_title_text")
			});
			base.enabled = true;
		}
	}

	private void OnClickTexOffButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		window_event_setting.TextType textType = this.m_textType;
		if (textType != window_event_setting.TextType.WEIGHT_SAVING)
		{
			if (textType == window_event_setting.TextType.FACEBOOK_ACCESS)
			{
				this.PlayCloseAnimation();
			}
		}
		else
		{
			this.SaveSystemDataTex(false);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "BackTitleSelect",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("MainMenu", "back_title_caption"),
				message = TextUtility.GetCommonText("Option", "weight_saving_back_title_text")
			});
			base.enabled = true;
		}
	}

	private void OnClickCloseButton()
	{
		this.m_State = window_event_setting.State.CLOSE;
		this.PlayCloseAnimation();
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
	}

	public void PlayOpenWindow()
	{
		this.m_isEnd = false;
		if (this.m_uiAnimation != null)
		{
			this.m_uiAnimation.Play(true);
			base.enabled = false;
		}
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
