using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
	private const float RANKING_INIT_LOAD_DELAY = 0.25f;

	private const RankingUtil.RankingScoreType DEFAULT_SCORE_TYPE = RankingUtil.RankingScoreType.HIGH_SCORE;

	private const RankingUtil.RankingRankerType DEFAULT_RANKER_TYPE = RankingUtil.RankingRankerType.RIVAL;

	public const int BTN_NOT_COUNT = 5;

	[Header("モードごとのカラー設定"), SerializeField]
	private Color m_quickModeColor1;

	[SerializeField]
	private Color m_quickModeColor2;

	[SerializeField]
	private Color m_endlessModeColor1;

	[SerializeField]
	private Color m_endlessModeColor2;

	[SerializeField]
	private List<UISprite> m_colorObjects1;

	[SerializeField]
	private List<UISprite> m_colorObjects2;

	[Header("読み込み中表示"), SerializeField]
	private GameObject m_loading;

	[Header("SNSログインページ"), SerializeField]
	private GameObject m_facebook;

	[Header("ランキング初期ページ(自分と上位3人)"), SerializeField]
	private GameObject m_pattern0;

	[SerializeField]
	private UIRectItemStorage m_pattern0MyDataArea;

	[SerializeField]
	private UIRectItemStorage m_pattern0TopRankerArea;

	[SerializeField]
	private GameObject m_pattern0More;

	[Header("ランキング一覧"), SerializeField]
	private GameObject m_pattern1;

	[SerializeField]
	private UIRectItemStorageRanking m_pattern1ListArea;

	[SerializeField]
	private UIDraggablePanel m_pattern1MainListPanel;

	[Header("ランキング一覧(リーグ)"), SerializeField]
	private GameObject m_pattern2;

	[SerializeField]
	private UIRectItemStorageRanking m_pattern2ListArea;

	[SerializeField]
	private UIDraggablePanel m_pattern2MainListPanel;

	[Header("ボタン類などのオブジェクト"), SerializeField]
	private GameObject m_parts;

	[SerializeField]
	private GameObject m_partsTabNormal;

	[SerializeField]
	private GameObject m_partsTabRival;

	[SerializeField]
	private GameObject m_partsTabFriend;

	[SerializeField]
	private UILabel m_partsInfo;

	[SerializeField]
	private UIImageButton[] m_partsBtns;

	[SerializeField]
	private UISprite m_partsRankIcon0;

	[SerializeField]
	private UISprite m_partsRankIcon1;

	[SerializeField]
	private UILabel m_partsRankText;

	[SerializeField]
	private UILabel m_partsRankTextEx;

	[SerializeField]
	private GameObject m_tallying;

	[SerializeField]
	private UIToggle m_partsTabNormalTogglH;

	[SerializeField]
	private UIToggle m_partsTabNormalTogglT;

	[SerializeField]
	private UIToggle m_partsTabRivalTogglH;

	[SerializeField]
	private UIToggle m_partsTabRivalTogglT;

	[SerializeField]
	private UIToggle m_partsTabFriendTogglH;

	[SerializeField]
	private UIToggle m_partsTabFriendTogglT;

	[SerializeField]
	private List<UIToggle> m_partsBtnToggls;

	[Header("ヘルプウインド"), SerializeField]
	private ranking_help m_help;

	public static SocialInterface s_socialInterface;

	private bool m_isInitilalized;

	private bool m_isHelp;

	private bool m_isDrawInit;

	private bool m_rankingInitDraw;

	private RankingUtil.RankChange m_rankingChange;

	private float m_rankingInitloadingTime;

	private float m_rankingChangeTime;

	private RankingUtil.RankingScoreType m_currentScoreType;

	private RankingUtil.RankingRankerType m_currentRankerType;

	private List<RankingUtil.Ranker> m_currentRankerList;

	private int m_page;

	private bool m_pageNext;

	private bool m_pagePrev;

	private bool m_toggleLock;

	private bool m_facebookLock;

	private bool m_facebookLockInit;

	private bool m_snsCompGetRanking;

	private bool m_snsLogin;

	private bool m_first;

	private float m_snsCompGetRankingTime;

	private RankingCallbackTemporarilySaved m_callbackTemporarilySaved;

	private float m_callbackTemporarilySavedDelay;

	private TimeSpan m_currentResetTimeSpan;

	private float m_resetTimeSpanSec;

	private int m_btnDelay = 5;

	private EasySnsFeed m_easySnsFeed;

	private RankingUtil.RankingMode m_currentMode = RankingUtil.RankingMode.COUNT;

	private RankingUtil.RankingScoreType m_lastSelectedScoreType;

	private bool m_displayFlag;

	private static RankingUI s_instance;

	private static RankingUI Instance
	{
		get
		{
			return RankingUI.s_instance;
		}
	}

	private void SetRankingMode(RankingUtil.RankingMode mode)
	{
		this.RankerToggleChange(RankingUtil.RankingRankerType.RIVAL);
		global::Debug.Log(string.Concat(new object[]
		{
			"RankingUI SetRankingMode mode:",
			mode,
			" old:",
			this.m_currentMode
		}));
		if (mode != this.m_currentMode || this.m_currentMode == RankingUtil.RankingMode.COUNT)
		{
			this.m_currentMode = mode;
			this.m_isDrawInit = false;
			this.m_isHelp = false;
			if (this.m_loading != null)
			{
				this.m_loading.SetActive(true);
			}
			this.m_callbackTemporarilySavedDelay = 0.25f;
		}
		this.Init();
		RankingUtil.SetCurrentRankingMode(mode);
	}

	public void SetDisplayEndlessModeOn()
	{
		if (!this.m_isInitilalized)
		{
			this.Init();
		}
		this.SetDisplay(true, RankingUtil.RankingMode.ENDLESS);
	}

	public void SetDisplayEndlessModeOff()
	{
		this.SetDisplay(false, RankingUtil.RankingMode.ENDLESS);
	}

	public void SetDisplayQuickModeOn()
	{
		if (!this.m_isInitilalized)
		{
			this.Init();
		}
		this.SetDisplay(true, RankingUtil.RankingMode.QUICK);
	}

	public void SetDisplayQuickModeOff()
	{
		this.SetDisplay(false, RankingUtil.RankingMode.QUICK);
	}

	public void SetDisplay(bool displayFlag, RankingUtil.RankingMode mode)
	{
		this.m_displayFlag = displayFlag;
		base.gameObject.SetActive(this.m_displayFlag);
		if (this.m_displayFlag)
		{
			this.SetRankingMode(mode);
		}
		if (this.m_first && displayFlag && this.m_loading != null)
		{
			this.m_loading.SetActive(true);
		}
		if (this.m_parts != null)
		{
			this.m_parts.SetActive(this.m_displayFlag);
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_ranking_word");
		if (uISprite != null)
		{
			if (mode == RankingUtil.RankingMode.QUICK)
			{
				uISprite.spriteName = "ui_mm_mode_word_quick";
			}
			else
			{
				uISprite.spriteName = "ui_mm_mode_word_endless";
			}
		}
		if (this.m_colorObjects1 != null && this.m_colorObjects2 != null)
		{
			Color color;
			Color color2;
			if (mode == RankingUtil.RankingMode.QUICK)
			{
				color = this.m_quickModeColor1;
				color2 = this.m_quickModeColor2;
			}
			else
			{
				color = this.m_endlessModeColor1;
				color2 = this.m_endlessModeColor2;
			}
			foreach (UISprite current in this.m_colorObjects1)
			{
				if (current != null)
				{
					current.color = color;
				}
			}
			foreach (UISprite current2 in this.m_colorObjects2)
			{
				if (current2 != null)
				{
					current2.color = color2;
				}
			}
		}
	}

	public void SetLoadingObject()
	{
		if (this.m_tallying != null)
		{
			this.m_tallying.SetActive(false);
		}
	}

	public bool IsInitLoading()
	{
		return this.m_loading != null && this.m_loading.activeSelf;
	}

	public RankingUtil.Ranker GetCurrentRanker(int slot)
	{
		RankingUtil.Ranker result = null;
		if (this.m_currentRankerList != null && slot >= 0 && this.m_currentRankerList.Count > slot + 1)
		{
			result = this.m_currentRankerList[slot + 1];
		}
		return result;
	}

	private void Start()
	{
		foreach (UIToggle current in this.m_partsBtnToggls)
		{
			if (current != null)
			{
				if (current.gameObject.name == "Btn_all")
				{
					EventDelegate.Add(current.onChange, new EventDelegate.Callback(this.OnAllToggleChange));
				}
				else if (current.gameObject.name == "Btn_friend")
				{
					EventDelegate.Add(current.onChange, new EventDelegate.Callback(this.OnFriendToggleChange));
				}
				else if (current.gameObject.name == "Btn_history")
				{
					EventDelegate.Add(current.onChange, new EventDelegate.Callback(this.OnHistoryToggleChange));
				}
				else if (current.gameObject.name == "Btn_rival")
				{
					EventDelegate.Add(current.onChange, new EventDelegate.Callback(this.OnRivalToggleChange));
				}
			}
		}
	}

	public static void DebugInitRankingChange()
	{
		if (RankingUI.s_instance != null)
		{
			RankingUI.s_instance.InitRankingChange();
		}
	}

	private void InitRankingChange()
	{
		this.m_rankingChange = SingletonGameObject<RankingManager>.Instance.GetRankingRankChange(RankingUtil.currentRankingMode);
		if (this.m_rankingChange != RankingUtil.RankChange.UP)
		{
			this.m_rankingChange = RankingUtil.RankChange.NONE;
		}
		this.m_rankingChangeTime = 0f;
	}

	private void Init()
	{
		if (this.m_snsLogin)
		{
			return;
		}
		this.m_snsCompGetRanking = false;
		this.m_snsCompGetRankingTime = 0f;
		this.m_first = true;
		this.m_isInitilalized = false;
		this.m_rankingInitloadingTime = 0f;
	}

	private void InitSetting()
	{
		this.m_callbackTemporarilySavedDelay = 0.25f;
		this.m_facebookLockInit = false;
		if (this.m_loading != null)
		{
			this.m_loading.SetActive(true);
		}
		if (this.m_tallying != null)
		{
			this.m_tallying.SetActive(false);
		}
		if (this.m_rankingInitDraw)
		{
			this.m_rankingChange = SingletonGameObject<RankingManager>.Instance.GetRankingRankChange(RankingUtil.currentRankingMode);
			if (this.m_rankingChange != RankingUtil.RankChange.UP)
			{
				this.m_rankingChange = RankingUtil.RankChange.NONE;
			}
			this.m_rankingChangeTime = 0f;
		}
		this.SetupLeague();
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface == null)
		{
			ServerInterface.DebugInit();
		}
		RankingUI.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		global::Debug.Log("RankingUI  Init !!!!!");
		this.m_currentRankerType = RankingUtil.RankingRankerType.RIVAL;
		this.m_currentScoreType = RankingManager.EndlessRivalRankingScoreType;
		RankingManager instance = SingletonGameObject<RankingManager>.Instance;
		if (instance != null && !instance.isLoading && instance.IsRankingTop(RankingUtil.currentRankingMode, RankingManager.EndlessRivalRankingScoreType, RankingUtil.RankingRankerType.RIVAL))
		{
			this.SetRanking(RankingUtil.RankingRankerType.RIVAL, RankingManager.EndlessRivalRankingScoreType, 0);
		}
		this.m_snsCompGetRankingTime = 0f;
		this.m_snsCompGetRanking = false;
		this.m_isDrawInit = false;
		this.m_isInitilalized = true;
		this.m_rankingInitloadingTime = 0f;
		this.m_btnDelay = 5;
		this.m_facebookLock = true;
		if (RegionManager.Instance != null)
		{
			this.m_facebookLock = !RegionManager.Instance.IsUseSNS();
		}
		UIImageButton[] partsBtns = this.m_partsBtns;
		for (int i = 0; i < partsBtns.Length; i++)
		{
			UIImageButton uIImageButton = partsBtns[i];
			if (uIImageButton.name.IndexOf("friend") != -1)
			{
				uIImageButton.isEnabled = !this.m_facebookLock;
			}
		}
		this.m_first = false;
	}

	private bool SetRanking(RankingUtil.RankingRankerType type, RankingUtil.RankingScoreType score, int page)
	{
		if (page == -1 || this.m_currentRankerType != type || this.m_currentScoreType != score || this.m_page != page)
		{
			RankingManager instance = SingletonGameObject<RankingManager>.Instance;
			if (instance != null && !instance.isLoading)
			{
				if (page <= 0)
				{
					this.ResetRankerList(0, type);
					this.ResetRankerList(1, type);
				}
				this.m_page = page;
				this.m_currentRankerType = type;
				this.m_currentScoreType = score;
				if (this.m_page < 0)
				{
					this.m_page = 0;
				}
				if (RankingUI.s_socialInterface != null)
				{
					if (type == RankingUtil.RankingRankerType.FRIEND && !RankingUI.s_socialInterface.IsLoggedIn)
					{
						if (this.m_facebook != null)
						{
							this.m_facebook.SetActive(true);
							this.ResetRankerList(0, type);
							if (this.m_partsInfo != null)
							{
								this.m_partsInfo.text = string.Empty;
							}
							return true;
						}
					}
					else if (type == RankingUtil.RankingRankerType.FRIEND && RankingUI.s_socialInterface.IsLoggedIn)
					{
						if (this.m_facebook != null)
						{
							this.m_facebook.SetActive(false);
						}
					}
					else if (this.m_facebook != null)
					{
						this.m_facebook.SetActive(false);
					}
				}
				else
				{
					if (type == RankingUtil.RankingRankerType.SP_FRIEND)
					{
						this.m_facebook.SetActive(true);
						this.ResetRankerList(0, type);
						if (this.m_partsInfo != null)
						{
							this.m_partsInfo.text = string.Empty;
						}
						return true;
					}
					if (this.m_facebook != null)
					{
						this.m_facebook.SetActive(false);
					}
				}
				if (this.m_pattern0More != null)
				{
					this.m_pattern0More.SetActive(false);
				}
				if (this.m_loading != null)
				{
					this.m_loading.SetActive(true);
				}
				return instance.GetRanking(RankingUtil.currentRankingMode, score, type, this.m_page, new RankingManager.CallbackRankingData(this.CallbackRanking));
			}
		}
		return true;
	}

	private void ResetRankerList(int page, RankingUtil.RankingRankerType type)
	{
		if (this.m_page > 1)
		{
			return;
		}
		if (this.m_parts != null)
		{
			this.m_parts.SetActive(true);
		}
		if (page > 0)
		{
			if (type == RankingUtil.RankingRankerType.RIVAL)
			{
				if (this.m_pattern0 != null)
				{
					this.m_pattern0.SetActive(false);
				}
				if (this.m_pattern1 != null)
				{
					this.m_pattern1.SetActive(false);
				}
				if (this.m_pattern2 != null)
				{
					this.m_pattern2.SetActive(true);
				}
			}
			else
			{
				if (this.m_pattern0 != null)
				{
					this.m_pattern0.SetActive(false);
				}
				if (this.m_pattern1 != null)
				{
					this.m_pattern1.SetActive(true);
				}
				if (this.m_pattern2 != null)
				{
					this.m_pattern2.SetActive(false);
				}
			}
		}
		else
		{
			if (this.m_pattern0 != null)
			{
				this.m_pattern0.SetActive(true);
			}
			if (this.m_pattern1 != null)
			{
				this.m_pattern1.SetActive(false);
			}
			if (this.m_pattern2 != null)
			{
				this.m_pattern2.SetActive(false);
			}
		}
		if (this.m_pattern1ListArea != null)
		{
			this.m_pattern1ListArea.Reset();
		}
		if (this.m_pattern2ListArea != null)
		{
			this.m_pattern2ListArea.Reset();
		}
		if (this.m_pattern0MyDataArea != null)
		{
			this.m_pattern0MyDataArea.maxItemCount = (this.m_pattern0MyDataArea.maxRows = 0);
			this.m_pattern0MyDataArea.Restart();
		}
		if (this.m_pattern0TopRankerArea != null)
		{
			this.m_pattern0TopRankerArea.maxItemCount = (this.m_pattern0TopRankerArea.maxRows = 0);
			this.m_pattern0TopRankerArea.Restart();
		}
	}

	private void CallbackRanking(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		if (this.m_callbackTemporarilySavedDelay > 0f)
		{
			this.m_callbackTemporarilySaved = new RankingCallbackTemporarilySaved(rankerList, score, type, page, isNext, isPrev, isCashData, new RankingManager.CallbackRankingData(this.CallbackRanking));
			return;
		}
		this.m_callbackTemporarilySavedDelay = 0f;
		global::Debug.Log(string.Concat(new object[]
		{
			"RankingUI:CallbackRanking  type:",
			type,
			"  score",
			score,
			"  num:",
			rankerList.Count,
			" isNext:",
			isNext,
			" !!!!"
		}));
		if (this.m_currentRankerType != type || this.m_currentScoreType != score)
		{
			return;
		}
		if (type == RankingUtil.RankingRankerType.RIVAL)
		{
			this.m_partsTabRival.SetActive(true);
			this.m_partsTabNormal.SetActive(false);
			this.m_partsTabFriend.SetActive(false);
		}
		else if (type == RankingUtil.RankingRankerType.FRIEND)
		{
			this.m_partsTabRival.SetActive(false);
			this.m_partsTabNormal.SetActive(false);
			this.m_partsTabFriend.SetActive(true);
			this.m_snsLogin = false;
		}
		else
		{
			this.m_partsTabRival.SetActive(false);
			this.m_partsTabNormal.SetActive(true);
			this.m_partsTabFriend.SetActive(false);
		}
		this.m_snsCompGetRankingTime = 0f;
		this.m_snsCompGetRanking = false;
		this.m_pageNext = isNext;
		this.m_pagePrev = isPrev;
		if (this.m_pattern1ListArea != null)
		{
			this.m_pattern1ListArea.rankingType = type;
		}
		if (this.m_pattern2ListArea != null)
		{
			this.m_pattern2ListArea.rankingType = type;
		}
		this.SetRankerList(rankerList, type, page);
		if (this.m_pattern1MainListPanel != null && page <= 1)
		{
			if (type == RankingUtil.RankingRankerType.RIVAL)
			{
				GameObject myDataGameObject = this.m_pattern2ListArea.GetMyDataGameObject();
				if (myDataGameObject != null)
				{
					float num = myDataGameObject.transform.localPosition.y * -1f - 166f;
					if (num < -36f)
					{
						num = -36f;
					}
					this.m_pattern2MainListPanel.transform.localPosition = new Vector3(this.m_pattern2MainListPanel.transform.localPosition.x, num, this.m_pattern2MainListPanel.transform.localPosition.z);
					this.m_pattern2MainListPanel.panel.clipRange = new Vector4(this.m_pattern2MainListPanel.panel.clipRange.x, -num, this.m_pattern2MainListPanel.panel.clipRange.z, this.m_pattern2MainListPanel.panel.clipRange.w);
					this.m_pattern2ListArea.CheckItemDrawAll(true);
				}
				else
				{
					this.m_pattern2MainListPanel.Scroll(0f);
					this.m_pattern2MainListPanel.ResetPosition();
				}
			}
			else
			{
				this.m_pattern1MainListPanel.Scroll(0f);
				this.m_pattern1MainListPanel.ResetPosition();
			}
		}
		if (isNext && this.m_pattern0More != null)
		{
			this.m_pattern0More.SetActive(true);
		}
		this.m_currentResetTimeSpan = SingletonGameObject<RankingManager>.Instance.GetRankigResetTimeSpan(RankingUtil.currentRankingMode, this.m_currentScoreType, this.m_currentRankerType);
		this.m_resetTimeSpanSec = (float)this.m_currentResetTimeSpan.Seconds + 0.1f;
		if (this.m_loading != null && this.m_rankingChange == RankingUtil.RankChange.NONE)
		{
			this.m_loading.SetActive(false);
		}
		this.SetTogglBtn();
		if (this.m_currentResetTimeSpan.Ticks <= 0L && rankerList.Count <= 1)
		{
			if (type != RankingUtil.RankingRankerType.FRIEND || (type == RankingUtil.RankingRankerType.FRIEND && this.IsActiveSnsLoginGameObject()))
			{
				if (this.m_tallying != null)
				{
					this.m_tallying.SetActive(true);
				}
			}
			else if (this.m_tallying != null)
			{
				this.m_tallying.SetActive(false);
			}
		}
		else if (this.m_tallying != null)
		{
			this.m_tallying.SetActive(false);
		}
		this.SetupRankingReset(this.m_currentResetTimeSpan);
	}

	private void SetTogglBtn()
	{
		this.m_toggleLock = true;
		if (this.m_partsBtnToggls != null && this.m_partsBtnToggls.Count > 0)
		{
			UIToggle uIToggle = null;
			switch (this.m_currentRankerType)
			{
			case RankingUtil.RankingRankerType.FRIEND:
				foreach (UIToggle current in this.m_partsBtnToggls)
				{
					if (current != null && (current.gameObject.name.IndexOf("friend") != -1 || current.gameObject.name.IndexOf("Friend") != -1))
					{
						uIToggle = current;
						break;
					}
				}
				break;
			case RankingUtil.RankingRankerType.RIVAL:
				foreach (UIToggle current2 in this.m_partsBtnToggls)
				{
					if (current2 != null && (current2.gameObject.name.IndexOf("rival") != -1 || current2.gameObject.name.IndexOf("Rival") != -1))
					{
						uIToggle = current2;
						break;
					}
				}
				break;
			}
			if (uIToggle != null)
			{
				uIToggle.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
		}
		if (this.m_currentRankerType == RankingUtil.RankingRankerType.RIVAL)
		{
			if (this.m_partsTabRivalTogglH != null && this.m_partsTabRivalTogglT != null)
			{
				if (this.m_currentScoreType == RankingUtil.RankingScoreType.HIGH_SCORE)
				{
					this.m_partsTabRivalTogglH.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					this.m_partsTabRivalTogglT.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		else if ((this.m_currentRankerType == RankingUtil.RankingRankerType.FRIEND && this.IsActiveSnsLoginGameObject()) || this.m_currentRankerType != RankingUtil.RankingRankerType.FRIEND)
		{
			if (this.m_partsTabNormalTogglH != null && this.m_partsTabNormalTogglT != null)
			{
				if (this.m_currentScoreType == RankingUtil.RankingScoreType.HIGH_SCORE)
				{
					this.m_partsTabNormalTogglH.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					this.m_partsTabNormalTogglT.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		else if (this.m_partsTabFriendTogglH != null && this.m_partsTabFriendTogglT != null)
		{
			if (this.m_currentScoreType == RankingUtil.RankingScoreType.HIGH_SCORE)
			{
				this.m_partsTabFriendTogglH.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				this.m_partsTabFriendTogglT.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
		}
		this.m_toggleLock = false;
	}

	private void SetRankerList(List<RankingUtil.Ranker> rankers, RankingUtil.RankingRankerType type, int page)
	{
		if (page > 0 || type == RankingUtil.RankingRankerType.RIVAL)
		{
			this.m_currentRankerList = rankers;
		}
		if (type != RankingUtil.RankingRankerType.FRIEND && this.m_facebook != null)
		{
			this.m_facebook.SetActive(false);
		}
		if (page > 0 || type == RankingUtil.RankingRankerType.RIVAL)
		{
			if (this.m_pattern0 != null)
			{
				this.m_pattern0.SetActive(false);
			}
			if (this.m_pattern1 != null)
			{
				this.m_pattern1.SetActive(type != RankingUtil.RankingRankerType.RIVAL);
			}
			if (this.m_pattern2 != null)
			{
				this.m_pattern2.SetActive(type == RankingUtil.RankingRankerType.RIVAL);
			}
			if (type == RankingUtil.RankingRankerType.RIVAL)
			{
				if (this.m_pattern2ListArea != null)
				{
					if (page < 1)
					{
						this.m_pattern2ListArea.Reset();
						this.AddRectItemStorageRanking(this.m_pattern2ListArea, rankers, type);
					}
					else
					{
						if (page == 1)
						{
							this.m_pattern2ListArea.Reset();
						}
						this.AddRectItemStorageRanking(this.m_pattern2ListArea, rankers, type);
					}
				}
			}
			else if (this.m_pattern1ListArea != null)
			{
				if (page == 1)
				{
					this.m_pattern1ListArea.Reset();
				}
				this.AddRectItemStorageRanking(this.m_pattern1ListArea, rankers, type);
			}
			if (this.m_pattern0MyDataArea != null)
			{
				this.m_pattern0MyDataArea.maxItemCount = (this.m_pattern0MyDataArea.maxRows = 0);
				this.m_pattern0MyDataArea.Restart();
			}
			if (this.m_pattern0TopRankerArea != null)
			{
				this.m_pattern0TopRankerArea.maxItemCount = (this.m_pattern0TopRankerArea.maxRows = 0);
				this.m_pattern0TopRankerArea.Restart();
			}
		}
		else
		{
			if (this.m_pattern0 != null)
			{
				this.m_pattern0.SetActive(true);
			}
			if (this.m_pattern1 != null)
			{
				this.m_pattern1.SetActive(false);
			}
			if (this.m_pattern2 != null)
			{
				this.m_pattern2.SetActive(false);
			}
			if (this.m_pattern1ListArea != null)
			{
				this.m_pattern1ListArea.Reset();
			}
			if (this.m_pattern2ListArea != null)
			{
				this.m_pattern2ListArea.Reset();
			}
			if (this.m_pattern0MyDataArea != null)
			{
				if (rankers != null && rankers.Count > 0)
				{
					if (rankers[0] != null)
					{
						this.m_pattern0MyDataArea.maxItemCount = (this.m_pattern0MyDataArea.maxRows = 1);
						this.UpdateRectItemStorage(this.m_pattern0MyDataArea, rankers, 0);
					}
					else
					{
						this.m_pattern0MyDataArea.maxItemCount = (this.m_pattern0MyDataArea.maxRows = 0);
						this.m_pattern0MyDataArea.Restart();
					}
				}
				else
				{
					this.m_pattern0MyDataArea.maxItemCount = (this.m_pattern0MyDataArea.maxRows = 0);
					this.m_pattern0MyDataArea.Restart();
				}
			}
			if (this.m_pattern0TopRankerArea != null && rankers != null)
			{
				if (rankers.Count - 1 >= RankingManager.GetRankingMax(type, page))
				{
					this.m_pattern0TopRankerArea.maxItemCount = (this.m_pattern0TopRankerArea.maxRows = 3);
				}
				else
				{
					this.m_pattern0TopRankerArea.maxItemCount = (this.m_pattern0TopRankerArea.maxRows = rankers.Count - 1);
				}
				this.UpdateRectItemStorage(this.m_pattern0TopRankerArea, rankers, 1);
			}
		}
	}

	private void AddRectItemStorageRanking(UIRectItemStorageRanking ui_rankers, List<RankingUtil.Ranker> rankerList, RankingUtil.RankingRankerType type)
	{
		int childCount = ui_rankers.childCount;
		int num = rankerList.Count - childCount;
		if (this.m_pageNext)
		{
			num--;
		}
		if (ui_rankers.callback == null)
		{
			ui_rankers.callback = new UIRectItemStorageRanking.CallbackCreated(this.CallbackItemStorageRanking);
			ui_rankers.callbackTopOrLast = new UIRectItemStorageRanking.CallbackTopOrLast(this.CallbackItemStorageRankingTopOrLast);
		}
		GameObject mainMenuUIObject = HudMenuUtility.GetMainMenuUIObject();
		if (mainMenuUIObject != null && mainMenuUIObject.activeSelf)
		{
			ui_rankers.AddItem(num, 0.02f);
		}
		else
		{
			ui_rankers.AddItem(num, 0f);
		}
	}

	private bool CallbackItemStorageRankingTopOrLast(bool isTop)
	{
		bool result = false;
		RankingManager instance = SingletonGameObject<RankingManager>.Instance;
		if (instance != null && !instance.isLoading)
		{
			if (isTop)
			{
				if (this.m_pagePrev)
				{
					result = false;
				}
			}
			else if (this.m_pageNext)
			{
				result = instance.GetRankingScroll(RankingUtil.currentRankingMode, true, new RankingManager.CallbackRankingData(this.CallbackRanking));
			}
		}
		return result;
	}

	private void CallbackItemStorageRanking(ui_ranking_scroll_dummy obj, UIRectItemStorageRanking storage)
	{
		if (obj != null && this.m_currentRankerList != null)
		{
			int num = obj.slot + 1;
			if (num > 0 && this.m_currentRankerList.Count > num)
			{
				RankingUtil.Ranker rankerData = this.m_currentRankerList[num];
				if (obj.myRankerData == null)
				{
					obj.myRankerData = this.m_currentRankerList[0];
				}
				obj.spWindow = null;
				obj.rankingUI = this;
				obj.rankerData = rankerData;
				obj.rankerType = this.m_currentRankerType;
				obj.scoreType = this.m_currentScoreType;
				obj.SetActiveObject(storage.CheckItemDraw(obj.slot), 0f);
				obj.end = (obj.slot + 1 == this.m_currentRankerList.Count);
			}
			else
			{
				UnityEngine.Object.Destroy(obj.gameObject);
			}
		}
	}

	private void UpdateRectItemStorage(UIRectItemStorage ui_rankers, List<RankingUtil.Ranker> rankerList, int head = 1)
	{
		ui_rankers.Restart();
		ui_ranking_scroll[] componentsInChildren = ui_rankers.GetComponentsInChildren<ui_ranking_scroll>(true);
		for (int i = 0; i < ui_rankers.maxItemCount; i++)
		{
			if (i + head >= rankerList.Count)
			{
				break;
			}
			RankingUtil.Ranker ranker = rankerList[i + head];
			if (ranker != null)
			{
				componentsInChildren[i].UpdateView(this.m_currentScoreType, this.m_currentRankerType, ranker, i == ui_rankers.maxItemCount - 1);
				bool myRanker = false;
				if (rankerList[0] != null && ranker.id == rankerList[0].id)
				{
					myRanker = true;
				}
				componentsInChildren[i].SetMyRanker(myRanker);
			}
		}
	}

	private static string[] GetFriendIdList(RankingUtil.RankingRankerType rankerType)
	{
		string[] result = null;
		if (rankerType == RankingUtil.RankingRankerType.FRIEND)
		{
			result = RankingUtil.GetFriendIdList();
		}
		return result;
	}

	private void Update()
	{
		if (this.m_isInitilalized)
		{
			if (this.m_easySnsFeed != null)
			{
				EasySnsFeed.Result result = this.m_easySnsFeed.Update();
				if (result != EasySnsFeed.Result.COMPLETED)
				{
					if (result == EasySnsFeed.Result.FAILED)
					{
						this.m_snsLogin = false;
						this.m_snsCompGetRankingTime = 0f;
						this.m_snsCompGetRanking = false;
						this.m_easySnsFeed = null;
					}
				}
				else
				{
					this.m_snsCompGetRanking = true;
					this.m_snsCompGetRankingTime = 0f;
					this.m_currentRankerList = null;
					this.ResetRankerList(0, this.m_currentRankerType);
					global::Debug.Log("SetRanking m_easySnsFeed");
					this.SetRanking(RankingUtil.RankingRankerType.FRIEND, this.m_lastSelectedScoreType, -1);
					this.m_easySnsFeed = null;
				}
			}
			else if (this.m_snsCompGetRanking)
			{
				this.m_snsCompGetRankingTime += Time.deltaTime;
				if (this.m_snsCompGetRankingTime > 5f)
				{
					global::Debug.Log("SetRanking m_easySnsFeed reload !");
					this.SetRanking(RankingUtil.RankingRankerType.FRIEND, this.m_lastSelectedScoreType, -1);
					this.m_snsCompGetRankingTime = -5f;
				}
			}
			else
			{
				this.m_snsCompGetRankingTime = 0f;
			}
		}
		if (this.m_isInitilalized && !this.m_isDrawInit)
		{
			global::Debug.Log("m_isInitilalized");
			RankingManager instance = SingletonGameObject<RankingManager>.Instance;
			if (instance != null && !instance.isLoading && instance.IsRankingTop(RankingUtil.currentRankingMode, RankingManager.EndlessRivalRankingScoreType, RankingUtil.RankingRankerType.RIVAL))
			{
				this.SetRanking(this.m_currentRankerType, this.m_currentScoreType, -1);
			}
			this.m_isDrawInit = true;
		}
		if (this.m_resetTimeSpanSec <= 0f)
		{
			this.m_currentResetTimeSpan = SingletonGameObject<RankingManager>.Instance.GetRankigResetTimeSpan(RankingUtil.currentRankingMode, this.m_currentScoreType, this.m_currentRankerType);
			this.m_resetTimeSpanSec = (float)this.m_currentResetTimeSpan.Seconds + 0.1f;
			if (this.m_currentResetTimeSpan.Ticks > 0L)
			{
				if (this.m_currentResetTimeSpan.Days < 1 && this.m_currentResetTimeSpan.Hours < 1 && this.m_currentResetTimeSpan.Minutes < 1)
				{
					this.m_resetTimeSpanSec = (float)this.m_currentResetTimeSpan.Milliseconds / 1000f + 0.005f;
				}
			}
			else
			{
				this.m_resetTimeSpanSec = 300f;
			}
			this.SetupRankingReset(this.m_currentResetTimeSpan);
		}
		else
		{
			this.m_resetTimeSpanSec -= Time.deltaTime;
		}
		if (this.m_btnDelay > 0)
		{
			this.m_btnDelay--;
		}
		if (this.m_rankingChange != RankingUtil.RankChange.NONE && this.m_rankingInitDraw)
		{
			if (this.IsRankingActive())
			{
				this.m_rankingChangeTime += Time.deltaTime;
			}
			else
			{
				this.m_rankingChangeTime = 0f;
			}
			if (this.m_rankingChangeTime > 0.25f)
			{
				RankingUtil.ShowRankingChangeWindow(RankingUtil.currentRankingMode);
				this.m_rankingChange = RankingUtil.RankChange.NONE;
				if (this.m_loading != null)
				{
					this.m_loading.SetActive(false);
				}
			}
		}
		if (this.m_isInitilalized && !this.m_facebookLockInit && !this.m_facebookLock && this.IsRankingActive())
		{
			if (RegionManager.Instance != null)
			{
				this.m_facebookLock = !RegionManager.Instance.IsUseSNS();
			}
			UIImageButton[] partsBtns = this.m_partsBtns;
			for (int i = 0; i < partsBtns.Length; i++)
			{
				UIImageButton uIImageButton = partsBtns[i];
				if (uIImageButton.name.IndexOf("friend") != -1)
				{
					uIImageButton.isEnabled = !this.m_facebookLock;
					break;
				}
			}
			this.m_facebookLockInit = true;
		}
		if (!this.m_rankingInitDraw)
		{
			GameObject x = GameObject.Find("UI Root (2D)");
			if (x != null)
			{
				this.m_rankingInitDraw = true;
			}
		}
		else if (this.m_loading != null && this.m_loading.activeSelf)
		{
			if (this.IsRankingActive())
			{
				this.m_rankingInitloadingTime += Time.deltaTime;
			}
			else
			{
				this.m_rankingInitloadingTime = 0f;
			}
			if (this.m_first && !this.m_isInitilalized && this.m_rankingInitloadingTime > 0.5f)
			{
				this.InitSetting();
				this.m_rankingInitloadingTime = -10f;
			}
			else if (this.m_rankingInitloadingTime > 5f)
			{
				if (SingletonGameObject<RankingManager>.Instance != null)
				{
					RankingManager instance2 = SingletonGameObject<RankingManager>.Instance;
					if (!instance2.isLoading)
					{
						if (this.m_currentRankerType == RankingUtil.RankingRankerType.RIVAL && !instance2.IsRankingTop(RankingUtil.currentRankingMode, RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.RIVAL))
						{
							SingletonGameObject<RankingManager>.Instance.InitNormal(RankingUtil.currentRankingMode, null);
							this.m_rankingInitloadingTime = -30f;
						}
						else
						{
							this.m_rankingInitloadingTime = -60f;
						}
					}
					else
					{
						this.m_rankingInitloadingTime = -40f;
					}
				}
				else
				{
					this.m_rankingInitloadingTime = -60f;
				}
			}
		}
		if (this.m_callbackTemporarilySaved != null && base.gameObject.activeSelf)
		{
			if (this.m_callbackTemporarilySavedDelay <= 0f)
			{
				this.m_callbackTemporarilySaved.SendCallback();
				this.m_callbackTemporarilySaved = null;
			}
			else if (this.IsRankingActive() && base.gameObject.activeSelf)
			{
				if (Time.deltaTime <= 0f)
				{
					this.m_callbackTemporarilySavedDelay -= 1f / (float)Application.targetFrameRate;
				}
				else
				{
					this.m_callbackTemporarilySavedDelay -= Time.deltaTime;
				}
			}
			else
			{
				this.m_callbackTemporarilySavedDelay = 0.25f;
			}
		}
	}

	private void OnClickNextButton()
	{
	}

	public void OnHighScoreToggleChange()
	{
		if (!this.m_toggleLock)
		{
			this.scoreToggleChange(RankingUtil.RankingScoreType.HIGH_SCORE);
		}
	}

	public void OnWeeklyToggleChange()
	{
		if (!this.m_toggleLock)
		{
			this.scoreToggleChange(RankingUtil.RankingScoreType.TOTAL_SCORE);
		}
	}

	private void OnClickHelpButton()
	{
		if (this.m_help != null)
		{
			this.m_help.Open(!this.m_isHelp);
			this.m_isHelp = true;
		}
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickScoreType()
	{
		SoundManager.SePlay("sys_page_skip", "SE");
	}

	public void scoreToggleChange(RankingUtil.RankingScoreType scoreType)
	{
		if (this.m_isInitilalized && scoreType != this.m_currentScoreType)
		{
			SoundManager.SePlay("sys_page_skip", "SE");
			global::Debug.Log("SetRanking scoreToggleChange");
			this.SetRanking(this.m_currentRankerType, scoreType, 0);
			if (this.m_currentRankerType != RankingUtil.RankingRankerType.RIVAL)
			{
				this.m_lastSelectedScoreType = scoreType;
			}
		}
	}

	public void OnFriendToggleChange()
	{
		if (!this.m_toggleLock && this.m_currentRankerType != RankingUtil.RankingRankerType.FRIEND)
		{
			this.RankerToggleChange(RankingUtil.RankingRankerType.FRIEND);
		}
	}

	public void OnAllToggleChange()
	{
		if (!this.m_toggleLock && this.m_currentRankerType != RankingUtil.RankingRankerType.ALL)
		{
			this.RankerToggleChange(RankingUtil.RankingRankerType.ALL);
		}
	}

	public void OnRivalToggleChange()
	{
		if (!this.m_toggleLock && this.m_currentRankerType != RankingUtil.RankingRankerType.RIVAL)
		{
			this.RankerToggleChange(RankingUtil.RankingRankerType.RIVAL);
		}
	}

	public void OnHistoryToggleChange()
	{
		if (!this.m_toggleLock && this.m_currentRankerType != RankingUtil.RankingRankerType.HISTORY)
		{
			this.RankerToggleChange(RankingUtil.RankingRankerType.HISTORY);
		}
	}

	private void RankerToggleChange(RankingUtil.RankingRankerType rankerType)
	{
		if (this.m_isInitilalized)
		{
			SoundManager.SePlay("sys_page_skip", "SE");
			if (rankerType == RankingUtil.RankingRankerType.RIVAL)
			{
				this.m_currentScoreType = ((!this.m_partsTabRivalTogglH.value) ? RankingUtil.RankingScoreType.TOTAL_SCORE : RankingUtil.RankingScoreType.HIGH_SCORE);
			}
			else
			{
				this.m_currentScoreType = this.m_lastSelectedScoreType;
			}
			if (!this.SetRanking(rankerType, this.m_currentScoreType, 0))
			{
				if (this.m_loading != null)
				{
					this.m_loading.SetActive(true);
				}
				if (this.m_tallying != null)
				{
					this.m_tallying.SetActive(false);
				}
			}
		}
	}

	private void OnClickMoreButton()
	{
		if (this.m_isInitilalized)
		{
			SoundManager.SePlay("sys_page_skip", "SE");
			this.SetRanking(this.m_currentRankerType, this.m_currentScoreType, 1);
		}
	}

	private void OnClickSnsLogin()
	{
		this.m_snsLogin = true;
		this.m_snsCompGetRankingTime = 0f;
		this.m_snsCompGetRanking = false;
		this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/menu_Anim/ui_mm_ranking_page/Anchor_5_MC");
	}

	private void OnClickFriendOption()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		GameObject loadMenuChildObject = HudMenuUtility.GetLoadMenuChildObject("RankingFriendOptionWindow", true);
		if (loadMenuChildObject != null)
		{
			RankingFriendOptionWindow component = loadMenuChildObject.GetComponent<RankingFriendOptionWindow>();
			if (component != null)
			{
				component.StartCoroutine("SetUp");
			}
		}
	}

	private void OnClickFriendOptionOk()
	{
		this.SetRanking(RankingUtil.RankingRankerType.FRIEND, this.m_currentScoreType, -1);
		if (SingletonGameObject<RankingManager>.Instance != null && EventManager.Instance != null && EventManager.Instance.Type == EventManager.EventType.SPECIAL_STAGE)
		{
			SingletonGameObject<RankingManager>.Instance.Reset(RankingUtil.RankingMode.ENDLESS, RankingUtil.RankingRankerType.SP_FRIEND);
		}
	}

	private void SetupRankingReset(TimeSpan span)
	{
		if (this.m_partsInfo != null)
		{
			RankingManager instance = SingletonGameObject<RankingManager>.Instance;
			if (instance != null)
			{
				if (this.m_currentRankerType == RankingUtil.RankingRankerType.FRIEND && RankingUI.s_socialInterface != null && !RankingUI.s_socialInterface.IsLoggedIn)
				{
					this.m_partsInfo.text = string.Empty;
				}
				else
				{
					this.m_partsInfo.text = RankingUtil.GetResetTime(span, true);
				}
			}
		}
	}

	private void SetupLeague()
	{
		RankingUtil.SetLeagueObject(RankingUtil.currentRankingMode, ref this.m_partsRankIcon0, ref this.m_partsRankIcon1, ref this.m_partsRankText, ref this.m_partsRankTextEx);
	}

	private bool IsActiveSnsLoginGameObject()
	{
		return this.m_currentRankerType == RankingUtil.RankingRankerType.FRIEND && RankingUI.s_socialInterface != null && !RankingUI.s_socialInterface.IsLoggedIn;
	}

	private void OnSettingPartsSnsAdditional()
	{
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms DEBUG_INFO " + s);
	}

	[Conditional("DEBUG_INFO2")]
	private static void DebugLog2(string s)
	{
		global::Debug.Log("@ms DEBUG_INFO2" + s);
	}

	[Conditional("DEBUG_INFO3")]
	private static void DebugLog3(string s)
	{
		global::Debug.Log("@ms DEBUG_INFO3" + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}

	public static RankingUI Setup()
	{
		if (RankingUI.s_instance != null)
		{
			RankingUI.s_instance.Init();
			return RankingUI.s_instance;
		}
		return null;
	}

	public static void CheckSnsUse()
	{
		if (RankingUI.s_instance != null)
		{
			RankingUI.s_instance.CheckSns();
		}
	}

	public void CheckSns()
	{
		if (RegionManager.Instance != null)
		{
			this.m_facebookLock = !RegionManager.Instance.IsUseSNS();
		}
		UIImageButton[] partsBtns = this.m_partsBtns;
		for (int i = 0; i < partsBtns.Length; i++)
		{
			UIImageButton uIImageButton = partsBtns[i];
			if (uIImageButton.name.IndexOf("friend") != -1)
			{
				uIImageButton.isEnabled = !this.m_facebookLock;
				break;
			}
		}
		this.m_facebookLockInit = true;
	}

	public void UpdateSendChallengeOrg(RankingUtil.RankingRankerType type, string id)
	{
		global::Debug.Log("RankingUI:UpdateSendChallengeOrg type:" + type);
		if (this.m_currentRankerType == type)
		{
			if (type == RankingUtil.RankingRankerType.RIVAL)
			{
				if (this.m_pattern2 != null && this.m_pattern2.activeSelf && this.m_pattern2ListArea != null)
				{
					ui_ranking_scroll[] componentsInChildren = this.m_pattern2ListArea.GetComponentsInChildren<ui_ranking_scroll>();
					if (componentsInChildren != null && componentsInChildren.Length > 0)
					{
						ui_ranking_scroll[] array = componentsInChildren;
						for (int i = 0; i < array.Length; i++)
						{
							ui_ranking_scroll ui_ranking_scroll = array[i];
							ui_ranking_scroll.UpdateSendChallenge(id);
						}
					}
				}
			}
			else if (this.m_pattern0 != null && this.m_pattern0.activeSelf && this.m_pattern0TopRankerArea != null)
			{
				ui_ranking_scroll[] componentsInChildren2 = this.m_pattern0TopRankerArea.GetComponentsInChildren<ui_ranking_scroll>();
				if (componentsInChildren2 != null && componentsInChildren2.Length > 0)
				{
					ui_ranking_scroll[] array2 = componentsInChildren2;
					for (int j = 0; j < array2.Length; j++)
					{
						ui_ranking_scroll ui_ranking_scroll2 = array2[j];
						ui_ranking_scroll2.UpdateSendChallenge(id);
					}
				}
			}
			else if (this.m_pattern1 != null && this.m_pattern1.activeSelf && this.m_pattern1ListArea != null)
			{
				ui_ranking_scroll[] componentsInChildren3 = this.m_pattern1ListArea.GetComponentsInChildren<ui_ranking_scroll>();
				if (componentsInChildren3 != null && componentsInChildren3.Length > 0)
				{
					ui_ranking_scroll[] array3 = componentsInChildren3;
					for (int k = 0; k < array3.Length; k++)
					{
						ui_ranking_scroll ui_ranking_scroll3 = array3[k];
						ui_ranking_scroll3.UpdateSendChallenge(id);
					}
				}
			}
		}
	}

	private bool IsRankingActive()
	{
		return this.m_displayFlag;
	}

	public static void UpdateSendChallenge(RankingUtil.RankingRankerType type, string id)
	{
		if (RankingUI.s_instance != null)
		{
			RankingUI.s_instance.UpdateSendChallengeOrg(type, id);
		}
	}

	public static void SetLoading()
	{
		if (RankingUI.s_instance != null)
		{
			RankingUI.s_instance.SetLoadingObject();
		}
	}

	private void Awake()
	{
		this.SetInstance();
	}

	private void OnDestroy()
	{
		if (RankingUI.s_instance == this)
		{
			RankingUI.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (RankingUI.s_instance == null)
		{
			RankingUI.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
