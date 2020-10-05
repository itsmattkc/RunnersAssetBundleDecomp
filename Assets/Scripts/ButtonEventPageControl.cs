using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButtonEventPageControl : MonoBehaviour
{
	private class WaitSendMessageParam
	{
		public Component m_component;

		public int m_waitCount;

		public string m_methodName;

		public object m_value;
	}

	public delegate void ResourceLoadedCallback();

	private sealed class _OnCurrentPageAnimationEndCallbackCoroutine_c__Iterator39 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal HudDisplay.ObjType _obj_type___0;

		internal int _PC;

		internal object _current;

		internal ButtonEventPageControl __f__this;

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
				this.__f__this.m_hud_display.SetAllDisableDisplay();
				this._current = this.__f__this.StartCoroutine(this.__f__this.m_resLoader.LoadAtlasResourceIfNotLoaded());
				this._PC = 1;
				return true;
			case 1u:
				this._current = this.__f__this.StartCoroutine(this.__f__this.m_resLoader.LoadPageResourceIfNotLoadedSync(this.__f__this.m_currentPageType, delegate
				{
					this.__f__this.m_resourceLoadedCallback();
				}));
				this._PC = 2;
				return true;
			case 2u:
				this.__f__this.m_hud_display = new HudDisplay();
				this._obj_type___0 = HudDisplay.CalcObjTypeFromSequenceType(this.__f__this.m_current_sequence_type);
				this.__f__this.m_hud_display.SetDisplayHudObject(this._obj_type___0);
				HudMenuUtility.SendMsgMenuSequenceToMainMenu(this.__f__this.m_current_sequence_type);
				this.__f__this.SendMessageNextPage(this.__f__this.m_currentPageType);
				this.__f__this.m_animation.PageInAnimation(this.__f__this.m_currentPageType, new ButtonEventAnimation.AnimationEndCallback(this.__f__this.OnNextPageAnimEndCallback));
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

		internal void __m__67()
		{
			this.__f__this.m_resourceLoadedCallback();
		}
	}

	private sealed class _WaitSendMessageCoroutine_c__Iterator3A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _i___0;

		internal ButtonEventPageControl.WaitSendMessageParam param;

		internal int _PC;

		internal object _current;

		internal ButtonEventPageControl.WaitSendMessageParam ___param;

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
				this._i___0 = 0;
				break;
			case 1u:
				this._i___0++;
				break;
			default:
				return false;
			}
			if (this._i___0 < this.param.m_waitCount)
			{
				if (!this.param.m_component.gameObject.activeInHierarchy)
				{
					this._current = null;
					this._PC = 1;
					return true;
				}
			}
			this.param.m_component.SendMessage(this.param.m_methodName, this.param.m_value, SendMessageOptions.DontRequireReceiver);
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

	private const int WAIT_SEND_MESSAGE_WAIT = 30;

	private ButtonInfoTable.PageType m_currentPageType;

	private MsgMenuSequence.SequeneceType m_current_sequence_type;

	private ButtonInfoTable m_info_table = new ButtonInfoTable();

	private GameObject m_menu_anim_obj;

	private ButtonEventResourceLoader m_resLoader;

	private ButtonEventBackButton m_backButton;

	private ButtonEventPageHistory m_pageHistory;

	private HudDisplay m_hud_display;

	private ButtonEventAnimation m_animation;

	private bool m_transform;

	private ButtonEventPageControl.ResourceLoadedCallback m_resourceLoadedCallback;

	public bool IsTransform
	{
		get
		{
			return this.m_transform;
		}
	}

	public void Initialize(ButtonEventPageControl.ResourceLoadedCallback callback)
	{
		this.m_menu_anim_obj = HudMenuUtility.GetMenuAnimUIObject();
		GameObject parent = GameObjectUtil.FindChildGameObject(this.m_menu_anim_obj, "MainMenuUI4");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(parent, "page_1");
		this.m_resLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
		HudMenuUtility.SendMsgInitMainMenuUI();
		this.m_pageHistory = new ButtonEventPageHistory();
		this.m_pageHistory.Push(ButtonInfoTable.PageType.MAIN);
		this.m_hud_display = new HudDisplay();
		this.m_animation = base.gameObject.AddComponent<ButtonEventAnimation>();
		this.m_animation.Initialize();
		this.ChangeHeaderText(ButtonInfoTable.PageType.MAIN);
		this.m_resourceLoadedCallback = callback;
	}

	public void PageBack()
	{
		if (this.m_transform)
		{
			return;
		}
		ButtonInfoTable.ButtonType buttonType = this.m_info_table.m_platformBackButtonType[(int)this.m_currentPageType];
		this.PageChange(buttonType, false, true);
	}

	public void PageChange(ButtonInfoTable.ButtonType buttonType, bool clearHistory, bool buttonPressed)
	{
		if (this.m_transform)
		{
			return;
		}
		if (this.CheckEventTopRewardListRoutletteButtonClick(buttonType))
		{
			this.SendMsgEventWindow(buttonType);
		}
		else if (this.CheckEventTopShopButtonClick(buttonType))
		{
			this.SendMsgEventWindow(buttonType);
			this.SetClickedEvent(buttonType, clearHistory);
		}
		else if (this.m_currentPageType == ButtonInfoTable.PageType.EVENT)
		{
			this.SendMsgEventWindow(buttonType);
			this.SetClickedEvent(buttonType, clearHistory);
		}
		else
		{
			this.SetClickedEvent(buttonType, clearHistory);
		}
		if (buttonPressed)
		{
			this.m_info_table.PlaySE(buttonType);
		}
	}

	private void SetClickedEvent(ButtonInfoTable.ButtonType button_type, bool clearHistory)
	{
		global::Debug.Log("SetClicedEvent " + button_type.ToString());
		MsgMenuSequence.SequeneceType sequeneceType = this.m_info_table.GetSequeneceType(button_type);
		if (sequeneceType == MsgMenuSequence.SequeneceType.MAIN)
		{
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		}
		bool flag = sequeneceType == MsgMenuSequence.SequeneceType.BACK;
		ButtonInfoTable.PageType pageType;
		ButtonInfoTable.PageType currentPageType;
		if (flag)
		{
			pageType = this.m_pageHistory.Pop();
			currentPageType = this.m_currentPageType;
		}
		else
		{
			pageType = this.m_info_table.GetPageType(button_type);
			currentPageType = this.m_currentPageType;
			if (clearHistory)
			{
				this.m_pageHistory.Clear();
			}
			else
			{
				bool flag2 = button_type != ButtonInfoTable.ButtonType.VIRTUAL_NEW_ITEM;
				if (flag2 && pageType != ButtonInfoTable.PageType.NON)
				{
					this.m_pageHistory.Push(currentPageType);
				}
			}
		}
		this.m_current_sequence_type = this.m_info_table.GetSequeneceType(pageType);
		this.m_currentPageType = pageType;
		bool flag3 = pageType == ButtonInfoTable.PageType.NON && !flag;
		if (flag3)
		{
			HudMenuUtility.SendMsgMenuSequenceToMainMenu(sequeneceType);
		}
		else
		{
			bool flag4 = currentPageType == ButtonInfoTable.PageType.MAIN && flag;
			if (flag4)
			{
				HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.BACK);
			}
			else
			{
				this.m_transform = true;
				HudMenuUtility.SetConnectAlertSimpleUI(true);
				this.ChangeHeaderText(pageType);
				ButtonInfoTable.PageType pageType2 = pageType;
				switch (pageType2)
				{
				case ButtonInfoTable.PageType.EPISODE_RANKING:
					RankingUtil.SetCurrentRankingMode(RankingUtil.RankingMode.ENDLESS);
					goto IL_15A;
				case ButtonInfoTable.PageType.QUICK:
					IL_12A:
					if (pageType2 != ButtonInfoTable.PageType.ROULETTE)
					{
						goto IL_15A;
					}
					this.SetRoulletePage(button_type);
					goto IL_15A;
				case ButtonInfoTable.PageType.QUICK_RANKING:
					RankingUtil.SetCurrentRankingMode(RankingUtil.RankingMode.QUICK);
					goto IL_15A;
				}
				goto IL_12A;
				IL_15A:
				this.SendMessageEndPage(currentPageType);
				this.m_animation.PageOutAnimation(currentPageType, pageType, new ButtonEventAnimation.AnimationEndCallback(this.OnCurrentPageAnimEndCallback));
			}
		}
	}

	private bool CheckEventTopShopButtonClick(ButtonInfoTable.ButtonType btnType)
	{
		return (btnType == ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP || btnType == ButtonInfoTable.ButtonType.RING_TO_SHOP || btnType == ButtonInfoTable.ButtonType.CHALLENGE_TO_SHOP || btnType == ButtonInfoTable.ButtonType.RAIDENERGY_TO_SHOP) && this.m_currentPageType == ButtonInfoTable.PageType.EVENT;
	}

	private bool CheckEventTopRewardListRoutletteButtonClick(ButtonInfoTable.ButtonType btnType)
	{
		return btnType == ButtonInfoTable.ButtonType.REWARDLIST_TO_CHAO_ROULETTE && this.m_currentPageType == ButtonInfoTable.PageType.EVENT;
	}

	public void SetRoulletePage(ButtonInfoTable.ButtonType button_type)
	{
		if (RouletteUtility.rouletteDefault != RouletteCategory.RAID)
		{
			if (button_type == ButtonInfoTable.ButtonType.ITEM_ROULETTE)
			{
				RouletteUtility.rouletteDefault = RouletteCategory.ITEM;
			}
			else
			{
				RouletteUtility.rouletteDefault = RouletteCategory.PREMIUM;
			}
		}
	}

	private void ChangeHeaderText(ButtonInfoTable.PageType pageType)
	{
		string[] array = new string[]
		{
			"ui_Header_main_menu",
			"ui_Header_ChaoSet",
			string.Empty,
			"ui_Header_Information",
			"ui_Header_Item",
			string.Empty,
			"ui_Header_Option",
			"ui_Header_PlayerSet",
			"ui_Header_PlayerSet",
			"ui_Header_PresentBox",
			"ui_Header_Information",
			"ui_Header_daily_battle",
			"ui_Header_Roulette_top",
			"ui_Header_Shop",
			"ui_Header_Shop",
			"ui_Header_Shop",
			"ui_Header_Shop",
			"ui_Header_MainPage2",
			"ui_Header_Item",
			"ui_Header_episodemode_score_ranking",
			"ui_Header_Item",
			"ui_Header_quickmode_score_ranking",
			"ui_Header_Item"
		};
		HudMenuUtility.SendChangeHeaderText(array[(int)pageType]);
	}

	private void OnCurrentPageAnimEndCallback()
	{
		base.StartCoroutine(this.OnCurrentPageAnimationEndCallbackCoroutine());
		switch (this.m_currentPageType)
		{
		case ButtonInfoTable.PageType.DAILY_BATTLE:
		case ButtonInfoTable.PageType.QUICK:
		case ButtonInfoTable.PageType.QUICK_RANKING:
			SoundManager.BgmChange("bgm_sys_menu", "BGM");
			break;
		case ButtonInfoTable.PageType.ROULETTE:
			break;
		case ButtonInfoTable.PageType.SHOP_RSR:
		case ButtonInfoTable.PageType.SHOP_RING:
		case ButtonInfoTable.PageType.SHOP_ENERGY:
		case ButtonInfoTable.PageType.SHOP_EVENT:
			break;
		case ButtonInfoTable.PageType.EPISODE:
		case ButtonInfoTable.PageType.EPISODE_PLAY:
		case ButtonInfoTable.PageType.EPISODE_RANKING:
		case ButtonInfoTable.PageType.PLAY_AT_EPISODE_PAGE:
			SoundManager.BgmChange("bgm_sys_menu", "BGM");
			break;
		default:
			SoundManager.BgmChange("bgm_sys_menu_v2", "BGM_menu_v2");
			break;
		}
	}

	private IEnumerator OnCurrentPageAnimationEndCallbackCoroutine()
	{
		ButtonEventPageControl._OnCurrentPageAnimationEndCallbackCoroutine_c__Iterator39 _OnCurrentPageAnimationEndCallbackCoroutine_c__Iterator = new ButtonEventPageControl._OnCurrentPageAnimationEndCallbackCoroutine_c__Iterator39();
		_OnCurrentPageAnimationEndCallbackCoroutine_c__Iterator.__f__this = this;
		return _OnCurrentPageAnimationEndCallbackCoroutine_c__Iterator;
	}

	private void OnNextPageAnimEndCallback()
	{
		this.m_transform = false;
		HudMenuUtility.SetConnectAlertSimpleUI(false);
	}

	private void SendMsgEventWindow(ButtonInfoTable.ButtonType button_type)
	{
		if (EventManager.Instance != null)
		{
			GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
			if (cameraUIObject != null)
			{
				if (EventManager.Instance.Type == EventManager.EventType.SPECIAL_STAGE)
				{
					GameObjectUtil.SendMessageFindGameObject("SpecialStageWindowUI", "OnClickEndButton", button_type, SendMessageOptions.RequireReceiver);
				}
				else if (EventManager.Instance.Type == EventManager.EventType.RAID_BOSS)
				{
					GameObjectUtil.SendMessageFindGameObject("RaidBossWindowUI", "OnClickEndButton", button_type, SendMessageOptions.RequireReceiver);
				}
			}
		}
	}

	private void SendMessageEndPage(ButtonInfoTable.PageType endPage)
	{
		if (endPage == ButtonInfoTable.PageType.ITEM || endPage == ButtonInfoTable.PageType.QUICK || endPage == ButtonInfoTable.PageType.EPISODE_PLAY || endPage == ButtonInfoTable.PageType.PLAY_AT_EPISODE_PAGE)
		{
			if (this.m_currentPageType != ButtonInfoTable.PageType.STAGE)
			{
				GameObjectUtil.SendMessageFindGameObject("ItemSet_3_UI", "OnMsgMenuBack", null, SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			ButtonInfoTable.MessageInfo pageMessageInfo = this.m_info_table.GetPageMessageInfo(endPage, false);
			this.SendMessage(pageMessageInfo, false);
		}
	}

	private void SendMessageNextPage(ButtonInfoTable.PageType nextPage)
	{
		if (nextPage == ButtonInfoTable.PageType.ITEM || nextPage == ButtonInfoTable.PageType.QUICK || nextPage == ButtonInfoTable.PageType.EPISODE_PLAY || nextPage == ButtonInfoTable.PageType.PLAY_AT_EPISODE_PAGE)
		{
			MsgMenuItemSetStart.SetMode msgMenuItemSetStartMode = ItemSetUtility.GetMsgMenuItemSetStartMode();
			MsgMenuItemSetStart value = new MsgMenuItemSetStart(msgMenuItemSetStartMode);
			GameObjectUtil.SendMessageFindGameObject("ItemSet_3_UI", "OnMsgMenuItemSetStart", value, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			ButtonInfoTable.MessageInfo pageMessageInfo = this.m_info_table.GetPageMessageInfo(nextPage, true);
			this.SendMessage(pageMessageInfo, true);
		}
	}

	private void SendMessage(ButtonInfoTable.MessageInfo msgInfo, bool waitFlag = false)
	{
		if (msgInfo != null)
		{
			if (!msgInfo.uiFlag)
			{
				GameObjectUtil.SendMessageFindGameObject(msgInfo.targetName, msgInfo.methodName, null, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				int waitCount = (!waitFlag) ? 0 : 30;
				GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
				if (cameraUIObject != null)
				{
					GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, msgInfo.targetName);
					if (gameObject != null)
					{
						Component component = gameObject.GetComponent(msgInfo.componentName);
						if (component != null)
						{
							this.WaitSendMessage(component, waitCount, msgInfo.methodName, null);
						}
					}
				}
			}
		}
	}

	public void WaitSendMessage(Component component, int waitCount, string methodName, object value)
	{
		base.StartCoroutine(this.WaitSendMessageCoroutine(new ButtonEventPageControl.WaitSendMessageParam
		{
			m_component = component,
			m_waitCount = waitCount,
			m_methodName = methodName,
			m_value = value
		}));
	}

	private IEnumerator WaitSendMessageCoroutine(ButtonEventPageControl.WaitSendMessageParam param)
	{
		ButtonEventPageControl._WaitSendMessageCoroutine_c__Iterator3A _WaitSendMessageCoroutine_c__Iterator3A = new ButtonEventPageControl._WaitSendMessageCoroutine_c__Iterator3A();
		_WaitSendMessageCoroutine_c__Iterator3A.param = param;
		_WaitSendMessageCoroutine_c__Iterator3A.___param = param;
		return _WaitSendMessageCoroutine_c__Iterator3A;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
