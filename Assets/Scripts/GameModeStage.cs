using App;
using DataTable;
using Message;
using Mission;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using Tutorial;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/GameMode/Stage")]
public class GameModeStage : MonoBehaviour
{
	private struct PostGameResultInfo
	{
		public bool m_existBoss;

		public StageInfo.MileageMapInfo m_prevMapInfo;
	}

	private enum LoadType
	{
		COMMON_OBJECT_RESOURCE,
		COMMON_OBJECT_PREFABS,
		COMMON_ENEMY_RESOURCE,
		COMMON_ENEMY_PREFABS,
		RESOURCES_COMMON_EFFECT,
		CHARACTER_COMMON_RESOURCE,
		EVENT_RESOURCE_STAGE,
		EVENT_RESOURCE_COMMON,
		NUM
	}

	public enum ProgressBarLeaveState
	{
		IDLE = -1,
		StateInit,
		StateLoad,
		StateLoad2,
		StateRequestStartAct,
		StateSoundConnectIfNotFound,
		StateAccessNetworkForStartAct,
		StateSetupPrepareBlock,
		StateSetupBlock,
		StateSendApolloStageStart,
		NUM
	}

	private enum ChangeLevelSubState
	{
		FADEOUT,
		FADEOUT_STOPCHARACTER,
		SETUP_SPEEDLEVEL,
		WAITPREPARE_STAGE,
		SETUP_STAGE,
		WAIT,
		FADEIN
	}

	private enum TutorialMissionEndSubState
	{
		SHOWRESULT,
		FADEOUT,
		WAIT,
		FADEIN,
		END
	}

