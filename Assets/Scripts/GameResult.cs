using AnimationOrTween;
using App.Utility;
using SaveData;
using System;
using Text;
using UnityEngine;

public class GameResult : MonoBehaviour
{
	public enum ResultType
	{
		NONE = -1,
		NORMAL,
		BOSS
	}

	private enum EventSignal
	{
		SIG_BG_START_IN_ANIM = 100,
		SIG_END_BG_IN_ANIM,
		SIG_END_SCORE_IN_ANIM,
		SIG_NEXT_BUTTON_PRESSED,
		SIG_DETAILS_BUTTON_PRESSED,
		SIG_DETAILS_END_BUTTON_PRESSED,
		SIG_END_SCORE_OUT_ANIM,
		SIG_END_OUT_ANIM
	}

	private enum Status
	{
		PRESS_NEXT,
		END_OUT_ANIM
	}

	private TinyFsmBehavior m_fsm;

	private GameObject m_AnimationObject;

	private GameObject m_resultRoot;

	private GameObject m_eventRoot;

	private UIImageButton m_imageButtonNext;

	private UIImageButton m_imageButtonDetails;

	private GameResultScores m_scores;

	private bool m_isNomiss;

	private bool m_isBossTutorialClear;

	private bool m_isReplay;

	private bool m_isBossDestroyed;

	private GameResult.ResultType m_resultType;

	private bool m_isScoreStart;

	private bool m_eventTimeup;

	private EventResultState m_eventResultState;

	private HudEventResult m_eventResult;

	private Bitset32 m_status;

	public bool IsPressNext
	{
		get
		{
			return this.m_status.Test(0);
		}
		private set
		{
			this.m_status.Set(0, value);
		}
	}

	public bool IsEndOutAnimation
	{
		get
		{
			return this.m_status.Test(1);
		}
		private set
		{
			this.m_status.Set(1, value);
		}
	}

