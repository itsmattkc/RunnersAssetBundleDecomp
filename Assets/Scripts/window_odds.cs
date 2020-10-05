using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class window_odds : WindowBase
{
	private sealed class _OpenCoroutine_c__Iterator54 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal ui_roulette_window_odds_scroll[] _ui_roulette_window_odds_scrolls___0;

		internal int _i___1;

		internal int _PC;

		internal object _current;

		internal window_odds __f__this;

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
			case 2u:
				SoundManager.SePlay("sys_window_open", "SE");
				if (this.__f__this.m_oddsList != null)
				{
					this.__f__this.m_oddsItemStorage.maxItemCount = (this.__f__this.m_oddsItemStorage.maxRows = this.__f__this.m_oddsList.Count);
				}
				else
				{
					this.__f__this.m_oddsItemStorage.maxItemCount = (this.__f__this.m_oddsItemStorage.maxRows = 0);
				}
				this.__f__this.m_oddsItemStorage.Restart();
				this._ui_roulette_window_odds_scrolls___0 = this.__f__this.m_oddsItemStorage.GetComponentsInChildren<ui_roulette_window_odds_scroll>(true);
				if (this.__f__this.m_oddsList != null)
				{
					this._i___1 = 0;
					while (this._i___1 < this.__f__this.m_oddsItemStorage.maxItemCount)
					{
						this._ui_roulette_window_odds_scrolls___0[this._i___1].UpdateView(this.__f__this.m_oddsList[this._i___1][0], this.__f__this.m_oddsList[this._i___1][1]);
						this._i___1++;
					}
				}
				this.__f__this.m_noteLabel.text = this.__f__this.m_note;
				this._PC = -1;
				return false;
			default:
				return false;
			}
			if (this.__f__this.gameObject.activeInHierarchy)
			{
				this._current = null;
				this._PC = 2;
			}
			else
			{
				this._current = null;
				this._PC = 1;
			}
			return true;
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
	private UIRectItemStorage m_oddsItemStorage;

	[SerializeField]
	private UILabel m_noteLabel;

	private List<string[]> m_oddsList;

	private string m_note;

	public void Init()
	{
		GameObject gameObject = base.gameObject.transform.parent.gameObject;
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
	}

	public void Open(List<string[]> oddsList, string note)
	{
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, false);
		}
		GameObject gameObject = base.gameObject.transform.parent.gameObject;
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			Animation component = gameObject.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation.Play(component, Direction.Forward);
			}
		}
		this.m_oddsList = oddsList;
		this.m_note = note;
		base.StartCoroutine(this.OpenCoroutine());
	}

	public void Open(ServerPrizeState prize, ServerWheelOptionsData data)
	{
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, false);
		}
		GameObject gameObject = base.gameObject.transform.parent.gameObject;
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			Animation component = gameObject.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation.Play(component, Direction.Forward);
			}
		}
		this.m_oddsList = prize.GetItemOdds(data);
		this.m_note = prize.GetPrizeText(data);
		base.StartCoroutine(this.OpenCoroutine());
		RouletteManager.OpenRouletteWindow();
	}

	private IEnumerator OpenCoroutine()
	{
		window_odds._OpenCoroutine_c__Iterator54 _OpenCoroutine_c__Iterator = new window_odds._OpenCoroutine_c__Iterator54();
		_OpenCoroutine_c__Iterator.__f__this = this;
		return _OpenCoroutine_c__Iterator;
	}

	private void OnClickCloseButton()
	{
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, true);
		}
		SoundManager.SePlay("sys_window_close", "SE");
		RouletteManager.CloseRouletteWindow();
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		if (base.gameObject.activeSelf)
		{
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_close");
			if (uIButtonMessage != null)
			{
				uIButtonMessage.SendMessage("OnClick");
			}
		}
	}
}
