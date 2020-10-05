using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RaidBosshelpRequestWindow : WindowBase
{
	private enum BUTTON_ACT
	{
		CLOSE,
		INFO,
		END,
		NONE
	}

	private sealed class _OnSetup_c__Iterator13 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal List<ServerEventRaidBossDesiredState> data;

		internal GameObject _ui_button___0;

		internal UIButtonMessage _button_msg___1;

		internal ui_ranking_scroll[] _list___2;

		internal int _i___3;

		internal ActiveAnimation _activeAnim___4;

		internal int _PC;

		internal object _current;

		internal List<ServerEventRaidBossDesiredState> ___data;

		internal RaidBosshelpRequestWindow __f__this;

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
				BackKeyManager.AddWindowCallBack(this.__f__this.gameObject);
				this.__f__this.mainPanel.alpha = 1f;
				this.__f__this.m_close = false;
				this.__f__this.m_btnAct = RaidBosshelpRequestWindow.BUTTON_ACT.NONE;
				this.__f__this.m_desiredList = this.data;
				this._ui_button___0 = GameObjectUtil.FindChildGameObject(this.__f__this.mainPanel.gameObject, "Btn_ok");
				if (this._ui_button___0 != null)
				{
					this._button_msg___1 = this._ui_button___0.GetComponent<UIButtonMessage>();
					if (this._button_msg___1 != null)
					{
						this._button_msg___1.enabled = true;
						this._button_msg___1.trigger = UIButtonMessage.Trigger.OnClick;
						this._button_msg___1.target = this.__f__this.gameObject;
						this._button_msg___1.functionName = "OnClickOkButton";
					}
				}
				if (this.__f__this.m_desiredList != null)
				{
					this.__f__this.m_listPanel.enabled = true;
					this.__f__this.m_storage = this.__f__this.m_listPanel.GetComponentInChildren<UIRectItemStorage>();
					if (this.__f__this.m_storage != null && this.__f__this.m_desiredList.Count > 0)
					{
						this.__f__this.m_storage.maxItemCount = (this.__f__this.m_storage.maxRows = this.__f__this.m_desiredList.Count);
						this.__f__this.m_storage.Restart();
						this._list___2 = this.__f__this.m_storage.GetComponentsInChildren<ui_ranking_scroll>();
						if (this._list___2 != null && this._list___2.Length > 0)
						{
							this._i___3 = 0;
							while (this._i___3 < this.__f__this.m_desiredList.Count)
							{
								if (this._list___2.Length <= this._i___3)
								{
									break;
								}
								this._list___2[this._i___3].UpdateViewForRaidbossDesired(this.__f__this.m_desiredList[this._i___3]);
								this._i___3++;
							}
						}
					}
				}
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				if (this.__f__this.m_animation != null)
				{
					this._activeAnim___4 = ActiveAnimation.Play(this.__f__this.m_animation, "ui_cmn_window_Anim", Direction.Forward);
					EventDelegate.Add(this._activeAnim___4.onFinished, new EventDelegate.Callback(this.__f__this.WindowAnimationFinishCallback), true);
					SoundManager.SePlay("sys_window_open", "SE");
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

	private const float UPDATE_TIME = 0.25f;

	[SerializeField]
	private UIPanel mainPanel;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIDraggablePanel m_listPanel;

	private bool m_close;

	private RaidBosshelpRequestWindow.BUTTON_ACT m_btnAct = RaidBosshelpRequestWindow.BUTTON_ACT.NONE;

	private UIRectItemStorage m_storage;

	private List<ServerEventRaidBossDesiredState> m_desiredList;

	private static RaidBosshelpRequestWindow s_instance;

	private static RaidBosshelpRequestWindow Instance
	{
		get
		{
			return RaidBosshelpRequestWindow.s_instance;
		}
	}

	public bool isFinished()
	{
		return this.m_btnAct == RaidBosshelpRequestWindow.BUTTON_ACT.END;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Setup(List<ServerEventRaidBossDesiredState> data)
	{
		base.StartCoroutine(this.OnSetup(data));
	}

	public IEnumerator OnSetup(List<ServerEventRaidBossDesiredState> data)
	{
		RaidBosshelpRequestWindow._OnSetup_c__Iterator13 _OnSetup_c__Iterator = new RaidBosshelpRequestWindow._OnSetup_c__Iterator13();
		_OnSetup_c__Iterator.data = data;
		_OnSetup_c__Iterator.___data = data;
		_OnSetup_c__Iterator.__f__this = this;
		return _OnSetup_c__Iterator;
	}

	public void OnClickOkButton()
	{
		this.m_btnAct = RaidBosshelpRequestWindow.BUTTON_ACT.CLOSE;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_ok");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			component.Play(true);
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_close)
		{
			RaidBosshelpRequestWindow.BUTTON_ACT btnAct = this.m_btnAct;
			if (btnAct != RaidBosshelpRequestWindow.BUTTON_ACT.CLOSE)
			{
				if (btnAct != RaidBosshelpRequestWindow.BUTTON_ACT.INFO)
				{
					base.gameObject.SetActive(false);
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
			else
			{
				this.m_btnAct = RaidBosshelpRequestWindow.BUTTON_ACT.END;
				base.gameObject.SetActive(false);
			}
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		this.OnClickOkButton();
	}

	public static RaidBosshelpRequestWindow Create(List<ServerEventRaidBossDesiredState> data)
	{
		if (RaidBosshelpRequestWindow.s_instance != null)
		{
			RaidBosshelpRequestWindow.s_instance.gameObject.SetActive(true);
			RaidBosshelpRequestWindow.s_instance.Setup(data);
			return RaidBosshelpRequestWindow.s_instance;
		}
		return null;
	}

	private void Awake()
	{
		this.SetInstance();
		base.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (RaidBosshelpRequestWindow.s_instance == this)
		{
			RaidBosshelpRequestWindow.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (RaidBosshelpRequestWindow.s_instance == null)
		{
			RaidBosshelpRequestWindow.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
