using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class DailyBattleDetailWindow : WindowBase
{
	private sealed class _SetupObject_c__Iterator2F : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal UIPlayAnimation _btnClose___0;

		internal GameObject _root___1;

		internal GameObject _body___2;

		internal GameObject _vsObj___3;

		internal bool _flg___4;

		internal UIRectItemStorage _storage___5;

		internal RankingUtil.Ranker _ranker___6;

		internal UIRectItemStorage _storage___7;

		internal RankingUtil.Ranker _ranker___8;

		internal UILabel _caption___9;

		internal int _PC;

		internal object _current;

		internal DailyBattleDetailWindow __f__this;

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
				if (this.__f__this.m_mainPanel != null)
				{
					this.__f__this.m_mainPanel.alpha = 1f;
				}
				if (this.__f__this.m_animation != null)
				{
					ActiveAnimation.Play(this.__f__this.m_animation, "ui_cmn_window_Anim2", Direction.Forward);
				}
				this._btnClose___0 = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(this.__f__this.gameObject, "Btn_close");
				if (this._btnClose___0 != null && !EventDelegate.IsValid(this._btnClose___0.onFinished))
				{
					EventDelegate.Add(this._btnClose___0.onFinished, new EventDelegate.Callback(this.__f__this.OnFinished), true);
				}
				this.__f__this.m_targetScore = 0L;
				this.__f__this.m_time = 0f;
				this.__f__this.m_winFlag = 0;
				if (this.__f__this.m_battleData != null)
				{
					this.__f__this.m_winFlag = this.__f__this.m_battleData.winFlag;
					if (this.__f__this.m_battleData != null)
					{
						if (this.__f__this.m_battleData.myBattleData != null)
						{
							this.__f__this.m_targetScore = this.__f__this.m_battleData.myBattleData.maxScore;
						}
						if (this.__f__this.m_battleData.rivalBattleData != null && this.__f__this.m_targetScore < this.__f__this.m_battleData.rivalBattleData.maxScore)
						{
							this.__f__this.m_targetScore = this.__f__this.m_battleData.rivalBattleData.maxScore;
						}
					}
				}
				this._root___1 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "window_contents");
				this.__f__this.m_mineData = null;
				this.__f__this.m_adversaryData = null;
				if (this._root___1 != null)
				{
					this._body___2 = GameObjectUtil.FindChildGameObject(this._root___1, "body");
					if (this._body___2 != null)
					{
						if (this.__f__this.m_battleData != null)
						{
							this._body___2.SetActive(true);
							this.__f__this.m_mineSet = GameObjectUtil.FindChildGameObject(this._body___2, "duel_mine_set");
							this.__f__this.m_adversarySet = GameObjectUtil.FindChildGameObject(this._body___2, "duel_adversary_set");
							this._vsObj___3 = GameObjectUtil.FindChildGameObject(this._body___2, "img_word_vs");
							if (this._vsObj___3 != null)
							{
								this._flg___4 = false;
								if (this.__f__this.m_battleData.myBattleData != null && !string.IsNullOrEmpty(this.__f__this.m_battleData.myBattleData.userId) && this.__f__this.m_battleData.rivalBattleData != null && !string.IsNullOrEmpty(this.__f__this.m_battleData.rivalBattleData.userId))
								{
									this._flg___4 = true;
								}
								this._vsObj___3.SetActive(this._flg___4);
							}
							if (this.__f__this.m_mineSet != null)
							{
								this._storage___5 = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.__f__this.m_mineSet, "score_mine");
								if (this._storage___5 != null)
								{
									if (this.__f__this.m_battleData.myBattleData != null && !string.IsNullOrEmpty(this.__f__this.m_battleData.myBattleData.userId))
									{
										this._storage___5.maxItemCount = (this._storage___5.maxRows = 1);
										this._storage___5.Restart();
										this.__f__this.m_mineData = this.__f__this.m_mineSet.GetComponentInChildren<ui_ranking_scroll>();
										if (this.__f__this.m_mineData != null)
										{
											this._ranker___6 = new RankingUtil.Ranker(this.__f__this.m_battleData.myBattleData);
											this._ranker___6.isBoxCollider = false;
											this.__f__this.m_mineData.UpdateView(RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.RIVAL, this._ranker___6, true);
											this.__f__this.m_mineData.UpdateViewScore(0L);
											this.__f__this.m_mineData.SetMyRanker(true);
										}
									}
									else
									{
										this._storage___5.maxItemCount = (this._storage___5.maxRows = 0);
										this._storage___5.Restart();
									}
								}
							}
							if (this.__f__this.m_adversarySet != null)
							{
								this._storage___7 = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.__f__this.m_adversarySet, "score_adversary");
								if (this._storage___7 != null)
								{
									if (this.__f__this.m_battleData.rivalBattleData != null && !string.IsNullOrEmpty(this.__f__this.m_battleData.rivalBattleData.userId))
									{
										this._storage___7.maxItemCount = (this._storage___7.maxRows = 1);
										this._storage___7.Restart();
										this.__f__this.m_adversaryData = this.__f__this.m_adversarySet.GetComponentInChildren<ui_ranking_scroll>();
										if (this.__f__this.m_adversaryData != null)
										{
											this._ranker___8 = new RankingUtil.Ranker(this.__f__this.m_battleData.rivalBattleData);
											this._ranker___8.isBoxCollider = false;
											if (this._ranker___8.isFriend)
											{
												this._ranker___8.isSentEnergy = true;
											}
											this.__f__this.m_adversaryData.UpdateView(RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.RIVAL, this._ranker___8, true);
											this.__f__this.m_adversaryData.UpdateViewScore(0L);
											this.__f__this.m_adversaryData.SetMyRanker(false);
										}
									}
									else
									{
										this._storage___7.maxItemCount = (this._storage___7.maxRows = 0);
										this._storage___7.Restart();
									}
								}
							}
							this.__f__this.m_result = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._body___2, "Lbl_result");
							this.__f__this.SetupUserData(0);
						}
						else
						{
							this._body___2.SetActive(false);
						}
					}
					this._caption___9 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._root___1, "Lbl_caption");
					if (this._caption___9 != null)
					{
						this._caption___9.text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_detail_caption");
					}
				}
				this.__f__this.m_isEnd = false;
				this.__f__this.m_isClickClose = false;
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

	private const float OPEN_EFFECT_START = 0.5f;

	private const float OPEN_EFFECT_TIME = 1.5f;

	private static bool s_isActive;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIPanel m_mainPanel;

	private bool m_isClickClose;

	private bool m_isEnd;

	private bool m_isOpenEffectStart;

	private bool m_isOpenEffectIssue;

	private bool m_isOpenEffectEnd;

	private float m_time;

	private long m_targetScore;

	private int m_winFlag;

	private GameObject m_mineSet;

	private GameObject m_adversarySet;

	private UILabel m_result;

	private ui_ranking_scroll m_mineData;

	private ui_ranking_scroll m_adversaryData;

	private ServerDailyBattleDataPair m_battleData;

	public static bool isActive
	{
		get
		{
			return DailyBattleDetailWindow.s_isActive;
		}
	}

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	private void Update()
	{
		if (base.gameObject.activeSelf && !this.m_isOpenEffectEnd)
		{
			if (!this.m_isOpenEffectStart && this.m_time >= 0.5f)
			{
				this.m_isOpenEffectStart = true;
				if (this.m_winFlag > 1)
				{
					SoundManager.SePlay("sys_rank_up", "SE");
				}
				else
				{
					SoundManager.SePlay("sys_league_down", "SE");
				}
			}
			else if (this.m_isOpenEffectStart)
			{
				float num = (this.m_time - 0.5f) / 1.5f;
				if (num < 0f)
				{
					num = 0f;
				}
				else if (num > 1f)
				{
					num = 1f;
				}
				long num2 = (long)((float)this.m_targetScore * num);
				if (this.m_mineData != null)
				{
					if (num2 < this.m_mineData.rankerScore)
					{
						this.m_mineData.UpdateViewScore(num2);
					}
					else
					{
						this.m_mineData.UpdateViewScore(-1L);
						if (!this.m_isOpenEffectIssue)
						{
							this.m_isOpenEffectIssue = true;
							this.SetupUserData(this.m_winFlag);
						}
					}
				}
				if (this.m_adversaryData != null)
				{
					if (num2 < this.m_adversaryData.rankerScore)
					{
						this.m_adversaryData.UpdateViewScore(num2);
					}
					else
					{
						this.m_adversaryData.UpdateViewScore(-1L);
						if (!this.m_isOpenEffectIssue)
						{
							this.m_isOpenEffectIssue = true;
							this.SetupUserData(this.m_winFlag);
						}
					}
				}
				if (num >= 1f)
				{
					this.m_isOpenEffectEnd = true;
					if (this.m_mineData != null)
					{
						this.m_mineData.UpdateViewScore(-1L);
					}
					if (this.m_adversaryData != null)
					{
						this.m_adversaryData.UpdateViewScore(-1L);
					}
					this.SetupUserData(this.m_winFlag);
				}
			}
			this.m_time += Time.deltaTime;
		}
	}

	public static bool Open(ServerDailyBattleDataPair data)
	{
		bool result = false;
		GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
		if (menuAnimUIObject != null)
		{
			DailyBattleDetailWindow dailyBattleDetailWindow = GameObjectUtil.FindChildGameObjectComponent<DailyBattleDetailWindow>(menuAnimUIObject, "DailyBattleDetailWindow");
			if (dailyBattleDetailWindow == null)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(menuAnimUIObject, "DailyBattleDetailWindow");
				if (gameObject != null)
				{
					dailyBattleDetailWindow = gameObject.AddComponent<DailyBattleDetailWindow>();
				}
			}
			if (dailyBattleDetailWindow != null)
			{
				dailyBattleDetailWindow.Setup(data);
				result = true;
			}
		}
		return result;
	}

	private IEnumerator SetupObject()
	{
		DailyBattleDetailWindow._SetupObject_c__Iterator2F _SetupObject_c__Iterator2F = new DailyBattleDetailWindow._SetupObject_c__Iterator2F();
		_SetupObject_c__Iterator2F.__f__this = this;
		return _SetupObject_c__Iterator2F;
	}

	private void Setup(ServerDailyBattleDataPair data)
	{
		DailyBattleDetailWindow.s_isActive = true;
		base.gameObject.SetActive(true);
		this.m_battleData = data;
		this.m_isEnd = false;
		this.m_isClickClose = false;
		base.enabled = true;
		this.m_isOpenEffectStart = false;
		this.m_isOpenEffectIssue = false;
		this.m_isOpenEffectEnd = false;
		base.StartCoroutine(this.SetupObject());
	}

	private void SetupUserData(int winOrLose = 0)
	{
		if (this.m_battleData != null)
		{
			if (this.m_mineSet != null)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_mineSet, "duel_win_set");
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_mineSet, "duel_lose_set");
				if (winOrLose <= 0 || this.m_battleData.myBattleData == null || string.IsNullOrEmpty(this.m_battleData.myBattleData.userId))
				{
					gameObject.SetActive(false);
					gameObject2.SetActive(false);
				}
				else if (winOrLose == 1)
				{
					gameObject.SetActive(false);
					gameObject2.SetActive(true);
				}
				else
				{
					gameObject.SetActive(true);
					gameObject2.SetActive(false);
				}
			}
			if (this.m_adversarySet != null)
			{
				GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_adversarySet, "duel_win_set");
				GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_adversarySet, "duel_lose_set");
				if (winOrLose <= 0 || this.m_battleData.rivalBattleData == null || string.IsNullOrEmpty(this.m_battleData.rivalBattleData.userId))
				{
					gameObject3.SetActive(false);
					gameObject4.SetActive(false);
				}
				else if (winOrLose == 1)
				{
					gameObject3.SetActive(true);
					gameObject4.SetActive(false);
				}
				else
				{
					gameObject3.SetActive(false);
					gameObject4.SetActive(true);
				}
			}
			if (this.m_result != null)
			{
				if (winOrLose <= 0)
				{
					this.m_result.text = string.Empty;
				}
				else if (winOrLose == 4 && !GeneralUtil.IsOverTime(this.m_battleData.endTime, 0))
				{
					this.m_result.text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_result_still");
				}
				else if (winOrLose == 1)
				{
					this.m_result.text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_result_lose");
				}
				else
				{
					this.m_result.text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_result_win");
				}
			}
			if (this.m_winFlag == 0)
			{
				this.m_isOpenEffectStart = true;
				this.m_isOpenEffectIssue = true;
				this.m_isOpenEffectEnd = true;
				if (this.m_mineData != null)
				{
					this.m_mineData.UpdateViewScore(-1L);
				}
				if (this.m_adversaryData != null)
				{
					this.m_adversaryData.UpdateViewScore(-1L);
				}
				if (this.m_result != null)
				{
					this.m_result.text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_result_failure");
				}
			}
		}
	}

	public void OnOpen()
	{
	}

	public void OnFinished()
	{
		DailyBattleDetailWindow.s_isActive = false;
		this.m_isEnd = true;
		if (this.m_mainPanel != null)
		{
			this.m_mainPanel.alpha = 0f;
		}
		base.gameObject.SetActive(false);
		base.enabled = false;
		SoundManager.SePlay("sys_window_close", "SE");
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_isEnd)
		{
			return;
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
		if (!ranking_window.isActive && !this.m_isClickClose)
		{
			this.m_isClickClose = true;
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim2", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
			}
		}
	}
}