	public void PlayBGStart(GameResult.ResultType resultType, bool isNoMiss, bool isBossTutorialClear, bool isBossDestroy, EventResultState eventResultState)
	{
		this.m_isBossDestroyed = isBossDestroy;
		this.SetupResultType(resultType, isNoMiss, isBossTutorialClear, this.m_isBossDestroyed);
		this.m_resultType = resultType;
		this.m_eventResultState = eventResultState;
		this.IsPressNext = false;
		this.IsEndOutAnimation = false;
		GameResultUtility.SetRaidbossBeatBonus(0);
		global::Debug.Log("GameResult:PlayBGStart >>> eventResultState=" + eventResultState.ToString());
		this.m_eventTimeup = false;
		EventResultState eventResultState2 = this.m_eventResultState;
		if (eventResultState2 != EventResultState.TIMEUP)
		{
			if (eventResultState2 != EventResultState.TIMEUP_RESULT)
			{
				this.m_eventTimeup = false;
			}
			else
			{
				this.m_eventTimeup = true;
			}
		}
		else
		{
			this.m_eventTimeup = true;
		}
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	public void PlayScoreStart()
	{
		this.m_isScoreStart = true;
	}

	public void SetRaidbossBeatBonus(int value)
	{
		GameResultUtility.SetRaidbossBeatBonus(value);
	}

	private void Start()
	{
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm == null)
		{
			return;
		}
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
		GameObject gameObject = base.transform.Find("result_Anim").gameObject;
		GameObject gameObject2 = base.transform.Find("result_boss_Anim").gameObject;
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
		}
		GameResultUtility.SaveOldBestScore();
		this.m_isNomiss = false;
		this.m_isBossTutorialClear = false;
	}

	private void Update()
	{
	}

	private void SetupHudResultWindow(bool isCampaignBonus)
	{
		string windowName = "HudResultWindow";
		string windowName2 = "HudResultWindow2";
		if (isCampaignBonus)
		{
			windowName = "HudResultWindow2";
			windowName2 = "HudResultWindow";
		}
		HudResultWindow hudResultWindow = this.GetHudResultWindow(windowName);
		if (hudResultWindow != null)
		{
			hudResultWindow.Setup(base.gameObject, this.m_resultType == GameResult.ResultType.BOSS);
			hudResultWindow.gameObject.SetActive(false);
		}
		HudResultWindow hudResultWindow2 = this.GetHudResultWindow(windowName2);
		if (hudResultWindow2 != null)
		{
			hudResultWindow2.gameObject.SetActive(false);
		}
	}

	private HudResultWindow GetHudResultWindow(string windowName)
	{
		GameObject gameObject = base.transform.Find(windowName).gameObject;
		if (gameObject != null)
		{
			return gameObject.GetComponent<HudResultWindow>();
		}
		return null;
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
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInAnimation)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateInAnimation(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_AnimationObject.SetActive(true);
			bool isCampaignBonus = false;
			if (this.m_scores != null)
			{
				this.m_scores.Setup(base.gameObject, this.m_resultRoot, this.m_eventRoot);
				isCampaignBonus = this.m_scores.IsCampaignBonus();
			}
			this.SetupHudResultWindow(isCampaignBonus);
			if (this.m_isReplay)
			{
				return TinyFsmState.End();
			}
			Animation anim = GameResultUtility.SearchAnimation(this.m_AnimationObject);
			string clipName = string.Empty;
			GameResult.ResultType resultType = this.m_resultType;
			if (resultType != GameResult.ResultType.NORMAL)
			{
				if (resultType == GameResult.ResultType.BOSS)
				{
					clipName = "ui_result_boss_intro_bg_Anim";
				}
			}
			else
			{
				clipName = "ui_result_intro_bg_Anim";
			}
			ActiveAnimation activeAnimation = ActiveAnimation.Play(anim, clipName, Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.InAnimationEndCallback), true);
			BoxCollider boxCollider = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this.m_resultRoot, "Btn_next");
			if (boxCollider != null)
			{
				boxCollider.isTrigger = false;
			}
			BoxCollider boxCollider2 = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this.m_resultRoot, "Btn_details");
			if (boxCollider2 != null)
			{
				boxCollider2.isTrigger = false;
			}
			SoundManager.SePlay("sys_window_open", "SE");
			return TinyFsmState.End();
		}
		case 2:
		case 3:
			IL_25:
			if (signal != 101)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateScoreInWait)));
			return TinyFsmState.End();
		case 4:
			if (this.m_isReplay)
			{
				this.OnSetEnableDetailsButton(true);
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateScoreInAnimation)));
			}
			return TinyFsmState.End();
		}
		goto IL_25;
	}

	private TinyFsmState StateScoreInWait(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (this.m_isScoreStart)
			{
				bool flag = false;
				EventManager instance = EventManager.Instance;
				if (instance != null)
				{
					flag = (instance.EventStage || instance.IsCollectEvent());
				}
				if (this.m_resultType == GameResult.ResultType.BOSS)
				{
					flag = EventManager.Instance.IsRaidBossStage();
				}
				if (flag)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEventScoreDisplaying)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateScoreInAnimation)));
				}
				if (this.m_isReplay)
				{
					this.OnSetEnableDetailsButton(true);
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEventScoreDisplaying(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (EventManager.Instance.IsSpecialStage())
			{
				this.m_eventResult = GameObjectUtil.FindChildGameObjectComponent<HudEventResult>(base.gameObject, "EventResult_spstage");
			}
			else if (EventManager.Instance.IsRaidBossStage())
			{
				this.m_eventResult = GameObjectUtil.FindChildGameObjectComponent<HudEventResult>(base.gameObject, "EventResult_raidboss");
			}
			else
			{
				this.m_eventResult = GameObjectUtil.FindChildGameObjectComponent<HudEventResult>(base.gameObject, "EventResult_animal");
			}
			if (this.m_eventResult != null)
			{
				this.m_eventResult.Setup(this.m_eventTimeup);
				this.m_eventResult.PlayStart();
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 103)
			{
				return TinyFsmState.End();
			}
			if (this.m_eventResult != null)
			{
				this.m_eventResult.PlayOutAnimation();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_eventResult != null)
			{
				if (this.m_eventResult.IsEndOutAnim)
				{
					bool flag = false;
					EventManager instance = EventManager.Instance;
					if (instance != null)
					{
						if (instance.Type == EventManager.EventType.COLLECT_OBJECT)
						{
							flag = true;
						}
						EventResultState eventResultState = this.m_eventResultState;
						if (eventResultState != EventResultState.TIMEUP)
						{
							if (eventResultState != EventResultState.TIMEUP_RESULT)
							{
							}
							flag = true;
						}
						if (instance.IsRaidBossStage())
						{
							flag = false;
						}
					}
					if (!this.m_eventTimeup || flag)
					{
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateScoreInAnimation)));
					}
					else
					{
						if (this.m_scores != null)
						{
							this.m_scores.AllSkip();
						}
						this.IsPressNext = true;
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateOutScoreAnimation)));
					}
				}
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateScoreInAnimation)));
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateScoreInAnimation(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_scores != null)
			{
				this.m_scores.PlayScoreInAnimation(new EventDelegate.Callback(this.ScoreInAnimationEndCallback));
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 102)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateScoreChanging)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateScoreChanging(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_scores != null)
			{
				this.m_scores.PlayStart();
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 103)
			{
				return TinyFsmState.End();
			}
			if (this.m_scores != null)
			{
				this.m_scores.AllSkip();
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_scores != null && this.m_scores.IsEnd)
			{
				bool flag = false;
				if (!this.m_isReplay)
				{
					if (this.m_isBossTutorialClear)
					{
						flag = true;
					}
					else if (this.m_isNomiss && RouletteManager.Instance != null && RouletteManager.Instance.specialEgg >= 10)
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSpEggMessage)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitButtonPressed)));
				}
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateSpEggMessage(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
			info.name = "SpEggMessage";
			info.buttonType = GeneralWindow.ButtonType.Ok;
			if (this.m_isBossTutorialClear)
			{
				info.caption = TextUtility.GetCommonText("ChaoRoulette", "sp_egg_get_caption");
				info.message = TextUtility.GetCommonText("ChaoRoulette", "sp_egg_get_text");
			}
			else
			{
				info.caption = TextUtility.GetCommonText("ChaoRoulette", "sp_egg_max_caption");
				info.message = TextUtility.GetCommonText("ChaoRoulette", "sp_egg_max_text");
			}
			GeneralWindow.Create(info);
			return TinyFsmState.End();
		}
		case 4:
			if (GeneralWindow.IsCreated("SpEggMessage") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitButtonPressed)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitButtonPressed(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.OnSetEnableNextButton(true);
			this.OnSetEnableDetailsButton(true);
			if (!this.m_scores.IsBonusEvent())
			{
				this.OnSetEnableDetailsButton(false);
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			switch (signal)
			{
			case 103:
				if (this.m_scores != null)
				{
					this.m_scores.AllSkip();
				}
				this.IsPressNext = true;
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateOutScoreAnimation)));
				return TinyFsmState.End();
			case 104:
				this.OnSetEnableNextButton(false);
				this.OnSetEnableDetailsButton(false);
				this.m_isReplay = true;
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInAnimation)));
				return TinyFsmState.End();
			case 105:
				this.OnSetEnableNextButton(true);
				this.OnSetEnableDetailsButton(true);
				return TinyFsmState.End();
			default:
				return TinyFsmState.End();
			}
			break;
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateOutScoreAnimation(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_scores != null)
			{
				this.m_scores.PlayScoreOutAnimation(new EventDelegate.Callback(this.ScoreOutAnimationCallback));
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 106)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateShowCharacterGlowUp)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateShowCharacterGlowUp(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			GlowUpWindow glowUpWindow = GameObjectUtil.FindChildGameObjectComponent<GlowUpWindow>(base.gameObject, "ResultPlayerExpWindow");
			if (glowUpWindow != null)
			{
				GlowUpWindow.ExpType expType;
				if (this.m_resultType == GameResult.ResultType.BOSS)
				{
					if (this.m_isBossDestroyed)
					{
						expType = GlowUpWindow.ExpType.BOSS_SUCCESS;
					}
					else
					{
						expType = GlowUpWindow.ExpType.BOSS_FAILED;
					}
				}
				else
				{
					expType = GlowUpWindow.ExpType.RUN_STAGE;
				}
				glowUpWindow.PlayStart(expType);
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			bool flag = false;
			GlowUpWindow glowUpWindow2 = GameObjectUtil.FindChildGameObjectComponent<GlowUpWindow>(base.gameObject, "ResultPlayerExpWindow");
			if (glowUpWindow2 != null)
			{
				if (glowUpWindow2.IsPlayEnd)
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
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateOutAnimation)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateOutAnimation(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_scores != null)
			{
				this.m_scores.OnFinishScore();
				this.m_scores.PlayScoreOutAnimation(new EventDelegate.Callback(this.OutAnimationEndCallback));
			}
			this.OnSetEnableDetailsButton(false);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 107)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFinished)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateFinished(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_resultRoot != null)
			{
				this.m_resultRoot.SetActive(true);
			}
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void OnClickNextButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(103);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	private void OnClickDetailsButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(104);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
		if (this.m_scores != null)
		{
			if (this.m_isReplay)
			{
				this.m_scores.PlaySkip();
			}
			else
			{
				this.m_scores.AllSkip();
			}
		}
		if (this.m_scores != null)
		{
			this.m_scores.SetActiveDetailsButton(true);
		}
	}

	private void OnClickSkipButton()
	{
		if (this.m_scores != null)
		{
			this.m_scores.PlaySkip();
		}
	}

	private void OnClickDetailsEndButton()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(105);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
		if (this.m_scores != null)
		{
			this.m_scores.SetActiveDetailsButton(false);
		}
	}

	private void SetupResultType(GameResult.ResultType resultType, bool isNoMiss, bool isBossTutorialClear, bool isRaidBossDestroy)
	{
		GameObject gameObject = base.transform.Find("result_Anim").gameObject;
		GameObject gameObject2 = base.transform.Find("result_boss_Anim").gameObject;
		GameObject gameObject3 = base.transform.Find("result_word_Anim").gameObject;
		GameResultUtility.SetBossDestroyFlag(isRaidBossDestroy);
		if (resultType != GameResult.ResultType.NORMAL)
		{
			if (resultType == GameResult.ResultType.BOSS)
			{
				if (EventManager.Instance.IsRaidBossStage())
				{
					GameResultScoresRaidBoss gameResultScoresRaidBoss = base.gameObject.AddComponent<GameResultScoresRaidBoss>();
					gameResultScoresRaidBoss.SetBossDestroyFlag(isRaidBossDestroy);
					this.m_isNomiss = false;
					this.m_isBossTutorialClear = false;
					this.m_scores = gameResultScoresRaidBoss;
					this.m_resultRoot = gameObject2;
				}
				else
				{
					GameResultScoresBoss gameResultScoresBoss = base.gameObject.AddComponent<GameResultScoresBoss>();
					gameResultScoresBoss.SetNoMissFlag(isNoMiss);
					this.m_isNomiss = isNoMiss;
					this.m_isBossTutorialClear = isBossTutorialClear;
					this.m_scores = gameResultScoresBoss;
					this.m_resultRoot = gameObject2;
				}
				this.m_eventRoot = gameObject3;
			}
		}
		else
		{
			this.m_isNomiss = false;
			this.m_isBossTutorialClear = false;
			this.m_scores = base.gameObject.AddComponent<GameResultScoresNormal>();
			this.m_resultRoot = gameObject;
			this.m_eventRoot = gameObject3;
		}
		BackKeyManager.AddWindowCallBack(base.gameObject);
		this.m_AnimationObject = this.m_resultRoot;
		Animation animation = GameResultUtility.SearchAnimation(this.m_AnimationObject);
		if (animation != null)
		{
			animation.Stop();
		}
		this.m_imageButtonNext = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_resultRoot, "Btn_next");
		if (this.m_imageButtonNext != null)
		{
			this.m_imageButtonNext.isEnabled = false;
		}
		this.m_imageButtonDetails = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_resultRoot, "Btn_details");
		if (this.m_imageButtonDetails != null)
		{
			this.m_imageButtonDetails.isEnabled = false;
			UIButtonMessage uIButtonMessage = this.m_imageButtonDetails.gameObject.GetComponent<UIButtonMessage>();
			if (uIButtonMessage == null)
			{
				uIButtonMessage = this.m_imageButtonDetails.gameObject.AddComponent<UIButtonMessage>();
			}
			if (uIButtonMessage != null)
			{
				uIButtonMessage.enabled = true;
				uIButtonMessage.trigger = UIButtonMessage.Trigger.OnClick;
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickDetailsButton";
			}
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			instance.AddPlayCount();
		}
	}

	private void InAnimationEndCallback()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(101);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ScoreInAnimationEndCallback()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(102);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	private void ScoreOutAnimationCallback()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(106);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	private void OutAnimationEndCallback()
	{
		this.IsEndOutAnimation = true;
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(107);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	private void OnSetEnableNextButton(bool enabled)
	{
		if (this.m_imageButtonNext != null)
		{
			this.m_imageButtonNext.isEnabled = enabled;
		}
	}

	private void OnSetEnableDetailsButton(bool enabled)
	{
		if (this.m_imageButtonDetails != null)
		{
			this.m_imageButtonDetails.isEnabled = enabled;
		}
	}

	public void OnClickPlatformBackButton()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (this.m_eventResult != null && !this.m_eventResult.IsBackkeyEnable())
		{
			return;
		}
		if (this.m_imageButtonNext != null && this.m_imageButtonNext.isEnabled)
		{
			this.OnClickNextButton();
		}
	}
}
