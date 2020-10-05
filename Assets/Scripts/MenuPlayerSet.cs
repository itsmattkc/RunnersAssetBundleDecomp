using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class MenuPlayerSet : MonoBehaviour
{
	private enum TutorialMode
	{
		Idle,
		WaitWindow,
		WaitClickLevelUpButton,
		EndClickLevelUpButton
	}

	private enum State
	{
		NOT_SETUP,
		SETUPING,
		SETUPED
	}

	private sealed class _JumpCharacterPage_c__Iterator44 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal MenuPlayerSetContents _contetns___0;

		internal int pageIndex;

		internal int _PC;

		internal object _current;

		internal int ___pageIndex;

		internal MenuPlayerSet __f__this;

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
			if (this.__f__this.m_state != MenuPlayerSet.State.SETUPED)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this._contetns___0 = this.__f__this.gameObject.GetComponent<MenuPlayerSetContents>();
			if (this._contetns___0 != null)
			{
				this._contetns___0.JumpCharacterPage(this.pageIndex);
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

	private sealed class _Setup_c__Iterator45 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal UIRectItemStorage _itemStoragePage___0;

		internal int _charaCount___1;

		internal GameObject _gripRootObject___2;

		internal UIRectItemStorage _itemStorageShotCut___3;

		internal int _charaCount___4;

		internal MenuPlayerSetContents _contents___5;

		internal bool _isEndSetup___6;

		internal MenuPlayerSetGripL _gripL___7;

		internal MenuPlayerSetGripR _gripR___8;

		internal int _PC;

		internal object _current;

		internal MenuPlayerSet __f__this;

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
				this.__f__this.m_state = MenuPlayerSet.State.SETUPING;
				this._itemStoragePage___0 = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.__f__this.gameObject, "grid_slot");
				if (this._itemStoragePage___0 != null)
				{
					this._charaCount___1 = MenuPlayerSetUtil.GetOpenedCharaCount();
					this._itemStoragePage___0.maxRows = 1;
					this._itemStoragePage___0.maxColumns = this._charaCount___1;
					this._itemStoragePage___0.maxItemCount = this._charaCount___1;
					this._itemStoragePage___0.m_activeType = UIRectItemStorage.ActiveType.NOT_ACTTIVE;
				}
				this._gripRootObject___2 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "player_set_pager_alpha_clip");
				if (this._gripRootObject___2 != null)
				{
					this._itemStorageShotCut___3 = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this._gripRootObject___2, "slot");
					if (this._itemStorageShotCut___3 != null)
					{
						this._charaCount___4 = MenuPlayerSetUtil.GetOpenedCharaCount();
						this._itemStorageShotCut___3.maxRows = 1;
						this._itemStorageShotCut___3.maxColumns = this._charaCount___4;
						this._itemStorageShotCut___3.maxItemCount = this._charaCount___4;
					}
				}
				this.__f__this.m_partsList.Add(this.__f__this.gameObject.AddComponent<MenuPlayerSetBG>());
				this.__f__this.m_partsList.Add(this.__f__this.gameObject.AddComponent<MenuPlayerSetGrip>());
				this.__f__this.m_partsList.Add(this.__f__this.gameObject.AddComponent<MenuPlayerSetGripL>());
				this.__f__this.m_partsList.Add(this.__f__this.gameObject.AddComponent<MenuPlayerSetGripR>());
				this.__f__this.m_partsList.Add(this.__f__this.gameObject.AddComponent<MenuPlayerSetReturnButton>());
				this.__f__this.m_partsList.Add(this.__f__this.gameObject.AddComponent<MenuPlayerSetContents>());
				this._contents___5 = this.__f__this.gameObject.GetComponent<MenuPlayerSetContents>();
				if (!(this._contents___5 != null))
				{
					goto IL_266;
				}
				this._contents___5.SetCallback(new MenuPlayerSetContents.PageChangeCallback(this.__f__this.PageChangeCallback));
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._isEndSetup___6 = this._contents___5.IsEndSetup;
			if (!this._isEndSetup___6)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			IL_266:
			this._gripL___7 = this.__f__this.gameObject.GetComponent<MenuPlayerSetGripL>();
			if (this._gripL___7 != null)
			{
				this._gripL___7.SetCallback(new MenuPlayerSetGripL.ButtonClickCallback(this.__f__this.OnClickLeftScrollButton));
			}
			this._gripR___8 = this.__f__this.gameObject.GetComponent<MenuPlayerSetGripR>();
			if (this._gripR___8 != null)
			{
				this._gripR___8.SetCallback(new MenuPlayerSetGripR.ButtonClickCallback(this.__f__this.OnClickRightScrollButton));
			}
			this.__f__this.m_state = MenuPlayerSet.State.SETUPED;
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

	private sealed class _ReSetUp_c__Iterator46 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal UIRectItemStorage _itemStoragePage___0;

		internal int _charaCount___1;

		internal GameObject _gripRootObject___2;

		internal UIRectItemStorage _itemStorageShotCut___3;

		internal MenuPlayerSetContents _contents___4;

		internal bool _isEndSetup___5;

		internal MenuPlayerSetContents _contents___6;

		internal int _PC;

		internal object _current;

		internal MenuPlayerSet __f__this;

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
				if (this.__f__this.m_state != MenuPlayerSet.State.SETUPED)
				{
					goto IL_1FD;
				}
				this._itemStoragePage___0 = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.__f__this.gameObject, "grid_slot");
				if (!(this._itemStoragePage___0 != null))
				{
					goto IL_1CB;
				}
				this._charaCount___1 = MenuPlayerSetUtil.GetOpenedCharaCount();
				if (this._itemStoragePage___0.maxColumns == this._charaCount___1)
				{
					goto IL_1CB;
				}
				this.__f__this.m_state = MenuPlayerSet.State.SETUPING;
				this._itemStoragePage___0.maxColumns = this._charaCount___1;
				this._itemStoragePage___0.maxItemCount = this._charaCount___1;
				this._itemStoragePage___0.Restart();
				this._gripRootObject___2 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "player_set_pager_alpha_clip");
				if (this._gripRootObject___2 != null)
				{
					this._itemStorageShotCut___3 = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this._gripRootObject___2, "slot");
					if (this._itemStorageShotCut___3 != null)
					{
						this._itemStorageShotCut___3.maxColumns = this._charaCount___1;
						this._itemStorageShotCut___3.maxItemCount = this._charaCount___1;
						this._itemStorageShotCut___3.Restart();
					}
				}
				this._contents___4 = this.__f__this.gameObject.GetComponent<MenuPlayerSetContents>();
				if (!(this._contents___4 != null))
				{
					goto IL_1BF;
				}
				this._contents___4.Reset();
				this._contents___4.SetCallback(new MenuPlayerSetContents.PageChangeCallback(this.__f__this.PageChangeCallback));
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._isEndSetup___5 = this._contents___4.IsEndSetup;
			if (!this._isEndSetup___5)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			IL_1BF:
			this.__f__this.m_state = MenuPlayerSet.State.SETUPED;
			IL_1CB:
			this._contents___6 = this.__f__this.gameObject.GetComponent<MenuPlayerSetContents>();
			if (this._contents___6 != null)
			{
				this._contents___6.UpdateRibbon();
			}
			IL_1FD:
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

	private MenuPlayerSet.TutorialMode m_tutorialMode;

	private List<MenuPlayerSetPartsBase> m_partsList;

	private SendApollo m_sendApollo;

	private MenuPlayerSet.State m_state;

	private int m_currentPage;

	public bool SetUpped
	{
		get
		{
			return this.m_state == MenuPlayerSet.State.SETUPED;
		}
	}

	public void StartMainCharacter()
	{
		if (MenuPlayerSetUtil.IsMarkCharaPage())
		{
			int pageIndexFromCharaType = MenuPlayerSetUtil.GetPageIndexFromCharaType(MenuPlayerSetUtil.MarkCharaType);
			base.StartCoroutine(this.JumpCharacterPage(pageIndexFromCharaType));
			MenuPlayerSetUtil.ResetMarkCharaPage();
		}
		else
		{
			CharaType mainChara = SaveDataManager.Instance.PlayerData.MainChara;
			int pageIndexFromCharaType2 = MenuPlayerSetUtil.GetPageIndexFromCharaType(mainChara);
			base.StartCoroutine(this.JumpCharacterPage(pageIndexFromCharaType2));
		}
		if (HudMenuUtility.IsTutorial_CharaLevelUp())
		{
			this.PrepareTutorialLevelUp();
		}
	}

	public void StartSubCharacter()
	{
		if (MenuPlayerSetUtil.IsMarkCharaPage())
		{
			int pageIndexFromCharaType = MenuPlayerSetUtil.GetPageIndexFromCharaType(MenuPlayerSetUtil.MarkCharaType);
			base.StartCoroutine(this.JumpCharacterPage(pageIndexFromCharaType));
			MenuPlayerSetUtil.ResetMarkCharaPage();
		}
		else
		{
			CharaType subChara = SaveDataManager.Instance.PlayerData.SubChara;
			int pageIndexFromCharaType2 = MenuPlayerSetUtil.GetPageIndexFromCharaType(subChara);
			base.StartCoroutine(this.JumpCharacterPage(pageIndexFromCharaType2));
		}
		if (HudMenuUtility.IsTutorial_CharaLevelUp())
		{
			this.PrepareTutorialLevelUp();
		}
	}

	public void StartCharacter(CharaType type)
	{
		int pageIndexFromCharaType = MenuPlayerSetUtil.GetPageIndexFromCharaType(type);
		base.StartCoroutine(this.JumpCharacterPage(pageIndexFromCharaType));
		if (HudMenuUtility.IsTutorial_CharaLevelUp())
		{
			this.PrepareTutorialLevelUp();
		}
	}

	private IEnumerator JumpCharacterPage(int pageIndex)
	{
		MenuPlayerSet._JumpCharacterPage_c__Iterator44 _JumpCharacterPage_c__Iterator = new MenuPlayerSet._JumpCharacterPage_c__Iterator44();
		_JumpCharacterPage_c__Iterator.pageIndex = pageIndex;
		_JumpCharacterPage_c__Iterator.___pageIndex = pageIndex;
		_JumpCharacterPage_c__Iterator.__f__this = this;
		return _JumpCharacterPage_c__Iterator;
	}

	private void PrepareTutorialLevelUp()
	{
		HudMenuUtility.SetConnectAlertSimpleUI(true);
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "chara_level_up_explan",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextUtility.GetCommonText("MainMenu", "chara_level_up_explan_caption"),
			message = TextUtility.GetCommonText("MainMenu", "chara_level_up_explan"),
			finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowCharaLevelUpCloseCallback)
		});
		string[] value = new string[1];
		SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP7, ref value);
		this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_START, value);
		this.m_tutorialMode = MenuPlayerSet.TutorialMode.WaitWindow;
	}

	private void GeneralWindowCharaLevelUpCloseCallback()
	{
		HudMenuUtility.SetConnectAlertSimpleUI(false);
		TutorialCursor.StartTutorialCursor(TutorialCursor.Type.CHARASELECT_LEVEL_UP);
		this.m_tutorialMode = MenuPlayerSet.TutorialMode.WaitClickLevelUpButton;
	}

	private void OnClickedLevelUpButton()
	{
		TutorialCursor.DestroyTutorialCursor();
		HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.CHARA_LEVEL_UP_EXPLAINED);
		string[] value = new string[1];
		SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP7, ref value);
		this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value);
		this.m_tutorialMode = MenuPlayerSet.TutorialMode.EndClickLevelUpButton;
	}

	private void Start()
	{
		this.m_partsList = new List<MenuPlayerSetPartsBase>();
		base.StartCoroutine(this.Setup());
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.ReSetUp());
	}

	private void Update()
	{
		switch (this.m_tutorialMode)
		{
		case MenuPlayerSet.TutorialMode.WaitWindow:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd() && GeneralWindow.IsCreated("chara_level_up_explan") && GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
			}
			break;
		case MenuPlayerSet.TutorialMode.EndClickLevelUpButton:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
				this.m_tutorialMode = MenuPlayerSet.TutorialMode.Idle;
			}
			break;
		}
	}

	private IEnumerator Setup()
	{
		MenuPlayerSet._Setup_c__Iterator45 _Setup_c__Iterator = new MenuPlayerSet._Setup_c__Iterator45();
		_Setup_c__Iterator.__f__this = this;
		return _Setup_c__Iterator;
	}

	private IEnumerator ReSetUp()
	{
		MenuPlayerSet._ReSetUp_c__Iterator46 _ReSetUp_c__Iterator = new MenuPlayerSet._ReSetUp_c__Iterator46();
		_ReSetUp_c__Iterator.__f__this = this;
		return _ReSetUp_c__Iterator;
	}

	private void PageChangeCallback(int pageIndex)
	{
		MenuPlayerSetGripL component = base.gameObject.GetComponent<MenuPlayerSetGripL>();
		if (component != null)
		{
			if (pageIndex <= 0)
			{
				component.SetDisplayFlag(false);
			}
			else
			{
				component.SetDisplayFlag(true);
			}
		}
		MenuPlayerSetGripR component2 = base.gameObject.GetComponent<MenuPlayerSetGripR>();
		if (component2 != null)
		{
			int openedCharaCount = MenuPlayerSetUtil.GetOpenedCharaCount();
			if (pageIndex >= openedCharaCount - 1)
			{
				component2.SetDisplayFlag(false);
			}
			else
			{
				component2.SetDisplayFlag(true);
			}
		}
		this.m_currentPage = pageIndex;
	}

	private void OnClickLeftScrollButton()
	{
		this.m_currentPage--;
		if (this.m_currentPage < 0)
		{
			this.m_currentPage = 0;
		}
		base.StartCoroutine(this.JumpCharacterPage(this.m_currentPage));
	}

	private void OnClickRightScrollButton()
	{
		this.m_currentPage++;
		int openedCharaCount = MenuPlayerSetUtil.GetOpenedCharaCount();
		if (this.m_currentPage >= openedCharaCount - 1)
		{
			this.m_currentPage = openedCharaCount - 1;
		}
		base.StartCoroutine(this.JumpCharacterPage(this.m_currentPage));
	}
}
