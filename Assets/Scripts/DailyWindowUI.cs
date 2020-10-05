using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DailyWindowUI : WindowBase
{
	private sealed class _DisplayProgressBar_c__Iterator31 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _dailyChallengeObj___0;

		internal long _progress___1;

		internal DailyMissionData _nowData___2;

		internal DailyMissionData _beforeData___3;

		internal int _PC;

		internal object _current;

		internal DailyWindowUI __f__this;

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
				this._dailyChallengeObj___0 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "daily_challenge");
				if (this._dailyChallengeObj___0 != null)
				{
					this._progress___1 = -1L;
					if (SaveDataManager.Instance != null && !this.__f__this.m_isDisplay)
					{
						this._nowData___2 = SaveDataManager.Instance.PlayerData.DailyMission;
						this._beforeData___3 = SaveDataManager.Instance.PlayerData.BeforeDailyMissionData;
						if (this._nowData___2.date == this._beforeData___3.date && this._nowData___2.id == this._beforeData___3.id)
						{
							this._progress___1 = this._beforeData___3.progress;
							this.__f__this.m_isDisplay = true;
						}
					}
					this._dailyChallengeObj___0.SendMessage("OnStartDailyMissionInMileageMap", this._progress___1, SendMessageOptions.DontRequireReceiver);
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

	[SerializeField]
	public bool m_isDebug;

	public bool m_isClickClose;

	public bool m_isEnd;

	private bool m_isDisplay;

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
		this.m_isDisplay = false;
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	public void PlayStart()
	{
		base.gameObject.SetActive(true);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "daily_window");
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			Animation component = gameObject.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation.Play(component, "ui_cmn_window_Anim", Direction.Forward);
			}
			base.StartCoroutine(this.DisplayProgressBar());
		}
		this.m_isEnd = false;
		this.m_isClickClose = false;
	}

	private IEnumerator DisplayProgressBar()
	{
		DailyWindowUI._DisplayProgressBar_c__Iterator31 _DisplayProgressBar_c__Iterator = new DailyWindowUI._DisplayProgressBar_c__Iterator31();
		_DisplayProgressBar_c__Iterator.__f__this = this;
		return _DisplayProgressBar_c__Iterator;
	}

	private void OnClickNextButton()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "daily_challenge");
		if (gameObject != null)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			gameObject.SendMessage("OnClickNextButton", base.gameObject, SendMessageOptions.DontRequireReceiver);
		}
		this.m_isClickClose = true;
	}

	public void OnClosedWindowAnim()
	{
		SoundManager.SeStop("sys_gauge", "SE");
		base.gameObject.SetActive(false);
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
		if (!this.m_isClickClose && !daily_challenge.isUpdateEffect)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_next");
			if (gameObject != null)
			{
				UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
				if (component != null)
				{
					component.SendMessage("OnClick");
				}
			}
		}
	}
}