	private sealed class _NotSendPostGameResult_c__Iterator1D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal GameModeStage __f__this;

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
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.DispatchMessage(new MsgPostGameResultsSucceed());
				this._PC = -1;
				break;
			}
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

	private sealed class _NotSendEventUpdateGameResult_c__Iterator1E : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal GameModeStage __f__this;

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
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.DispatchMessage(new MsgEventUpdateGameResultsSucceed());
				this._PC = -1;
				break;
			}
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

	private sealed class _NotSendEventPostGameResult_c__Iterator1F : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal GameModeStage __f__this;

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
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.DispatchMessage(new MsgEventPostGameResultsSucceed());
				this._PC = -1;
				break;
			}
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

	private const float LightModeFixedTimeStep = 0.033333f;

	private const float LightModeMaxFixedTimeStep = 0.333333f;

	private const string pre_characterModelResourceName = "CharacterModel";

	private const string pre_characterEffectName = "CharacterEffect";

	private const string PathManagerName = "StagePathManager";

	private bool m_isLoadResources = true;

	public bool m_isCreatespawnableManager;

	public bool m_isCreateTerrainPlacementManager;

	public bool m_notPlaceTerrain;

	public bool m_showBlockInfo;

	public bool m_randomBlock;

	public bool m_useTemporarySet;

	public int m_blockNumOfNotPlaceTerrain = -1;

	public bool m_bossStage;

	public int m_debugBossLevel;

	public BossType m_bossType = BossType.MAP1;

	public bool m_useCharaInInspector;

	public bool m_noStartHud;

	private bool m_exitFromResult;

	private bool m_bossClear;

	private bool m_bossTimeUp;

	private bool m_quickModeTimeUp;

	[SerializeField]
	private bool m_tutorialStage;

	[SerializeField]
	private bool m_eventStage;

	private bool m_showMapBossTutorial;

	private bool m_showFeverBossTutorial;

	private bool m_showEventBossTutorial;

	private int m_showItemTutorial = -1;

	private int m_showCharaTutorial = -1;

	private int m_showActionTutorial = -1;

	private int m_showQuickTurorial = -1;

	private bool m_fromTitle;

	private bool m_serverActEnd;

	private bool m_bossNoMissChance;

	private bool m_saveFlag;

	private bool m_firstTutorial;

	private bool m_equipItemTutorial;

	private bool m_missonCompleted;

	private int m_oldNumBossAttack;

	private bool m_retired;

	private TinyFsmBehavior m_fsm;

	[SerializeField]
	private string m_stageName = "w01";

	[SerializeField]
	private TenseType m_stageTenseType = TenseType.NONE;

	[SerializeField]
	private CharaType m_mainChara;

	[SerializeField]
	private CharaType m_subChara = CharaType.UNKNOWN;

	private int m_substate;

	private List<ItemType> m_useEquippedItem = new List<ItemType>();

	private List<BoostItemType> m_useBoostItem = new List<BoostItemType>();

	private GameObject m_sceneLoader;

	private GameObject m_stageBlockManager;

	private PathManager m_stagePathManager;

	private List<GameObject> m_pausedObject;

	private PlayerInformation m_playerInformation;

	private LevelInformation m_levelInformation;

	private HudCaution m_hudCaution;

	private CharacterContainer m_characterContainer;

	private StageMissionManager m_missionManager;

	private StageTutorialManager m_tutorialManager;

	private FriendSignManager m_friendSignManager;

	private EventManager m_eventManager;

	private string m_terrainDataName;

	private string m_stageResourceName;

	private string m_stageResourceObjectName;

	private bool m_onSpeedUp;

	private bool m_onDestroyRingMode;

	[SerializeField]
	private int m_numEnableContinue = 2;

	private int m_invalidExtremeCount;

	private bool m_invalidExtremeFlag;

	private float m_timer;

	private int m_counter;

	private bool m_reqPause;

	private bool m_reqPauseBackMain;

	private bool m_reqTutorialPause;

	private bool m_IsNowLastChanceHudCautionBoss;

	private bool m_receiveInvincibleMsg;

	[SerializeField]
	private float m_defaultTimeScale = 1f;

	private float m_chaoEasyTimeScale = 1f;

	private float m_gameResultTimer;

	private GameResult m_gameResult;

	private MsgTutorialPlayEnd m_tutorialEndMsg;

	private Tutorial.EventID m_tutorialMissionID;

	private HudTutorial.Kind m_tutorialKind;

	private string m_mainBgmName = "bgm_z_w01";

	private RareEnemyTable m_rareEnemyTable;

	private EnemyExtendItemTable m_enemyExtendItemTable;

	private BossTable m_bossTable;

	private BossMap3Table m_bossMap3Table;

	private ObjectPartTable m_objectPartTable;

	private float m_savedFixedTimeStep;

	private float m_savedMaxFixedTimeStep;

	private GameModeStage.PostGameResultInfo m_postGameResults = default(GameModeStage.PostGameResultInfo);

	private ServerMileageMapState m_resultMapState;

	private List<ServerMileageIncentive> m_mileageIncentive;

	private List<ServerItemState> m_dailyIncentive;

	private SendApollo m_sendApollo;

	private StageEffect m_stageEffect;

	private List<ResourceSceneLoader.ResourceInfo> m_loadInfo = new List<ResourceSceneLoader.ResourceInfo>
	{
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.OBJECT_RESOURCE, "CommonObjectResource", true, false, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.OBJECT_PREFAB, "CommonObjectPrefabs", false, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ENEMY_RESOURCE, "CommonEnemyResource", true, false, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ENEMY_PREFAB, "CommonEnemyPrefabs", false, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.COMMON_EFFECT, "ResourcesCommonEffect", true, false, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.PLAYER_COMMON, "CharacterCommonResource", true, false, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.EVENT_RESOURCE, "EventResourceStage", true, false, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.EVENT_RESOURCE, "EventResourceCommon", true, false, false, null, false)
	};

	private List<ResourceSceneLoader.ResourceInfo> m_quickModeLoadInfo = new List<ResourceSceneLoader.ResourceInfo>
	{
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.QUICK_MODE, "StageTimeTable", true, false, true, null, false)
	};

	private GameObject m_uiRootObj;

	private GameObject m_continueWindowObj;

	private ServerEventRaidBossBonus m_raidBossBonus;

	private HudProgressBar m_progressBar;

	private GameObject m_connectAlertUI2;

	private bool m_quickMode;

	private readonly Vector3 PlayerResetPosition = Vector3.zero;

	private readonly Quaternion PlayerResetRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

	public int numEnableContinue
	{
		get
		{
			return this.m_numEnableContinue;
		}
	}

	public string GetStageName()
	{
		return this.m_stageName;
	}

	public static int ContinueRestCount()
	{
		GameObject gameObject = GameObject.Find("GameModeStage");
		if (gameObject != null)
		{
			GameModeStage component = gameObject.GetComponent<GameModeStage>();
			if (component != null)
			{
				return component.numEnableContinue;
			}
		}
		return 0;
	}

	private void Awake()
	{
		Application.targetFrameRate = SystemSettings.TargetFrameRate;
	}

	private void Start()
	{
		HudUtility.SetInvalidNGUIMitiTouch();
		base.gameObject.tag = "GameModeStage";
		this.m_pausedObject = new List<GameObject>();
		this.m_rareEnemyTable = new RareEnemyTable();
		this.m_enemyExtendItemTable = new EnemyExtendItemTable();
		this.m_bossTable = new BossTable();
		this.m_bossMap3Table = new BossMap3Table();
		this.m_objectPartTable = new ObjectPartTable();
		this.m_savedFixedTimeStep = Time.fixedDeltaTime;
		this.m_savedMaxFixedTimeStep = Time.maximumDeltaTime;
		this.m_useEquippedItem = new List<ItemType>();
		if (SystemSaveManager.GetSystemSaveData() != null && SystemSaveManager.GetSystemSaveData().lightMode)
		{
			Time.fixedDeltaTime = 0.033333f;
			Time.maximumDeltaTime = 0.333333f;
		}
		if (FontManager.Instance != null && FontManager.Instance.IsNecessaryLoadFont())
		{
			FontManager.Instance.LoadResourceData();
		}
		BackKeyManager.AddEventCallBack(base.gameObject);
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm != null)
		{
			TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
			description.initState = new TinyFsmState(new EventFunction(this.StateInit));
			description.onFixedUpdate = false;
			this.m_fsm.SetUp(description);
		}
		SoundManager.AddStageCommonCueSheet();
		if (ServerInterface.SettingState != null)
		{
			this.m_numEnableContinue = ServerInterface.SettingState.m_onePlayContinueCount;
		}
		this.m_uiRootObj = GameObject.Find("UI Root (2D)");
		if (this.m_uiRootObj != null)
		{
			this.m_connectAlertUI2 = GameObjectUtil.FindChildGameObject(this.m_uiRootObj, "ConnectAlert_2_UI");
			if (this.m_connectAlertUI2 != null)
			{
				this.m_connectAlertUI2.SetActive(true);
				this.m_progressBar = GameObjectUtil.FindChildGameObjectComponent<HudProgressBar>(this.m_connectAlertUI2, "Pgb_loading");
				if (this.m_progressBar != null)
				{
					this.m_progressBar.SetUp(9);
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (this.m_reqPause)
		{
			this.ChangeState(new TinyFsmState(new EventFunction(this.StatePause)));
		}
		else if (this.m_quickMode && !this.m_quickModeTimeUp && StageTimeManager.Instance != null && StageTimeManager.Instance.IsTimeUp())
		{
			this.OnQuickModeTimeUp(new MsgQuickModeTimeUp());
		}
		if (this.m_levelInformation != null)
		{
			this.m_levelInformation.RequestCharaChange = false;
			this.m_levelInformation.RequestEqitpItem = false;
		}
	}

	private IEnumerator NotSendPostGameResult()
	{
		GameModeStage._NotSendPostGameResult_c__Iterator1D _NotSendPostGameResult_c__Iterator1D = new GameModeStage._NotSendPostGameResult_c__Iterator1D();
		_NotSendPostGameResult_c__Iterator1D.__f__this = this;
		return _NotSendPostGameResult_c__Iterator1D;
	}

	private IEnumerator NotSendEventUpdateGameResult()
	{
		GameModeStage._NotSendEventUpdateGameResult_c__Iterator1E _NotSendEventUpdateGameResult_c__Iterator1E = new GameModeStage._NotSendEventUpdateGameResult_c__Iterator1E();
		_NotSendEventUpdateGameResult_c__Iterator1E.__f__this = this;
		return _NotSendEventUpdateGameResult_c__Iterator1E;
	}

	private IEnumerator NotSendEventPostGameResult()
	{
		GameModeStage._NotSendEventPostGameResult_c__Iterator1F _NotSendEventPostGameResult_c__Iterator1F = new GameModeStage._NotSendEventPostGameResult_c__Iterator1F();
		_NotSendEventPostGameResult_c__Iterator1F.__f__this = this;
		return _NotSendEventPostGameResult_c__Iterator1F;
	}

	private void OnDestroy()
	{
		if (this.m_fsm)
		{
			this.m_fsm.ShutDown();
			this.m_fsm = null;
		}
		this.RemoveAllResource();
		Time.fixedDeltaTime = this.m_savedFixedTimeStep;
		Time.maximumDeltaTime = this.m_savedMaxFixedTimeStep;
		this.StopStageEffect();
	}

	private void OnMsgNotifyDead(MsgNotifyDead message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgNotifyStartPause(MsgNotifyStartPause message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgNotifyEndPause(MsgNotifyEndPause message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgNotifyEndPauseExitStage(MsgNotifyEndPauseExitStage message)
	{
		this.DispatchMessage(message);
	}

	private void OnSendToGameModeStage(MessageBase message)
	{
		this.DispatchMessage(message);
	}

	private void OnBossEnd(MsgBossEnd message)
	{
		this.DispatchMessage(message);
	}

	private void OnBossClear(MsgBossClear message)
	{
		this.DispatchMessage(message);
	}

	private void OnBossTimeUp()
	{
		this.m_bossTimeUp = true;
	}

	private void OnQuickModeTimeUp(MsgQuickModeTimeUp message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgChangeChara(MsgChangeChara message)
	{
		this.DispatchMessage(message);
	}

	private void OnTransformPhantom(MsgTransformPhantom message)
	{
		this.DispatchMessage(message);
		if (message.m_type == PhantomType.LASER)
		{
			ObjUtil.CreatePrism();
		}
	}

	private void OnReturnFromPhantom(MsgReturnFromPhantom message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgInvincible(MsgInvincible message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgExternalGamePause(MsgExternalGamePause message)
	{
		this.m_reqPauseBackMain = message.m_backMainMenu;
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialBackKey(MsgTutorialBackKey message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgContinueBackKey(MsgContinueBackKey message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialPlayStart(MsgTutorialPlayStart message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialPlayAction(MsgTutorialPlayAction message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialItemButtonEnd(MsgTutorialEnd message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialPlayEnd(MsgTutorialPlayEnd message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialMapBoss(MsgTutorialMapBoss message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialFeverBoss(MsgTutorialFeverBoss message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialItem(MsgTutorialItem message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialItemButton(MsgTutorialItemButton message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialChara(MsgTutorialChara message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialAction(MsgTutorialAction message)
	{
		this.DispatchMessage(message);
	}

	private void OnMsgTutorialQuickMode(MsgTutorialQuickMode message)
	{
		this.DispatchMessage(message);
	}

	private void OnChangeCharaSucceed(MsgChangeCharaSucceed message)
	{
		if (HudTutorial.IsCharaTutorial((CharaType)this.m_playerInformation.SubCharacterID))
		{
			this.m_showCharaTutorial = (int)CharaTypeUtil.GetCharacterTutorialID((CharaType)this.m_playerInformation.SubCharacterID);
			if (this.m_showCharaTutorial != -1)
			{
				GameObjectUtil.SendDelayedMessageToGameObject(base.gameObject, "OnMsgTutorialChara", new MsgTutorialChara((HudTutorial.Id)this.m_showCharaTutorial));
			}
		}
	}

	private void OnClickPlatformBackButtonEvent()
	{
		bool backMainMenu = !this.m_firstTutorial;
		this.OnMsgExternalGamePause(new MsgExternalGamePause(backMainMenu, true));
		this.OnMsgTutorialBackKey(new MsgTutorialBackKey());
		this.OnMsgContinueBackKey(new MsgContinueBackKey());
	}

	private void ServerPostGameResults_Succeeded(MsgPostGameResultsSucceed message)
	{
		NetUtil.SyncSaveDataAndDataBase(message.m_playerState);
		this.m_resultMapState = message.m_mileageMapState;
		if (message.m_mileageIncentive != null)
		{
			this.m_mileageIncentive = new List<ServerMileageIncentive>(message.m_mileageIncentive.Count);
			foreach (ServerMileageIncentive current in message.m_mileageIncentive)
			{
				this.m_mileageIncentive.Add(current);
			}
		}
		if (message.m_dailyIncentive != null)
		{
			this.m_dailyIncentive = new List<ServerItemState>(message.m_dailyIncentive.Count);
			foreach (ServerItemState current2 in message.m_dailyIncentive)
			{
				this.m_dailyIncentive.Add(current2);
			}
		}
		this.DispatchMessage(message);
	}

	private void ServerPostGameResults_Failed(MsgServerConnctFailed message)
	{
		this.DispatchMessage(message);
	}

	private void ServerStartAct_Succeeded(MsgActStartSucceed message)
	{
		this.m_serverActEnd = true;
		NetUtil.SyncSaveDataAndDataBase(message.m_playerState);
		this.DispatchMessage(message);
	}

	private void ServerStartAct_Failed(MsgServerConnctFailed message)
	{
		this.DispatchMessage(message);
	}

	private void ServerQuickModeStartAct_Succeeded(MsgQuickModeActStartSucceed message)
	{
		this.m_serverActEnd = true;
		NetUtil.SyncSaveDataAndDataBase(message.m_playerState);
		this.DispatchMessage(message);
	}

	private void ServerQuickModePostGameResults_Succeeded(MsgQuickModePostGameResultsSucceed message)
	{
		NetUtil.SyncSaveDataAndDataBase(message.m_playerState);
		if (message.m_dailyIncentive != null)
		{
			this.m_dailyIncentive = new List<ServerItemState>(message.m_dailyIncentive.Count);
			foreach (ServerItemState current in message.m_dailyIncentive)
			{
				this.m_dailyIncentive.Add(current);
			}
		}
		this.DispatchMessage(message);
	}

	private void ServerEventStartAct_Succeeded(MsgEventActStartSucceed message)
	{
		this.m_serverActEnd = true;
		NetUtil.SyncSaveDataAndDataBase(message.m_playerState);
		this.DispatchMessage(message);
	}

	private void ServerEventStartAct_Failed(MsgServerConnctFailed message)
	{
		this.DispatchMessage(message);
	}

	private void ServerUpdateGameResults_Succeeded(MsgEventUpdateGameResultsSucceed message)
	{
		if (message.m_bonus != null)
		{
			this.m_raidBossBonus = new ServerEventRaidBossBonus();
			message.m_bonus.CopyTo(this.m_raidBossBonus);
		}
		this.DispatchMessage(message);
	}

	private void ServerEventPostGameResults_Succeeded(MsgEventPostGameResultsSucceed message)
	{
		this.DispatchMessage(message);
	}

	private void ServerDrawRaidBoss_Succeeded(MsgDrawRaidBossSucceed message)
	{
		this.DispatchMessage(message);
	}

	private void ServerGetEventUserRaidBossState_Succeeded(MsgGetEventUserRaidBossStateSucceed message)
	{
		this.DispatchMessage(message);
	}

	private void DailyBattleResultCallBack()
	{
		MessageBase message = new MessageBase(61515);
		this.DispatchMessage(message);
	}

	private void DispatchMessage(MessageBase message)
	{
		if (this.m_fsm != null && message != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateMessage(message);
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ChangeState(TinyFsmState nextState)
	{
		this.m_fsm.ChangeState(nextState);
		this.m_substate = 0;
	}

	private TinyFsmState StateInit(TinyFsmEvent e)
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
		{
			TimeProfiler.EndCountTime("MainMenu-GameModeStage");
			TimeProfiler.StartCountTime("GameModeStage:StateInit");
			GC.Collect();
			Resources.UnloadUnusedAssets();
			GC.Collect();
			CameraFade.StartAlphaFade(Color.black, true, 1f, 0f);
			HudLoading.StartScreen(null);
			if (this.m_uiRootObj == null)
			{
				this.m_uiRootObj = GameObject.Find("UI Root (2D)");
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			GameObject[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject = array2[i];
				if (gameObject.activeInHierarchy)
				{
					this.m_pausedObject.Add(gameObject);
				}
				gameObject.SetActive(false);
			}
			GameObject[] array3 = GameObject.FindGameObjectsWithTag("Chao");
			GameObject[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				GameObject gameObject2 = array4[j];
				this.m_pausedObject.Add(gameObject2);
				gameObject2.SetActive(false);
			}
			if (AtlasManager.Instance == null)
			{
				GameObject gameObject3 = new GameObject("AtlasManager");
				gameObject3.AddComponent<AtlasManager>();
			}
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
			if (GameObjectUtil.FindGameObjectComponent<StageAbilityManager>("StageAbilityManager") == null)
			{
				GameObject gameObject4 = new GameObject("StageAbilityManager");
				gameObject4.AddComponent<StageAbilityManager>();
				gameObject4.tag = "Manager";
			}
			if (InformationDataTable.Instance == null)
			{
				InformationDataTable.Create();
				InformationDataTable.Instance.Initialize(base.gameObject);
			}
			this.m_levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
			if (this.m_levelInformation == null)
			{
				this.m_levelInformation = new GameObject("LevelInformation")
				{
					tag = "StageManager"
				}.AddComponent<LevelInformation>();
			}
			this.m_eventManager = EventManager.Instance;
			bool flag = false;
			StageInfo stageInfo = GameObjectUtil.FindGameObjectComponent<StageInfo>("StageInfo");
			if (stageInfo != null)
			{
				if (this.m_levelInformation != null)
				{
					this.m_levelInformation.Missed = false;
					this.m_stageName = stageInfo.SelectedStageName;
					this.m_firstTutorial = stageInfo.FirstTutorial;
					this.m_tutorialStage = stageInfo.TutorialStage;
					this.m_fromTitle = stageInfo.FromTitle;
					this.m_quickMode = stageInfo.QuickMode;
					this.m_bossStage = (!this.m_quickMode && stageInfo.BossStage);
					if (this.m_firstTutorial)
					{
						this.m_quickMode = false;
						this.m_tutorialStage = false;
						this.m_bossStage = false;
					}
					else if (this.m_tutorialStage)
					{
						this.m_quickMode = false;
						this.m_bossStage = false;
					}
					if (!this.m_firstTutorial && !this.m_tutorialStage && HudMenuUtility.IsItemTutorial())
					{
						this.m_equipItemTutorial = true;
					}
					if (StageModeManager.Instance != null)
					{
						StageModeManager.Instance.FirstTutorial = this.m_firstTutorial;
						if (this.m_quickMode)
						{
							StageModeManager.Instance.StageMode = StageModeManager.Mode.QUICK;
						}
						else if (this.m_firstTutorial || this.m_tutorialStage)
						{
							StageModeManager.Instance.StageMode = StageModeManager.Mode.UNKNOWN;
						}
						else
						{
							StageModeManager.Instance.StageMode = StageModeManager.Mode.ENDLESS;
						}
					}
					if (this.m_bossStage)
					{
						this.m_bossType = stageInfo.BossType;
					}
					else
					{
						this.m_bossType = BossType.FEVER;
					}
					this.m_eventStage = stageInfo.EventStage;
					flag = stageInfo.BoostItemValid[2];
					if (TenseEffectManager.Instance != null)
					{
						TenseEffectManager.Type type = (stageInfo.TenseType != TenseType.AFTERNOON) ? TenseEffectManager.Type.TENSE_B : TenseEffectManager.Type.TENSE_A;
						TenseEffectManager.Instance.SetType(type);
						this.m_stageTenseType = stageInfo.TenseType;
						TenseEffectManager.Instance.NotChangeTense = stageInfo.NotChangeTense;
					}
					this.m_postGameResults.m_existBoss = stageInfo.ExistBoss;
					this.m_postGameResults.m_prevMapInfo = new StageInfo.MileageMapInfo();
					this.m_postGameResults.m_prevMapInfo.m_mapState.Set(stageInfo.MileageInfo.m_mapState);
					stageInfo.MileageInfo.m_pointScore.CopyTo(this.m_postGameResults.m_prevMapInfo.m_pointScore, 0);
					this.m_levelInformation.NumBossAttack = stageInfo.NumBossAttack;
					this.m_oldNumBossAttack = this.m_levelInformation.NumBossAttack;
					if (this.m_levelInformation.NumBossAttack > 0)
					{
						this.m_bossNoMissChance = false;
					}
					else
					{
						this.m_bossNoMissChance = true;
					}
					bool flag2 = true;
					if (SystemSaveManager.GetSystemSaveData() != null)
					{
						this.m_levelInformation.LightMode = SystemSaveManager.GetSystemSaveData().lightMode;
						int numRank = ServerInterface.PlayerState.m_numRank;
						if (numRank < 2)
						{
							flag2 = SystemSaveManager.GetSystemSaveData().IsFlagStatus(SystemData.FlagStatus.TUTORIAL_FEVER_BOSS);
						}
					}
					if (this.m_bossStage)
					{
						if (this.m_eventManager != null && this.m_eventManager.IsRaidBossStage() && this.m_eventStage)
						{
							int num = 0;
							if (RaidBossInfo.currentRaidData != null)
							{
								num = RaidBossInfo.currentRaidData.lv;
							}
							if (this.m_levelInformation.NumBossAttack == 0 && (num == 1 || num == 5))
							{
								this.m_showEventBossTutorial = true;
							}
						}
						else if (stageInfo.MileageInfo.m_mapState != null && this.m_levelInformation.NumBossAttack == 0)
						{
							MileageMapState mapState = stageInfo.MileageInfo.m_mapState;
							if (mapState.m_episode == 1)
							{
								this.m_showMapBossTutorial = true;
							}
							if (mapState.m_episode == 2)
							{
								this.m_showMapBossTutorial = true;
							}
							if (mapState.m_episode == 3)
							{
								this.m_showMapBossTutorial = true;
							}
						}
					}
					else
					{
						this.m_showFeverBossTutorial = !flag2;
					}
				}
				if (StageItemManager.Instance != null)
				{
					StageItemManager.Instance.SetEquipItemTutorial(this.m_equipItemTutorial);
					StageItemManager.Instance.SetEquippedItem(stageInfo.EquippedItems);
					this.m_useEquippedItem.Clear();
					if (stageInfo.EquippedItems != null)
					{
						ItemType[] equippedItems = stageInfo.EquippedItems;
						for (int k = 0; k < equippedItems.Length; k++)
						{
							ItemType item = equippedItems[k];
							this.m_useEquippedItem.Add(item);
						}
					}
				}
				for (int l = 0; l < 3; l++)
				{
					bool flag3 = stageInfo.BoostItemValid[l];
					if (flag3)
					{
						BoostItemType item2 = (BoostItemType)l;
						this.m_useBoostItem.Add(item2);
					}
					if (l == 1 && StageItemManager.Instance != null)
					{
						StageItemManager.Instance.SetActiveAltitudeTrampoline(flag3);
					}
				}
				UnityEngine.Object.Destroy(stageInfo.gameObject);
			}
			TerrainXmlData.SetAssetName(this.m_stageName);
			StageScoreManager instance = StageScoreManager.Instance;
			if (instance != null)
			{
				instance.Setup(this.m_bossStage);
			}
			this.m_stagePathManager = GameObjectUtil.FindGameObjectComponent<PathManager>("StagePathManager");
			if (this.m_stagePathManager == null)
			{
				GameObject gameObject5 = new GameObject("StagePathManager");
				this.m_stagePathManager = gameObject5.AddComponent<PathManager>();
			}
			this.m_playerInformation = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
			if (this.m_playerInformation != null)
			{
				if (this.m_playerInformation != null && SaveDataManager.Instance != null)
				{
					PlayerData playerData = SaveDataManager.Instance.PlayerData;
					this.m_mainChara = playerData.MainChara;
					this.m_subChara = ((!flag) ? CharaType.UNKNOWN : playerData.SubChara);
				}
				else
				{
					this.m_mainChara = CharaType.SONIC;
					this.m_subChara = CharaType.UNKNOWN;
				}
				this.m_playerInformation.SetPlayerCharacter((int)this.m_mainChara, (int)this.m_subChara);
			}
			this.m_characterContainer = GameObjectUtil.FindGameObjectComponent<CharacterContainer>("CharacterContainer");
			if (this.m_characterContainer != null)
			{
				this.m_characterContainer.Init();
			}
			this.m_hudCaution = HudCaution.Instance;
			this.m_missionManager = GameObjectUtil.FindGameObjectComponent<StageMissionManager>("StageMissionManager");
			this.m_tutorialManager = GameObjectUtil.FindGameObjectComponent<StageTutorialManager>("StageTutorialManager");
			this.m_friendSignManager = GameObjectUtil.FindGameObjectComponent<FriendSignManager>("FriendSignManager");
			if (this.m_tutorialStage)
			{
				if (this.m_tutorialManager == null)
				{
					GameObject gameObject6 = new GameObject("StageTutorialManager");
					this.m_tutorialManager = gameObject6.AddComponent<StageTutorialManager>();
				}
			}
			else if (this.m_tutorialManager != null)
			{
				UnityEngine.Object.Destroy(this.m_tutorialManager.gameObject);
				this.m_tutorialManager = null;
			}
			if (this.m_uiRootObj != null)
			{
				GameObject gameObject7 = GameObjectUtil.FindChildGameObject(this.m_uiRootObj, "Result");
				if (gameObject7 != null)
				{
					this.m_gameResult = gameObject7.GetComponent<GameResult>();
					gameObject7.SetActive(false);
				}
			}
			if (ServerInterface.PlayerState != null && this.m_levelInformation != null)
			{
				this.m_levelInformation.PlayerRank = ServerInterface.PlayerState.m_numRank;
			}
			this.CreateStageBlockManager();
			if (this.m_eventManager != null && this.m_eventManager.IsRaidBossStage())
			{
				if (this.m_bossType == BossType.EVENT2)
				{
					this.SendPlayerSpeedLevel(PlayerSpeed.LEVEL_2);
				}
				else if (this.m_bossType == BossType.EVENT3)
				{
					this.SendPlayerSpeedLevel(PlayerSpeed.LEVEL_3);
				}
			}
			else if (this.m_quickMode)
			{
				int a = 0;
				if (this.m_stageBlockManager != null)
				{
					a = this.m_stageBlockManager.GetComponent<StageBlockManager>().GetBlockLevel();
				}
				int speedLevel = Mathf.Min(a, 2);
				this.SendPlayerSpeedLevel((PlayerSpeed)speedLevel);
			}
			if (DelayedMessageManager.Instance == null)
			{
				GameObject gameObject8 = new GameObject("DelayedMessageManager");
				gameObject8.AddComponent<DelayedMessageManager>();
			}
			this.m_tutorialKind = HudTutorial.Kind.NONE;
			this.m_showItemTutorial = -1;
			if (this.m_playerInformation != null)
			{
				if (HudTutorial.IsCharaTutorial((CharaType)this.m_playerInformation.MainCharacterID))
				{
					this.m_showCharaTutorial = (int)CharaTypeUtil.GetCharacterTutorialID((CharaType)this.m_playerInformation.MainCharacterID);
				}
				else
				{
					this.m_showCharaTutorial = -1;
				}
			}
			this.m_showActionTutorial = -1;
			if (this.m_quickMode)
			{
				if (HudTutorial.IsQuickModeTutorial(HudTutorial.Id.QUICK_1))
				{
					this.m_showQuickTurorial = 54;
				}
			}
			else
			{
				this.m_showQuickTurorial = -1;
			}
			this.m_saveFlag = false;
			GameObject gameObject9 = GameObject.Find("AllocationStatus");
			if (gameObject9 != null)
			{
				UnityEngine.Object.Destroy(gameObject9);
			}
			TimeProfiler.EndCountTime("GameModeStage:StateInit");
			return TinyFsmState.End();
		}
		case 4:
			if (!Env.useAssetBundle || AssetBundleLoader.Instance.IsEnableDownlad())
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoad)));
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoad(TinyFsmEvent e)
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
			this.m_sceneLoader = new GameObject("SceneLoader");
			ResourceSceneLoader resourceSceneLoader = this.m_sceneLoader.AddComponent<ResourceSceneLoader>();
			TextManager.LoadCommonText(resourceSceneLoader);
			TextManager.LoadEventText(resourceSceneLoader);
			TextManager.LoadChaoText(resourceSceneLoader);
			resourceSceneLoader.AddLoad("TenseEffectTable", true, false);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_sceneLoader != null && this.m_sceneLoader.GetComponent<ResourceSceneLoader>().Loaded)
			{
				TextManager.SetupCommonText();
				TextManager.SetupEventText();
				TextManager.SetupChaoText();
				UnityEngine.Object.Destroy(this.m_sceneLoader);
				this.m_sceneLoader = null;
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateLoad2)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoad2(TinyFsmEvent e)
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
			TimeProfiler.StartCountTime("GameModeStage:Load");
			this.m_counter = 0;
			if (AtlasManager.Instance != null)
			{
				AtlasManager.Instance.StartLoadAtlasForStage();
			}
			if (this.m_isLoadResources)
			{
				this.m_sceneLoader = new GameObject("SceneLoader");
				ResourceSceneLoader resourceSceneLoader = this.m_sceneLoader.AddComponent<ResourceSceneLoader>();
				for (int i = 0; i < this.m_loadInfo.Count; i++)
				{
					ResourceSceneLoader.ResourceInfo resourceInfo = this.m_loadInfo[i];
					if (i == 6)
					{
						if (this.m_quickMode && this.m_eventManager != null && this.m_eventManager.Type == EventManager.EventType.QUICK)
						{
							ResourceSceneLoader.ResourceInfo resourceInfo2 = resourceInfo;
							ResourceSceneLoader.ResourceInfo expr_CB = resourceInfo2;
							expr_CB.m_scenename += EventManager.GetResourceName();
							resourceSceneLoader.AddLoadAndResourceManager(resourceInfo2);
						}
					}
					else if (i == 7)
					{
						if (this.m_eventManager != null && this.m_eventManager.Type != EventManager.EventType.UNKNOWN)
						{
							ResourceSceneLoader.ResourceInfo resourceInfo3 = resourceInfo;
							ResourceSceneLoader.ResourceInfo expr_11B = resourceInfo3;
							expr_11B.m_scenename += EventManager.GetResourceName();
							resourceSceneLoader.AddLoadAndResourceManager(resourceInfo3);
						}
					}
					else
					{
						resourceSceneLoader.AddLoadAndResourceManager(resourceInfo);
					}
				}
				if (this.m_quickMode)
				{
					foreach (ResourceSceneLoader.ResourceInfo current in this.m_quickModeLoadInfo)
					{
						if (current.m_category == ResourceCategory.EVENT_RESOURCE)
						{
							if (this.m_eventManager != null && this.m_eventStage)
							{
								ResourceSceneLoader.ResourceInfo resourceInfo4 = current;
								ResourceSceneLoader.ResourceInfo expr_1B1 = resourceInfo4;
								expr_1B1.m_scenename += EventManager.GetResourceName();
								resourceSceneLoader.AddLoadAndResourceManager(resourceInfo4);
							}
						}
						else
						{
							resourceSceneLoader.AddLoadAndResourceManager(current);
						}
					}
				}
				this.m_terrainDataName = this.m_stageName + "_TerrainData";
				resourceSceneLoader.AddLoad(this.m_terrainDataName, true, false);
				this.m_stageResourceName = this.m_stageName + "_StageResource";
				this.m_stageResourceObjectName = this.m_stageName + "_StageModelResource";
				resourceSceneLoader.AddLoad(this.m_stageResourceName, true, false);
				if (this.m_playerInformation != null)
				{
					string mainCharacterName = this.m_playerInformation.MainCharacterName;
					if (mainCharacterName != null)
					{
						resourceSceneLoader.AddLoad("CharacterModel" + mainCharacterName, true, false);
						resourceSceneLoader.AddLoad("CharacterEffect" + mainCharacterName, true, false);
					}
					string subCharacterName = this.m_playerInformation.SubCharacterName;
					if (subCharacterName != null)
					{
						resourceSceneLoader.AddLoad("CharacterModel" + subCharacterName, true, false);
						resourceSceneLoader.AddLoad("CharacterEffect" + subCharacterName, true, false);
					}
					BossType tutorialBossType = BossType.NONE;
					if (this.m_showMapBossTutorial)
					{
						tutorialBossType = this.m_bossType;
					}
					else if (this.m_showFeverBossTutorial)
					{
						tutorialBossType = this.m_bossType;
					}
					else if (this.m_tutorialStage)
					{
						tutorialBossType = BossType.FEVER;
						this.m_showFeverBossTutorial = true;
					}
					HudTutorial.Load(resourceSceneLoader, this.m_tutorialStage, this.m_bossStage, tutorialBossType, (CharaType)this.m_playerInformation.MainCharacterID, (CharaType)this.m_playerInformation.SubCharacterID);
				}
				SaveDataManager instance = SaveDataManager.Instance;
				if (instance != null)
				{
					bool onAssetBundle = true;
					int mainChaoID = instance.PlayerData.MainChaoID;
					int subChaoID = instance.PlayerData.SubChaoID;
					if (mainChaoID >= 0)
					{
						resourceSceneLoader.AddLoad("chao_" + mainChaoID.ToString("0000"), onAssetBundle, false);
					}
					if (subChaoID >= 0 && subChaoID != mainChaoID)
					{
						resourceSceneLoader.AddLoad("chao_" + subChaoID.ToString("0000"), onAssetBundle, false);
					}
				}
				StageAbilityManager.LoadAbilityDataTable(resourceSceneLoader);
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_sceneLoader != null && AtlasManager.Instance != null)
			{
				if (this.m_sceneLoader.GetComponent<ResourceSceneLoader>().Loaded && AtlasManager.Instance.IsLoadAtlas())
				{
					TimeProfiler.EndCountTime("GameModeStage:Load");
					this.RegisterAllResource();
					UnityEngine.Object.Destroy(this.m_sceneLoader);
					this.m_sceneLoader = null;
					AtlasManager.Instance.ReplaceAtlasForStage();
					this.m_stagePathManager.CreatePathObjectData();
					if (this.m_quickMode)
					{
						StageTimeManager stageTimeManager = GameObjectUtil.FindGameObjectComponent<StageTimeManager>("StageTimeManager");
						if (stageTimeManager != null)
						{
							stageTimeManager.SetTable();
						}
					}
					EventObjectTable.LoadSetup();
					EventSPStageObjectTable.LoadSetup();
					EventBossObjectTable.LoadSetup();
					EventBossParamTable.LoadSetup();
					EventCommonDataTable.LoadSetup();
					TimeProfiler.StartCountTime("GameModeStage:SetupStageBlocks");
					StageBlockManager component = this.m_stageBlockManager.GetComponent<StageBlockManager>();
					if (component != null)
					{
						component.Setup(this.m_bossStage);
						component.PauseTerrainPlacement(this.m_notPlaceTerrain);
					}
					TimeProfiler.EndCountTime("GameModeStage:SetupStageBlocks");
					ResourceManager instance2 = ResourceManager.Instance;
					GameObject gameObject = instance2.GetGameObject(ResourceCategory.TERRAIN_MODEL, TerrainXmlData.DataAssetName);
					if (gameObject != null)
					{
						TerrainXmlData component2 = gameObject.GetComponent<TerrainXmlData>();
						if (component2 != null)
						{
							if (this.m_levelInformation != null)
							{
								this.m_levelInformation.MoveTrapBooRand = component2.MoveTrapBooRand;
							}
							if (StageItemManager.Instance != null)
							{
								ItemTable itemTable = StageItemManager.Instance.GetItemTable();
								if (itemTable != null)
								{
									itemTable.Setup(component2);
								}
							}
							RareEnemyTable rareEnemyTable = this.GetRareEnemyTable();
							if (rareEnemyTable != null)
							{
								rareEnemyTable.Setup(component2);
							}
							EnemyExtendItemTable enemyExtendItemTable = this.GetEnemyExtendItemTable();
							if (enemyExtendItemTable != null)
							{
								enemyExtendItemTable.Setup(component2);
							}
							BossTable bossTable = this.GetBossTable();
							if (bossTable != null)
							{
								bossTable.Setup(component2);
							}
							BossMap3Table bossMap3Table = this.GetBossMap3Table();
							if (bossMap3Table != null)
							{
								bossMap3Table.Setup(component2);
							}
							ObjectPartTable objectPartTable = this.GetObjectPartTable();
							if (objectPartTable != null)
							{
								objectPartTable.Setup(component2);
							}
						}
					}
					if (this.m_characterContainer != null)
					{
						this.m_characterContainer.SetupCharacter();
					}
					StageComboManager instance3 = StageComboManager.Instance;
					if (instance3 != null)
					{
						instance3.Setup();
						instance3.SetComboTime(this.m_quickMode);
					}
					if (this.m_uiRootObj != null)
					{
						this.m_continueWindowObj = GameObjectUtil.FindChildGameObject(this.m_uiRootObj, "ContinueWindow");
						if (this.m_continueWindowObj != null)
						{
							HudContinue component3 = this.m_continueWindowObj.GetComponent<HudContinue>();
							if (component3 != null)
							{
								component3.Setup(this.m_bossStage);
							}
							this.m_continueWindowObj.SetActive(false);
						}
					}
					if (this.m_hudCaution != null)
					{
						this.m_hudCaution.SetBossWord(this.m_bossStage);
					}
					BossType bossType = BossType.NONE;
					if (this.m_bossStage)
					{
						bossType = this.m_bossType;
					}
					bool spCrystal = EventManager.Instance != null && EventManager.Instance.IsSpecialStage();
					bool animal = EventManager.Instance != null && EventManager.Instance.IsGetAnimalStage();
					GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnSetup", new MsgHudCockpitSetup(bossType, spCrystal, animal, !this.m_tutorialStage, this.m_firstTutorial), SendMessageOptions.DontRequireReceiver);
					if (FontManager.Instance != null)
					{
						FontManager.Instance.ReplaceFont();
					}
					if (StageEffectManager.Instance != null)
					{
						StageEffectManager.Instance.StockStageEffect(this.m_bossStage | this.m_tutorialStage);
					}
					if (AnimalResourceManager.Instance != null)
					{
						AnimalResourceManager.Instance.StockAnimalObject(this.m_bossStage | this.m_tutorialStage);
					}
					this.PlayStageEffect();
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateRequestActStart)));
				}
			}
			else
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateRequestActStart)));
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRequestActStart(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(3);
			}
			return TinyFsmState.End();
		case 1:
			this.RequestServerStartAct();
			return TinyFsmState.End();
		case 4:
			if (this.m_serverActEnd)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateSoundConnectIfNotFound)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSoundConnectIfNotFound(TinyFsmEvent e)
	{
		int signal = e.Signal;
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
			string text = null;
			string text2 = null;
			string cueSheetName = this.GetCueSheetName();
			if (!string.IsNullOrEmpty(cueSheetName))
			{
				text = cueSheetName + ".acb";
				text2 = cueSheetName + "_streamfiles.awb";
			}
			string downloadURL = SoundManager.GetDownloadURL();
			string downloadedDataPath = SoundManager.GetDownloadedDataPath();
			StreamingDataLoader instance = StreamingDataLoader.Instance;
			if (instance != null)
			{
				if (text != null)
				{
					instance.AddFileIfNotDownloaded(downloadURL + text, downloadedDataPath + text);
				}
				if (text2 != null)
				{
					instance.AddFileIfNotDownloaded(downloadURL + text2, downloadedDataPath + text2);
				}
				StageStreamingDataLoadRetryProcess process = new StageStreamingDataLoadRetryProcess(base.gameObject, this);
				NetMonitor.Instance.StartMonitor(process, 0f, HudNetworkConnect.DisplayType.ALL);
				instance.StartDownload(0, base.gameObject);
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			StreamingDataLoader instance2 = StreamingDataLoader.Instance;
			if (instance2 != null)
			{
				if (instance2.Loaded)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateAccessNetworkForStartAct)));
				}
			}
			else
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateAccessNetworkForStartAct)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAccessNetworkForStartAct(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			TimeProfiler.EndCountTime("GameModeStage:StateAccessNetworkForStartAct");
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(5);
			}
			return TinyFsmState.End();
		case 1:
			TimeProfiler.StartCountTime("GameModeStage:StateAccessNetworkForStartAct");
			return TinyFsmState.End();
		case 4:
			if (this.m_serverActEnd && this.m_stagePathManager.SetupEnd && this.m_stageBlockManager.GetComponent<StageBlockManager>().IsSetupEnded())
			{
				this.SetMainBGMName();
				SoundManager.AddStageCueSheet(this.GetCueSheetName());
				ObjUtil.CreateSharedMateriaDummyObject(ResourceCategory.OBJECT_RESOURCE, "obj_cmn_ring");
				ObjUtil.CreateSharedMateriaDummyObject(ResourceCategory.OBJECT_RESOURCE, "obj_cmn_movetrap");
				if (EventManager.Instance != null && EventManager.Instance.IsQuickEvent() && this.m_quickMode)
				{
					ObjUtil.CreateSharedMateriaDummyObject(ResourceCategory.EVENT_RESOURCE, "obj_sp_goldcoin");
					ObjUtil.CreateSharedMateriaDummyObject(ResourceCategory.EVENT_RESOURCE, "obj_sp_goldcoin10");
					ObjUtil.CreateSharedMateriaDummyObject(ResourceCategory.EVENT_RESOURCE, "obj_sp_pearl10");
				}
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateSetupPrepareBlock)));
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSetupPrepareBlock(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(6);
			}
			return TinyFsmState.End();
		case 1:
			if (this.m_stageBlockManager && this.m_playerInformation)
			{
				MsgPrepareStageReplace value = new MsgPrepareStageReplace(this.m_playerInformation.SpeedLevel, this.m_stageName);
				this.m_stageBlockManager.SendMessage("OnMsgPrepareStageReplace", value);
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_stageBlockManager.GetComponent<StageBlockManager>().IsSetupEnded())
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateSetupBlock)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSetupBlock(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			foreach (GameObject current in this.m_pausedObject)
			{
				if (current != null)
				{
					current.SetActive(true);
				}
			}
			this.m_pausedObject.Clear();
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(7);
			}
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (this.m_stageBlockManager && this.m_playerInformation)
			{
				TimeProfiler.StartCountTime("GameModeStage:SetupBlock");
				MsgStageReplace value = new MsgStageReplace(this.m_playerInformation.SpeedLevel, this.PlayerResetPosition, this.PlayerResetRotation, this.m_stageName);
				if (this.m_bossStage)
				{
					BossType bossType = this.m_bossType;
					MsgSetStageOnMapBoss value2 = new MsgSetStageOnMapBoss(this.PlayerResetPosition, this.PlayerResetRotation, this.m_stageName, bossType);
					this.m_stageBlockManager.SendMessage("OnMsgSetStageOnMapBoss", value2);
					if (this.m_levelInformation != null)
					{
						this.m_levelInformation.NowBoss = true;
					}
				}
				else if (this.m_tutorialStage)
				{
					this.m_stageBlockManager.GetComponent<StageBlockManager>().SetStageOnTutorial(this.PlayerResetPosition);
				}
				else
				{
					this.m_stageBlockManager.SendMessage("OnMsgStageReplace", value);
				}
				StageFarTerrainManager stageFarTerrainManager = GameObjectUtil.FindGameObjectComponent<StageFarTerrainManager>("StageFarManager");
				if (stageFarTerrainManager != null)
				{
					stageFarTerrainManager.SendMessage("OnMsgStageReplace", value);
				}
				TimeProfiler.EndCountTime("GameModeStage:SetupBlock");
			}
			if (!this.m_tutorialStage && !this.m_firstTutorial && this.m_missionManager != null)
			{
				this.m_missionManager.SetupMissions();
				this.m_missonCompleted = this.m_missionManager.Completed;
			}
			if (this.m_tutorialManager != null)
			{
				this.m_tutorialManager.SetupTutorial();
			}
			TimeProfiler.StartCountTime("GameModeStage:SetupFriendManager");
			if (this.m_friendSignManager != null && !this.m_bossStage)
			{
				this.m_friendSignManager.SetupFriendSignManager();
			}
			TimeProfiler.EndCountTime("GameModeStage:SetupFriendManager");
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloStageStart)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSendApolloStageStart(TinyFsmEvent e)
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
			if (this.m_progressBar != null)
			{
				this.m_progressBar.SetState(8);
			}
			return TinyFsmState.End();
		case 1:
		{
			ApolloTutorialIndex apolloStartTutorialIndex = this.GetApolloStartTutorialIndex();
			if (apolloStartTutorialIndex != ApolloTutorialIndex.NONE)
			{
				string[] value = new string[1];
				SendApollo.GetTutorialValue(apolloStartTutorialIndex, ref value);
				this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_START, value);
			}
			else
			{
				this.m_sendApollo = null;
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			bool flag = true;
			if (this.m_sendApollo != null && this.m_sendApollo.GetState() == SendApollo.State.Succeeded)
			{
				flag = false;
			}
			if (flag)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateFadeIn)));
			}
			return TinyFsmState.End();
		}
		case 5:
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
			if (this.m_connectAlertUI2 != null)
			{
				this.m_connectAlertUI2.SetActive(false);
			}
			return TinyFsmState.End();
		case 1:
		{
			GC.Collect();
			Resources.UnloadUnusedAssets();
			GC.Collect();
			SoundManager.BgmPlay(this.m_mainBgmName, "BGM", false);
			this.m_timer = 0.5f;
			this.SetChaoAblityTimeScale();
			this.SetDefaultTimeScale();
			HudLoading.EndScreen(null);
			MsgDisableInput value = new MsgDisableInput(true);
			GameObjectUtil.SendMessageToTagObjects("Player", "OnInputDisable", value, SendMessageOptions.DontRequireReceiver);
			return TinyFsmState.End();
		}
		case 4:
			this.m_timer -= e.GetDeltaTime;
			if (this.m_timer < 0f)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateGameStart)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGameStart(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
		{
			MsgDisableInput value = new MsgDisableInput(false);
			GameObjectUtil.SendMessageToTagObjects("Player", "OnInputDisable", value, SendMessageOptions.DontRequireReceiver);
			if (this.m_bossStage || ObjUtil.IsUseTemporarySet())
			{
				ObjUtil.SendStartItemAndChao();
			}
			return TinyFsmState.End();
		}
		case 1:
			if (this.m_bossStage)
			{
				if (this.m_eventManager != null && this.m_eventManager.IsRaidBossStage() && this.m_eventStage)
				{
					this.SendMessageToHudCaution(HudCaution.Type.EVENTBOSS);
				}
				else
				{
					this.SendMessageToHudCaution(HudCaution.Type.BOSS);
				}
				SoundManager.SePlay("sys_boss_warning", "SE");
			}
			else
			{
				this.SendMessageToHudCaution(HudCaution.Type.GO);
				SoundManager.SePlay("sys_go", "SE");
			}
			this.m_timer = 1f;
			return TinyFsmState.End();
		case 4:
			this.m_timer -= e.GetDeltaTime;
			if (this.m_timer < 0f)
			{
				BackKeyManager.StartScene();
				this.HudPlayerChangeCharaButton(true, false);
				if (this.m_chaoEasyTimeScale < 1f)
				{
					ObjUtil.RequestStartAbilityToChao(ChaoAbility.EASY_SPEED, false);
				}
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateNormal(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
		{
			bool pause = false;
			if (this.m_reqPause || this.m_reqTutorialPause)
			{
				pause = true;
			}
			this.HudPlayerChangeCharaButton(false, pause);
			this.m_reqPause = false;
			if (this.m_levelInformation != null)
			{
				this.m_levelInformation.RequestPause = this.m_reqPause;
			}
			this.EnablePause(false);
			return TinyFsmState.End();
		}
		case 1:
			this.EnablePause(true);
			if (this.m_quickMode && this.m_showQuickTurorial != -1)
			{
				GameObjectUtil.SendDelayedMessageToGameObject(base.gameObject, "OnMsgTutorialQuickMode", new MsgTutorialQuickMode((HudTutorial.Id)this.m_showQuickTurorial));
			}
			else if (this.m_showCharaTutorial != -1)
			{
				GameObjectUtil.SendDelayedMessageToGameObject(base.gameObject, "OnMsgTutorialChara", new MsgTutorialChara((HudTutorial.Id)this.m_showCharaTutorial));
			}
			this.m_reqPause = false;
			if (this.m_levelInformation != null)
			{
				this.m_levelInformation.RequestPause = this.m_reqPause;
			}
			this.m_reqTutorialPause = false;
			if (this.m_IsNowLastChanceHudCautionBoss)
			{
				this.SendMessageToHudCaution(HudCaution.Type.BOSS);
				SoundManager.SePlay("sys_boss_warning", "SE");
				this.m_IsNowLastChanceHudCautionBoss = false;
			}
			return TinyFsmState.End();
		case 4:
			if (this.IsEventTimeup())
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateGameOver)));
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			switch (num)
			{
			case 12339:
				if (this.m_showMapBossTutorial)
				{
					this.m_tutorialKind = HudTutorial.Kind.MAPBOSS;
					this.m_reqTutorialPause = true;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
				}
				else if (this.m_showEventBossTutorial)
				{
					this.m_tutorialKind = HudTutorial.Kind.EVENTBOSS;
					this.m_reqTutorialPause = true;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
				}
				return TinyFsmState.End();
			case 12340:
				if (this.m_showFeverBossTutorial)
				{
					this.m_tutorialKind = HudTutorial.Kind.FEVERBOSS;
					this.m_reqTutorialPause = true;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
				}
				return TinyFsmState.End();
			case 12341:
				if (!this.m_bossStage)
				{
					MsgTutorialItem msgTutorialItem = e.GetMessage as MsgTutorialItem;
					this.m_showItemTutorial = (int)msgTutorialItem.m_id;
					this.m_tutorialKind = HudTutorial.Kind.ITEM;
					this.m_reqTutorialPause = true;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
				}
				return TinyFsmState.End();
			case 12342:
				this.m_reqTutorialPause = true;
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateItemButtonTutorialPause)));
				return TinyFsmState.End();
			case 12343:
				this.m_tutorialKind = HudTutorial.Kind.CHARA;
				this.m_reqTutorialPause = true;
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
				return TinyFsmState.End();
			case 12344:
				if (!this.m_bossStage && !this.m_tutorialStage)
				{
					MsgTutorialAction msgTutorialAction = e.GetMessage as MsgTutorialAction;
					this.m_showActionTutorial = (int)msgTutorialAction.m_id;
					this.m_tutorialKind = HudTutorial.Kind.ACTION;
					this.m_reqTutorialPause = true;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
				}
				return TinyFsmState.End();
			case 12345:
				if (this.m_quickMode)
				{
					MsgTutorialQuickMode msgTutorialQuickMode = e.GetMessage as MsgTutorialQuickMode;
					this.m_showQuickTurorial = (int)msgTutorialQuickMode.m_id;
					this.m_tutorialKind = HudTutorial.Kind.QUICK;
					this.m_reqTutorialPause = true;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
				}
				return TinyFsmState.End();
			case 12346:
			case 12347:
			case 12348:
			case 12349:
			case 12352:
			case 12353:
			case 12354:
			case 12355:
			case 12356:
			case 12357:
				IL_1E4:
				switch (num)
				{
				case 12304:
				{
					MsgTransformPhantom msgTransformPhantom = e.GetMessage as MsgTransformPhantom;
					PhantomType type = msgTransformPhantom.m_type;
					string text = null;
					switch (type)
					{
					case PhantomType.LASER:
						text = "bgm_p_laser";
						break;
					case PhantomType.DRILL:
						text = "bgm_p_drill";
						break;
					case PhantomType.ASTEROID:
						text = "bgm_p_asteroid";
						break;
					}
					if (text != null && !this.m_bossStage)
					{
						SoundManager.BgmChange(this.m_mainBgmName, "BGM");
						SoundManager.BgmCrossFadePlay(text, "BGM_jingle", 0f);
					}
					return TinyFsmState.End();
				}
				case 12305:
					if (!this.m_bossStage)
					{
						SoundManager.BgmCrossFadeStop(1f, 1f);
					}
					return TinyFsmState.End();
				case 12306:
					if (this.m_levelInformation != null)
					{
						this.m_levelInformation.NowFeverBoss = true;
					}
					if (StageItemManager.Instance != null)
					{
						MsgPauseItemOnBoss msg = new MsgPauseItemOnBoss();
						StageItemManager.Instance.OnPauseItemOnBoss(msg);
					}
					this.SendBossStartMessageToChao();
					if (!this.m_playerInformation.IsNowLastChance())
					{
						this.SendMessageToHudCaution(HudCaution.Type.BOSS);
						SoundManager.SePlay("sys_boss_warning", "SE");
					}
					else
					{
						this.m_IsNowLastChanceHudCautionBoss = true;
					}
					if (this.m_quickMode && StageTimeManager.Instance != null)
					{
						StageTimeManager.Instance.Pause();
					}
					goto IL_9DF;
				case 12307:
				{
					MsgBossEnd msgBossEnd = e.GetMessage as MsgBossEnd;
					if (msgBossEnd == null)
					{
						goto IL_9DF;
					}
					if (this.m_bossStage)
					{
						if (this.m_levelInformation != null)
						{
							this.m_levelInformation.BossDestroy = msgBossEnd.m_dead;
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StatePrepareUpdateDatabase)));
						return TinyFsmState.End();
					}
					if (this.m_levelInformation != null)
					{
						this.m_levelInformation.InvalidExtreme = false;
						this.DrawingInvalidExtreme();
					}
					if (StageItemManager.Instance != null)
					{
						MsgPauseItemOnChageLevel msg2 = new MsgPauseItemOnChageLevel();
						StageItemManager.Instance.OnPauseItemOnChangeLevel(msg2);
					}
					GameObjectUtil.SendMessageToTagObjects("Chao", "OnPauseChangeLevel", null, SendMessageOptions.DontRequireReceiver);
					if (this.m_tutorialStage)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateEndFadeOut)));
					}
					else
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateChangeLevel)));
					}
					return TinyFsmState.End();
				}
				case 12308:
					if (this.m_bossStage)
					{
						this.m_bossClear = true;
					}
					goto IL_9DF;
				case 12309:
				case 12310:
				case 12311:
				case 12312:
					IL_219:
					switch (num)
					{
					case 12333:
						if (this.m_tutorialManager != null)
						{
							MsgTutorialPlayStart msgTutorialPlayStart = e.GetMessage as MsgTutorialPlayStart;
							if (msgTutorialPlayStart != null)
							{
								this.m_tutorialMissionID = msgTutorialPlayStart.m_eventID;
								this.m_tutorialKind = HudTutorial.Kind.MISSION;
								this.m_reqTutorialPause = true;
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialPause)));
							}
						}
						return TinyFsmState.End();
					case 12334:
						IL_232:
						if (num == 4096)
						{
							this.m_reqPauseBackMain = false;
							this.m_reqPause = true;
							if (this.m_levelInformation != null)
							{
								this.m_levelInformation.RequestPause = this.m_reqPause;
							}
							return TinyFsmState.End();
						}
						if (num == 12329)
						{
							MsgInvincible msgInvincible = e.GetMessage as MsgInvincible;
							if (msgInvincible != null)
							{
								if (msgInvincible.m_mode == MsgInvincible.Mode.Start)
								{
									SoundManager.ItemBgmCrossFadePlay("jingle_invincible", "BGM_jingle", 0f);
								}
								else
								{
									SoundManager.BgmCrossFadeStop(1f, 0.5f);
								}
							}
							return TinyFsmState.End();
						}
						if (num == 12361)
						{
							if (this.m_quickMode)
							{
								this.m_quickModeTimeUp = true;
								if (StageTimeManager.Instance != null)
								{
									StageTimeManager.Instance.Pause();
								}
								if (this.IsEnableContinue())
								{
									this.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckContinue)));
								}
								else
								{
									this.ChangeState(new TinyFsmState(new EventFunction(this.StateQuickModeTimeUp)));
								}
							}
							return TinyFsmState.End();
						}
						if (num != 20480)
						{
							goto IL_9DF;
						}
						if (!this.m_bossClear)
						{
							if (this.m_quickMode && StageTimeManager.Instance != null)
							{
								StageTimeManager.Instance.Pause();
							}
							if (this.IsEnableContinue())
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckContinue)));
							}
							else if (this.m_firstTutorial)
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloStageEnd)));
							}
							else if (this.m_tutorialStage)
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateEndFadeOut)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateGameOver)));
							}
						}
						return TinyFsmState.End();
					case 12335:
					{
						MsgTutorialPlayEnd tutorialEndMsg = e.GetMessage as MsgTutorialPlayEnd;
						this.m_tutorialEndMsg = tutorialEndMsg;
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateTutorialMissionEnd)));
						return TinyFsmState.End();
					}
					}
					goto IL_232;
				case 12313:
					if (this.m_levelInformation != null)
					{
						this.m_levelInformation.RequestCharaChange = true;
					}
					if (this.m_characterContainer != null)
					{
						MsgChangeChara msgChangeChara = e.GetMessage as MsgChangeChara;
						if (msgChangeChara != null)
						{
							this.m_characterContainer.SendMessage("OnMsgChangeChara", e.GetMessage);
						}
					}
					goto IL_9DF;
				}
				goto IL_219;
				IL_9DF:
				return TinyFsmState.End();
			case 12350:
				GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnPhantomActStart", e.GetMessage, SendMessageOptions.DontRequireReceiver);
				return TinyFsmState.End();
			case 12351:
				GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnPhantomActEnd", e.GetMessage, SendMessageOptions.DontRequireReceiver);
				return TinyFsmState.End();
			case 12358:
			{
				MsgExternalGamePause msgExternalGamePause = e.GetMessage as MsgExternalGamePause;
				this.m_reqPauseBackMain = msgExternalGamePause.m_backMainMenu;
				this.m_reqPause = true;
				if (this.m_levelInformation != null)
				{
					this.m_levelInformation.RequestPause = this.m_reqPause;
				}
				return TinyFsmState.End();
			}
			}
			goto IL_1E4;
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StatePause(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			StageDebugInformation.DestroyActivateButton();
			this.SetDefaultTimeScale();
			return TinyFsmState.End();
		case 1:
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnSetPause", new MsgSetPause(this.m_reqPauseBackMain, false), SendMessageOptions.DontRequireReceiver);
			this.m_reqPauseBackMain = false;
			this.SetTimeScale(0f);
			SoundManager.BgmPause(true);
			SoundManager.SePausePlaying(true);
			SoundManager.SePlay("sys_pause", "SE");
			StageDebugInformation.CreateActivateButton();
			GC.Collect();
			Resources.UnloadUnusedAssets();
			GC.Collect();
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num == 4097)
			{
				SoundManager.BgmPause(false);
				SoundManager.SePausePlaying(false);
				this.HudPlayerChangeCharaButton(true, true);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
				return TinyFsmState.End();
			}
			if (num == 4098)
			{
				this.m_retired = true;
				this.HoldPlayerAndDestroyTerrainOnEnd();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateUpdateDatabase)));
				return TinyFsmState.End();
			}
			if (num != 12358)
			{
				return TinyFsmState.End();
			}
			MsgExternalGamePause msgExternalGamePause = e.GetMessage as MsgExternalGamePause;
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnSetPause", new MsgSetPause(msgExternalGamePause.m_backMainMenu, msgExternalGamePause.m_backKey), SendMessageOptions.DontRequireReceiver);
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateChangeLevel(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			this.m_onSpeedUp = false;
			this.m_onDestroyRingMode = false;
			return TinyFsmState.End();
		case 1:
			this.m_timer = 0.4f;
			this.SendMessageToHudCaution(HudCaution.Type.STAGE_OUT);
			SoundManager.SePlay("boss_scene_change", "SE");
			this.m_substate = 0;
			return TinyFsmState.End();
		case 4:
			switch (this.m_substate)
			{
			case 0:
				this.m_timer -= e.GetDeltaTime;
				if (this.m_timer <= 0f)
				{
					GC.Collect();
					Resources.UnloadUnusedAssets();
					GC.Collect();
					StageScoreManager instance = StageScoreManager.Instance;
					if (instance != null)
					{
						StageScorePool scorePool = instance.ScorePool;
						if (scorePool != null)
						{
							scorePool.AddScore(ScoreType.distance, (int)this.m_playerInformation.TotalDistance);
							scorePool.CheckHalfWay();
						}
					}
					MsgPLHold value = new MsgPLHold();
					GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgPLHold", value, SendMessageOptions.DontRequireReceiver);
					this.ResetPosStageEffect(true);
					if (TenseEffectManager.Instance)
					{
						TenseEffectManager.Instance.FlipTenseType();
					}
					if (this.m_stageBlockManager != null)
					{
						StageBlockManager component = this.m_stageBlockManager.GetComponent<StageBlockManager>();
						if (component != null)
						{
							component.ReCreateTerrain();
						}
					}
					this.m_timer = 0.6f;
					this.m_counter = 2;
					this.m_substate = 1;
				}
				break;
			case 1:
				this.m_timer -= e.GetDeltaTime;
				this.m_counter--;
				if (this.m_timer <= 0f && this.m_counter < 0)
				{
					PlayerSpeed speedLevel = (PlayerSpeed)Mathf.Min((int)(this.m_playerInformation.SpeedLevel + 1), 2);
					MsgPrepareStageReplace value2 = new MsgPrepareStageReplace(speedLevel, this.m_stageName);
					this.m_stageBlockManager.SendMessage("OnMsgPrepareStageReplace", value2);
					ObjUtil.PauseCombo(MsgPauseComboTimer.State.PAUSE, -1f);
					this.m_substate = 2;
				}
				break;
			case 2:
				if (this.m_playerInformation)
				{
					bool flag = false;
					if (this.m_stageBlockManager != null)
					{
						flag = this.m_stageBlockManager.GetComponent<StageBlockManager>().IsBlockLevelUp();
					}
					switch (this.m_playerInformation.SpeedLevel)
					{
					case PlayerSpeed.LEVEL_1:
						if (flag)
						{
							this.m_onSpeedUp = true;
							this.SendPlayerSpeedLevel(PlayerSpeed.LEVEL_2);
						}
						break;
					case PlayerSpeed.LEVEL_2:
						if (flag)
						{
							this.m_onSpeedUp = true;
							this.SendPlayerSpeedLevel(PlayerSpeed.LEVEL_3);
						}
						break;
					case PlayerSpeed.LEVEL_3:
						if (flag)
						{
							this.m_onDestroyRingMode = true;
							if (this.m_levelInformation != null)
							{
								this.m_levelInformation.Extreme = true;
							}
						}
						break;
					}
				}
				this.m_counter = 3;
				this.m_substate = 3;
				break;
			case 3:
				if (this.m_counter > 0)
				{
					this.m_counter--;
				}
				if (this.m_counter <= 0 && this.m_stageBlockManager.GetComponent<StageBlockManager>().IsSetupEnded())
				{
					this.m_substate = 4;
				}
				break;
			case 4:
			{
				if (this.m_playerInformation)
				{
					PlayerSpeed speedLevel2 = this.m_playerInformation.SpeedLevel;
					MsgStageReplace msgStageReplace = new MsgStageReplace(speedLevel2, this.PlayerResetPosition, this.PlayerResetRotation, this.m_stageName);
					GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgStageReplace", msgStageReplace, SendMessageOptions.RequireReceiver);
					GameObjectUtil.SendMessageToTagObjects("Chao", "OnMsgStageReplace", msgStageReplace, SendMessageOptions.DontRequireReceiver);
					if (this.m_stageBlockManager != null)
					{
						StageBlockManager component2 = this.m_stageBlockManager.GetComponent<StageBlockManager>();
						if (component2 != null)
						{
							component2.OnMsgStageReplace(msgStageReplace);
						}
					}
					StageFarTerrainManager stageFarTerrainManager = GameObjectUtil.FindGameObjectComponent<StageFarTerrainManager>("StageFarManager");
					if (stageFarTerrainManager != null)
					{
						stageFarTerrainManager.SendMessage("OnMsgStageReplace", msgStageReplace, SendMessageOptions.DontRequireReceiver);
					}
				}
				if (this.m_levelInformation != null)
				{
					this.m_levelInformation.NowFeverBoss = false;
				}
				HudCockpit hudCockpit = GameObjectUtil.FindGameObjectComponent<HudCockpit>("HudCockpit");
				if (hudCockpit != null)
				{
					MsgBossEnd value3 = new MsgBossEnd(true);
					hudCockpit.SendMessage("OnBossEnd", value3);
				}
				this.m_counter = 3;
				this.m_substate = 5;
				break;
			}
			case 5:
				if (--this.m_counter < 0)
				{
					MsgStageRestart value4 = new MsgStageRestart();
					GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgStageRestart", value4, SendMessageOptions.RequireReceiver);
					this.SendMessageToHudCaution(HudCaution.Type.STAGE_IN);
					this.m_substate = 6;
					this.m_timer = 0.5f;
				}
				break;
			case 6:
				this.m_timer -= e.GetDeltaTime;
				if (this.m_timer <= 0f)
				{
					this.ResetPosStageEffect(false);
					if (this.m_onSpeedUp || this.m_onDestroyRingMode || this.m_invalidExtremeFlag)
					{
						bool flag2 = false;
						if (this.m_onSpeedUp)
						{
							this.SendMessageToHudCaution(HudCaution.Type.SPEEDUP);
						}
						else
						{
							if (this.m_levelInformation != null && this.m_levelInformation.InvalidExtreme)
							{
								flag2 = true;
								if (this.m_levelInformation != null && this.m_levelInformation.InvalidExtreme)
								{
									ObjUtil.RequestStartAbilityToChao(ChaoAbility.INVALIDI_EXTREME_STAGE, false);
								}
							}
							if (!flag2)
							{
								this.m_invalidExtremeFlag = false;
								this.SendMessageToHudCaution(HudCaution.Type.EXTREMEMODE);
							}
						}
						if (!flag2)
						{
							SoundManager.SePlay("sys_speedup", "SE");
						}
					}
					if (this.m_tutorialStage && this.m_playerInformation.SpeedLevel == PlayerSpeed.LEVEL_2)
					{
						if (StageItemManager.Instance != null)
						{
							MsgUseEquipItem msg = new MsgUseEquipItem();
							StageItemManager.Instance.OnUseEquipItem(msg);
						}
						if (this.m_levelInformation != null)
						{
							this.m_levelInformation.NowTutorial = false;
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
					}
					else
					{
						this.HudPlayerChangeCharaButton(true, false);
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
					}
					return TinyFsmState.End();
				}
				break;
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void SendPlayerSpeedLevel(PlayerSpeed speedLevel)
	{
		MsgUpSpeedLevel msgUpSpeedLevel = new MsgUpSpeedLevel(speedLevel);
		if (msgUpSpeedLevel != null)
		{
			GameObjectUtil.SendMessageToTagObjects("Player", "OnUpSpeedLevel", msgUpSpeedLevel, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendMessageFindGameObject("PlayerInformation", "OnUpSpeedLevel", msgUpSpeedLevel, SendMessageOptions.DontRequireReceiver);
		}
	}

	private HudTutorial.Id GetHudTutorialID(HudTutorial.Kind kind)
	{
		switch (kind)
		{
		case HudTutorial.Kind.MISSION:
			return (HudTutorial.Id)this.m_tutorialMissionID;
		case HudTutorial.Kind.MISSION_END:
			return HudTutorial.Id.MISSION_END;
		case HudTutorial.Kind.FEVERBOSS:
			return HudTutorial.Id.FEVERBOSS;
		case HudTutorial.Kind.MAPBOSS:
			return HudTutorial.Id.MAPBOSS_1 + BossTypeUtil.GetIndexNumber(this.m_bossType);
		case HudTutorial.Kind.EVENTBOSS:
			return HudTutorial.Id.EVENTBOSS_1 + BossTypeUtil.GetIndexNumber(this.m_bossType);
		case HudTutorial.Kind.ITEM:
			return (HudTutorial.Id)this.m_showItemTutorial;
		case HudTutorial.Kind.CHARA:
			return (HudTutorial.Id)this.m_showCharaTutorial;
		case HudTutorial.Kind.ACTION:
			return (HudTutorial.Id)this.m_showActionTutorial;
		case HudTutorial.Kind.QUICK:
			return (HudTutorial.Id)this.m_showQuickTurorial;
		}
		return HudTutorial.Id.NONE;
	}

	private void EndTutorial(HudTutorial.Kind kind)
	{
		switch (kind)
		{
		case HudTutorial.Kind.MISSION_END:
			if (StageItemManager.Instance != null)
			{
				MsgUseEquipItem msg = new MsgUseEquipItem();
				StageItemManager.Instance.OnUseEquipItem(msg);
			}
			if (this.m_levelInformation != null)
			{
				this.m_levelInformation.NowTutorial = false;
			}
			break;
		case HudTutorial.Kind.FEVERBOSS:
			this.SetEndBossTutorialFlag(SystemData.FlagStatus.TUTORIAL_FEVER_BOSS);
			this.m_showFeverBossTutorial = false;
			break;
		case HudTutorial.Kind.MAPBOSS:
			switch (this.m_bossType)
			{
			case BossType.MAP1:
				this.SetEndBossTutorialFlag(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_1);
				break;
			case BossType.MAP2:
				this.SetEndBossTutorialFlag(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_2);
				break;
			case BossType.MAP3:
				this.SetEndBossTutorialFlag(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_3);
				break;
			}
			break;
		case HudTutorial.Kind.ITEM:
			if (this.m_showItemTutorial != -1)
			{
				ItemType itemType = ItemType.UNKNOWN;
				for (int i = 0; i < 8; i++)
				{
					HudTutorial.Id itemTutorialID = ItemTypeName.GetItemTutorialID((ItemType)i);
					if (itemTutorialID == (HudTutorial.Id)this.m_showItemTutorial)
					{
						itemType = (ItemType)i;
						break;
					}
				}
				if (itemType != ItemType.UNKNOWN)
				{
					this.SetEndItemTutorialFlag(ItemTypeName.GetItemTutorialStatus(itemType));
				}
				this.m_showItemTutorial = -1;
			}
			break;
		case HudTutorial.Kind.CHARA:
			if (this.m_showCharaTutorial != -1)
			{
				CharaType commonTextCharaName = HudTutorial.GetCommonTextCharaName((HudTutorial.Id)this.m_showCharaTutorial);
				if (commonTextCharaName != CharaType.UNKNOWN)
				{
					this.SetEndCharaTutorialFlag(CharaTypeUtil.GetCharacterSaveDataFlagStatus(commonTextCharaName));
				}
				this.m_showCharaTutorial = -1;
			}
			break;
		case HudTutorial.Kind.ACTION:
			if (this.m_showActionTutorial != -1)
			{
				this.SetEndActionTutorialFlag(HudTutorial.GetActionTutorialSaveFlag((HudTutorial.Id)this.m_showActionTutorial));
				this.m_showActionTutorial = -1;
			}
			break;
		case HudTutorial.Kind.QUICK:
			if (this.m_showQuickTurorial != -1)
			{
				this.SetEndQuickModeTutorialFlag(HudTutorial.GetQuickModeTutorialSaveFlag((HudTutorial.Id)this.m_showQuickTurorial));
				this.m_showQuickTurorial = -1;
			}
			break;
		}
	}

	private void SetEndBossTutorialFlag(SystemData.FlagStatus flagStatus)
	{
		if (flagStatus != SystemData.FlagStatus.NONE)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null && !systemdata.IsFlagStatus(flagStatus))
				{
					systemdata.SetFlagStatus(flagStatus, true);
					this.m_saveFlag = true;
				}
			}
		}
	}

	private void SetEndItemTutorialFlag(SystemData.ItemTutorialFlagStatus flagStatus)
	{
		if (flagStatus != SystemData.ItemTutorialFlagStatus.NONE)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null && !systemdata.IsFlagStatus(flagStatus))
				{
					systemdata.SetFlagStatus(flagStatus, true);
					this.m_saveFlag = true;
				}
			}
		}
	}

	private void SetEndCharaTutorialFlag(SystemData.CharaTutorialFlagStatus flagStatus)
	{
		if (flagStatus != SystemData.CharaTutorialFlagStatus.NONE)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null && !systemdata.IsFlagStatus(flagStatus))
				{
					systemdata.SetFlagStatus(flagStatus, true);
					this.m_saveFlag = true;
				}
			}
		}
	}

	private void SetEndActionTutorialFlag(SystemData.ActionTutorialFlagStatus flagStatus)
	{
		if (flagStatus != SystemData.ActionTutorialFlagStatus.NONE)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null && !systemdata.IsFlagStatus(flagStatus))
				{
					systemdata.SetFlagStatus(flagStatus, true);
					this.m_saveFlag = true;
				}
			}
		}
	}

	private void SetEndQuickModeTutorialFlag(SystemData.QuickModeTutorialFlagStatus flagStatus)
	{
		if (flagStatus != SystemData.QuickModeTutorialFlagStatus.NONE)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null && !systemdata.IsFlagStatus(flagStatus))
				{
					systemdata.SetFlagStatus(flagStatus, true);
					this.m_saveFlag = true;
				}
			}
		}
	}

	private TinyFsmState StateTutorialPause(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			this.SetDefaultTimeScale();
			SoundManager.SePausePlaying(false);
			return TinyFsmState.End();
		case 1:
		{
			this.SetTimeScale(0f);
			HudTutorial.Id hudTutorialID = this.GetHudTutorialID(this.m_tutorialKind);
			HudTutorial.StartTutorial(hudTutorialID, this.m_bossType);
			SoundManager.SePausePlaying(true);
			SoundManager.SePlay("sys_pause", "SE");
			return TinyFsmState.End();
		}
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num == 12334)
			{
				this.EndTutorial(this.m_tutorialKind);
				this.HudPlayerChangeCharaButton(true, true);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
				return TinyFsmState.End();
			}
			if (num != 12349)
			{
				return TinyFsmState.End();
			}
			HudTutorial.PushBackKey();
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateItemButtonTutorialPause(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			this.SetDefaultTimeScale();
			SoundManager.SePausePlaying(false);
			return TinyFsmState.End();
		case 1:
			this.SetTimeScale(0f);
			SoundManager.SePausePlaying(true);
			SoundManager.SePlay("sys_pause", "SE");
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			if (iD == 12331)
			{
				this.HudPlayerChangeCharaButton(true, true);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateTutorialMissionEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			this.m_tutorialEndMsg = null;
			if (StageTutorialManager.Instance)
			{
				GameObjectUtil.SendDelayedMessageToGameObject(StageTutorialManager.Instance.gameObject, "OnMsgTutorialNext", new MsgTutorialNext());
			}
			return TinyFsmState.End();
		case 1:
			if (this.m_tutorialEndMsg != null)
			{
				if (this.m_tutorialEndMsg.m_retry)
				{
					HudTutorial.RetryTutorial();
				}
				else
				{
					HudTutorial.SuccessTutorial();
				}
				if (!this.m_tutorialEndMsg.m_complete)
				{
					this.m_timer = 1f;
					this.m_substate = 0;
				}
				else
				{
					this.m_substate = 4;
				}
			}
			return TinyFsmState.End();
		case 4:
			switch (this.m_substate)
			{
			case 0:
				this.m_timer -= e.GetDeltaTime;
				if (this.m_timer <= 0f)
				{
					if (!this.m_tutorialEndMsg.m_complete)
					{
						CameraFade.StartAlphaFade(Color.white, false, 1f);
						this.m_timer = 1f;
					}
					else
					{
						this.m_timer = 0f;
					}
					this.m_substate = 1;
				}
				break;
			case 1:
				this.m_timer -= e.GetDeltaTime;
				if (this.m_timer <= 0f)
				{
					if (this.m_tutorialEndMsg != null)
					{
						if (!this.m_tutorialEndMsg.m_complete)
						{
							MsgWarpPlayer value = new MsgWarpPlayer(this.m_tutorialEndMsg.m_pos, this.PlayerResetRotation, true);
							GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgWarpPlayer", value, SendMessageOptions.DontRequireReceiver);
						}
						if (this.m_tutorialEndMsg.m_retry)
						{
							bool blink = true;
							if (this.m_tutorialEndMsg.m_nextEventID <= Tutorial.EventID.JUMP)
							{
								blink = false;
							}
							MsgTutorialResetForRetry value2 = new MsgTutorialResetForRetry(this.m_tutorialEndMsg.m_pos, this.PlayerResetRotation, blink);
							GameObjectUtil.SendMessageFindGameObject("StageBlockManager", "OnMsgTutorialResetForRetry", value2, SendMessageOptions.DontRequireReceiver);
							GameObjectUtil.SendMessageFindGameObject("StageTutorialManager", "OnMsgTutorialResetForRetry", value2, SendMessageOptions.DontRequireReceiver);
							GameObjectUtil.SendMessageToTagObjects("MainCamera", "OnMsgTutorialResetForRetry", value2, SendMessageOptions.DontRequireReceiver);
						}
					}
					GameObject[] array = GameObject.FindGameObjectsWithTag("Animal");
					GameObject[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						GameObject gameObject = array2[i];
						gameObject.SendMessage("OnDestroyAnimal", SendMessageOptions.DontRequireReceiver);
					}
					ObjAnimalBase.DestroyAnimalEffect();
					ObjUtil.StopCombo();
					HudTutorial.EndTutorial();
					this.m_counter = 4;
					this.m_substate = 2;
				}
				break;
			case 2:
				if (--this.m_counter < 0)
				{
					MsgPLReleaseHold value3 = new MsgPLReleaseHold();
					GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgPLReleaseHold", value3, SendMessageOptions.DontRequireReceiver);
					MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.COME_IN);
					if (!this.m_tutorialEndMsg.m_complete)
					{
						this.m_timer = 1f;
						CameraFade.StartAlphaFade(Color.white, true, 1f);
					}
					else
					{
						this.m_timer = 0f;
					}
					this.m_substate = 3;
				}
				ObjAnimalBase.DestroyAnimalEffect();
				break;
			case 3:
				this.m_timer -= e.GetDeltaTime;
				if (this.m_timer <= 0f)
				{
					this.m_substate = 4;
					return TinyFsmState.End();
				}
				break;
			case 4:
				this.HudPlayerChangeCharaButton(true, false);
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num != 12334)
			{
				return TinyFsmState.End();
			}
			if (this.m_substate == 0)
			{
				CameraFade.StartAlphaFade(Color.white, false, 1f);
				this.m_timer = 1f;
				this.m_substate = 1;
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGameOver(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_gameResultTimer = 1f;
			return TinyFsmState.End();
		case 4:
			this.m_gameResultTimer -= Time.deltaTime;
			if (this.m_gameResultTimer < 0f)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StatePrepareUpdateDatabase)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCheckContinue(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			this.SetDefaultTimeScale();
			return TinyFsmState.End();
		case 1:
			this.m_gameResultTimer = 1f;
			this.SetTimeScale(0f);
			ObjUtil.SetHudStockRingEffectOff(true);
			SoundManager.BgmChange(this.m_mainBgmName, "BGM");
			SoundManager.BgmCrossFadePlay("bgm_continue", "BGM_jingle", 0f);
			if (this.m_continueWindowObj != null)
			{
				HudContinue component = this.m_continueWindowObj.GetComponent<HudContinue>();
				if (component != null)
				{
					if (this.m_quickMode)
					{
						component.SetTimeUp(this.m_quickModeTimeUp);
					}
					this.m_continueWindowObj.SetActive(true);
					component.PlayStart();
				}
			}
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			switch (num)
			{
			case 12352:
			{
				MsgContinueResult msgContinueResult = e.GetMessage as MsgContinueResult;
				if (msgContinueResult != null)
				{
					if (!msgContinueResult.m_result)
					{
						SoundManager.BgmStop();
						this.ChangeState(new TinyFsmState(new EventFunction(this.StatePrepareUpdateDatabase)));
						return TinyFsmState.End();
					}
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePrepareContinue)));
				}
				goto IL_1FA;
			}
			case 12353:
			{
				IL_100:
				if (num != 12329)
				{
					goto IL_1FA;
				}
				MsgInvincible msgInvincible = e.GetMessage as MsgInvincible;
				if (msgInvincible != null && msgInvincible.m_mode == MsgInvincible.Mode.Start)
				{
					SoundManager.ItemBgmCrossFadePlay("jingle_invincible", "BGM_jingle", 0f);
					this.m_receiveInvincibleMsg = true;
				}
				goto IL_1FA;
			}
			case 12354:
			{
				MsgContinueBackKey msgContinueBackKey = e.GetMessage as MsgContinueBackKey;
				if (msgContinueBackKey != null && this.m_continueWindowObj != null)
				{
					HudContinue component2 = this.m_continueWindowObj.GetComponent<HudContinue>();
					if (component2 != null)
					{
						component2.PushBackKey();
					}
				}
				goto IL_1FA;
			}
			}
			goto IL_100;
			IL_1FA:
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StatePrepareContinue(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			ObjUtil.SetHudStockRingEffectOff(false);
			return TinyFsmState.End();
		case 1:
		{
			this.m_numEnableContinue--;
			MsgPrepareContinue value = new MsgPrepareContinue(this.m_bossStage, this.m_quickModeTimeUp);
			GameObjectUtil.SendMessageFindGameObject("CharacterContainer", "OnMsgPrepareContinue", value, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgPrepareContinue", value, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnMsgPrepareContinue", value, SendMessageOptions.DontRequireReceiver);
			if (this.m_quickMode && StageTimeManager.Instance != null)
			{
				StageTimeManager.Instance.ExtendTime(StageTimeManager.ExtendPattern.CONTINUE);
			}
			if (this.m_continueWindowObj != null)
			{
				this.m_continueWindowObj.SetActive(false);
			}
			if (this.m_bossStage && this.m_levelInformation != null)
			{
				this.m_levelInformation.DistanceToBossOnStart = this.m_levelInformation.DistanceOnStage;
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_quickMode)
			{
				this.m_quickModeTimeUp = false;
				if (StageTimeManager.Instance != null)
				{
					StageTimeManager.Instance.PlayStart();
				}
			}
			if (!this.m_receiveInvincibleMsg)
			{
				SoundManager.BgmCrossFadeStop(0f, 1f);
			}
			this.m_receiveInvincibleMsg = false;
			this.HudPlayerChangeCharaButton(true, true);
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateNormal)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateQuickModeTimeUp(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_timer = 0.4f;
			this.SendMessageToHudCaution(HudCaution.Type.STAGE_OUT);
			SoundManager.BgmStop();
			if (this.m_levelInformation != null)
			{
				this.m_levelInformation.RequestPause = this.m_reqPause;
			}
			GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgPLHold", new MsgPLHold(), SendMessageOptions.DontRequireReceiver);
			this.m_substate = 0;
			return TinyFsmState.End();
		case 4:
			this.m_timer -= e.GetDeltaTime;
			if (this.m_timer <= 0f)
			{
				GC.Collect();
				Resources.UnloadUnusedAssets();
				GC.Collect();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StatePrepareUpdateDatabase)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StatePrepareUpdateDatabase(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			ObjUtil.SetHudStockRingEffectOff(true);
			ObjUtil.SendMessageScoreCheck(new StageScoreData(10, (int)this.m_playerInformation.TotalDistance));
			ObjUtil.SendMessageFinalScoreBeforeResult();
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			GameObject[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject = array2[i];
				gameObject.SetActive(false);
			}
			bool flag = false;
			if (this.m_levelInformation != null)
			{
				flag = this.m_levelInformation.BossDestroy;
			}
			if (this.m_gameResult != null)
			{
				this.m_gameResult.gameObject.SetActive(true);
				SaveDataUtil.ReflctResultsData();
				bool isNoMiss = this.EnableChaoEgg();
				bool isBossTutorialClear = this.IsBossTutorialClear();
				this.m_gameResult.PlayBGStart((!this.m_bossStage) ? GameResult.ResultType.NORMAL : GameResult.ResultType.BOSS, isNoMiss, isBossTutorialClear, flag, this.GetEventResultState());
			}
			SoundManager.BgmStop();
			if (this.m_bossStage && flag)
			{
				SoundManager.BgmPlay("jingle_sys_bossclear", "BGM_jingle", false);
			}
			else
			{
				SoundManager.BgmPlay("jingle_sys_clear", "BGM_jingle", false);
			}
			this.m_timer = 0.5f;
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_timer > 0f)
			{
				this.m_timer -= e.GetDeltaTime;
				if (this.m_timer <= 0f)
				{
					this.HoldPlayerAndDestroyTerrainOnEnd();
					GameObjectUtil.SendDelayedMessageFindGameObject("HudCockpit", "OnMsgExitStage", new MsgExitStage());
					if (EventManager.Instance.IsRaidBossStage())
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateUpdateRaidBossState)));
					}
					else if (this.m_quickMode)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateUpdateQuickModeDatabase)));
					}
					else
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateUpdateDatabase)));
					}
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateUpdateDatabase(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_counter = 0;
			if (this.m_fromTitle || this.m_firstTutorial)
			{
				this.m_retired = true;
			}
			this.m_exitFromResult = !this.m_retired;
			if (this.m_exitFromResult)
			{
				this.SetMissionResult();
			}
			EventResultState eventResultState = this.GetEventResultState();
			EventManager instance = EventManager.Instance;
			bool flag;
			bool flag2;
			if (eventResultState == EventResultState.TIMEUP)
			{
				if (instance.IsCollectEvent())
				{
					flag = true;
					flag2 = false;
				}
				else
				{
					flag = false;
					flag2 = false;
				}
			}
			else
			{
				flag = true;
				flag2 = true;
			}
			if (!flag || this.m_firstTutorial)
			{
				base.StartCoroutine(this.NotSendPostGameResult());
			}
			else
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null && SaveDataManager.Instance != null)
				{
					bool chaoEggPresent = this.EnableChaoEgg();
					int? eventId = null;
					if (flag2 && (instance.EventStage || instance.IsCollectEvent()))
					{
						eventId = new int?(0);
						eventId = new int?(instance.Id);
					}
					long? eventValue = null;
					if (eventId.HasValue)
					{
						StageScoreManager instance2 = StageScoreManager.Instance;
						if (instance2 != null)
						{
							eventValue = new long?(0L);
							if (instance.IsCollectEvent())
							{
								eventValue = new long?(instance2.CollectEventCount);
							}
							else
							{
								eventValue = new long?(instance2.FinalCountData.sp_crystal);
							}
						}
					}
					ServerGameResults serverGameResults = new ServerGameResults(!this.m_exitFromResult, this.m_tutorialStage, chaoEggPresent, this.m_bossStage, this.m_oldNumBossAttack, eventId, eventValue);
					if (serverGameResults != null)
					{
						if (this.m_postGameResults.m_prevMapInfo != null)
						{
							serverGameResults.SetMapProgress(this.m_postGameResults.m_prevMapInfo.m_mapState, ref this.m_postGameResults.m_prevMapInfo.m_pointScore, this.m_postGameResults.m_existBoss);
						}
						loggedInServerInterface.RequestServerPostGameResults(serverGameResults, base.gameObject);
					}
				}
				else
				{
					this.m_counter++;
				}
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_counter > 0 && AchievementManager.IsRequestEnd())
			{
				if (this.m_saveFlag)
				{
					SystemSaveManager instance3 = SystemSaveManager.Instance;
					if (instance3 != null)
					{
						instance3.SaveSystemData();
					}
				}
				if (this.m_retired)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloStageEnd)));
				}
				else if (this.IsRaidBossStateUpdate())
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateEventDrawRaidBoss)));
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateResult)));
				}
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num == 61449)
			{
				if (ServerInterface.LoggedInServerInterface != null)
				{
					ServerTickerInfo tickerInfo = ServerInterface.TickerInfo;
					if (tickerInfo != null)
					{
						tickerInfo.ExistNewData = true;
					}
				}
				AchievementManager.RequestUpdate();
				if (this.m_exitFromResult && this.IsBossTutorialLose())
				{
					this.SetBossTutorialPresent();
				}
				this.m_counter++;
				return TinyFsmState.End();
			}
			if (num != 61517)
			{
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateUpdateQuickModeDatabase(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_counter = 0;
			this.m_exitFromResult = !this.m_retired;
			if (!this.m_exitFromResult)
			{
				base.StartCoroutine(this.NotSendPostGameResult());
			}
			else
			{
				this.SetMissionResult();
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null && SaveDataManager.Instance != null)
				{
					if (StageTimeManager.Instance != null)
					{
						StageTimeManager.Instance.CheckResultTimer();
					}
					ServerQuickModeGameResults serverQuickModeGameResults = new ServerQuickModeGameResults(!this.m_exitFromResult);
					if (serverQuickModeGameResults != null)
					{
						loggedInServerInterface.RequestServerQuickModePostGameResults(serverQuickModeGameResults, base.gameObject);
					}
				}
				else
				{
					this.m_counter++;
				}
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_counter > 0 && AchievementManager.IsRequestEnd())
			{
				if (this.m_saveFlag)
				{
					SystemSaveManager instance = SystemSaveManager.Instance;
					if (instance != null)
					{
						instance.SaveSystemData();
					}
				}
				if (this.m_retired)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloStageEnd)));
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateDailyBattleResult)));
				}
			}
			return TinyFsmState.End();
		case 5:
			switch (e.GetMessage.ID)
			{
			case 61514:
				if (ServerInterface.LoggedInServerInterface != null)
				{
					ServerTickerInfo tickerInfo = ServerInterface.TickerInfo;
					if (tickerInfo != null)
					{
						tickerInfo.ExistNewData = true;
					}
				}
				AchievementManager.RequestUpdate();
				this.m_counter++;
				return TinyFsmState.End();
			case 61517:
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDailyBattleResult(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_counter = 0;
			if (ServerInterface.LoggedInServerInterface != null)
			{
				if (SingletonGameObject<DailyBattleManager>.Instance != null && !this.m_bossStage)
				{
					SingletonGameObject<DailyBattleManager>.Instance.ResultSetup(new DailyBattleManager.CallbackSetup(this.DailyBattleResultCallBack));
				}
				else
				{
					this.m_counter++;
				}
			}
			else
			{
				this.m_counter++;
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_counter > 0)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateResult)));
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num != 61515)
			{
				return TinyFsmState.End();
			}
			this.m_counter++;
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEventDrawRaidBoss(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_counter = 0;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				long score = 0L;
				StageScoreManager instance = StageScoreManager.Instance;
				if (instance != null)
				{
					score = instance.FinalScore;
				}
				loggedInServerInterface.RequestServerDrawRaidBoss(EventManager.Instance.Id, score, base.gameObject);
			}
			else
			{
				this.m_counter++;
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_counter > 0)
			{
				if (EventUtility.CheckRaidbossEntry())
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateEventGetEventUserRaidBoss)));
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateResult)));
				}
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num != 61511)
			{
				return TinyFsmState.End();
			}
			this.m_counter++;
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEventGetEventUserRaidBoss(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_counter = 0;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerGetEventUserRaidBossState(EventManager.Instance.Id, base.gameObject);
			}
			else
			{
				this.m_counter++;
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_counter > 0)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateResult)));
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num != 61506)
			{
				return TinyFsmState.End();
			}
			this.m_counter++;
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateUpdateRaidBossState(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_counter = 0;
			if (this.m_fromTitle)
			{
				this.m_retired = true;
			}
			if (this.GetEventResultState() == EventResultState.TIMEUP)
			{
				base.StartCoroutine(this.NotSendEventUpdateGameResult());
			}
			else
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					long eventValue = 0L;
					long id = RaidBossInfo.currentRaidData.id;
					ServerEventGameResults serverEventGameResults = new ServerEventGameResults(this.m_retired, EventManager.Instance.Id, eventValue, id);
					if (serverEventGameResults != null)
					{
						DailyMissionData dailyMission = SaveDataManager.Instance.PlayerData.DailyMission;
						if (this.m_missionManager != null)
						{
							serverEventGameResults.m_dailyMissionComplete = this.m_missionManager.Completed;
						}
						else
						{
							serverEventGameResults.m_dailyMissionComplete = false;
						}
						serverEventGameResults.m_dailyMissionValue = dailyMission.progress;
						loggedInServerInterface.RequestServerEventUpdateGameResults(serverEventGameResults, base.gameObject);
					}
				}
				else
				{
					this.m_counter++;
				}
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_counter > 0)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateEventPostGameResult)));
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num != 61509)
			{
				return TinyFsmState.End();
			}
			this.m_counter++;
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEventPostGameResult(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_counter = 0;
			if (this.GetEventResultState() == EventResultState.TIMEUP)
			{
				base.StartCoroutine(this.NotSendEventPostGameResult());
			}
			else
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					int numRaidBossRings = 0;
					StageScoreManager instance = StageScoreManager.Instance;
					if (instance != null)
					{
						numRaidBossRings = (int)instance.FinalCountData.raid_boss_ring;
					}
					loggedInServerInterface.RequestServerEventPostGameResults(EventManager.Instance.Id, numRaidBossRings, base.gameObject);
				}
				else
				{
					this.m_counter++;
				}
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_counter > 0)
			{
				EventUtility.SetRaidBossFirstBattle();
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateResult)));
			}
			return TinyFsmState.End();
		case 5:
		{
			int iD = e.GetMessage.ID;
			int num = iD;
			if (num != 61510)
			{
				return TinyFsmState.End();
			}
			this.m_counter++;
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateResult(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			EventUtility.UpdateCollectObjectCount();
			if (!this.m_fromTitle && this.m_gameResult != null)
			{
				this.m_gameResult.PlayScoreStart();
				if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
				{
					int raidbossBeatBonus = 0;
					if (this.m_raidBossBonus != null)
					{
						raidbossBeatBonus = this.m_raidBossBonus.BeatBonus;
					}
					this.m_gameResult.SetRaidbossBeatBonus(raidbossBeatBonus);
				}
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_gameResult != null && this.m_gameResult.IsEndOutAnimation)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloStageEnd)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSendApolloStageEnd(TinyFsmEvent e)
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
		{
			this.SetTimeScale(1f);
			MsgExitStage msgExitStage = new MsgExitStage();
			GameObjectUtil.SendMessageToTagObjects("StageManager", "OnMsgExitStage", msgExitStage, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgExitStage", msgExitStage, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnMsgExitStage", msgExitStage, SendMessageOptions.DontRequireReceiver);
			if (this.m_hudCaution != null)
			{
				this.m_hudCaution.SetMsgExitStage(msgExitStage);
			}
			this.StopStageEffect();
			ApolloTutorialIndex apolloEndTutorialIndex = this.GetApolloEndTutorialIndex();
			if (apolloEndTutorialIndex != ApolloTutorialIndex.NONE)
			{
				string[] value = new string[1];
				SendApollo.GetTutorialValue(apolloEndTutorialIndex, ref value);
				this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value);
			}
			else
			{
				this.m_sendApollo = null;
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			bool flag = true;
			if (this.m_sendApollo != null && !this.m_sendApollo.IsEnd())
			{
				flag = false;
			}
			if (flag)
			{
				if (this.m_equipItemTutorial && this.m_exitFromResult)
				{
					HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.TUTORIAL_EQIP_ITEM_END);
				}
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateEndFadeOut)));
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEndFadeOut(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			BackKeyManager.EndScene();
			this.SetTimeScale(1f);
			CameraFade.StartAlphaFade(Color.white, false, 1f);
			SoundManager.BgmFadeOut(0.5f);
			this.m_timer = 1f;
			return TinyFsmState.End();
		case 4:
			this.m_timer -= e.GetDeltaTime;
			if (this.m_timer < 0f)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
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
			this.SetTimeScale(1f);
			this.ResetReplaceAtlas();
			this.RemoveAllResource();
			SoundManager.BgmStop();
			AtlasManager.Instance.ClearAllAtlas();
			GC.Collect();
			Resources.UnloadUnusedAssets();
			GC.Collect();
			if (this.m_fromTitle || this.m_firstTutorial)
			{
				if (this.m_firstTutorial)
				{
					GameModeTitle.FirstTutorialReturned = true;
				}
				UnityEngine.SceneManagement.SceneManager.LoadScene(TitleDefine.TitleSceneName);
			}
			else
			{
				this.CreateResultInfo();
				UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
			}
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private bool IsRaidBossStateUpdate()
	{
		return !this.m_firstTutorial && (EventManager.Instance != null && EventManager.Instance.Type == EventManager.EventType.RAID_BOSS && EventManager.Instance.IsChallengeEvent() && !EventManager.Instance.IsRaidBossStage() && !EventManager.Instance.IsEncounterRaidBoss());
	}

	private void RequestServerStartAct()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface == null)
		{
			this.m_serverActEnd = true;
			return;
		}
		if (this.m_fromTitle || this.m_tutorialStage || this.m_firstTutorial)
		{
			this.m_serverActEnd = true;
			return;
		}
		List<ItemType> list = new List<ItemType>();
		foreach (ItemType current in this.m_useEquippedItem)
		{
			if (current != ItemType.UNKNOWN)
			{
				list.Add(current);
			}
		}
		this.m_serverActEnd = false;
		List<BoostItemType> list2 = new List<BoostItemType>(this.m_useBoostItem);
		if (this.m_quickMode)
		{
			bool tutorial = this.IsTutorialOnActStart();
			if (this.IsTutorialItem())
			{
				list.Clear();
				list2.Clear();
			}
			loggedInServerInterface.RequestServerQuickModeStartAct(list, list2, tutorial, base.gameObject);
		}
		else
		{
			EventManager instance = EventManager.Instance;
			if (instance != null)
			{
				if (instance.IsRaidBossStage())
				{
					loggedInServerInterface.RequestServerEventStartAct(instance.Id, instance.UseRaidbossChallengeCount, RaidBossInfo.currentRaidData.id, list, list2, base.gameObject);
				}
				else
				{
					List<string> list3 = new List<string>();
					SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
					if (socialInterface != null && socialInterface.IsLoggedIn)
					{
						List<SocialUserData> friendList = socialInterface.FriendList;
						if (friendList != null)
						{
							foreach (SocialUserData current2 in friendList)
							{
								string gameId = current2.CustomData.GameId;
								list3.Add(gameId);
							}
						}
					}
					if (this.IsTutorialItem())
					{
						list.Clear();
					}
					bool tutorial2 = this.IsTutorialOnActStart();
					int? eventId = null;
					if (instance.EventStage || instance.IsCollectEvent())
					{
						eventId = new int?(0);
						eventId = new int?(instance.Id);
					}
					loggedInServerInterface.RequestServerStartAct(list, list2, list3, tutorial2, eventId, base.gameObject);
				}
			}
		}
	}

	private void RegisterAllResource()
	{
		if (ResourceManager.Instance != null)
		{
			ResourceManager.Instance.AddCategorySceneObjectsAndSetActive(ResourceCategory.TERRAIN_MODEL, this.m_terrainDataName, GameObject.Find(TerrainXmlData.DataAssetName), false);
			ResourceManager.Instance.AddCategorySceneObjectsAndSetActive(ResourceCategory.STAGE_RESOURCE, this.m_stageResourceName, GameObject.Find(this.m_stageResourceObjectName), false);
			ResourceManager.Instance.RemoveNotActiveContainer(ResourceCategory.TERRAIN_MODEL);
			ResourceManager.Instance.RemoveNotActiveContainer(ResourceCategory.STAGE_RESOURCE);
			if (this.m_playerInformation)
			{
				if (this.m_playerInformation.MainCharacterName != null)
				{
					string text = "CharacterModel" + this.m_playerInformation.MainCharacterName;
					string text2 = "CharacterEffect" + this.m_playerInformation.MainCharacterName;
					ResourceManager.Instance.AddCategorySceneObjectsAndSetActive(ResourceCategory.CHARA_MODEL, text, GameObject.Find(text), true);
					ResourceManager.Instance.AddCategorySceneObjectsAndSetActive(ResourceCategory.CHARA_EFFECT, text2, GameObject.Find(text2), true);
				}
				if (this.m_playerInformation.SubCharacterName != null)
				{
					string text3 = "CharacterModel" + this.m_playerInformation.SubCharacterName;
					string text4 = "CharacterEffect" + this.m_playerInformation.SubCharacterName;
					ResourceManager.Instance.AddCategorySceneObjectsAndSetActive(ResourceCategory.CHARA_MODEL, text3, GameObject.Find(text3), true);
					ResourceManager.Instance.AddCategorySceneObjectsAndSetActive(ResourceCategory.CHARA_EFFECT, text4, GameObject.Find(text4), true);
				}
			}
			ResourceManager.Instance.RemoveNotActiveContainer(ResourceCategory.CHARA_MODEL);
			ResourceManager.Instance.RemoveNotActiveContainer(ResourceCategory.CHARA_EFFECT);
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				int mainChaoID = instance.PlayerData.MainChaoID;
				int subChaoID = instance.PlayerData.SubChaoID;
				if (mainChaoID >= 0)
				{
					ResourceManager.Instance.AddCategorySceneObjects(ResourceCategory.CHAO_MODEL, null, GameObject.Find("ChaoModel" + mainChaoID.ToString("0000")), false);
				}
				if (subChaoID >= 0 && subChaoID != mainChaoID)
				{
					ResourceManager.Instance.AddCategorySceneObjects(ResourceCategory.CHAO_MODEL, null, GameObject.Find("ChaoModel" + subChaoID.ToString("0000")), false);
				}
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
			StageAbilityManager.SetupAbilityDataTable();
		}
	}

	private void ResetReplaceAtlas()
	{
		if (AtlasManager.Instance != null)
		{
			AtlasManager.Instance.ResetReplaceAtlas();
		}
	}

	private void RemoveAllResource()
	{
		if (ResourceManager.Instance != null)
		{
			ResourceManager.Instance.RemoveResourcesOnThisScene();
			ResourceManager.Instance.SetContainerActive(ResourceCategory.TERRAIN_MODEL, this.m_terrainDataName, false);
			ResourceManager.Instance.SetContainerActive(ResourceCategory.STAGE_RESOURCE, this.m_stageResourceName, false);
			if (this.m_playerInformation.MainCharacterName != null)
			{
				ResourceManager.Instance.SetContainerActive(ResourceCategory.CHARA_MODEL, "CharacterModel" + this.m_playerInformation.MainCharacterName, false);
				ResourceManager.Instance.SetContainerActive(ResourceCategory.CHARA_EFFECT, "CharacterEffect" + this.m_playerInformation.MainCharacterName, false);
			}
			if (this.m_playerInformation.SubCharacterName != null)
			{
				ResourceManager.Instance.SetContainerActive(ResourceCategory.CHARA_MODEL, "CharacterModel" + this.m_playerInformation.SubCharacterName, false);
				ResourceManager.Instance.SetContainerActive(ResourceCategory.CHARA_EFFECT, "CharacterEffect" + this.m_playerInformation.SubCharacterName, false);
			}
		}
		Resources.UnloadUnusedAssets();
	}

	private void CreateStageBlockManager()
	{
		if (this.m_stageBlockManager == null)
		{
			this.m_stageBlockManager = GameObject.Find("StageBlockManager");
			StageBlockManager stageBlockManager;
			if (this.m_stageBlockManager == null)
			{
				this.m_stageBlockManager = new GameObject("StageBlockManager");
				stageBlockManager = this.m_stageBlockManager.AddComponent<StageBlockManager>();
			}
			else
			{
				stageBlockManager = this.m_stageBlockManager.GetComponent<StageBlockManager>();
			}
			if (stageBlockManager != null)
			{
				stageBlockManager.Initialize(new StageBlockManager.CreateInfo
				{
					stageName = this.m_stageName,
					isTerrainManager = this.m_isCreateTerrainPlacementManager,
					isSpawnableManager = this.m_isCreatespawnableManager,
					isPathBlockManager = (this.m_stagePathManager != null),
					pathManager = this.m_stagePathManager,
					showInfo = this.m_showBlockInfo,
					randomBlock = this.m_randomBlock,
					bossMode = this.m_bossStage,
					quickMode = this.m_quickMode
				});
			}
		}
	}

	public RareEnemyTable GetRareEnemyTable()
	{
		return this.m_rareEnemyTable;
	}

	public EnemyExtendItemTable GetEnemyExtendItemTable()
	{
		return this.m_enemyExtendItemTable;
	}

	public BossTable GetBossTable()
	{
		return this.m_bossTable;
	}

	public BossMap3Table GetBossMap3Table()
	{
		return this.m_bossMap3Table;
	}

	public ObjectPartTable GetObjectPartTable()
	{
		return this.m_objectPartTable;
	}

	public void RetryStreamingDataLoad(int retryCount)
	{
		StageStreamingDataLoadRetryProcess process = new StageStreamingDataLoadRetryProcess(base.gameObject, this);
		NetMonitor.Instance.StartMonitor(process, 0f, HudNetworkConnect.DisplayType.ALL);
		StreamingDataLoader.Instance.StartDownload(retryCount, base.gameObject);
	}

	private void SendMessageToHudCaution(HudCaution.Type hudType)
	{
		if (this.m_hudCaution != null)
		{
			MsgCaution caution = new MsgCaution(hudType);
			this.m_hudCaution.SetCaution(caution);
		}
	}

	private void SendBossStartMessageToChao()
	{
		GameObjectUtil.SendMessageToTagObjects("Chao", "OnMsgStartBoss", null, SendMessageOptions.DontRequireReceiver);
	}

	private string GetChaoBGMName(int chaoId)
	{
		DataTable.ChaoData chaoData = ChaoTable.GetChaoData(chaoId);
		if (chaoData != null)
		{
			return chaoData.bgmName;
		}
		return string.Empty;
	}

	private string GetCueSheetName(int chaoId)
	{
		DataTable.ChaoData chaoData = ChaoTable.GetChaoData(chaoId);
		if (chaoData != null)
		{
			return chaoData.cueSheetName;
		}
		return string.Empty;
	}

	private void SetMainBGMName()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			string chaoBGMName = this.GetChaoBGMName(instance.PlayerData.MainChaoID);
			if (!string.IsNullOrEmpty(chaoBGMName))
			{
				this.m_mainBgmName = chaoBGMName;
				return;
			}
		}
		if (EventManager.Instance != null)
		{
			EventStageData stageData = EventManager.Instance.GetStageData();
			if (stageData != null)
			{
				string text = null;
				if (EventManager.Instance.Type == EventManager.EventType.QUICK)
				{
					if (this.m_quickMode)
					{
						text = stageData.quickStageBGM;
					}
				}
				else if (EventManager.Instance.Type == EventManager.EventType.BGM)
				{
					if (this.m_quickMode && stageData.IsQuickModeBGM())
					{
						text = stageData.quickStageBGM;
					}
					else if (this.m_bossStage && stageData.IsEndlessModeBGM())
					{
						text = stageData.bossStagBGM;
					}
					else if (stageData.IsEndlessModeBGM())
					{
						text = stageData.stageBGM;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.m_mainBgmName = text;
					return;
				}
			}
		}
		if (this.m_quickMode)
		{
			this.m_mainBgmName = StageTypeUtil.GetQuickModeBgmName(this.m_stageName);
			return;
		}
		if (this.m_bossStage)
		{
			this.m_mainBgmName = BossTypeUtil.GetBossBgmCueSheetName(this.m_bossType);
			return;
		}
		if (this.m_tutorialStage)
		{
			this.m_mainBgmName = StageTypeUtil.GetBgmName(StageType.W01);
			return;
		}
		string bgmName = StageTypeUtil.GetBgmName(this.m_stageName);
		if (bgmName != string.Empty)
		{
			this.m_mainBgmName = bgmName;
		}
	}

	private string GetCueSheetName()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			string cueSheetName = this.GetCueSheetName(instance.PlayerData.MainChaoID);
			if (!string.IsNullOrEmpty(cueSheetName))
			{
				return cueSheetName;
			}
		}
		if (EventManager.Instance != null)
		{
			EventStageData stageData = EventManager.Instance.GetStageData();
			if (stageData != null)
			{
				string text = null;
				if (EventManager.Instance.Type == EventManager.EventType.QUICK)
				{
					if (this.m_quickMode)
					{
						text = stageData.quickStageCueSheetName;
					}
				}
				else if (EventManager.Instance.Type == EventManager.EventType.BGM)
				{
					if (this.m_quickMode && stageData.IsQuickModeBGM())
					{
						text = stageData.quickStageCueSheetName;
					}
					else if (this.m_bossStage && stageData.IsEndlessModeBGM())
					{
						text = stageData.bossStagCueSheetName;
					}
					else if (stageData.IsEndlessModeBGM())
					{
						text = stageData.stageCueSheetName;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
		}
		if (this.m_quickMode)
		{
			return StageTypeUtil.GetQuickModeCueSheetName(this.m_stageName);
		}
		if (this.m_bossStage)
		{
			return BossTypeUtil.GetBossBgmName(this.m_bossType);
		}
		if (this.m_tutorialStage)
		{
			return StageTypeUtil.GetCueSheetName(StageType.W01);
		}
		return StageTypeUtil.GetCueSheetName(this.m_stageName);
	}

	private void CreateResultInfo()
	{
		if (this.m_tutorialStage)
		{
			ResultInfo.CreateOptionTutorialResultInfo();
		}
		else
		{
			ResultInfo resultInfo = ResultInfo.CreateResultInfo();
			if (resultInfo != null)
			{
				ResultData resultData = new ResultData();
				if (resultData != null)
				{
					resultData.m_stageName = this.m_stageName;
					resultData.m_validResult = this.m_exitFromResult;
					resultData.m_fromOptionTutorial = this.m_tutorialStage;
					resultData.m_bossStage = this.m_bossStage;
					resultData.m_bossDestroy = (this.m_levelInformation != null && this.m_levelInformation.BossDestroy);
					resultData.m_eventStage = this.m_eventStage;
					resultData.m_quickMode = this.m_quickMode;
					bool flag = false;
					if (this.m_missionManager != null)
					{
						flag = this.m_missionManager.Completed;
					}
					resultData.m_missionComplete = (!this.m_missonCompleted && flag);
					if (this.m_resultMapState != null)
					{
						resultData.m_newMapState = new MileageMapState(this.m_resultMapState);
					}
					if (this.m_postGameResults.m_prevMapInfo != null && this.m_postGameResults.m_prevMapInfo.m_mapState != null)
					{
						resultData.m_oldMapState = new MileageMapState(this.m_postGameResults.m_prevMapInfo.m_mapState);
					}
					if (this.m_mileageIncentive != null)
					{
						resultData.m_mileageIncentiveList = new List<ServerMileageIncentive>(this.m_mileageIncentive.Count);
						foreach (ServerMileageIncentive current in this.m_mileageIncentive)
						{
							resultData.m_mileageIncentiveList.Add(current);
						}
					}
					if (!this.m_eventStage)
					{
						StageScoreManager instance = StageScoreManager.Instance;
						if (instance != null)
						{
							if (resultData.m_validResult)
							{
								RankingUtil.RankingMode rankingMode = RankingUtil.RankingMode.ENDLESS;
								if (this.m_quickMode)
								{
									rankingMode = RankingUtil.RankingMode.QUICK;
								}
								long finalScore = instance.FinalScore;
								long num = 0L;
								long num2 = 0L;
								int num3 = 0;
								bool flag2 = false;
								RankingManager.GetCurrentHighScoreRank(rankingMode, false, ref finalScore, out flag2, out num, out num2, out num3);
								resultData.m_rivalHighScore = flag2;
								if (flag2)
								{
									resultData.m_highScore = finalScore;
								}
								resultData.m_totalScore = finalScore;
							}
							else
							{
								resultData.m_rivalHighScore = false;
								resultData.m_highScore = 0L;
								resultData.m_totalScore = 0L;
							}
						}
					}
					if (this.m_dailyIncentive != null)
					{
						resultData.m_dailyMissionIncentiveList = new List<ServerItemState>(this.m_dailyIncentive.Count);
						foreach (ServerItemState current2 in this.m_dailyIncentive)
						{
							resultData.m_dailyMissionIncentiveList.Add(current2);
						}
					}
					resultInfo.SetInfo(resultData);
				}
			}
		}
	}

	private void EnablePause(bool value)
	{
		MsgEnablePause value2 = new MsgEnablePause(value);
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnEnablePause", value2, SendMessageOptions.DontRequireReceiver);
	}

	private void HoldPlayerAndDestroyTerrainOnEnd()
	{
		MsgPLHold value = new MsgPLHold();
		GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgPLHold", value, SendMessageOptions.DontRequireReceiver);
		if (this.m_stageBlockManager != null)
		{
			StageBlockManager component = this.m_stageBlockManager.GetComponent<StageBlockManager>();
			if (component != null)
			{
				component.DeactivateAll();
			}
		}
		GameObjectUtil.SendDelayedMessageFindGameObject("HudCockpit", "OnMsgExitStage", new MsgExitStage());
	}

	private bool IsBossTutorialLose()
	{
		bool flag = this.m_levelInformation != null && this.m_levelInformation.BossDestroy;
		if (this.m_bossStage && !flag && this.m_postGameResults.m_prevMapInfo != null)
		{
			MileageMapState mapState = this.m_postGameResults.m_prevMapInfo.m_mapState;
			if (mapState != null && mapState.m_episode == 1 && mapState.m_chapter == 1)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsBossTutorialPresent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				return systemdata.IsFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_PRESENT);
			}
		}
		return false;
	}

	private void SetBossTutorialPresent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null && !systemdata.IsFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_PRESENT))
			{
				systemdata.SetFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_PRESENT, true);
				instance.SaveSystemData();
			}
		}
	}

	private bool IsBossTutorialClear()
	{
		bool flag = this.m_levelInformation != null && this.m_levelInformation.BossDestroy;
		if (this.m_bossStage && flag && this.m_postGameResults.m_prevMapInfo != null)
		{
			MileageMapState mapState = this.m_postGameResults.m_prevMapInfo.m_mapState;
			if (mapState != null && mapState.m_episode == 1 && mapState.m_chapter == 1)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsTutorialOnActStart()
	{
		return this.m_tutorialStage || this.m_firstTutorial || this.m_equipItemTutorial;
	}

	private bool IsTutorialItem()
	{
		return this.m_tutorialStage || this.m_firstTutorial || this.m_equipItemTutorial;
	}

	private bool EnableChaoEgg()
	{
		if (this.IsBossTutorialClear())
		{
			return true;
		}
		bool flag = false;
		if (this.m_levelInformation != null)
		{
			flag = this.m_levelInformation.BossDestroy;
		}
		return this.m_bossStage && this.m_bossNoMissChance && flag;
	}

	private bool IsEnableContinue()
	{
		return !this.m_firstTutorial && (!this.m_bossStage || !this.m_bossTimeUp) && this.m_numEnableContinue > 0;
	}

	private ApolloTutorialIndex GetApolloStartTutorialIndex()
	{
		if (this.m_firstTutorial)
		{
			return ApolloTutorialIndex.START_STEP1;
		}
		if (this.m_equipItemTutorial)
		{
			return ApolloTutorialIndex.START_STEP5;
		}
		return ApolloTutorialIndex.NONE;
	}

	private ApolloTutorialIndex GetApolloEndTutorialIndex()
	{
		if (this.m_firstTutorial)
		{
			return ApolloTutorialIndex.START_STEP1;
		}
		if (this.m_exitFromResult && this.m_equipItemTutorial)
		{
			return ApolloTutorialIndex.START_STEP5;
		}
		return ApolloTutorialIndex.NONE;
	}

	private void OnApplicationPause(bool flag)
	{
		if (flag)
		{
			this.OnMsgExternalGamePause(new MsgExternalGamePause(false, false));
		}
	}

	private void OnGetMileageMapState(MsgGetMileageMapState msg)
	{
		if (this.m_postGameResults.m_prevMapInfo != null)
		{
			msg.m_mileageMapState = this.m_postGameResults.m_prevMapInfo.m_mapState;
		}
		else
		{
			msg.m_mileageMapState = null;
		}
		msg.m_debugLevel = (uint)this.m_debugBossLevel;
		msg.m_succeed = true;
	}

	private void SetMissionResult()
	{
		if (this.m_missionManager != null)
		{
			if (!this.m_bossStage)
			{
				StageScoreManager instance = StageScoreManager.Instance;
				long distance = instance.FinalCountData.distance;
				MsgMissionEvent msg = new MsgMissionEvent(Mission.EventID.TOTALDISTANCE, distance);
				ObjUtil.SendMessageMission2(msg);
				long finalScore = instance.FinalScore;
				MsgMissionEvent msg2 = new MsgMissionEvent(Mission.EventID.GET_SCORE, finalScore);
				ObjUtil.SendMessageMission2(msg2);
				long ring = instance.FinalCountData.ring;
				MsgMissionEvent msg3 = new MsgMissionEvent(Mission.EventID.GET_RING, ring);
				ObjUtil.SendMessageMission2(msg3);
				long animal = instance.FinalCountData.animal;
				MsgMissionEvent msg4 = new MsgMissionEvent(Mission.EventID.GET_ANIMALS, animal);
				ObjUtil.SendMessageMission2(msg4);
			}
			this.m_missionManager.SaveMissions();
		}
	}

	private bool IsEventTimeup()
	{
		if (this.m_eventStage && this.m_eventManager != null && (this.m_eventManager.IsSpecialStage() || this.m_eventManager.IsRaidBossStage()) && !this.m_eventManager.IsPlayEventForStage())
		{
			global::Debug.Log("*****Event Timeup!!!!!*****");
			return true;
		}
		return false;
	}

	private EventResultState GetEventResultState()
	{
		if (this.m_eventStage && this.m_eventManager != null)
		{
			if (!this.m_eventManager.IsResultEvent())
			{
				return EventResultState.TIMEUP;
			}
			if (!this.m_eventManager.IsPlayEventForStage())
			{
				return EventResultState.TIMEUP_RESULT;
			}
		}
		return EventResultState.NONE;
	}

	private void PlayStageEffect()
	{
		if (this.m_stageEffect == null)
		{
			this.m_stageEffect = StageEffect.CreateStageEffect(this.m_stageName);
		}
	}

	private void StopStageEffect()
	{
		if (this.m_stageEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_stageEffect.gameObject);
			this.m_stageEffect = null;
		}
	}

	private void ResetPosStageEffect(bool reset)
	{
		if (this.m_stageEffect != null)
		{
			this.m_stageEffect.ResetPos(reset);
		}
	}

	private void HudPlayerChangeCharaButton(bool val, bool pause)
	{
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnChangeCharaButton", new MsgChangeCharaButton(val, pause), SendMessageOptions.DontRequireReceiver);
	}

	private void SetChaoAblityTimeScale()
	{
		if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.EASY_SPEED))
		{
			int num = (int)StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.EASY_SPEED);
			int num2 = UnityEngine.Random.Range(0, 100);
			if (num >= num2)
			{
				float chaoAbilityExtraValue = StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.EASY_SPEED);
				this.m_chaoEasyTimeScale = this.m_defaultTimeScale * (1f - chaoAbilityExtraValue * 0.01f);
			}
		}
	}

	private void SetDefaultTimeScale()
	{
		float num = this.m_defaultTimeScale;
		if (StageAbilityManager.Instance != null)
		{
			num = StageAbilityManager.Instance.GetTeamAbliltyTimeScale(num);
		}
		if (this.m_chaoEasyTimeScale < num)
		{
			num = this.m_chaoEasyTimeScale;
		}
		this.SetTimeScale(num);
	}

	private void SetTimeScale(float timeScale)
	{
		Time.timeScale = timeScale;
	}

	private void DrawingInvalidExtreme()
	{
		if (this.m_levelInformation != null && this.m_levelInformation.Extreme)
		{
			StageAbilityManager instance = StageAbilityManager.Instance;
			if (instance != null && instance.HasChaoAbility(ChaoAbility.INVALIDI_EXTREME_STAGE))
			{
				int num = (int)instance.GetChaoAbilityExtraValue(ChaoAbility.INVALIDI_EXTREME_STAGE);
				if (this.m_invalidExtremeCount < num)
				{
					float chaoAbilityValue = instance.GetChaoAbilityValue(ChaoAbility.INVALIDI_EXTREME_STAGE);
					float num2 = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue >= num2)
					{
						this.m_levelInformation.InvalidExtreme = true;
						this.m_invalidExtremeCount++;
						this.m_invalidExtremeFlag = true;
					}
				}
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
