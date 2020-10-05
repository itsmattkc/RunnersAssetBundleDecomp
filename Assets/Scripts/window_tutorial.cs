using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class window_tutorial : WindowBase
{
	public enum DisplayType
	{
		TUTORIAL,
		QUICK,
		CHARA,
		BOSS_MAP_1,
		BOSS_MAP_2,
		BOSS_MAP_3,
		UNKNOWN
	}

	public class ScrollInfo
	{
		private window_tutorial.DisplayType m_dispType = window_tutorial.DisplayType.UNKNOWN;

		private CharaType m_charaType = CharaType.UNKNOWN;

		private HudTutorial.Id m_hudId = HudTutorial.Id.NONE;

		private window_tutorial m_parentWindow;

		public window_tutorial.DisplayType DispType
		{
			get
			{
				return this.m_dispType;
			}
		}

		public CharaType Chara
		{
			get
			{
				return this.m_charaType;
			}
		}

		public HudTutorial.Id HudId
		{
			get
			{
				return this.m_hudId;
			}
		}

		public window_tutorial Parent
		{
			get
			{
				return this.m_parentWindow;
			}
		}

		public ScrollInfo()
		{
		}

		public ScrollInfo(window_tutorial window, window_tutorial.DisplayType dispType, CharaType charaType = CharaType.UNKNOWN)
		{
			this.m_parentWindow = window;
			this.m_dispType = dispType;
			this.m_charaType = charaType;
			switch (this.m_dispType)
			{
			case window_tutorial.DisplayType.QUICK:
				this.m_hudId = HudTutorial.Id.QUICK_1;
				break;
			case window_tutorial.DisplayType.CHARA:
				this.m_hudId = CharaTypeUtil.GetCharacterTutorialID(this.m_charaType);
				break;
			case window_tutorial.DisplayType.BOSS_MAP_1:
				this.m_hudId = HudTutorial.Id.MAPBOSS_1;
				break;
			case window_tutorial.DisplayType.BOSS_MAP_2:
				this.m_hudId = HudTutorial.Id.MAPBOSS_2;
				break;
			case window_tutorial.DisplayType.BOSS_MAP_3:
				this.m_hudId = HudTutorial.Id.MAPBOSS_3;
				break;
			}
		}
	}

	private sealed class _DelayUpdateRectItemStorage_c__Iterator42 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _waite_frame___0;

		internal int _PC;

		internal object _current;

		internal window_tutorial __f__this;

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
				this._waite_frame___0 = 1;
				break;
			case 1u:
				break;
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
			this.__f__this.UpdateRectItemStorage();
			this.__f__this.m_inited = true;
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

	private readonly BossType[] TUTORIAL_BOSS_TYPE_TABLE = new BossType[]
	{
		BossType.MAP2,
		BossType.MAP3
	};

	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private UIRectItemStorage m_itemStorage;

	[SerializeField]
	private UILabel m_headerTextLabel;

	private bool m_isEnd;

	private bool m_inited;

	private UIPlayAnimation m_uiAnimation;

	private List<window_tutorial.ScrollInfo> m_scrollInfos = new List<window_tutorial.ScrollInfo>();

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
		OptionMenuUtility.TranObj(base.gameObject);
		base.enabled = false;
		if (this.m_closeBtn != null)
		{
			UIPlayAnimation component = this.m_closeBtn.GetComponent<UIPlayAnimation>();
			if (component != null)
			{
				EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedCloseAnimationCallback), false);
			}
			UIButtonMessage uIButtonMessage = this.m_closeBtn.GetComponent<UIButtonMessage>();
			if (uIButtonMessage == null)
			{
				uIButtonMessage = this.m_closeBtn.AddComponent<UIButtonMessage>();
			}
			if (uIButtonMessage != null)
			{
				uIButtonMessage.enabled = true;
				uIButtonMessage.trigger = UIButtonMessage.Trigger.OnClick;
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickCloseButton";
			}
		}
		TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "tutorial");
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component2 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component2;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		if (ServerInterface.MileageMapState != null)
		{
			if (!HudMenuUtility.IsSystemDataFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_2) && ServerInterface.MileageMapState.m_episode > 2)
			{
				HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_2);
			}
			if (!HudMenuUtility.IsSystemDataFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_3) && ServerInterface.MileageMapState.m_episode > 3)
			{
				HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.TUTORIAL_BOSS_MAP_3);
			}
		}
	}

	private void OnClickCloseButton()
	{
		this.SetCloseBtnColliderTrigger(true);
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnFinishedInAnimationCallback()
	{
		this.SetCloseBtnColliderTrigger(false);
	}

	private void OnFinishedCloseAnimationCallback()
	{
		this.m_isEnd = true;
	}

	public void PlayOpenWindow()
	{
		this.m_isEnd = false;
		if (this.m_inited)
		{
			this.UpdateRectItemStorage();
		}
		else
		{
			base.StartCoroutine(this.DelayUpdateRectItemStorage());
		}
		if (this.m_uiAnimation != null)
		{
			EventDelegate.Add(this.m_uiAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedInAnimationCallback), false);
			this.m_uiAnimation.Play(true);
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	public IEnumerator DelayUpdateRectItemStorage()
	{
		window_tutorial._DelayUpdateRectItemStorage_c__Iterator42 _DelayUpdateRectItemStorage_c__Iterator = new window_tutorial._DelayUpdateRectItemStorage_c__Iterator42();
		_DelayUpdateRectItemStorage_c__Iterator.__f__this = this;
		return _DelayUpdateRectItemStorage_c__Iterator;
	}

	private void UpdateRectItemStorage()
	{
		if (this.m_itemStorage != null)
		{
			this.m_scrollInfos.Clear();
			this.m_scrollInfos.Add(new window_tutorial.ScrollInfo(this, window_tutorial.DisplayType.TUTORIAL, CharaType.UNKNOWN));
			this.m_scrollInfos.Add(new window_tutorial.ScrollInfo(this, window_tutorial.DisplayType.QUICK, CharaType.UNKNOWN));
			if (SystemSaveManager.Instance != null)
			{
				SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
				if (systemdata != null)
				{
					this.m_scrollInfos.Add(new window_tutorial.ScrollInfo(this, window_tutorial.DisplayType.CHARA, CharaType.SONIC));
					for (int i = 1; i < 29; i++)
					{
						SystemData.CharaTutorialFlagStatus characterSaveDataFlagStatus = CharaTypeUtil.GetCharacterSaveDataFlagStatus((CharaType)i);
						if (systemdata.IsFlagStatus(characterSaveDataFlagStatus))
						{
							this.m_scrollInfos.Add(new window_tutorial.ScrollInfo(this, window_tutorial.DisplayType.CHARA, (CharaType)i));
						}
					}
					this.m_scrollInfos.Add(new window_tutorial.ScrollInfo(this, window_tutorial.DisplayType.BOSS_MAP_1, CharaType.UNKNOWN));
					SystemData.FlagStatus bossSaveDataFlagStatus = BossTypeUtil.GetBossSaveDataFlagStatus(BossType.MAP2);
					if (systemdata.IsFlagStatus(bossSaveDataFlagStatus))
					{
						this.m_scrollInfos.Add(new window_tutorial.ScrollInfo(this, window_tutorial.DisplayType.BOSS_MAP_2, CharaType.UNKNOWN));
					}
					SystemData.FlagStatus bossSaveDataFlagStatus2 = BossTypeUtil.GetBossSaveDataFlagStatus(BossType.MAP3);
					if (systemdata.IsFlagStatus(bossSaveDataFlagStatus2))
					{
						this.m_scrollInfos.Add(new window_tutorial.ScrollInfo(this, window_tutorial.DisplayType.BOSS_MAP_3, CharaType.UNKNOWN));
					}
				}
			}
			this.m_itemStorage.maxItemCount = this.m_scrollInfos.Count;
			this.m_itemStorage.maxRows = this.m_scrollInfos.Count;
			this.m_itemStorage.Restart();
			ui_option_tutorial_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_option_tutorial_scroll>(true);
			int num = componentsInChildren.Length;
			for (int j = 0; j < num; j++)
			{
				componentsInChildren[j].gameObject.SetActive(true);
				componentsInChildren[j].UpdateView(this.m_scrollInfos[j]);
			}
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		bool flag = false;
		ui_option_tutorial_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_option_tutorial_scroll>(true);
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			if (componentsInChildren[i].OpenWindow)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			UIButtonMessage component = this.m_closeBtn.GetComponent<UIButtonMessage>();
			if (component != null)
			{
				component.SendMessage("OnClick");
			}
		}
	}

	public void SetCloseBtnColliderTrigger(bool triggerFlag)
	{
		if (this.m_closeBtn != null)
		{
			BoxCollider component = this.m_closeBtn.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.isTrigger = triggerFlag;
			}
		}
		ui_option_tutorial_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_option_tutorial_scroll>(true);
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			BoxCollider boxCollider = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(componentsInChildren[i].gameObject, "Btn_character");
			if (boxCollider != null)
			{
				boxCollider.isTrigger = triggerFlag;
			}
		}
	}
}
