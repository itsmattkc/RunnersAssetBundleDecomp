using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButtonEventAnimation : MonoBehaviour
{
	public delegate void AnimationEndCallback();

	private sealed class _SetInAnimationCoroutine_c__Iterator36 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal ButtonInfoTable.AnimInfo animInfo;

		internal bool reverseFlag;

		internal int _PC;

		internal object _current;

		internal ButtonInfoTable.AnimInfo ___animInfo;

		internal bool ___reverseFlag;

		internal ButtonEventAnimation __f__this;

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
				this.__f__this.InitInAnimation(this.animInfo);
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._current = null;
				this._PC = 2;
				return true;
			case 2u:
				if (this.animInfo != null)
				{
					this._current = this.__f__this.StartCoroutine(this.__f__this.DelayPlayAnimation(this.animInfo, this.reverseFlag));
					this._PC = 3;
					return true;
				}
				this.__f__this.OnFinishedInAnimationCallback();
				break;
			case 3u:
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

	private sealed class _DelayPlayAnimation_c__Iterator37 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _waite_frame___0;

		internal ButtonInfoTable.AnimInfo animInfo;

		internal GameObject _obj___1;

		internal ShopUI _shop___2;

		internal daily_challenge _dailyChallenge___3;

		internal ChaoSetUI _chao___4;

		internal ItemSetMenu _item___5;

		internal MenuPlayerSet _player___6;

		internal OptionUI _option___7;

		internal PresentBoxUI _presentBox___8;

		internal DailyInfo _dailyInfo___9;

		internal Animation _anim___10;

		internal bool reverseFlag;

		internal Direction _dire___11;

		internal ActiveAnimation _acviteAnim___12;

		internal int _PC;

		internal object _current;

		internal ButtonInfoTable.AnimInfo ___animInfo;

		internal bool ___reverseFlag;

		internal ButtonEventAnimation __f__this;

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
				this._waite_frame___0 = 2;
				break;
			case 1u:
				break;
			case 2u:
				goto IL_F5;
			case 3u:
				IL_147:
				if (RouletteManager.IsRouletteEnabled())
				{
					goto IL_151;
				}
				this._current = null;
				this._PC = 3;
				return true;
			case 4u:
				IL_1AA:
				if (this._dailyChallenge___3.IsEndSetup)
				{
					goto IL_1BA;
				}
				this._current = null;
				this._PC = 4;
				return true;
			case 5u:
				IL_1F4:
				if (this._chao___4.IsEndSetup)
				{
					goto IL_204;
				}
				this._current = null;
				this._PC = 5;
				return true;
			case 6u:
				IL_23E:
				if (this._item___5.IsEndSetup)
				{
					goto IL_24E;
				}
				this._current = null;
				this._PC = 6;
				return true;
			case 7u:
				IL_288:
				if (this._player___6.SetUpped)
				{
					goto IL_298;
				}
				this._current = null;
				this._PC = 7;
				return true;
			case 8u:
				IL_2D2:
				if (this._option___7.IsEndSetup)
				{
					goto IL_2E2;
				}
				this._current = null;
				this._PC = 8;
				return true;
			case 9u:
				IL_31D:
				if (this._presentBox___8.IsEndSetup)
				{
					goto IL_32D;
				}
				this._current = null;
				this._PC = 9;
				return true;
			case 10u:
				IL_363:
				if (this.animInfo.animName != null)
				{
					this._anim___10 = this._obj___1.GetComponent<Animation>();
					if (this._anim___10 != null)
					{
						if (this.animInfo.animName == "ui_mm_Anim")
						{
							this.reverseFlag = !this.reverseFlag;
						}
						this._dire___11 = ((!this.reverseFlag) ? Direction.Forward : Direction.Reverse);
						this._acviteAnim___12 = ActiveAnimation.Play(this._anim___10, this.animInfo.animName, this._dire___11);
						if (this._acviteAnim___12 != null)
						{
							EventDelegate.Add(this._acviteAnim___12.onFinished, new EventDelegate.Callback(this.__f__this.OnFinishedInAnimationCallback), true);
						}
					}
					goto IL_43B;
				}
				this.__f__this.OnFinishedInAnimationCallback();
				goto IL_43B;
			default:
				return false;
			}
			if (this._waite_frame___0 > 0)
			{
				this._waite_frame___0--;
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.animInfo == null)
			{
				goto IL_43B;
			}
			this._obj___1 = GameObjectUtil.FindChildGameObject(this.__f__this.m_menu_anim_obj, this.animInfo.targetName);
			if (!(this._obj___1 != null))
			{
				goto IL_43B;
			}
			this._shop___2 = this._obj___1.GetComponent<ShopUI>();
			if (!(this._shop___2 != null))
			{
				goto IL_105;
			}
			IL_F5:
			if (!this._shop___2.IsInitShop)
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			IL_105:
			if (this._obj___1.name == "RouletteTopUI")
			{
				RouletteManager.Instance.gameObject.SetActive(true);
				goto IL_147;
			}
			IL_151:
			if (this._obj___1.name == "DailyChallengeInformationUI")
			{
				this._dailyChallenge___3 = GameObjectUtil.FindChildGameObjectComponent<daily_challenge>(this._obj___1, "daily_challenge");
				if (this._dailyChallenge___3 != null)
				{
					goto IL_1AA;
				}
			}
			IL_1BA:
			this._chao___4 = this._obj___1.GetComponent<ChaoSetUI>();
			if (this._chao___4 != null)
			{
				goto IL_1F4;
			}
			IL_204:
			this._item___5 = this._obj___1.GetComponent<ItemSetMenu>();
			if (this._item___5 != null)
			{
				goto IL_23E;
			}
			IL_24E:
			this._player___6 = this._obj___1.GetComponent<MenuPlayerSet>();
			if (this._player___6 != null)
			{
				goto IL_288;
			}
			IL_298:
			this._option___7 = this._obj___1.GetComponent<OptionUI>();
			if (this._option___7 != null)
			{
				goto IL_2D2;
			}
			IL_2E2:
			this._presentBox___8 = this._obj___1.GetComponent<PresentBoxUI>();
			if (this._presentBox___8 != null)
			{
				goto IL_31D;
			}
			IL_32D:
			this._dailyInfo___9 = this._obj___1.GetComponent<DailyInfo>();
			if (this._dailyInfo___9 != null)
			{
				this._current = null;
				this._PC = 10;
				return true;
			}
			goto IL_363;
			IL_43B:
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

	private sealed class _WaitRouletteClose_c__Iterator38 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal ButtonEventAnimation __f__this;

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
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!RouletteManager.IsRouletteClose())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this.__f__this.OnFinishedOutAnimationCallback();
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

	private GameObject m_menu_anim_obj;

	private ButtonInfoTable m_info_table = new ButtonInfoTable();

	private ButtonEventAnimation.AnimationEndCallback m_inAnimEndCallback;

	private ButtonEventAnimation.AnimationEndCallback m_outAnimEndCallback;

	private ButtonInfoTable.PageType m_currentPageType;

	public void Initialize()
	{
		this.m_menu_anim_obj = HudMenuUtility.GetMenuAnimUIObject();
	}

	public void PageOutAnimation(ButtonInfoTable.PageType currentPageType, ButtonInfoTable.PageType nextPageType, ButtonEventAnimation.AnimationEndCallback animEndCallback)
	{
		this.m_currentPageType = currentPageType;
		this.m_outAnimEndCallback = animEndCallback;
		ButtonInfoTable.AnimInfo pageAnimInfo = this.m_info_table.GetPageAnimInfo(currentPageType);
		if (pageAnimInfo == null)
		{
			this.OnFinishedOutAnimationCallback();
			return;
		}
		if (nextPageType == ButtonInfoTable.PageType.STAGE)
		{
			this.SetOutAnimation(new ButtonInfoTable.AnimInfo("ItemSet_3_UI", "ui_itemset_3_outro_Anim"), false);
		}
		else
		{
			this.SetOutAnimation(pageAnimInfo, true);
		}
	}

	public void PageInAnimation(ButtonInfoTable.PageType nextPageType, ButtonEventAnimation.AnimationEndCallback animEndCallback)
	{
		this.m_inAnimEndCallback = animEndCallback;
		ButtonInfoTable.AnimInfo pageAnimInfo = this.m_info_table.GetPageAnimInfo(nextPageType);
		base.StartCoroutine(this.SetInAnimationCoroutine(pageAnimInfo, false));
	}

	private void SetOutAnimation(ButtonInfoTable.AnimInfo animInfo, bool reverseFlag)
	{
		if (animInfo != null && animInfo.animName != null)
		{
			Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(this.m_menu_anim_obj, animInfo.targetName);
			if (animation != null)
			{
				if (animInfo.animName == "ui_mm_Anim")
				{
					reverseFlag = !reverseFlag;
				}
				Direction playDirection = (!reverseFlag) ? Direction.Forward : Direction.Reverse;
				ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, animInfo.animName, playDirection);
				if (activeAnimation != null)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedOutAnimationCallback), true);
				}
			}
			else
			{
				this.OnFinishedOutAnimationCallback();
			}
		}
		else if (animInfo.targetName == "RouletteTopUI")
		{
			base.StartCoroutine(this.WaitRouletteClose());
		}
		else
		{
			this.OnFinishedOutAnimationCallback();
		}
	}

	private IEnumerator SetInAnimationCoroutine(ButtonInfoTable.AnimInfo animInfo, bool reverseFlag)
	{
		ButtonEventAnimation._SetInAnimationCoroutine_c__Iterator36 _SetInAnimationCoroutine_c__Iterator = new ButtonEventAnimation._SetInAnimationCoroutine_c__Iterator36();
		_SetInAnimationCoroutine_c__Iterator.animInfo = animInfo;
		_SetInAnimationCoroutine_c__Iterator.reverseFlag = reverseFlag;
		_SetInAnimationCoroutine_c__Iterator.___animInfo = animInfo;
		_SetInAnimationCoroutine_c__Iterator.___reverseFlag = reverseFlag;
		_SetInAnimationCoroutine_c__Iterator.__f__this = this;
		return _SetInAnimationCoroutine_c__Iterator;
	}

	private void InitInAnimation(ButtonInfoTable.AnimInfo animInfo)
	{
		if (animInfo != null)
		{
			bool flag = false;
			Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(this.m_menu_anim_obj, animInfo.targetName);
			if (animation != null)
			{
				if (animInfo.animName == "ui_mm_Anim")
				{
					flag = !flag;
				}
				Direction playDirection = (!flag) ? Direction.Forward : Direction.Reverse;
				ActiveAnimation.Play(animation, animInfo.animName, playDirection);
				animation.Stop(animInfo.animName);
			}
		}
	}

	public IEnumerator DelayPlayAnimation(ButtonInfoTable.AnimInfo animInfo, bool reverseFlag)
	{
		ButtonEventAnimation._DelayPlayAnimation_c__Iterator37 _DelayPlayAnimation_c__Iterator = new ButtonEventAnimation._DelayPlayAnimation_c__Iterator37();
		_DelayPlayAnimation_c__Iterator.animInfo = animInfo;
		_DelayPlayAnimation_c__Iterator.reverseFlag = reverseFlag;
		_DelayPlayAnimation_c__Iterator.___animInfo = animInfo;
		_DelayPlayAnimation_c__Iterator.___reverseFlag = reverseFlag;
		_DelayPlayAnimation_c__Iterator.__f__this = this;
		return _DelayPlayAnimation_c__Iterator;
	}

	public IEnumerator WaitRouletteClose()
	{
		ButtonEventAnimation._WaitRouletteClose_c__Iterator38 _WaitRouletteClose_c__Iterator = new ButtonEventAnimation._WaitRouletteClose_c__Iterator38();
		_WaitRouletteClose_c__Iterator.__f__this = this;
		return _WaitRouletteClose_c__Iterator;
	}

	private void OnFinishedOutAnimationCallback()
	{
		if (this.m_outAnimEndCallback != null)
		{
			this.m_outAnimEndCallback();
			this.m_outAnimEndCallback = null;
		}
	}

	private void OnFinishedInAnimationCallback()
	{
		if (this.m_inAnimEndCallback != null)
		{
			this.m_inAnimEndCallback();
			this.m_inAnimEndCallback = null;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
