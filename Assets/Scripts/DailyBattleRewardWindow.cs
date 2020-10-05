using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class DailyBattleRewardWindow : WindowBase
{
	private sealed class _SetupObject_c__Iterator30 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _loseWindow___0;

		internal GameObject _winWindow___1;

		internal string _loseText___2;

		internal UILabel _loseLabel___3;

		internal UILabel _loseLabel_sh___4;

		internal int _winCount___5;

		internal TextObject _textObject___6;

		internal UILabel _winLabel___7;

		internal UILabel _winLabel_sh___8;

		internal GameObject _window___9;

		internal UIPlayAnimation _btnClose___10;

		internal UIPlayAnimation _blinder___11;

		internal int _PC;

		internal object _current;

		internal DailyBattleRewardWindow __f__this;

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
				if (this.__f__this.m_battleData != null)
				{
					this._loseWindow___0 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "lose");
					this._winWindow___1 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "win");
					if (this._winWindow___1 != null && this._loseWindow___0 != null)
					{
						switch (this.__f__this.m_battleData.winFlag)
						{
						case 0:
						case 1:
							this._winWindow___1.SetActive(false);
							this._loseWindow___0.SetActive(true);
							this._loseText___2 = TextUtility.GetCommonText("DailyMission", "battle_vsreward_text3");
							this._loseLabel___3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._loseWindow___0, "Lbl_daily_battle_lose");
							if (this._loseLabel___3 != null)
							{
								this._loseLabel___3.text = this._loseText___2;
							}
							this._loseLabel_sh___4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._loseWindow___0, "Lbl_daily_battle_lose_sh");
							if (this._loseLabel_sh___4 != null)
							{
								this._loseLabel_sh___4.text = this._loseText___2;
							}
							break;
						case 2:
						case 3:
						case 4:
							this._winWindow___1.SetActive(true);
							this._loseWindow___0.SetActive(false);
							this._winCount___5 = this.__f__this.m_battleData.goOnWin;
							if (this._winCount___5 < 2)
							{
								this._textObject___6 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_vsreward_text1");
							}
							else
							{
								this._textObject___6 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_vsreward_text2");
								this._textObject___6.ReplaceTag("{PARAM_WIN}", this._winCount___5.ToString());
							}
							this._winLabel___7 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._winWindow___1, "Lbl_daily_battle_win");
							if (this._winLabel___7 != null)
							{
								this._winLabel___7.text = this._textObject___6.text;
							}
							this._winLabel_sh___8 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._winWindow___1, "Lbl_daily_battle_win_sh");
							if (this._winLabel_sh___8 != null)
							{
								this._winLabel_sh___8.text = this._textObject___6.text;
							}
							break;
						}
					}
				}
				if (this.__f__this.m_mainPanel != null)
				{
					this.__f__this.m_mainPanel.alpha = 1f;
				}
				this._window___9 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "ranking_window");
				if (this._window___9 != null)
				{
					this._window___9.SetActive(true);
				}
				if (this.__f__this.m_animation != null)
				{
					ActiveAnimation.Play(this.__f__this.m_animation, "ui_cmn_window_Anim", Direction.Forward);
				}
				this._btnClose___10 = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(this.__f__this.gameObject, "Btn_close");
				if (this._btnClose___10 != null && !EventDelegate.IsValid(this._btnClose___10.onFinished))
				{
					EventDelegate.Add(this._btnClose___10.onFinished, new EventDelegate.Callback(this.__f__this.OnFinished), true);
				}
				this._blinder___11 = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(this.__f__this.gameObject, "blinder");
				if (this._blinder___11 != null && !EventDelegate.IsValid(this._blinder___11.onFinished))
				{
					EventDelegate.Add(this._blinder___11.onFinished, new EventDelegate.Callback(this.__f__this.OnFinished), true);
				}
				this.__f__this.m_isEnd = false;
				this.__f__this.m_isClickClose = false;
				if (this.__f__this.m_battleData != null && this.__f__this.m_battleData.winFlag >= 2)
				{
					SoundManager.SePlay("sys_league_up", "SE");
				}
				else
				{
					SoundManager.SePlay("sys_league_down", "SE");
				}
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

	private const float OPEN_EFFECT_TIME = 2f;

	private static bool s_isActive;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIPanel m_mainPanel;

	private bool m_isClickClose;

	private bool m_isEnd;

	private ServerDailyBattleDataPair m_battleData;

	public static bool isActive
	{
		get
		{
			return DailyBattleRewardWindow.s_isActive;
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
	}

	private IEnumerator SetupObject()
	{
		DailyBattleRewardWindow._SetupObject_c__Iterator30 _SetupObject_c__Iterator = new DailyBattleRewardWindow._SetupObject_c__Iterator30();
		_SetupObject_c__Iterator.__f__this = this;
		return _SetupObject_c__Iterator;
	}

	private void Setup(ServerDailyBattleDataPair data)
	{
		DailyBattleRewardWindow.s_isActive = true;
		base.gameObject.SetActive(true);
		this.m_battleData = data;
		this.m_isEnd = false;
		this.m_isClickClose = false;
		base.enabled = true;
		base.StartCoroutine(this.SetupObject());
	}

	public static DailyBattleRewardWindow Open(ServerDailyBattleDataPair data)
	{
		DailyBattleRewardWindow dailyBattleRewardWindow = null;
		GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
		if (menuAnimUIObject != null)
		{
			dailyBattleRewardWindow = GameObjectUtil.FindChildGameObjectComponent<DailyBattleRewardWindow>(menuAnimUIObject, "DailybattleRewardWindowUI");
			if (dailyBattleRewardWindow == null)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(menuAnimUIObject, "DailybattleRewardWindowUI");
				if (gameObject != null)
				{
					dailyBattleRewardWindow = gameObject.AddComponent<DailyBattleRewardWindow>();
				}
			}
			if (dailyBattleRewardWindow != null)
			{
				dailyBattleRewardWindow.Setup(data);
			}
		}
		return dailyBattleRewardWindow;
	}

	public void OnFinished()
	{
		DailyBattleRewardWindow.s_isActive = false;
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
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
			}
		}
	}
}
