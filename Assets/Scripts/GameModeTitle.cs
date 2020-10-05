using AnimationOrTween;
using App;
using DataTable;
using Message;
using NoahUnity;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class GameModeTitle : MonoBehaviour
{
	public enum ProgressBarLeaveState
	{
		IDLE = -1,
		StateEndDataLoad,
		StateGameServerLogin,
		StateGetServerContinueParameter,
		StateCheckAtom,
		StateCheckNoLoginIncentive,
		StateSnsAdditionalData,
		StateWaitToGetMenuData_PlayerState,
		StateWaitToGetMenuData_CharacterState,
		StateWaitToGetMenuData_ChaoState,
		StateWaitToGetMenuData_WheelOptions,
		StateWaitToGetMenuData_DailyMissionData,
		StateWaitToGetMenuData_MessageList,
		StateWaitToGetMenuData_RedStarExchangeList,
		StateWaitToGetMenuData_RingExchangeList,
		StateWaitToGetMenuData_ChallengeExchangeList,
		StateWaitToGetMenuData_RaidBossEnergyExchangeList,
		StateWaitToGetMenuData,
		StateAchievementLogin,
		StateGetLeagueData,
		StateGetCostList,
		StateGetEventList,
		StateGetMileageMap,
		StateIapInitialize,
		StateLoadEventResource,
		StateLoadingUIData,
		NUM
	}

	private enum EventSignal
	{
		SCENE_CHANGE_REQUESTED = 100,
		SERVER_GETVERSION_END,
		SERVER_GET_CONTINUE_PARAMETER_END,
		SERVER_GETMENUDATA_END,
		SERVER_GET_RANKING_END,
		SERVER_GET_LEAGUE_DATA_END,
		SERVER_GET_COSTLIST_END,
		SERVER_GET_MILEAGEMAP_END,
		SERVER_GET_EVENT_LIST_END,
		FADE_END,
		SCREEN_TOUCHED,
		TAKEOVER_REQUESTED,
		TAKEOVER_ERROR,
		TAKEOVER_PASSERROR
	}

	private class AtomDataInfo
	{
		public string campain;

		public string serial;
	}

	private enum RedStarExchangeType
	{
		RSRING,
		RING,
		CHALLENGE,
		RAIDBOSS_ENERGY,
		Count
	}

	private enum SubStateSaveError
	{
		ShowMessage,
		Error
	}

	private enum SubStateTakeover
	{
		CautionWindow,
		InputIdAndPass,
		End
	}

	private enum SubStateCheckAtom
	{
		StartText,
		WaitServer,
		EndText,
		End
	}

	private enum SubStateCheckNoLogin
	{
		WaitServer,
		EndText,
		End
	}

	private sealed class _OpenAgreementWindow_c__Iterator21 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string _url___0;

		internal GameObject _htmlParserGameObject___1;

		internal HtmlParser _htmlParser___2;

		internal TextObject _title___3;

		internal int _PC;

		internal object _current;

		internal GameModeTitle __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this.__f__this.m_agreementText = "test";
				if (this.__f__this.m_agreementText != null && !(this.__f__this.m_agreementText == string.Empty))
				{
					goto IL_135;
				}
				this._url___0 = NetUtil.GetWebPageURL(InformationDataTable.Type.TERMS_OF_SERVICE);
				this._htmlParserGameObject___1 = HtmlParserFactory.Create(this._url___0, HtmlParser.SyncType.TYPE_ASYNC, HtmlParser.SyncType.TYPE_ASYNC);
				if (this._htmlParserGameObject___1 == null)
				{
					this._current = null;
					this._PC = 1;
					return true;
				}
				break;
			case 1u:
				break;
			case 2u:
				goto IL_DB;
			case 3u:
				goto IL_104;
			default:
				return false;
			}
			this._htmlParser___2 = this._htmlParserGameObject___1.GetComponent<HtmlParser>();
			if (this._htmlParser___2 == null)
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			IL_DB:
			if (!(this._htmlParser___2 != null))
			{
				goto IL_135;
			}
			IL_104:
			if (!this._htmlParser___2.IsEndParse)
			{
				this._current = null;
				this._PC = 3;
				return true;
			}
			this.__f__this.m_agreementText = this._htmlParser___2.ParsedString;
			UnityEngine.Object.Destroy(this._htmlParserGameObject___1);
			IL_135:
			if (this.__f__this.m_agreementText != null && this.__f__this.m_agreementText != string.Empty)
			{
				this._title___3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "gw_legal_caption");
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "AgreementLegal",
					anchor_path = "Camera/Anchor_5_MC",
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = this._title___3.text,
					message = this.__f__this.m_agreementText
				});
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const string GameSceneName = "s_playingstage";

	private const string MainMenuSceneName = "MainMenu";

	private HudProgressBar m_progressBar;

	private static bool s_first = true;

	private readonly string ANCHOR_PATH = "Camera/menu_Anim/MainMenuUI4/Anchor_5_MC";

	private readonly string VersionStr = "Ver:" + CurrentBundleVersion.version.ToString();

	private string m_nextSceneName;

	private SettingPartsPushNotice m_pushNotice;

	private SettingTakeoverInput m_takeoverInput;

	private bool m_isTakeoverLogin;

	private float m_timer;

	private bool m_isGetCountry;

	private static readonly float TAKEOVER_WAIT_TIME = 2f;

	private ResourceSceneLoader.ResourceInfo m_loadInfoForEvent = new ResourceSceneLoader.ResourceInfo(ResourceCategory.EVENT_RESOURCE, "EventResourceCommon", true, false, true, "EventResourceCommon", false);

	private TinyFsmBehavior m_fsm;

	private StageInfo m_stageInfo;

	private TitleDataLoader m_loader;

	private static bool m_isLogined;

	private bool m_isSessionValid;

	private bool m_isFirstTutorial;

	private static bool m_isReturnFirstTutorial;

	private GameObject m_loginLabel;

	private GameObject m_touchScreenObject;

	private GameObject m_startButton;

	private GameObject m_movetButton;

	private GameObject m_cacheButton;

	private GameObject m_sceneLoader;

	private UILabel m_userIdLabel;

	private SettingPartsSnsLogin m_snsLogin;

	private HudLoadingWindow m_loadingWindow;

	private HudNetworkConnect m_loadingConnect;

	private GameModeTitle.AtomDataInfo m_atomInfo;

	private readonly int ACHIEVEMENT_HIDE_COUNT = 3;

	private bool m_isSendNoahId;

	private bool m_isSkip;

	private int m_subState;

	private bool m_initUser;

	private string m_agreementText;

	private GameModeTitle.RedStarExchangeType m_exchangeType;

	public static bool Logined
	{
		get
		{
			return GameModeTitle.m_isLogined;
		}
		set
		{
			GameModeTitle.m_isLogined = value;
		}
	}

	public static bool FirstTutorialReturned
	{
		get
		{
			return GameModeTitle.m_isReturnFirstTutorial;
		}
		set
		{
			GameModeTitle.m_isReturnFirstTutorial = value;
		}
	}

	private void Awake()
	{
		Application.targetFrameRate = 60;
		base.gameObject.AddComponent<HudNetworkConnect>();
	}

	private void Start()
	{
		HudUtility.SetInvalidNGUIMitiTouch();
		SystemSettings.ChangeQualityLevelBySaveData();
		SystemData systemSaveData = SystemSaveManager.GetSystemSaveData();
		bool flag = false;
		if (systemSaveData != null)
		{
			flag = systemSaveData.highTexture;
		}
		if (flag)
		{
			Caching.maximumAvailableDiskSpace = 524288000L;
		}
		else
		{
			Caching.maximumAvailableDiskSpace = 314572800L;
		}
		Caching.expirationDelay = 2592000;
		GameObject gameObject = GameObject.Find("StageInfo");
		if (gameObject == null)
		{
			gameObject = new GameObject();
			if (gameObject != null)
			{
				gameObject.name = "StageInfo";
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				gameObject.AddComponent(typeof(StageInfo));
			}
		}
		if (gameObject != null)
		{
			this.m_stageInfo = gameObject.GetComponent<StageInfo>();
			if (this.m_stageInfo == null)
			{
				return;
			}
		}
		SoundManager.AddTitleCueSheet();
		GameObject gameObject2 = GameObject.Find("UI Root (2D)");
		if (gameObject2 == null)
		{
			return;
		}
		this.m_touchScreenObject = GameObjectUtil.FindChildGameObject(gameObject2, "Lbl_mainmenu");
		if (this.m_touchScreenObject != null)
		{
			this.m_touchScreenObject.SetActive(false);
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject2, "sega_logo");
		if (gameObject3 != null)
		{
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "sega_logo_img");
			if (uISprite != null)
			{
				if (Env.language == Env.Language.JAPANESE || Env.language == Env.Language.KOREAN || Env.language == Env.Language.CHINESE_ZH || Env.language == Env.Language.CHINESE_ZHJ)
				{
					uISprite.spriteName = "ui_title_img_segalogo_jp";
				}
				else
				{
					uISprite.spriteName = "ui_title_img_segalogo_en";
				}
			}
			gameObject3.SetActive(GameModeTitle.s_first);
		}
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm == null)
		{
			return;
		}
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		RouletteManager.Remove();
		if (GameModeTitle.m_isReturnFirstTutorial)
		{
			if (SystemSaveManager.Instance != null)
			{
				SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
				if (systemdata != null)
				{
					systemdata.SetFlagStatus(SystemData.FlagStatus.FIRST_LAUNCH_TUTORIAL_END, true);
					SystemSaveManager.Instance.SaveSystemData();
				}
			}
			GameObject gameObject4 = GameObject.Find("UI Root (2D)");
			if (gameObject4 != null)
			{
				GameObject gameObject5 = GameObjectUtil.FindChildGameObject(gameObject4, "img_blinder");
				if (gameObject5 != null)
				{
					gameObject5.SetActive(true);
				}
			}
			this.NoahBannerDisplay(false);
			this.m_nextSceneName = "MainMenu";
			description.initState = new TinyFsmState(new EventFunction(this.StateSnsInitialize));
		}
		else if (GameModeTitle.m_isLogined)
		{
			description.initState = new TinyFsmState(new EventFunction(this.StateFadeOut));
		}
		else
		{
			description.initState = new TinyFsmState(new EventFunction(this.StateLoadFont));
		}
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_ver");
		if (uILabel == null)
		{
			return;
		}
		string str = string.Empty;
		if (systemSaveData != null && systemSaveData.highTexture)
		{
			str = "t";
		}
		string text = this.VersionStr + str + NetBaseUtil.ServerTypeString;
		if (!string.IsNullOrEmpty(NetBaseUtil.ServerTypeString))
		{
			text = "[ff0000]" + text;
		}
		uILabel.text = text;
		this.m_loginLabel = GameObjectUtil.FindChildGameObject(gameObject2, "Lbl_login");
		if (this.m_loginLabel != null)
		{
			this.m_loginLabel.SetActive(false);
		}
		this.m_loadingWindow = GameObjectUtil.FindChildGameObjectComponent<HudLoadingWindow>(gameObject2, "DownloadWindow");
		this.m_userIdLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_userid");
		if (this.m_userIdLabel != null)
		{
			if (TitleUtil.IsExistSaveDataGameId())
			{
				this.m_initUser = true;
				this.m_userIdLabel.gameObject.SetActive(true);
				this.m_userIdLabel.text = this.GetViewUserID();
				GameObject gameObject6 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_policy");
				if (gameObject6 != null)
				{
					gameObject6.SetActive(false);
				}
				GameObject gameObject7 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_mainmenu");
				if (gameObject7 != null)
				{
					gameObject7.SetActive(true);
				}
				this.m_startButton = gameObject7;
			}
			else
			{
				this.m_initUser = false;
				this.m_userIdLabel.gameObject.SetActive(false);
				GameObject gameObject8 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_policy");
				if (gameObject8 != null)
				{
					gameObject8.SetActive(true);
				}
				GameObject gameObject9 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_mainmenu");
				if (gameObject9 != null)
				{
					gameObject9.SetActive(false);
				}
				this.m_startButton = gameObject8;
			}
			BackKeyManager.AddEventCallBack(base.gameObject);
			BackKeyManager.StartScene();
			BackKeyManager.InvalidFlag = true;
		}
		this.m_snsLogin = base.gameObject.AddComponent<SettingPartsSnsLogin>();
		this.m_snsLogin.SetCancelWindowUseFlag(false);
		this.m_snsLogin.Setup("Camera/TitleScreen/Anchor_5_MC");
		global::Debug.Log("GetLang:" + Language.GetLocalLanguage());
		GameObject gameObject10 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_fb");
		if (gameObject10 != null)
		{
			gameObject10.SetActive(false);
		}
		if (this.m_userIdLabel != null)
		{
			this.m_userIdLabel.gameObject.SetActive(true);
			this.m_userIdLabel.text = this.GetViewUserID();
		}
		CameraFade.StartAlphaFade(Color.black, true, 2f, 0f, new Action(this.FinishFadeCallback));
		this.m_movetButton = null;
		this.m_cacheButton = null;
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(gameObject2, "Btn_move");
		if (uIButtonMessage != null)
		{
			this.m_movetButton = uIButtonMessage.gameObject;
			this.m_movetButton.SetActive(false);
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnClickMigrationButton";
		}
		UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(gameObject2, "Btn_cache");
		if (uIButtonMessage2 != null)
		{
			this.m_cacheButton = uIButtonMessage2.gameObject;
			this.m_cacheButton.SetActive(false);
			uIButtonMessage2.target = base.gameObject;
			uIButtonMessage2.functionName = "OnClickCacheClearButton";
		}
		GameObject gameObject11 = GameObjectUtil.FindChildGameObject(gameObject2, "logo_jp");
		if (gameObject11 != null)
		{
			if (Env.language == Env.Language.JAPANESE)
			{
				gameObject11.SetActive(true);
			}
			else
			{
				gameObject11.SetActive(false);
			}
		}
		this.m_loadingConnect = base.gameObject.GetComponent<HudNetworkConnect>();
		if (!GameModeTitle.m_isReturnFirstTutorial)
		{
			GameObject gameObject12 = GameObjectUtil.FindChildGameObject(gameObject2, "img_titlelogo");
			if (gameObject12 != null)
			{
				gameObject12.SetActive(false);
			}
		}
		this.m_progressBar = GameObjectUtil.FindChildGameObjectComponent<HudProgressBar>(gameObject2, "Pgb_loading");
		if (this.m_progressBar != null)
		{
			this.m_progressBar.SetUp(25);
		}
		GameObject gameObject13 = GameObject.Find("UI Root 2(2D)");
		if (gameObject13 != null)
		{
			GameObject gameObject14 = GameObjectUtil.FindChildGameObject(gameObject13, "col_noah");
			if (gameObject14 != null)
			{
				Vector2 vector = new Vector2(960f, 640f);
				Resolution currentResolution = Screen.currentResolution;
				Vector2 vector2 = new Vector2((float)(currentResolution.width / Screen.width), (float)(currentResolution.height / Screen.height));
				vector2.x *= 2f;
				vector2.y *= 2f;
				BoxCollider component = gameObject14.GetComponent<BoxCollider>();
				if (component != null)
				{
					Vector2 vector3 = component.size;
					component.size = new Vector3(vector3.x * vector2.x, vector3.y * vector2.y, 1f);
					global::Debug.Log("NoahColliderSize = " + component.size.x.ToString() + " , " + component.size.y.ToString());
				}
				UISprite component2 = gameObject14.GetComponent<UISprite>();
				if (component2 != null)
				{
					Vector2 vector4 = new Vector2((float)component2.width, (float)component2.height);
					component2.width = (int)(vector4.x * vector2.x);
					component2.height = (int)(vector4.y * vector2.y);
					global::Debug.Log("NoahSpriteSize = " + component2.width.ToString() + " , " + component2.height.ToString());
				}
			}
		}
	}

	private TinyFsmState StateLoadFont(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (FontManager.Instance != null && FontManager.Instance.IsNecessaryLoadFont())
			{
				FontManager.Instance.LoadResourceData();
				FontManager.Instance.ReplaceFont();
			}
			return TinyFsmState.End();
		case 4:
			if (GameModeTitle.s_first)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeOut)));
			}
			else
			{
				this.SegaLogoAnimationSkip();
				if (SystemSaveManager.Instance != null && SystemSaveManager.Instance.ErrorOnStart())
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSaveDataError)));
					return TinyFsmState.End();
				}
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsInitialize)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateFadeOut(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_touchScreenObject != null)
			{
				this.m_touchScreenObject.SetActive(true);
				TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_mainmenu");
				UILabel component = this.m_touchScreenObject.GetComponent<UILabel>();
				if (component != null)
				{
					component.text = text.text;
				}
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_touchScreenObject, "Lbl_mainmenu_sh");
				if (uILabel != null)
				{
					uILabel.text = text.text;
				}
			}
			return TinyFsmState.End();
		case 2:
		case 3:
		{
			IL_25:
			if (signal != 109)
			{
				return TinyFsmState.End();
			}
			GameObject parent = GameObject.Find("UI Root (2D)");
			GameObject gameObject = GameObjectUtil.FindChildGameObject(parent, "img_titlelogo");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
			Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(parent, "TitleScreen");
			if (animation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_title_intro_logo_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.SegaLogoAnimationFinishCallback), false);
			}
			GameModeTitle.s_first = false;
			if (SystemSaveManager.Instance != null && SystemSaveManager.Instance.ErrorOnStart())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSaveDataError)));
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsInitialize)));
			return TinyFsmState.End();
		}
		case 4:
			return TinyFsmState.End();
		}
		goto IL_25;
	}

	private TinyFsmState StateSaveDataError(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 1:
			this.CreateSaveErrorWindow(false);
			this.m_subState = 0;
			BackKeyManager.InvalidFlag = false;
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 109)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsInitialize)));
			return TinyFsmState.End();
		case 4:
		{
			int subState = this.m_subState;
			if (subState != 0)
			{
				if (subState == 1)
				{
					if (GeneralWindow.IsButtonPressed)
					{
						GeneralWindow.Close();
						this.CreateSaveErrorWindow(false);
						this.m_subState = 0;
					}
				}
			}
			else if (GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				if (SystemSaveManager.Instance.SaveForStartingError())
				{
					if (GameModeTitle.m_isLogined)
					{
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsInitialize)));
					}
					else
					{
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsInitialize)));
					}
				}
				else
				{
					this.CreateSaveErrorWindow(true);
					this.m_subState = 1;
				}
			}
			return TinyFsmState.End();
		}
		}
		goto IL_23;
	}

	private void CreateSaveErrorWindow(bool error)
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", (!error) ? "savedata_recreate" : "savedata_error").text,
			anchor_path = "Camera/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.Ok
		});
	}

	private TinyFsmState StateSnsInitialize(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface != null)
			{
				socialInterface.Initialize(base.gameObject);
			}
			return TinyFsmState.End();
		}
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateNoahConnect)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateNoahConnect(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.TryConnectNoah(false);
			return TinyFsmState.End();
		case 4:
			if (GameModeTitle.m_isReturnFirstTutorial)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGameServerPreLogin)));
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitTouchScreen)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitTouchScreen(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 1:
			BackKeyManager.InvalidFlag = false;
			if (this.m_startButton != null)
			{
				this.m_startButton.SetActive(true);
			}
			if (this.m_movetButton != null)
			{
				this.m_movetButton.SetActive(true);
			}
			if (this.m_cacheButton != null)
			{
				this.m_cacheButton.SetActive(true);
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_25:
			if (signal == 100)
			{
				this.NoahBannerDisplay(false);
				if (this.m_startButton != null)
				{
					this.m_startButton.SetActive(false);
				}
				if (this.m_movetButton != null)
				{
					this.m_movetButton.SetActive(false);
				}
				if (this.m_cacheButton != null)
				{
					this.m_cacheButton.SetActive(false);
				}
				if (GameModeTitle.m_isLogined)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeIn)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGameServerPreLogin)));
				}
				GameObject parent = GameObject.Find("UI Root (2D)");
				UIObjectContainer uIObjectContainer = GameObjectUtil.FindChildGameObjectComponent<UIObjectContainer>(parent, "TitleScreen");
				if (uIObjectContainer != null)
				{
					GameObject[] objects = uIObjectContainer.Objects;
					if (objects != null)
					{
						GameObject[] array = objects;
						for (int i = 0; i < array.Length; i++)
						{
							GameObject gameObject = array[i];
							if (gameObject != null)
							{
								gameObject.SetActive(false);
							}
						}
					}
				}
				return TinyFsmState.End();
			}
			if (signal != 111)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateTakeoverFunction)));
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("quit_app"))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						Application.Quit();
					}
					else if (GeneralWindow.IsNoButtonPressed)
					{
						this.NoahBannerDisplay(true);
					}
					GeneralWindow.Close();
					this.SetUIEffect(true);
				}
			}
			else if (GeneralWindow.IsCreated("cache_clear"))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						GeneralUtil.CleanAllCache();
						GeneralWindow.Close();
						GeneralWindow.Create(new GeneralWindow.CInfo
						{
							name = "cache_clear_end",
							caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_confirmation_bar"),
							message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_confirmation_title"),
							anchor_path = "Camera/Anchor_5_MC",
							buttonType = GeneralWindow.ButtonType.Ok
						});
					}
					else
					{
						GeneralWindow.Close();
						this.NoahBannerDisplay(true);
					}
				}
			}
			else if (GeneralWindow.IsCreated("cache_clear_end") && GeneralWindow.IsButtonPressed)
			{
				this.NoahBannerDisplay(true);
				GeneralWindow.Close();
			}
			return TinyFsmState.End();
		}
		goto IL_25;
	}

	private TinyFsmState StateTakeoverFunction(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 1:
			BackKeyManager.InvalidFlag = false;
			this.m_subState = 0;
			this.CreateTakeoverCautionWindow();
			this.NoahBannerDisplay(false);
			return TinyFsmState.End();
		case 4:
		{
			int subState = this.m_subState;
			if (subState != 0)
			{
				if (subState == 1)
				{
					if (this.m_takeoverInput != null && this.m_takeoverInput.IsEndPlay())
					{
						if (this.m_takeoverInput.IsDicide)
						{
							string inputIdText = this.m_takeoverInput.InputIdText;
							string inputPassText = this.m_takeoverInput.InputPassText;
							global::Debug.Log("Input Finished! Input ID is " + inputIdText);
							global::Debug.Log("Input Finished! Input PASS is " + inputPassText);
							if (inputIdText.Length == 0 || inputPassText.Length == 0)
							{
								this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateTakeoverError)));
								this.m_subState = 2;
							}
							else
							{
								this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateTakeoverExecute)));
								this.m_subState = 2;
							}
						}
						if (this.m_takeoverInput.IsCanceled)
						{
							this.m_subState = 2;
							this.BacktoTouchScreenFromTakeover();
							this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitTouchScreen)));
						}
						this.SetUIEffect(true);
					}
				}
			}
			else if (GeneralWindow.IsCreated("TakeoverCaution") && GeneralWindow.IsButtonPressed)
			{
				bool flag = false;
				if (GeneralWindow.IsYesButtonPressed)
				{
					this.m_subState = 1;
					if (this.m_takeoverInput == null)
					{
						this.m_takeoverInput = base.gameObject.AddComponent<SettingTakeoverInput>();
						this.m_takeoverInput.Setup(this.ANCHOR_PATH);
					}
					if (this.m_takeoverInput != null)
					{
						this.m_takeoverInput.PlayStart();
					}
					flag = true;
				}
				else if (GeneralWindow.IsNoButtonPressed)
				{
					this.BacktoTouchScreenFromTakeover();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitTouchScreen)));
				}
				GeneralWindow.Close();
				if (flag)
				{
					this.SetUIEffect(false);
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateTakeoverError(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 1:
			BackKeyManager.InvalidFlag = false;
			this.CreateTakeoverErrorWindow();
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("TakeoverError") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.BacktoTouchScreenFromTakeover();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitTouchScreen)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateTakeoverExecute(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 1:
		{
			BackKeyManager.InvalidFlag = false;
			this.m_isTakeoverLogin = false;
			string inputIdText = this.m_takeoverInput.InputIdText;
			string inputPassText = this.m_takeoverInput.InputPassText;
			ServerInterface serverInterface = GameObjectUtil.FindGameObjectComponent<ServerInterface>("ServerInterface");
			if (serverInterface != null)
			{
				string migrationPassword = NetUtil.CalcMD5String(inputPassText);
				serverInterface.RequestServerMigration(inputIdText, migrationPassword, base.gameObject);
			}
			this.CreateTakeoverExecWindow();
			this.m_timer = 0f;
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_25:
			if (signal != 112)
			{
				if (signal == 113)
				{
					if (GeneralWindow.IsCreated("TakeoverExec"))
					{
						GeneralWindow.Close();
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateTakeoverError)));
					}
				}
			}
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("TakeoverExec"))
			{
				this.m_timer += Time.deltaTime;
				if (this.m_timer >= GameModeTitle.TAKEOVER_WAIT_TIME)
				{
					this.m_timer = GameModeTitle.TAKEOVER_WAIT_TIME;
				}
				if (this.m_isTakeoverLogin && this.m_timer >= GameModeTitle.TAKEOVER_WAIT_TIME)
				{
					global::Debug.Log("Takeover Finished! Result Success! ");
					GeneralWindow.Close();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateTakeoverFinished)));
				}
			}
			return TinyFsmState.End();
		}
		goto IL_25;
	}

	private TinyFsmState StateTakeoverFinished(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 1:
			BackKeyManager.InvalidFlag = false;
			this.CreateTakeoverFinishedWindow();
			global::Debug.Log("Takeover Finished!");
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsCreated("TakeoverFinished") && GeneralWindow.IsButtonPressed)
			{
				this.OnMsgGotoHead();
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void CreateTakeoverCautionWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "TakeoverCaution",
			buttonType = GeneralWindow.ButtonType.YesNo,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_text").text
		});
	}

	private void CreateTakeoverErrorWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "TakeoverError",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_error_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_error_text").text
		});
	}

	private void CreateTakeoverExecWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "TakeoverExec",
			buttonType = GeneralWindow.ButtonType.None,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_exec_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_exec_text").text
		});
	}

	private void CreateTakeoverFinishedWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "TakeoverFinished",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_finished_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "takeover_finished_text").text
		});
	}

	private void BacktoTouchScreenFromTakeover()
	{
		this.NoahBannerDisplay(true);
	}

	private TinyFsmState StateGamePushNotification(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			BackKeyManager.InvalidFlag = true;
			return TinyFsmState.End();
		case 1:
			BackKeyManager.InvalidFlag = false;
			this.m_pushNotice = base.gameObject.AddComponent<SettingPartsPushNotice>();
			this.m_pushNotice.Setup(this.ANCHOR_PATH);
			this.m_pushNotice.PlayStart();
			this.m_pushNotice.SetCloseButtonEnabled(false);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGameServerPreLogin)));
			return TinyFsmState.End();
		case 4:
			if (this.m_pushNotice != null && this.m_pushNotice.IsEndPlay())
			{
				if (this.m_pushNotice.IsOverwrite && SystemSaveManager.Instance != null)
				{
					SystemSaveManager.Instance.SaveSystemData();
				}
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGameServerPreLogin)));
				global::Debug.Log("m_fsm.ChangeState(new TinyFsmState(this.StateGameServerPreLogin));");
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateGameServerPreLogin(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			ServerSessionWatcher serverSessionWatcher = GameObjectUtil.FindGameObjectComponent<ServerSessionWatcher>("NetMonitor");
			if (serverSessionWatcher != null)
			{
				this.m_isSessionValid = false;
				serverSessionWatcher.ValidateSession(ServerSessionWatcher.ValidateType.PRELOGIN, new ServerSessionWatcher.ValidateSessionEndCallback(this.ValidateSessionCallback));
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_isSessionValid)
			{
				global::Debug.Log("GameModeTitle.StateGameServerPreLogin:Finished");
				if (this.m_userIdLabel != null)
				{
					this.m_userIdLabel.gameObject.SetActive(true);
					this.m_userIdLabel.text = this.GetViewUserID();
				}
				bool flag = true;
				if (SystemSaveManager.Instance != null)
				{
					string countryCode = SystemSaveManager.GetCountryCode();
					if (string.IsNullOrEmpty(countryCode))
					{
						flag = false;
					}
				}
				if (flag)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAssetBundleInitialize)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetCountryCodeRetry)));
					global::Debug.Log("GameModeTitle.StateGameServerPreLogin:LostCountryCode!!");
				}
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGetCountryCodeRetry(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_isGetCountry = false;
			ServerInterface serverInterface = GameObjectUtil.FindGameObjectComponent<ServerInterface>("ServerInterface");
			if (serverInterface != null)
			{
				serverInterface.RequestServerGetCountry(base.gameObject);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_isGetCountry)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAssetBundleInitialize)));
				global::Debug.Log("GameModeTitle.StateGetCountryCodeRetry:Finished");
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAssetBundleInitialize(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			Screen.sleepTimeout = -2;
			return TinyFsmState.End();
		case 1:
			NetBaseUtil.SetAssetServerURL();
			Screen.sleepTimeout = -1;
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
			return TinyFsmState.End();
		case 4:
			if (!Env.useAssetBundle || AssetBundleLoader.Instance.IsEnableDownlad())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateStreamingLoaderInitialize)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateStreamingLoaderInitialize(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			Screen.sleepTimeout = -2;
			return TinyFsmState.End();
		case 1:
			Screen.sleepTimeout = -1;
			if (StreamingDataLoader.Instance == null)
			{
				StreamingDataKeyRetryProcess process = new StreamingDataKeyRetryProcess(base.gameObject, this);
				NetMonitor.Instance.StartMonitor(process, 0f, HudNetworkConnect.DisplayType.ALL);
				StreamingDataLoader.Create();
				StreamingDataLoader.Instance.Initialize(base.gameObject);
			}
			return TinyFsmState.End();
		case 4:
			if (StreamingDataLoader.Instance.IsEnableDownlad())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckExistDownloadData)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCheckExistDownloadData(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_loader == null)
			{
				GameObject gameObject = new GameObject();
				this.m_loader = gameObject.AddComponent<TitleDataLoader>();
				if (StreamingDataLoader.Instance != null)
				{
					if (SystemSaveManager.Instance != null)
					{
						SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
						if (systemdata != null && !systemdata.IsFlagStatus(SystemData.FlagStatus.FIRST_LAUNCH_TUTORIAL_END) && !GameModeTitle.m_isReturnFirstTutorial)
						{
							if (systemdata.IsFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_1))
							{
								systemdata.SetFlagStatus(SystemData.FlagStatus.FIRST_LAUNCH_TUTORIAL_END, true);
								SystemSaveManager.Instance.SaveSystemData();
							}
							else
							{
								this.m_isFirstTutorial = true;
							}
						}
					}
					List<string> list = new List<string>();
					if (this.m_isFirstTutorial)
					{
						this.m_loader.AddStreamingSoundData("BGM_z01.acb");
						this.m_loader.AddStreamingSoundData("BGM_z01_streamfiles.awb");
						this.m_loader.AddStreamingSoundData("BGM_jingle.acb");
						this.m_loader.AddStreamingSoundData("BGM_jingle_streamfiles.awb");
						this.m_loader.AddStreamingSoundData("se_runners.acb");
					}
					else
					{
						StreamingDataLoader.Instance.GetLoadList(ref list);
						foreach (string current in list)
						{
							bool flag = current.Contains("BGM_z");
							bool flag2 = current.Contains("BGM_boss");
							if (!flag && !flag2)
							{
								this.m_loader.AddStreamingSoundData(current);
							}
						}
					}
				}
				this.m_loader.Setup(this.m_isFirstTutorial);
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_loader.EndCheckExistingDownloadData)
			{
				if (this.m_loader.RequestedDownloadCount > 0)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAskDataDownload)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitDataLoad)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAskDataDownload(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
			if (this.m_isFirstTutorial)
			{
				info.caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_load_caption").text;
				info.message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_load_ask_FirstTutorial").text;
			}
			else if (GameModeTitle.m_isReturnFirstTutorial)
			{
				info.caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_load_caption").text;
				info.message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_load_ask_FirstTutorialReturn").text;
			}
			else
			{
				info.caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_load_caption").text;
				if (TitleUtil.initUser)
				{
					info.message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_load_ask").text;
				}
				else
				{
					info.message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_load_ask_2").text;
				}
			}
			info.anchor_path = "Camera/Anchor_5_MC";
			info.buttonType = GeneralWindow.ButtonType.YesNo;
			GeneralWindow.Create(info);
			return TinyFsmState.End();
		}
		case 4:
			if (GeneralWindow.IsYesButtonPressed)
			{
				if (GameModeTitle.m_isReturnFirstTutorial)
				{
					GameModeTitle.m_isReturnFirstTutorial = false;
				}
				GeneralWindow.Close();
				SoundManager.BgmPlay("bgm_sys_load", "BGM", false);
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitDataDownload)));
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				GeneralWindow.Close();
				GameModeTitle.m_isLogined = false;
				if (GameModeTitle.m_isReturnFirstTutorial)
				{
					GameModeTitle.m_isReturnFirstTutorial = false;
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadTitleResetScene)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitTouchScreen)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitDataDownload(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_loader != null)
			{
				UnityEngine.Object.Destroy(this.m_loader.gameObject);
				this.m_loader = null;
			}
			Screen.sleepTimeout = -2;
			return TinyFsmState.End();
		case 1:
			Screen.sleepTimeout = -1;
			if (this.m_loadingWindow != null)
			{
				this.m_loadingWindow.PlayStart();
			}
			if (this.m_loader != null)
			{
				this.m_loader.StartLoad();
			}
			this.SetUIEffect(false);
			return TinyFsmState.End();
		case 4:
			if (this.m_loader == null)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEndDataLoad)));
			}
			else if (this.m_loader.LoadEnd)
			{
				TextManager.NotLoadSetupCommonText();
				TextManager.NotLoadSetupChaoText();
				TextManager.NotLoadSetupEventText();
				MissionTable.LoadSetup();
				CharacterDataNameInfo.LoadSetup();
				StageAbilityManager.SetupAbilityDataTable();
				OverlapBonusTable overlapBonusTable = GameObjectUtil.FindGameObjectComponent<OverlapBonusTable>("OverlapBonusTable");
				if (overlapBonusTable != null)
				{
					overlapBonusTable.Setup();
				}
				this.SetUIEffect(true);
				if (this.m_loadingWindow != null)
				{
					this.m_loadingWindow.PlayEnd();
				}
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEndDataLoad)));
			}
			else if (this.m_loader != null && this.m_loadingWindow != null)
			{
				float num = (float)this.m_loader.RequestedLoadCount;
				float num2 = (float)this.m_loader.LoadEndCount;
				if (num == 0f)
				{
					num = 1f;
				}
				float loadingPercentage = num2 * 100f / num;
				this.m_loadingWindow.SetLoadingPercentage(loadingPercentage);
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitDataLoad(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_loader != null)
			{
				UnityEngine.Object.Destroy(this.m_loader.gameObject);
				this.m_loader = null;
			}
			Screen.sleepTimeout = -2;
			return TinyFsmState.End();
		case 1:
			Screen.sleepTimeout = -1;
			if (this.m_loadingConnect != null)
			{
				this.m_loadingConnect.Setup();
				this.m_loadingConnect.PlayStart(HudNetworkConnect.DisplayType.NO_BG);
			}
			if (this.m_loader != null)
			{
				this.m_loader.StartLoad();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_loader == null)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEndDataLoad)));
			}
			else if (this.m_loader.LoadEnd)
			{
				TextManager.NotLoadSetupCommonText();
				TextManager.NotLoadSetupChaoText();
				TextManager.NotLoadSetupEventText();
				MissionTable.LoadSetup();
				CharacterDataNameInfo.LoadSetup();
				StageAbilityManager.SetupAbilityDataTable();
				OverlapBonusTable overlapBonusTable = GameObjectUtil.FindGameObjectComponent<OverlapBonusTable>("OverlapBonusTable");
				if (overlapBonusTable != null)
				{
					overlapBonusTable.Setup();
				}
				if (this.m_loadingConnect != null)
				{
					this.m_loadingConnect.PlayEnd();
				}
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEndDataLoad)));
			}
			else if (this.m_loader != null && this.m_loadingWindow != null)
			{
				float num = (float)this.m_loader.RequestedLoadCount;
				float num2 = (float)this.m_loader.LoadEndCount;
				if (num == 0f)
				{
					num = 1f;
				}
				float loadingPercentage = num2 * 100f / num;
				this.m_loadingWindow.SetLoadingPercentage(loadingPercentage);
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEndDataLoad(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(0);
			}
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGameServerLogin)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGameServerLogin(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(1);
			}
			return TinyFsmState.End();
		case 1:
		{
			ServerSessionWatcher serverSessionWatcher = GameObjectUtil.FindGameObjectComponent<ServerSessionWatcher>("NetMonitor");
			if (serverSessionWatcher != null)
			{
				this.m_isSessionValid = false;
				serverSessionWatcher.ValidateSession(ServerSessionWatcher.ValidateType.LOGIN_OR_RELOGIN, new ServerSessionWatcher.ValidateSessionEndCallback(this.ValidateSessionCallback));
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_isSessionValid)
			{
				GameModeTitle.m_isLogined = true;
				global::Debug.Log("GameModeTitle.StateGameServerLogin:Finished");
				if (this.m_isFirstTutorial)
				{
					this.m_nextSceneName = "s_playingstage";
					this.SetFirstTutorialInfo();
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeIn)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetServerContinueParameter)));
				}
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGetServerContinueParameter(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(2);
			}
			return TinyFsmState.End();
		case 1:
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetVariousParameter(base.gameObject);
			}
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_23:
			if (signal != 102)
			{
				return TinyFsmState.End();
			}
			if (RegionManager.Instance.IsUseHardlightAds())
			{
				GameObject gameObject = GameObject.Find("HardlightAds");
				if (gameObject == null)
				{
					gameObject = new GameObject();
					if (gameObject != null)
					{
						gameObject.name = "HardlightAds";
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
						gameObject.AddComponent(typeof(HardlightAds));
					}
				}
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckAtom)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateCheckAtom(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(3);
			}
			this.m_atomInfo = null;
			return TinyFsmState.End();
		case 1:
			if (Binding.Instance != null)
			{
				string urlSchemeStr = Binding.Instance.GetUrlSchemeStr();
				Binding.Instance.ClearUrlSchemeStr();
				if (!string.IsNullOrEmpty(urlSchemeStr))
				{
					string empty = string.Empty;
					string empty2 = string.Empty;
					if (ServerAtomSerial.GetSerialFromScheme(urlSchemeStr, ref empty, ref empty2))
					{
						this.m_atomInfo = new GameModeTitle.AtomDataInfo();
						this.m_atomInfo.campain = empty;
						this.m_atomInfo.serial = empty2;
						GeneralWindow.Create(new GeneralWindow.CInfo
						{
							message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "atom_check"),
							caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "atom_check_caption"),
							anchor_path = "Camera/Anchor_5_MC",
							buttonType = GeneralWindow.ButtonType.Ok
						});
						this.m_subState = 0;
					}
					else
					{
						this.m_subState = 3;
					}
				}
				else
				{
					this.m_subState = 3;
				}
			}
			else
			{
				this.m_subState = 3;
			}
			return TinyFsmState.End();
		case 4:
			switch (this.m_subState)
			{
			case 0:
				if (GeneralWindow.IsButtonPressed)
				{
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
					loggedInServerInterface.RequestServerAtomSerial(this.m_atomInfo.campain, this.m_atomInfo.serial, new_user, base.gameObject);
					GeneralWindow.Close();
					this.m_subState = 1;
				}
				break;
			case 1:
				global::Debug.Log("Wait Server");
				return TinyFsmState.End();
			case 2:
				if (GeneralWindow.IsButtonPressed)
				{
					global::Debug.Log("EndText end:");
					GeneralWindow.Close();
					this.m_subState = 3;
				}
				return TinyFsmState.End();
			case 3:
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckNoLoginIncentive)));
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			TextManager.TextType type = TextManager.TextType.TEXTTYPE_FIXATION_TEXT;
			int num = iD;
			if (num != 61495)
			{
				if (num == 61517)
				{
					MsgServerConnctFailed msgServerConnctFailed = e.GetMessage as MsgServerConnctFailed;
					string cellID = "atom_invalid_serial";
					if (msgServerConnctFailed != null && msgServerConnctFailed.m_status == ServerInterface.StatusCode.UsedSerialCode)
					{
						cellID = "atom_used_serial";
					}
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						message = TextUtility.GetText(type, "Title", cellID),
						caption = TextUtility.GetText(type, "Title", "atom_failure_caption"),
						anchor_path = "Camera/Anchor_5_MC",
						buttonType = GeneralWindow.ButtonType.Ok
					});
					this.m_subState = 2;
				}
			}
			else
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					message = TextUtility.GetText(type, "Title", "atom_present_get"),
					caption = TextUtility.GetText(type, "Title", "atom_success_caption"),
					anchor_path = "Camera/Anchor_5_MC",
					buttonType = GeneralWindow.ButtonType.Ok
				});
				this.m_subState = 2;
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCheckNoLoginIncentive(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(4);
			}
			this.m_atomInfo = null;
			return TinyFsmState.End();
		case 1:
			if (PnoteNotification.CheckEnableGetNoLoginIncentive())
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					loggedInServerInterface.RequestServerGetFacebookIncentive(4, 1, base.gameObject);
					this.m_subState = 0;
				}
			}
			else
			{
				this.m_subState = 2;
			}
			return TinyFsmState.End();
		case 4:
			switch (this.m_subState)
			{
			case 0:
				global::Debug.Log("Wait Server");
				return TinyFsmState.End();
			case 1:
				if (GeneralWindow.IsButtonPressed)
				{
					global::Debug.Log("EndText end:");
					GeneralWindow.Close();
					this.m_subState = 3;
				}
				return TinyFsmState.End();
			case 2:
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsAdditionalData)));
				return TinyFsmState.End();
			default:
				return TinyFsmState.End();
			}
			break;
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num != 61490)
			{
				if (num == 61517)
				{
					this.m_subState = 2;
				}
			}
			else
			{
				this.m_subState = 2;
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSnsAdditionalData(TinyFsmEvent e)
	{
		int signal = e.Signal;
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
			SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface != null && socialInterface.IsLoggedIn)
			{
				socialInterface.RequestFriendRankingInfoSet(null, null, SettingPartsSnsAdditional.Mode.BACK_GROUND_LOAD);
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			SocialInterface socialInterface2 = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface2 != null && socialInterface2.IsLoggedIn)
			{
				if (socialInterface2.IsEnableFriendInfo)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StatePushNoticeCheck)));
				}
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StatePushNoticeCheck)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StatePushNoticeCheck(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (SystemSaveManager.Instance != null)
			{
				SystemData systemSaveData = SystemSaveManager.GetSystemSaveData();
				if (systemSaveData != null)
				{
					if (systemSaveData.pushNotice)
					{
						PnoteNotification.RequestRegister();
					}
					else
					{
						PnoteNotification.RequestUnregister();
					}
				}
			}
			return TinyFsmState.End();
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitToGetMenuData)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitToGetMenuData(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(16);
			}
			return TinyFsmState.End();
		case 1:
		{
			NoahHandler.Instance.SetGUID(TitleUtil.GetSystemSaveDataGameId());
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerRetrievePlayerState(base.gameObject);
			}
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_23:
			if (signal != 103)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAchievementLogin)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateAchievementLogin(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(17);
			}
			return TinyFsmState.End();
		case 1:
		{
			AchievementManager.Setup();
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
				if (systemdata != null && systemdata.achievementCancelCount >= this.ACHIEVEMENT_HIDE_COUNT)
				{
					AchievementManager.RequestSkipAuthenticate();
					return TinyFsmState.End();
				}
			}
			AchievementManager.RequestUpdate();
			return TinyFsmState.End();
		}
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetCostList)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGetCostList(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(19);
			}
			return TinyFsmState.End();
		case 1:
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetCostList(base.gameObject);
			}
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_23:
			if (signal != 106)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetEventList)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateGetEventList(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(20);
			}
			return TinyFsmState.End();
		case 1:
		{
			this.m_isSkip = true;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetEventList(base.gameObject);
				this.m_isSkip = false;
			}
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_23:
			if (signal != 108)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetMileageMap)));
			return TinyFsmState.End();
		case 4:
			if (this.m_isSkip)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetMileageMap)));
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateGetMileageMap(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(21);
			}
			return TinyFsmState.End();
		case 1:
		{
			this.m_isSkip = true;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				List<string> list = new List<string>();
				if (list != null)
				{
					SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
					if (socialInterface != null)
					{
						list = SocialInterface.GetGameIdList(socialInterface.FriendList);
					}
				}
				if (list != null && list.Count > 0)
				{
					loggedInServerInterface.RequestServerGetMileageData(list.ToArray(), base.gameObject);
					this.m_isSkip = false;
				}
				else
				{
					loggedInServerInterface.RequestServerGetMileageData(null, base.gameObject);
					this.m_isSkip = false;
				}
			}
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_23:
			if (signal != 107)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIapInitialize)));
			return TinyFsmState.End();
		case 4:
			if (this.m_isSkip)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIapInitialize)));
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateIapInitialize(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(22);
			}
			return TinyFsmState.End();
		case 1:
			this.IapInitializeEndCallback(NativeObserver.IAPResult.ProductsRequestCompleted);
			return TinyFsmState.End();
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadEventResource)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadEventResource(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(23);
			}
			return TinyFsmState.End();
		case 1:
			if (EventManager.Instance != null && EventManager.Instance.IsInEvent())
			{
				this.m_sceneLoader = new GameObject("SceneLoader");
				if (this.m_sceneLoader != null)
				{
					ResourceSceneLoader resourceSceneLoader = this.m_sceneLoader.AddComponent<ResourceSceneLoader>();
					this.m_loadInfoForEvent.m_scenename = "EventResourceCommon" + EventManager.GetResourceName();
					resourceSceneLoader.AddLoadAndResourceManager(this.m_loadInfoForEvent);
				}
				AtlasManager.Instance.StartLoadAtlasForTitle();
			}
			return TinyFsmState.End();
		case 4:
		{
			bool flag = true;
			if (this.m_sceneLoader != null)
			{
				if (this.m_sceneLoader.GetComponent<ResourceSceneLoader>().Loaded && AtlasManager.Instance.IsLoadAtlas())
				{
					flag = true;
					UnityEngine.Object.Destroy(this.m_sceneLoader);
					this.m_sceneLoader = null;
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadingUIData)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadingUIData(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(24);
			}
			return TinyFsmState.End();
		case 1:
			ChaoTextureManager.Instance.RequestTitleLoadChaoTexture();
			return TinyFsmState.End();
		case 4:
			if (ChaoTextureManager.Instance.IsLoaded())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateJailBreakCheck)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateJailBreakCheck(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			CPlusPlusLink instance = CPlusPlusLink.Instance;
			if (instance != null)
			{
				global::Debug.Log("GameModeTitle.StateJailBreakCheck");
				instance.BootGameCheatCheck();
			}
			return TinyFsmState.End();
		}
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeIn)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateFadeIn(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			CameraFade.StartAlphaFade(Color.black, false, 1f, 0f, new Action(this.FinishFadeCallback));
			SoundManager.BgmFadeOut(0.5f);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 109)
			{
				return TinyFsmState.End();
			}
			CameraFade.StartAlphaFade(Color.black, false, -1f);
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadLevel)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateLoadLevel(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			BackKeyManager.EndScene();
			this.m_stageInfo.FromTitle = true;
			Resources.UnloadUnusedAssets();
			GC.Collect();
			TimeProfiler.StartCountTime("Title-NextScene");
			NoahHandler.Instance.SetCallback(null);
			NativeObserver.Instance.CheckCurrentTransaction();
			UnityEngine.SceneManagement.SceneManager.LoadScene(this.m_nextSceneName);
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadTitleResetScene(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			CameraFade.StartAlphaFade(Color.black, false, 2f, 0f, new Action(this.FinishFadeCallback));
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 109)
			{
				return TinyFsmState.End();
			}
			UnityEngine.SceneManagement.SceneManager.LoadScene("s_title_reset");
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private void TrySnsLoginButtonActive()
	{
	}

	private void TryConnectNoah(bool isResume)
	{
		bool flag = false;
		if (this.m_initUser)
		{
			RegionManager instance = RegionManager.Instance;
			if (instance != null && instance.IsJapan())
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			if (!isResume)
			{
				NoahHandler.Instance.SetCallback(base.gameObject);
			}
			else
			{
				NoahHandler.Instance.SetCallback(null);
			}
			Noah.Instance.Connect(NoahHandler.consumer_key, NoahHandler.secret_key, NoahHandler.action_id);
		}
	}

	public void OnApplicationPause(bool flag)
	{
		if (!flag)
		{
			this.TryConnectNoah(true);
		}
	}

	private void FinishFadeCallback()
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(109);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void InitEndCallback(MsgSocialNormalResponse msg)
	{
		global::Debug.Log("InitEndCallback");
		this.TrySnsLoginButtonActive();
	}

	private void ValidateSessionCallback(bool isSuccess)
	{
		global::Debug.Log("GameModeTitle.ValidateSessionCallback");
		this.m_isSessionValid = true;
	}

	private void ServerGetVersion_Succeeded(MsgGetVersionSucceed msg)
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(101);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ServerGetVariousParameter_Succeeded(MsgGetVariousParameterSucceed msg)
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(102);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ServerRetrievePlayerState_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.m_progressBar.SetState(6);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetCharacterState(base.gameObject);
		}
	}

	private void ServerGetCharacterState_Succeeded(MsgGetCharacterStateSucceed msg)
	{
		this.m_progressBar.SetState(7);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetChaoState(base.gameObject);
		}
	}

	private void ServerGetChaoState_Succeeded(MsgGetChaoStateSucceed msg)
	{
		this.m_progressBar.SetState(8);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetWheelOptions(base.gameObject);
		}
	}

	private void ServerGetWheelOptions_Succeeded(MsgGetWheelOptionsSucceed msg)
	{
		this.m_progressBar.SetState(9);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetDailyMissionData(base.gameObject);
		}
	}

	private void ServerGetDailyMissionData_Succeeded(MsgGetDailyMissionDataSucceed msg)
	{
		this.m_progressBar.SetState(10);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetMessageList(base.gameObject);
		}
	}

	private void ServerGetMessageList_Succeeded(MsgGetMessageListSucceed msg)
	{
		this.m_progressBar.SetState(11);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			this.m_exchangeType = GameModeTitle.RedStarExchangeType.RSRING;
			loggedInServerInterface.RequestServerGetRedStarExchangeList((int)this.m_exchangeType, base.gameObject);
		}
	}

	private void ServerGetRedStarExchangeList_Succeeded(MsgGetRedStarExchangeListSucceed msg)
	{
		switch (this.m_exchangeType)
		{
		case GameModeTitle.RedStarExchangeType.RSRING:
			this.m_progressBar.SetState(12);
			break;
		case GameModeTitle.RedStarExchangeType.RING:
			this.m_progressBar.SetState(13);
			break;
		case GameModeTitle.RedStarExchangeType.CHALLENGE:
			this.m_progressBar.SetState(14);
			break;
		case GameModeTitle.RedStarExchangeType.RAIDBOSS_ENERGY:
			this.m_progressBar.SetState(15);
			break;
		}
		bool flag = false;
		this.m_exchangeType++;
		if (this.m_exchangeType >= GameModeTitle.RedStarExchangeType.Count)
		{
			flag = true;
		}
		if (flag)
		{
			ServerTickerInfo tickerInfo = ServerInterface.TickerInfo;
			tickerInfo.Init(0);
			if (this.m_fsm != null)
			{
				TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(103);
				this.m_fsm.Dispatch(signal);
			}
			ServerLoginState loginState = ServerInterface.LoginState;
			if (loginState != null)
			{
				loginState.IsChangeDataVersion = false;
				loginState.IsChangeAssetsVersion = false;
			}
		}
		else
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				if (this.m_exchangeType == GameModeTitle.RedStarExchangeType.RAIDBOSS_ENERGY)
				{
					loggedInServerInterface.RequestServerGetRedStarExchangeList(4, base.gameObject);
				}
				else
				{
					loggedInServerInterface.RequestServerGetRedStarExchangeList((int)this.m_exchangeType, base.gameObject);
				}
			}
		}
	}

	private void ServerGetLeaderboardEntries_Succeeded(MsgGetLeaderboardEntriesSucceed msg)
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(104);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ServerGetLeagueData_Succeeded(MsgGetLeagueDataSucceed msg)
	{
		RankingLeagueTable.SetupRankingLeagueTable();
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(105);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void GetCostList_Succeeded(MsgGetCostListSucceed msg)
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(106);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ServerAtomSerial_Succeeded(MsgSendAtomSerialSucceed msg)
	{
		this.DispatchMessage(msg);
	}

	private void ServerAtomSerial_Failed(MsgServerConnctFailed msg)
	{
		this.DispatchMessage(msg);
	}

	private void ServerGetFacebookIncentive_Succeeded(MsgGetNormalIncentiveSucceed msg)
	{
		this.DispatchMessage(msg);
	}

	private void ServerGetFacebookIncentive_Failed(MsgServerConnctFailed msg)
	{
		this.DispatchMessage(msg);
	}

	private void ServerMigration_Succeeded(MsgLoginSucceed msg)
	{
		this.m_isTakeoverLogin = true;
		if (SystemSaveManager.Instance != null)
		{
			SystemSaveManager.Instance.DeleteSystemFile();
		}
		if (InformationSaveManager.Instance != null)
		{
			InformationSaveManager.Instance.DeleteInformationFile();
		}
		if (SystemSaveManager.Instance != null)
		{
			SystemSaveManager.SetGameID(msg.m_userId);
			SystemSaveManager.SetGamePassword(msg.m_password);
			SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
			if (systemdata != null)
			{
				systemdata.SetFlagStatus(SystemData.FlagStatus.FIRST_LAUNCH_TUTORIAL_END, true);
				if (!string.IsNullOrEmpty(msg.m_countryCode))
				{
					SystemSaveManager.SetCountryCode(msg.m_countryCode);
					SystemSaveManager.CheckIAPMessage();
				}
				SystemSaveManager.Instance.CheckLightMode();
				SystemSaveManager.Instance.SaveSystemData();
			}
		}
	}

	private void ServerMigration_Failed(MsgServerConnctFailed msg)
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal;
			if (msg.m_status == ServerInterface.StatusCode.PassWordError)
			{
				signal = TinyFsmEvent.CreateUserEvent(113);
			}
			else
			{
				signal = TinyFsmEvent.CreateUserEvent(112);
			}
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ServerGetCountry_Succeeded(MsgGetCountrySucceed msg)
	{
		this.m_isGetCountry = true;
		if (SystemSaveManager.Instance != null)
		{
			SystemSaveManager.SetCountryCode(msg.m_countryCode);
			SystemSaveManager.CheckIAPMessage();
		}
	}

	private void DispatchMessage(MessageBase message)
	{
		if (this.m_fsm != null && message != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateMessage(message);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ServerGetMileageData_Succeeded()
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(107);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ServerGetEventList_Succeeded(MsgGetEventListSucceed msg)
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(108);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void OnConnectNoah(string result)
	{
		bool flag = false;
		switch (Noah.ConvertResultState(int.Parse(result)))
		{
		case Noah.ResultState.Success:
			flag = true;
			break;
		}
		global::Debug.Log("Title: Noah OnConnectNoah called");
		if (!flag)
		{
			return;
		}
		this.NoahBannerDisplay(true);
	}

	private void IapInitializeEndCallback(NativeObserver.IAPResult result)
	{
	}

	public void OnTouchedScreen()
	{
		if (this.m_initUser)
		{
			this.m_nextSceneName = "MainMenu";
			SoundManager.SePlay("sys_menu_decide", "SE");
			if (this.m_fsm != null)
			{
				TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
				this.m_fsm.Dispatch(signal);
			}
		}
	}

	public void OnTouchedAcknowledgment()
	{
		this.m_nextSceneName = "MainMenu";
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
			this.m_fsm.Dispatch(signal);
		}
	}

	public void OnTouchedAgreement()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		Application.OpenURL(NetBaseUtil.RedirectTrmsOfServicePageUrlForTitle);
	}

	private IEnumerator OpenAgreementWindow()
	{
		GameModeTitle._OpenAgreementWindow_c__Iterator21 _OpenAgreementWindow_c__Iterator = new GameModeTitle._OpenAgreementWindow_c__Iterator21();
		_OpenAgreementWindow_c__Iterator.__f__this = this;
		return _OpenAgreementWindow_c__Iterator;
	}

	public void OnClickMigrationButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		global::Debug.Log("GameModeTitle:Migration button pressed");
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(111);
			this.m_fsm.Dispatch(signal);
		}
	}

	public void OnClickCacheClearButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		global::Debug.Log("GameModeTitle:cache clear button pressed");
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "cache_clear",
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_bar"),
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Option", "cash_cashclear_explanation_title"),
			anchor_path = "Camera/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.YesNo
		});
		this.NoahBannerDisplay(false);
	}

	public void SegaLogoAnimationSkip()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject == null)
		{
			return;
		}
		SoundManager.BgmPlay("bgm_sys_title", "BGM", false);
		if (this.m_touchScreenObject != null)
		{
			this.m_touchScreenObject.SetActive(true);
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "ui_Lbl_mainmenu");
			UILabel component = this.m_touchScreenObject.GetComponent<UILabel>();
			if (component != null)
			{
				component.text = text.text;
			}
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_touchScreenObject, "Lbl_mainmenu_sh");
			if (uILabel != null)
			{
				uILabel.text = text.text;
			}
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "img_titlelogo");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(true);
		}
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject, "title_logo");
		if (animation != null)
		{
			foreach (AnimationState animationState in animation)
			{
				if (!(animationState == null))
				{
					animationState.time = animationState.length * 0.99f;
				}
			}
			animation.enabled = true;
			animation.Play("ui_title_loop_Anim");
		}
		Animation animation2 = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject, "TitleScreen");
		if (animation2 != null)
		{
			foreach (AnimationState animationState2 in animation2)
			{
				if (!(animationState2 == null))
				{
					animationState2.time = animationState2.length * 0.99f;
				}
			}
			ActiveAnimation.Play(animation2, "ui_title_intro_all_Anim", Direction.Forward);
		}
	}

	public void SegaLogoAnimationFinishCallback()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject == null)
		{
			return;
		}
		SoundManager.BgmPlay("bgm_sys_title", "BGM", false);
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject, "title_logo");
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_title_intro_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.TitleLogoAnimationFinishCallback), false);
		}
		Animation animation2 = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject, "TitleScreen");
		if (animation2 != null)
		{
			ActiveAnimation.Play(animation2, "ui_title_intro_all_Anim", Direction.Forward);
		}
	}

	public void TitleLogoAnimationFinishCallback()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject == null)
		{
			return;
		}
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject, "title_logo");
		if (animation != null)
		{
			animation.enabled = true;
			animation.Play("ui_title_loop_Anim");
		}
	}

	public void OnMsgGotoHead()
	{
		if (this.m_fsm != null)
		{
			if (this.m_loader != null)
			{
				UnityEngine.Object.Destroy(this.m_loader);
				this.m_loader = null;
			}
			if (this.m_loadingWindow != null)
			{
				UnityEngine.Object.Destroy(this.m_loadingWindow);
				this.m_loadingWindow = null;
				GameObject gameObject = GameObject.Find("UI Root (2D)");
				if (gameObject != null)
				{
					this.m_loadingWindow = GameObjectUtil.FindChildGameObjectComponent<HudLoadingWindow>(gameObject, "DownloadWindow");
				}
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadTitleResetScene)));
		}
	}

	public void StreamingKeyDataRetry()
	{
		StreamingDataKeyRetryProcess process = new StreamingDataKeyRetryProcess(base.gameObject, this);
		NetMonitor.Instance.StartMonitor(process, 0f, HudNetworkConnect.DisplayType.ALL);
		StreamingDataLoader.Instance.LoadServerKey(base.gameObject);
	}

	private void OnClickPlatformBackButtonEvent()
	{
		global::Debug.Log("GameModeTitle::Platform Back button pressed");
		this.SetUIEffect(false);
		this.NoahBannerDisplay(false);
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "quit_app",
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "gw_quit_app_caption"),
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "gw_quit_app_text"),
			anchor_path = "Camera/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.YesNo
		});
	}

	private void NoahBannerDisplay(bool display)
	{
		if (display)
		{
			Noah.Instance.SetBannerEffect(Noah.BannerEffect.EffectUp);
			Noah.BannerSize size = Noah.BannerSize.SizeWideFillParentWidth;
			Vector2 bannerSize = Noah.Instance.GetBannerSize(size);
			Noah.Instance.ShowBannerView(size, 0f, (float)Screen.height - bannerSize.y);
		}
		else
		{
			Noah.Instance.CloseBanner();
			NoahHandler.Instance.SetCallback(null);
		}
	}

	private void SetUIEffect(bool flag)
	{
		if (UIEffectManager.Instance != null)
		{
			UIEffectManager.Instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, flag);
		}
	}

	private string GetViewUserID()
	{
		string text = TitleUtil.GetSystemSaveDataGameId();
		if (text.Length > 7)
		{
			text = text.Insert(6, " ");
			text = text.Insert(3, " ");
		}
		return text;
	}

	private void SetFirstTutorialInfo()
	{
		GameObject gameObject = GameObject.Find("StageInfo");
		if (gameObject != null)
		{
			StageInfo component = gameObject.GetComponent<StageInfo>();
			if (component != null)
			{
				StageInfo.MileageMapInfo mileageMapInfo = new StageInfo.MileageMapInfo();
				mileageMapInfo.m_mapState.m_episode = 1;
				mileageMapInfo.m_mapState.m_chapter = 1;
				mileageMapInfo.m_mapState.m_point = 0;
				mileageMapInfo.m_mapState.m_score = 0L;
				component.SelectedStageName = StageInfo.GetStageNameByIndex(1);
				component.TenseType = TenseType.AFTERNOON;
				component.ExistBoss = true;
				component.BossStage = false;
				component.TutorialStage = false;
				component.FromTitle = false;
				component.FirstTutorial = true;
				component.MileageInfo = mileageMapInfo;
			}
		}
		LoadingInfo loadingInfo = LoadingInfo.CreateLoadingInfo();
		if (loadingInfo != null)
		{
			LoadingInfo.LoadingData info = loadingInfo.GetInfo();
			if (info != null)
			{
				string cellID = CharaName.Name[0];
				string commonText = TextUtility.GetCommonText("CharaName", cellID);
				info.m_titleText = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FirstLoading", "ui_Lbl_title_text").text;
				info.m_mainText = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FirstLoading", "ui_Lbl_main_text").text;
				info.m_optionTutorial = true;
				info.m_texture = null;
			}
		}
	}

	private void StreamingDataLoad_Succeed()
	{
		NetMonitor.Instance.EndMonitorForward(new MsgAssetBundleResponseSucceed(null, null), null, null);
		NetMonitor.Instance.EndMonitorBackward();
	}

	private void StreamingDataLoad_Failed()
	{
		NetMonitor.Instance.EndMonitorForward(new MsgAssetBundleResponseFailedMonitor(), null, null);
		NetMonitor.Instance.EndMonitorBackward();
	}
}
