using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class LoginBonusWindowUI : WindowBase
{
	public enum LoginBonusType
	{
		NORMAL,
		FIRST,
		MAX
	}

	private sealed class _Setup_c__Iterator34 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal LoginBonusWindowUI.LoginBonusType type;

		internal ServerInterface _serverInterface___0;

		internal GameObject _loginBonusObj___1;

		internal int _nowBonusCount___2;

		internal int _dayCount___3;

		internal int _i___4;

		internal string _objName___5;

		internal GameObject _loginDayObj___6;

		internal LoginDayObject _dayObj___7;

		internal ServerLoginBonusReward _reward___8;

		internal UILabel _Label_days___9;

		internal int _nowDayCount___10;

		internal int _totalDays___11;

		internal int _lastDayCount___12;

		internal UILabel _Label_BonusItems___13;

		internal string _rewardText___14;

		internal string _lineBreakText___15;

		internal int _itemCount___16;

		internal int _i___17;

		internal ServerItem _itemIndex___18;

		internal string _itemName___19;

		internal int _numItemCount___20;

		internal int _PC;

		internal object _current;

		internal LoginBonusWindowUI.LoginBonusType ___type;

		internal LoginBonusWindowUI __f__this;

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
				this.__f__this.m_type = this.type;
				this._serverInterface___0 = ServerInterface.LoggedInServerInterface;
				if (this._serverInterface___0 != null)
				{
					this.__f__this.m_days = new List<LoginDayObject>();
					this._loginBonusObj___1 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "login_bonus");
					this.__f__this.m_BonusData = ServerInterface.LoginBonusData;
					if (this.__f__this.m_BonusData != null && this._loginBonusObj___1 != null)
					{
						this._nowBonusCount___2 = 0;
						if (this.type == LoginBonusWindowUI.LoginBonusType.NORMAL)
						{
							this.__f__this.m_RewardList = this.__f__this.m_BonusData.m_loginBonusRewardList;
							this._nowBonusCount___2 = this.__f__this.m_BonusData.m_loginBonusState.m_numBonus;
						}
						else
						{
							this.__f__this.m_RewardList = this.__f__this.m_BonusData.m_firstLoginBonusRewardList;
							this._nowBonusCount___2 = this.__f__this.m_BonusData.m_loginBonusState.m_numLogin;
						}
						if (this.__f__this.m_RewardList != null)
						{
							this._dayCount___3 = this.__f__this.m_RewardList.Count;
							this._i___4 = 0;
							while (this._i___4 < this._dayCount___3)
							{
								this._objName___5 = "ui_login_day" + (this._i___4 + 1);
								if (this._i___4 == this._dayCount___3 - 1)
								{
									this._objName___5 = "ui_login_big";
								}
								this._loginDayObj___6 = GameObjectUtil.FindChildGameObject(this._loginBonusObj___1, this._objName___5);
								if (this._loginDayObj___6 != null)
								{
									this._dayObj___7 = new LoginDayObject(this._loginDayObj___6, this._i___4 + 1);
									this._reward___8 = this.__f__this.m_RewardList[this._i___4];
									if (this._reward___8 != null)
									{
										if (this._reward___8.m_itemList != null)
										{
											this._dayObj___7.SetItem(this._reward___8.m_itemList[0].m_itemId);
											this._dayObj___7.count = this._reward___8.m_itemList[0].m_num;
										}
										this._dayObj___7.SetAlready(this._i___4 < this._nowBonusCount___2);
									}
									this.__f__this.m_days.Add(this._dayObj___7);
								}
								this._i___4++;
							}
							this._Label_days___9 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._loginBonusObj___1, "Lbl_days");
							if (this._Label_days___9 != null)
							{
								this._nowDayCount___10 = this.__f__this.m_BonusData.CalcTodayCount();
								this._totalDays___11 = this.__f__this.m_BonusData.getTotalDays();
								if (this.__f__this.m_type == LoginBonusWindowUI.LoginBonusType.FIRST)
								{
									this._nowDayCount___10 = this.__f__this.m_BonusData.m_loginBonusState.m_numLogin;
									this._totalDays___11 = this._dayCount___3;
								}
								if (this._nowDayCount___10 > -1)
								{
									this._lastDayCount___12 = this._totalDays___11 - this._nowDayCount___10;
									if (this._lastDayCount___12 < 0)
									{
										this._lastDayCount___12 = 0;
									}
									if (this.__f__this.m_type == LoginBonusWindowUI.LoginBonusType.FIRST)
									{
										this._Label_days___9.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "LoginBonus", "count").text, new Dictionary<string, string>
										{
											{
												"{COUNT}",
												this._lastDayCount___12.ToString()
											}
										});
									}
									else
									{
										this._Label_days___9.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "LoginBonus", "day").text, new Dictionary<string, string>
										{
											{
												"{DAY}",
												this._lastDayCount___12.ToString()
											}
										});
									}
								}
							}
							this._Label_BonusItems___13 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this._loginBonusObj___1, "Lbl_loginbonus_feature");
							if (this._Label_BonusItems___13 != null && this.__f__this.m_BonusData.m_loginBonusState != null)
							{
								if (this.type == LoginBonusWindowUI.LoginBonusType.NORMAL)
								{
									this.__f__this.m_TodayReward = this.__f__this.m_BonusData.m_lastBonusReward;
								}
								else
								{
									this.__f__this.m_TodayReward = this.__f__this.m_BonusData.m_firstLastBonusReward;
								}
								this._rewardText___14 = string.Empty;
								this._lineBreakText___15 = "\n";
								if (this.__f__this.m_TodayReward != null)
								{
									this._itemCount___16 = this.__f__this.m_TodayReward.m_itemList.Count;
									this._i___17 = 0;
									while (this._i___17 < this._itemCount___16)
									{
										if (this.__f__this.m_TodayReward.m_itemList[this._i___17] != null)
										{
											this._itemIndex___18 = new ServerItem((ServerItem.Id)this.__f__this.m_TodayReward.m_itemList[this._i___17].m_itemId);
											this._itemName___19 = this._itemIndex___18.serverItemName;
											this._numItemCount___20 = this.__f__this.m_TodayReward.m_itemList[this._i___17].m_num;
											if (!string.IsNullOrEmpty(this._itemName___19))
											{
												string text = this._rewardText___14;
												this._rewardText___14 = string.Concat(new string[]
												{
													text,
													this._itemName___19,
													"×",
													this._numItemCount___20.ToString(),
													this._lineBreakText___15
												});
											}
										}
										this._i___17++;
									}
								}
								if (!string.IsNullOrEmpty(this._rewardText___14))
								{
									this._Label_BonusItems___13.text = this._rewardText___14;
								}
							}
						}
						this._current = null;
						this._PC = 1;
						return true;
					}
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._PC = -1;
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

	private bool m_isClickClose;

	private bool m_isEnd;

	private bool m_isOpened;

	private Animation m_animation;

	private List<LoginDayObject> m_days;

	private ServerLoginBonusData m_BonusData;

	private List<ServerLoginBonusReward> m_RewardList;

	private ServerLoginBonusReward m_TodayReward;

	private LoginBonusWindowUI.LoginBonusType m_type;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
		base.enabled = false;
		this.m_isEnd = false;
		this.m_isOpened = false;
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	private IEnumerator Setup(LoginBonusWindowUI.LoginBonusType type)
	{
		LoginBonusWindowUI._Setup_c__Iterator34 _Setup_c__Iterator = new LoginBonusWindowUI._Setup_c__Iterator34();
		_Setup_c__Iterator.type = type;
		_Setup_c__Iterator.___type = type;
		_Setup_c__Iterator.__f__this = this;
		return _Setup_c__Iterator;
	}

	public void PlayStart(LoginBonusWindowUI.LoginBonusType type)
	{
		base.enabled = true;
		this.m_TodayReward = null;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "login_window");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
			base.StartCoroutine(this.Setup(type));
			if (this.m_RewardList == null || this.m_TodayReward == null)
			{
				this.OnClosedWindowAnim();
				global::Debug.Log("LoginBonusWindowUI::PlayStart >> NotLoginBonusReward! = " + type.ToString());
				return;
			}
			gameObject.SetActive(true);
			this.m_animation = gameObject.GetComponent<Animation>();
			if (this.m_animation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnOpenWindowAnim), true);
			}
			UIPlayAnimation uIPlayAnimation = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(gameObject, "Btn_next");
			if (uIPlayAnimation != null)
			{
				uIPlayAnimation.enabled = false;
			}
			UIPlayAnimation uIPlayAnimation2 = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(gameObject, "blinder");
			if (uIPlayAnimation2 != null)
			{
				uIPlayAnimation2.enabled = false;
			}
		}
		this.m_isEnd = false;
		this.m_isClickClose = false;
		this.m_isOpened = false;
	}

	private void Update()
	{
	}

	private void OnClickNextButton()
	{
		if (!this.m_isClickClose)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnClosedWindowAnim), true);
			}
			this.m_isClickClose = true;
		}
	}

	public void OnOpenWindowAnim()
	{
		if (!this.m_isOpened)
		{
			this.m_isOpened = true;
			if (this.m_days != null && this.m_BonusData != null && this.m_BonusData.m_loginBonusState != null)
			{
				int num;
				int count;
				if (this.m_type == LoginBonusWindowUI.LoginBonusType.NORMAL)
				{
					num = this.m_BonusData.m_loginBonusState.m_numBonus - 1;
					count = this.m_BonusData.m_loginBonusRewardList.Count;
				}
				else
				{
					num = this.m_BonusData.m_loginBonusState.m_numLogin - 1;
					count = this.m_BonusData.m_firstLoginBonusRewardList.Count;
				}
				if (count < num)
				{
					num = count - 1;
				}
				if (num > -1)
				{
					this.m_days[num].PlayGetAnimation();
				}
			}
		}
	}

	public void OnClosedWindowAnim()
	{
		base.gameObject.SetActive(false);
		base.enabled = false;
		this.m_isEnd = true;
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
		if (!this.m_isClickClose)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnClosedWindowAnim), true);
			}
			this.m_isClickClose = true;
		}
	}
}
