using AnimationOrTween;
using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RouletteTop : CustomGameObject
{
	public enum ROULETTE_EFFECT_TYPE
	{
		BG_PARTICLE,
		SPIN,
		BOARD,
		NUM
	}

	[Header("ルーレット種別ごとのカラー設定"), SerializeField]
	private Color m_premiumColor;

	[SerializeField]
	private Color m_specialColor;

	[SerializeField]
	private Color m_defaultColor;

	[SerializeField]
	private RouletteBoard m_orgRouletteBoard;

	[SerializeField]
	private RouletteStandardPart m_orgStdPartsBoard;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private List<UIPanel> m_panels;

	[SerializeField]
	private GameObject m_topPageObject;

	[SerializeField]
	private GameObject m_rouletteBase;

	[SerializeField]
	private GameObject m_stdPartsBase;

	[SerializeField]
	private GameObject m_buttonsBase;

	[SerializeField]
	private GameObject m_buttonsBaseBg;

	[SerializeField]
	private window_odds m_odds;

	[SerializeField]
	public Texture m_itemRouletteDefaultTexture;

	private bool m_updateRequest;

	private bool m_close;

	private bool m_change;

	private bool m_opened;

	private ServerWheelOptionsData m_wheelData;

	private ServerWheelOptionsData m_wheelDataAfter;

	private List<RoulettePartsBase> m_parts;

	private List<UIButtonMessage> m_buttons;

	private bool m_tutorial;

	private bool m_tutorialSpin;

	private bool m_addSpecialEgg;

	private bool m_word;

	private bool m_spin;

	private long m_spinCount;

	private bool m_spinSkip;

	private bool m_spinDecision;

	private int m_spinDecisionIndex = -1;

	private float m_spinTime;

	private float m_multiGetDelayTime;

	private float m_closeTime;

	private float m_removeTime;

	private bool m_wheelSetup;

	private List<RouletteCategory> m_rouletteList;

	private List<RouletteCategory> m_rouletteCostItemLoadedList;

	private RouletteCategory m_requestCostItemCategory;

	private ServerChaoSpinResult m_spinResult;

	private ServerSpinResultGeneral m_spinResultGeneral;

	private RouletteUtility.NextType m_nextType;

	private RouletteCategory m_requestCategory;

	private RouletteCategory m_setupNoCommunicationCategory;

	private UIImageButton m_backButtonImg;

	private List<RouletteTop.ROULETTE_EFFECT_TYPE> m_notEffectList;

	private bool m_clickBack;

	private UIRectItemStorage m_topPageStorage;

	private List<GameObject> m_topPageRouletteList;

	private List<GameObject> m_topPageHeaderList;

	private RouletteCategory m_topPageOddsSelect;

	private bool m_topPageWheelData;

	private Dictionary<RouletteCategory, InformationWindow.Information> m_rouletteInfoList;

	private UILabel m_premiumRouletteLabel;

	private UILabel m_premiumRouletteShLabel;

	private SendApollo m_sendApollo;

	private float m_inputLimitTime;

	private bool m_isWindow;

	private bool m_isTopPage;

	private static RouletteTop s_instance;

	public bool addSpecialEgg
	{
		get
		{
			return this.m_addSpecialEgg;
		}
	}

	public bool isWindow
	{
		get
		{
			return this.m_isWindow;
		}
	}

	public bool isEnabled
	{
		get
		{
			bool result = false;
			if (base.gameObject.activeSelf && this.m_parts != null && this.m_parts.Count > 0)
			{
				result = true;
			}
			return result;
		}
	}

	public ServerWheelOptionsData wheelData
	{
		get
		{
			return this.m_wheelData;
		}
	}

	public RouletteCategory category
	{
		get
		{
			if (this.m_wheelData == null)
			{
				return RouletteCategory.NONE;
			}
			return this.m_wheelData.category;
		}
	}

	public float spinTime
	{
		get
		{
			if (!this.m_spin)
			{
				return 0f;
			}
			return this.m_spinTime;
		}
	}

	public bool isSpin
	{
		get
		{
			return this.m_spin;
		}
	}

	public bool isSpinSkip
	{
		get
		{
			return this.m_spinSkip;
		}
	}

	public bool isSpinDecision
	{
		get
		{
			return this.m_spinDecision;
		}
	}

	public int spinDecisionIndex
	{
		get
		{
			return this.m_spinDecisionIndex;
		}
	}

	public bool isSpinGetWindow
	{
		get
		{
			bool result = false;
			if (this.m_wheelDataAfter != null)
			{
				result = true;
			}
			return result;
		}
	}

	public bool isWordAnime
	{
		get
		{
			return this.m_word;
		}
	}

	public static RouletteTop Instance
	{
		get
		{
			return RouletteTop.s_instance;
		}
	}

	public bool IsClose()
	{
		return !this.m_opened;
	}

	public Color GetBtnColor(RouletteCategory category)
	{
		Color result = this.m_defaultColor;
		if (category != RouletteCategory.PREMIUM)
		{
			if (category == RouletteCategory.SPECIAL)
			{
				result = this.m_specialColor;
			}
		}
		else
		{
			result = this.m_premiumColor;
		}
		return result;
	}

	public bool IsEffect(RouletteTop.ROULETTE_EFFECT_TYPE type)
	{
		bool result = false;
		if (this.m_notEffectList != null && this.m_notEffectList.Count > 0)
		{
			if (!this.m_notEffectList.Contains(type))
			{
				result = true;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void OnRouletteOpenItem()
	{
		if (this.m_removeTime == 0f || Mathf.Abs(Time.realtimeSinceStartup - this.m_removeTime) > 0.5f)
		{
			if (this.m_rouletteCostItemLoadedList != null)
			{
				this.m_rouletteCostItemLoadedList.Clear();
			}
			else
			{
				this.m_rouletteCostItemLoadedList = new List<RouletteCategory>();
			}
			this.m_isTopPage = false;
			this.m_tutorialSpin = false;
			this.m_opened = true;
			global::Debug.Log("RouletteTop:OnRouletteOpenItem!");
			RouletteManager.RouletteBgmReset();
			RouletteManager.RouletteOpen(RouletteCategory.ITEM);
		}
	}

	public void OnRouletteOpenPremium()
	{
		if (this.m_removeTime == 0f || Mathf.Abs(Time.realtimeSinceStartup - this.m_removeTime) > 0.5f)
		{
			if (this.m_rouletteCostItemLoadedList != null)
			{
				this.m_rouletteCostItemLoadedList.Clear();
			}
			else
			{
				this.m_rouletteCostItemLoadedList = new List<RouletteCategory>();
			}
			this.m_isTopPage = false;
			this.m_tutorialSpin = false;
			this.m_opened = true;
			global::Debug.Log("RouletteTop:OnRouletteOpenPremium!");
			RouletteManager.RouletteBgmReset();
			RouletteManager.RouletteOpen(RouletteCategory.PREMIUM);
		}
	}

	public void OnRouletteOpenRaid()
	{
		if (this.m_removeTime == 0f || Mathf.Abs(Time.realtimeSinceStartup - this.m_removeTime) > 0.5f)
		{
			if (this.m_rouletteCostItemLoadedList != null)
			{
				this.m_rouletteCostItemLoadedList.Clear();
			}
			else
			{
				this.m_rouletteCostItemLoadedList = new List<RouletteCategory>();
			}
			this.m_isTopPage = false;
			this.m_tutorialSpin = false;
			this.m_opened = true;
			global::Debug.Log("RouletteTop:OnRouletteOpenRaid!");
			RouletteManager.RouletteBgmReset();
			RouletteManager.RouletteOpen(RouletteCategory.RAID);
		}
	}

	public void OnRouletteOpenDefault()
	{
		if (this.m_removeTime == 0f || Mathf.Abs(Time.realtimeSinceStartup - this.m_removeTime) > 0.5f)
		{
			if (this.m_rouletteCostItemLoadedList != null)
			{
				this.m_rouletteCostItemLoadedList.Clear();
			}
			else
			{
				this.m_rouletteCostItemLoadedList = new List<RouletteCategory>();
			}
			this.m_isTopPage = false;
			this.m_tutorialSpin = false;
			this.m_opened = true;
			global::Debug.Log("RouletteTop:OnRouletteOpenDefault!  rouletteDefault:" + RouletteUtility.rouletteDefault);
			RouletteManager.RouletteBgmReset();
			RouletteManager.RouletteOpen(RouletteCategory.NONE);
		}
	}

	public void OnRouletteEnd()
	{
		RouletteManager.RouletteClose();
	}

	public void UpdateCostItemList(List<ServerItem.Id> costItemList)
	{
		this.SetTopPageHeaderObject();
	}

	protected override void UpdateStd(float deltaTime, float timeRate)
	{
		if (this.m_setupNoCommunicationCategory != RouletteCategory.NONE)
		{
			if (GeneralWindow.IsCreated("SetupNoCommunication") && GeneralWindow.IsButtonPressed)
			{
				if (!GeneralUtil.IsNetwork())
				{
					GeneralUtil.ShowNoCommunication("SetupNoCommunication");
				}
				else
				{
					this.Setup(this.m_setupNoCommunicationCategory);
				}
			}
			return;
		}
		if (this.m_tutorial)
		{
			if (!GeneralWindow.Created)
			{
				if (GeneralWindow.IsCreated("RouletteTutorial") && GeneralWindow.IsOkButtonPressed && !this.m_tutorialSpin)
				{
					TutorialCursor.StartTutorialCursor(TutorialCursor.Type.ROULETTE_SPIN);
					this.m_tutorialSpin = true;
				}
				else if (GeneralWindow.IsCreated("RouletteTutorialEnd") && GeneralWindow.IsButtonPressed)
				{
					RouletteUtility.rouletteTurtorialEnd = true;
					this.m_tutorial = false;
					ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
					if (itemGetWindow != null)
					{
						itemGetWindow.Create(new ItemGetWindow.CInfo
						{
							name = "TutorialEndAddSp",
							caption = TextUtility.GetCommonText("MainMenu", "tutorial_sp_egg1_caption"),
							serverItemId = 220000,
							imageCount = TextUtility.GetCommonText("MainMenu", "tutorial_sp_egg1_text", "{COUNT}", 10.ToString())
						});
						SoundManager.SePlay("sys_specialegg", "SE");
					}
					global::Debug.Log("TurtorialEnd:" + RouletteUtility.rouletteTurtorialEnd + " !!!!!!!!!!!!!!!!!!!! ");
				}
				else if (GeneralWindow.IsCreated("RouletteTutorialError") && GeneralWindow.IsButtonPressed)
				{
					if (GeneralUtil.IsNetwork())
					{
						GeneralWindow.Create(new GeneralWindow.CInfo
						{
							name = "RouletteTutorialEnd",
							buttonType = GeneralWindow.ButtonType.Ok,
							caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "end_of_tutorial_caption").text,
							message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "end_of_tutorial_text").text
						});
						string[] value = new string[1];
						SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP6, ref value);
						this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value);
					}
					else
					{
						GeneralUtil.ShowNoCommunication("RouletteTutorialError");
					}
				}
			}
		}
		else if (RouletteUtility.rouletteTurtorialEnd)
		{
			ItemGetWindow itemGetWindow2 = ItemGetWindowUtil.GetItemGetWindow();
			if (itemGetWindow2 != null && itemGetWindow2.IsCreated("TutorialEndAddSp") && itemGetWindow2.IsEnd)
			{
				itemGetWindow2.Reset();
				if (RouletteManager.Instance != null && RouletteManager.Instance.specialEgg >= 10)
				{
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = "SpEggMax",
						buttonType = GeneralWindow.ButtonType.Ok,
						caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "sp_egg_max_caption").text,
						message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "sp_egg_max_text").text
					});
				}
				RouletteManager.RequestRoulette(this.category, base.gameObject);
				RouletteUtility.rouletteTurtorialEnd = false;
				this.m_addSpecialEgg = false;
				global::Debug.Log("TurtorialEnd:" + RouletteUtility.rouletteTurtorialEnd + " !!!!!!!!!!!!!!!!!!!! ");
			}
		}
		if (this.m_spin)
		{
			this.m_spinTime += deltaTime;
			if (this.m_multiGetDelayTime > 0f && this.m_spinDecision)
			{
				this.m_multiGetDelayTime -= deltaTime;
				if (this.m_multiGetDelayTime <= 0f)
				{
					this.OnRouletteSpinEnd();
					this.m_multiGetDelayTime = 0f;
				}
			}
			this.m_spinCount += 1L;
		}
		if (this.m_closeTime > 0f)
		{
			this.m_closeTime -= deltaTime;
			if (this.m_closeTime <= 0f)
			{
				this.m_clickBack = true;
				this.Close(RouletteUtility.NextType.NONE);
				this.m_closeTime = 0f;
			}
		}
		if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
		{
			UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
			this.m_sendApollo = null;
		}
		if (this.m_inputLimitTime > 0f)
		{
			this.m_inputLimitTime -= Time.deltaTime;
			if (this.m_inputLimitTime <= 0f)
			{
				this.m_inputLimitTime = 0f;
				HudMenuUtility.SetConnectAlertSimpleUI(false);
			}
		}
		if (this.m_topPageWheelData && this.m_topPageOddsSelect != RouletteCategory.NONE && RouletteManager.Instance != null && !RouletteManager.Instance.isCurrentPrizeLoading)
		{
			ServerPrizeState prizeList = RouletteManager.Instance.GetPrizeList(this.m_topPageOddsSelect);
			ServerWheelOptionsData rouletteDataOrg = RouletteManager.Instance.GetRouletteDataOrg(this.m_topPageOddsSelect);
			if (prizeList != null && rouletteDataOrg != null)
			{
				this.OpenOdds(prizeList, rouletteDataOrg);
				this.m_topPageWheelData = false;
				this.m_topPageOddsSelect = RouletteCategory.NONE;
			}
		}
	}

	public void SetPanelsAlpha(float alpha)
	{
		if (this.m_panels != null && this.m_panels.Count > 0)
		{
			foreach (UIPanel current in this.m_panels)
			{
				current.alpha = alpha;
			}
		}
	}

	public float GetPanelsAlpha()
	{
		float num = -1f;
		if (this.m_parts != null && this.m_parts.Count > 0)
		{
			num = 0f;
			foreach (UIPanel current in this.m_panels)
			{
				float num2 = current.alpha;
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				num += num2;
			}
			num /= (float)this.m_panels.Count;
			if (num > 1f)
			{
				num = 1f;
			}
			else if (num < 0.01f)
			{
				num = 0f;
			}
		}
		return num;
	}

	public bool OpenOdds(ServerPrizeState prize, ServerWheelOptionsData wheelOptionsData = null)
	{
		bool result = false;
		if (this.m_odds != null)
		{
			if (wheelOptionsData != null)
			{
				this.m_odds.Open(prize, wheelOptionsData);
			}
			else
			{
				this.m_odds.Open(prize, this.wheelData);
			}
			result = true;
		}
		return result;
	}

	public bool OnRouletteSpinStart(ServerWheelOptionsData data, int num)
	{
		if (data != null && data.category == RouletteCategory.RAID && EventManager.Instance != null && EventManager.Instance.TypeInTime != EventManager.EventType.RAID_BOSS)
		{
			GeneralUtil.ShowEventEnd("ShowEventEnd");
			this.Setup(this.m_rouletteList[0]);
			return false;
		}
		bool result = false;
		this.m_nextType = RouletteUtility.NextType.NONE;
		this.m_closeTime = 0f;
		if (!this.isSpin)
		{
			this.m_spinCount = 0L;
			GC.Collect();
			if (this.m_tutorial)
			{
				TutorialCursor.EndTutorialCursor(TutorialCursor.Type.ROULETTE_SPIN);
			}
			if (this.m_backButtonImg != null)
			{
				this.m_backButtonImg.isEnabled = false;
			}
			this.m_spinDecisionIndex = -1;
			if (this.m_parts != null && this.m_parts.Count > 0)
			{
				foreach (RoulettePartsBase current in this.m_parts)
				{
					if (current != null)
					{
						current.OnSpinStart();
					}
				}
			}
			this.m_wheelSetup = false;
			this.m_spinTime = 0f;
			this.m_word = false;
			this.m_spin = true;
			this.m_spinSkip = false;
			this.m_spinDecision = false;
			result = RouletteManager.RequestCommitRoulette(data, num, base.gameObject);
		}
		return result;
	}

	public bool OnRouletteSpinSkip()
	{
		bool result = false;
		this.m_closeTime = 0f;
		if (this.isSpin && !this.isSpinSkip && this.m_spinDecisionIndex >= 0 && this.m_spinTime > 0.1f)
		{
			if (this.m_parts != null && this.m_parts.Count > 0)
			{
				foreach (RoulettePartsBase current in this.m_parts)
				{
					if (current != null)
					{
						current.OnSpinSkip();
					}
				}
			}
			this.m_spinSkip = true;
			this.m_spinDecision = true;
		}
		return result;
	}

	public bool OnRouletteSpinDecision(int decIndex)
	{
		bool result = false;
		this.m_closeTime = 0f;
		if (this.isSpin && !this.isSpinDecision)
		{
			if (decIndex >= 0)
			{
				if (this.m_backButtonImg != null)
				{
					this.m_backButtonImg.isEnabled = true;
				}
				this.m_spinDecisionIndex = decIndex;
				if (this.m_parts != null && this.m_parts.Count > 0)
				{
					foreach (RoulettePartsBase current in this.m_parts)
					{
						if (current != null)
						{
							current.OnSpinDecision();
						}
					}
				}
				this.m_spinSkip = false;
			}
			else
			{
				if (this.m_backButtonImg != null)
				{
					this.m_backButtonImg.isEnabled = false;
				}
				this.m_multiGetDelayTime = 5f;
				if (this.m_parts != null && this.m_parts.Count > 0)
				{
					foreach (RoulettePartsBase current2 in this.m_parts)
					{
						if (current2 != null)
						{
							current2.OnSpinDecisionMulti();
						}
					}
				}
				this.m_spinSkip = true;
			}
			this.m_spinDecision = true;
			result = true;
		}
		return result;
	}

	public bool OnRouletteSpinEnd()
	{
		HudMenuUtility.SetConnectAlertSimpleUI(true);
		this.m_inputLimitTime = 5f;
		bool result = false;
		this.m_closeTime = 0f;
		this.m_multiGetDelayTime = 0f;
		if (this.isSpin && this.isSpinDecision)
		{
			GC.Collect();
			this.SetDelayTime(0.25f);
			if (this.m_backButtonImg != null)
			{
				this.m_backButtonImg.isEnabled = true;
			}
			this.m_word = true;
			if (this.m_parts != null && this.m_parts.Count > 0)
			{
				foreach (RoulettePartsBase current in this.m_parts)
				{
					if (current != null)
					{
						current.OnSpinEnd();
					}
				}
			}
			this.m_spin = false;
			this.m_spinSkip = false;
			this.m_spinDecision = false;
			result = true;
		}
		return result;
	}

	public bool OnRouletteWordAnimeEnd()
	{
		this.m_inputLimitTime = 0.25f;
		bool result = false;
		float delay = 0f;
		this.m_closeTime = 0f;
		this.m_multiGetDelayTime = 0f;
		if (this.m_word)
		{
			this.SetDelayTime(0.25f);
			this.m_word = false;
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			if (this.m_spinResultGeneral != null)
			{
				if (this.m_spinResultGeneral.ItemWon >= 0 && this.m_wheelData.GetRouletteRank() != RouletteUtility.WheelRank.Super)
				{
					int num = 0;
					if (this.m_wheelData.GetCellItem(this.m_spinResultGeneral.ItemWon, out num).idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
					{
						this.CloseGetWindow(RouletteUtility.AchievementType.NONE, RouletteUtility.NextType.NONE);
					}
					else
					{
						RouletteUtility.ShowGetWindow(this.m_spinResultGeneral);
						delay = 0.5f;
					}
				}
				else
				{
					RouletteUtility.ShowGetWindow(this.m_spinResultGeneral);
					delay = 0.5f;
				}
				this.m_spinResultGeneral = null;
			}
			else if (this.m_spinResult != null)
			{
				RouletteUtility.ShowGetWindow(this.m_spinResult);
				this.m_spinResult = null;
				delay = 0.5f;
			}
			else
			{
				global::Debug.Log("OnRouletteWordAnimeEnd error?");
				if (this.m_wheelData.itemWonData.idType == ServerItem.IdType.ITEM_ROULLETE_WIN && this.m_wheelData.GetRouletteRank() != RouletteUtility.WheelRank.Super)
				{
					this.CloseGetWindow(RouletteUtility.AchievementType.NONE, RouletteUtility.NextType.NONE);
				}
				else
				{
					RouletteUtility.ShowGetWindow(this.m_wheelData.GetOrgRankupData());
					delay = 0.5f;
				}
			}
			result = true;
		}
		ServerWheelOptionsData serverWheelOptionsData = RouletteManager.UpdateRoulette(this.m_wheelData.category, delay);
		if (serverWheelOptionsData != null)
		{
			this.UpdateWheelData(serverWheelOptionsData, false);
		}
		return result;
	}

	public void OnRouletteGetError(MsgServerConnctFailed msg)
	{
		this.m_spin = false;
		this.m_spinSkip = false;
		this.m_spinDecision = false;
		this.SetDelayTime(0.5f);
		this.m_closeTime = 0.1f;
		this.m_multiGetDelayTime = 0f;
	}

	public bool OnRouletteSpinError(MsgServerConnctFailed msg)
	{
		global::Debug.Log("RouletteTop  OnRouletteSpinError !!!!!!!");
		bool result = false;
		this.SetDelayTime(0.5f);
		this.m_closeTime = 0.1f;
		this.m_multiGetDelayTime = 0f;
		if (this.isSpin)
		{
			if (this.m_backButtonImg != null)
			{
				this.m_backButtonImg.isEnabled = true;
			}
			if (this.m_parts != null && this.m_parts.Count > 0)
			{
				foreach (RoulettePartsBase current in this.m_parts)
				{
					if (current != null)
					{
						current.OnSpinError();
					}
				}
			}
			this.m_spin = false;
			this.m_spinSkip = false;
			this.m_spinDecision = false;
			result = true;
		}
		return result;
	}

	public void UpdateEffectSetting()
	{
		if (this.m_parts != null && this.m_parts.Count > 0)
		{
			foreach (RoulettePartsBase current in this.m_parts)
			{
				if (current != null)
				{
					current.UpdateEffectSetting();
				}
			}
		}
	}

	public void OpenRouletteWindow()
	{
		if (base.gameObject.activeSelf && this.m_parts != null && this.m_parts.Count > 0)
		{
			this.m_isWindow = true;
			foreach (RoulettePartsBase current in this.m_parts)
			{
				if (current != null)
				{
					current.windowOpen();
				}
			}
		}
	}

	public void CloseRouletteWindow()
	{
		if (base.gameObject.activeSelf)
		{
			if (this.m_parts != null && this.m_parts.Count > 0)
			{
				this.m_isWindow = false;
				foreach (RoulettePartsBase current in this.m_parts)
				{
					if (current != null)
					{
						current.windowClose();
					}
				}
			}
			if (this.m_tutorial)
			{
				if (GeneralUtil.IsNetwork())
				{
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = "RouletteTutorialEnd",
						buttonType = GeneralWindow.ButtonType.Ok,
						caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "end_of_tutorial_caption").text,
						message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "end_of_tutorial_text").text
					});
					string[] value = new string[1];
					SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP6, ref value);
					this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value);
				}
				else
				{
					GeneralUtil.ShowNoCommunication("RouletteTutorialError");
				}
				TutorialCursor.EndTutorialCursor(TutorialCursor.Type.ROULETTE_OK);
			}
		}
	}

	public void CloseGetWindow(RouletteUtility.AchievementType achievement, RouletteUtility.NextType nextType = RouletteUtility.NextType.NONE)
	{
		this.CloseRouletteWindow();
		if (nextType == RouletteUtility.NextType.NONE)
		{
			if (this.m_wheelDataAfter != null && !this.m_change)
			{
				this.UpdateWheelData(this.m_wheelDataAfter, true);
				if (!this.m_change)
				{
					this.m_wheelDataAfter = null;
				}
			}
		}
		else
		{
			this.Close(nextType);
		}
	}

	public void SetDelayTime(float delay = 0.2f)
	{
		if (this.m_parts != null && this.m_parts.Count > 0)
		{
			foreach (RoulettePartsBase current in this.m_parts)
			{
				if (current != null)
				{
					current.SetDelayTime(delay);
				}
			}
		}
	}

	public void BtnInit()
	{
		if (this.m_buttonsBase != null)
		{
			if (this.m_buttons == null)
			{
				bool activeSelf = this.m_buttonsBase.activeSelf;
				this.m_buttonsBase.SetActive(true);
				for (int i = 0; i < 10; i++)
				{
					UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_buttonsBase, "Btn_" + i);
					if (!(uIButtonMessage != null))
					{
						break;
					}
					if (this.m_buttons == null)
					{
						this.m_buttons = new List<UIButtonMessage>();
					}
					uIButtonMessage.gameObject.SetActive(false);
					this.m_buttons.Add(uIButtonMessage);
				}
				this.m_buttonsBase.SetActive(activeSelf);
			}
			else if (this.m_buttons.Count > 0)
			{
				foreach (UIButtonMessage current in this.m_buttons)
				{
					current.gameObject.SetActive(false);
				}
			}
		}
	}

	public bool SetupTopPage(bool init = true)
	{
		if (this.m_close)
		{
			return false;
		}
		if (RouletteManager.Instance == null)
		{
			return false;
		}
		this.ResetParts();
		if (this.m_topPageObject != null)
		{
			this.m_topPageObject.SetActive(true);
		}
		if (HudMenuUtility.IsNumPlayingRouletteTutorial() && RouletteUtility.isTutorial)
		{
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.ROULETTE_TOP_PAGE);
		}
		if (this.m_topPageStorage == null && this.m_topPageObject != null)
		{
			this.m_topPageStorage = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_topPageObject, "list");
		}
		if (this.m_topPageHeaderList == null && this.m_topPageObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_topPageObject, "window_header");
			if (gameObject != null)
			{
				string text = "img_{PARAM}_bg";
				for (int i = 0; i < 10; i++)
				{
					string name = text.Replace("{PARAM}", i.ToString());
					GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, name);
					if (gameObject2 != null)
					{
						if (this.m_topPageHeaderList == null)
						{
							this.m_topPageHeaderList = new List<GameObject>();
						}
						gameObject2.SetActive(false);
						this.m_topPageHeaderList.Add(gameObject2);
					}
				}
			}
		}
		else
		{
			this.SetTopPageHeaderObject();
		}
		if (this.m_topPageRouletteList != null)
		{
			this.m_topPageRouletteList.Clear();
		}
		if (this.m_topPageStorage != null)
		{
			this.m_topPageStorage.maxItemCount = (this.m_topPageStorage.maxRows = 0);
			this.m_topPageStorage.Restart();
		}
		this.m_isTopPage = true;
		RouletteUtility.ChangeRouletteHeader(RouletteCategory.ALL);
		if (this.m_backButtonImg == null)
		{
			this.m_backButtonImg = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_cmn_back");
		}
		if (this.m_buttonsBase != null)
		{
			if (this.m_buttons == null)
			{
				for (int j = 0; j < 10; j++)
				{
					UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_buttonsBase, "Btn_" + j);
					if (!(uIButtonMessage != null))
					{
						break;
					}
					if (this.m_buttons == null)
					{
						this.m_buttons = new List<UIButtonMessage>();
					}
					uIButtonMessage.gameObject.SetActive(false);
					this.m_buttons.Add(uIButtonMessage);
				}
			}
			else if (this.m_buttons.Count > 0)
			{
				foreach (UIButtonMessage current in this.m_buttons)
				{
					current.gameObject.SetActive(false);
				}
			}
		}
		this.m_isWindow = false;
		this.m_requestCategory = RouletteCategory.NONE;
		RouletteUtility.rouletteDefault = RouletteCategory.NONE;
		if (RouletteUtility.rouletteDefault != RouletteCategory.ITEM && RouletteUtility.rouletteDefault != RouletteCategory.PREMIUM)
		{
			RouletteUtility.rouletteDefault = RouletteCategory.PREMIUM;
		}
		base.gameObject.SetActive(true);
		if (init)
		{
			if (this.m_buttonsBase != null)
			{
				this.m_buttonsBase.SetActive(false);
			}
			if (this.m_buttonsBaseBg != null)
			{
				this.m_buttonsBaseBg.SetActive(false);
			}
			RouletteManager.Instance.RequestRouletteBasicInformation(base.gameObject);
		}
		else
		{
			if (this.m_buttonsBase != null)
			{
				this.m_buttonsBase.SetActive(false);
			}
			if (this.m_buttonsBaseBg != null)
			{
				this.m_buttonsBaseBg.SetActive(false);
			}
			this.SetTopPageHeaderObject();
			this.UpdateChangeBotton(RouletteCategory.ALL);
		}
		EventManager.EventType type = EventManager.Instance.Type;
		string text2 = null;
		string cueSheetName = "BGM";
		if (type != EventManager.EventType.NUM && type != EventManager.EventType.UNKNOWN && type != EventManager.EventType.ADVERT && EventManager.Instance.IsInEvent() && EventCommonDataTable.Instance != null)
		{
			string data = EventCommonDataTable.Instance.GetData(EventCommonDataItem.Roulette_BgmName);
			if (!string.IsNullOrEmpty(data))
			{
				cueSheetName = "BGM_" + EventManager.GetEventTypeName(EventManager.Instance.Type);
				text2 = data;
			}
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "bgm_sys_roulette";
		}
		if (!string.IsNullOrEmpty(text2))
		{
			SoundManager.BgmChange(text2, cueSheetName);
		}
		return true;
	}

	public bool Setup(RouletteCategory category)
	{
		if (this.m_close)
		{
			return false;
		}
		if (RouletteManager.Instance == null)
		{
			return false;
		}
		if (this.m_topPageObject != null)
		{
			this.m_topPageObject.SetActive(false);
		}
		if (HudMenuUtility.IsNumPlayingRouletteTutorial() && RouletteUtility.isTutorial)
		{
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.ROULETTE_TOP_PAGE);
		}
		this.m_setupNoCommunicationCategory = RouletteCategory.NONE;
		if (!GeneralUtil.IsNetwork())
		{
			this.m_setupNoCommunicationCategory = category;
			GeneralUtil.ShowNoCommunication("SetupNoCommunication");
			return false;
		}
		bool isTopPage = this.m_isTopPage;
		this.m_isTopPage = false;
		if (this.m_backButtonImg == null)
		{
			this.m_backButtonImg = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_cmn_back");
		}
		if (this.m_buttonsBase != null)
		{
			if (this.m_buttons == null)
			{
				for (int i = 0; i < 10; i++)
				{
					UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_buttonsBase, "Btn_" + i);
					if (!(uIButtonMessage != null))
					{
						break;
					}
					if (this.m_buttons == null)
					{
						this.m_buttons = new List<UIButtonMessage>();
					}
					uIButtonMessage.gameObject.SetActive(false);
					this.m_buttons.Add(uIButtonMessage);
				}
			}
			else if (this.m_buttons.Count > 0)
			{
				foreach (UIButtonMessage current in this.m_buttons)
				{
					current.gameObject.SetActive(false);
				}
			}
		}
		if (category == RouletteCategory.SPECIAL)
		{
			category = RouletteCategory.PREMIUM;
		}
		this.m_isWindow = false;
		this.m_requestCategory = category;
		RouletteUtility.rouletteDefault = category;
		if (RouletteUtility.rouletteDefault != RouletteCategory.ITEM && RouletteUtility.rouletteDefault != RouletteCategory.PREMIUM)
		{
			RouletteUtility.rouletteDefault = RouletteCategory.PREMIUM;
		}
		if (this.isEnabled && !isTopPage)
		{
			if (this.m_buttonsBase != null)
			{
				this.m_buttonsBase.SetActive(false);
			}
			if (this.m_buttonsBaseBg != null)
			{
				this.m_buttonsBaseBg.SetActive(false);
			}
			this.SetDelayTime(1f);
			ServerWheelOptionsData rouletteData = RouletteManager.GetRouletteData(category);
			if (rouletteData != null)
			{
				this.UpdateWheelData(rouletteData, true);
				if (this.m_buttons.Count > 0)
				{
					int num = 0;
					foreach (RouletteCategory current2 in this.m_rouletteList)
					{
						if (this.m_buttons.Count <= num)
						{
							break;
						}
						this.UpdateChangeBottonIcon(current2, this.m_buttons[num], num, current2 != category);
						num++;
					}
				}
			}
			else
			{
				this.m_updateRequest = true;
				RouletteManager.RequestRoulette(category, base.gameObject);
			}
		}
		else
		{
			if (this.m_buttonsBase != null)
			{
				this.m_buttonsBase.SetActive(false);
			}
			if (this.m_buttonsBaseBg != null)
			{
				this.m_buttonsBaseBg.SetActive(false);
			}
			if (category != RouletteCategory.ITEM)
			{
				RouletteManager.RouletteBgmReset();
			}
			RouletteManager.ResetRoulette(RouletteCategory.ALL);
			if (!isTopPage)
			{
				this.SetPanelsAlpha(0f);
			}
			base.gameObject.SetActive(true);
			this.m_updateRequest = false;
			RouletteManager.RequestRoulette(category, base.gameObject);
		}
		if (!isTopPage)
		{
			RouletteManager.Instance.RequestRouletteBasicInformation(base.gameObject);
		}
		else
		{
			this.SetTopPageHeaderObject();
			this.UpdateChangeBotton(this.m_requestCategory);
		}
		return true;
	}

	private void SetupWheelData(ServerWheelOptionsData wheelData)
	{
		this.m_setupNoCommunicationCategory = RouletteCategory.NONE;
		if (wheelData != null)
		{
			this.m_isTopPage = false;
			this.m_wheelSetup = true;
			this.m_wheelDataAfter = null;
			this.m_closeTime = 0f;
			this.m_nextType = RouletteUtility.NextType.NONE;
			this.m_spinTime = 0f;
			this.m_multiGetDelayTime = 0f;
			this.m_word = false;
			this.m_spin = false;
			this.m_spinSkip = false;
			this.m_spinDecision = false;
			this.m_spinDecisionIndex = -1;
			this.m_wheelData = wheelData;
			this.m_close = false;
			this.m_clickBack = false;
			this.SetPanelsAlpha(1f);
			base.gameObject.SetActive(true);
			if (this.m_animation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_mm_roulette_intro_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.AnimationFinishCallback), true);
				if (this.m_wheelData.category == RouletteCategory.SPECIAL)
				{
					SoundManager.BgmStop();
					this.m_wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Change, 0f);
					this.m_wheelData.PlayBgm(2.2f);
				}
				else
				{
					this.m_wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Open, 0f);
					this.m_wheelData.PlayBgm(0.3f);
				}
			}
			this.ResetParts();
			if (this.m_rouletteBase != null && this.m_orgRouletteBoard != null)
			{
				this.CreateParts(this.m_orgRouletteBoard.gameObject, this.m_rouletteBase);
			}
			if (this.m_stdPartsBase != null && this.m_orgStdPartsBoard != null)
			{
				this.CreateParts(this.m_orgStdPartsBoard.gameObject, this.m_stdPartsBase);
			}
			RouletteUtility.ChangeRouletteHeader(this.m_wheelData.category);
			if (this.m_buttonsBase != null)
			{
				this.m_buttonsBase.SetActive(false);
			}
			if (this.m_buttonsBaseBg != null)
			{
				this.m_buttonsBaseBg.SetActive(false);
			}
			this.UpdateChangeBotton(this.m_wheelData.category);
		}
	}

	public void UpdateWheel(ServerWheelOptionsData wheelData, bool changeEffect)
	{
		this.m_isTopPage = false;
		if (base.gameObject.activeSelf)
		{
			this.UpdateWheelData(wheelData, changeEffect);
		}
	}

	private void UpdateWheelData(ServerWheelOptionsData wheelData, bool changeEffect = true)
	{
		this.m_setupNoCommunicationCategory = RouletteCategory.NONE;
		if (wheelData != null)
		{
			this.m_isTopPage = false;
			if (wheelData.isGeneral && (this.m_rouletteCostItemLoadedList == null || !this.m_rouletteCostItemLoadedList.Contains(wheelData.category)) && ServerInterface.LoggedInServerInterface != null)
			{
				List<int> spinCostItemIdList = wheelData.GetSpinCostItemIdList();
				if (spinCostItemIdList != null && spinCostItemIdList.Count > 0)
				{
					List<int> list = new List<int>();
					foreach (int current in spinCostItemIdList)
					{
						if (current != 910000 && current != 900000)
						{
							list.Add(current);
						}
					}
					if (list.Count > 0)
					{
						this.m_requestCostItemCategory = wheelData.category;
						ServerInterface.LoggedInServerInterface.RequestServerGetItemStockNum(EventManager.Instance.Id, list, base.gameObject);
					}
				}
			}
			this.m_closeTime = 0f;
			this.m_nextType = RouletteUtility.NextType.NONE;
			this.m_spinTime = 0f;
			this.m_multiGetDelayTime = 0f;
			this.m_spin = false;
			this.m_spinSkip = false;
			this.m_spinDecision = false;
			this.m_spinDecisionIndex = -1;
			if (this.m_wheelData.category == RouletteCategory.PREMIUM && wheelData.category == RouletteCategory.SPECIAL)
			{
				SoundManager.BgmStop();
				wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Change, 0f);
				wheelData.PlayBgm(2.2f);
				changeEffect = false;
			}
			else if (this.m_wheelData.category != RouletteCategory.SPECIAL && wheelData.category == RouletteCategory.SPECIAL)
			{
				SoundManager.BgmStop();
				wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Change, 0f);
				wheelData.PlayBgm(2.2f);
				changeEffect = false;
			}
			else if (this.m_wheelData.category == RouletteCategory.SPECIAL && wheelData.category == RouletteCategory.PREMIUM)
			{
				wheelData.PlayBgm(0.3f);
				changeEffect = false;
			}
			else
			{
				bool flag = false;
				if (this.m_wheelData.category == RouletteCategory.SPECIAL && wheelData.category == RouletteCategory.SPECIAL && RouletteManager.Instance != null && string.IsNullOrEmpty(RouletteManager.Instance.oldBgmName))
				{
					flag = true;
				}
				if (flag)
				{
					wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Change, 0f);
					wheelData.PlayBgm(2.2f);
					changeEffect = false;
				}
				else
				{
					wheelData.PlayBgm(0.3f);
					if (changeEffect)
					{
						wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Open, 0f);
					}
					changeEffect = false;
				}
			}
			if (changeEffect && this.m_wheelData.category != wheelData.category)
			{
				this.m_wheelDataAfter = wheelData;
				this.m_close = false;
				this.m_clickBack = false;
				this.m_change = true;
				if (this.m_animation != null)
				{
					ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_mm_roulette_intro_Anim", Direction.Reverse);
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.AnimationFinishCallback), true);
				}
			}
			else
			{
				int multi = this.m_wheelData.multi;
				this.m_wheelData = new ServerWheelOptionsData(wheelData);
				this.m_wheelData.ChangeMulti(multi);
				this.UpdateChangeBotton(this.m_wheelData.category);
				this.m_wheelDataAfter = null;
				this.m_close = false;
				this.m_clickBack = false;
				this.m_change = false;
				if (this.m_parts != null && this.m_parts.Count > 0)
				{
					foreach (RoulettePartsBase current2 in this.m_parts)
					{
						if (current2 != null)
						{
							current2.OnUpdateWheelData(this.m_wheelData);
						}
					}
				}
				RouletteUtility.ChangeRouletteHeader(this.m_wheelData.category);
				this.m_wheelSetup = true;
			}
		}
	}

	private void ServerAddSpecialEgg_Succeeded(MsgAddSpecialEggSucceed msg)
	{
	}

	private void ServerAddSpecialEgg_Failed(MsgServerConnctFailed msg)
	{
	}

	private void ServerGetItemStockNum_Succeeded(MsgGetItemStockNumSucceed msg)
	{
		if (this.m_rouletteCostItemLoadedList == null)
		{
			this.m_rouletteCostItemLoadedList = new List<RouletteCategory>();
		}
		if (!this.m_rouletteCostItemLoadedList.Contains(this.m_requestCostItemCategory))
		{
			this.m_rouletteCostItemLoadedList.Add(this.m_requestCostItemCategory);
		}
		if (this.m_parts != null && this.m_parts.Count > 0)
		{
			foreach (RoulettePartsBase current in this.m_parts)
			{
				if (current != null)
				{
					current.PartsSendMessage("CostItemUpdate");
				}
			}
		}
		this.m_requestCostItemCategory = RouletteCategory.NONE;
	}

	public bool Close(RouletteUtility.NextType nextType = RouletteUtility.NextType.NONE)
	{
		if (this.m_close)
		{
			return false;
		}
		RouletteUtility.loginRoulette = false;
		if (this.m_rouletteCostItemLoadedList != null)
		{
			this.m_rouletteCostItemLoadedList.Clear();
		}
		this.SetDelayTime(0.25f);
		this.m_nextType = nextType;
		this.m_spinTime = 0f;
		this.m_multiGetDelayTime = 0f;
		this.m_spin = false;
		this.m_spinSkip = false;
		this.m_spinDecision = false;
		this.m_spinDecisionIndex = -1;
		if (this.m_nextType != RouletteUtility.NextType.NONE)
		{
			RouletteUtility.NextType nextType2 = this.m_nextType;
			if (nextType2 != RouletteUtility.NextType.EQUIP)
			{
				if (nextType2 == RouletteUtility.NextType.CHARA_EQUIP)
				{
					HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHARA_MAIN, true);
				}
			}
			else
			{
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHAO, true);
			}
		}
		else
		{
			this.m_close = true;
			if (this.m_animation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_mm_roulette_intro_Anim", Direction.Reverse);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.AnimationFinishCallback), true);
				if (this.m_isTopPage)
				{
					SoundManager.SePlay("sys_window_close", "SE");
				}
				else if (this.m_wheelData != null)
				{
					this.m_wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Close, 0f);
				}
			}
		}
		return true;
	}

	public void Remove()
	{
		global::Debug.Log("RouletteTop Remove!");
		this.m_closeTime = 0f;
		this.m_spinTime = 0f;
		this.m_multiGetDelayTime = 0f;
		this.m_spin = false;
		this.m_spinSkip = false;
		this.m_spinDecision = false;
		this.m_spinDecisionIndex = -1;
		this.m_opened = false;
		this.m_close = false;
		this.SetPanelsAlpha(0f);
		HudMenuUtility.SendEnableShopButton(true);
		ChaoTextureManager.Instance.RemoveChaoTexture();
		this.ResetParts();
		RouletteManager.RouletteBgmReset();
		HudMenuUtility.ChangeMainMenuBGM();
		this.m_removeTime = Time.realtimeSinceStartup;
		if (this.m_clickBack)
		{
			HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.ROULETTE_BACK, false);
		}
		this.m_clickBack = false;
	}

	private void CreateParts(GameObject org, GameObject parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(org, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.gameObject.transform.parent = parent.transform;
		gameObject.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		RoulettePartsBase component = gameObject.gameObject.GetComponent<RoulettePartsBase>();
		if (component != null)
		{
			component.Setup(this);
			component.SetDelayTime(0f);
			if (this.m_parts == null)
			{
				this.m_parts = new List<RoulettePartsBase>();
			}
			this.m_parts.Add(component);
		}
	}

	private void ResetParts()
	{
		if (this.m_parts != null && this.m_parts.Count > 0)
		{
			for (int i = 0; i < this.m_parts.Count; i++)
			{
				if (this.m_parts[i] != null)
				{
					this.m_parts[i].DestroyParts();
				}
			}
			this.m_parts.Clear();
			this.m_parts = null;
		}
	}

	public static RouletteTop RouletteTopPageCreate()
	{
		if (RouletteTop.s_instance != null)
		{
			RouletteTop.s_instance.SetupTopPage(true);
			return RouletteTop.s_instance;
		}
		return null;
	}

	public static RouletteTop RouletteCreate(RouletteCategory category)
	{
		if (RouletteTop.s_instance != null && category != RouletteCategory.NONE && category != RouletteCategory.ALL)
		{
			RouletteTop.s_instance.Setup(category);
			return RouletteTop.s_instance;
		}
		return null;
	}

	public void UpdateChangeBotton(RouletteCategory current)
	{
		if (current == RouletteCategory.SPECIAL)
		{
			current = RouletteCategory.PREMIUM;
		}
		if (this.m_rouletteList != null && this.m_rouletteList.Count > 0)
		{
			if (this.m_rouletteList.Contains(RouletteCategory.RAID) && EventManager.Instance != null && EventManager.Instance.TypeInTime != EventManager.EventType.RAID_BOSS)
			{
				this.m_rouletteList.Remove(RouletteCategory.RAID);
			}
			this.SetTopPageObject();
			if (this.m_buttons != null && current != RouletteCategory.NONE && current != RouletteCategory.GENERAL)
			{
				if (current != RouletteCategory.ALL)
				{
					for (int i = 0; i < this.m_buttons.Count; i++)
					{
						this.m_buttons[i].gameObject.SetActive(false);
					}
				}
				else
				{
					for (int j = 0; j < this.m_buttons.Count; j++)
					{
						this.m_buttons[j].gameObject.SetActive(false);
					}
				}
			}
		}
	}

	private void ResetChangeBotton()
	{
		if (this.m_rouletteList != null && this.m_rouletteList.Count > 0 && this.m_buttons != null)
		{
			for (int i = 0; i < this.m_buttons.Count; i++)
			{
				this.m_buttons[i].gameObject.SetActive(false);
			}
		}
	}

	private void UpdateChangeBottonIcon(RouletteCategory category, UIButtonMessage button, int idx, bool enabled)
	{
		UIImageButton component = button.GetComponent<UIImageButton>();
		if (component != null)
		{
			component.isEnabled = enabled;
		}
		if (category != RouletteCategory.PREMIUM && category != RouletteCategory.SPECIAL)
		{
			button.functionName = "OnClickChangeBtn_" + idx;
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_btn_icon");
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_move_icon");
			if (uISprite != null)
			{
				uISprite.gameObject.SetActive(true);
				uISprite.spriteName = RouletteUtility.GetRouletteChangeIconSpriteName(category);
			}
			if (uISprite2 != null)
			{
				uISprite2.gameObject.SetActive(false);
			}
		}
		else
		{
			button.functionName = "OnClickChangeBtn_" + idx;
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_btn_icon");
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_move_icon");
			if (RouletteManager.Instance.specialEgg >= 10 && !this.m_tutorial && !RouletteUtility.rouletteTurtorialEnd)
			{
				if (uISprite != null)
				{
					uISprite.gameObject.SetActive(false);
				}
				if (uISprite2 != null)
				{
					uISprite2.spriteName = RouletteUtility.GetRouletteChangeIconSpriteName(RouletteCategory.SPECIAL);
					uISprite2.gameObject.SetActive(true);
				}
			}
			else
			{
				if (uISprite != null)
				{
					uISprite.gameObject.SetActive(true);
					uISprite.spriteName = RouletteUtility.GetRouletteChangeIconSpriteName(category);
				}
				if (uISprite2 != null)
				{
					uISprite2.gameObject.SetActive(false);
				}
			}
		}
	}

	private void UpdateChangeBottonIcon(RouletteCategory category, UIButtonMessage button, int idx)
	{
		if (category != RouletteCategory.PREMIUM && category != RouletteCategory.SPECIAL)
		{
			button.functionName = "OnClickChangeBtn_" + idx;
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_btn_icon");
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_move_icon");
			if (uISprite != null)
			{
				uISprite.gameObject.SetActive(true);
				uISprite.spriteName = RouletteUtility.GetRouletteChangeIconSpriteName(category);
			}
			if (uISprite2 != null)
			{
				uISprite2.gameObject.SetActive(false);
			}
		}
		else
		{
			button.functionName = "OnClickChangeBtn_" + idx;
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_btn_icon");
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(button.gameObject, "img_move_icon");
			if (RouletteManager.Instance.specialEgg >= 10 && !this.m_tutorial && !RouletteUtility.rouletteTurtorialEnd)
			{
				if (uISprite != null)
				{
					uISprite.gameObject.SetActive(false);
				}
				if (uISprite2 != null)
				{
					uISprite2.spriteName = RouletteUtility.GetRouletteChangeIconSpriteName(RouletteCategory.SPECIAL);
					uISprite2.gameObject.SetActive(true);
				}
			}
			else
			{
				if (uISprite != null)
				{
					uISprite.gameObject.SetActive(true);
					uISprite.spriteName = RouletteUtility.GetRouletteChangeIconSpriteName(category);
				}
				if (uISprite2 != null)
				{
					uISprite2.gameObject.SetActive(false);
				}
			}
		}
	}

	public void RequestBasicInfo_Succeeded(List<RouletteCategory> rouletteList)
	{
		if (rouletteList != null && rouletteList.Count > 0)
		{
			this.m_rouletteList = rouletteList;
			if (this.m_buttons != null && this.m_buttons.Count > 0)
			{
				for (int i = 0; i < this.m_buttons.Count; i++)
				{
					if (rouletteList.Count > i)
					{
						this.m_buttons[i].gameObject.SetActive(true);
						bool enabled = this.m_requestCategory != this.m_rouletteList[i];
						if (this.m_isTopPage)
						{
							enabled = true;
						}
						this.UpdateChangeBottonIcon(this.m_rouletteList[i], this.m_buttons[i], i, enabled);
					}
					else
					{
						this.m_buttons[i].gameObject.SetActive(false);
					}
				}
			}
			this.SetTopPageObject();
			if (this.m_buttonsBase != null)
			{
				this.m_buttonsBase.SetActive(false);
			}
			if (this.m_buttonsBaseBg != null)
			{
				this.m_buttonsBaseBg.SetActive(false);
			}
		}
	}

	public void RequestBasicInfo_Failed()
	{
	}

	private void SetTopPageObject()
	{
		if (this.m_topPageStorage != null)
		{
			this.m_topPageStorage.maxItemCount = (this.m_topPageStorage.maxRows = this.m_rouletteList.Count);
			this.m_topPageStorage.Restart();
			this.m_topPageRouletteList = GameObjectUtil.FindChildGameObjects(this.m_topPageStorage.gameObject, "ui_rouletteTop_scroll(Clone)");
			int specialEgg = RouletteManager.Instance.specialEgg;
			if (this.m_rouletteInfoList != null)
			{
				this.m_rouletteInfoList.Clear();
			}
			if (RouletteInformationManager.Instance != null)
			{
				RouletteInformationManager.Instance.GetCurrentInfoParam(out this.m_rouletteInfoList);
			}
			if (this.m_topPageRouletteList != null)
			{
				UIDraggablePanel uIDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(this.m_topPageObject, "ScrollView");
				if (uIDraggablePanel != null)
				{
					uIDraggablePanel.enabled = (!HudMenuUtility.IsNumPlayingRouletteTutorial() || !RouletteUtility.isTutorial);
				}
				for (int i = 0; i < this.m_topPageRouletteList.Count; i++)
				{
					UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_topPageRouletteList[i], "base");
					UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_topPageRouletteList[i], "Lbl_btn_roulette");
					UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_topPageRouletteList[i], "Lbl_btn_roulette_sh");
					UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_topPageRouletteList[i], "img_ad_tex");
					UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_topPageRouletteList[i], "Btn_information");
					RouletteCategory rouletteCategory = this.m_rouletteList[i];
					if (this.m_rouletteList[i] == RouletteCategory.PREMIUM && specialEgg >= 10 && !RouletteUtility.isTutorial)
					{
						rouletteCategory = RouletteCategory.SPECIAL;
					}
					ServerWheelOptionsData rouletteDataOrg = RouletteManager.Instance.GetRouletteDataOrg(rouletteCategory);
					if (rouletteDataOrg != null && this.m_isTopPage)
					{
						rouletteDataOrg.ChangeMulti(1);
					}
					if (uISprite != null)
					{
						uISprite.color = this.GetBtnColor(rouletteCategory);
					}
					if (uILabel != null && uILabel2 != null)
					{
						UILabel arg_208_0 = uILabel;
						string rouletteCategoryHeaderText = RouletteUtility.GetRouletteCategoryHeaderText(rouletteCategory);
						uILabel2.text = rouletteCategoryHeaderText;
						arg_208_0.text = rouletteCategoryHeaderText;
					}
					if (rouletteCategory == RouletteCategory.PREMIUM || rouletteCategory == RouletteCategory.SPECIAL)
					{
						this.m_premiumRouletteLabel = uILabel;
						this.m_premiumRouletteShLabel = uILabel2;
					}
					if (rouletteCategory != RouletteCategory.ITEM)
					{
						RouletteCategory rouletteCategory2 = RouletteCategory.NONE;
						RouletteCategory rouletteCategory3 = rouletteCategory;
						switch (rouletteCategory3)
						{
						case RouletteCategory.PREMIUM:
							goto IL_25E;
						case RouletteCategory.ITEM:
							IL_251:
							if (rouletteCategory3 != RouletteCategory.SPECIAL)
							{
								goto IL_290;
							}
							goto IL_25E;
						case RouletteCategory.RAID:
							if (this.m_rouletteInfoList.ContainsKey(RouletteCategory.RAID))
							{
								rouletteCategory2 = RouletteCategory.RAID;
							}
							goto IL_290;
						}
						goto IL_251;
						IL_290:
						if (rouletteCategory2 != RouletteCategory.NONE)
						{
							if (uITexture != null && RouletteInformationManager.Instance != null)
							{
								RouletteInformationManager.InfoBannerRequest bannerRequest = new RouletteInformationManager.InfoBannerRequest(uITexture);
								RouletteInformationManager.Instance.LoadInfoBaner(bannerRequest, rouletteCategory2);
							}
							GeneralUtil.SetButtonFunc(this.m_topPageRouletteList[i], "Btn_information", base.gameObject, "OnClickInfoBtn_" + rouletteCategory2);
							if (uIImageButton != null)
							{
								uIImageButton.isEnabled = true;
							}
						}
						else
						{
							GeneralUtil.SetButtonFunc(this.m_topPageRouletteList[i], "Btn_information", base.gameObject, "OnClickInfoBtn_" + RouletteCategory.ITEM);
							if (uIImageButton != null)
							{
								uIImageButton.isEnabled = false;
							}
						}
						goto IL_3BC;
						IL_25E:
						if (this.m_rouletteInfoList.ContainsKey(RouletteCategory.PREMIUM))
						{
							rouletteCategory2 = RouletteCategory.PREMIUM;
						}
						goto IL_290;
					}
					if (uITexture != null && this.m_itemRouletteDefaultTexture != null)
					{
						uITexture.mainTexture = this.m_itemRouletteDefaultTexture;
					}
					GeneralUtil.SetButtonFunc(this.m_topPageRouletteList[i], "Btn_information", base.gameObject, "OnClickDummy");
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = false;
					}
					IL_3BC:
					GeneralUtil.SetButtonFunc(this.m_topPageRouletteList[i], "Btn_all_item", base.gameObject, "OnClickOddsBtn_" + i);
					GeneralUtil.SetButtonFunc(this.m_topPageRouletteList[i], "Btn_roulette", base.gameObject, "OnClickChangeBtn_" + i);
				}
			}
		}
	}

	private void SetTopPageHeaderObject()
	{
		if (this.m_topPageHeaderList != null && this.m_topPageHeaderList.Count > 0 && RouletteManager.Instance != null)
		{
			List<ServerItem.Id> rouletteCostItemIdList = RouletteManager.Instance.rouletteCostItemIdList;
			Dictionary<ServerItem.Id, string> dictionary = new Dictionary<ServerItem.Id, string>();
			dictionary.Add(ServerItem.Id.ROULLETE_TICKET_ITEM, "ui_roulette_ticket_2");
			dictionary.Add(ServerItem.Id.ROULLETE_TICKET_PREMIAM, "ui_roulette_ticket_1");
			dictionary.Add(ServerItem.Id.SPECIAL_EGG, "ui_result_special_egg");
			dictionary.Add(ServerItem.Id.RAIDRING, "ui_event_ring_icon");
			int num = 88;
			int num2 = 108;
			float num3 = Mathf.Sqrt((float)(num * num2));
			for (int i = 0; i < this.m_topPageHeaderList.Count; i++)
			{
				GameObject gameObject = this.m_topPageHeaderList[i];
				if (gameObject != null && rouletteCostItemIdList.Count > i)
				{
					ServerItem.Id id = rouletteCostItemIdList[i];
					long itemCount = GeneralUtil.GetItemCount(id);
					if (id == ServerItem.Id.SPECIAL_EGG && this.m_premiumRouletteLabel != null && this.m_premiumRouletteShLabel != null)
					{
						if (itemCount >= 10L && !RouletteUtility.isTutorial)
						{
							UILabel arg_131_0 = this.m_premiumRouletteLabel;
							string rouletteCategoryHeaderText = RouletteUtility.GetRouletteCategoryHeaderText(RouletteCategory.SPECIAL);
							this.m_premiumRouletteShLabel.text = rouletteCategoryHeaderText;
							arg_131_0.text = rouletteCategoryHeaderText;
						}
						else
						{
							UILabel arg_158_0 = this.m_premiumRouletteLabel;
							string rouletteCategoryHeaderText = RouletteUtility.GetRouletteCategoryHeaderText(RouletteCategory.PREMIUM);
							this.m_premiumRouletteShLabel.text = rouletteCategoryHeaderText;
							arg_158_0.text = rouletteCategoryHeaderText;
						}
					}
					UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_icon");
					UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_num");
					if (uISprite != null)
					{
						if (dictionary.ContainsKey(id))
						{
							uISprite.spriteName = dictionary[id];
							UISpriteData atlasSprite = uISprite.GetAtlasSprite();
							if (atlasSprite != null)
							{
								int width = atlasSprite.width;
								int height = atlasSprite.height;
								float num4 = Mathf.Sqrt((float)(width * height));
								float num5 = num3 / num4;
								uISprite.width = (int)((float)width * num5);
								uISprite.height = (int)((float)height * num5);
							}
						}
						else
						{
							uISprite.spriteName = string.Empty;
						}
					}
					if (uILabel != null)
					{
						uILabel.text = HudUtility.GetFormatNumString<long>(itemCount);
					}
					gameObject.SetActive(true);
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	public void RequestGetRoulette_Succeeded(ServerWheelOptionsData wheelData)
	{
		this.m_tutorial = false;
		if (this.m_topPageOddsSelect == RouletteCategory.NONE)
		{
			if (wheelData != null)
			{
				if (RouletteUtility.isTutorial && wheelData.category == RouletteCategory.PREMIUM)
				{
					this.m_tutorial = true;
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = "RouletteTutorial",
						buttonType = GeneralWindow.ButtonType.Ok,
						caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "roulette_move_explan_caption").text,
						message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "roulette_move_explan").text
					});
					string[] value = new string[1];
					SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP6, ref value);
					this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_START, value);
				}
				if (this.m_updateRequest)
				{
					this.SetDelayTime(0.25f);
					this.UpdateWheelData(wheelData, true);
					this.m_updateRequest = false;
				}
				else
				{
					this.SetupWheelData(wheelData);
				}
			}
		}
		else if (wheelData.category == RouletteCategory.ITEM)
		{
			RouletteManager.Instance.RequestRoulettePrizeOrg(this.m_topPageOddsSelect, base.gameObject);
		}
		else
		{
			this.m_topPageOddsSelect = wheelData.category;
			this.m_topPageWheelData = true;
		}
	}

	public void RequestCommitRoulette_Succeeded(ServerWheelOptionsData wheelData)
	{
		if (wheelData != null)
		{
			if (this.m_tutorial)
			{
				this.m_addSpecialEgg = true;
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					loggedInServerInterface.RequestServerAddSpecialEgg(10, base.gameObject);
				}
			}
			RouletteManager instance = RouletteManager.Instance;
			this.m_wheelDataAfter = wheelData;
			if (instance != null)
			{
				this.m_spinResultGeneral = instance.GetResult();
				if (this.m_spinResultGeneral != null)
				{
					this.m_spinResult = null;
					this.OnRouletteSpinDecision(this.m_spinResultGeneral.ItemWon);
				}
				else
				{
					this.m_spinResult = instance.GetResultChao();
					this.OnRouletteSpinDecision(this.m_spinResult.ItemWon);
				}
			}
		}
	}

	private void OnClickBack()
	{
		if (this.m_isTopPage)
		{
			if (!this.m_spin && this.m_closeTime <= 0f)
			{
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.ROULETTE_BACK, false);
			}
		}
		else
		{
			SoundManager.SePlay("sys_window_close", "SE");
			this.SetupTopPage(false);
		}
	}

	public void OnClickOddsBtn_0()
	{
		this.OnClickOddsBtn(0);
	}

	public void OnClickOddsBtn_1()
	{
		this.OnClickOddsBtn(1);
	}

	public void OnClickOddsBtn_2()
	{
		this.OnClickOddsBtn(2);
	}

	public void OnClickOddsBtn_3()
	{
		this.OnClickOddsBtn(3);
	}

	public void OnClickOddsBtn_4()
	{
		this.OnClickOddsBtn(4);
	}

	public void OnClickOddsBtn_5()
	{
		this.OnClickOddsBtn(5);
	}

	public void OnClickOddsBtn_6()
	{
		this.OnClickOddsBtn(6);
	}

	public void OnClickOddsBtn_7()
	{
		this.OnClickOddsBtn(7);
	}

	public void OnClickOddsBtn_8()
	{
		this.OnClickOddsBtn(8);
	}

	public void OnClickOddsBtn_9()
	{
		this.OnClickOddsBtn(9);
	}

	private void OnClickOddsBtn(int index)
	{
		global::Debug.Log("OnClickOddsBtn  " + index);
		if (RouletteManager.Instance != null && this.m_rouletteList != null && this.m_rouletteList.Count > index)
		{
			RouletteCategory rouletteCategory = this.m_rouletteList[index];
			if (rouletteCategory == RouletteCategory.SPECIAL && RouletteUtility.isTutorial)
			{
				rouletteCategory = RouletteCategory.PREMIUM;
			}
			this.m_topPageOddsSelect = rouletteCategory;
			this.m_topPageWheelData = false;
			ServerWheelOptionsData rouletteDataOrg = RouletteManager.Instance.GetRouletteDataOrg(rouletteCategory);
			global::Debug.Log(string.Concat(new object[]
			{
				"OnClickOddsBtn data:",
				rouletteDataOrg != null,
				" categ:",
				rouletteCategory,
				" !!!!!!"
			}));
			if (rouletteDataOrg != null)
			{
				RouletteManager.Instance.RequestRoulettePrizeOrg(this.m_topPageOddsSelect, base.gameObject);
			}
			else if (GeneralUtil.IsNetwork())
			{
				RouletteManager.Instance.RequestRouletteOrg(this.m_topPageOddsSelect, base.gameObject);
			}
			else
			{
				this.m_topPageOddsSelect = RouletteCategory.NONE;
				this.m_topPageWheelData = false;
				GeneralUtil.ShowNoCommunication("ShowNoCommunication");
			}
		}
	}

	private void RequestRoulettePrize_Succeeded(ServerPrizeState prize)
	{
		ServerWheelOptionsData rouletteDataOrg = RouletteManager.Instance.GetRouletteDataOrg(this.m_topPageOddsSelect);
		this.OpenOdds(prize, rouletteDataOrg);
		this.m_topPageOddsSelect = RouletteCategory.NONE;
	}

	public void OnClickCurrentRouletteBanner()
	{
		RouletteCategory rouletteCategory = this.m_wheelData.category;
		if (rouletteCategory == RouletteCategory.SPECIAL)
		{
			rouletteCategory = RouletteCategory.PREMIUM;
		}
		this.OnClickInfoBtn(rouletteCategory);
	}

	public void OnClickInfoBtn_ITEM()
	{
		this.OnClickInfoBtn(RouletteCategory.ITEM);
		global::Debug.Log("OnClickInfoBtn_ITEM  !!!!!!!!!");
	}

	public void OnClickInfoBtn_PREMIUM()
	{
		this.OnClickInfoBtn(RouletteCategory.PREMIUM);
	}

	public void OnClickInfoBtn_SPECIAL()
	{
		this.OnClickInfoBtn(RouletteCategory.PREMIUM);
	}

	public void OnClickInfoBtn_RAID()
	{
		this.OnClickInfoBtn(RouletteCategory.RAID);
	}

	public void OnClickInfoBtn_EVENT()
	{
		this.OnClickInfoBtn(RouletteCategory.EVENT);
	}

	private void OnClickInfoBtn(RouletteCategory category)
	{
		if (this.m_rouletteInfoList != null && this.m_rouletteInfoList.ContainsKey(category))
		{
			RouletteInformationUtility.ShowNewsWindow(this.m_rouletteInfoList[category]);
		}
		global::Debug.Log("OnClickInfoBtn  " + category);
	}

	public void OnClickChangeBtn_0()
	{
		this.OnClickChangeBtn(0);
	}

	public void OnClickChangeBtn_1()
	{
		this.OnClickChangeBtn(1);
	}

	public void OnClickChangeBtn_2()
	{
		this.OnClickChangeBtn(2);
	}

	public void OnClickChangeBtn_3()
	{
		this.OnClickChangeBtn(3);
	}

	public void OnClickChangeBtn_4()
	{
		this.OnClickChangeBtn(4);
	}

	public void OnClickChangeBtn_5()
	{
		this.OnClickChangeBtn(5);
	}

	public void OnClickChangeBtn_6()
	{
		this.OnClickChangeBtn(6);
	}

	public void OnClickChangeBtn_7()
	{
		this.OnClickChangeBtn(7);
	}

	public void OnClickChangeBtn_8()
	{
		this.OnClickChangeBtn(8);
	}

	public void OnClickChangeBtn_9()
	{
		this.OnClickChangeBtn(9);
	}

	private void OnClickChangeBtn(int index)
	{
		if (GeneralUtil.IsNetwork())
		{
			if (this.m_rouletteList != null && this.m_rouletteList.Count > index)
			{
				if (this.m_rouletteList[index] == RouletteCategory.RAID)
				{
					if (EventManager.Instance != null)
					{
						EventManager.EventType typeInTime = EventManager.Instance.TypeInTime;
						if (typeInTime != EventManager.EventType.RAID_BOSS)
						{
							this.m_rouletteList.Remove(RouletteCategory.RAID);
							GeneralUtil.ShowEventEnd("ShowEventEnd");
							if (this.m_requestCategory == RouletteCategory.RAID)
							{
								this.Setup(this.m_rouletteList[0]);
							}
							else
							{
								this.ResetChangeBotton();
							}
						}
						else
						{
							this.Setup(this.m_rouletteList[index]);
						}
					}
					else
					{
						this.Setup(this.m_rouletteList[index]);
					}
				}
				else
				{
					this.Setup(this.m_rouletteList[index]);
				}
			}
		}
		else
		{
			GeneralUtil.ShowNoCommunication("ShowNoCommunication");
		}
	}

	private void AnimationFinishCallback()
	{
		if (this.m_close)
		{
			this.Remove();
		}
		else if (this.m_change && this.m_wheelDataAfter != null)
		{
			this.UpdateWheelData(this.m_wheelDataAfter, false);
			this.m_wheelDataAfter = null;
			this.m_change = false;
			if (this.m_animation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_mm_roulette_intro_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.AnimationFinishCallback), true);
				this.m_wheelData.PlaySe(ServerWheelOptionsData.SE_TYPE.Open, 0f);
			}
		}
	}

	private void Awake()
	{
		this.SetInstance();
		if (RouletteTop.s_instance != null)
		{
			this.SetPanelsAlpha(0f);
		}
	}

	private void OnDestroy()
	{
		if (RouletteTop.s_instance == this)
		{
			this.RemoveBackKeyCallBack();
			RouletteTop.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (RouletteTop.s_instance == null)
		{
			this.EntryBackKeyCallBack();
			RouletteTop.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void EntryBackKeyCallBack()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	private void RemoveBackKeyCallBack()
	{
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}

	public void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (base.gameObject.activeSelf)
		{
			bool flag = false;
			if (this.m_panels != null && this.m_parts != null && this.m_parts.Count > 0)
			{
				foreach (UIPanel current in this.m_panels)
				{
					if (current.alpha > 0.1f)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				if (msg != null)
				{
					msg.StaySequence();
				}
				if (!this.m_spin && this.m_wheelSetup && !GeneralWindow.Created)
				{
					ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
					if (((itemGetWindow != null && itemGetWindow.IsEnd) || itemGetWindow == null) && !GeneralWindow.Created && !EventBestChaoWindow.Created && !this.m_isWindow && !this.m_tutorial)
					{
						if (this.m_isTopPage)
						{
							HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.ROULETTE_BACK, false);
						}
						else
						{
							this.SetupTopPage(false);
							SoundManager.SePlay("sys_window_close", "SE");
						}
					}
				}
			}
		}
	}

	private void UpdateDebug()
	{
	}
}
