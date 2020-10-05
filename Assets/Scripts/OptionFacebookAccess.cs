using System;
using Text;
using UnityEngine;

public class OptionFacebookAccess : MonoBehaviour
{
	private enum State
	{
		INIT,
		IDLE,
		LOGIN,
		LOGOUT,
		LOGOUT_COMPLETE_SETTING,
		LOGOUT_COMPLETE,
		CLOSE
	}

	private window_event_setting m_eventSetting;

	private GameObject m_gameObject;

	private ui_option_scroll m_ui_option_scroll;

	private bool m_initFlag;

	private EasySnsFeed m_easySnsFeed;

	private OptionFacebookAccess.State m_State;

	public void Setup(ui_option_scroll scroll)
	{
		base.enabled = true;
		if (this.m_ui_option_scroll == null && scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		if (this.m_gameObject != null)
		{
			this.m_initFlag = true;
			this.m_gameObject.SetActive(true);
			if (this.m_eventSetting != null)
			{
				this.m_eventSetting.Setup(window_event_setting.TextType.FACEBOOK_ACCESS);
				this.m_eventSetting.PlayOpenWindow();
				this.m_State = OptionFacebookAccess.State.IDLE;
			}
		}
		else
		{
			this.m_initFlag = false;
			this.m_gameObject = HudMenuUtility.GetLoadMenuChildObject("window_event_setting", true);
		}
	}

	private void SetEventSetting()
	{
		if (this.m_gameObject != null && this.m_eventSetting == null)
		{
			this.m_eventSetting = this.m_gameObject.GetComponent<window_event_setting>();
		}
	}

	public void Update()
	{
		if (!this.m_initFlag)
		{
			this.m_initFlag = true;
			this.SetEventSetting();
			if (this.m_eventSetting != null)
			{
				this.m_eventSetting.Setup(window_event_setting.TextType.FACEBOOK_ACCESS);
				this.m_eventSetting.PlayOpenWindow();
				this.m_State = OptionFacebookAccess.State.IDLE;
			}
		}
		else
		{
			switch (this.m_State)
			{
			case OptionFacebookAccess.State.IDLE:
				if (this.m_eventSetting != null && this.m_eventSetting.IsEnd)
				{
					switch (this.m_eventSetting.EndState)
					{
					case window_event_setting.State.PRESS_LOGIN:
						this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/Anchor_5_MC");
						this.m_State = OptionFacebookAccess.State.LOGIN;
						break;
					case window_event_setting.State.PRESS_LOGOUT:
						this.CreateLogoutWindow();
						this.m_State = OptionFacebookAccess.State.LOGOUT;
						break;
					case window_event_setting.State.CLOSE:
						this.CloseFunction();
						break;
					}
				}
				break;
			case OptionFacebookAccess.State.LOGIN:
				if (this.m_easySnsFeed != null)
				{
					EasySnsFeed.Result result = this.m_easySnsFeed.Update();
					if (result != EasySnsFeed.Result.COMPLETED)
					{
						if (result == EasySnsFeed.Result.FAILED)
						{
							this.m_easySnsFeed = null;
							this.CloseFunction();
						}
					}
					else
					{
						this.m_easySnsFeed = null;
						this.CloseFunction();
					}
				}
				break;
			case OptionFacebookAccess.State.LOGOUT:
				if (GeneralWindow.IsCreated("FacebookLogout") && GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
						if (socialInterface != null)
						{
							socialInterface.Logout();
							socialInterface.IsLoggedIn = false;
							PlayerImageManager instance = PlayerImageManager.Instance;
							if (instance != null)
							{
								instance.ClearAllPlayerImage();
							}
							HudMenuUtility.SetUpdateRankingFlag();
						}
						this.m_State = OptionFacebookAccess.State.LOGOUT_COMPLETE_SETTING;
					}
					else
					{
						this.CloseFunction();
					}
					GeneralWindow.Close();
				}
				break;
			case OptionFacebookAccess.State.LOGOUT_COMPLETE_SETTING:
				this.CreateLogoutCompleteWindow();
				this.m_State = OptionFacebookAccess.State.LOGOUT_COMPLETE;
				break;
			case OptionFacebookAccess.State.LOGOUT_COMPLETE:
				if (GeneralWindow.IsCreated("LogoutComplete") && GeneralWindow.IsButtonPressed)
				{
					this.CloseFunction();
					GeneralWindow.Close();
				}
				break;
			}
		}
	}

	private void CreateLogoutWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "FacebookLogout",
			buttonType = GeneralWindow.ButtonType.YesNo,
			caption = TextUtility.GetCommonText("Option", "logout"),
			message = TextUtility.GetCommonText("Option", "logout_message")
		});
	}

	private void CreateLogoutCompleteWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "LogoutComplete",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextUtility.GetCommonText("Option", "logout"),
			message = TextUtility.GetCommonText("Option", "logout_complete")
		});
	}

	private void CloseFunction()
	{
		if (this.m_ui_option_scroll != null)
		{
			this.m_ui_option_scroll.OnEndChildPage();
		}
		base.enabled = false;
		if (this.m_gameObject != null)
		{
			this.m_gameObject.SetActive(false);
		}
		this.m_State = OptionFacebookAccess.State.CLOSE;
	}
}
