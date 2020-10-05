using Message;
using SaveData;
using System;
using Text;
using UnityEngine;

public class FirstLaunchUserSetting : MonoBehaviour
{
	private enum EventSignal
	{
		PLAY_START = 100,
		SNS_LOGIN_END
	}

	private readonly string ANCHOR_PATH = "Camera/menu_Anim/MainMenuUI4/Anchor_5_MC";

	private TinyFsmBehavior m_fsm;

	private SettingPartsSnsLogin m_snsLogin;

	private SettingPartsAcceptInvite m_acceptInvite;

	private EasySnsFeed m_feed;

	private SendApollo m_sendApollo;

	private int m_specialEggNum = -1;

	private int m_chaoRouletteNum = -1;

	private bool m_isEndPlay;

	private float m_timer;

	private bool m_addSpecialEgg;

	private bool m_getSpecialEggNum;

	private bool m_getUserResult;

	public bool IsEndPlay
	{
		get
		{
			return this.m_isEndPlay;
		}
		private set
		{
		}
	}

	public void PlayStart()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
		this.m_isEndPlay = false;
	}

	private void Start()
	{
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
		this.m_snsLogin = base.gameObject.AddComponent<SettingPartsSnsLogin>();
		this.m_snsLogin.Setup(this.ANCHOR_PATH);
		this.m_acceptInvite = base.gameObject.AddComponent<SettingPartsAcceptInvite>();
		this.m_acceptInvite.Setup(this.ANCHOR_PATH);
	}

	private void Update()
	{
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateHelp)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateSnsLogin(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_snsLogin != null)
			{
				this.m_snsLogin.PlayStart();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_snsLogin.IsEnd)
			{
				if (this.m_snsLogin.IsCalceled)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateHelp)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAskInputInviteCode)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAskInputInviteCode(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "StateAskInputInviteCode",
				buttonType = GeneralWindow.ButtonType.YesNo,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_ask_accept_invite_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_ask_accept_invite_text").text,
				anchor_path = this.ANCHOR_PATH
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("StateAskInputInviteCode"))
			{
				if (GeneralWindow.IsYesButtonPressed)
				{
					GeneralWindow.Close();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInputInviteCode)));
				}
				else if (GeneralWindow.IsNoButtonPressed)
				{
					GeneralWindow.Close();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateHelp)));
				}
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateHelp)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateInputInviteCode(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_acceptInvite != null)
			{
				this.m_acceptInvite.PlayStart();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_acceptInvite != null && this.m_acceptInvite.IsEndPlay())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateHelp)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateHelp(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "StateHelp",
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_about_help_menu_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_about_help_menu_text").text,
				anchor_path = this.ANCHOR_PATH,
				buttonType = GeneralWindow.ButtonType.Ok
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("StateHelp"))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					GeneralWindow.Close();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateOptionButton)));
				}
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateOptionButton)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateOptionButton(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.OPTION);
			this.m_timer = 3f;
			return TinyFsmState.End();
		case 4:
			this.m_timer -= Time.deltaTime;
			if (TutorialCursor.IsTouchScreen() || this.m_timer < 0f)
			{
				TutorialCursor.DestroyTutorialCursor();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetUserResult)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGetUserResult(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_getUserResult = false;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerOptionUserResult(base.gameObject);
			}
			else
			{
				this.m_getUserResult = true;
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_getUserResult)
			{
				if (this.m_chaoRouletteNum == 0)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetSpecialEggNum)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEndProcessSpEgg)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void ServerGetOptionUserResult_Succeeded(MsgGetOptionUserResultSucceed msg)
	{
		if (msg != null && msg.m_serverOptionUserResult != null)
		{
			this.m_chaoRouletteNum = msg.m_serverOptionUserResult.m_numChaoRoulette;
		}
		this.m_getUserResult = true;
	}

	private void ServerGetOptionUserResult_Failed(MsgServerConnctFailed msg)
	{
		this.m_getUserResult = true;
	}

	private TinyFsmState StateGetSpecialEggNum(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_getSpecialEggNum = false;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetChaoWheelOptions(base.gameObject);
			}
			else
			{
				this.m_getSpecialEggNum = true;
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_getSpecialEggNum)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEndProcessSpEgg)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void ServerGetChaoWheelOptions_Succeeded(MsgGetChaoWheelOptionsSucceed msg)
	{
		if (msg != null && msg.m_options != null)
		{
			this.m_specialEggNum = msg.m_options.NumSpecialEggs;
		}
		this.m_getSpecialEggNum = true;
	}

	private void ServerGetChaoWheelOptions_Failed(MsgServerConnctFailed msg)
	{
		this.m_getSpecialEggNum = true;
	}

	private bool IsCheater()
	{
		return this.m_specialEggNum != 1 || this.m_chaoRouletteNum != 0;
	}

	private TinyFsmState StateEndProcessSpEgg(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					systemdata.SetFlagStatus(SystemData.FlagStatus.TUTORIAL_END, true);
					instance.SaveSystemData();
				}
			}
			FoxManager.SendLtvPoint(FoxLtvType.CompeleteTutorial);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			return TinyFsmState.End();
		}
		case 1:
			if (this.IsCheater())
			{
				this.m_addSpecialEgg = true;
			}
			else
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					loggedInServerInterface.RequestServerAddSpecialEgg(9, base.gameObject);
					this.m_addSpecialEgg = false;
				}
				else
				{
					this.m_addSpecialEgg = true;
				}
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_addSpecialEgg)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEndProcessApollo)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void ServerAddSpecialEgg_Succeeded(MsgAddSpecialEggSucceed msg)
	{
		this.m_addSpecialEgg = true;
	}

	private void ServerAddSpecialEgg_Failed(MsgServerConnctFailed msg)
	{
		this.m_addSpecialEgg = true;
	}

	private TinyFsmState StateEndProcessApollo(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_sendApollo != null)
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
			}
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialEnd)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateTutorialEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "StateTutorialEnd",
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "end_of_tutorial_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "end_of_tutorial_text").text,
				anchor_path = this.ANCHOR_PATH,
				buttonType = GeneralWindow.ButtonType.TweetCancel
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("StateTutorialEnd"))
			{
				if (GeneralWindow.IsYesButtonPressed)
				{
					string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MileageMap", "feed_highscore_caption").text;
					string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "tutorial_end_feed_text").text;
					this.m_feed = new EasySnsFeed(base.gameObject, this.ANCHOR_PATH, text, text2, null);
					GeneralWindow.Close();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitSnsFeedEnd)));
				}
				else if (GeneralWindow.IsNoButtonPressed)
				{
					SystemSaveManager instance = SystemSaveManager.Instance;
					if (instance != null)
					{
						SystemData systemdata = instance.GetSystemdata();
						if (systemdata != null)
						{
							systemdata.SetFacebookWindow(false);
							instance.SaveSystemData();
						}
					}
					GeneralWindow.Close();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSpEggHelp)));
				}
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSpEggHelp)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitSnsFeedEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (this.m_feed != null)
			{
				EasySnsFeed.Result result = this.m_feed.Update();
				if (result == EasySnsFeed.Result.COMPLETED || result == EasySnsFeed.Result.FAILED)
				{
					global::Debug.Log("FirstLaunchUserSetting.EasySnsFeed.Result=" + result.ToString());
					this.m_feed = null;
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSpEggHelp)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSpEggHelp(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
			if (itemGetWindow != null)
			{
				itemGetWindow.Create(new ItemGetWindow.CInfo
				{
					caption = TextUtility.GetCommonText("MainMenu", "tutorial_sp_egg1_caption"),
					serverItemId = 220000,
					imageCount = TextUtility.GetCommonText("MainMenu", "tutorial_sp_egg1_text", "{COUNT}", 9.ToString())
				});
				SoundManager.SePlay("sys_specialegg", "SE");
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			ItemGetWindow itemGetWindow2 = ItemGetWindowUtil.GetItemGetWindow();
			if (itemGetWindow2 != null && itemGetWindow2.IsEnd)
			{
				HudMenuUtility.SendMsgUpdateSaveDataDisplay();
				itemGetWindow2.Reset();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSpEggHelp2)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSpEggHelp2(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "StateSpEggHelp2",
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "tutorial_sp_egg2_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "tutorial_sp_egg2_text").text,
				anchor_path = this.ANCHOR_PATH,
				buttonType = GeneralWindow.ButtonType.Ok
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("StateSpEggHelp2"))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					GeneralWindow.Close();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
				}
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_isEndPlay = true;
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}
}
