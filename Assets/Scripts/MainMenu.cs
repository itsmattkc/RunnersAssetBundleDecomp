using App;
using App.Utility;
using DataTable;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public enum SequenceState
	{
		Init,
		RequestDailyBattle,
		RequestChaoWheelOption,
		RequestDayCrossWatcher,
		RequestMsgList,
		RequestNoticeInfo,
		Load,
		LoadAtlas,
		StartMessage,
		RankingWait,
		FadeIn,
		Main,
		MainConnect,
		TickerCommunicate,
		MsgBoxCommunicate,
		SchemeCommunicate,
		ResultSchemeCommunicate,
		DayCrossCommunicate,
		LoadMileageXml,
		LoadNextMileageXml,
		LoadMileageTexture,
		MileageReward,
		WaitFadeIfNotEndFade,
		Episode,
		DailyMissionWindow,
		LoginBonusWindow,
		LoginBonusWindowDisplay,
		FirstLoginBonusWindow,
		FirstLoginBonusWindowDisplay,
		PlayButton,
		ChallengeDisplyWindow,
		RecommendReview,
		InformationWindow,
		InformationWindowCreate,
		EventRankingResultWindow,
		RankingResultLeagueWindow,
		DisplayInformaon,
		DailyBattle,
		DailyBattleRewardWindow,
		DailyBattleRewardWindowDisplay,
		CharaSelect,
		ChaoSelect,
		Option,
		Ranking,
		Infomation,
		Roulette,
		Shop,
		PresentBox,
		PlayItem,
		TutorialMenuRoulette,
		TutorialCharaLevelUpMenuStart,
		TutorialCharaLevelUpMenuMoveChara,
		BestRecordCheckEnableFeed,
		BestRecordAskFeed,
		BestRecordFeed,
		QuickModeRankUp,
		QuickModeRankUpDisplay,
		LoadEventResource,
		LoadEventTextureResource,
		EventDisplayProduction,
		UserNameSetting,
		UserNameSettingDisplay,
		AgeVerification,
		AgeVerificationDisplay,
		CheckStage,
		CautionStage,
		Stage,
		VersionChangeWindow,
		EventResourceChangeWindow,
		Title,
		CheckBackTitle,
		FadeOut,
		End,
		NUM
	}

	private enum EventSignal
	{
		SERVER_GET_EVENT_REWARD_END = 100,
		SERVER_GET_EVENT_STATE_END,
		TITLE_BACK
	}

	private enum Flags
	{
		FadeIn,
		FadeOut,
		ForceBackMainMenu,
		GoStage,
		GoSpecialStage,
		GoRaidBoss,
		MileageNextMapLoad,
		MileageProduction,
		LoginRanking,
		EndMileageMapProduction,
		ReceiveMileageState,
		RecieveDailyChallengeInfo,
		MileageReward,
		EventLoadAgain,
		OptionResourceLoaded,
		OptionTutorialStage,
		DailyChallenge,
		LoginBonus,
		FirstLoginBonus,
		MsgBox,
		EndMainConnect,
		REQUEST_CHECK_SCHEME,
		EventWait,
		EventConnetctBeforeLoadMenu,
		TuorialWindow,
		QuickRankingResult,
		QuickRankingUpProduction,
		BestRecordFeed,
		FromMileage,
		NUM
	}

	public enum ProgressBarLeaveState
	{
		IDLE = -1,
		StateInit,
		StateRequestDayCrossWatcher,
		StateRequestDailyBattle,
		StateRequestChaoWheelOption,
		StateRequestMsgList,
		StateRequestNoticeInfo,
		StateLoad,
		StateLoadAtlas,
		StateStartMessage,
		StateRankingWait,
		StateEventRankingWait,
		NUM
	}

	private enum CautionType
	{
		NON,
		CHALLENGE_COUNT,
		NEW_EVENT,
		END_EVENT,
		EVENT_LAST_TIME
	}

	private enum PressedButtonType
	{
		NONE = -1,
		NEXT_STATE,
		GOTO_SHOP,
		BACK,
		CANCEL,
		NUM
	}

	private enum CollisionType
	{
		ALERT_BUTTON_ON,
		ALERT_BUTTON_OFF,
		NON
	}

	private enum ResType
	{
		EVENT_COMMON,
		EVENT_MENU,
		MENU_TOP_TEXTURE,
		NUM
	}

	private enum Communicate
	{
		TICKER,
		TICKER_END,
		MSGBOX,
		MSGBOX_END,
		SCHEME,
		SCHEME_FAILD,
		VERSION,
		DAY_CROSS,
		DAY_CROSS_END,
		LOAD_EVENT_RESOURCE
	}

	private enum CallBack
	{
		DAY_CROSS,
		DAY_CROSS_SERVER_CONNECT,
		DAILY_MISSION_CHALLENGE_END,
		DAILY_MISSION_CHALLENGE_END_SERVER_CONNECT,
		DAILY_MISSION_CHALLENGE_INFO_END,
		DAILY_MISSION_CHALLENGE_INFO_END_SERVER_CONNECT,
		LOGINBONUS_UPDATE_SERVER_CONNECT,
		LOGINBONUS_UPDATE_END
	}

	private const string STAGE_MODE_INFO = "StageInfo";

	public bool m_debugInfo;

	private TinyFsmBehavior m_fsm_behavior;

	private GameObject m_stage_info_obj;

	private MainMenuWindow m_main_menu_window;

	private GameObject m_scene_loader_obj;

	private List<int> m_request_face_list;

	private List<int> m_request_bg_list;

	private ButtonEventResourceLoader m_buttonEventResourceLoader;

	private ServerMileageMapState m_mileage_map_state;

	private ServerMileageMapState m_prev_mileage_map_state;

	private ResultData m_stageResultData;

	private int m_eventResourceId;

	private SendApollo m_sendApollo;

	private ButtonEvent m_buttonEvent;

	private bool m_bossChallenge;

	private Bitset32 m_flags;

	private HudProgressBar m_progressBar;

	private MainMenu.CautionType m_cautionType;

	private MainMenu.PressedButtonType m_pressedButtonType;

	private readonly MainMenu.CollisionType[] COLLISION_TYPE_TABLE;

	private bool m_alertBtnFlag;

	private bool m_eventConnectSkip;

	private EasySnsFeed m_easySnsFeed;

	private DailyBattleRewardWindow m_dailyBattleRewardWindow;

	private DailyWindowUI m_dailyWindowUI;

	private bool m_episodeLoaded;

	private ButtonInfoTable.ButtonType m_ButtonOfNextMenu;

	private RankingResultLeague m_rankingResultLeagueWindow;

	private RankingResultWorldRanking m_eventRankingResult;

	private NetNoticeItem m_currentResultInfo;

	private NetNoticeItem m_eventRankingResultInfo;

	private List<NetNoticeItem> m_rankingResultList;

	private ServerInformationWindow m_serverInformationWindow;

	private bool m_is_end_notice_connect;

	private bool m_connected;

	private FirstLaunchInviteFriend m_fristLaunchInviteFriend;

	private bool m_startLauncherInviteFriendFlag;

	private bool m_eventSpecficText;

	private List<ResourceSceneLoader.ResourceInfo> m_loadInfo;

	private LoginBonusWindowUI m_LoginWindowUI;

	private Bitset32 m_communicateFlag;

	private Bitset32 m_callBackFlag;

	private string m_atomCampain;

	private string m_atomSerial;

	private string m_atomInvalidTextId;

	private static int m_debug;

	private FirstLaunchUserName m_userNameSetting;

	private AgeVerification m_ageVerification;

	private FirstLaunchRecommendReview m_fristLaunchRecommendReview;

	private bool m_startLauncherRecommendReviewFlag;

	private bool m_rankingCallBack;

	private bool m_eventRankingCallBack;

	public bool BossChallenge
	{
		get
		{
			return this.m_bossChallenge;
		}
		set
		{
			this.m_bossChallenge = value;
		}
	}

	public MainMenu()
	{
		MainMenu.CollisionType[] expr_08 = new MainMenu.CollisionType[73];
		expr_08[0] = MainMenu.CollisionType.NON;
		expr_08[1] = MainMenu.CollisionType.NON;
		expr_08[2] = MainMenu.CollisionType.NON;
		expr_08[3] = MainMenu.CollisionType.NON;
		expr_08[4] = MainMenu.CollisionType.NON;
		expr_08[5] = MainMenu.CollisionType.NON;
		expr_08[6] = MainMenu.CollisionType.NON;
		expr_08[7] = MainMenu.CollisionType.NON;
		expr_08[8] = MainMenu.CollisionType.NON;
		expr_08[9] = MainMenu.CollisionType.NON;
		expr_08[11] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[23] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[29] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[37] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[40] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[41] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[42] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[43] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[44] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[45] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[46] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[47] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[48] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[49] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		expr_08[51] = MainMenu.CollisionType.ALERT_BUTTON_OFF;
		this.COLLISION_TYPE_TABLE = expr_08;
		this.m_rankingResultList = new List<NetNoticeItem>();
		this.m_loadInfo = new List<ResourceSceneLoader.ResourceInfo>
		{
			new ResourceSceneLoader.ResourceInfo(ResourceCategory.EVENT_RESOURCE, "EventResourceCommon", true, false, true, "EventResourceCommon", false),
			new ResourceSceneLoader.ResourceInfo(ResourceCategory.EVENT_RESOURCE, "EventResourceMenu", true, false, false, "EventResourceMenu", false),
			new ResourceSceneLoader.ResourceInfo(ResourceCategory.UI, string.Empty, true, false, false, null, false)
		};
		this.m_atomCampain = string.Empty;
		this.m_atomSerial = string.Empty;
		this.m_atomInvalidTextId = string.Empty;
		this.m_request_face_list = new List<int>();
		this.m_request_bg_list = new List<int>();
		this.m_mileage_map_state = new ServerMileageMapState();
		this.m_prev_mileage_map_state = new ServerMileageMapState();
		this.m_eventResourceId = -1;
		this.m_pressedButtonType = MainMenu.PressedButtonType.NONE;
		
	}

	private void Awake()
	{
		Application.targetFrameRate = SystemSettings.TargetFrameRate;
		float fadeDuration = 0.3f;
		float fadeDelay = 0f;
		bool isFadeIn = true;
		CameraFade.StartAlphaFade(Color.black, isFadeIn, fadeDuration, fadeDelay, new Action(this.OnFinishedFadeOutCallback));
	}

	private void Start()
	{
		TimeProfiler.EndCountTime("Title-NextScene");
		HudUtility.SetInvalidNGUIMitiTouch();
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "ConnectAlertMaskUI");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
			}
		}
		ConnectAlertMaskUI.StartScreen();
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SoundManager.BgmVolume = (float)systemdata.bgmVolume / 100f;
				SoundManager.SeVolume = (float)systemdata.seVolume / 100f;
			}
		}
		MenuPlayerSetUtil.ResetMarkCharaPage();
		SoundManager.AddMainMenuCommonCueSheet();
		HudMenuUtility.StartMainMenuBGM();
		GC.Collect();
		this.m_flags.Reset();
		this.m_flags.Set(25, true);
		this.m_flags.Set(26, true);
		this.m_flags.Set(27, true);
		GameObject gameObject3 = GameObject.Find("AllocationStatus");
		if (gameObject3 != null)
		{
			gameObject3.SetActive(false);
			UnityEngine.Object.Destroy(gameObject3);
		}
		this.m_stage_info_obj = GameObject.Find("StageInfo");
		if (this.m_stage_info_obj == null)
		{
			this.m_stage_info_obj = new GameObject();
			if (this.m_stage_info_obj != null)
			{
				this.m_stage_info_obj.name = "StageInfo";
				UnityEngine.Object.DontDestroyOnLoad(this.m_stage_info_obj);
				this.m_stage_info_obj.AddComponent("StageInfo");
			}
		}
		GameObject mainMenuUIObject = HudMenuUtility.GetMainMenuUIObject();
		GameObject mainMenuCmnUIObject = HudMenuUtility.GetMainMenuCmnUIObject();
		if (mainMenuUIObject != null)
		{
			mainMenuUIObject.SetActive(false);
		}
		if (mainMenuCmnUIObject != null)
		{
			mainMenuCmnUIObject.SetActive(false);
		}
		GameObject gameObject4 = GameObject.Find("MainMenuWindow");
		if (gameObject4 != null)
		{
			this.m_main_menu_window = gameObject4.GetComponent<MainMenuWindow>();
		}
		GameObject gameObject5 = GameObject.Find("MainMenuButtonEvent");
		if (gameObject5 != null)
		{
			this.m_buttonEvent = gameObject5.GetComponent<ButtonEvent>();
		}
		if (EventManager.Instance != null && EventManager.Instance.IsStandby())
		{
			this.m_flags.Set(22, true);
		}
		BackKeyManager.AddTutorialEventCallBack(base.gameObject);
		this.m_fsm_behavior = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm_behavior != null)
		{
			TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
			description.initState = new TinyFsmState(new EventFunction(this.StateInit));
			this.m_fsm_behavior.SetUp(description);
		}
		GameObject gameObject6 = GameObject.Find("ConnectAlertMaskUI");
		if (gameObject6 != null)
		{
			this.m_progressBar = GameObjectUtil.FindChildGameObjectComponent<HudProgressBar>(gameObject6, "Pgb_loading");
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetUp(11);
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_fsm_behavior)
		{
			this.m_fsm_behavior.ShutDown();
			this.m_fsm_behavior = null;
		}
	}

	private void OnApplicationPause(bool pause)
	{
		this.m_flags.Set(21, true);
	}

	private void ChangeState(TinyFsmState nextState, MainMenu.SequenceState sequenceState)
	{
		bool flag = this.m_fsm_behavior.ChangeState(nextState);
		if (flag)
		{
			this.SetCollisionState(this.COLLISION_TYPE_TABLE[(int)sequenceState]);
		}
		this.DebugInfoDraw("MainMenu SequenceState = " + sequenceState.ToString());
	}

	private void OnClickPlatformBackButtonTutorialEvent()
	{
		if (this.m_fsm_behavior != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(102);
			this.m_fsm_behavior.Dispatch(signal);
		}
	}

	private void OnMsgReceive(MsgMenuSequence message)
	{
		this.DebugInfoDraw("MainMenu OnMsgReceive " + message.Sequenece.ToString());
		if (this.m_fsm_behavior != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateMessage(message);
			this.m_fsm_behavior.Dispatch(signal);
		}
	}

	private void SetEventStage(bool flag)
	{
		if (EventManager.Instance != null)
		{
			EventManager.Instance.EventStage = flag;
		}
	}

	private void DebugInfoDraw(string msg)
	{
	}

	private void ServerEquipChao_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		NetUtil.SyncSaveDataAndDataBase(msg.m_playerState);
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void ServerEquipChao_Failed(MsgServerConnctFailed msg)
	{
	}

	private void SetCollisionState(MainMenu.CollisionType enterCollisionType)
	{
		if (enterCollisionType != MainMenu.CollisionType.ALERT_BUTTON_ON)
		{
			if (enterCollisionType == MainMenu.CollisionType.ALERT_BUTTON_OFF)
			{
				this.SetConnectAlertButtonCollision(false);
			}
		}
		else
		{
			this.SetConnectAlertButtonCollision(true);
		}
	}

	private void SetConnectAlertButtonCollision(bool on)
	{
		if (on)
		{
			if (!this.m_alertBtnFlag)
			{
				HudMenuUtility.SetConnectAlertMenuButtonUI(true);
				BackKeyManager.MenuSequenceTransitionFlag = true;
				this.m_alertBtnFlag = true;
			}
		}
		else if (this.m_alertBtnFlag)
		{
			HudMenuUtility.SetConnectAlertMenuButtonUI(false);
			BackKeyManager.MenuSequenceTransitionFlag = false;
			this.m_alertBtnFlag = false;
		}
	}

	private MainMenu.CautionType GetCautionType()
	{
		if (StageModeManager.Instance != null && EventManager.Instance != null)
		{
			if (StageModeManager.Instance.IsQuickMode())
			{
				if (EventManager.Instance.IsStandby())
				{
					if (EventManager.Instance.IsInEvent())
					{
						return MainMenu.CautionType.NEW_EVENT;
					}
				}
				else if (EventManager.Instance.Type != EventManager.EventType.UNKNOWN && !EventManager.Instance.IsInEvent())
				{
					return MainMenu.CautionType.END_EVENT;
				}
			}
			else if (EventManager.Instance.IsStandby())
			{
				if ((EventManager.Instance.StandbyType == EventManager.EventType.COLLECT_OBJECT || EventManager.Instance.StandbyType == EventManager.EventType.BGM) && EventManager.Instance.IsInEvent())
				{
					return MainMenu.CautionType.NEW_EVENT;
				}
			}
			else if ((EventManager.Instance.Type == EventManager.EventType.COLLECT_OBJECT || EventManager.Instance.Type == EventManager.EventType.BGM) && !EventManager.Instance.IsInEvent())
			{
				return MainMenu.CautionType.END_EVENT;
			}
		}
		if (SaveDataManager.Instance != null && SaveDataManager.Instance.PlayerData.ChallengeCount == 0u)
		{
			return MainMenu.CautionType.CHALLENGE_COUNT;
		}
		return MainMenu.CautionType.NON;
	}

	private void CreateStageCautionWindow()
	{
		this.m_pressedButtonType = MainMenu.PressedButtonType.NONE;
		switch (this.m_cautionType)
		{
		case MainMenu.CautionType.CHALLENGE_COUNT:
			this.m_main_menu_window.CreateWindow(MainMenuWindow.WindowType.ChallengeGoShop, delegate(bool yesButtonClicked)
			{
				this.m_pressedButtonType = ((!yesButtonClicked) ? MainMenu.PressedButtonType.CANCEL : MainMenu.PressedButtonType.GOTO_SHOP);
			});
			break;
		case MainMenu.CautionType.NEW_EVENT:
			this.m_main_menu_window.CreateWindow(MainMenuWindow.WindowType.EventStart, delegate(bool yesButtonClicked)
			{
				this.m_pressedButtonType = MainMenu.PressedButtonType.BACK;
			});
			break;
		case MainMenu.CautionType.END_EVENT:
			this.m_main_menu_window.CreateWindow(MainMenuWindow.WindowType.EventOutOfTime, delegate(bool yesButtonClicked)
			{
				this.m_pressedButtonType = MainMenu.PressedButtonType.BACK;
			});
			break;
		case MainMenu.CautionType.EVENT_LAST_TIME:
			this.m_main_menu_window.CreateWindow(MainMenuWindow.WindowType.EventLastPlay, delegate(bool yesButtonClicked)
			{
				this.m_pressedButtonType = MainMenu.PressedButtonType.NEXT_STATE;
			});
			break;
		}
	}

	private void CreateTitleBackWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "BackTitle",
			buttonType = GeneralWindow.ButtonType.YesNo,
			caption = TextUtility.GetCommonText("MainMenu", "back_title_caption"),
			message = TextUtility.GetCommonText("MainMenu", "back_title_text")
		});
	}

	private void CheckTutoralWindow()
	{
		if (GeneralWindow.IsCreated("BackTitle") && GeneralWindow.IsButtonPressed)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateTitle)), MainMenu.SequenceState.FadeOut);
			}
			GeneralWindow.Close();
		}
	}

	private TinyFsmState MenuStateLoadEventResource(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.CeateSceneLoader();
			if (EventManager.Instance != null)
			{
				if (EventManager.Instance.Type == EventManager.EventType.QUICK || EventManager.Instance.Type == EventManager.EventType.BGM)
				{
					this.SetLoadEventResource();
					if (this.m_flags.Test(22) && AtlasManager.Instance != null)
					{
						AtlasManager.Instance.StartLoadAtlasForEventMenu();
					}
				}
				else if (EventManager.Instance.Type == EventManager.EventType.UNKNOWN)
				{
					this.SetLoadTopMenuTexture();
				}
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		case 4:
		{
			bool flag = true;
			if (this.m_buttonEventResourceLoader != null)
			{
				flag = this.m_buttonEventResourceLoader.IsLoaded;
			}
			if (flag && this.CheckSceneLoad())
			{
				this.DestroySceneLoader();
				this.SetEventResources();
				if (EventManager.Instance != null)
				{
					EventManager.EventType type = EventManager.Instance.Type;
					if (type != EventManager.EventType.QUICK)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStateLoadEventTextureResource)), MainMenu.SequenceState.LoadEventTextureResource);
					}
					else if (this.m_flags.Test(22))
					{
						this.m_flags.Set(22, false);
						StageModeManager.Instance.DrawQuickStageIndex();
						this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStateLoadEventTextureResource)), MainMenu.SequenceState.LoadEventTextureResource);
					}
					else
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
					}
				}
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		goto IL_27;
	}

	private TinyFsmState MenuStateLoadEventTextureResource(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CeateSceneLoader();
			this.SetLoadTopMenuTexture();
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		case 4:
			if (this.CheckSceneLoad())
			{
				this.DestroySceneLoader();
				this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStateEventDisplayProduction)), MainMenu.SequenceState.EventDisplayProduction);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		goto IL_27;
	}

	private TinyFsmState MenuStateEventDisplayProduction(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			bool isFadeIn = false;
			float fadeDuration = 1f;
			float fadeDelay = 0f;
			this.m_flags.Set(1, false);
			CameraFade.StartAlphaFade(Color.black, isFadeIn, fadeDuration, fadeDelay, new Action(this.OnFinishedFadeOutCallback));
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_29:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_flags.Test(1))
			{
				this.m_flags.Set(1, false);
				if (EventManager.Instance != null)
				{
					EventManager.EventType type = EventManager.Instance.Type;
					if (type != EventManager.EventType.QUICK)
					{
						GameObjectUtil.SendMessageFindGameObject("MainMenuUI4", "OnUpdateQuickModeData", null, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						GameObjectUtil.SendMessageFindGameObject("MainMenuUI4", "OnUpdateQuickModeData", null, SendMessageOptions.DontRequireReceiver);
					}
				}
				float fadeDuration2 = 2f;
				float fadeDelay2 = 0f;
				bool isFadeIn2 = true;
				CameraFade.StartAlphaFade(Color.black, isFadeIn2, fadeDuration2, fadeDelay2, null);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		goto IL_29;
	}

	private void ServerGetEventReward_Succeeded(MsgGetEventRewardSucceed msg)
	{
		if (this.m_fsm_behavior != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
			this.m_fsm_behavior.Dispatch(signal);
		}
	}

	private void ServerGetEventState_Succeeded(MsgGetEventStateSucceed msg)
	{
		if (this.m_fsm_behavior != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(101);
			this.m_fsm_behavior.Dispatch(signal);
		}
	}

	private TinyFsmState StateBestRecordCheckEnableFeed(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
		{
			bool flag = false;
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					flag = systemdata.IsFacebookWindow();
				}
			}
			if (flag)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateBestRecordAskFeed)), MainMenu.SequenceState.BestRecordAskFeed);
			}
			else
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateBestRecordAskFeed(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				buttonType = GeneralWindow.ButtonType.TweetCancel,
				caption = MileageMapUtility.GetText("gw_highscore_caption", null),
				message = MileageMapUtility.GetText("gw_highscore_text", null)
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsButtonPressed)
			{
				if (GeneralWindow.IsYesButtonPressed)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateBestRecordFeed)), MainMenu.SequenceState.BestRecordFeed);
				}
				else
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
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				GeneralWindow.Close();
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateBestRecordFeed(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			long highScore = this.m_stageResultData.m_highScore;
			this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/menu_Anim/MainMenuUI4/Anchor_5_MC", MileageMapUtility.GetText("feed_highscore_caption", null), MileageMapUtility.GetText("feed_highscore_text", new Dictionary<string, string>
			{
				{
					"{HIGHSCORE}",
					highScore.ToString()
				}
			}), null);
			return TinyFsmState.End();
		}
		case 4:
		{
			EasySnsFeed.Result result = this.m_easySnsFeed.Update();
			if (result == EasySnsFeed.Result.COMPLETED || result == EasySnsFeed.Result.FAILED)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateChaoSelect(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.CHAO_ROULETTE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.ROULETTE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateMainCharaSelect(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.CHAO_ROULETTE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.ROULETTE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.CHAO)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateChaoSelect)), MainMenu.SequenceState.ChaoSelect);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDailyBattle(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null && msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDailyBattleRewardWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("DailybattleRewardWindowUI", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			return TinyFsmState.End();
		case 4:
			if (this.m_buttonEventResourceLoader.IsLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateDailyBattleRewardWindowDisplay)), MainMenu.SequenceState.DailyBattleRewardWindowDisplay);
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDailyBattleRewardWindowDisplay(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			this.m_dailyBattleRewardWindow = null;
			return TinyFsmState.End();
		case 1:
			if (SingletonGameObject<DailyBattleManager>.Instance != null)
			{
				this.m_dailyBattleRewardWindow = DailyBattleRewardWindow.Open(SingletonGameObject<DailyBattleManager>.Instance.GetRewardDataPair(false));
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_dailyBattleRewardWindow != null && this.m_dailyBattleRewardWindow.IsEnd)
			{
				DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
				if (instance != null)
				{
					instance.RestReward();
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateInfomation(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.ROULETTE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.CHAO_ROULETTE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.SHOP)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateShop)), MainMenu.SequenceState.Shop);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDailyMissionWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_flags.Set(16, false);
			return TinyFsmState.End();
		case 4:
			if (this.m_dailyWindowUI == null)
			{
				GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
				if (menuAnimUIObject != null)
				{
					this.m_dailyWindowUI = GameObjectUtil.FindChildGameObjectComponent<DailyWindowUI>(menuAnimUIObject, "DailyWindowUI");
					if (this.m_dailyWindowUI != null)
					{
						this.m_dailyWindowUI.gameObject.SetActive(true);
						this.m_dailyWindowUI.PlayStart();
					}
				}
			}
			else if (this.m_dailyWindowUI.IsEnd)
			{
				this.m_dailyWindowUI = null;
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.DAILY_CHALLENGE_BACK, false);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEnd(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.ResetRingCountOffset();
			this.SetEventManagerParam();
			ChaoTextureManager.Instance.RemoveChaoTextureForMainMenuEnd();
			AtlasManager.Instance.ClearAllAtlas();
			this.CeateSceneLoader();
			if (StageModeManager.Instance.StageMode == StageModeManager.Mode.ENDLESS)
			{
				this.LoadMileageText();
				this.m_request_face_list.Clear();
				this.m_request_bg_list.Clear();
				this.EntryMileageTexturesList();
				this.LoadMileageTextures();
			}
			return TinyFsmState.End();
		case 4:
		{
			CPlusPlusLink instance = CPlusPlusLink.Instance;
			if (instance != null)
			{
				instance.BeforeGameCheatCheck();
			}
			if (this.CheckSceneLoad())
			{
				this.SetupMileageText();
				this.TransTextureObj();
				if (this.m_flags.Test(4))
				{
					this.DestroySceneLoader();
					EventUtility.SetDontDestroyLoadingFaceTexture();
					this.EndSpecialStageProcessing();
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)), MainMenu.SequenceState.End);
				}
				else if (this.m_flags.Test(5))
				{
					this.DestroySceneLoader();
					EventUtility.SetDontDestroyLoadingFaceTexture();
					this.EndRaidBossProcessing();
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)), MainMenu.SequenceState.End);
				}
				else if (this.m_flags.Test(3))
				{
					this.EndStageProcessing();
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)), MainMenu.SequenceState.End);
				}
				else
				{
					this.EndTitleProcessing();
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)), MainMenu.SequenceState.End);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateIdle(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void EndStageProcessing()
	{
		if (this.m_flags.Test(15))
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				instance.PlayerData.MainChaoID = -1;
				instance.PlayerData.SubChaoID = -1;
				instance.PlayerData.MainChara = CharaType.SONIC;
			}
			this.SetTutorialStageInfo();
			this.SetTutorialLoadingInfo();
		}
		else
		{
			this.SetStageInfo();
			this.SetLoadingInfo();
			this.DestroyMileageInfo();
		}
		this.PrepareForSceneMove();
		TimeProfiler.StartCountTime("MainMenu-GameModeStage");
		UnityEngine.SceneManagement.SceneManager.LoadScene("s_playingstage");
	}

	private void EndSpecialStageProcessing()
	{
		this.SetSpecialStageInfo();
		this.SetEventLoadingInfo();
		this.PrepareForSceneMove();
		TimeProfiler.StartCountTime("MainMenu-GameModeStage");
		UnityEngine.SceneManagement.SceneManager.LoadScene("s_playingstage");
	}

	private void EndRaidBossProcessing()
	{
		this.SetRaidBossInfo();
		this.SetEventLoadingInfo();
		this.PrepareForSceneMove();
		TimeProfiler.StartCountTime("MainMenu-GameModeStage");
		UnityEngine.SceneManagement.SceneManager.LoadScene("s_playingstage");
	}

	private void EndTitleProcessing()
	{
		if (this.m_stage_info_obj != null)
		{
			UnityEngine.Object.Destroy(this.m_stage_info_obj);
		}
		if (MileageMapDataManager.Instance != null)
		{
			UnityEngine.Object.Destroy(MileageMapDataManager.Instance.gameObject);
		}
		this.PrepareForSceneMove();
		HudMenuUtility.GoToTitleScene();
	}

	private StageInfo.MileageMapInfo CreateMileageInfo()
	{
		StageInfo.MileageMapInfo mileageMapInfo = new StageInfo.MileageMapInfo();
		if (this.m_mileage_map_state != null)
		{
			mileageMapInfo.m_mapState.m_episode = this.m_mileage_map_state.m_episode;
			mileageMapInfo.m_mapState.m_chapter = this.m_mileage_map_state.m_chapter;
			mileageMapInfo.m_mapState.m_point = this.m_mileage_map_state.m_point;
			mileageMapInfo.m_mapState.m_score = this.m_mileage_map_state.m_stageTotalScore;
		}
		else
		{
			mileageMapInfo.m_mapState.m_episode = 1;
			mileageMapInfo.m_mapState.m_chapter = 1;
			mileageMapInfo.m_mapState.m_point = 0;
			mileageMapInfo.m_mapState.m_score = 0L;
		}
		if (mileageMapInfo.m_pointScore != null)
		{
			int num = mileageMapInfo.m_pointScore.Length;
			long num2 = (long)MileageMapUtility.GetPointInterval();
			for (int i = 0; i < num; i++)
			{
				mileageMapInfo.m_pointScore[i] = num2 * (long)i;
			}
		}
		return mileageMapInfo;
	}

	private void SetBoostItemValidFlag(StageInfo stageInfo)
	{
		StageAbilityManager instance = StageAbilityManager.Instance;
		if (instance != null)
		{
			bool[] boostItemValidFlag = instance.BoostItemValidFlag;
			if (boostItemValidFlag != null)
			{
				for (int i = 0; i < 3; i++)
				{
					boostItemValidFlag[i] = stageInfo.BoostItemValid[i];
				}
			}
		}
	}

	private void ResetRingCountOffset()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			instance.ItemData.RingCountOffset = 0;
		}
	}

	private void PrepareForSceneMove()
	{
		if (InformationImageManager.Instance != null)
		{
			InformationImageManager.Instance.ResetImage();
		}
		AtlasManager.Instance.ResetReplaceAtlas();
		MileageMapText.DestroyPreEPisodeText();
		ResourceManager.Instance.RemoveResourcesOnThisScene();
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	private void SetStageInfo()
	{
		if (this.m_stage_info_obj != null)
		{
			StageInfo component = this.m_stage_info_obj.GetComponent<StageInfo>();
			if (component != null)
			{
				component.MileageInfo = this.CreateMileageInfo();
				int point_type = 0;
				int numBossAttack = 0;
				if (this.m_mileage_map_state != null)
				{
					point_type = this.m_mileage_map_state.m_point;
					numBossAttack = this.m_mileage_map_state.m_numBossAttack;
				}
				if (StageModeManager.Instance.IsQuickMode())
				{
					if (EventManager.Instance != null && EventManager.Instance.Type == EventManager.EventType.QUICK)
					{
						int index = 1;
						TenseType tenseType = TenseType.AFTERNOON;
						EventStageData stageData = EventManager.Instance.GetStageData();
						if (stageData != null)
						{
							index = MileageMapUtility.GetStageIndex(stageData.stage_key);
							tenseType = MileageMapUtility.GetTenseType(stageData.stage_key);
						}
						component.SelectedStageName = StageInfo.GetStageNameByIndex(index);
						component.TenseType = tenseType;
						component.NotChangeTense = true;
					}
					else
					{
						switch (StageModeManager.Instance.QuickStageCharaAttribute)
						{
						case CharacterAttribute.SPEED:
							component.SelectedStageName = StageInfo.GetStageNameByIndex(1);
							break;
						case CharacterAttribute.FLY:
							component.SelectedStageName = StageInfo.GetStageNameByIndex(2);
							break;
						case CharacterAttribute.POWER:
							component.SelectedStageName = StageInfo.GetStageNameByIndex(3);
							break;
						default:
							component.SelectedStageName = StageInfo.GetStageNameByIndex(1);
							break;
						}
						component.TenseType = TenseType.AFTERNOON;
						component.NotChangeTense = true;
					}
					component.ExistBoss = false;
					component.BossStage = false;
					component.QuickMode = true;
					component.FromTitle = false;
					component.BossType = BossType.NONE;
					component.NumBossAttack = 0;
				}
				else
				{
					component.SelectedStageName = MileageMapUtility.GetMileageStageName();
					component.TenseType = MileageMapUtility.GetTenseType((PointType)point_type);
					component.NotChangeTense = !MileageMapUtility.GetChangeTense((PointType)point_type);
					component.ExistBoss = MileageMapUtility.IsExistBoss();
					component.BossStage = this.BossChallenge;
					component.QuickMode = false;
					component.FromTitle = false;
					component.BossType = MileageMapUtility.GetBossType();
					component.NumBossAttack = numBossAttack;
				}
				if (this.m_flags.Test(15))
				{
					component.TutorialStage = true;
				}
				if (!component.TutorialStage && EventManager.Instance != null)
				{
					if (EventManager.Instance.Type == EventManager.EventType.COLLECT_OBJECT && EventManager.Instance.IsInEvent())
					{
						this.SetEventStage(true);
					}
					component.EventStage = EventManager.Instance.EventStage;
				}
				this.SetBoostItemValidFlag(component);
			}
		}
	}

	private void SetSpecialStageInfo()
	{
		if (this.m_stage_info_obj != null)
		{
			StageInfo component = this.m_stage_info_obj.GetComponent<StageInfo>();
			if (component != null)
			{
				EventStageData stageData = EventManager.Instance.GetStageData();
				if (stageData != null)
				{
					string stage_key = stageData.stage_key;
					component.SelectedStageName = MileageMapUtility.GetEventStageName(stage_key);
					component.TenseType = MileageMapUtility.GetTenseType(stage_key);
					component.NotChangeTense = !MileageMapUtility.GetChangeTense(stage_key);
					component.ExistBoss = false;
					component.BossStage = false;
					component.TutorialStage = false;
				}
				if (EventManager.Instance != null)
				{
					component.EventStage = EventManager.Instance.EventStage;
				}
				component.MileageInfo = this.CreateMileageInfo();
				this.SetBoostItemValidFlag(component);
				component.FromTitle = false;
			}
		}
	}

	private void SetRaidBossInfo()
	{
		if (this.m_stage_info_obj != null)
		{
			StageInfo component = this.m_stage_info_obj.GetComponent<StageInfo>();
			if (component != null)
			{
				EventStageData stageData = EventManager.Instance.GetStageData();
				if (stageData != null)
				{
					string stage_key = stageData.stage_key;
					component.SelectedStageName = MileageMapUtility.GetEventStageName(stage_key);
					component.TenseType = MileageMapUtility.GetTenseType(stage_key);
					component.NotChangeTense = !MileageMapUtility.GetChangeTense(stage_key);
					component.ExistBoss = true;
					component.BossStage = true;
					component.TutorialStage = false;
					if (RaidBossInfo.currentRaidData != null)
					{
						component.NumBossAttack = (int)(RaidBossInfo.currentRaidData.hpMax - RaidBossInfo.currentRaidData.hp);
						switch (RaidBossInfo.currentRaidData.rarity)
						{
						case 0:
							component.BossType = BossType.EVENT1;
							break;
						case 1:
							component.BossType = BossType.EVENT2;
							break;
						case 2:
							component.BossType = BossType.EVENT3;
							break;
						default:
							component.BossType = BossType.EVENT1;
							break;
						}
					}
					else
					{
						component.NumBossAttack = 0;
						component.BossType = BossType.EVENT1;
					}
				}
				if (EventManager.Instance != null)
				{
					component.EventStage = EventManager.Instance.EventStage;
				}
				component.MileageInfo = this.CreateMileageInfo();
				this.SetBoostItemValidFlag(component);
				component.FromTitle = false;
			}
		}
	}

	private void DestroyMileageInfo()
	{
		if (MileageMapDataManager.Instance != null)
		{
			MileageMapDataManager.Instance.DestroyData();
		}
	}

	private void SetLoadingInfo()
	{
		LoadingInfo loadingInfo = LoadingInfo.CreateLoadingInfo();
		if (loadingInfo != null)
		{
			if (StageModeManager.Instance.IsQuickMode())
			{
				LoadingInfo.LoadingData info = loadingInfo.GetInfo();
				if (info != null)
				{
					UnityEngine.Random.seed = NetUtil.GetCurrentUnixTime();
					int num = UnityEngine.Random.Range(1, 13);
					info.m_titleText = TextUtility.GetCommonText("quick", "loading_title");
					if (num == 1)
					{
						info.m_mainText = TextUtility.GetCommonText("quick", "loading_text");
					}
					else
					{
						info.m_mainText = TextUtility.GetCommonText("quick", "loading_text" + num.ToString());
					}
				}
			}
			else if (MileageMapDataManager.Instance != null)
			{
				int episode = 1;
				int chapter = 1;
				if (this.m_mileage_map_state != null)
				{
					episode = this.m_mileage_map_state.m_episode;
					chapter = this.m_mileage_map_state.m_chapter;
				}
				MileageMapData mileageMapData = MileageMapDataManager.Instance.GetMileageMapData(episode, chapter);
				if (mileageMapData != null)
				{
					LoadingInfo.LoadingData info2 = loadingInfo.GetInfo();
					if (info2 != null)
					{
						int num2 = (!this.IsBossLoading()) ? mileageMapData.loading.window_id : mileageMapData.loading.boss_window_id;
						if (num2 < mileageMapData.window_data.Length)
						{
							info2.m_titleText = MileageMapText.GetText(mileageMapData.scenario.episode, mileageMapData.scenario.title_cell_id);
							info2.m_mainText = MileageMapText.GetText(mileageMapData.scenario.episode, mileageMapData.window_data[num2].body[0].text_cell_id);
							int face_id = mileageMapData.window_data[num2].body[0].product[0].face_id;
							info2.m_texture = MileageMapUtility.GetFaceTexture(face_id);
						}
					}
				}
			}
		}
	}

	private void SetEventLoadingInfo()
	{
		LoadingInfo loadingInfo = LoadingInfo.CreateLoadingInfo();
		if (loadingInfo != null)
		{
			LoadingInfo.LoadingData info = loadingInfo.GetInfo();
			if (info != null && EventManager.Instance != null)
			{
				WindowEventData loadingEventData = EventUtility.GetLoadingEventData();
				if (loadingEventData != null)
				{
					TextManager.TextType type = TextManager.TextType.TEXTTYPE_EVENT_SPECIFIC;
					info.m_titleText = TextUtility.GetText(type, "Production", loadingEventData.title_cell_id);
					info.m_mainText = TextUtility.GetText(type, "Production", loadingEventData.body[0].text_cell_id);
					info.m_texture = EventUtility.GetLoadingFaceTexture();
				}
			}
		}
	}

	private bool IsBossLoading()
	{
		return this.m_mileage_map_state != null && MileageMapUtility.IsExistBoss() && this.BossChallenge;
	}

	private void SetTutorialStageInfo()
	{
		if (this.m_stage_info_obj != null)
		{
			StageInfo component = this.m_stage_info_obj.GetComponent<StageInfo>();
			if (component != null)
			{
				StageInfo.MileageMapInfo mileageMapInfo = new StageInfo.MileageMapInfo();
				mileageMapInfo.m_mapState.m_episode = 1;
				mileageMapInfo.m_mapState.m_chapter = 1;
				mileageMapInfo.m_mapState.m_point = 0;
				mileageMapInfo.m_mapState.m_score = 0L;
				component.SelectedStageName = StageInfo.GetStageNameByIndex(1);
				component.TenseType = TenseType.AFTERNOON;
				component.ExistBoss = false;
				component.BossStage = false;
				component.TutorialStage = true;
				StageAbilityManager instance = StageAbilityManager.Instance;
				if (instance != null)
				{
					bool[] boostItemValidFlag = instance.BoostItemValidFlag;
					if (boostItemValidFlag != null)
					{
						for (int i = 0; i < 3; i++)
						{
							boostItemValidFlag[i] = false;
						}
					}
				}
				component.FromTitle = false;
				component.MileageInfo = mileageMapInfo;
			}
		}
	}

	private void SetTutorialLoadingInfo()
	{
		LoadingInfo loadingInfo = LoadingInfo.CreateLoadingInfo();
		if (loadingInfo != null)
		{
			LoadingInfo.LoadingData info = loadingInfo.GetInfo();
			if (info != null)
			{
				string cellID = CharaName.Name[0];
				string commonText = TextUtility.GetCommonText("CharaName", cellID);
				info.m_titleText = TextUtility.GetCommonText("Option", "chara_operation_method", "{CHARA_NAME}", commonText);
				info.m_mainText = TextUtility.GetCommonText("Option", "sonic_operation_comment");
				info.m_optionTutorial = true;
				int face_id = 1;
				info.m_texture = MileageMapUtility.GetFaceTexture(face_id);
			}
		}
	}

	private void SetEventManagerParam()
	{
		if (EventManager.Instance != null)
		{
			if (this.m_flags.Test(4) || this.m_flags.Test(5))
			{
				EventManager.Instance.EventStage = true;
				EventManager.Instance.ReCalcEndPlayTime();
			}
			else
			{
				EventManager.Instance.EventStage = false;
			}
		}
	}

	private TinyFsmState StateLoadMileageXml(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (!this.m_episodeLoaded)
			{
				this.CeateSceneLoader();
				if (!this.IsExistMileageMapData(this.m_mileage_map_state))
				{
					this.AddSceneLoader(this.GetMileageMapDataScenaName(this.m_mileage_map_state));
				}
				if (GameObject.Find("MileageDataTable") == null)
				{
					this.AddSceneLoader("MileageDataTable");
				}
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_episodeLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMileageReward)), MainMenu.SequenceState.MileageReward);
			}
			else if (this.CheckSceneLoad())
			{
				this.m_episodeLoaded = true;
				this.DestroySceneLoader();
				this.SetupMileageDataTable();
				if (this.m_flags.Test(6))
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadNextMileageXml)), MainMenu.SequenceState.LoadNextMileageXml);
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadMileageTexture)), MainMenu.SequenceState.LoadMileageTexture);
				}
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadNextMileageXml(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CeateSceneLoader();
			if (!this.IsExistMileageMapData(this.m_prev_mileage_map_state))
			{
				this.AddSceneLoader(this.GetMileageMapDataScenaName(this.m_prev_mileage_map_state));
			}
			return TinyFsmState.End();
		case 4:
			if (this.CheckSceneLoad())
			{
				this.DestroySceneLoader();
				MileageMapDataManager instance = MileageMapDataManager.Instance;
				if (instance != null)
				{
					instance.SetCurrentData(this.m_prev_mileage_map_state.m_episode, this.m_prev_mileage_map_state.m_chapter);
				}
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadMileageTexture)), MainMenu.SequenceState.LoadMileageTexture);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadMileageTexture(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			MileageMapDataManager instance = MileageMapDataManager.Instance;
			if (instance != null)
			{
				this.m_request_face_list.Clear();
				this.m_request_bg_list.Clear();
				this.EntryMileageTexturesList();
				this.CeateSceneLoader();
				this.LoadMileageText();
				this.LoadMileageTextures();
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.CheckSceneLoad())
			{
				this.SetIncentive();
				this.SetupMileageText();
				this.TransTextureObj();
				this.DestroySceneLoader();
				Resources.UnloadUnusedAssets();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMileageReward)), MainMenu.SequenceState.MileageReward);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateMileageReward(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (ServerInterface.LoggedInServerInterface != null)
			{
				ServerMileageMapState mileageMapState = ServerInterface.MileageMapState;
				List<ServerMileageReward> mileageRewardList = ServerInterface.MileageRewardList;
				foreach (ServerMileageReward current in mileageRewardList)
				{
					if (current.m_episode == mileageMapState.m_episode && current.m_episode == mileageMapState.m_chapter)
					{
						this.ServerGetMileageReward_Succeeded(null);
						break;
					}
				}
				if (!this.m_flags.Test(12))
				{
					ServerInterface.LoggedInServerInterface.RequestServerGetMileageReward(mileageMapState.m_episode, mileageMapState.m_chapter, base.gameObject);
				}
			}
			else
			{
				this.ServerGetMileageReward_Succeeded(null);
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_flags.Test(12))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitFadeIfNotEndFade)), MainMenu.SequenceState.WaitFadeIfNotEndFade);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitFadeIfNotEndFade(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (!this.m_flags.Test(0))
			{
				ConnectAlertMaskUI.EndScreen(new Action(this.OnFinishedFadeInCallback));
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_flags.Test(0))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateEpisode)), MainMenu.SequenceState.Episode);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEpisode(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_flags.Set(28, true);
			if (this.m_flags.Test(7))
			{
				this.m_flags.Set(7, false);
				HudMenuUtility.SendMsgPrepareMileageMapProduction(this.m_stageResultData);
			}
			else
			{
				HudMenuUtility.SendMsgUpdateMileageMapDisplayToMileage();
			}
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.PLAY_AT_EPISODE_PAGE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStatePlayButton)), MainMenu.SequenceState.PlayButton);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.SHOP)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateShop)), MainMenu.SequenceState.Shop);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private void TransTextureObj()
	{
		if (MileageMapDataManager.Instance != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(MileageMapDataManager.Instance.gameObject, "MileageMapBG");
			if (gameObject != null && MileageMapBGDataTable.Instance != null)
			{
				foreach (int current in this.m_request_bg_list)
				{
					GameObject gameObject2 = GameObject.Find(MileageMapBGDataTable.Instance.GetTextureName(current));
					if (gameObject2 != null)
					{
						gameObject2.transform.parent = gameObject.transform;
					}
				}
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(MileageMapDataManager.Instance.gameObject, "MileageMapFace");
			if (gameObject3 != null)
			{
				foreach (int current2 in this.m_request_face_list)
				{
					GameObject gameObject4 = GameObject.Find(MileageMapUtility.GetFaceTextureName(current2));
					if (gameObject4 != null)
					{
						gameObject4.transform.parent = gameObject3.transform;
					}
				}
			}
		}
	}

	private void EntryMileageTexturesList()
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		MileageMapData mileageMapData = null;
		MileageMapData mileageMapData2 = null;
		if (instance != null)
		{
			mileageMapData2 = instance.GetMileageMapData(this.m_mileage_map_state.m_episode, this.m_mileage_map_state.m_chapter);
			if (this.m_flags.Test(6))
			{
				mileageMapData = instance.GetMileageMapData(this.m_prev_mileage_map_state.m_episode, this.m_prev_mileage_map_state.m_chapter);
			}
		}
		if (mileageMapData2 != null)
		{
			bool keep = true;
			bool keep2 = false;
			int bg_id = mileageMapData2.map_data.bg_id;
			this.AddIDList(ref this.m_request_bg_list, bg_id, "bg", keep);
			if (mileageMapData != null)
			{
				bg_id = mileageMapData.map_data.bg_id;
				this.AddIDList(ref this.m_request_bg_list, bg_id, "bg", keep2);
			}
			List<int> list = new List<int>();
			if (this.m_mileage_map_state.m_point == 5 && mileageMapData2.event_data.IsBossEvent())
			{
				int boss_window_id = mileageMapData2.loading.boss_window_id;
				this.SetLoadingFaceTexture(ref this.m_request_face_list, ref list, mileageMapData2, boss_window_id);
			}
			int window_id = mileageMapData2.loading.window_id;
			this.SetLoadingFaceTexture(ref this.m_request_face_list, ref list, mileageMapData2, window_id);
			int num = 1;
			if (!this.m_request_face_list.Contains(num))
			{
				this.AddIDList(ref this.m_request_face_list, num, "face", keep);
				list.Add(num);
			}
			instance.SetLoadingFaceId(list);
			if (this.m_flags.Test(6))
			{
				int num2 = mileageMapData.event_data.point.Length;
				for (int i = 0; i < num2; i++)
				{
					if (this.m_prev_mileage_map_state.m_point <= i)
					{
						if (i != 5 || !mileageMapData.event_data.IsBossEvent())
						{
							int balloon_face_id = mileageMapData.event_data.point[i].balloon_face_id;
							int balloon_on_arrival_face_id = mileageMapData.event_data.point[i].balloon_on_arrival_face_id;
							this.AddIDList(ref this.m_request_face_list, balloon_face_id, "face", keep2);
							this.AddIDList(ref this.m_request_face_list, balloon_on_arrival_face_id, "face", keep2);
						}
						else
						{
							BossEvent bossEvent = mileageMapData.event_data.GetBossEvent();
							this.AddIDList(ref this.m_request_face_list, bossEvent.balloon_on_arrival_face_id, "face", keep2);
							this.AddIDList(ref this.m_request_face_list, bossEvent.balloon_clear_face_id, "face", keep2);
						}
					}
				}
				num2 = mileageMapData2.event_data.point.Length;
				for (int j = 0; j < num2; j++)
				{
					if (this.m_mileage_map_state.m_point <= j)
					{
						if (j != 5 || !mileageMapData2.event_data.IsBossEvent())
						{
							int balloon_face_id2 = mileageMapData2.event_data.point[j].balloon_face_id;
							this.AddIDList(ref this.m_request_face_list, balloon_face_id2, "face", keep);
						}
						else
						{
							BossEvent bossEvent2 = mileageMapData2.event_data.GetBossEvent();
							this.AddIDList(ref this.m_request_face_list, bossEvent2.balloon_init_face_id, "face", keep);
						}
					}
				}
				this.SetLoadWindowFaceTexture(ref this.m_request_face_list, mileageMapData, this.m_prev_mileage_map_state);
				this.SetLoadWindowFaceTexture(ref this.m_request_face_list, mileageMapData2, this.m_mileage_map_state);
			}
			else
			{
				int num3 = mileageMapData2.event_data.point.Length;
				for (int k = 0; k < num3; k++)
				{
					if (this.m_prev_mileage_map_state.m_point <= k)
					{
						if (k != 5 || !mileageMapData2.event_data.IsBossEvent())
						{
							int balloon_face_id3 = mileageMapData2.event_data.point[k].balloon_face_id;
							int balloon_on_arrival_face_id2 = mileageMapData2.event_data.point[k].balloon_on_arrival_face_id;
							this.AddIDList(ref this.m_request_face_list, balloon_face_id3, "face", keep);
							this.AddIDList(ref this.m_request_face_list, balloon_on_arrival_face_id2, "face", keep2);
						}
						else
						{
							BossEvent bossEvent3 = mileageMapData2.event_data.GetBossEvent();
							this.AddIDList(ref this.m_request_face_list, bossEvent3.balloon_init_face_id, "face", keep);
							this.AddIDList(ref this.m_request_face_list, bossEvent3.balloon_on_arrival_face_id, "face", keep);
							this.AddIDList(ref this.m_request_face_list, bossEvent3.balloon_clear_face_id, "face", keep2);
						}
					}
				}
				if (this.m_mileage_map_state.m_episode == 1 || this.m_flags.Test(7))
				{
					this.SetLoadWindowFaceTexture(ref this.m_request_face_list, mileageMapData2, null);
				}
			}
		}
	}

	private void LoadEventProductTexture()
	{
	}

	private void LoadMileageTextures()
	{
		if (MileageMapDataManager.Instance != null)
		{
			foreach (int current in this.m_request_bg_list)
			{
				string textureName = MileageMapBGDataTable.Instance.GetTextureName(current);
				if (GameObjectUtil.FindChildGameObject(MileageMapDataManager.Instance.gameObject, textureName) == null)
				{
					this.AddSceneLoader(textureName);
				}
			}
			foreach (int current2 in this.m_request_face_list)
			{
				string faceTextureName = MileageMapUtility.GetFaceTextureName(current2);
				if (GameObjectUtil.FindChildGameObject(MileageMapDataManager.Instance.gameObject, faceTextureName) == null)
				{
					this.AddSceneLoader(faceTextureName);
				}
			}
		}
	}

	private void LoadMileageText()
	{
		int episode = 1;
		int pre_episode = -1;
		this.GetMileageTextParam(ref episode, ref pre_episode);
		if (this.m_scene_loader_obj != null)
		{
			MileageMapText.Load(this.m_scene_loader_obj.GetComponent<ResourceSceneLoader>(), episode, pre_episode);
		}
	}

	private void GetMileageTextParam(ref int episode, ref int pre_episode)
	{
		if (this.m_mileage_map_state != null)
		{
			episode = this.m_mileage_map_state.m_episode;
		}
		if (this.m_stageResultData != null && this.m_stageResultData.m_oldMapState != null)
		{
			pre_episode = this.m_stageResultData.m_oldMapState.m_episode;
		}
	}

	public void SetupMileageText()
	{
		MileageMapText.Setup();
	}

	public void SetIncentive()
	{
		if (this.m_stageResultData != null && this.m_stageResultData.m_mileageIncentiveList != null)
		{
			MileageMapDataManager instance = MileageMapDataManager.Instance;
			if (instance != null)
			{
				int episode = this.m_mileage_map_state.m_episode;
				int chapter = this.m_mileage_map_state.m_chapter;
				int num = 0;
				int num2 = 0;
				foreach (ServerMileageIncentive current in this.m_stageResultData.m_mileageIncentiveList)
				{
					RewardData src_reward = new RewardData(current.m_itemId, current.m_num);
					if (current.m_type == ServerMileageIncentive.Type.POINT)
					{
						if (this.m_stageResultData != null && this.m_stageResultData.m_oldMapState != null && current.m_pointId > this.m_stageResultData.m_oldMapState.m_point)
						{
							episode = this.m_stageResultData.m_oldMapState.m_episode;
							chapter = this.m_stageResultData.m_oldMapState.m_chapter;
						}
						instance.SetPointIncentiveData(episode, chapter, current.m_pointId, src_reward);
					}
					else if (current.m_type == ServerMileageIncentive.Type.CHAPTER)
					{
						if (this.m_stageResultData != null && this.m_stageResultData.m_oldMapState != null)
						{
							episode = this.m_stageResultData.m_oldMapState.m_episode;
							chapter = this.m_stageResultData.m_oldMapState.m_chapter;
						}
						instance.SetChapterIncentiveData(episode, chapter, num, src_reward);
						num++;
					}
					else if (current.m_type == ServerMileageIncentive.Type.EPISODE)
					{
						if (this.m_stageResultData != null && this.m_stageResultData.m_oldMapState != null)
						{
							episode = this.m_stageResultData.m_oldMapState.m_episode;
							chapter = this.m_stageResultData.m_oldMapState.m_chapter;
						}
						instance.SetEpisodeIncentiveData(episode, chapter, num2, src_reward);
						num2++;
					}
				}
			}
		}
	}

	private void SetupMileageDataTable()
	{
		if (GameObject.Find("MileageDataTable") == null)
		{
			GameObject gameObject = new GameObject("MileageDataTable");
			GameObject gameObject2 = GameObject.Find("BGDataTable");
			if (gameObject2 != null)
			{
				gameObject2.transform.parent = gameObject.gameObject.transform;
			}
			GameObject gameObject3 = GameObject.Find("PointDataTable");
			if (gameObject3 != null)
			{
				gameObject3.transform.parent = gameObject.gameObject.transform;
			}
			GameObject gameObject4 = GameObject.Find("RouteDataTable");
			if (gameObject4 != null)
			{
				gameObject4.transform.parent = gameObject.gameObject.transform;
			}
			GameObject gameObject5 = GameObject.Find("StageSuggestedDataTable");
			if (gameObject5 != null)
			{
				gameObject5.transform.parent = gameObject.gameObject.transform;
			}
			GameObject gameObject6 = GameObject.Find("MileageMapDataManager");
			if (gameObject6 != null)
			{
				gameObject.transform.parent = gameObject6.transform;
			}
		}
	}

	private bool IsExistMileageMapData(ServerMileageMapState state)
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		return instance != null && instance.IsExist(state.m_episode, state.m_chapter);
	}

	private TinyFsmState StateFadeIn(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.StartScene();
			return TinyFsmState.End();
		case 1:
		{
			TimeProfiler.StartCountTime("StateFadeIn");
			if (this.m_flags.Test(7))
			{
				MileageMapUtility.SetDisplayOffset_FromResultData(this.m_stageResultData);
			}
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			HudMenuUtility.SendStartMainMenuDlsplay();
			HudMenuUtility.SendMsgTickerUpdate();
			this.m_ButtonOfNextMenu = ButtonInfoTable.ButtonType.UNKNOWN;
			bool stageResultData = this.m_stageResultData != null;
			if (stageResultData && !this.m_stageResultData.m_quickMode)
			{
				if (this.m_stageResultData.m_fromOptionTutorial)
				{
					this.m_ButtonOfNextMenu = ButtonInfoTable.ButtonType.OPTION;
				}
				else
				{
					this.m_ButtonOfNextMenu = ButtonInfoTable.ButtonType.EPISODE;
				}
			}
			if (this.m_ButtonOfNextMenu == ButtonInfoTable.ButtonType.UNKNOWN)
			{
				ConnectAlertMaskUI.EndScreen(new Action(this.OnFinishedFadeInCallback));
			}
			else
			{
				this.OnFinishedFadeInCallback();
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_flags.Test(0))
			{
				TimeProfiler.EndCountTime("StateFadeIn");
				if (this.m_ButtonOfNextMenu == ButtonInfoTable.ButtonType.UNKNOWN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else
				{
					HudMenuUtility.SendMenuButtonClicked(this.m_ButtonOfNextMenu, false);
					this.m_flags.Set(0, false);
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenu)), MainMenu.SequenceState.Main);
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void OnFinishedFadeInCallback()
	{
		this.m_flags.Set(0, true);
	}

	private TinyFsmState StateFadeOut(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			BackKeyManager.EndScene();
			bool isFadeIn = false;
			float fadeDuration = 1f;
			float fadeDelay = 0f;
			this.m_flags.Set(1, false);
			CameraFade.StartAlphaFade(Color.black, isFadeIn, fadeDuration, fadeDelay, new Action(this.OnFinishedFadeOutCallback));
			SoundManager.BgmFadeOut(0.5f);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_flags.Test(1))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)), MainMenu.SequenceState.End);
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void OnFinishedFadeOutCallback()
	{
		this.m_flags.Set(1, true);
	}

	private TinyFsmState StateInformationWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync(ButtonInfoTable.PageType.INFOMATION, new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			return TinyFsmState.End();
		case 4:
			if (this.m_buttonEventResourceLoader.IsLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateInformationWindowCreate)), MainMenu.SequenceState.InformationWindowCreate);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateInformationWindowCreate(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.SetRankingResult();
			this.m_eventRankingResultInfo = this.GetEventRankingResultInformation();
			this.CreateInformationWindow();
			return TinyFsmState.End();
		case 4:
			if (this.m_eventRankingResultInfo != null)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateEventRankingResultWindow)), MainMenu.SequenceState.EventRankingResultWindow);
			}
			else if (this.m_rankingResultList.Count > 0)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRankingResultLeagueWindow)), MainMenu.SequenceState.RankingResultLeagueWindow);
			}
			else if (this.m_serverInformationWindow != null)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateDisplayInformaon)), MainMenu.SequenceState.DisplayInformaon);
			}
			else
			{
				this.ChangeNextStateForInformation();
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEventRankingResultWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_eventRankingResult != null)
			{
				this.m_eventRankingResult = null;
			}
			return TinyFsmState.End();
		case 1:
			this.m_eventRankingResult = this.CreateWorldRankingWindow(this.m_eventRankingResultInfo);
			return TinyFsmState.End();
		case 4:
			if (this.m_eventRankingResult != null && this.m_eventRankingResult.IsEnd)
			{
				this.ServerNoticeInfoUpdateChecked(this.m_eventRankingResultInfo);
				this.ServerNoticeInfoSave();
				if (this.m_rankingResultList.Count > 0)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRankingResultLeagueWindow)), MainMenu.SequenceState.RankingResultLeagueWindow);
				}
				else if (this.m_serverInformationWindow != null)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateDisplayInformaon)), MainMenu.SequenceState.DisplayInformaon);
				}
				else
				{
					this.ChangeNextStateForInformation();
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRankingResultLeagueWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_rankingResultLeagueWindow != null)
			{
				this.m_rankingResultLeagueWindow = null;
			}
			return TinyFsmState.End();
		case 1:
			this.m_rankingResultLeagueWindow = null;
			using (List<NetNoticeItem>.Enumerator enumerator = this.m_rankingResultList.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					NetNoticeItem current = enumerator.Current;
					this.m_rankingResultLeagueWindow = RankingResultLeague.Create(current);
					this.m_currentResultInfo = current;
				}
			}
			return TinyFsmState.End();
		case 4:
		{
			bool flag = this.m_rankingResultLeagueWindow == null;
			if (this.m_rankingResultLeagueWindow != null && this.m_rankingResultLeagueWindow.IsEnd())
			{
				this.ServerNoticeInfoUpdateChecked(this.m_currentResultInfo);
				this.m_rankingResultList.Remove(this.m_currentResultInfo);
				this.m_rankingResultLeagueWindow = null;
				this.m_currentResultInfo = null;
				using (List<NetNoticeItem>.Enumerator enumerator2 = this.m_rankingResultList.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						NetNoticeItem current2 = enumerator2.Current;
						this.m_rankingResultLeagueWindow = RankingResultLeague.Create(current2);
						this.m_currentResultInfo = current2;
					}
				}
			}
			if (flag)
			{
				if (this.m_serverInformationWindow != null)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateDisplayInformaon)), MainMenu.SequenceState.DisplayInformaon);
				}
				else
				{
					this.ChangeNextStateForInformation();
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDisplayInformaon(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_serverInformationWindow != null)
			{
				this.m_serverInformationWindow = null;
			}
			return TinyFsmState.End();
		case 1:
			if (this.m_serverInformationWindow != null)
			{
				this.m_serverInformationWindow.SetSaveFlag();
				this.m_serverInformationWindow.PlayStart();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_serverInformationWindow != null && this.m_serverInformationWindow.IsEnd())
			{
				this.ChangeNextStateForInformation();
				if (InformationImageManager.Instance != null)
				{
					InformationImageManager.Instance.ClearWinowImage();
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void CreateInformationWindow()
	{
		if (this.m_serverInformationWindow == null)
		{
			GameObject gameObject = new GameObject();
			if (gameObject != null)
			{
				gameObject.name = "ServerInformationWindow";
				gameObject.AddComponent<ServerInformationWindow>();
				this.m_serverInformationWindow = gameObject.GetComponent<ServerInformationWindow>();
			}
		}
	}

	private RankingResultWorldRanking CreateWorldRankingWindow(NetNoticeItem item)
	{
		if (item != null)
		{
			RankingResultWorldRanking resultWorldRanking = RankingResultWorldRanking.GetResultWorldRanking();
			if (resultWorldRanking != null)
			{
				resultWorldRanking.Setup(item);
				resultWorldRanking.PlayStart();
				return resultWorldRanking;
			}
		}
		return null;
	}

	private void SetRankingResult()
	{
		this.m_eventRankingResultInfo = this.GetInformation(NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID);
		NetNoticeItem information = this.GetInformation(NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID);
		NetNoticeItem information2 = this.GetInformation(NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID);
		if (information != null && information2 != null)
		{
			if (information.Priority < information2.Priority)
			{
				this.m_rankingResultList.Add(information);
				this.m_rankingResultList.Add(information2);
			}
			else
			{
				this.m_rankingResultList.Add(information2);
				this.m_rankingResultList.Add(information);
			}
		}
		else
		{
			if (information != null)
			{
				this.m_rankingResultList.Add(information);
			}
			if (information2 != null)
			{
				this.m_rankingResultList.Add(information2);
			}
		}
	}

	private NetNoticeItem GetRankingResultInformation()
	{
		return this.GetInformation(NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID);
	}

	private NetNoticeItem GetEventRankingResultInformation()
	{
		return this.GetInformation(NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID);
	}

	private NetNoticeItem GetInformation(int id)
	{
		if (ServerInterface.NoticeInfo != null)
		{
			List<NetNoticeItem> noticeItems = ServerInterface.NoticeInfo.m_noticeItems;
			if (noticeItems != null)
			{
				foreach (NetNoticeItem current in noticeItems)
				{
					if (current != null && current.Id == (long)id && !ServerInterface.NoticeInfo.IsChecked(current))
					{
						return current;
					}
				}
			}
		}
		return null;
	}

	private void ServerNoticeInfoUpdateChecked(NetNoticeItem item)
	{
		if (ServerInterface.NoticeInfo != null)
		{
			ServerInterface.NoticeInfo.UpdateChecked(item);
		}
	}

	private void ServerNoticeInfoSave()
	{
		if (ServerInterface.NoticeInfo != null)
		{
			ServerInterface.NoticeInfo.m_isShowedNoticeInfo = true;
			ServerInterface.NoticeInfo.SaveInformation();
		}
	}

	private void ChangeNextStateForInformation()
	{
		HudMenuUtility.SendMsgInformationDisplay();
		this.ServerNoticeInfoSave();
		HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.INFOMATION, false);
		this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenu)), MainMenu.SequenceState.Main);
	}

	private void ResourceLoadEndCallback()
	{
		GameObjectUtil.SendMessageFindGameObject("MainMenuButtonEvent", "OnPageResourceLoadedCallback", null, SendMessageOptions.DontRequireReceiver);
	}

	private TinyFsmState StateInit(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			TimeProfiler.StartCountTime("StateInit");
			if (Env.useAssetBundle)
			{
				if (AssetBundleLoader.Instance == null)
				{
					AssetBundleLoader.Create();
				}
				if (!AssetBundleLoader.Instance.IsEnableDownlad())
				{
					AssetBundleLoader.Instance.Initialize();
				}
			}
			StageAbilityManager instance = StageAbilityManager.Instance;
			if (instance != null)
			{
				for (int i = 0; i < 3; i++)
				{
					instance.BoostItemValidFlag[i] = false;
				}
			}
			NativeObserver.Instance.CheckCurrentTransaction();
			return TinyFsmState.End();
		}
		case 4:
			if (!Env.useAssetBundle || AssetBundleLoader.Instance.IsEnableDownlad())
			{
				TimeProfiler.EndCountTime("StateInit");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRequestDayCrossWatcher)), MainMenu.SequenceState.RequestDayCrossWatcher);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRequestDayCrossWatcher(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			this.m_callBackFlag.Reset();
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(1);
			}
			return TinyFsmState.End();
		case 1:
		{
			TimeProfiler.StartCountTime("StateRequestDayCrossWatcher");
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_callBackFlag.Reset();
				if (ServerDayCrossWatcher.Instance != null)
				{
					ServerDayCrossWatcher.Instance.UpdateClientInfosByDayCross(new ServerDayCrossWatcher.UpdateInfoCallback(this.DataCrossCallBack));
				}
			}
			else
			{
				this.m_callBackFlag.Set(0, true);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_callBackFlag.Test(0))
			{
				TimeProfiler.EndCountTime("StateRequestDayCrossWatcher");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRequestDailyBattle)), MainMenu.SequenceState.RequestDailyBattle);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRequestDailyBattle(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(2);
			}
			return TinyFsmState.End();
		case 1:
			TimeProfiler.StartCountTime("StateRequestDailyBattle");
			if (this.IsRequestDailyBattle())
			{
				if (SingletonGameObject<DailyBattleManager>.Instance != null)
				{
					SingletonGameObject<DailyBattleManager>.Instance.FirstSetup(new DailyBattleManager.CallbackSetup(this.DailyBattleManagerCallBack));
				}
				else
				{
					this.m_connected = true;
				}
			}
			else
			{
				this.m_connected = true;
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_connected)
			{
				TimeProfiler.EndCountTime("StateRequestDailyBattle");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRequestChaoWheelOption)), MainMenu.SequenceState.RequestChaoWheelOption);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRequestChaoWheelOption(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(3);
			}
			return TinyFsmState.End();
		case 1:
			TimeProfiler.StartCountTime("StateRequestChaoWheelOption");
			if (this.IsRequestChoaWheelOption())
			{
				if (RouletteManager.Instance != null)
				{
					RouletteManager.CallbackRouletteInit callback = new RouletteManager.CallbackRouletteInit(this.CallbackRouletteInit);
					RouletteManager.Instance.InitRouletteRequest(callback);
				}
				else
				{
					this.m_connected = true;
				}
			}
			else
			{
				this.m_connected = true;
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_connected)
			{
				this.SetStageResultData();
				this.CheckEventEnd();
				TimeProfiler.EndCountTime("StateRequestChaoWheelOption");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRequestMsgList)), MainMenu.SequenceState.RequestMsgList);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRequestMsgList(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(4);
			}
			return TinyFsmState.End();
		case 1:
		{
			TimeProfiler.StartCountTime("StateRequestMsgList");
			this.m_flags.Set(19, false);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetMessageList(base.gameObject);
				this.m_flags.Set(19, true);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (!this.m_flags.Test(19))
			{
				TimeProfiler.EndCountTime("StateRequestMsgList");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRequestNoticeInfo)), MainMenu.SequenceState.RequestNoticeInfo);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRequestNoticeInfo(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(5);
			}
			return TinyFsmState.End();
		case 1:
		{
			TimeProfiler.StartCountTime("StateRequestNoticeInfo");
			this.m_is_end_notice_connect = false;
			bool flag = true;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				ServerNoticeInfo noticeInfo = ServerInterface.NoticeInfo;
				if (noticeInfo != null && noticeInfo.IsNeedUpdateInfo())
				{
					loggedInServerInterface.RequestServerGetInformation(base.gameObject);
					flag = false;
				}
			}
			if (flag)
			{
				this.ServerGetInformation_Succeeded(null);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_is_end_notice_connect)
			{
				RouletteInformationManager instance = RouletteInformationManager.Instance;
				if (instance != null)
				{
					instance.SetUp();
				}
				TimeProfiler.EndCountTime("StateRequestNoticeInfo");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoad)), MainMenu.SequenceState.Load);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void SetStageResultData()
	{
		this.m_flags.Set(6, false);
		this.m_flags.Set(7, false);
		this.m_flags.Set(8, false);
		ServerMileageMapState mileageMapState = ServerInterface.MileageMapState;
		if (mileageMapState != null)
		{
			mileageMapState.CopyTo(this.m_mileage_map_state);
			mileageMapState.CopyTo(this.m_prev_mileage_map_state);
		}
		GameObject gameObject = GameObject.Find("ResultInfo");
		bool flag = gameObject != null;
		bool flag2 = !flag;
		if (flag)
		{
			ResultInfo component = gameObject.GetComponent<ResultInfo>();
			if (component != null)
			{
				this.m_stageResultData = component.GetInfo();
				UnityEngine.Object.Destroy(gameObject);
				bool quickMode = this.m_stageResultData.m_quickMode;
				if (quickMode && this.m_stageResultData.m_missionComplete)
				{
					this.m_flags.Set(16, true);
				}
				if (this.m_stageResultData.m_validResult)
				{
					this.ReflectMileageProductionResultData();
					this.m_flags.Set(7, true);
				}
				else if (this.m_stageResultData.m_fromOptionTutorial && SaveDataManager.Instance != null)
				{
					PlayerData playerData = SaveDataManager.Instance.PlayerData;
					ServerPlayerState playerState = ServerInterface.PlayerState;
					if (playerState != null && playerData != null)
					{
						PlayerData arg_148_0 = playerData;
						ServerItem serverItem = new ServerItem((ServerItem.Id)playerState.m_mainCharaId);
						arg_148_0.MainChara = serverItem.charaType;
						PlayerData arg_164_0 = playerData;
						ServerItem serverItem2 = new ServerItem((ServerItem.Id)playerState.m_mainChaoId);
						arg_164_0.MainChaoID = serverItem2.chaoId;
						PlayerData arg_180_0 = playerData;
						ServerItem serverItem3 = new ServerItem((ServerItem.Id)playerState.m_subChaoId);
						arg_180_0.SubChaoID = serverItem3.chaoId;
					}
				}
			}
		}
		else if (flag2)
		{
			this.m_flags.Set(8, false);
			bool flag3 = !HudMenuUtility.IsTutorial_11() && !HudMenuUtility.IsRouletteTutorial() && !HudMenuUtility.IsTutorialCharaLevelUp();
			if (flag3 && SaveDataManager.Instance != null && !SaveDataManager.Instance.PlayerData.DailyMission.missions_complete)
			{
				this.m_flags.Set(16, true);
			}
		}
	}

	private bool HaveNoticeInfo()
	{
		return ServerInterface.NoticeInfo != null && !ServerInterface.NoticeInfo.IsAllChecked();
	}

	private void ReflectMileageProductionResultData()
	{
		if (this.m_stageResultData.m_oldMapState == null)
		{
			this.m_stageResultData.m_oldMapState = new MileageMapState();
		}
		if (this.m_stageResultData.m_newMapState == null)
		{
			this.m_stageResultData.m_newMapState = this.m_stageResultData.m_oldMapState;
		}
		if (ServerInterface.MileageMapState != null)
		{
			ServerInterface.MileageMapState.CopyTo(this.m_mileage_map_state);
		}
		this.SetMapState(ref this.m_mileage_map_state, this.m_stageResultData.m_newMapState);
		this.SetMapState(ref this.m_prev_mileage_map_state, this.m_stageResultData.m_oldMapState);
		if (this.CheckNextMap())
		{
			this.m_flags.Set(6, true);
		}
	}

	private void SetMapState(ref ServerMileageMapState map_state, MileageMapState result_map_state)
	{
		if (map_state != null && result_map_state != null)
		{
			map_state.m_episode = result_map_state.m_episode;
			map_state.m_chapter = result_map_state.m_chapter;
			map_state.m_point = result_map_state.m_point;
			map_state.m_stageTotalScore = result_map_state.m_score;
			if (map_state.m_episode == 0)
			{
				map_state.m_episode = 1;
			}
			if (map_state.m_chapter == 0)
			{
				map_state.m_chapter = 1;
			}
		}
	}

	private bool IsRequestChoaWheelOption()
	{
		ServerMileageMapState mileageMapState = ServerInterface.MileageMapState;
		return mileageMapState != null && !ServerInterface.ChaoWheelOptions.IsConnected;
	}

	private bool IsRequestDailyBattle()
	{
		if (ServerInterface.LoggedInServerInterface != null)
		{
			GameObject x = GameObject.Find("ResultInfo");
			if (x == null)
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckNextMap()
	{
		if (this.m_stageResultData != null && this.m_stageResultData.m_newMapState != null && this.m_stageResultData.m_oldMapState != null)
		{
			if (this.m_stageResultData.m_oldMapState.m_episode != this.m_stageResultData.m_newMapState.m_episode)
			{
				return true;
			}
			if (this.m_stageResultData.m_oldMapState.m_chapter != this.m_stageResultData.m_newMapState.m_chapter)
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckEventStateRequest()
	{
		return EventManager.Instance != null && EventManager.Instance.Type == EventManager.EventType.COLLECT_OBJECT && !EventManager.Instance.IsSetEventStateInfo && EventManager.Instance.IsInEvent();
	}

	private void CheckEventEnd()
	{
		if (EventManager.Instance != null && EventManager.Instance.Type != EventManager.EventType.UNKNOWN && !EventManager.Instance.IsInEvent())
		{
			EventManager.Instance.CheckEvent();
			if (ResourceManager.Instance != null)
			{
				ResourceManager.Instance.RemoveResources(ResourceCategory.EVENT_RESOURCE);
			}
		}
	}

	private void ServerGetChaoWheelOptions_Succeeded(MsgGetChaoWheelOptionsSucceed msg)
	{
		this.m_connected = true;
	}

	private void CallbackRouletteInit(int specialEggNum)
	{
		this.m_connected = true;
	}

	private void ServerGetInformation_Succeeded(MsgGetInformationSucceed msg)
	{
		this.m_is_end_notice_connect = true;
	}

	private void DailyBattleManagerCallBack()
	{
		this.m_connected = true;
	}

	private TinyFsmState StateInviteFriend(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_fristLaunchInviteFriend != null)
			{
				UnityEngine.Object.Destroy(this.m_fristLaunchInviteFriend.gameObject);
			}
			return TinyFsmState.End();
		case 1:
			this.CreateFirstLaunchInviteFriend();
			return TinyFsmState.End();
		case 4:
			if (this.m_fristLaunchInviteFriend != null)
			{
				if (this.m_startLauncherInviteFriendFlag)
				{
					if (this.m_fristLaunchInviteFriend.IsEndPlay)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
					}
				}
				else
				{
					this.m_fristLaunchInviteFriend.Setup("Camera/menu_Anim/MainMenuUI4/Anchor_5_MC");
					this.m_fristLaunchInviteFriend.PlayStart();
					this.m_startLauncherInviteFriendFlag = true;
				}
			}
			else
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void CreateFirstLaunchInviteFriend()
	{
		if (this.m_fristLaunchInviteFriend == null)
		{
			GameObject gameObject = new GameObject();
			if (gameObject != null)
			{
				gameObject.name = "FirstLaunchInviteFriend";
				gameObject.AddComponent<FirstLaunchInviteFriend>();
				this.m_fristLaunchInviteFriend = gameObject.GetComponent<FirstLaunchInviteFriend>();
			}
		}
	}

	private TinyFsmState StatePlayItem(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.SHOP)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateShop)), MainMenu.SequenceState.Shop);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.STAGE_CHECK)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckStage)), MainMenu.SequenceState.CheckStage);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.EPISODE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadMileageXml)), MainMenu.SequenceState.LoadMileageXml);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoad(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(6);
			}
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
		{
			TimeProfiler.StartCountTime("StateLoad");
			this.CeateSceneLoader();
			if (this.m_scene_loader_obj != null)
			{
				ResourceSceneLoader component = this.m_scene_loader_obj.GetComponent<ResourceSceneLoader>();
				TextManager.LoadCommonText(component);
				TextManager.LoadChaoText(component);
				TextManager.LoadEventText(component);
			}
			this.CeateSceneLoader();
			this.SetLoadEventResource();
			if (GameObject.Find("MissionTable") == null)
			{
				this.AddSceneLoader("MissionTable");
			}
			if (GameObject.Find("CharacterDataNameInfo") == null)
			{
				this.AddSceneLoader("CharacterDataNameInfo");
			}
			if (!this.IsExistMileageMapData(this.m_mileage_map_state))
			{
				this.AddSceneLoader(this.GetMileageMapDataScenaName(this.m_mileage_map_state));
			}
			if (GameObject.Find("MileageDataTable") == null)
			{
				this.AddSceneLoader("MileageDataTable");
			}
			string suffixe = TextUtility.GetSuffixe();
			if (GameObject.Find("ui_tex_ranking_" + suffixe) == null)
			{
				this.AddSceneLoader("ui_tex_ranking_" + suffixe);
			}
			if (InformationDataTable.Instance == null)
			{
				InformationDataTable.Create();
				InformationDataTable.Instance.Initialize(base.gameObject);
			}
			if (this.m_scene_loader_obj != null)
			{
				StageAbilityManager.LoadAbilityDataTable(this.m_scene_loader_obj.GetComponent<ResourceSceneLoader>());
			}
			this.AddSceneLoader("MainMenuPages");
			return TinyFsmState.End();
		}
		case 4:
		{
			bool flag = true;
			if (this.m_buttonEventResourceLoader != null)
			{
				flag = this.m_buttonEventResourceLoader.IsLoaded;
			}
			if (flag && this.CheckSceneLoad())
			{
				TextManager.SetupCommonText();
				TextManager.SetupChaoText();
				TextManager.SetupEventText();
				if (this.m_eventSpecficText)
				{
					TextManager.SetupEventProductionText();
				}
				this.DestroySceneLoader();
				this.SetMainMenuPages();
				this.SetEventResources();
				StageAbilityManager.SetupAbilityDataTable();
				TimeProfiler.EndCountTime("StateLoad");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadAtlas)), MainMenu.SequenceState.LoadAtlas);
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadAtlas(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(7);
			}
			return TinyFsmState.End();
		case 1:
			TimeProfiler.StartCountTime("StateLoadAtlas");
			this.CeateSceneLoader();
			this.StartLoadAtlas();
			return TinyFsmState.End();
		case 4:
			TimeProfiler.EndCountTime("StateLoadAtlas");
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateStartMessage)), MainMenu.SequenceState.StartMessage);
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private bool CheckSceneLoad()
	{
		if (this.m_scene_loader_obj != null)
		{
			ResourceSceneLoader component = this.m_scene_loader_obj.GetComponent<ResourceSceneLoader>();
			if (component != null)
			{
				return component.Loaded;
			}
		}
		return true;
	}

	private string GetMileageMapDataScenaName(ServerMileageMapState state)
	{
		if (state != null)
		{
			return "MileageMapData" + state.m_episode.ToString("000") + "_" + state.m_chapter.ToString("00");
		}
		return null;
	}

	private void CeateSceneLoader()
	{
		if (this.m_scene_loader_obj == null)
		{
			this.m_scene_loader_obj = new GameObject("SceneLoader");
			if (this.m_scene_loader_obj != null)
			{
				this.m_scene_loader_obj.AddComponent<ResourceSceneLoader>();
			}
		}
	}

	private void DestroySceneLoader()
	{
		UnityEngine.Object.Destroy(this.m_scene_loader_obj);
		this.m_scene_loader_obj = null;
	}

	private void AddSceneLoader(string scene_name)
	{
		if (scene_name != null && this.m_scene_loader_obj != null)
		{
			ResourceSceneLoader component = this.m_scene_loader_obj.GetComponent<ResourceSceneLoader>();
			if (component != null)
			{
				bool onAssetBundle = true;
				component.AddLoad(scene_name, onAssetBundle, false);
			}
		}
	}

	private void AddSceneLoaderAndResourceManager(ResourceSceneLoader.ResourceInfo resInfo)
	{
		if (this.m_scene_loader_obj != null)
		{
			ResourceSceneLoader component = this.m_scene_loader_obj.GetComponent<ResourceSceneLoader>();
			if (component != null)
			{
				component.AddLoadAndResourceManager(resInfo);
			}
		}
	}

	private void AddIDList(ref List<int> request_list, int id, string type, bool keep)
	{
		if (request_list != null && id != -1 && id != 0)
		{
			MileageMapDataManager instance = MileageMapDataManager.Instance;
			if (instance != null)
			{
				string text = string.Empty;
				if (type == "bg")
				{
					text = MileageMapBGDataTable.Instance.GetTextureName(id);
				}
				else if (type == "face")
				{
					text = MileageMapUtility.GetFaceTextureName(id);
				}
				if (text != string.Empty && keep)
				{
					instance.AddKeepList(text);
				}
			}
			foreach (int current in request_list)
			{
				if (current == id)
				{
					return;
				}
			}
			request_list.Add(id);
		}
	}

	private void SetLoadingFaceTexture(ref List<int> requestFaceList, ref List<int> loadingList, MileageMapData mileageMapData, int windowId)
	{
		if (mileageMapData != null && windowId < mileageMapData.window_data.Length)
		{
			int face_id = mileageMapData.window_data[windowId].body[0].product[0].face_id;
			this.AddIDList(ref requestFaceList, face_id, "face", true);
			loadingList.Add(face_id);
		}
	}

	private void SetLoadWindowFaceTexture(ref List<int> requestFaceList, MileageMapData mileageMapData, ServerMileageMapState state = null)
	{
		if (mileageMapData == null)
		{
			return;
		}
		List<int> list = new List<int>();
		int num = mileageMapData.event_data.point.Length;
		if (state != null)
		{
			for (int i = 0; i < num; i++)
			{
				if (state.m_point <= i)
				{
					if (i != 5 || !mileageMapData.event_data.IsBossEvent())
					{
						list.Add(mileageMapData.event_data.point[i].window_id);
					}
					else
					{
						BossEvent bossEvent = mileageMapData.event_data.GetBossEvent();
						list.Add(bossEvent.after_window_id);
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < num; j++)
			{
				if (this.m_prev_mileage_map_state.m_point <= j && this.m_mileage_map_state.m_point >= j)
				{
					if (j != 5 || !mileageMapData.event_data.IsBossEvent())
					{
						list.Add(mileageMapData.event_data.point[j].window_id);
					}
					else
					{
						list.Add(mileageMapData.event_data.point[j].boss.before_window_id);
					}
				}
			}
		}
		int num2 = mileageMapData.window_data.Length;
		for (int k = 0; k < num2; k++)
		{
			int num3 = mileageMapData.window_data[k].body.Length;
			for (int l = 0; l < num3; l++)
			{
				if (mileageMapData.window_data[k].body[l].product == null)
				{
					mileageMapData.window_data[k].body[l].product = new WindowProductData[0];
				}
				if (list.Contains(mileageMapData.window_data[k].id))
				{
					int num4 = mileageMapData.window_data[k].body[l].product.Length;
					for (int m = 0; m < num4; m++)
					{
						this.AddIDList(ref requestFaceList, mileageMapData.window_data[k].body[l].product[m].face_id, "face", false);
					}
				}
			}
		}
	}

	public void SetMainMenuPages()
	{
		GameObject gameObject = GameObject.Find("MainMenuPages");
		if (gameObject != null)
		{
			GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
			GameObject mainMenuGeneralAnchor = HudMenuUtility.GetMainMenuGeneralAnchor();
			if (menuAnimUIObject != null && mainMenuGeneralAnchor != null)
			{
				Transform transform = gameObject.transform;
				int childCount = transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = transform.GetChild(0);
					Vector3 localPosition = child.localPosition;
					Vector3 localScale = child.localScale;
					if (child.name == "item_get_Window" || child.name == "RankingWindowUI")
					{
						child.parent = mainMenuGeneralAnchor.transform;
					}
					else
					{
						child.parent = menuAnimUIObject.transform;
					}
					child.localPosition = localPosition;
					child.localScale = localScale;
					child.gameObject.SetActive(false);
				}
			}
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private void SetLoadEventResource()
	{
		if (EventManager.Instance != null)
		{
			if (EventManager.Instance.IsQuickEvent())
			{
				string resourceName = EventManager.GetResourceName();
				ResourceSceneLoader.ResourceInfo resourceInfo = this.m_loadInfo[0];
				resourceInfo.m_scenename += resourceName;
				this.AddSceneLoaderAndResourceManager(resourceInfo);
				ResourceSceneLoader.ResourceInfo resourceInfo2 = this.m_loadInfo[1];
				resourceInfo2.m_scenename += resourceName;
				this.AddSceneLoaderAndResourceManager(resourceInfo2);
				this.m_eventResourceId = EventManager.Instance.Id;
				this.LoadNewsWindow();
			}
			else if (EventManager.Instance.IsBGMEvent())
			{
				string resourceName2 = EventManager.GetResourceName();
				ResourceSceneLoader.ResourceInfo resourceInfo3 = this.m_loadInfo[0];
				resourceInfo3.m_scenename += resourceName2;
				this.AddSceneLoaderAndResourceManager(resourceInfo3);
				this.m_eventResourceId = EventManager.Instance.Id;
				this.LoadNewsWindow();
			}
		}
	}

	private void LoadNewsWindow()
	{
		this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
		this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("NewsWindow", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
	}

	private void SetLoadTopMenuTexture()
	{
		ResourceSceneLoader.ResourceInfo resourceInfo = this.m_loadInfo[2];
		int num = 1;
		if (StageModeManager.Instance != null)
		{
			num = StageModeManager.Instance.QuickStageIndex;
		}
		resourceInfo.m_scenename = "ui_tex_mile_w" + num.ToString("D2") + "A";
		this.AddSceneLoaderAndResourceManager(resourceInfo);
	}

	private void SetEventResources()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsQuickEvent())
		{
			EventCommonDataTable.LoadSetup();
		}
	}

	private void StartLoadAtlas()
	{
		if (FontManager.Instance != null)
		{
			FontManager.Instance.LoadResourceData();
		}
		if (AtlasManager.Instance != null)
		{
			AtlasManager.Instance.StartLoadAtlasForMenu();
		}
		if (TextureAsyncLoadManager.Instance != null)
		{
			PlayerData playerData = SaveDataManager.Instance.PlayerData;
			TextureRequestChara request = new TextureRequestChara(playerData.MainChara, null);
			TextureRequestChara request2 = new TextureRequestChara(playerData.SubChara, null);
			TextureAsyncLoadManager.Instance.Request(request);
			TextureAsyncLoadManager.Instance.Request(request2);
			UnityEngine.Random.seed = NetUtil.GetCurrentUnixTime();
			int textureIndex = UnityEngine.Random.Range(0, TextureRequestEpisodeBanner.BannerCount);
			TextureRequestEpisodeBanner request3 = new TextureRequestEpisodeBanner(textureIndex, null);
			TextureAsyncLoadManager.Instance.Request(request3);
			StageModeManager instance = StageModeManager.Instance;
			if (instance != null)
			{
				instance.DrawQuickStageIndex();
				TextureRequestStagePicture request4 = new TextureRequestStagePicture(instance.QuickStageIndex, null);
				TextureAsyncLoadManager.Instance.Request(request4);
			}
		}
		if (ChaoTextureManager.Instance != null)
		{
			ChaoTextureManager.Instance.RequestLoadingPageChaoTexture();
		}
	}

	private bool IsLoadedAtlas()
	{
		return !(AtlasManager.Instance != null) || AtlasManager.Instance.IsLoadAtlas();
	}

	private void SetReplaceAtlas()
	{
		if (FontManager.Instance != null)
		{
			FontManager.Instance.ReplaceFont();
		}
		if (AtlasManager.Instance != null)
		{
			AtlasManager.Instance.ReplaceAtlasForMenu(true);
		}
	}

	private TinyFsmState StateLoginBonusWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("LoginWindowUI", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			this.m_flags.Set(18, false);
			return TinyFsmState.End();
		case 4:
			if (this.m_buttonEventResourceLoader.IsLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoginBonusWindowDisplay)), MainMenu.SequenceState.LoginBonusWindowDisplay);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoginBonusWindowDisplay(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
			if (menuAnimUIObject != null)
			{
				this.m_LoginWindowUI = GameObjectUtil.FindChildGameObjectComponent<LoginBonusWindowUI>(menuAnimUIObject, "LoginWindowUI");
				if (this.m_LoginWindowUI != null)
				{
					this.m_LoginWindowUI.gameObject.SetActive(true);
					this.m_LoginWindowUI.PlayStart(LoginBonusWindowUI.LoginBonusType.NORMAL);
				}
			}
			this.m_flags.Set(17, false);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_LoginWindowUI != null && this.m_LoginWindowUI.IsEnd)
			{
				this.m_LoginWindowUI = null;
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateFirstLoginBonusWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("StartDashWindowUI", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			this.m_flags.Set(18, false);
			return TinyFsmState.End();
		case 4:
			if (this.m_buttonEventResourceLoader.IsLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateFirstLoginBonusWindowDisplay)), MainMenu.SequenceState.FirstLoginBonusWindowDisplay);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateFirstLoginBonusWindowDisplay(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
			if (menuAnimUIObject != null)
			{
				this.m_LoginWindowUI = GameObjectUtil.FindChildGameObjectComponent<LoginBonusWindowUI>(menuAnimUIObject, "StartDashWindowUI");
				if (this.m_LoginWindowUI != null)
				{
					this.m_LoginWindowUI.gameObject.SetActive(true);
					this.m_LoginWindowUI.PlayStart(LoginBonusWindowUI.LoginBonusType.FIRST);
				}
			}
			this.m_flags.Set(18, false);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_LoginWindowUI != null && this.m_LoginWindowUI.IsEnd)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				this.m_LoginWindowUI = null;
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoginRanking(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_flags.Set(8, false);
			return TinyFsmState.End();
		case 4:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null && msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.RANKING_END)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateMainMenu(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.BossChallenge = false;
			this.m_flags.Set(4, false);
			this.m_flags.Set(5, false);
			this.m_flags.Set(28, false);
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			HudMenuUtility.OnForceDisableShopButton(false);
			GameObjectUtil.SendMessageFindGameObject("MainMenuUI4", "UpdateView", null, SendMessageOptions.DontRequireReceiver);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal != 102)
			{
				return TinyFsmState.End();
			}
			this.CreateTitleBackWindow();
			return TinyFsmState.End();
		case 4:
			this.CheckTutoralWindow();
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null && this.JudgeMainMenuReceiveEvent(msgMenuSequence))
			{
				this.m_communicateFlag.Reset();
			}
			return TinyFsmState.End();
		}
		}
		goto IL_27;
	}

	private TinyFsmState StateMainMenuConnect(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CheckEventParameter();
			this.m_communicateFlag.Set(0, this.IsTickerCommunicate());
			this.m_communicateFlag.Set(2, this.IsMsgBoxCommunicate());
			this.m_communicateFlag.Set(4, this.IsSchemeCommunicate());
			this.m_communicateFlag.Set(6, this.IsChangeDataVersion());
			if (!this.m_communicateFlag.Test(8))
			{
				this.m_communicateFlag.Set(7, true);
			}
			return TinyFsmState.End();
		case 4:
		{
			if (this.m_buttonEvent != null && !this.m_buttonEvent.IsTransform && this.m_flags.Test(21))
			{
				this.m_communicateFlag.Set(4, this.IsSchemeCommunicate());
				this.m_flags.Set(21, false);
			}
			bool stageResultData = this.m_stageResultData != null;
			bool flag = this.m_stageResultData != null && this.m_stageResultData.m_quickMode;
			bool flag2 = this.m_stageResultData != null && this.m_stageResultData.m_rivalHighScore;
			bool flag3 = false;
			if (SingletonGameObject<RankingManager>.Instance != null)
			{
				RankingUtil.RankChange rankingRankChange = SingletonGameObject<RankingManager>.Instance.GetRankingRankChange(RankingUtil.RankingMode.QUICK, RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.RIVAL);
				flag3 = (rankingRankChange == RankingUtil.RankChange.UP);
			}
			if (this.m_flags.Test(25) && stageResultData && flag && flag2)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateBestRecordCheckEnableFeed)), MainMenu.SequenceState.BestRecordCheckEnableFeed);
				this.m_flags.Set(25, false);
			}
			else if (this.m_flags.Test(26) && stageResultData && flag && flag2 && flag3)
			{
				MainMenu.m_debug++;
				this.m_flags.Set(26, false);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateQuickModeRankUp)), MainMenu.SequenceState.QuickModeRankUp);
			}
			else if (this.m_communicateFlag.Test(6))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateVersionChangeWindow)), MainMenu.SequenceState.VersionChangeWindow);
			}
			else if (this.m_flags.Test(13))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateEventResourceChangeWindow)), MainMenu.SequenceState.EventResourceChangeWindow);
			}
			else if (FirstLaunchUserName.IsFirstLaunch)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameSetting)), MainMenu.SequenceState.UserNameSetting);
			}
			else if (!AgeVerification.IsAgeVerificated)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateAgeVerification)), MainMenu.SequenceState.AgeVerification);
			}
			else if (this.m_communicateFlag.Test(0) && !this.m_communicateFlag.Test(1))
			{
				HudMenuUtility.SendMsgTickerReset();
				this.m_communicateFlag.Set(1, true);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateTickerCommunicate)), MainMenu.SequenceState.TickerCommunicate);
			}
			else if (this.m_communicateFlag.Test(4))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateSchemeCommunicate)), MainMenu.SequenceState.SchemeCommunicate);
			}
			else if (this.m_communicateFlag.Test(2) && !this.m_communicateFlag.Test(3))
			{
				this.m_communicateFlag.Set(3, true);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMsgBoxCommunicate)), MainMenu.SequenceState.MsgBoxCommunicate);
			}
			else if (this.m_communicateFlag.Test(7))
			{
				this.m_communicateFlag.Set(8, true);
				this.m_communicateFlag.Set(7, false);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateDayCrossCommunicate)), MainMenu.SequenceState.DayCrossCommunicate);
			}
			else if (this.m_communicateFlag.Test(9))
			{
				this.m_communicateFlag.Set(9, false);
				this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStateLoadEventResource)), MainMenu.SequenceState.LoadEventResource);
			}
			else
			{
				this.m_communicateFlag.Set(7, false);
				this.m_communicateFlag.Set(8, false);
				this.m_flags.Set(20, true);
				if (this.m_flags.Test(18))
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateFirstLoginBonusWindow)), MainMenu.SequenceState.FirstLoginBonusWindow);
				}
				else if (this.m_flags.Test(17))
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoginBonusWindow)), MainMenu.SequenceState.LoginBonusWindow);
				}
				else if (this.m_flags.Test(16))
				{
					HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.DAILY_CHALLENGE, false);
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateDailyMissionWindow)), MainMenu.SequenceState.DailyMissionWindow);
				}
				else if (SingletonGameObject<DailyBattleManager>.Instance != null && SingletonGameObject<DailyBattleManager>.Instance.currentRewardFlag)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateDailyBattleRewardWindow)), MainMenu.SequenceState.DailyBattleRewardWindow);
				}
				else if (this.HaveNoticeInfo())
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateInformationWindow)), MainMenu.SequenceState.InformationWindow);
				}
				else if (HudMenuUtility.IsTutorialCharaLevelUp())
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialCharaLevelUpMenuStart)), MainMenu.SequenceState.TutorialCharaLevelUpMenuStart);
				}
				else if (HudMenuUtility.IsRouletteTutorial())
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialMenuRoulette)), MainMenu.SequenceState.TutorialMenuRoulette);
				}
				else if (HudMenuUtility.IsRecommendReviewTutorial())
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRecommendReview)), MainMenu.SequenceState.RecommendReview);
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenu)), MainMenu.SequenceState.Main);
				}
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateTickerCommunicate(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetTicker(base.gameObject);
			}
			else
			{
				this.m_communicateFlag.Set(0, false);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (!this.m_communicateFlag.Test(0))
			{
				HudMenuUtility.SendMsgTickerUpdate();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateMsgBoxCommunicate(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetMessageList(base.gameObject);
			}
			else
			{
				this.m_communicateFlag.Set(2, false);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (!this.m_communicateFlag.Test(2))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSchemeCommunicate(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			this.ClearUrlScheme();
			return TinyFsmState.End();
		case 1:
			if (ServerAtomSerial.GetSerialFromScheme(this.GetUrlSchemeStr(), ref this.m_atomCampain, ref this.m_atomSerial))
			{
				this.CreateSchemeCheckWinow();
			}
			else
			{
				this.m_communicateFlag.Set(4, false);
			}
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("SchemeCheck") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				bool new_user = true;
				SystemSaveManager instance = SystemSaveManager.Instance;
				if (instance != null)
				{
					SystemData systemdata = instance.GetSystemdata();
					if (systemdata != null)
					{
						new_user = systemdata.IsNewUser();
					}
				}
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				loggedInServerInterface.RequestServerAtomSerial(this.m_atomCampain, this.m_atomSerial, new_user, base.gameObject);
			}
			if (!this.m_communicateFlag.Test(4))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateResultSchemeCommunicate)), MainMenu.SequenceState.ResultSchemeCommunicate);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateResultSchemeCommunicate(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			this.ClearUrlScheme();
			return TinyFsmState.End();
		case 1:
			this.CreateSchemeResultWinow();
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("SchemeResult") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDayCrossCommunicate(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_callBackFlag.Reset();
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null && ServerDayCrossWatcher.Instance != null)
			{
				ServerDayCrossWatcher.Instance.UpdateClientInfosByDayCross(new ServerDayCrossWatcher.UpdateInfoCallback(this.DataCrossCallBack));
				ServerDayCrossWatcher.Instance.UpdateDailyMissionForOneDay(new ServerDayCrossWatcher.UpdateInfoCallback(this.DailyMissionForOneDataCallBack));
				ServerDayCrossWatcher.Instance.UpdateDailyMissionInfoByChallengeEnd(new ServerDayCrossWatcher.UpdateInfoCallback(this.DailyMissionChallengeEndCallBack));
				ServerDayCrossWatcher.Instance.UpdateLoginBonusEnd(new ServerDayCrossWatcher.UpdateInfoCallback(this.LoginBonusUpdateCallBack));
			}
			else
			{
				this.m_callBackFlag.Set(0, true);
				this.m_callBackFlag.Set(2, true);
				this.m_callBackFlag.Set(4, true);
				this.m_callBackFlag.Set(7, true);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_callBackFlag.Test(0) && this.m_callBackFlag.Test(2) && this.m_callBackFlag.Test(4) && this.m_callBackFlag.Test(7))
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void CreateSchemeCheckWinow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "SchemeCheck",
			buttonType = GeneralWindow.ButtonType.Ok,
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "atom_check"),
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "atom_check_caption")
		});
	}

	private void CreateSchemeResultWinow()
	{
		string message = string.Empty;
		string caption = string.Empty;
		if (this.m_communicateFlag.Test(5))
		{
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", this.m_atomInvalidTextId);
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "atom_failure_caption");
		}
		else
		{
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "atom_present_get");
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "atom_success_caption");
		}
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "SchemeResult",
			buttonType = GeneralWindow.ButtonType.Ok,
			message = message,
			caption = caption
		});
	}

	private bool JudgeMainMenuReceiveEvent(MsgMenuSequence msg_sequence)
	{
		switch (msg_sequence.Sequenece)
		{
		case MsgMenuSequence.SequeneceType.TITLE:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateTitle)), MainMenu.SequenceState.Title);
			return true;
		case MsgMenuSequence.SequeneceType.STAGE:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateStage)), MainMenu.SequenceState.Stage);
			return true;
		case MsgMenuSequence.SequeneceType.PRESENT_BOX:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StatePresentBox)), MainMenu.SequenceState.PresentBox);
			return true;
		case MsgMenuSequence.SequeneceType.DAILY_CHALLENGE:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateDailyMissionWindow)), MainMenu.SequenceState.DailyMissionWindow);
			return true;
		case MsgMenuSequence.SequeneceType.DAILY_BATTLE:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateDailyBattle)), MainMenu.SequenceState.DailyBattle);
			return true;
		case MsgMenuSequence.SequeneceType.CHARA_MAIN:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainCharaSelect)), MainMenu.SequenceState.CharaSelect);
			return true;
		case MsgMenuSequence.SequeneceType.CHAO:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateChaoSelect)), MainMenu.SequenceState.ChaoSelect);
			return true;
		case MsgMenuSequence.SequeneceType.OPTION:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateOption)), MainMenu.SequenceState.Option);
			return true;
		case MsgMenuSequence.SequeneceType.INFOMATION:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateInfomation)), MainMenu.SequenceState.Infomation);
			return true;
		case MsgMenuSequence.SequeneceType.ROULETTE:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
			return true;
		case MsgMenuSequence.SequeneceType.SHOP:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateShop)), MainMenu.SequenceState.Shop);
			return true;
		case MsgMenuSequence.SequeneceType.EPISODE:
		{
			StageModeManager instance = StageModeManager.Instance;
			if (instance != null)
			{
				instance.StageMode = StageModeManager.Mode.ENDLESS;
			}
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadMileageXml)), MainMenu.SequenceState.LoadMileageXml);
			return true;
		}
		case MsgMenuSequence.SequeneceType.EPISODE_PLAY:
		case MsgMenuSequence.SequeneceType.MAIN_PLAY_BUTTON:
		{
			StageModeManager instance2 = StageModeManager.Instance;
			if (instance2 != null)
			{
				instance2.StageMode = StageModeManager.Mode.ENDLESS;
			}
			this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStatePlayButton)), MainMenu.SequenceState.PlayButton);
			return true;
		}
		case MsgMenuSequence.SequeneceType.EPISODE_RANKING:
		case MsgMenuSequence.SequeneceType.QUICK_RANKING:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateRanking)), MainMenu.SequenceState.Ranking);
			return true;
		case MsgMenuSequence.SequeneceType.QUICK:
		{
			StageModeManager instance3 = StageModeManager.Instance;
			if (instance3 != null)
			{
				instance3.StageMode = StageModeManager.Mode.QUICK;
			}
			this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStatePlayButton)), MainMenu.SequenceState.PlayButton);
			return true;
		}
		case MsgMenuSequence.SequeneceType.BACK:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckBackTitle)), MainMenu.SequenceState.CheckBackTitle);
			return true;
		}
		return false;
	}

	private void ServerGetTicker_Succeeded(MsgGetTickerDataSucceed msg)
	{
		this.SetTickerCommunicate();
		this.m_communicateFlag.Set(0, false);
	}

	private void ServerGetTicker_Failed(MsgServerConnctFailed msg)
	{
		this.m_communicateFlag.Set(0, false);
	}

	private void ServerGetMessageList_Succeeded(MsgGetMessageListSucceed msg)
	{
		if (this.m_communicateFlag.Test(2))
		{
			this.SetMsgBoxCommunicate(false);
			this.m_communicateFlag.Set(2, false);
		}
		this.m_flags.Set(19, false);
	}

	private void ServerGetMessageList_Failed(MsgServerConnctFailed msg)
	{
		this.m_communicateFlag.Set(2, false);
	}

	private void ServerAtomSerial_Succeeded(MsgSendAtomSerialSucceed msg)
	{
		this.m_communicateFlag.Set(4, false);
		this.m_communicateFlag.Set(5, false);
		this.SetMsgBoxCommunicate(true);
	}

	private void ServerAtomSerial_Failed(MsgServerConnctFailed msg)
	{
		this.m_communicateFlag.Set(4, false);
		this.m_communicateFlag.Set(5, true);
		this.m_atomInvalidTextId = "atom_invalid_serial";
		if (msg != null && msg.m_status == ServerInterface.StatusCode.UsedSerialCode)
		{
			this.m_atomInvalidTextId = "atom_used_serial";
		}
	}

	private void DataCrossCallBack(ServerDayCrossWatcher.MsgDayCross msg)
	{
		if (msg != null && msg.ServerConnect)
		{
			this.m_callBackFlag.Set(1, true);
		}
		this.m_callBackFlag.Set(0, true);
	}

	private void DailyMissionForOneDataCallBack(ServerDayCrossWatcher.MsgDayCross msg)
	{
		if (msg != null && msg.ServerConnect)
		{
			this.m_callBackFlag.Set(3, true);
		}
		this.m_callBackFlag.Set(2, true);
	}

	private void DailyMissionChallengeEndCallBack(ServerDayCrossWatcher.MsgDayCross msg)
	{
		if (msg != null && msg.ServerConnect)
		{
			this.m_callBackFlag.Set(5, true);
		}
		this.m_callBackFlag.Set(4, true);
	}

	private void LoginBonusUpdateCallBack(ServerDayCrossWatcher.MsgDayCross msg)
	{
		if (msg != null)
		{
			if (msg.ServerConnect)
			{
				this.m_callBackFlag.Set(6, true);
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					ServerLoginBonusData loginBonusData = ServerInterface.LoginBonusData;
					if (loginBonusData != null)
					{
						if (!loginBonusData.isGetLoginBonusToday())
						{
							loggedInServerInterface.RequestServerLoginBonusSelect(loginBonusData.m_rewardId, loginBonusData.m_rewardDays, 0, loginBonusData.m_firstRewardDays, 0, base.gameObject);
						}
						else
						{
							this.m_callBackFlag.Set(7, true);
						}
					}
				}
			}
			else
			{
				this.m_callBackFlag.Set(7, true);
			}
		}
	}

	private void ServerLoginBonusSelect_Succeeded(MsgLoginBonusSelectSucceed msg)
	{
		bool flag = false;
		if (msg.m_loginBonusReward != null)
		{
			this.m_flags.Set(17, true);
			flag = true;
		}
		if (msg.m_firstLoginBonusReward != null)
		{
			this.m_flags.Set(18, true);
			flag = true;
		}
		this.m_callBackFlag.Set(7, true);
		if (flag)
		{
			this.SetMsgBoxCommunicate(true);
		}
	}

	private bool IsTickerCommunicate()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerTickerInfo tickerInfo = ServerInterface.TickerInfo;
			if (tickerInfo != null && tickerInfo.ExistNewData)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsMsgBoxCommunicate()
	{
		return SaveDataManager.Instance != null && SaveDataManager.Instance.ConnectData != null && SaveDataManager.Instance.ConnectData.ReplaceMessageBox;
	}

	private bool IsSchemeCommunicate()
	{
		if (Binding.Instance != null)
		{
			string urlSchemeStr = Binding.Instance.GetUrlSchemeStr();
			return !string.IsNullOrEmpty(urlSchemeStr);
		}
		return false;
	}

	private string GetUrlSchemeStr()
	{
		if (Binding.Instance != null)
		{
			return Binding.Instance.GetUrlSchemeStr();
		}
		return string.Empty;
	}

	private void ClearUrlScheme()
	{
		if (Binding.Instance != null)
		{
			Binding.Instance.ClearUrlSchemeStr();
		}
	}

	private bool IsChangeDataVersion()
	{
		if (ServerInterface.LoginState != null)
		{
			if (ServerInterface.LoginState.IsChangeDataVersion)
			{
				return true;
			}
			if (ServerInterface.LoginState.IsChangeAssetsVersion)
			{
				return true;
			}
		}
		return false;
	}

	private void SetTickerCommunicate()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerTickerInfo tickerInfo = ServerInterface.TickerInfo;
			if (tickerInfo != null)
			{
				tickerInfo.ExistNewData = false;
			}
		}
	}

	private void SetMsgBoxCommunicate(bool flag)
	{
		if (SaveDataManager.Instance != null && SaveDataManager.Instance.ConnectData != null)
		{
			SaveDataManager.Instance.ConnectData.ReplaceMessageBox = flag;
		}
	}

	private void CheckLimitEvent()
	{
		if (EventManager.Instance == null)
		{
			return;
		}
		if (EventManager.Instance.IsStandby())
		{
			this.m_flags.Set(22, true);
			if (EventManager.Instance.IsInEvent())
			{
				EventManager.Instance.CheckEvent();
				if (this.m_eventResourceId > 0 && this.m_eventResourceId != EventManager.Instance.Id)
				{
					this.m_flags.Set(13, true);
				}
				else
				{
					this.m_communicateFlag.Set(9, true);
				}
			}
		}
		else if (EventManager.Instance.Type != EventManager.EventType.UNKNOWN && !EventManager.Instance.IsInEvent())
		{
			EventManager.EventType type = EventManager.Instance.Type;
			EventManager.Instance.CheckEvent();
			if (EventManager.Instance.Type != EventManager.EventType.UNKNOWN && EventManager.Instance.IsInEvent())
			{
				this.m_flags.Set(13, true);
			}
			else
			{
				if (type == EventManager.EventType.QUICK)
				{
					StageModeManager.Instance.DrawQuickStageIndex();
				}
				this.m_communicateFlag.Set(9, true);
				if (AtlasManager.Instance != null)
				{
					AtlasManager.Instance.ResetEventRelaceAtlas();
				}
				if (ResourceManager.Instance != null)
				{
					ResourceManager.Instance.RemoveResources(ResourceCategory.EVENT_RESOURCE);
				}
			}
		}
	}

	private void CheckEventParameter()
	{
		this.SetEventStage(false);
		this.CheckLimitEvent();
	}

	private void SetMilageStageIndex()
	{
		int episode = 1;
		int chapter = 1;
		int type = 0;
		if (this.m_mileage_map_state != null)
		{
			episode = this.m_mileage_map_state.m_episode;
			chapter = this.m_mileage_map_state.m_chapter;
			type = this.m_mileage_map_state.m_point;
		}
		MileageMapUtility.SetMileageStageIndex(episode, chapter, (PointType)type);
	}

	private void ServerGetMileageReward_Succeeded(MsgGetMileageRewardSucceed msg)
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			if (msg != null)
			{
				instance.SetRewardData(msg.m_mileageRewardList);
			}
			else
			{
				instance.SetRewardData(ServerInterface.MileageRewardList);
			}
		}
		this.m_flags.Set(12, true);
	}

	private void ServerGetMileageReward_Failed()
	{
	}

	private TinyFsmState StateOption(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (!this.m_flags.Test(0))
			{
				ConnectAlertMaskUI.EndScreen(new Action(this.OnFinishedFadeInCallback));
			}
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.STAGE)
				{
					this.m_flags.Set(15, true);
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateStage)), MainMenu.SequenceState.Stage);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.TITLE)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTitle)), MainMenu.SequenceState.Title);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState MenuStatePlayButton(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_cautionType = this.GetCautionType();
			return TinyFsmState.End();
		case 4:
			if (this.m_cautionType != MainMenu.CautionType.CHALLENGE_COUNT)
			{
				this.m_bossChallenge = MileageMapUtility.IsBossStage();
				HudMenuUtility.SendVirtualNewItemSelectClicked(HudMenuUtility.ITEM_SELECT_MODE.NORMAL);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StatePlayItem)), MainMenu.SequenceState.PlayItem);
			}
			else
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateChallengeDisplyWindow)), MainMenu.SequenceState.ChallengeDisplyWindow);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateChallengeDisplyWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CreateStageCautionWindow();
			return TinyFsmState.End();
		case 4:
		{
			MainMenu.PressedButtonType pressedButtonType = this.m_pressedButtonType;
			switch (pressedButtonType + 1)
			{
			case MainMenu.PressedButtonType.BACK:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHALLENGE_TO_SHOP, false);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateShop)), MainMenu.SequenceState.Shop);
				break;
			case MainMenu.PressedButtonType.CANCEL:
			case MainMenu.PressedButtonType.NUM:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.ITEM_BACK, false);
				if (this.m_flags.Test(28))
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadMileageXml)), MainMenu.SequenceState.LoadMileageXml);
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				break;
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StatePresentBox(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.CHARA_MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainCharaSelect)), MainMenu.SequenceState.CharaSelect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.CHAO)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateChaoSelect)), MainMenu.SequenceState.ChaoSelect);
				}
				else if (msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.SHOP)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateShop)), MainMenu.SequenceState.Shop);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateUserNameSetting(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("window_name_setting", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 4:
			if (this.m_buttonEventResourceLoader.IsLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameSettingDisplay)), MainMenu.SequenceState.UserNameSettingDisplay);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateUserNameSettingDisplay(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = false;
			return TinyFsmState.End();
		case 1:
			if (this.m_userNameSetting == null)
			{
				GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
				if (menuAnimUIObject != null)
				{
					this.m_userNameSetting = GameObjectUtil.FindChildGameObjectComponent<FirstLaunchUserName>(menuAnimUIObject, "window_name_setting");
				}
			}
			if (this.m_userNameSetting != null)
			{
				this.m_userNameSetting.Setup(string.Empty);
				this.m_userNameSetting.PlayStart();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_userNameSetting != null && this.m_userNameSetting.IsEndPlay)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAgeVerification(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("AgeVerificationWindow", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			this.m_flags.Set(18, false);
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 4:
			if (this.m_buttonEventResourceLoader.IsLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateAgeVerificationDisplay)), MainMenu.SequenceState.AgeVerificationDisplay);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAgeVerificationDisplay(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = false;
			return TinyFsmState.End();
		case 1:
			if (this.m_ageVerification == null)
			{
				GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
				if (menuAnimUIObject != null)
				{
					this.m_ageVerification = GameObjectUtil.FindChildGameObjectComponent<AgeVerification>(menuAnimUIObject, "AgeVerificationWindow");
				}
			}
			if (this.m_ageVerification != null)
			{
				this.m_ageVerification.Setup(string.Empty);
				this.m_ageVerification.PlayStart();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_ageVerification != null && this.m_ageVerification.IsEnd)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateQuickModeRankUp(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
			this.m_buttonEventResourceLoader = null;
			return TinyFsmState.End();
		case 1:
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("RankingResultBitWindow", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			return TinyFsmState.End();
		case 4:
			if (this.m_buttonEventResourceLoader.IsLoaded)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateQuickModeRankUpDisplay)), MainMenu.SequenceState.QuickModeRankUpDisplay);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateQuickModeRankUpDisplay(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			RankingUtil.ShowRankingChangeWindow(RankingUtil.RankingMode.QUICK);
			return TinyFsmState.End();
		case 4:
			if (RankingUtil.IsEndRankingChangeWindow())
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRanking(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null && msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.MAIN)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRecommendReview(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_fristLaunchRecommendReview != null)
			{
				UnityEngine.Object.Destroy(this.m_fristLaunchRecommendReview.gameObject);
			}
			return TinyFsmState.End();
		case 1:
			this.CreateFirstLaunchRecommendReview();
			return TinyFsmState.End();
		case 4:
			if (this.m_fristLaunchRecommendReview != null)
			{
				if (this.m_startLauncherRecommendReviewFlag)
				{
					if (this.m_fristLaunchRecommendReview.IsEndPlay)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
					}
				}
				else
				{
					this.m_fristLaunchRecommendReview.Setup("Camera/menu_Anim/MainMenuUI4/Anchor_5_MC");
					this.m_fristLaunchRecommendReview.PlayStart();
					this.m_startLauncherRecommendReviewFlag = true;
				}
			}
			else
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void CreateFirstLaunchRecommendReview()
	{
		if (this.m_fristLaunchRecommendReview == null)
		{
			GameObject gameObject = new GameObject();
			if (gameObject != null)
			{
				gameObject.name = "FirstLaunchRecommendReview";
				gameObject.AddComponent<FirstLaunchRecommendReview>();
				this.m_fristLaunchRecommendReview = gameObject.GetComponent<FirstLaunchRecommendReview>();
			}
		}
	}

	private TinyFsmState StateRoulette(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				MsgMenuSequence.SequeneceType sequenece = msgMenuSequence.Sequenece;
				if (sequenece == MsgMenuSequence.SequeneceType.MAIN)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateShop(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null)
			{
				MsgMenuSequence.SequeneceType sequenece = msgMenuSequence.Sequenece;
				switch (sequenece)
				{
				case MsgMenuSequence.SequeneceType.PRESENT_BOX:
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePresentBox)), MainMenu.SequenceState.PresentBox);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.DAILY_CHALLENGE:
				case MsgMenuSequence.SequeneceType.DAILY_BATTLE:
				case MsgMenuSequence.SequeneceType.ITEM:
				case MsgMenuSequence.SequeneceType.RANKING:
				case MsgMenuSequence.SequeneceType.RANKING_END:
				case MsgMenuSequence.SequeneceType.CHAO_ROULETTE:
				case MsgMenuSequence.SequeneceType.ITEM_ROULETTE:
				case MsgMenuSequence.SequeneceType.SHOP:
				case MsgMenuSequence.SequeneceType.EPISODE_RANKING:
				case MsgMenuSequence.SequeneceType.QUICK_RANKING:
					IL_B3:
					if (sequenece != MsgMenuSequence.SequeneceType.MAIN)
					{
						goto IL_1CC;
					}
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.CHARA_MAIN:
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainCharaSelect)), MainMenu.SequenceState.CharaSelect);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.CHAO:
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateChaoSelect)), MainMenu.SequenceState.ChaoSelect);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.PLAY_ITEM:
				case MsgMenuSequence.SequeneceType.EPISODE_PLAY:
				case MsgMenuSequence.SequeneceType.QUICK:
				case MsgMenuSequence.SequeneceType.PLAY_AT_EPISODE_PAGE:
					this.ChangeState(new TinyFsmState(new EventFunction(this.MenuStatePlayButton)), MainMenu.SequenceState.PlayButton);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.OPTION:
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateOption)), MainMenu.SequenceState.Option);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.INFOMATION:
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateInfomation)), MainMenu.SequenceState.Infomation);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.ROULETTE:
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
					goto IL_1CC;
				case MsgMenuSequence.SequeneceType.EPISODE:
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadMileageXml)), MainMenu.SequenceState.LoadMileageXml);
					goto IL_1CC;
				}
				goto IL_B3;
			}
			IL_1CC:
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCheckStage(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_cautionType = this.GetCautionType();
			return TinyFsmState.End();
		case 4:
			if (this.m_cautionType == MainMenu.CautionType.NON)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateStage)), MainMenu.SequenceState.Stage);
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.GO_STAGE, false);
			}
			else
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateStageCautionWindow)), MainMenu.SequenceState.CautionStage);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateStageCautionWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CreateStageCautionWindow();
			return TinyFsmState.End();
		case 4:
		{
			MainMenu.PressedButtonType pressedButtonType = this.m_pressedButtonType;
			switch (pressedButtonType + 1)
			{
			case MainMenu.PressedButtonType.GOTO_SHOP:
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateStage)), MainMenu.SequenceState.Stage);
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.GO_STAGE, false);
				break;
			case MainMenu.PressedButtonType.BACK:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHALLENGE_TO_SHOP, false);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateShop)), MainMenu.SequenceState.Shop);
				break;
			case MainMenu.PressedButtonType.CANCEL:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.FORCE_MAIN_BACK, true);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenuConnect)), MainMenu.SequenceState.MainConnect);
				break;
			case MainMenu.PressedButtonType.NUM:
				this.ChangeState(new TinyFsmState(new EventFunction(this.StatePlayItem)), MainMenu.SequenceState.PlayItem);
				break;
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateStage(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_flags.Set(3, true);
			return TinyFsmState.End();
		case 4:
			this.LoadEventFaceTexture();
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeOut)), MainMenu.SequenceState.FadeOut);
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void LoadEventFaceTexture()
	{
		if (this.m_flags.Test(4) || this.m_flags.Test(5))
		{
			string eventResourceLoadingTextureName = EventUtility.GetEventResourceLoadingTextureName();
			if (eventResourceLoadingTextureName != null)
			{
				this.CeateSceneLoader();
				this.AddSceneLoader(eventResourceLoadingTextureName);
			}
		}
	}

	private TinyFsmState StateStartMessage(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(8);
			}
			return TinyFsmState.End();
		case 1:
			TimeProfiler.StartCountTime("StateStartMessage");
			RouletteUtility.rouletteDefault = RouletteCategory.PREMIUM;
			this.SetMilageStageIndex();
			HudMenuUtility.OnForceDisableShopButton(true);
			return TinyFsmState.End();
		case 4:
			TimeProfiler.EndCountTime("StateStartMessage");
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateRankingWait)), MainMenu.SequenceState.RankingWait);
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRankingWait(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(9);
			}
			return TinyFsmState.End();
		case 1:
		{
			TimeProfiler.StartCountTime("StateRankingWait");
			this.m_rankingCallBack = false;
			this.m_eventRankingCallBack = false;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null && SingletonGameObject<RankingManager>.Instance != null)
			{
				SingletonGameObject<RankingManager>.Instance.Init(new RankingManager.CallbackRankingData(this.NormalRankingDataCallback), new RankingManager.CallbackRankingData(this.EventDataCallback));
			}
			else
			{
				this.m_rankingCallBack = true;
				this.m_eventRankingCallBack = true;
			}
			GeneralUtil.SetDailyBattleBtnIcon(null, "Btn_2_battle");
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_rankingCallBack && this.m_eventRankingCallBack && this.IsLoadedAtlas() && this.CheckSceneLoad())
			{
				this.DestroySceneLoader();
				this.SetReplaceAtlas();
				TimeProfiler.EndCountTime("StateRankingWait");
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeIn)), MainMenu.SequenceState.FadeIn);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void NormalRankingDataCallback(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		this.m_rankingCallBack = true;
		RankingUI.Setup();
		bool flag = false;
		if (EventManager.Instance != null && EventManager.Instance.IsInEvent() && EventManager.Instance.Type == EventManager.EventType.SPECIAL_STAGE)
		{
			flag = true;
		}
		if (!flag)
		{
			this.m_eventRankingCallBack = true;
		}
	}

	private void EventDataCallback(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		this.m_eventRankingCallBack = true;
	}

	private TinyFsmState StateTitle(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_flags.Set(3, false);
			this.m_flags.Set(4, false);
			return TinyFsmState.End();
		case 4:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeOut)), MainMenu.SequenceState.FadeOut);
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCheckBackTitle(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CreateTitleBackWindow();
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("BackTitle") && GeneralWindow.IsButtonPressed)
			{
				if (GeneralWindow.IsYesButtonPressed)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTitle)), MainMenu.SequenceState.Title);
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainMenu)), MainMenu.SequenceState.Main);
				}
				GeneralWindow.Close();
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateTutorialCharaLevelUpMenuStart(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CreateCharaLevelUpWinow();
			BackKeyManager.TutorialFlag = true;
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("chara_level_up") && GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialCharaLevelUpMenuMoveChara)), MainMenu.SequenceState.TutorialCharaLevelUpMenuMoveChara);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateTutorialCharaLevelUpMenuMoveChara(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.TutorialFlag = false;
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.CHAOSELECT_MAIN);
			return TinyFsmState.End();
		case 1:
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.CHAOSELECT_MAIN);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal != 102)
			{
				return TinyFsmState.End();
			}
			this.CreateTitleBackWindow();
			return TinyFsmState.End();
		case 4:
			this.CheckTutoralWindow();
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null && msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.CHARA_MAIN)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateMainCharaSelect)), MainMenu.SequenceState.CharaSelect);
			}
			return TinyFsmState.End();
		}
		}
		goto IL_27;
	}

	private void CreateCharaLevelUpWinow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "chara_level_up",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextUtility.GetCommonText("MainMenu", "chara_level_up_move_explan_caption"),
			message = TextUtility.GetCommonText("MainMenu", "chara_level_up_move_explan")
		});
	}

	private TinyFsmState StateTutorialMenuRoulette(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.MAINMENU_ROULETTE);
			BackKeyManager.InvalidFlag = false;
			return TinyFsmState.End();
		case 1:
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.MAINMENU_ROULETTE);
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal != 102)
			{
				return TinyFsmState.End();
			}
			this.CreateTitleBackWindow();
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			MsgMenuSequence msgMenuSequence = fsm_event.GetMessage as MsgMenuSequence;
			if (msgMenuSequence != null && msgMenuSequence.Sequenece == MsgMenuSequence.SequeneceType.ROULETTE)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRoulette)), MainMenu.SequenceState.Roulette);
			}
			return TinyFsmState.End();
		}
		}
		goto IL_27;
	}

	private bool CheckTutorialRoulette()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		return loggedInServerInterface != null && RouletteUtility.isTutorial;
	}

	private void SetTutorialFlagToMainMenuUI()
	{
		GameObjectUtil.SendMessageFindGameObject("MainMenuUI4", "OnSetTutorial", null, SendMessageOptions.DontRequireReceiver);
	}

	private TinyFsmState StateVersionChangeWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CreateVersionChangeWindow(false);
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("VersionChange") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateTitle)), MainMenu.SequenceState.Title);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEventResourceChangeWindow(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.CreateVersionChangeWindow(true);
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("VersionChange") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateTitle)), MainMenu.SequenceState.Title);
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void CreateVersionChangeWindow(bool eventFlag)
	{
		string name = "VersionChange";
		if (!GeneralWindow.IsCreated(name))
		{
			if (eventFlag)
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = name,
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = TextUtility.GetCommonText("Option", "take_over_attention"),
					message = TextUtility.GetCommonText("MainMenu", "title_back_message")
				});
			}
			else
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = name,
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_reboot_app_caption"),
					message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_reboot_app")
				});
			}
		}
	}
}
