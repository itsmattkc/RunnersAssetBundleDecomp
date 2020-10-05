using DataTable;
using GooglePlayGames;
using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class AchievementManager : MonoBehaviour
{
	private enum State
	{
		Idle,
		Authenticate,
		AuthenticateError,
		AuthenticateSkip,
		LoadAchievement,
		LoadAchievementError,
		Report,
		RequestIncentive,
		RequestEnd
	}

	public bool m_debugInfo = true;

	private bool m_debugInfo2;

	public bool m_debugAllOpen;

	private AchievementManager.State m_state;

	private int m_reportCount;

	private int m_reportSuccessCount;

	private List<AchievementTempData> m_loadData = new List<AchievementTempData>();

	private List<AchievementTempData> m_clearData = new List<AchievementTempData>();

	private List<AchievementData> m_data = new List<AchievementData>();

	private bool m_setupDataTable;

	private float m_waitTime;

	private static float WAIT_TIME = 10f;

	private bool m_connectAnim;

	private AchievementIncentive m_achievementIncentive;

	private static AchievementManager m_instance;

	public static AchievementManager Instance
	{
		get
		{
			return AchievementManager.m_instance;
		}
	}

	private void Awake()
	{
		if (AchievementManager.m_instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			AchievementManager.m_instance = this;
			base.gameObject.AddComponent<HudNetworkConnect>();
			PlayGamesPlatform.DebugLogEnabled = false;
			PlayGamesPlatform.Activate();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
			this.DestroyDataTable();
		}
	}

	private void OnDestroy()
	{
		if (AchievementManager.m_instance == this)
		{
			AchievementManager.m_instance = null;
		}
	}

	private void Update()
	{
		if (this.m_state == AchievementManager.State.Report && (this.m_reportCount == this.m_reportSuccessCount || this.IsRequestEndAchievement()))
		{
			this.SetDebugDraw(string.Concat(new object[]
			{
				"ReportResult m_reportCount=",
				this.m_reportCount,
				" m_reportSuccessCount=",
				this.m_reportSuccessCount
			}));
			this.SetNetworkConnect(false);
			this.RequestAchievementIncentive(this.m_reportSuccessCount);
			this.m_state = AchievementManager.State.RequestIncentive;
		}
		if (this.m_state == AchievementManager.State.RequestIncentive && this.IsRequestEndIncentive())
		{
			this.m_state = AchievementManager.State.RequestEnd;
		}
		if (this.m_waitTime > 0f)
		{
			this.m_waitTime -= Time.deltaTime;
			if (this.m_waitTime < 0f)
			{
				this.SetNetworkConnect(false);
				this.m_waitTime = 0f;
			}
		}
	}

	public void RequestUpdateAchievement()
	{
		if (!this.IsSetupEndAchievement())
		{
			this.SetDebugDraw("RequestUpdateAchievement Not Setup m_state=" + this.m_state.ToString());
			return;
		}
		this.SetDebugDraw("RequestUpdateAchievement m_state=" + this.m_state.ToString());
		switch (this.m_state)
		{
		case AchievementManager.State.Idle:
			this.SetNetworkConnect(true);
			this.Authenticate();
			break;
		case AchievementManager.State.LoadAchievementError:
			this.SetNetworkConnect(true);
			this.LoadAchievements();
			break;
		case AchievementManager.State.RequestEnd:
			this.SetNetworkConnect(true);
			this.LoadAchievements2();
			break;
		}
	}

	public void RequestResetAchievement()
	{
		this.SetDebugDraw("RequestResetAchievement m_state=" + this.m_state.ToString());
		this.ResetAchievements();
	}

	public void ShowAchievementsUI()
	{
		this.SetDebugDraw("ShowAchievementsUI");
		Social.ShowAchievementsUI();
	}

	public bool IsSetupEndAchievement()
	{
		return this.m_setupDataTable;
	}

	public bool IsRequestEndAchievement()
	{
		if (this.IsSetupEndAchievement())
		{
			switch (this.m_state)
			{
			case AchievementManager.State.Authenticate:
			case AchievementManager.State.LoadAchievement:
			case AchievementManager.State.Report:
			case AchievementManager.State.RequestIncentive:
				return this.m_waitTime.Equals(0f);
			}
		}
		return true;
	}

	private void Authenticate()
	{
		this.SetDebugDraw("Authenticate");
		this.m_state = AchievementManager.State.Authenticate;
		this.SetWaitTime();
		if (!Social.localUser.authenticated)
		{
			Social.localUser.Authenticate(new Action<bool>(this.ProcessAuthentication));
		}
		else
		{
			this.ProcessAuthentication(false);
		}
	}

	private void LoadAchievements()
	{
		this.SetDebugDraw("LoadAchievements");
		this.m_state = AchievementManager.State.LoadAchievement;
		this.SetWaitTime();
		if (this.m_setupDataTable && this.m_data.Count > 0)
		{
			string[] array = new string[this.m_data.Count];
			for (int i = 0; i < this.m_data.Count; i++)
			{
				array[i] = this.m_data[i].GetID();
			}
			PlayGamesPlatform.Instance.LoadAchievementDescriptions(array, new Action<IAchievement[]>(this.ProcessLoadedAchievements1));
		}
		else
		{
			this.ProcessLoadedAchievements1(null);
		}
	}

	private void LoadAchievements2()
	{
		if (this.m_setupDataTable && this.m_data.Count > 0)
		{
			string[] array = new string[this.m_data.Count];
			for (int i = 0; i < this.m_data.Count; i++)
			{
				array[i] = this.m_data[i].GetID();
			}
			PlayGamesPlatform.Instance.LoadAchievements(array, new Action<IAchievement[]>(this.ProcessLoadedAchievements2));
		}
		else
		{
			this.ProcessLoadedAchievements2(null);
		}
	}

	private void ReportProgress()
	{
		this.SetDebugDraw("ReportProgress");
		this.m_state = AchievementManager.State.Report;
		this.SetWaitTime();
		this.m_reportCount = 0;
		this.m_reportSuccessCount = 0;
		if (this.m_loadData.Count > 0)
		{
			this.UpdateAchievement();
			this.SetDebugDraw(string.Concat(new object[]
			{
				"m_loadData=",
				this.m_loadData.Count,
				" m_clearData=",
				this.m_clearData.Count
			}));
			if (this.m_clearData.Count > 0)
			{
				foreach (AchievementTempData current in this.m_loadData)
				{
					if (!current.m_reportEnd && this.IsClearAchievement(current.m_id))
					{
						this.m_reportCount++;
						Social.ReportProgress(current.m_id, 100.0, new Action<bool>(this.ProcessReportProgress));
					}
				}
			}
		}
	}

	private void ResetAchievements()
	{
		this.SetDebugDraw("ResetAchievements");
		this.m_state = AchievementManager.State.Idle;
		GameCenterPlatform.ResetAllAchievements(new Action<bool>(this.ProcessResetAllAchievements));
	}

	private void ProcessAuthentication(bool success)
	{
		if (success)
		{
			this.LoadAchievements();
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
				if (systemdata != null && systemdata.achievementCancelCount != 0)
				{
					systemdata.achievementCancelCount = 0;
					instance.SaveSystemData();
				}
			}
		}
		else
		{
			this.SetNetworkConnect(false);
			this.m_state = AchievementManager.State.AuthenticateError;
			this.SetDebugDraw("Authenticate ERROR");
			SystemSaveManager instance2 = SystemSaveManager.Instance;
			if (instance2 != null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == TitleDefine.TitleSceneName)
			{
				SystemData systemdata2 = SystemSaveManager.Instance.GetSystemdata();
				if (systemdata2 != null)
				{
					systemdata2.achievementCancelCount++;
					instance2.SaveSystemData();
				}
			}
		}
	}

	private void SetLoadedAchievements(string[] achievementsIDList)
	{
		if (achievementsIDList == null || achievementsIDList.Length == 0)
		{
			this.SetNetworkConnect(false);
			this.m_state = AchievementManager.State.LoadAchievementError;
			this.SetDebugDraw("LoadAchievements ERROR");
		}
		else
		{
			this.SetDebugDraw("LoadAchievements1 OK " + achievementsIDList.Length + " achievements");
			this.m_loadData.Clear();
			for (int i = 0; i < achievementsIDList.Length; i++)
			{
				string text = achievementsIDList[i];
				if (text != null)
				{
					this.SetDebugDraw("Load achievementID=" + text);
					this.m_loadData.Add(new AchievementTempData(text));
				}
			}
			this.LoadAchievements2();
		}
	}

	private void ProcessLoadedAchievements1(IAchievement[] achievements)
	{
		if (achievements == null || achievements.Length == 0)
		{
			this.SetLoadedAchievements(null);
		}
		else
		{
			string[] array = new string[achievements.Length];
			for (int i = 0; i < achievements.Length; i++)
			{
				array[i] = achievements[i].id;
			}
			this.SetLoadedAchievements(array);
		}
	}

	private void ProcessLoadedAchievements2(IAchievement[] achievements)
	{
		if (achievements != null && achievements.Length > 0)
		{
			this.SetDebugDraw("LoadAchievements2 " + achievements.Length + " achievements");
			for (int i = 0; i < achievements.Length; i++)
			{
				IAchievement achievement = achievements[i];
				if (achievement != null)
				{
					this.SetReporteEnd(achievement.id);
				}
			}
		}
		this.ReportProgress();
	}

	private void ProcessReportProgress(bool result)
	{
		if (result)
		{
			this.m_reportSuccessCount++;
			this.SetDebugDraw("ReportProgress OK");
		}
		else
		{
			this.SetDebugDraw("ReportProgress ERROR");
		}
	}

	private void ProcessResetAllAchievements(bool result)
	{
		if (result)
		{
			this.SetDebugDraw("ProcessResetAllAchievements OK");
		}
		else
		{
			this.SetDebugDraw("ProcessResetAllAchievements ERROR");
		}
	}

	private void UpdateAchievement()
	{
		this.m_clearData.Clear();
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			this.SetDebugDraw2("DataTable m_data.Count=" + this.m_data.Count);
			for (int i = 0; i < this.m_data.Count; i++)
			{
				AchievementData achievementData = this.m_data[i];
				if (achievementData != null)
				{
					this.SetDebugDraw2(string.Concat(new object[]
					{
						"number=",
						achievementData.number,
						" explanation=",
						achievementData.explanation,
						" type=",
						achievementData.type.ToString(),
						" itemID=",
						achievementData.itemID,
						" value=",
						achievementData.value,
						" id=",
						achievementData.GetID()
					}));
					if (achievementData.GetID() != null && achievementData.GetID() != string.Empty)
					{
						bool flag = false;
						switch (achievementData.type)
						{
						case AchievementData.Type.ANIMAL:
							flag = this.CheckClearAnimal(achievementData, playerState.m_numAnimals);
							break;
						case AchievementData.Type.DISTANCE:
							flag = this.CheckClearDistance(achievementData, (uint)playerState.m_totalDistance);
							break;
						case AchievementData.Type.PLAYER_OPEN:
							flag = this.CheckClearPlayerOpen(achievementData, playerState.CharacterStateByItemID(achievementData.itemID));
							break;
						case AchievementData.Type.PLAYER_LEVEL:
							flag = this.CheckClearPlayerLevel(achievementData, playerState.CharacterStateByItemID(achievementData.itemID));
							break;
						case AchievementData.Type.CHAO_OPEN:
							flag = this.CheckClearChaoOpen(achievementData, playerState.ChaoStateByItemID(achievementData.itemID));
							break;
						case AchievementData.Type.CHAO_LEVEL:
							flag = this.CheckClearChaoLevel(achievementData, playerState.ChaoStateByItemID(achievementData.itemID));
							break;
						}
						string text = string.Empty;
						if (this.IsDebugAllOpen())
						{
							flag = true;
							text = "DebugAllOpen ";
						}
						if (flag)
						{
							this.SetDebugDraw2(string.Concat(new string[]
							{
								text,
								"Clear!! ID=",
								achievementData.GetID(),
								" / ",
								achievementData.explanation
							}));
							this.m_clearData.Add(new AchievementTempData(achievementData.GetID()));
						}
					}
				}
			}
		}
	}

	private bool CheckClearAnimal(AchievementData data, int animal)
	{
		if (data != null)
		{
			this.SetDebugDraw2(string.Concat(new object[]
			{
				"animal=",
				animal,
				" / data.value=",
				data.value
			}));
			if (this.IsClear((uint)animal, (uint)data.value))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckClearDistance(AchievementData data, uint distance)
	{
		if (data != null)
		{
			this.SetDebugDraw2(string.Concat(new object[]
			{
				"distance=",
				distance,
				" / data.value=",
				data.value
			}));
			if (this.IsClear(distance, (uint)data.value))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckClearPlayerOpen(AchievementData data, ServerCharacterState state)
	{
		if (data != null && state != null)
		{
			this.SetDebugDraw2("player IsUnlocked=" + state.IsUnlocked.ToString());
			if (state.IsUnlocked)
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckClearPlayerLevel(AchievementData data, ServerCharacterState state)
	{
		if (data != null && state != null)
		{
			this.SetDebugDraw2(string.Concat(new object[]
			{
				"player IsUnlocked=",
				state.IsUnlocked.ToString(),
				" level=",
				state.Level,
				" / data.value=",
				data.value
			}));
			if (state.IsUnlocked && this.IsClear((uint)state.Level, (uint)data.value))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckClearChaoOpen(AchievementData data, ServerChaoState state)
	{
		if (data != null && state != null)
		{
			this.SetDebugDraw2("chao IsOwned=" + state.IsOwned.ToString());
			if (state.IsOwned)
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckClearChaoLevel(AchievementData data, ServerChaoState state)
	{
		if (data != null && state != null)
		{
			this.SetDebugDraw2(string.Concat(new object[]
			{
				"chao IsOwned=",
				state.IsOwned.ToString(),
				" level=",
				state.Level,
				" / data.value=",
				data.value
			}));
			if (state.IsOwned && this.IsClear((uint)state.Level, (uint)data.value))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsClear(uint myParam, uint cmpParam)
	{
		return myParam >= cmpParam;
	}

	private bool IsClearAchievement(string id)
	{
		foreach (AchievementTempData current in this.m_clearData)
		{
			if (current.m_id == id)
			{
				return true;
			}
		}
		return false;
	}

	private void SetReporteEnd(string id)
	{
		foreach (AchievementTempData current in this.m_loadData)
		{
			if (current.m_id == id)
			{
				current.m_reportEnd = true;
				break;
			}
		}
	}

	public void SetupDataAchievementTable()
	{
		if (this.m_setupDataTable || this.m_data.Count != 0)
		{
			return;
		}
		AchievementTable achievementTable = GameObjectUtil.FindGameObjectComponent<AchievementTable>("AchievementTable");
		if (achievementTable != null)
		{
			this.m_data.Clear();
			AchievementData[] dataTable = AchievementTable.GetDataTable();
			if (dataTable != null)
			{
				AchievementData[] array = dataTable;
				for (int i = 0; i < array.Length; i++)
				{
					AchievementData item = array[i];
					this.m_data.Add(item);
				}
			}
			this.SetDebugDraw("SetupDataAchievementTable m_data=" + this.m_data.Count);
			UnityEngine.Object.Destroy(achievementTable.gameObject);
		}
		else
		{
			this.SetDebugDraw("SetupDataAchievementTable Error");
		}
		this.m_setupDataTable = true;
	}

	public void SkipAuthenticate()
	{
		this.m_state = AchievementManager.State.AuthenticateSkip;
	}

	private void DestroyDataTable()
	{
		AchievementTable achievementTable = GameObjectUtil.FindGameObjectComponent<AchievementTable>("AchievementTable");
		if (achievementTable != null)
		{
			UnityEngine.Object.Destroy(achievementTable.gameObject);
		}
	}

	private void SetWaitTime()
	{
		this.m_waitTime = AchievementManager.WAIT_TIME;
	}

	private void RequestAchievementIncentive(int incentivCount)
	{
		if (incentivCount > 0)
		{
			AchievementIncentive.AddAchievementIncentiveCount(incentivCount);
		}
		if (this.m_achievementIncentive == null)
		{
			this.SetWaitTime();
			GameObject gameObject = new GameObject("AchievementIncentive");
			this.m_achievementIncentive = gameObject.AddComponent<AchievementIncentive>();
			if (this.m_achievementIncentive != null)
			{
				this.m_achievementIncentive.RequestServer();
			}
			this.SetDebugDraw("RequestAchievementIncentive RequestServer");
		}
	}

	private bool IsRequestEndIncentive()
	{
		if (this.m_achievementIncentive != null)
		{
			AchievementIncentive.State state = this.m_achievementIncentive.GetState();
			if (state == AchievementIncentive.State.Request)
			{
				return false;
			}
			UnityEngine.Object.Destroy(this.m_achievementIncentive.gameObject);
			this.m_achievementIncentive = null;
			this.SetDebugDraw("IsRequestEndIncentive Destroy state=" + state.ToString());
		}
		return true;
	}

	private void SetNetworkConnect(bool on)
	{
		if (this.m_connectAnim == on)
		{
			return;
		}
		HudNetworkConnect component = base.gameObject.GetComponent<HudNetworkConnect>();
		if (component != null)
		{
			if (on)
			{
				this.SetDebugDraw("SetNetworkConnect PlayStart");
				component.Setup();
				component.PlayStart(HudNetworkConnect.DisplayType.ALL);
				this.m_connectAnim = true;
			}
			else
			{
				this.SetDebugDraw("SetNetworkConnect PlayEnd");
				component.PlayEnd();
				this.m_connectAnim = false;
			}
		}
	}

	private void SetDebugDraw(string msg)
	{
	}

	private void SetDebugDraw2(string msg)
	{
	}

	private bool IsDebugAllOpen()
	{
		return false;
	}

	public static AchievementManager GetAchievementManager()
	{
		AchievementManager achievementManager = AchievementManager.Instance;
		if (achievementManager == null)
		{
			GameObject gameObject = new GameObject("AchievementManager");
			achievementManager = gameObject.AddComponent<AchievementManager>();
		}
		return achievementManager;
	}

	public static void Setup()
	{
		AchievementManager instance = AchievementManager.Instance;
		if (instance != null)
		{
			instance.SetupDataAchievementTable();
		}
	}

	public static bool IsSetupEnd()
	{
		AchievementManager instance = AchievementManager.Instance;
		return !(instance != null) || instance.IsSetupEndAchievement();
	}

	public static void RequestSkipAuthenticate()
	{
		AchievementManager instance = AchievementManager.Instance;
		if (instance != null)
		{
			instance.SkipAuthenticate();
		}
	}

	public static void RequestUpdate()
	{
		AchievementManager achievementManager = AchievementManager.GetAchievementManager();
		if (achievementManager != null)
		{
			achievementManager.RequestUpdateAchievement();
		}
	}

	public static bool IsRequestEnd()
	{
		AchievementManager instance = AchievementManager.Instance;
		return !(instance != null) || instance.IsRequestEndAchievement();
	}

	public static void RequestReset()
	{
		AchievementManager achievementManager = AchievementManager.GetAchievementManager();
		if (achievementManager != null)
		{
			achievementManager.RequestResetAchievement();
		}
	}

	public static void RequestShowAchievementsUI()
	{
		AchievementManager achievementManager = AchievementManager.GetAchievementManager();
		if (achievementManager != null)
		{
			if (!Social.localUser.authenticated)
			{
				achievementManager.Authenticate();
			}
			achievementManager.ShowAchievementsUI();
		}
	}

	public static void RequestDebugInfo(bool flag)
	{
		AchievementManager achievementManager = AchievementManager.GetAchievementManager();
		if (achievementManager != null)
		{
			achievementManager.m_debugInfo = flag;
		}
	}

	public static void RequestDebugAllOpen(bool flag)
	{
		AchievementManager achievementManager = AchievementManager.GetAchievementManager();
		if (achievementManager != null)
		{
			achievementManager.m_debugAllOpen = flag;
		}
	}
}
