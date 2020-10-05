using AnimationOrTween;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class HudContinueWindow : MonoBehaviour
{
	private enum PressedButton
	{
		NO_PRESSING,
		YES,
		NO,
		VIDEO
	}

	private enum State
	{
		IDLE,
		START,
		WAIT_TOUCH_BUTTON,
		TOUCHED_BUTTON
	}

	private const float WAIT_TIME = 0.5f;

	private bool m_debugInfo;

	private HudContinueWindow.PressedButton m_pressedButton;

	private HudContinueWindow.State m_state;

	private GameObject m_parentPanel;

	private GameObject m_timeUpObj;

	private bool m_videoEnabled;

	private bool m_bossStage;

	private float m_waitTime;

	private string m_scoreText;

	private string m_dailyBattleText;

	public bool IsYesButtonPressed
	{
		get
		{
			return this.m_state == HudContinueWindow.State.TOUCHED_BUTTON && this.m_pressedButton == HudContinueWindow.PressedButton.YES;
		}
		private set
		{
		}
	}

	public bool IsNoButtonPressed
	{
		get
		{
			return this.m_state == HudContinueWindow.State.TOUCHED_BUTTON && this.m_pressedButton == HudContinueWindow.PressedButton.NO;
		}
		private set
		{
		}
	}

	public bool IsVideoButtonPressed
	{
		get
		{
			return this.m_state == HudContinueWindow.State.TOUCHED_BUTTON && this.m_pressedButton == HudContinueWindow.PressedButton.VIDEO;
		}
		private set
		{
		}
	}

	public void Setup(bool bossStage)
	{
		this.m_bossStage = bossStage;
		this.m_parentPanel = base.gameObject;
		if (this.m_timeUpObj == null)
		{
			this.m_timeUpObj = GameObjectUtil.FindChildGameObject(base.gameObject, "timesup");
			this.SetTimeUpObj(false);
		}
		bool flag = false;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "HL-AdsEnabled");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "HL-AdsDisabled");
		if (gameObject2 != null && gameObject != null)
		{
			if (flag)
			{
				this.m_parentPanel = gameObject;
				gameObject.SetActive(true);
				gameObject2.SetActive(false);
			}
			else
			{
				this.m_parentPanel = gameObject2;
				gameObject2.SetActive(true);
				gameObject.SetActive(false);
			}
		}
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_parentPanel, "Btn_yes");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnClickYesButton";
		}
		UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_parentPanel, "Btn_no");
		if (uIButtonMessage2 != null)
		{
			uIButtonMessage2.target = base.gameObject;
			uIButtonMessage2.functionName = "OnClickNoButton";
		}
		if (flag)
		{
			UIButtonMessage uIButtonMessage3 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_parentPanel, "Btn_video");
			if (uIButtonMessage3 != null)
			{
				uIButtonMessage3.target = base.gameObject;
				uIButtonMessage3.functionName = "OnClickVideoButton";
			}
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbt_caption");
		if (uILabel != null)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Continue", "rsring_continue_caption").text;
			uILabel.text = text;
		}
	}

	public void SetTimeUpObj(bool enablFlag)
	{
		if (this.m_timeUpObj != null)
		{
			this.m_timeUpObj.SetActive(enablFlag);
		}
	}

	public void SetVideoButton(bool enablFlag)
	{
		this.m_videoEnabled = enablFlag;
		this.Setup(this.m_bossStage);
	}

	private void UpdateRedStarRingCount()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "textView");
		if (gameObject == null)
		{
			return;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_body");
		if (uILabel == null)
		{
			return;
		}
		TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Continue", "label_rsring_continue");
		text.ReplaceTag("{RING_NUM}", HudContinueUtility.GetContinueCostString());
		text.ReplaceTag("{OWNED_RSRING}", HudContinueUtility.GetRedStarRingCountString());
		if (string.IsNullOrEmpty(this.m_scoreText))
		{
			this.m_scoreText = this.GetScoreText();
		}
		if (string.IsNullOrEmpty(this.m_dailyBattleText))
		{
			this.m_dailyBattleText = this.GetDailyBattleText();
		}
		uILabel.text = this.m_scoreText + this.m_dailyBattleText + text.text;
	}

	private string GetScoreText()
	{
		if (this.m_bossStage)
		{
			LevelInformation levelInformation = ObjUtil.GetLevelInformation();
			if (!(levelInformation != null))
			{
				return string.Empty;
			}
			int num = levelInformation.NumBossHpMax - levelInformation.NumBossAttack;
			int num2 = GameModeStage.ContinueRestCount();
			if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
			{
				TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Continue", "label_rsring_continue_RAID");
				text.ReplaceTag("{PARAM_LIFE}", num.ToString());
				text.ReplaceTag("{PARAM}", num2.ToString());
				return text.text;
			}
			TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Continue", "label_rsring_continue_E");
			text2.ReplaceTag("{PARAM_LIFE}", num.ToString());
			text2.ReplaceTag("{PARAM}", num2.ToString());
			return text2.text;
		}
		else
		{
			bool flag = false;
			if (EventManager.Instance != null && EventManager.Instance.IsSpecialStage())
			{
				flag = true;
			}
			ObjUtil.SendMessageFinalScore();
			StageScoreManager instance = StageScoreManager.Instance;
			if (instance == null)
			{
				return string.Empty;
			}
			RankingManager instance2 = SingletonGameObject<RankingManager>.Instance;
			if (instance2 == null)
			{
				return string.Empty;
			}
			long num3;
			if (!flag)
			{
				num3 = instance.FinalScore;
			}
			else
			{
				num3 = instance.FinalCountData.sp_crystal;
			}
			RankingUtil.RankingMode rankingMode = RankingUtil.RankingMode.ENDLESS;
			if (StageModeManager.Instance != null && StageModeManager.Instance.IsQuickMode())
			{
				rankingMode = RankingUtil.RankingMode.QUICK;
			}
			long num4 = 0L;
			long num5 = 0L;
			int num6 = 0;
			RankingManager.GetCurrentRankingStatus(rankingMode, flag, out num4, out num5, out num6);
			long score = num3;
			bool flag2 = false;
			long num7 = 0L;
			long num8 = 0L;
			int rank = 0;
			RankingUtil.RankingScoreType currentRankingScoreType = RankingManager.GetCurrentRankingScoreType(rankingMode, flag);
			int currentHighScoreRank = RankingManager.GetCurrentHighScoreRank(rankingMode, flag, ref score, out flag2, out num7, out num8, out rank);
			if (currentRankingScoreType == RankingUtil.RankingScoreType.TOTAL_SCORE)
			{
				flag2 = true;
			}
			LeagueType league = (LeagueType)num6;
			if (currentHighScoreRank == 1 && flag2 && num7 == 0L && num8 == 0L)
			{
				if (!flag)
				{
					return this.GetTextObject("label_rsring_continue_F", league, 0, 0, 0L, currentRankingScoreType);
				}
				return this.GetSpStageTextObject("label_rsring_continue_SP_A", 1, 0, score, 0L, currentRankingScoreType);
			}
			else if (!flag2)
			{
				if (!flag)
				{
					long score2 = num5 - num3 + 1L;
					return this.GetTextObject("label_rsring_continue_A", league, currentHighScoreRank, 0, score2, currentRankingScoreType);
				}
				return this.GetSpStageTextObject("label_rsring_continue_SP_A", currentHighScoreRank, 0, score, 0L, currentRankingScoreType);
			}
			else if (currentHighScoreRank == 1 && num7 == 0L && num8 == 0L)
			{
				if (!flag)
				{
					return this.GetTextObject("label_rsring_continue_F", league, 0, 0, 0L, currentRankingScoreType);
				}
				return this.GetSpStageTextObject("label_rsring_continue_SP_A", 1, 0, score, 0L, currentRankingScoreType);
			}
			else if (currentHighScoreRank == 1 && num7 == 0L && num8 >= 0L)
			{
				if (!flag)
				{
					return this.GetTextObject("label_rsring_continue_C", league, 0, 0, num8, currentRankingScoreType);
				}
				return this.GetSpStageTextObject("label_rsring_continue_SP_C", 0, 0, score, num8, currentRankingScoreType);
			}
			else
			{
				if (currentHighScoreRank <= 1)
				{
					return string.Empty;
				}
				if (!flag)
				{
					return this.GetTextObject("label_rsring_continue_B", league, currentHighScoreRank, rank, num7, currentRankingScoreType);
				}
				return this.GetSpStageTextObject("label_rsring_continue_SP_B", currentHighScoreRank, rank, score, num7, currentRankingScoreType);
			}
		}
	}

	private string GetTextObject(string cellName, LeagueType league, int rank1, int rank2, long score, RankingUtil.RankingScoreType scoreType)
	{
		string replaceString = string.Empty;
		if (scoreType == RankingUtil.RankingScoreType.HIGH_SCORE)
		{
			replaceString = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_high_score").text;
		}
		else
		{
			replaceString = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_total_score").text;
		}
		TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Continue", cellName);
		text.ReplaceTag("{PARAM_LEAGUE}", RankingUtil.GetLeagueName(league));
		text.ReplaceTag("{PARAM_RANK}", rank1.ToString());
		text.ReplaceTag("{PARAM_RANK2}", rank2.ToString());
		text.ReplaceTag("{PARAM_SCORE}", HudUtility.GetFormatNumString<long>(score));
		text.ReplaceTag("{SCORE}", replaceString);
		return text.text;
	}

	private string GetSpStageTextObject(string cellName, int rank1, int rank2, long score, long score2, RankingUtil.RankingScoreType scoreType)
	{
		string replaceString = string.Empty;
		if (scoreType == RankingUtil.RankingScoreType.HIGH_SCORE)
		{
			replaceString = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_high_score").text;
		}
		else
		{
			replaceString = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_total_score").text;
		}
		TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Continue", cellName);
		text.ReplaceTag("{PARAM_STAGE}", HudUtility.GetEventStageName());
		text.ReplaceTag("{PARAM_OBJECT}", HudUtility.GetEventSpObjectName());
		text.ReplaceTag("{PARAM_RANK}", HudUtility.GetFormatNumString<int>(rank1));
		text.ReplaceTag("{PARAM_RANK2}", HudUtility.GetFormatNumString<int>(rank2));
		text.ReplaceTag("{PARAM_SCORE}", HudUtility.GetFormatNumString<long>(score));
		text.ReplaceTag("{PARAM_SCORE2}", HudUtility.GetFormatNumString<long>(score2));
		text.ReplaceTag("{SCORE}", replaceString);
		return text.text;
	}

	private bool IsRankerNone(ServerLeaderboardEntries serverLeaderboardEntries)
	{
		return serverLeaderboardEntries.m_leaderboardEntries == null || serverLeaderboardEntries.m_leaderboardEntries.Count <= 0;
	}

	private bool IsRankerAlone(ServerLeaderboardEntries serverLeaderboardEntries, ServerLeaderboardEntry myServerLeaderboardEntry)
	{
		return serverLeaderboardEntries.m_leaderboardEntries.Count == 1 && serverLeaderboardEntries.m_leaderboardEntries[0].m_hspId == myServerLeaderboardEntry.m_hspId;
	}

	private ServerLeaderboardEntry GetOtherRanker(ServerLeaderboardEntries serverLeaderboardEntries, ServerLeaderboardEntry myServerLeaderboardEntry)
	{
		int num = serverLeaderboardEntries.m_leaderboardEntries.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			if (myServerLeaderboardEntry.m_hspId != serverLeaderboardEntries.m_leaderboardEntries[i].m_hspId)
			{
				return serverLeaderboardEntries.m_leaderboardEntries[i];
			}
		}
		return null;
	}

	private ServerLeaderboardEntry GetRankUpPlayerData(ServerLeaderboardEntries serverLeaderboardEntries, ServerLeaderboardEntry myServerLeaderboardEntry, long playScore)
	{
		List<ServerLeaderboardEntry> list = new List<ServerLeaderboardEntry>();
		foreach (ServerLeaderboardEntry current in serverLeaderboardEntries.m_leaderboardEntries)
		{
			if (current.m_score > playScore && current.m_hspId != myServerLeaderboardEntry.m_hspId)
			{
				list.Add(current);
				this.DebugInfo(string.Concat(new object[]
				{
					"GetRankUpPlayerData LIST rank=",
					current.m_grade,
					" score=",
					current.m_score
				}));
			}
		}
		if (list.Count > 0)
		{
			list.Sort(new Comparison<ServerLeaderboardEntry>(HudContinueWindow.RankComparer));
			return list[0];
		}
		return null;
	}

	private static int RankComparer(ServerLeaderboardEntry itemA, ServerLeaderboardEntry itemB)
	{
		if (itemA != null && itemB != null)
		{
			if (itemA.m_grade > itemB.m_grade)
			{
				return -1;
			}
			if (itemA.m_grade < itemB.m_grade)
			{
				return 1;
			}
		}
		return 0;
	}

	private string GetDailyBattleText()
	{
		string result = string.Empty;
		long num = 0L;
		if (StageModeManager.Instance != null && !StageModeManager.Instance.IsQuickMode())
		{
			return string.Empty;
		}
		DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
		if (instance != null && !this.m_bossStage)
		{
			string cellName = string.Empty;
			long num2 = 0L;
			long num3 = 0L;
			long num4 = 0L;
			if (instance.currentWinFlag > 0)
			{
				ServerDailyBattleDataPair currentDataPair = instance.currentDataPair;
				if (currentDataPair != null)
				{
					bool flag = true;
					StageScoreManager instance2 = StageScoreManager.Instance;
					if (instance2 != null)
					{
						num4 = (num = instance2.FinalScore);
						if (currentDataPair.myBattleData != null && !string.IsNullOrEmpty(currentDataPair.myBattleData.userId) && currentDataPair.myBattleData.maxScore > num)
						{
							num = currentDataPair.myBattleData.maxScore;
							flag = false;
						}
					}
					if (currentDataPair.rivalBattleData == null || string.IsNullOrEmpty(currentDataPair.rivalBattleData.userId))
					{
						if (flag)
						{
							cellName = "label_rsring_continue_DB_C";
						}
						else
						{
							num3 = num - num4;
							cellName = "label_rsring_continue_DB_F";
						}
					}
					else
					{
						long maxScore = currentDataPair.rivalBattleData.maxScore;
						if (maxScore > num)
						{
							num2 = maxScore - num;
							if (flag)
							{
								cellName = "label_rsring_continue_DB_B";
							}
							else
							{
								num3 = num - num4;
								cellName = "label_rsring_continue_DB_E";
							}
						}
						else
						{
							num2 = num - maxScore;
							if (flag)
							{
								cellName = "label_rsring_continue_DB_A";
							}
							else
							{
								num3 = num - num4;
								cellName = "label_rsring_continue_DB_D";
							}
						}
					}
				}
			}
			else
			{
				cellName = "label_rsring_continue_DB_C";
			}
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Continue", cellName);
			if (text != null)
			{
				text.ReplaceTag("{PARAM_SCORE}", HudUtility.GetFormatNumString<long>(num2));
				text.ReplaceTag("{PARAM_HIGHSCORE}", HudUtility.GetFormatNumString<long>(num3));
				result = text.text;
			}
		}
		return result;
	}

	public void PlayStart()
	{
		this.m_pressedButton = HudContinueWindow.PressedButton.NO_PRESSING;
		this.m_state = HudContinueWindow.State.START;
		this.m_waitTime = 0f;
		base.gameObject.SetActive(true);
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation.Play(component, Direction.Forward);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_state == HudContinueWindow.State.IDLE)
		{
			return;
		}
		if (this.m_state == HudContinueWindow.State.START)
		{
			this.m_waitTime += RealTime.deltaTime;
			if (this.m_waitTime > 0.5f)
			{
				this.m_state = HudContinueWindow.State.WAIT_TOUCH_BUTTON;
				this.m_waitTime = 0f;
			}
		}
		if (this.m_parentPanel == null)
		{
			this.m_parentPanel = base.gameObject;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_parentPanel, "Btn_yes");
		if (gameObject != null)
		{
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_rs_cost");
			if (uILabel != null)
			{
				uILabel.text = HudContinueUtility.GetContinueCostString();
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "img_sale_icon");
			if (gameObject2 != null)
			{
				bool active = HudContinueUtility.IsInContinueCostCampaign();
				gameObject2.SetActive(active);
			}
		}
		this.UpdateRedStarRingCount();
	}

	private void OnClickYesButton()
	{
		if (this.m_state != HudContinueWindow.State.WAIT_TOUCH_BUTTON)
		{
			return;
		}
		this.m_pressedButton = HudContinueWindow.PressedButton.YES;
		SoundManager.SePlay("sys_menu_decide", "SE");
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(GetComponent<Animation>(), Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallbck), true);
			}
		}
	}

	private void OnClickNoButton()
	{
		if (this.m_state != HudContinueWindow.State.WAIT_TOUCH_BUTTON)
		{
			return;
		}
		this.m_pressedButton = HudContinueWindow.PressedButton.NO;
		SoundManager.SePlay("sys_window_close", "SE");
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(GetComponent<Animation>(), Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallbck), true);
			}
		}
	}

	private void OnClickVideoButton()
	{
		if (this.m_state != HudContinueWindow.State.WAIT_TOUCH_BUTTON)
		{
			return;
		}
		this.m_pressedButton = HudContinueWindow.PressedButton.VIDEO;
		SoundManager.SePlay("sys_menu_decide", "SE");
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(GetComponent<Animation>(), Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallbck), true);
			}
		}
	}

	public void OnPushBackKey()
	{
		this.OnClickNoButton();
	}

	private void OnFinishedAnimationCallbck()
	{
		base.gameObject.SetActive(false);
		this.m_scoreText = string.Empty;
		this.m_dailyBattleText = string.Empty;
		this.m_state = HudContinueWindow.State.TOUCHED_BUTTON;
	}

	private void DebugInfo(string msg)
	{
	}
}
