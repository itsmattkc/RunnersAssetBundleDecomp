using DataTable;
using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RouletteManager : CustomGameObject
{
	public delegate void CallbackRouletteInit(int specialEggNum);

	public const string REQUEST_GET_SUCCEEDED = "RequestGetRoulette_Succeeded";

	public const string REQUEST_GET_FAILED = "RequestGetRoulette_Failed";

	public const string REQUEST_COMMIT_SUCCEEDED = "RequestCommitRoulette_Succeeded";

	public const string REQUEST_COMMIT_FAILED = "RequestCommitRoulette_Failed";

	public const string REQUEST_PRIZE_SUCCEEDED = "RequestRoulettePrize_Succeeded";

	public const string REQUEST_PRIZE_FAILED = "RequestRoulettePrize_Failed";

	public const string REQUEST_BASIC_INFO_SUCCEEDED = "RequestBasicInfo_Succeeded";

	public const string REQUEST_BASIC_INFO_FAILED = "RequestBasicInfo_Failed";

	private const float DUMMY_COMMIT_DELAY = 2f;

	private Dictionary<int, GameObject> m_callbackList;

	private GameObject m_prizeCallback;

	private GameObject m_basicInfoCallback;

	private Dictionary<RouletteCategory, ServerWheelOptionsData> m_rouletteList;

	private Dictionary<RouletteCategory, ServerPrizeState> m_prizeList;

	private ServerWheelOptions m_rouletteItemBak;

	private ServerWheelOptionsGeneral m_rouletteGeneralBak;

	private ServerChaoWheelOptions m_rouletteChaoBak;

	private ServerChaoSpinResult m_resultData;

	private ServerSpinResultGeneral m_resultGeneral;

	private RouletteCategory m_rouletteGeneralBakCategory;

	private RouletteCategory m_rouletteChaoBakCategory;

	private Dictionary<RouletteCategory, float> m_loadingList;

	private RouletteCategory m_isCurrentPrizeLoading;

	private RouletteCategory m_isCurrentPrizeLoadingAuto;

	private RouletteCategory m_lastCommitCategory;

	private RouletteTop m_rouletteTop;

	private EasySnsFeed m_easySnsFeed;

	private List<RouletteCategory> m_basicRouletteCategorys;

	private DateTime m_basicRouletteCategorysGetLastTime;

	private int m_requestRouletteId;

	private bool m_currentRankup;

	private bool m_networkError;

	private static bool s_multiGetWindow;

	private float m_updateRouletteDelay;

	private static RouletteUtility.AchievementType m_achievementType;

	private static RouletteUtility.NextType m_nextType;

	private static bool s_isShowGetWindow;

	private static int s_numJackpotRing;

	private List<ServerItem.Id> m_rouletteCostItemIdList;

	private float m_rouletteCostItemIdListGetTime = -1f;

	private GameObject m_dummyCallback;

	private ServerWheelOptionsData m_dummyData;

	private float m_dummyTime;

	private Dictionary<RouletteCategory, List<CharaType>> m_picupCharaList;

	private bool m_isPicupCharaListInit;

	private DateTime m_picupCharaListTime;

	private bool m_initReq;

	private RouletteManager.CallbackRouletteInit m_initReqCallback;

	private string m_oldBgm;

	private bool m_bgmReset;

	private static RouletteManager s_instance;

	public static int numJackpotRing
	{
		get
		{
			return RouletteManager.s_numJackpotRing;
		}
		set
		{
			RouletteManager.s_numJackpotRing = value;
		}
	}

	public static bool isShowGetWindow
	{
		get
		{
			return RouletteManager.s_isShowGetWindow;
		}
		set
		{
			RouletteManager.s_isShowGetWindow = value;
		}
	}

	public static bool isMultiGetWindow
	{
		get
		{
			return RouletteManager.s_multiGetWindow;
		}
		set
		{
			RouletteManager.s_multiGetWindow = value;
		}
	}

	public bool isCurrentPrizeLoading
	{
		get
		{
			return this.m_isCurrentPrizeLoading != RouletteCategory.NONE || this.m_isCurrentPrizeLoadingAuto != RouletteCategory.NONE;
		}
	}

	public List<RouletteCategory> rouletteCategorys
	{
		get
		{
			return this.m_basicRouletteCategorys;
		}
	}

	public int specialEgg
	{
		get
		{
			return (int)GeneralUtil.GetItemCount(ServerItem.Id.SPECIAL_EGG);
		}
		set
		{
			GeneralUtil.SetItemCount(ServerItem.Id.SPECIAL_EGG, (long)value);
		}
	}

	public bool currentRankup
	{
		get
		{
			return this.m_currentRankup;
		}
	}

	public List<ServerItem.Id> rouletteCostItemIdList
	{
		get
		{
			return this.m_rouletteCostItemIdList;
		}
	}

	public string oldBgmName
	{
		get
		{
			return this.m_oldBgm;
		}
	}

	public static RouletteManager Instance
	{
		get
		{
			return RouletteManager.s_instance;
		}
	}

	public void InitRouletteRequest(RouletteManager.CallbackRouletteInit callback = null)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			this.m_initReq = true;
			this.m_initReqCallback = callback;
			if (!this.RequestRouletteOrg(RouletteCategory.PREMIUM, null))
			{
				callback(0);
				this.m_initReq = false;
				this.m_initReqCallback = null;
			}
		}
		else if (callback != null)
		{
			callback(0);
		}
	}

	private bool RouletteOpenOrg(RouletteCategory category)
	{
		this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
		bool flag = GeneralUtil.IsNetwork();
		if (this.m_networkError && flag)
		{
			this.m_networkError = false;
		}
		if (!flag)
		{
			if (RouletteTop.Instance != null)
			{
				RouletteTop.Instance.BtnInit();
			}
			this.m_networkError = true;
		}
		this.m_rouletteTop = RouletteTop.RouletteTopPageCreate();
		return this.m_rouletteTop != null;
	}

	private bool RouletteCloseOrg()
	{
		this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
		return this.m_rouletteTop != null && RouletteManager.IsRouletteEnabled() && this.m_rouletteTop.Close(RouletteUtility.NextType.NONE);
	}

	private void GetWindowClose(RouletteUtility.AchievementType achievement, RouletteUtility.NextType nextType = RouletteUtility.NextType.NONE)
	{
		if (this.m_rouletteTop != null)
		{
			if (achievement != RouletteUtility.AchievementType.Multi)
			{
				this.UpdateChangeBotton(RouletteCategory.NONE);
			}
			this.m_rouletteTop.CloseGetWindow(achievement, nextType);
		}
	}

	public void UpdateChangeBotton(RouletteCategory target = RouletteCategory.NONE)
	{
		if (this.m_rouletteTop != null)
		{
			if (target != RouletteCategory.NONE)
			{
				global::Debug.Log("UpdateChangeBotton target:" + target + " !!!!!!!!!!!!!!!!!! ");
				this.m_rouletteTop.UpdateChangeBotton(target);
			}
			else if (this.m_lastCommitCategory != RouletteCategory.NONE)
			{
				this.m_rouletteTop.UpdateChangeBotton(this.m_lastCommitCategory);
			}
		}
	}

	private bool IsRouletteEnabledOrg()
	{
		bool flag = false;
		if (this.m_rouletteTop != null)
		{
			flag = this.m_rouletteTop.gameObject.activeSelf;
			if (flag)
			{
				float panelsAlpha = this.m_rouletteTop.GetPanelsAlpha();
				if (panelsAlpha == 0f)
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	private bool OpenRouletteWindowOrg()
	{
		bool result = false;
		if (this.m_rouletteTop != null)
		{
			this.m_rouletteTop.OpenRouletteWindow();
			result = true;
		}
		return result;
	}

	private bool CloseRouletteWindowOrg()
	{
		bool result = false;
		if (this.m_rouletteTop != null)
		{
			this.m_rouletteTop.CloseRouletteWindow();
			result = true;
		}
		return result;
	}

	public static bool RouletteOpen(RouletteCategory category = RouletteCategory.NONE)
	{
		if (category == RouletteCategory.NONE || category == RouletteCategory.ALL)
		{
			category = RouletteUtility.rouletteDefault;
		}
		if (category == RouletteCategory.RAID && EventManager.Instance != null)
		{
			EventManager.EventType typeInTime = EventManager.Instance.TypeInTime;
			if (typeInTime != EventManager.EventType.RAID_BOSS)
			{
				RouletteUtility.rouletteDefault = RouletteCategory.PREMIUM;
				category = RouletteUtility.rouletteDefault;
			}
		}
		return RouletteManager.s_instance != null && RouletteManager.s_instance.RouletteOpenOrg(category);
	}

	public static bool RouletteClose()
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			RouletteManager.s_instance.RouletteCloseOrg();
		}
		return result;
	}

	public static bool IsRouletteClose()
	{
		bool result = false;
		if (RouletteManager.s_instance != null && RouletteManager.s_instance.m_rouletteTop != null)
		{
			return RouletteManager.s_instance.m_rouletteTop.IsClose();
		}
		return result;
	}

	public static void RouletteGetWindowClose(RouletteUtility.AchievementType achievement, RouletteUtility.NextType nextType = RouletteUtility.NextType.NONE)
	{
		if (achievement != RouletteUtility.AchievementType.NONE)
		{
			if (RouletteManager.IsRouletteEnabled())
			{
				if (achievement == RouletteUtility.AchievementType.Multi)
				{
					if (!RouletteUtility.IsGetOtomoOrCharaWindow())
					{
						if (!RouletteUtility.ShowGetAllOtomoAndChara())
						{
							RouletteUtility.ShowGetAllListEnd();
							RouletteManager.s_multiGetWindow = false;
							if (RouletteManager.s_instance != null)
							{
								RouletteManager.s_instance.UpdateChangeBotton(RouletteCategory.NONE);
							}
						}
					}
					else
					{
						RouletteManager.s_multiGetWindow = true;
					}
				}
				else
				{
					RouletteManager.m_achievementType = achievement;
					RouletteManager.m_nextType = nextType;
				}
			}
			else
			{
				RouletteManager.m_achievementType = RouletteUtility.AchievementType.NONE;
				RouletteManager.m_nextType = RouletteUtility.NextType.NONE;
			}
			AchievementManager.RequestUpdate();
		}
	}

	public static bool IsRouletteEnabled()
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.IsRouletteEnabledOrg();
		}
		return result;
	}

	public static bool OpenRouletteWindow()
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.OpenRouletteWindowOrg();
		}
		return result;
	}

	public static bool CloseRouletteWindow()
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.CloseRouletteWindowOrg();
		}
		return result;
	}

	public void Init()
	{
		base.AddUpdateCustom(new CustomGameObject.UpdateCustom(this.OnUpdateCustom), "Check", 0.1f);
		this.m_currentRankup = false;
	}

	protected override void UpdateStd(float deltaTime, float timeRate)
	{
		this.UpdateLoading(deltaTime);
		if (RouletteManager.m_achievementType != RouletteUtility.AchievementType.NONE && AchievementManager.IsRequestEnd())
		{
			if (RouletteManager.IsRouletteEnabled())
			{
				this.GetWindowClose(RouletteManager.m_achievementType, RouletteManager.m_nextType);
			}
			RouletteManager.m_achievementType = RouletteUtility.AchievementType.NONE;
			RouletteManager.m_nextType = RouletteUtility.NextType.NONE;
		}
		if (this.m_dummyTime > 0f)
		{
			this.m_dummyTime -= Time.deltaTime;
			if (this.m_dummyTime <= 0f && this.m_dummyCallback != null && this.m_dummyData != null)
			{
				this.m_dummyCallback.SendMessage("RequestCommitRoulette_Succeeded", this.m_dummyData, SendMessageOptions.DontRequireReceiver);
				this.m_dummyTime = 0f;
				this.m_dummyCallback = null;
			}
		}
		if (this.m_updateRouletteDelay > 0f)
		{
			this.m_updateRouletteDelay -= Time.deltaTime;
			if (this.m_updateRouletteDelay <= 0f)
			{
				if (this.m_rouletteItemBak != null && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.ITEM) && this.m_rouletteGeneralBakCategory == RouletteCategory.NONE)
				{
					this.m_rouletteList[RouletteCategory.ITEM].Setup(this.m_rouletteItemBak);
					this.m_rouletteItemBak = null;
					if (this.m_rouletteTop != null)
					{
						this.m_rouletteTop.UpdateWheel(this.m_rouletteList[RouletteCategory.ITEM], false);
					}
				}
				else if (this.m_rouletteGeneralBak != null && this.m_rouletteGeneralBakCategory != RouletteCategory.NONE)
				{
					RouletteCategory rouletteGeneralBakCategory = this.m_rouletteGeneralBakCategory;
					if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(rouletteGeneralBakCategory))
					{
						this.m_rouletteList[rouletteGeneralBakCategory].Setup(this.m_rouletteGeneralBak);
						this.m_rouletteGeneralBak = null;
						this.m_rouletteGeneralBakCategory = RouletteCategory.NONE;
						if (this.m_rouletteTop != null)
						{
							this.m_rouletteTop.UpdateWheel(this.m_rouletteList[rouletteGeneralBakCategory], false);
						}
					}
				}
				else if (this.m_rouletteChaoBak != null && this.m_rouletteChaoBakCategory != RouletteCategory.NONE)
				{
					RouletteCategory rouletteChaoBakCategory = this.m_rouletteChaoBakCategory;
					if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(rouletteChaoBakCategory))
					{
						this.m_rouletteList[rouletteChaoBakCategory].Setup(this.m_rouletteChaoBak);
						this.m_rouletteChaoBak = null;
						this.m_rouletteChaoBakCategory = RouletteCategory.NONE;
						if (this.m_rouletteTop != null)
						{
							this.m_rouletteTop.UpdateWheel(this.m_rouletteList[rouletteChaoBakCategory], false);
						}
					}
				}
				this.m_updateRouletteDelay = 0f;
			}
		}
		if (this.m_rouletteCostItemIdListGetTime < 0f && GeneralWindow.IsCreated("ShowNoCommunicationCostItem") && GeneralWindow.IsOkButtonPressed)
		{
			if (GeneralUtil.IsNetwork())
			{
				if (this.m_rouletteCostItemIdList != null && this.m_rouletteCostItemIdList.Count > 0)
				{
					List<int> list = new List<int>();
					foreach (ServerItem.Id current in this.m_rouletteCostItemIdList)
					{
						list.Add((int)current);
					}
					ServerInterface.LoggedInServerInterface.RequestServerGetItemStockNum(EventManager.Instance.Id, list, base.gameObject);
				}
			}
			else
			{
				GeneralUtil.ShowNoCommunication("ShowNoCommunicationCostItem");
			}
		}
	}

	private void OnUpdateCustom(string updateName, float timeRate)
	{
		if (RouletteManager.isShowGetWindow)
		{
			ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
			if (itemGetWindow != null && itemGetWindow.IsCreated("Jackpot") && itemGetWindow.IsEnd)
			{
				if (itemGetWindow.IsYesButtonPressed)
				{
					this.m_easySnsFeed = new EasySnsFeed(this.m_rouletteTop.gameObject, "Camera/menu_Anim/RouletteTopUI/Anchor_5_MC", RouletteManager.GetText("feed_jackpot_caption", null), RouletteUtility.jackpotFeedText, null);
					global::Debug.Log("Jackpot feed text:" + RouletteUtility.jackpotFeedText + " !!!!!");
				}
				else if (RouletteManager.IsRouletteEnabled())
				{
					this.GetWindowClose(RouletteUtility.AchievementType.NONE, RouletteUtility.NextType.NONE);
				}
				RouletteManager.isShowGetWindow = false;
				itemGetWindow.Reset();
			}
			if (GeneralWindow.IsCreated("RouletteGetAllList"))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					RouletteManager.s_multiGetWindow = false;
					RouletteManager.isShowGetWindow = false;
					GeneralWindow.Close();
				}
			}
			else if (GeneralWindow.IsCreated("RouletteGetAllListEnd"))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					RouletteManager.isShowGetWindow = false;
					GeneralWindow.Close();
				}
			}
			else if (GeneralWindow.IsCreated("RouletteGetAllListEndChara"))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHARA_MAIN, true);
					}
					RouletteManager.isShowGetWindow = false;
					GeneralWindow.Close();
				}
			}
			else if (GeneralWindow.IsCreated("RouletteGetAllListEndChao") && GeneralWindow.IsButtonPressed)
			{
				if (GeneralWindow.IsYesButtonPressed)
				{
					HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHAO, true);
				}
				RouletteManager.isShowGetWindow = false;
				GeneralWindow.Close();
			}
		}
		if (this.m_easySnsFeed != null)
		{
			EasySnsFeed.Result result = this.m_easySnsFeed.Update();
			if (result == EasySnsFeed.Result.COMPLETED || result == EasySnsFeed.Result.FAILED)
			{
				if (RouletteManager.IsRouletteEnabled())
				{
					this.GetWindowClose(RouletteUtility.AchievementType.NONE, RouletteUtility.NextType.NONE);
				}
				RouletteManager.isShowGetWindow = false;
				this.m_easySnsFeed = null;
			}
		}
		if (RouletteManager.s_multiGetWindow && !RouletteUtility.IsGetOtomoOrCharaWindow() && !RouletteUtility.ShowGetAllOtomoAndChara())
		{
			RouletteUtility.ShowGetAllListEnd();
			RouletteManager.s_multiGetWindow = false;
		}
		if (this.m_isCurrentPrizeLoadingAuto != RouletteCategory.NONE)
		{
			RouletteCategory isCurrentPrizeLoadingAuto = this.m_isCurrentPrizeLoadingAuto;
			switch (isCurrentPrizeLoadingAuto)
			{
			case RouletteCategory.PREMIUM:
				if (ServerInterface.PremiumRoulettePrizeList != null && ServerInterface.PremiumRoulettePrizeList.IsData())
				{
					this.SetPrizeList(this.m_isCurrentPrizeLoadingAuto, ServerInterface.PremiumRoulettePrizeList);
					this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
				}
				return;
			case RouletteCategory.ITEM:
				IL_212:
				if (isCurrentPrizeLoadingAuto != RouletteCategory.SPECIAL)
				{
					this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
					return;
				}
				if (ServerInterface.SpecialRoulettePrizeList != null && ServerInterface.SpecialRoulettePrizeList.IsData())
				{
					this.SetPrizeList(this.m_isCurrentPrizeLoadingAuto, ServerInterface.SpecialRoulettePrizeList);
					this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
				}
				return;
			case RouletteCategory.RAID:
				if (ServerInterface.RaidRoulettePrizeList != null && ServerInterface.RaidRoulettePrizeList.IsData())
				{
					this.SetPrizeList(this.m_isCurrentPrizeLoadingAuto, ServerInterface.RaidRoulettePrizeList);
					this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
				}
				return;
			}
			goto IL_212;
		}
	}

	public void RouletteBgmResetOrg()
	{
		SoundManager.BgmStop();
		this.m_oldBgm = null;
		this.m_bgmReset = false;
		base.RemoveCallback(null);
	}

	public void PlayBgmOrg(string soundName, float delay = 0f, string cueSheetName = "BGM", bool bgmReset = false)
	{
		if (!string.IsNullOrEmpty(soundName) && RouletteManager.IsRouletteEnabled())
		{
			string text = soundName + ":" + cueSheetName;
			if (delay <= 0f)
			{
				bool flag = false;
				if (bgmReset)
				{
					flag = true;
				}
				else if (!string.IsNullOrEmpty(this.m_oldBgm))
				{
					if (this.m_oldBgm != text)
					{
						flag = true;
					}
					string text2 = this.m_oldBgm;
					if (text2.IndexOf(":") >= 0)
					{
						string[] array = text2.Split(new char[]
						{
							':'
						});
						if (array != null && array.Length > 1)
						{
							text2 = array[0];
						}
					}
					if (text2 == soundName)
					{
						return;
					}
				}
				this.m_oldBgm = text;
				if (flag)
				{
					SoundManager.BgmChange(soundName, cueSheetName);
				}
				else
				{
					SoundManager.BgmChange(soundName, cueSheetName);
				}
			}
			else
			{
				bool flag2 = true;
				if (!bgmReset)
				{
					if (!string.IsNullOrEmpty(this.m_oldBgm) && this.m_oldBgm == text)
					{
						flag2 = false;
					}
				}
				else
				{
					this.m_oldBgm = null;
				}
				if (flag2)
				{
					this.m_bgmReset = bgmReset;
					base.RemoveCallbackPartialMatch("bgm_sys_");
					base.AddCallback(new CustomGameObject.Callback(this.OnCallbackBgm), text, delay);
				}
				else
				{
					this.m_bgmReset = false;
					this.m_oldBgm = null;
				}
			}
		}
	}

	public void PlaySeOrg(string soundName, float delay = 0f, string cueSheetName = "SE")
	{
		if (!string.IsNullOrEmpty(soundName) && RouletteManager.IsRouletteEnabled())
		{
			if (delay <= 0f)
			{
				SoundManager.SePlay(soundName, cueSheetName);
			}
			else
			{
				base.AddCallback(new CustomGameObject.Callback(this.OnCallbackSe), soundName + ":" + cueSheetName, delay);
			}
		}
	}

	private void OnCallbackBgm(string callbackName)
	{
		if (!string.IsNullOrEmpty(callbackName) && RouletteManager.IsRouletteEnabled())
		{
			string cueName = callbackName;
			string cueSheetName = "BGM";
			if (callbackName.IndexOf(":") >= 0)
			{
				string[] array = callbackName.Split(new char[]
				{
					':'
				});
				if (array.Length > 1)
				{
					cueSheetName = array[1];
					cueName = array[0];
				}
			}
			bool flag = false;
			if (this.m_bgmReset)
			{
				flag = true;
			}
			else if (!string.IsNullOrEmpty(this.m_oldBgm) && this.m_oldBgm != callbackName)
			{
				flag = true;
			}
			this.m_oldBgm = callbackName;
			if (flag)
			{
				SoundManager.BgmChange(cueName, cueSheetName);
			}
			else
			{
				SoundManager.BgmChange(cueName, cueSheetName);
			}
		}
	}

	private void OnCallbackSe(string callbackName)
	{
		if (!string.IsNullOrEmpty(callbackName) && RouletteManager.IsRouletteEnabled())
		{
			string cueName = callbackName;
			string cueSheetName = "SE";
			if (callbackName.IndexOf(":") >= 0)
			{
				string[] array = callbackName.Split(new char[]
				{
					':'
				});
				if (array.Length > 1)
				{
					cueSheetName = array[1];
					cueName = array[0];
				}
			}
			SoundManager.SePlay(cueName, cueSheetName);
		}
	}

	public static void RouletteBgmReset()
	{
		if (RouletteManager.s_instance != null)
		{
			RouletteManager.s_instance.RouletteBgmResetOrg();
		}
	}

	public static void PlayBgm(string soundName, float delay = 0f, string cueSheetName = "BGM", bool bgmReset = false)
	{
		if (RouletteManager.s_instance != null)
		{
			RouletteManager.s_instance.PlayBgmOrg(soundName, delay, cueSheetName, bgmReset);
		}
	}

	public static void PlaySe(string soundName, float delay = 0f, string cueSheetName = "SE")
	{
		if (RouletteManager.s_instance != null)
		{
			RouletteManager.s_instance.PlaySeOrg(soundName, delay, cueSheetName);
		}
	}

	public Dictionary<RouletteCategory, float> GetCurrentLoadingOrg()
	{
		if (this.m_loadingList != null && this.m_loadingList.Count > 0)
		{
			return this.m_loadingList;
		}
		return null;
	}

	public bool IsLoadingOrg(RouletteCategory category)
	{
		bool result = false;
		if (category != RouletteCategory.ALL)
		{
			if (this.m_loadingList != null && this.m_loadingList.ContainsKey(category))
			{
				result = true;
			}
		}
		else if (this.m_loadingList != null && this.m_loadingList.Count > 0)
		{
			result = true;
		}
		return result;
	}

	public bool IsPrizeLoadingOrg(RouletteCategory category)
	{
		if (category == RouletteCategory.ALL)
		{
			return this.m_isCurrentPrizeLoading != RouletteCategory.NONE;
		}
		return this.m_isCurrentPrizeLoading == category;
	}

	private bool StartLoading(RouletteCategory category)
	{
		bool result = false;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL)
		{
			if (this.m_loadingList == null)
			{
				this.m_loadingList = new Dictionary<RouletteCategory, float>();
				this.m_loadingList.Add(category, 0f);
				result = true;
			}
			else if (this.m_loadingList.ContainsKey(category))
			{
				if (this.m_loadingList[category] < 0f)
				{
					this.m_loadingList[category] = 0f;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else
			{
				this.m_loadingList.Add(category, 0f);
				result = true;
			}
		}
		this.m_currentRankup = false;
		return result;
	}

	private void UpdateLoading(float deltaTime)
	{
		if (this.m_loadingList != null && this.m_loadingList.Count > 0)
		{
			List<RouletteCategory> list = new List<RouletteCategory>(this.m_loadingList.Keys);
			RouletteCategory rouletteCategory = RouletteCategory.NONE;
			foreach (RouletteCategory current in list)
			{
				if (this.m_loadingList[current] < 0f)
				{
					rouletteCategory = current;
				}
				else
				{
					Dictionary<RouletteCategory, float> loadingList;
					Dictionary<RouletteCategory, float> expr_66 = loadingList = this.m_loadingList;
					RouletteCategory key;
					RouletteCategory expr_6A = key = current;
					float num = loadingList[key];
					expr_66[expr_6A] = num + deltaTime;
				}
			}
			if (rouletteCategory != RouletteCategory.NONE)
			{
				this.m_loadingList.Remove(rouletteCategory);
			}
		}
	}

	private float GetLoadingTime(RouletteCategory category)
	{
		float result = -1f;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && this.m_loadingList != null && this.m_loadingList.Count > 0 && this.m_loadingList.ContainsKey(category))
		{
			result = this.m_loadingList[category];
		}
		return result;
	}

	private bool EndLoading(RouletteCategory category)
	{
		bool result = false;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && this.m_loadingList != null && this.m_loadingList.Count > 0 && this.m_loadingList.ContainsKey(category))
		{
			this.m_loadingList[category] = -1f;
			result = true;
		}
		return result;
	}

	public bool RequestRouletteOrg(RouletteCategory category, GameObject callbackObject = null)
	{
		bool result = false;
		this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL)
		{
			if (category == RouletteCategory.SPECIAL)
			{
				category = RouletteCategory.PREMIUM;
			}
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				if (this.StartLoading(category))
				{
					int id = EventManager.Instance.Id;
					switch (category)
					{
					case RouletteCategory.PREMIUM:
						global::Debug.Log("RequestRouletteOrg : RouletteCategory.PREMIUM");
						this.SetCallbackObject((int)(1000 + category), callbackObject);
						loggedInServerInterface.RequestServerGetChaoWheelOptions(base.gameObject);
						goto IL_100;
					case RouletteCategory.ITEM:
						global::Debug.Log("RequestRouletteOrg : RouletteCategory.ITEM");
						this.SetCallbackObject((int)(1000 + category), callbackObject);
						loggedInServerInterface.RequestServerGetWheelOptions(base.gameObject);
						goto IL_100;
					}
					this.EndLoading(category);
					this.StartLoading(RouletteCategory.GENERAL);
					this.m_requestRouletteId = (int)category;
					this.m_requestRouletteId = 0;
					global::Debug.Log("RequestRouletteOrg : RouletteCategory.RAID");
					this.SetCallbackObject(this.m_requestRouletteId, callbackObject);
					loggedInServerInterface.RequestServerGetWheelOptionsGeneral(id, 0, base.gameObject);
					IL_100:
					result = true;
				}
				else
				{
					result = false;
				}
			}
		}
		return result;
	}

	private ServerWheelOptionsData UpdateRouletteOrg(RouletteCategory category = RouletteCategory.NONE, float delay = 0f)
	{
		if (delay <= 0f)
		{
			if (category != RouletteCategory.NONE && category != RouletteCategory.ALL)
			{
				if (this.m_rouletteItemBak != null && category == RouletteCategory.ITEM && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.ITEM) && this.m_rouletteGeneralBakCategory == RouletteCategory.NONE)
				{
					this.m_rouletteList[RouletteCategory.ITEM].Setup(this.m_rouletteItemBak);
					this.m_rouletteItemBak = null;
					return this.m_rouletteList[RouletteCategory.ITEM];
				}
				if (this.m_rouletteGeneralBak != null && this.m_rouletteGeneralBakCategory != RouletteCategory.NONE)
				{
					RouletteCategory rouletteGeneralBakCategory = this.m_rouletteGeneralBakCategory;
					if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(rouletteGeneralBakCategory))
					{
						this.m_rouletteList[rouletteGeneralBakCategory].Setup(this.m_rouletteGeneralBak);
						this.m_rouletteGeneralBak = null;
						this.m_rouletteGeneralBakCategory = RouletteCategory.NONE;
						return this.m_rouletteList[rouletteGeneralBakCategory];
					}
				}
				else if (this.m_rouletteChaoBak != null && this.m_rouletteChaoBakCategory != RouletteCategory.NONE)
				{
					RouletteCategory rouletteChaoBakCategory = this.m_rouletteChaoBakCategory;
					if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(rouletteChaoBakCategory))
					{
						this.m_rouletteList[rouletteChaoBakCategory].Setup(this.m_rouletteChaoBak);
						this.m_rouletteChaoBak = null;
						this.m_rouletteChaoBakCategory = RouletteCategory.NONE;
						return this.m_rouletteList[rouletteChaoBakCategory];
					}
				}
			}
		}
		else if (category != RouletteCategory.NONE && category != RouletteCategory.ALL)
		{
			if (this.m_rouletteItemBak != null && category == RouletteCategory.ITEM && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.ITEM))
			{
				this.m_updateRouletteDelay = delay;
			}
			else if (this.m_rouletteGeneralBak != null && this.m_rouletteGeneralBakCategory != RouletteCategory.NONE)
			{
				if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(this.m_rouletteGeneralBakCategory))
				{
					this.m_updateRouletteDelay = delay;
				}
			}
			else if (this.m_rouletteChaoBak != null && this.m_rouletteChaoBakCategory != RouletteCategory.NONE && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(this.m_rouletteChaoBakCategory))
			{
				this.m_updateRouletteDelay = delay;
			}
		}
		return null;
	}

	public bool IsRequestPicupCharaList()
	{
		bool result = false;
		if (this.m_isPicupCharaListInit && (NetBase.GetCurrentTime() - this.m_picupCharaListTime).TotalHours <= 1.0)
		{
			result = true;
		}
		return result;
	}

	public bool RequestPicupCharaList(bool isImmediatelyUpdate = false)
	{
		bool result = false;
		bool flag = false;
		if (!this.m_isPicupCharaListInit)
		{
			flag = true;
		}
		else if (isImmediatelyUpdate)
		{
			if ((NetBase.GetCurrentTime() - this.m_picupCharaListTime).TotalMinutes > 1.0)
			{
				flag = true;
			}
		}
		else if ((NetBase.GetCurrentTime() - this.m_picupCharaListTime).TotalHours > 1.0)
		{
			flag = true;
		}
		if (flag)
		{
			this.m_picupCharaListTime = NetBase.GetCurrentTime();
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				if (this.GetPrizeList(RouletteCategory.PREMIUM) == null)
				{
					loggedInServerInterface.RequestServerGetPrizeChaoWheelSpin(0, base.gameObject);
				}
				if (this.GetPrizeList(RouletteCategory.SPECIAL) == null)
				{
					loggedInServerInterface.RequestServerGetPrizeChaoWheelSpin(1, base.gameObject);
				}
				if (EventManager.Instance != null && EventManager.Instance.TypeInTime == EventManager.EventType.RAID_BOSS && this.GetPrizeList(RouletteCategory.RAID) == null)
				{
					int id = EventManager.Instance.Id;
					int spinType = 0;
					loggedInServerInterface.RequestServerGetPrizeWheelSpinGeneral(id, spinType, base.gameObject);
				}
				result = true;
			}
		}
		this.m_isPicupCharaListInit = true;
		return result;
	}

	public bool RequestRoulettePrizeOrg(RouletteCategory category, GameObject callbackObject = null)
	{
		bool result = false;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL)
		{
			this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
			ServerPrizeState serverPrizeState = this.GetPrizeList(category);
			this.m_prizeCallback = callbackObject;
			if (category == RouletteCategory.PREMIUM && this.specialEgg >= 10)
			{
				category = RouletteCategory.SPECIAL;
			}
			if (serverPrizeState == null && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(category))
			{
				ServerWheelOptionsData serverWheelOptionsData = this.m_rouletteList[category];
				if (serverWheelOptionsData != null)
				{
					if (serverWheelOptionsData.IsPrizeDataList())
					{
						if (!this.IsPrizeLoadingOrg(category))
						{
							ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
							if (loggedInServerInterface != null)
							{
								if (serverWheelOptionsData.isGeneral)
								{
									int eventId = 0;
									int spinType = 0;
									if (EventManager.Instance != null)
									{
										eventId = EventManager.Instance.Id;
									}
									loggedInServerInterface.RequestServerGetPrizeWheelSpinGeneral(eventId, spinType, base.gameObject);
								}
								else
								{
									int chaoWheelSpinType = (category != RouletteCategory.SPECIAL) ? 0 : 1;
									loggedInServerInterface.RequestServerGetPrizeChaoWheelSpin(chaoWheelSpinType, base.gameObject);
								}
								this.m_isCurrentPrizeLoading = category;
							}
						}
					}
					else
					{
						serverPrizeState = this.CreatePrizeList(category);
						if (this.m_prizeCallback != null && serverPrizeState != null)
						{
							if (RouletteManager.IsRouletteEnabled())
							{
								this.m_prizeCallback.SendMessage("RequestRoulettePrize_Succeeded", serverPrizeState, SendMessageOptions.DontRequireReceiver);
								this.m_prizeCallback = null;
								this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
							}
							else
							{
								global::Debug.Log("RouletteManager RequestRoulettePrizeOrg RouletteTop:false");
							}
						}
					}
				}
			}
			else if (this.m_prizeCallback != null)
			{
				if (RouletteManager.IsRouletteEnabled())
				{
					if (serverPrizeState != null)
					{
						this.m_prizeCallback.SendMessage("RequestRoulettePrize_Succeeded", serverPrizeState, SendMessageOptions.DontRequireReceiver);
						this.m_prizeCallback = null;
						this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
					}
					else
					{
						ServerInterface loggedInServerInterface2 = ServerInterface.LoggedInServerInterface;
						if (loggedInServerInterface2 != null)
						{
							if (category == RouletteCategory.ITEM)
							{
								serverPrizeState = this.CreatePrizeList(category);
								if (serverPrizeState == null)
								{
									this.m_isCurrentPrizeLoading = category;
									loggedInServerInterface2.RequestServerGetWheelOptions(base.gameObject);
								}
								else
								{
									this.m_prizeCallback.SendMessage("RequestRoulettePrize_Succeeded", serverPrizeState, SendMessageOptions.DontRequireReceiver);
									this.m_prizeCallback = null;
									this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
								}
							}
							else if (category == RouletteCategory.PREMIUM || category == RouletteCategory.SPECIAL)
							{
								this.m_isCurrentPrizeLoading = category;
								int chaoWheelSpinType2 = (category != RouletteCategory.SPECIAL) ? 0 : 1;
								loggedInServerInterface2.RequestServerGetPrizeChaoWheelSpin(chaoWheelSpinType2, base.gameObject);
							}
							else if (category == RouletteCategory.RAID)
							{
								int eventId2 = 0;
								int spinType2 = 0;
								if (EventManager.Instance != null)
								{
									eventId2 = EventManager.Instance.Id;
								}
								this.m_isCurrentPrizeLoading = category;
								loggedInServerInterface2.RequestServerGetPrizeWheelSpinGeneral(eventId2, spinType2, base.gameObject);
							}
							else
							{
								this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
								global::Debug.Log("RouletteManager RequestRoulettePrizeOrg RouletteTop:false");
							}
						}
						else
						{
							this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
							global::Debug.Log("RouletteManager RequestRoulettePrizeOrg RouletteTop:false");
						}
					}
				}
				else
				{
					this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
					global::Debug.Log("RouletteManager RequestRoulettePrizeOrg RouletteTop:false");
				}
			}
		}
		return result;
	}

	public void SetPrizeList(RouletteCategory category, ServerPrizeState prizeState)
	{
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && prizeState != null && prizeState.IsData())
		{
			if (this.m_prizeList == null)
			{
				this.m_prizeList = new Dictionary<RouletteCategory, ServerPrizeState>();
				this.m_prizeList.Add(category, prizeState);
			}
			else if (this.m_prizeList.ContainsKey(category))
			{
				this.m_prizeList[category] = prizeState;
			}
			else
			{
				this.m_prizeList.Add(category, prizeState);
			}
			this.SetPicupCharaList(category, prizeState);
		}
	}

	private void SetPicupCharaList(RouletteCategory category, ServerPrizeState prizeState)
	{
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && prizeState != null)
		{
			if (this.m_picupCharaList == null)
			{
				this.m_picupCharaList = new Dictionary<RouletteCategory, List<CharaType>>();
			}
			if (this.m_picupCharaList.ContainsKey(category))
			{
				this.m_picupCharaList[category].Clear();
				if (prizeState.IsData())
				{
					foreach (ServerPrizeData current in prizeState.prizeList)
					{
						if (current.itemId >= 300000 && current.itemId < 400000)
						{
							ServerItem serverItem = new ServerItem((ServerItem.Id)current.itemId);
							if (serverItem.idType == ServerItem.IdType.CHARA && !this.m_picupCharaList[category].Contains(serverItem.charaType))
							{
								this.m_picupCharaList[category].Add(serverItem.charaType);
							}
						}
					}
				}
			}
			else
			{
				List<CharaType> list = new List<CharaType>();
				if (prizeState.IsData())
				{
					foreach (ServerPrizeData current2 in prizeState.prizeList)
					{
						if (current2.itemId >= 300000 && current2.itemId < 400000)
						{
							ServerItem serverItem2 = new ServerItem((ServerItem.Id)current2.itemId);
							if (serverItem2.idType == ServerItem.IdType.CHARA && !list.Contains(serverItem2.charaType))
							{
								list.Add(serverItem2.charaType);
							}
						}
					}
				}
				this.m_picupCharaList.Add(category, list);
			}
		}
	}

	public ServerPrizeState GetPrizeList(RouletteCategory category)
	{
		ServerPrizeState result = null;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL)
		{
			if (this.m_prizeList == null)
			{
				this.m_prizeList = new Dictionary<RouletteCategory, ServerPrizeState>();
				result = null;
			}
			else if (this.m_prizeList.ContainsKey(category) && !this.m_prizeList[category].IsExpired())
			{
				result = this.m_prizeList[category];
			}
			switch (category)
			{
			case RouletteCategory.PREMIUM:
				if (ServerInterface.PremiumRoulettePrizeList != null && ServerInterface.PremiumRoulettePrizeList.IsData())
				{
					this.SetPrizeList(category, ServerInterface.PremiumRoulettePrizeList);
					result = ServerInterface.PremiumRoulettePrizeList;
				}
				return result;
			case RouletteCategory.ITEM:
				IL_77:
				if (category != RouletteCategory.SPECIAL)
				{
					return result;
				}
				if (ServerInterface.SpecialRoulettePrizeList != null && ServerInterface.SpecialRoulettePrizeList.IsData())
				{
					this.SetPrizeList(category, ServerInterface.SpecialRoulettePrizeList);
					result = ServerInterface.SpecialRoulettePrizeList;
				}
				return result;
			case RouletteCategory.RAID:
				if (ServerInterface.RaidRoulettePrizeList != null && ServerInterface.RaidRoulettePrizeList.IsData())
				{
					this.SetPrizeList(category, ServerInterface.RaidRoulettePrizeList);
					result = ServerInterface.RaidRoulettePrizeList;
				}
				return result;
			}
			goto IL_77;
		}
		return result;
	}

	private ServerPrizeState CreatePrizeList(RouletteCategory category)
	{
		ServerPrizeState serverPrizeState = null;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(category))
		{
			ServerWheelOptionsData data = this.m_rouletteList[category];
			serverPrizeState = new ServerPrizeState(data);
			if (this.m_prizeList == null)
			{
				this.m_prizeList = new Dictionary<RouletteCategory, ServerPrizeState>();
			}
			if (this.m_prizeList.ContainsKey(category))
			{
				this.m_prizeList[category] = serverPrizeState;
			}
			else
			{
				this.m_prizeList.Add(category, serverPrizeState);
			}
		}
		return serverPrizeState;
	}

	public void DummyCommit(ServerWheelOptionsData data, GameObject callbackObject = null)
	{
		global::Debug.Log("DummyCommit !!!!!!!!!!!!!!!!!!!!!!!!!");
		if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.PREMIUM))
		{
			ServerWheelOptionsData serverWheelOptionsData = this.m_rouletteList[RouletteCategory.PREMIUM];
			if (serverWheelOptionsData != null)
			{
				ServerSpinResultGeneral serverSpinResultGeneral = null;
				ServerChaoSpinResult serverChaoSpinResult = null;
				if (data.isGeneral)
				{
					serverSpinResultGeneral = new ServerSpinResultGeneral();
					this.m_dummyData = new ServerWheelOptionsData(data.GetOrgGeneralData());
				}
				else
				{
					serverChaoSpinResult = new ServerChaoSpinResult();
					this.m_dummyData = new ServerWheelOptionsData(data.GetOrgNormalData());
				}
				ServerChaoData serverChaoData = null;
				ChaoData[] dataTable = ChaoTable.GetDataTable();
				if (dataTable != null)
				{
					for (int i = 0; i < dataTable.Length; i++)
					{
						if (dataTable[i].level >= 0)
						{
							serverChaoData = new ServerChaoData();
							serverChaoData.Id = dataTable[i].id + 400000;
							serverChaoData.Level = dataTable[i].level;
							serverChaoData.Rarity = (int)dataTable[i].rarity;
							break;
						}
					}
				}
				List<int> list = new List<int>();
				if (serverChaoData != null)
				{
					for (int j = 0; j < 16; j++)
					{
						int cellEgg = data.GetCellEgg(j);
						if (cellEgg == serverChaoData.Rarity)
						{
							list.Add(j);
						}
					}
					if (list.Count <= 0)
					{
						list.Add(1);
					}
					int itemWon = list[UnityEngine.Random.Range(0, list.Count)];
					if (serverChaoSpinResult != null)
					{
						serverChaoSpinResult.AcquiredChaoData = serverChaoData;
						serverChaoSpinResult.IsRequiredChao = true;
						serverChaoSpinResult.ItemWon = itemWon;
					}
					if (serverSpinResultGeneral != null)
					{
						serverSpinResultGeneral.AddChaoState(serverChaoData);
						serverSpinResultGeneral.ItemWon = itemWon;
					}
					this.m_resultData = serverChaoSpinResult;
					this.m_resultGeneral = serverSpinResultGeneral;
				}
				this.m_dummyCallback = callbackObject;
				this.m_dummyTime = 2f;
			}
		}
	}

	public bool RequestCommitRouletteOrg(ServerWheelOptionsData data, int num, GameObject callbackObject = null)
	{
		if (data == null)
		{
			return false;
		}
		bool result = false;
		RouletteCategory rouletteCategory = data.category;
		if (rouletteCategory != RouletteCategory.NONE && rouletteCategory != RouletteCategory.ALL)
		{
			if (rouletteCategory == RouletteCategory.SPECIAL)
			{
				rouletteCategory = RouletteCategory.PREMIUM;
			}
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.ResetResult();
				this.m_lastCommitCategory = RouletteCategory.NONE;
				if (this.StartLoading(rouletteCategory))
				{
					int id = EventManager.Instance.Id;
					switch (rouletteCategory)
					{
					case RouletteCategory.PREMIUM:
					{
						global::Debug.Log("RequestCommitRouletteOrg : RouletteCategory.PREMIUM");
						int multi = data.multi;
						this.SetCallbackObject((int)(1000 + rouletteCategory), callbackObject);
						loggedInServerInterface.RequestServerCommitChaoWheelSpin(multi, base.gameObject);
						goto IL_13A;
					}
					case RouletteCategory.ITEM:
						global::Debug.Log("RequestCommitRouletteOrg : RouletteCategory.ITEM");
						this.SetCallbackObject((int)(1000 + rouletteCategory), callbackObject);
						loggedInServerInterface.RequestServerCommitWheelSpin(1, base.gameObject);
						goto IL_13A;
					}
					this.m_requestRouletteId = (int)rouletteCategory;
					this.m_requestRouletteId = 0;
					global::Debug.Log("RequestCommitRouletteOrg : RouletteCategory.RAID");
					this.EndLoading(rouletteCategory);
					this.StartLoading(RouletteCategory.GENERAL);
					int spinCostItemId = data.GetSpinCostItemId();
					int multi2 = data.multi;
					this.SetCallbackObject(this.m_requestRouletteId, callbackObject);
					loggedInServerInterface.RequestServerCommitWheelSpinGeneral(id, this.m_requestRouletteId, spinCostItemId, multi2, base.gameObject);
					IL_13A:
					result = true;
				}
				else
				{
					result = false;
				}
			}
		}
		return result;
	}

	public void ResetRouletteOrg(RouletteCategory category)
	{
		if (this.m_rouletteList != null && this.m_rouletteList.Count > 0 && category != RouletteCategory.NONE)
		{
			if (category == RouletteCategory.SPECIAL)
			{
				category = RouletteCategory.PREMIUM;
			}
			if (category != RouletteCategory.ALL)
			{
				if (this.m_rouletteList.ContainsKey(category))
				{
					this.m_rouletteList.Remove(category);
				}
			}
			else
			{
				this.m_rouletteList.Clear();
			}
		}
	}

	public void RequestRouletteBasicInformation(GameObject callback = null)
	{
		bool flag = false;
		this.m_basicInfoCallback = callback;
		if (this.m_basicRouletteCategorys == null || this.m_basicRouletteCategorys.Count <= 0 || this.m_basicInfoCallback == null)
		{
			flag = true;
			this.m_basicRouletteCategorysGetLastTime = NetUtil.GetCurrentTime();
		}
		else if ((NetUtil.GetCurrentTime() - this.m_basicRouletteCategorysGetLastTime).TotalMinutes > 5.0)
		{
			flag = true;
		}
		if (flag)
		{
			this.ServerGetRouletteList_Succeeded(null);
		}
		else
		{
			if (this.m_basicInfoCallback != null)
			{
				this.m_basicInfoCallback.SendMessage("RequestBasicInfo_Succeeded", this.m_basicRouletteCategorys, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				GeneralUtil.SetRouletteBtnIcon(null, "Btn_roulette");
			}
			if (this.m_rouletteCostItemIdList != null && this.m_rouletteCostItemIdList.Count > 0)
			{
				RouletteTop.Instance.UpdateCostItemList(this.m_rouletteCostItemIdList);
				this.m_rouletteCostItemIdListGetTime = Time.realtimeSinceStartup;
			}
		}
	}

	private void ServerGetRouletteList_Succeeded(MsgGetItemStockNumSucceed msg)
	{
		this.m_basicRouletteCategorysGetLastTime = NetUtil.GetCurrentTime();
		EventManager.EventType typeInTime = EventManager.Instance.TypeInTime;
		if (this.m_basicRouletteCategorys != null && this.m_basicRouletteCategorys.Contains(RouletteCategory.RAID) && typeInTime != EventManager.EventType.RAID_BOSS)
		{
			this.m_rouletteCostItemIdListGetTime = -1f;
		}
		this.m_basicRouletteCategorys = new List<RouletteCategory>();
		this.m_rouletteCostItemIdList = new List<ServerItem.Id>();
		this.m_basicRouletteCategorys.Add(RouletteCategory.PREMIUM);
		this.m_basicRouletteCategorys.Add(RouletteCategory.ITEM);
		this.m_rouletteCostItemIdList.Add(ServerItem.Id.ROULLETE_TICKET_PREMIAM);
		this.m_rouletteCostItemIdList.Add(ServerItem.Id.ROULLETE_TICKET_ITEM);
		this.m_rouletteCostItemIdList.Add(ServerItem.Id.SPECIAL_EGG);
		if (typeInTime == EventManager.EventType.RAID_BOSS)
		{
			this.m_basicRouletteCategorys.Add(RouletteCategory.RAID);
			this.m_rouletteCostItemIdList.Add(ServerItem.Id.RAIDRING);
		}
		if (this.m_rouletteCostItemIdListGetTime <= 0f || Time.realtimeSinceStartup - this.m_rouletteCostItemIdListGetTime > 1000f)
		{
			List<int> list = new List<int>();
			foreach (ServerItem.Id current in this.m_rouletteCostItemIdList)
			{
				list.Add((int)current);
			}
			if (GeneralUtil.IsNetwork())
			{
				ServerInterface.LoggedInServerInterface.RequestServerGetItemStockNum(EventManager.Instance.Id, list, base.gameObject);
			}
			else
			{
				GeneralUtil.ShowNoCommunication("ShowNoCommunicationCostItem");
			}
		}
		GeneralUtil.SetRouletteBtnIcon(null, "Btn_roulette");
		if (this.m_basicInfoCallback != null)
		{
			this.m_basicInfoCallback.SendMessage("RequestBasicInfo_Succeeded", this.m_basicRouletteCategorys, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void ServerGetItemStockNum_Succeeded(MsgGetItemStockNumSucceed msg)
	{
		if (RouletteTop.Instance != null)
		{
			RouletteTop.Instance.UpdateCostItemList(this.m_rouletteCostItemIdList);
			this.m_rouletteCostItemIdListGetTime = Time.realtimeSinceStartup;
		}
	}

	public static Dictionary<RouletteCategory, float> GetCurrentLoading()
	{
		Dictionary<RouletteCategory, float> result = null;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.GetCurrentLoadingOrg();
		}
		return result;
	}

	public static bool IsLoading(RouletteCategory category)
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.IsLoadingOrg(category);
		}
		return result;
	}

	public static bool IsPrizeLoading(RouletteCategory category)
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.IsPrizeLoadingOrg(category);
		}
		return result;
	}

	public static bool RequestRoulettePrize(RouletteCategory category, GameObject callbackObject = null)
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.RequestRoulettePrizeOrg(category, callbackObject);
		}
		return result;
	}

	public static bool RequestRoulette(RouletteCategory category, GameObject callbackObject = null)
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.RequestRouletteOrg(category, callbackObject);
		}
		return result;
	}

	public static bool RequestCommitRoulette(ServerWheelOptionsData data, int num, GameObject callbackObject = null)
	{
		bool result = false;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.RequestCommitRouletteOrg(data, num, callbackObject);
		}
		return result;
	}

	public static void ResetRoulette(RouletteCategory category = RouletteCategory.ALL)
	{
		if (RouletteManager.s_instance != null)
		{
			RouletteManager.s_instance.ResetRouletteOrg(category);
		}
	}

	public static ServerWheelOptionsData UpdateRoulette(RouletteCategory category, float delay = 0f)
	{
		if (RouletteManager.s_instance != null)
		{
			return RouletteManager.s_instance.UpdateRouletteOrg(category, delay);
		}
		return null;
	}

	public static void Remove()
	{
		if (RouletteManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(RouletteManager.s_instance.gameObject);
			RouletteManager.s_instance = null;
		}
	}

	private void ResetResult()
	{
		this.m_resultData = null;
		this.m_resultGeneral = null;
	}

	public bool IsResult()
	{
		bool result = false;
		if (this.m_resultData != null || this.m_resultGeneral != null)
		{
			result = true;
		}
		return result;
	}

	public bool IsPicupChara(CharaType charaType)
	{
		bool flag = false;
		if (this.m_picupCharaList != null && this.m_picupCharaList.Count > 0)
		{
			Dictionary<RouletteCategory, List<CharaType>>.KeyCollection keys = this.m_picupCharaList.Keys;
			foreach (RouletteCategory current in keys)
			{
				if (this.m_picupCharaList[current].Count > 0)
				{
					foreach (CharaType current2 in this.m_picupCharaList[current])
					{
						if (current2 == charaType)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	public RouletteCategory GetPicupCharaCategry(CharaType charaType)
	{
		RouletteCategory rouletteCategory = RouletteCategory.NONE;
		if (this.m_picupCharaList != null && this.m_picupCharaList.Count > 0)
		{
			Dictionary<RouletteCategory, List<CharaType>>.KeyCollection keys = this.m_picupCharaList.Keys;
			foreach (RouletteCategory current in keys)
			{
				if (this.m_picupCharaList[current].Count > 0)
				{
					foreach (CharaType current2 in this.m_picupCharaList[current])
					{
						if (current2 == charaType)
						{
							rouletteCategory = current;
							break;
						}
					}
				}
				if (rouletteCategory != RouletteCategory.NONE)
				{
					break;
				}
			}
		}
		return rouletteCategory;
	}

	public ServerSpinResultGeneral GetResult()
	{
		return this.m_resultGeneral;
	}

	public ServerChaoSpinResult GetResultChao()
	{
		return this.m_resultData;
	}

	private void ServerGetWheelOptionsGeneral_Succeeded(MsgGetWheelOptionsGeneralSucceed msg)
	{
		if (msg != null)
		{
			ServerWheelOptionsGeneral wheelOptionsGeneral = msg.m_wheelOptionsGeneral;
			RouletteCategory rouletteCategory = RouletteUtility.GetRouletteCategory(wheelOptionsGeneral);
			global::Debug.Log("ServerGetWheelOptionsGeneral_Succeeded Category:" + rouletteCategory);
			if (wheelOptionsGeneral != null)
			{
				if (wheelOptionsGeneral.jackpotRing >= 30000)
				{
					RouletteManager.s_numJackpotRing = wheelOptionsGeneral.jackpotRing;
				}
				if (this.m_rouletteList == null)
				{
					this.m_rouletteList = new Dictionary<RouletteCategory, ServerWheelOptionsData>();
					this.m_rouletteList.Add(rouletteCategory, new ServerWheelOptionsData(wheelOptionsGeneral));
				}
				else if (this.m_rouletteList.ContainsKey(rouletteCategory))
				{
					this.m_rouletteList[rouletteCategory].Setup(wheelOptionsGeneral);
				}
				else
				{
					this.m_rouletteList.Add(rouletteCategory, new ServerWheelOptionsData(wheelOptionsGeneral));
				}
				this.EndLoading(RouletteCategory.GENERAL);
				global::Debug.Log("rouletteId:" + wheelOptionsGeneral.rouletteId);
				this.RequestPrizeAuto(this.m_rouletteList[rouletteCategory]);
				if (this.GetCallbackObject(wheelOptionsGeneral.rouletteId) != null && this.m_rouletteList.ContainsKey(rouletteCategory))
				{
					if (RouletteManager.IsRouletteEnabled())
					{
						this.GetCallbackObject(wheelOptionsGeneral.rouletteId).SendMessage("RequestGetRoulette_Succeeded", this.m_rouletteList[rouletteCategory], SendMessageOptions.DontRequireReceiver);
						this.UpdateChangeBotton(rouletteCategory);
					}
					else
					{
						global::Debug.Log("RouletteManager ServerGetWheelOptionsGeneral_Succeeded RouletteTop:false");
					}
				}
			}
		}
	}

	private void ServerGetChaoWheelOptions_Succeeded(MsgGetChaoWheelOptionsSucceed msg)
	{
		if (msg != null)
		{
			ServerChaoWheelOptions options = msg.m_options;
			RouletteCategory rouletteCategory = RouletteCategory.PREMIUM;
			if (options.SpinType == ServerChaoWheelOptions.ChaoSpinType.Special)
			{
				rouletteCategory = RouletteCategory.SPECIAL;
			}
			if (this.m_rouletteList == null)
			{
				this.m_rouletteList = new Dictionary<RouletteCategory, ServerWheelOptionsData>();
				this.m_rouletteList.Add(rouletteCategory, new ServerWheelOptionsData(options));
			}
			else if (this.m_rouletteList.ContainsKey(rouletteCategory))
			{
				this.m_rouletteList[rouletteCategory].Setup(options);
			}
			else
			{
				this.m_rouletteList.Add(rouletteCategory, new ServerWheelOptionsData(options));
			}
			this.RequestPrizeAuto(this.m_rouletteList[rouletteCategory]);
			this.EndLoading(RouletteCategory.PREMIUM);
			if (!this.m_initReq)
			{
				if (this.GetCallbackObject(1001) != null && this.m_rouletteList.ContainsKey(rouletteCategory))
				{
					if (RouletteManager.IsRouletteEnabled())
					{
						this.GetCallbackObject(1001).SendMessage("RequestGetRoulette_Succeeded", this.m_rouletteList[rouletteCategory], SendMessageOptions.DontRequireReceiver);
						this.UpdateChangeBotton(rouletteCategory);
					}
					else
					{
						global::Debug.Log("RouletteManager ServerGetChaoWheelOptions_Succeeded RouletteTop:false");
					}
				}
			}
			else if (this.m_initReqCallback != null)
			{
				this.m_initReqCallback(this.specialEgg);
				this.m_initReqCallback = null;
				this.m_initReq = false;
			}
		}
	}

	private void ServerGetWheelOptions_Succeeded(MsgGetWheelOptionsSucceed msg)
	{
		if (msg != null)
		{
			ServerWheelOptions wheelOptions = msg.m_wheelOptions;
			if (wheelOptions != null)
			{
				if (wheelOptions.m_numJackpotRing >= 30000)
				{
					RouletteManager.s_numJackpotRing = wheelOptions.m_numJackpotRing;
				}
				if (this.m_rouletteList == null)
				{
					this.m_rouletteList = new Dictionary<RouletteCategory, ServerWheelOptionsData>();
					this.m_rouletteList.Add(RouletteCategory.ITEM, new ServerWheelOptionsData(wheelOptions));
				}
				else if (this.m_rouletteList.ContainsKey(RouletteCategory.ITEM))
				{
					this.m_rouletteList[RouletteCategory.ITEM].Setup(wheelOptions);
				}
				else
				{
					this.m_rouletteList.Add(RouletteCategory.ITEM, new ServerWheelOptionsData(wheelOptions));
				}
				this.EndLoading(RouletteCategory.ITEM);
				if (this.m_isCurrentPrizeLoading == RouletteCategory.ITEM)
				{
					ServerPrizeState value = this.CreatePrizeList(RouletteCategory.ITEM);
					if (this.m_prizeCallback != null)
					{
						this.m_prizeCallback.SendMessage("RequestRoulettePrize_Succeeded", value, SendMessageOptions.DontRequireReceiver);
					}
					this.m_prizeCallback = null;
					this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
				}
				else
				{
					this.RequestPrizeAuto(this.m_rouletteList[RouletteCategory.ITEM]);
					if (this.GetCallbackObject(1002) != null && this.m_rouletteList.ContainsKey(RouletteCategory.ITEM))
					{
						if (RouletteManager.IsRouletteEnabled() || RouletteUtility.rouletteDefault == RouletteCategory.ITEM)
						{
							this.GetCallbackObject(1002).SendMessage("RequestGetRoulette_Succeeded", this.m_rouletteList[RouletteCategory.ITEM], SendMessageOptions.DontRequireReceiver);
							this.UpdateChangeBotton(RouletteCategory.ITEM);
						}
						else
						{
							global::Debug.Log("RouletteManager ServerGetWheelOptions_Succeeded RouletteTop:false");
						}
					}
				}
			}
		}
	}

	private void ServerCommitWheelSpinGeneral_Succeeded(MsgCommitWheelSpinGeneralSucceed msg)
	{
		global::Debug.Log("RouletteManager ServerCommitWheelSpinGeneral_Succeeded !!!");
		if (msg != null)
		{
			ServerWheelOptionsData.SPIN_BUTTON sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.FREE;
			RouletteCategory rouletteCategory = RouletteCategory.NONE;
			ServerSpinResultGeneral resultSpinResultGeneral = msg.m_resultSpinResultGeneral;
			this.m_currentRankup = false;
			ServerWheelOptionsData serverWheelOptionsData = new ServerWheelOptionsData(msg.m_wheelOptionsGeneral);
			if (serverWheelOptionsData != null)
			{
				rouletteCategory = serverWheelOptionsData.category;
			}
			if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(rouletteCategory))
			{
				sPIN_BUTTON = this.m_rouletteList[rouletteCategory].GetSpinButtonSeting();
			}
			if (rouletteCategory != RouletteCategory.NONE)
			{
				ServerWheelOptionsData serverWheelOptionsData2 = null;
				if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(rouletteCategory))
				{
					serverWheelOptionsData2 = this.m_rouletteList[rouletteCategory];
				}
				int itemWon = resultSpinResultGeneral.ItemWon;
				if (serverWheelOptionsData2 != null)
				{
					if (resultSpinResultGeneral.ItemWon >= 0 && serverWheelOptionsData2.GetRouletteRank() != RouletteUtility.WheelRank.Super)
					{
						int num = 0;
						if (serverWheelOptionsData2.GetCellItem(resultSpinResultGeneral.ItemWon, out num).idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
						{
							this.m_currentRankup = true;
						}
						else
						{
							this.m_currentRankup = false;
						}
					}
					else
					{
						this.m_currentRankup = false;
					}
				}
				else
				{
					if (this.m_rouletteList != null)
					{
						this.m_rouletteList.Add(rouletteCategory, serverWheelOptionsData);
					}
					global::Debug.Log("RouletteManager ServerCommitWheelSpinGeneral_Succeeded error?");
				}
				this.m_rouletteItemBak = null;
				this.m_rouletteChaoBak = null;
				this.m_rouletteGeneralBakCategory = RouletteCategory.NONE;
				this.m_rouletteGeneralBak = msg.m_wheelOptionsGeneral;
				if (this.m_rouletteGeneralBak != null)
				{
					this.m_rouletteGeneralBakCategory = rouletteCategory;
				}
				this.EndLoading(RouletteCategory.GENERAL);
				if (rouletteCategory == RouletteCategory.PREMIUM)
				{
					FoxManager.SendLtvPointPremiumRoulette(sPIN_BUTTON == ServerWheelOptionsData.SPIN_BUTTON.FREE);
				}
				if (this.GetCallbackObject(serverWheelOptionsData.rouletteId) != null)
				{
					if (RouletteManager.IsRouletteEnabled())
					{
						this.m_resultGeneral = resultSpinResultGeneral;
						this.m_resultData = null;
						this.m_resultGeneral.ItemWon = itemWon;
						this.m_lastCommitCategory = rouletteCategory;
						this.GetCallbackObject(serverWheelOptionsData.rouletteId).SendMessage("RequestCommitRoulette_Succeeded", serverWheelOptionsData, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						global::Debug.Log("RouletteManager ServerCommitWheelSpinGeneral_Succeeded RouletteTop:false");
					}
				}
			}
		}
	}

	private void ServerCommitWheelSpinGeneral_Failed(MsgServerConnctFailed msg)
	{
		if (this.GetCallbackObject(9) != null)
		{
			if (RouletteManager.IsRouletteEnabled())
			{
				this.GetCallbackObject(9).SendMessage("RequestCommitRoulette_Failed", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.Log("RouletteManager ServerCommitWheelSpinGeneral_Failed RouletteTop:false");
			}
		}
	}

	private void ServerCommitChaoWheelSpin_Succeeded(MsgCommitChaoWheelSpicSucceed msg)
	{
		if (msg != null)
		{
			ServerSpinResultGeneral resultSpinResultGeneral = msg.m_resultSpinResultGeneral;
			this.m_currentRankup = false;
			bool flag = false;
			ServerChaoWheelOptions.ChaoSpinType spinType = msg.m_chaoWheelOptions.SpinType;
			ServerWheelOptionsData.SPIN_BUTTON sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.FREE;
			if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.PREMIUM))
			{
				sPIN_BUTTON = this.m_rouletteList[RouletteCategory.PREMIUM].GetSpinButtonSeting();
			}
			RouletteCategory rouletteCategory;
			if (spinType == ServerChaoWheelOptions.ChaoSpinType.Special)
			{
				if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.SPECIAL))
				{
					ServerWheelOptionsData serverWheelOptionsData = this.m_rouletteList[RouletteCategory.SPECIAL];
				}
				rouletteCategory = RouletteCategory.SPECIAL;
			}
			else
			{
				if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.PREMIUM))
				{
					ServerWheelOptionsData serverWheelOptionsData = this.m_rouletteList[RouletteCategory.PREMIUM];
					if (serverWheelOptionsData.category == RouletteCategory.PREMIUM)
					{
						flag = true;
					}
				}
				rouletteCategory = RouletteCategory.PREMIUM;
			}
			if (this.m_rouletteList == null || !this.m_rouletteList.ContainsKey(rouletteCategory))
			{
				ServerWheelOptionsData serverWheelOptionsData = new ServerWheelOptionsData(msg.m_chaoWheelOptions);
				if (this.m_rouletteList != null)
				{
					this.m_rouletteList.Add(rouletteCategory, serverWheelOptionsData);
				}
			}
			this.m_currentRankup = false;
			this.m_rouletteItemBak = null;
			this.m_rouletteChaoBak = msg.m_chaoWheelOptions;
			this.m_rouletteGeneralBakCategory = RouletteCategory.NONE;
			this.m_rouletteChaoBakCategory = RouletteCategory.NONE;
			this.m_rouletteGeneralBak = null;
			if (this.m_rouletteChaoBak != null)
			{
				this.m_rouletteChaoBakCategory = rouletteCategory;
			}
			this.EndLoading(RouletteCategory.PREMIUM);
			if (flag)
			{
				FoxManager.SendLtvPointPremiumRoulette(sPIN_BUTTON == ServerWheelOptionsData.SPIN_BUTTON.FREE);
			}
			if (this.GetCallbackObject(1001) != null)
			{
				if (RouletteManager.IsRouletteEnabled())
				{
					if (resultSpinResultGeneral != null)
					{
						this.m_resultGeneral = resultSpinResultGeneral;
						this.m_resultData = null;
					}
					else
					{
						this.m_resultGeneral = null;
					}
					this.m_lastCommitCategory = rouletteCategory;
					this.GetCallbackObject(1001).SendMessage("RequestCommitRoulette_Succeeded", this.m_rouletteList[rouletteCategory], SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					global::Debug.Log("RouletteManager ServerCommitChaoWheelSpin_Succeeded RouletteTop:false");
				}
			}
		}
	}

	private void ServerCommitChaoWheelSpin_Failed(MsgServerConnctFailed msg)
	{
		if (this.GetCallbackObject(1001) != null)
		{
			if (RouletteManager.IsRouletteEnabled())
			{
				this.GetCallbackObject(1001).SendMessage("RequestCommitRoulette_Failed", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.Log("RouletteManager ServerCommitChaoWheelSpin_Failed RouletteTop:false");
			}
		}
	}

	private void ServerCommitWheelSpin_Succeeded(MsgCommitWheelSpinSucceed msg)
	{
		if (msg != null)
		{
			ServerWheelOptionsData serverWheelOptionsData = null;
			if (this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.ITEM))
			{
				serverWheelOptionsData = this.m_rouletteList[RouletteCategory.ITEM];
			}
			if (serverWheelOptionsData != null)
			{
				if (serverWheelOptionsData.itemWonData.idType == ServerItem.IdType.ITEM_ROULLETE_WIN && serverWheelOptionsData.GetRouletteRank() != RouletteUtility.WheelRank.Super)
				{
					this.m_currentRankup = true;
				}
				else
				{
					this.m_currentRankup = false;
				}
			}
			else
			{
				if (this.m_rouletteList != null)
				{
					serverWheelOptionsData = new ServerWheelOptionsData(msg.m_wheelOptions);
					this.m_rouletteList.Add(RouletteCategory.ITEM, serverWheelOptionsData);
				}
				global::Debug.Log("RouletteManager ServerCommitWheelSpin_Succeeded error?");
			}
			this.m_rouletteItemBak = msg.m_wheelOptions;
			this.EndLoading(RouletteCategory.ITEM);
			if (this.GetCallbackObject(1002) != null)
			{
				if (RouletteManager.IsRouletteEnabled() && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(RouletteCategory.ITEM) && this.m_rouletteItemBak != null)
				{
					serverWheelOptionsData = new ServerWheelOptionsData(this.m_rouletteItemBak);
					if (msg.m_resultSpinResultGeneral != null)
					{
						this.m_resultData = null;
						this.m_resultGeneral = msg.m_resultSpinResultGeneral;
					}
					else
					{
						this.m_resultData = null;
						this.m_resultGeneral = new ServerSpinResultGeneral(this.m_rouletteItemBak, this.m_rouletteList[RouletteCategory.ITEM].GetOrgRankupData());
					}
					this.m_lastCommitCategory = RouletteCategory.ITEM;
					this.GetCallbackObject(1002).SendMessage("RequestCommitRoulette_Succeeded", serverWheelOptionsData, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					global::Debug.Log("RouletteManager ServerCommitWheelSpin_Succeeded RouletteTop:false");
				}
			}
		}
	}

	private void ServerCommitWheelSpin_Failed(MsgServerConnctFailed msg)
	{
		if (this.GetCallbackObject(1002) != null)
		{
			if (RouletteManager.IsRouletteEnabled())
			{
				this.GetCallbackObject(1002).SendMessage("RequestCommitRoulette_Failed", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.Log("RouletteManager ServerCommitWheelSpin_Failed RouletteTop:false");
			}
		}
	}

	private void ServerGetPrizeChaoWheelSpin_Succeeded(MsgGetPrizeChaoWheelSpinSucceed msg)
	{
		RouletteCategory rouletteCategory = (msg.m_type != 0) ? RouletteCategory.SPECIAL : RouletteCategory.PREMIUM;
		if (this.m_prizeList == null)
		{
			this.m_prizeList = new Dictionary<RouletteCategory, ServerPrizeState>();
		}
		this.SetPrizeList(rouletteCategory, msg.m_prizeState);
		if (rouletteCategory != RouletteCategory.NONE && this.m_prizeCallback != null)
		{
			this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
			if (RouletteManager.IsRouletteEnabled())
			{
				this.m_prizeCallback.SendMessage("RequestRoulettePrize_Succeeded", this.m_prizeList[rouletteCategory], SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.Log("RouletteManager ServerGetPrizeChaoWheelSpin_Succeeded RouletteTop:false");
			}
		}
		this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
		this.m_prizeCallback = null;
	}

	private void ServerGetPrizeWheelSpinGeneral_Succeeded(MsgGetPrizeWheelSpinGeneralSucceed msg)
	{
		RouletteCategory rouletteCategory = RouletteCategory.RAID;
		if (this.m_prizeList == null)
		{
			this.m_prizeList = new Dictionary<RouletteCategory, ServerPrizeState>();
		}
		this.SetPrizeList(rouletteCategory, msg.prizeState);
		if (rouletteCategory != RouletteCategory.NONE && this.m_prizeCallback != null)
		{
			this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
			if (RouletteManager.IsRouletteEnabled())
			{
				this.m_prizeCallback.SendMessage("RequestRoulettePrize_Succeeded", this.m_prizeList[rouletteCategory], SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.Log("RouletteManager ServerGetPrizeWheelSpinGeneral_Succeeded RouletteTop:false");
			}
		}
		this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
		this.m_prizeCallback = null;
	}

	private void ServerGetPrizeChaoWheelSpin_Failed()
	{
		RouletteCategory isCurrentPrizeLoading = this.m_isCurrentPrizeLoading;
		if (isCurrentPrizeLoading != RouletteCategory.NONE && this.m_prizeCallback != null)
		{
			this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
			if (RouletteManager.IsRouletteEnabled())
			{
				this.m_prizeCallback.SendMessage("RequestRoulettePrize_Failed", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.Log("RouletteManager ServerGetPrizeChaoWheelSpin_Failed RouletteTop:false");
			}
		}
		this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
		this.m_prizeCallback = null;
	}

	private void ServerGetPrizeWheelSpinGeneral_Failed()
	{
		RouletteCategory isCurrentPrizeLoading = this.m_isCurrentPrizeLoading;
		if (isCurrentPrizeLoading != RouletteCategory.NONE && this.m_prizeCallback != null)
		{
			this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
			if (RouletteManager.IsRouletteEnabled())
			{
				this.m_prizeCallback.SendMessage("RequestRoulettePrize_Failed", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.Log("RouletteManager ServerGetPrizeWheelSpinGeneral_Failed RouletteTop:false");
			}
		}
		this.m_isCurrentPrizeLoading = RouletteCategory.NONE;
		this.m_prizeCallback = null;
	}

	private void ServerAddSpecialEgg_Succeeded(MsgAddSpecialEggSucceed msg)
	{
	}

	private void ServerAddSpecialEgg_Failed(MsgServerConnctFailed msg)
	{
	}

	private void ServerRetrievePlayerState_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.specialEgg = msg.m_playerState.GetNumItemById(220000);
	}

	public ServerWheelOptionsData GetRouletteDataOrg(RouletteCategory category)
	{
		ServerWheelOptionsData serverWheelOptionsData = null;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && this.m_rouletteList != null && this.m_rouletteList.ContainsKey(category))
		{
			ServerWheelOptionsData serverWheelOptionsData2 = this.m_rouletteList[category];
			if (serverWheelOptionsData2.isValid)
			{
				serverWheelOptionsData = serverWheelOptionsData2;
				if (category == RouletteCategory.PREMIUM && this.specialEgg >= 10 && !RouletteUtility.isTutorial && serverWheelOptionsData != null && serverWheelOptionsData.category == RouletteCategory.PREMIUM)
				{
					serverWheelOptionsData = null;
				}
			}
		}
		return serverWheelOptionsData;
	}

	public Dictionary<RouletteCategory, ServerWheelOptionsData> GetRouletteDataAllOrg()
	{
		Dictionary<RouletteCategory, ServerWheelOptionsData> dictionary = null;
		if (this.m_rouletteList != null && this.m_rouletteList.Count > 0)
		{
			Dictionary<RouletteCategory, ServerWheelOptionsData>.KeyCollection keys = this.m_rouletteList.Keys;
			foreach (RouletteCategory current in keys)
			{
				if (this.m_rouletteList[current].isValid)
				{
					if (dictionary == null)
					{
						dictionary = new Dictionary<RouletteCategory, ServerWheelOptionsData>();
					}
					dictionary.Add(current, this.m_rouletteList[current]);
				}
			}
		}
		return dictionary;
	}

	public static ServerWheelOptionsData GetRouletteData(RouletteCategory category)
	{
		ServerWheelOptionsData result = null;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.GetRouletteDataOrg(category);
		}
		return result;
	}

	public static Dictionary<RouletteCategory, ServerWheelOptionsData> GetRouletteDataAll()
	{
		Dictionary<RouletteCategory, ServerWheelOptionsData> result = null;
		if (RouletteManager.s_instance != null)
		{
			result = RouletteManager.s_instance.GetRouletteDataAllOrg();
		}
		return result;
	}

	public GameObject GetCallbackObject(int key)
	{
		GameObject result = null;
		if (this.m_callbackList != null)
		{
			if (this.m_callbackList.ContainsKey(key))
			{
				result = this.m_callbackList[key];
			}
			else if (key == 8 && this.m_callbackList.ContainsKey(1))
			{
				result = this.m_callbackList[1];
			}
		}
		return result;
	}

	public void SetCallbackObject(int key, GameObject obj)
	{
		if (this.m_callbackList == null)
		{
			this.m_callbackList = new Dictionary<int, GameObject>();
			this.m_callbackList.Add(key, obj);
		}
		else if (this.m_callbackList.ContainsKey(key))
		{
			this.m_callbackList[key] = obj;
		}
		else
		{
			this.m_callbackList.Add(key, obj);
		}
	}

	private void RequestPrizeAuto(ServerWheelOptionsData data)
	{
		this.m_isCurrentPrizeLoadingAuto = RouletteCategory.NONE;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (data != null && loggedInServerInterface != null)
		{
			RouletteCategory category = data.category;
			if (this.m_prizeList != null && this.m_prizeList.ContainsKey(category) && this.m_prizeList[category] != null && this.m_prizeList[category].IsData())
			{
				return;
			}
			if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && category != RouletteCategory.ITEM)
			{
				if (data.isGeneral)
				{
					int eventId = 0;
					int spinType = 0;
					if (EventManager.Instance != null)
					{
						eventId = EventManager.Instance.Id;
					}
					loggedInServerInterface.RequestServerGetPrizeWheelSpinGeneral(eventId, spinType, null);
				}
				else
				{
					int chaoWheelSpinType = (category != RouletteCategory.SPECIAL) ? 0 : 1;
					loggedInServerInterface.RequestServerGetPrizeChaoWheelSpin(chaoWheelSpinType, null);
				}
				this.m_isCurrentPrizeLoadingAuto = category;
				global::Debug.Log("RequestPrizeAuto category:" + data.category + " isReq:true");
			}
			else
			{
				global::Debug.Log("RequestPrizeAuto category:" + data.category + " isReq:false");
			}
		}
	}

	private void Awake()
	{
		this.SetInstance();
	}

	private void OnDestroy()
	{
		if (RouletteManager.s_instance == this)
		{
			RouletteManager.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (RouletteManager.s_instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			RouletteManager.s_instance = this;
			RouletteManager.s_instance.Init();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private static string GetText(string cellName, Dictionary<string, string> dicReplaces = null)
	{
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", cellName).text;
		if (dicReplaces != null)
		{
			text = TextUtility.Replaces(text, dicReplaces);
		}
		return text;
	}

	private static string GetText(string cellName, string srcText, string dstText)
	{
		return RouletteManager.GetText(cellName, new Dictionary<string, string>
		{
			{
				srcText,
				dstText
			}
		});
	}
}
