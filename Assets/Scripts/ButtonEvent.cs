using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
	private sealed class _LoadEventMenuResourceIfNotLoaded_c__Iterator35 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject menuObj;

		internal int _PC;

		internal object _current;

		internal GameObject ___menuObj;

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
				if (this.menuObj != null)
				{
					this.menuObj.SendMessage("OnButtonEventCallBack", null, SendMessageOptions.DontRequireReceiver);
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

	private static bool m_debugInfo = true;

	private GameObject m_menu_anim_obj;

	private ButtonEventTimer m_timer;

	private ButtonEventBackButton m_backButton;

	private ButtonEventPageControl m_pageControl;

	private ButtonInfoTable m_info_table = new ButtonInfoTable();

	private bool m_updateRanking;

	public bool IsTransform
	{
		get
		{
			return this.m_pageControl != null && this.m_pageControl.IsTransform;
		}
	}

	private void Start()
	{
		BackKeyManager.AddEventCallBack(base.gameObject);
	}

	public void OnStartMainMenu()
	{
		ButtonEvent.DebugInfoDraw("OnStartMainMenu");
		this.Initialize();
	}

	private void Initialize()
	{
		this.m_menu_anim_obj = HudMenuUtility.GetMenuAnimUIObject();
		if (FontManager.Instance != null)
		{
			FontManager.Instance.ReplaceFont();
		}
		if (AtlasManager.Instance != null)
		{
			AtlasManager.Instance.ReplaceAtlasForMenu(true);
		}
		if (this.m_menu_anim_obj != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_menu_anim_obj, "MainMenuCmnUI");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_menu_anim_obj, "MainMenuUI4");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
			}
		}
		if (this.m_timer == null)
		{
			this.m_timer = base.gameObject.AddComponent<ButtonEventTimer>();
		}
		if (this.m_backButton == null)
		{
			this.m_backButton = base.gameObject.AddComponent<ButtonEventBackButton>();
			this.m_backButton.Initialize(new ButtonEventBackButton.ButtonClickedCallback(this.ButtonClickedCallback));
		}
		if (this.m_pageControl == null)
		{
			this.m_pageControl = base.gameObject.AddComponent<ButtonEventPageControl>();
			this.m_pageControl.Initialize(new ButtonEventPageControl.ResourceLoadedCallback(this.OnPageResourceLoadedCallback));
		}
	}

	public void PageChange(ButtonInfoTable.ButtonType buttonType, bool isClearHistory, bool isButtonPressed)
	{
		if (this.m_pageControl == null)
		{
			return;
		}
		if (buttonType == ButtonInfoTable.ButtonType.UNKNOWN)
		{
			return;
		}
		if (this.m_timer == null)
		{
			return;
		}
		if (this.m_timer.IsWaiting())
		{
			return;
		}
		this.m_pageControl.PageChange(buttonType, isClearHistory, isButtonPressed);
		this.m_timer.SetWaitTimeDefault();
	}

	public void PageChangeMessage(MsgMenuButtonEvent msg)
	{
		ButtonInfoTable.ButtonType buttonType = msg.ButtonType;
		bool clearHistories = msg.m_clearHistories;
		bool isButtonPressed = false;
		this.PageChange(buttonType, clearHistories, isButtonPressed);
	}

	private void ButtonClickedCallback(ButtonInfoTable.ButtonType buttonType)
	{
		bool isClearHistory = false;
		bool isButtonPressed = true;
		this.PageChange(buttonType, isClearHistory, isButtonPressed);
	}

	private void OnClickPlatformBackButtonEvent()
	{
		if (this.m_timer == null)
		{
			return;
		}
		if (this.m_timer.IsWaiting())
		{
			return;
		}
		if (this.m_pageControl == null)
		{
			return;
		}
		this.m_pageControl.PageBack();
		this.m_timer.SetWaitTimeDefault();
	}

	private void OnPageResourceLoadedCallback()
	{
		if (FontManager.Instance != null)
		{
			FontManager.Instance.ReplaceFont();
		}
		if (AtlasManager.Instance != null)
		{
			AtlasManager.Instance.ReplaceAtlasForMenu(true);
		}
		if (this.m_backButton != null)
		{
			for (uint num = 0u; num < 49u; num += 1u)
			{
				this.m_backButton.SetupBackButton((ButtonInfoTable.ButtonType)num);
			}
		}
	}

	private void OnShopBackButtonClicked()
	{
		GameObjectUtil.SendMessageFindGameObject("ShopUI2", "OnShopBackButtonClicked", null, SendMessageOptions.DontRequireReceiver);
		HudMenuUtility.SendEnableShopButton(true);
	}

	private void OnOptionBackButtonClicked()
	{
		GameObjectUtil.SendMessageFindGameObject("OptionUI", "OnEndOptionUI", null, SendMessageOptions.DontRequireReceiver);
		if (this.m_updateRanking)
		{
			HudMenuUtility.SendMsgUpdateRanking();
			this.m_updateRanking = false;
		}
	}

	private void OnUpdateRankingFlag()
	{
		this.m_updateRanking = true;
	}

	private void OnMenuEventClicked(GameObject menuObj)
	{
		base.StartCoroutine(this.LoadEventMenuResourceIfNotLoaded(menuObj));
	}

	private IEnumerator LoadEventMenuResourceIfNotLoaded(GameObject menuObj)
	{
		ButtonEvent._LoadEventMenuResourceIfNotLoaded_c__Iterator35 _LoadEventMenuResourceIfNotLoaded_c__Iterator = new ButtonEvent._LoadEventMenuResourceIfNotLoaded_c__Iterator35();
		_LoadEventMenuResourceIfNotLoaded_c__Iterator.menuObj = menuObj;
		_LoadEventMenuResourceIfNotLoaded_c__Iterator.___menuObj = menuObj;
		return _LoadEventMenuResourceIfNotLoaded_c__Iterator;
	}

	public static void DebugInfoDraw(string msg)
	{
	}
}
