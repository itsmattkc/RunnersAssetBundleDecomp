using DataTable;
using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class TitleDataLoader : MonoBehaviour
{
	private enum EventSignal
	{
		SuccessStreamingDataLoad = 100,
		NUM
	}

	private TinyFsmBehavior m_fsm_behavior;

	private ResourceSceneLoader m_sceneLoader;

	private static readonly float LoadWaitTime = 60f;

	private static readonly int CountToAskGiveUp = 3;

	private bool m_Retry;

	private bool m_loadEnd;

	private List<ResourceSceneLoader.ResourceInfo> m_defaultLoadInfoFirst = new List<ResourceSceneLoader.ResourceInfo>
	{
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.COMMON_EFFECT, "ResourcesCommonEffect", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.PLAYER_COMMON, "CharacterCommonResource", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.OBJECT_RESOURCE, "CommonObjectResource", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.OBJECT_PREFAB, "CommonObjectPrefabs", false, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ENEMY_RESOURCE, "CommonEnemyResource", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ENEMY_PREFAB, "CommonEnemyPrefabs", false, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "ChaoDataTable", true, false, true, "ChaoTable", false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "CharaAbilityDataTable", true, false, true, "ImportAbilityTable", false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "CharacterDataNameInfo", true, false, true, null, false)
	};

	private List<ResourceSceneLoader.ResourceInfo> m_defaultLoadInfo = new List<ResourceSceneLoader.ResourceInfo>
	{
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.COMMON_EFFECT, "ResourcesCommonEffect", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.PLAYER_COMMON, "CharacterCommonResource", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.OBJECT_RESOURCE, "CommonObjectResource", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.OBJECT_PREFAB, "CommonObjectPrefabs", false, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ENEMY_RESOURCE, "CommonEnemyResource", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ENEMY_PREFAB, "CommonEnemyPrefabs", false, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "AchievementTable", true, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "MissionTable", true, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "ChaoDataTable", true, false, true, "ChaoTable", false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "CharaAbilityDataTable", true, false, true, "ImportAbilityTable", false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "CharacterDataNameInfo", true, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, "OverlapBonusTable", true, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.QUICK_MODE, "StageTimeTable", true, false, true, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ChaoTextures", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "MainMenuPages", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "RouletteTopUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ChaoWindows", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ShopPage", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ChaoSetUIPage", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "OptionWindows", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ChaoSetWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "DailyInfoUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "DailyWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "DailybattleRewardWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "InformationUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ItemSet_3_UI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "LoginWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "NewsWindow", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "OptionUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "PlayerSetWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "PlayerSet_3_UI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "PresentBoxUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "StartDashWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "WorldRankingWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ui_mm_mileage2_page", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ui_mm_ranking_page", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ui_tex_mm_ep_001", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "ui_tex_mm_ep_002", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "window_name_setting", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "DailyBattleDetailWindow", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "DeckViewWindow", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "LeagueResultWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "Mileage_rankup", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "RankingFriendOptionWindow", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "RankingResultBitWindow", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "RankingWindowUI", true, true, false, null, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.UNKNOWN, "item_get_Window", true, true, false, null, false)
	};

	private List<string> m_streamingSoundData = new List<string>();

	private List<ResourceSceneLoader.ResourceInfo> m_nowLoadingList;

	private List<ResourceSceneLoader.ResourceInfo> m_loadInfo;

	private int m_requestDownloadCount;

	private int m_requestLoadCount;

	private bool m_endCheckExistingCheckDownloadData;

	public bool LoadEnd
	{
		get
		{
			return this.m_loadEnd;
		}
	}

	public int LoadEndCount
	{
		get
		{
			int num = (!(this.m_sceneLoader == null)) ? this.m_sceneLoader.LoadEndCount : 0;
			num += StreamingDataLoader.Instance.NumLoaded;
			if (InformationDataTable.Instance != null && InformationDataTable.Instance.Loaded)
			{
				num++;
			}
			return num;
		}
		private set
		{
		}
	}

	public int RequestedLoadCount
	{
		get
		{
			return this.m_requestLoadCount;
		}
	}

	public int RequestedDownloadCount
	{
		get
		{
			return this.m_requestDownloadCount;
		}
	}

	public bool EndCheckExistingDownloadData
	{
		get
		{
			return this.m_endCheckExistingCheckDownloadData;
		}
	}

	private void Start()
	{
	}

	public void AddStreamingSoundData(string data)
	{
		this.m_streamingSoundData.Add(data);
	}

	public void Setup(bool is_first)
	{
		this.Init(is_first);
		this.m_requestDownloadCount = 0;
		this.m_requestLoadCount = this.m_loadInfo.Count + this.m_streamingSoundData.Count + 1;
		this.m_endCheckExistingCheckDownloadData = false;
	}

	private void OnDestroy()
	{
		if (this.m_fsm_behavior)
		{
			this.m_fsm_behavior.ShutDown();
			this.m_fsm_behavior = null;
		}
	}

	private void Update()
	{
	}

	public void StartLoad()
	{
		if (this.m_fsm_behavior != null)
		{
			if (StreamingDataLoader.Instance.NumInLoadList > 0)
			{
				this.m_fsm_behavior.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadStreaming)));
			}
			else
			{
				this.m_fsm_behavior.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadScene)));
			}
		}
	}

	public void RetryStreamingDataLoad(int retryCount)
	{
		StreamingDataLoadRetryProcess process = new StreamingDataLoadRetryProcess(retryCount, base.gameObject, this);
		NetMonitor.Instance.StartMonitor(process, -1f, HudNetworkConnect.DisplayType.ALL);
		StreamingDataLoader.Instance.StartDownload(retryCount, base.gameObject);
	}

	public void RetryInformationDataLoad()
	{
		InformationDataLoadRetryProcess process = new InformationDataLoadRetryProcess(base.gameObject, this);
		NetMonitor.Instance.StartMonitor(process, 0f, HudNetworkConnect.DisplayType.ALL);
		InformationDataTable.Instance.Initialize(base.gameObject);
	}

	private void Init(bool is_first)
	{
		if (ResourceManager.Instance == null)
		{
			GameObject gameObject = new GameObject("ResourceManager");
			gameObject.AddComponent<ResourceManager>();
		}
		this.m_loadInfo = new List<ResourceSceneLoader.ResourceInfo>();
		if (is_first)
		{
			foreach (ResourceSceneLoader.ResourceInfo current in this.m_defaultLoadInfoFirst)
			{
				this.m_loadInfo.Add(current);
			}
		}
		else
		{
			foreach (ResourceSceneLoader.ResourceInfo current2 in this.m_defaultLoadInfo)
			{
				this.m_loadInfo.Add(current2);
			}
		}
		string suffixe = TextUtility.GetSuffixe();
		string name = "text_common_text_" + suffixe;
		string name2 = "text_event_common_text_" + suffixe;
		string name3 = "text_chao_text_" + suffixe;
		this.m_loadInfo.Add(new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, name, true, false, false, null, false));
		this.m_loadInfo.Add(new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, name3, true, false, false, null, false));
		if (!is_first)
		{
			this.m_loadInfo.Add(new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, name2, true, false, false, null, false));
		}
		if (!is_first)
		{
			this.AddSceneLoaderChaoTexture();
		}
		this.m_fsm_behavior = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm_behavior != null)
		{
			TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
			description.initState = new TinyFsmState(new EventFunction(this.StateAddDownloadFile));
			this.m_fsm_behavior.SetUp(description);
		}
		GameObject gameObject2 = new GameObject("ResourceSceneLoader");
		this.m_sceneLoader = gameObject2.AddComponent<ResourceSceneLoader>();
		if (this.m_sceneLoader != null)
		{
			this.m_sceneLoader.Pause(true);
		}
	}

	private void AddSceneLoaderChaoTexture()
	{
		GameObject gameObject = GameObject.Find("AssetBundleLoader");
		if (gameObject != null)
		{
			AssetBundleLoader component = gameObject.GetComponent<AssetBundleLoader>();
			if (component != null)
			{
				string[] chaoTextureList = component.GetChaoTextureList();
				string[] array = chaoTextureList;
				for (int i = 0; i < array.Length; i++)
				{
					string name = array[i];
					this.m_loadInfo.Add(new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, name, true, true, false, null, false));
				}
			}
		}
	}

	private TinyFsmState StateAddDownloadFile(TinyFsmEvent fsm_event)
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
			string[] array = new string[]
			{
				"CharacterEffectSonic",
				"CharacterModelSonic",
				"w01_StageResource",
				"w01_TerrainData",
				"TenseEffectTable"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string scenename = array2[i];
				ResourceSceneLoader.ResourceInfo resourceInfo = new ResourceSceneLoader.ResourceInfo();
				resourceInfo.m_category = ResourceCategory.UNKNOWN;
				resourceInfo.m_scenename = scenename;
				resourceInfo.m_onlyDownload = true;
				resourceInfo.m_isAssetBundle = true;
				this.m_loadInfo.Add(resourceInfo);
			}
			this.m_fsm_behavior.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckDownload)));
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCheckDownload(TinyFsmEvent fsm_event)
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
			this.m_requestLoadCount = 0;
			int num = 0;
			if (AssetBundleLoader.Instance != null)
			{
				foreach (ResourceSceneLoader.ResourceInfo current in this.m_loadInfo)
				{
					if (current.m_isAssetBundle && !AssetBundleLoader.Instance.IsDownloaded(current.m_scenename))
					{
						this.m_requestDownloadCount++;
					}
				}
			}
			this.m_nowLoadingList = new List<ResourceSceneLoader.ResourceInfo>();
			foreach (ResourceSceneLoader.ResourceInfo current2 in this.m_loadInfo)
			{
				if (this.m_sceneLoader.AddLoadAndResourceManager(current2))
				{
					this.m_nowLoadingList.Add(current2);
				}
			}
			if (StreamingDataLoader.Instance != null)
			{
				foreach (string current3 in this.m_streamingSoundData)
				{
					string url = SoundManager.GetDownloadURL() + current3;
					string path = SoundManager.GetDownloadedDataPath() + current3;
					StreamingDataLoader.Instance.AddFileIfNotDownloaded(url, path);
				}
				this.m_requestDownloadCount += StreamingDataLoader.Instance.NumInLoadList;
				num = StreamingDataLoader.Instance.NumInLoadList;
			}
			this.m_requestLoadCount = this.m_nowLoadingList.Count + num + 1;
			this.m_endCheckExistingCheckDownloadData = true;
			this.m_fsm_behavior.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
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
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadStreaming(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			StreamingDataLoadRetryProcess process = new StreamingDataLoadRetryProcess(0, base.gameObject, this);
			NetMonitor.Instance.StartMonitor(process, -1f, HudNetworkConnect.DisplayType.ALL);
			StreamingDataLoader.Instance.StartDownload(0, base.gameObject);
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_27:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_fsm_behavior.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadScene)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		goto IL_27;
	}

	private TinyFsmState StateLoadScene(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_sceneLoader.Pause(false);
			return TinyFsmState.End();
		case 4:
			if (this.m_sceneLoader.Loaded)
			{
				this.m_nowLoadingList = null;
				this.m_fsm_behavior.ChangeState(new TinyFsmState(new EventFunction(this.StateLoadInfoData)));
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateLoadInfoData(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (InformationDataTable.Instance == null)
			{
				InformationDataLoadRetryProcess process = new InformationDataLoadRetryProcess(base.gameObject, this);
				NetMonitor.Instance.StartMonitor(process, 0f, HudNetworkConnect.DisplayType.ALL);
				InformationDataTable.Create();
				InformationDataTable.Instance.Initialize(base.gameObject);
			}
			return TinyFsmState.End();
		case 4:
			this.m_loadEnd = true;
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void StreamingDataLoad_Succeed()
	{
		if (this.m_fsm_behavior != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
			this.m_fsm_behavior.Dispatch(signal);
		}
	}

	private void StreamingDataLoad_Failed()
	{
		NetMonitor.Instance.EndMonitorForward(new MsgAssetBundleResponseFailedMonitor(), null, null);
		NetMonitor.Instance.EndMonitorBackward();
	}

	private void InformationDataLoad_Succeed()
	{
		NetMonitor.Instance.EndMonitorForward(new MsgAssetBundleResponseSucceed(null, null), null, null);
		NetMonitor.Instance.EndMonitorBackward();
	}

	private void InformationDataLoad_Failed()
	{
		NetMonitor.Instance.EndMonitorForward(new MsgAssetBundleResponseFailedMonitor(), null, null);
		NetMonitor.Instance.EndMonitorBackward();
	}
}
