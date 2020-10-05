using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class MenuPlayerSetContents : MenuPlayerSetPartsBase
{
	public delegate void PageChangeCallback(int pageIndex);

	private sealed class _SetupCoroutine_c__Iterator49 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _playerSetRoot___0;

		internal GameObject _pageRootParent___1;

		internal List<GameObject> _pageList___2;

		internal int _openedCharaCount___3;

		internal int _index___4;

		internal GameObject _pageObject___5;

		internal MenuPlayerSetCharaPage _page___6;

		internal CharaType _charaType___7;

		internal int _index___8;

		internal GameObject _pageObject___9;

		internal GameObject _gripRootObject___10;

		internal UIScrollBar _scrollBar___11;

		internal int _pageCount___12;

		internal int _PC;

		internal object _current;

		internal MenuPlayerSetContents __f__this;

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
				this.__f__this.m_isEndSetup = false;
				this._playerSetRoot___0 = MenuPlayerSetUtil.GetPlayerSetRoot();
				this._pageRootParent___1 = GameObjectUtil.FindChildGameObject(this._playerSetRoot___0, "grid_slot");
				this._pageList___2 = null;
				this._openedCharaCount___3 = MenuPlayerSetUtil.GetOpenedCharaCount();
				break;
			case 1u:
				break;
			case 2u:
				IL_160:
				if (this._page___6.IsEndSetUp)
				{
					MenuPlayerSetUtil.ActivateCharaPageObjects(this._page___6.gameObject, false);
					this.__f__this.m_charaPage[this._index___4] = this._page___6;
					goto IL_199;
				}
				this._current = null;
				this._PC = 2;
				return true;
			case 3u:
				this._index___8 = 0;
				while (this._index___8 < this._pageList___2.Count)
				{
					this._pageObject___9 = this._pageList___2[this._index___8];
					if (!(this._pageObject___9 == null))
					{
						MenuPlayerSetUtil.ActivateCharaPageObjects(this.__f__this.m_charaPage[this._index___8].gameObject, false);
					}
					this._index___8++;
				}
				this._gripRootObject___10 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "player_set_pager_alpha_clip");
				if (!(this._gripRootObject___10 != null))
				{
					goto IL_30B;
				}
				this.__f__this.m_shortCutMenu = this._gripRootObject___10.AddComponent<MenuPlayerSetShortCutMenu>();
				if (this.__f__this.m_shortCutMenu != null)
				{
					this.__f__this.m_shortCutMenu.Setup(new MenuPlayerSetShortCutMenu.ShortCutCallback(this.__f__this.ShortCutButtonClickedCallback));
					goto IL_2C3;
				}
				goto IL_2C3;
			case 4u:
				goto IL_2C3;
			default:
				return false;
			}
			this._pageList___2 = GameObjectUtil.FindChildGameObjects(this._pageRootParent___1, "ui_player_set_2_cell(Clone)");
			if (this._pageList___2 != null)
			{
				if (this._pageList___2.Count < this._openedCharaCount___3)
				{
					this._current = null;
					this._PC = 1;
					return true;
				}
			}
			this._index___4 = 0;
			goto IL_1A7;
			IL_199:
			this._index___4++;
			IL_1A7:
			if (this._index___4 >= this._pageList___2.Count)
			{
				this._current = null;
				this._PC = 3;
				return true;
			}
			this._pageObject___5 = this._pageList___2[this._index___4];
			if (this._pageObject___5 == null)
			{
				goto IL_199;
			}
			this._page___6 = this._pageObject___5.AddComponent<MenuPlayerSetCharaPage>();
			this._charaType___7 = MenuPlayerSetUtil.GetCharaTypeFromPageIndex(this._index___4);
			if (this._charaType___7 == CharaType.UNKNOWN)
			{
				goto IL_199;
			}
			this._page___6.Setup(this._pageObject___5, this._charaType___7);
			goto IL_160;
			IL_2C3:
			if (!(this.__f__this.m_shortCutMenu != null) || !this.__f__this.m_shortCutMenu.IsEndSetup)
			{
				this._current = null;
				this._PC = 4;
				return true;
			}
			IL_30B:
			if (this.__f__this.m_scrollBar == null)
			{
				this.__f__this.m_scrollBar = this.__f__this.gameObject.AddComponent<HudScrollBar>();
			}
			this._scrollBar___11 = GameObjectUtil.FindChildGameObjectComponent<UIScrollBar>(this.__f__this.gameObject, "player_set_SB");
			this._pageCount___12 = this._pageList___2.Count;
			this.__f__this.m_scrollBar.Setup(this._scrollBar___11, this._pageCount___12);
			this.__f__this.m_scrollBar.SetPageChangeCallback(new HudScrollBar.PageChangeCallback(this.__f__this.ChangePageCallback));
			this.__f__this.m_isEndSetup = true;
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

	private sealed class _JumpCharaPageCoroutine_c__Iterator4A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int pageIndex;

		internal int _PC;

		internal object _current;

		internal int ___pageIndex;

		internal MenuPlayerSetContents __f__this;

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
			if (!this.__f__this.m_isEndSetup)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.__f__this.m_scrollBar != null)
			{
				this.__f__this.m_scrollBar.PageJump(this.pageIndex, true);
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

	private MenuPlayerSetCharaPage[] m_charaPage = new MenuPlayerSetCharaPage[29];

	private MenuPlayerSetShortCutMenu m_shortCutMenu;

	private HudScrollBar m_scrollBar;

	private MenuPlayerSetContents.PageChangeCallback m_callback;

	private bool m_isEndSetup;

	public bool IsEndSetup
	{
		get
		{
			return this.m_isEndSetup;
		}
		private set
		{
		}
	}

	public MenuPlayerSetContents() : base("player_set_contents")
	{
	}

	public void SetCallback(MenuPlayerSetContents.PageChangeCallback callback)
	{
		this.m_callback = callback;
	}

	protected override void OnSetup()
	{
		base.StartCoroutine(this.SetupCoroutine());
	}

	private void OnDestroy()
	{
		MenuPlayerSetCharaPage[] charaPage = this.m_charaPage;
		for (int i = 0; i < charaPage.Length; i++)
		{
			MenuPlayerSetCharaPage menuPlayerSetCharaPage = charaPage[i];
			if (!(menuPlayerSetCharaPage == null))
			{
				UnityEngine.Object.Destroy(menuPlayerSetCharaPage);
			}
		}
		if (this.m_scrollBar != null)
		{
			this.m_scrollBar.SetPageChangeCallback(null);
			UnityEngine.Object.Destroy(this.m_scrollBar);
		}
	}

	private IEnumerator SetupCoroutine()
	{
		MenuPlayerSetContents._SetupCoroutine_c__Iterator49 _SetupCoroutine_c__Iterator = new MenuPlayerSetContents._SetupCoroutine_c__Iterator49();
		_SetupCoroutine_c__Iterator.__f__this = this;
		return _SetupCoroutine_c__Iterator;
	}

	public void JumpCharacterPage(int pageIndex)
	{
		base.StartCoroutine(this.JumpCharaPageCoroutine(pageIndex));
	}

	public void ChangeMainChara(CharaType newCharaType)
	{
		PlayerData playerData = SaveDataManager.Instance.PlayerData;
		CharaType mainChara = playerData.MainChara;
		if (mainChara == newCharaType)
		{
			return;
		}
		CharaType subChara = playerData.SubChara;
		if (subChara == newCharaType)
		{
			CharaType mainChara2 = playerData.MainChara;
			playerData.MainChara = playerData.SubChara;
			playerData.SubChara = mainChara2;
		}
		else
		{
			if (playerData.SubChara == CharaType.UNKNOWN)
			{
				playerData.SubChara = playerData.MainChara;
			}
			playerData.MainChara = newCharaType;
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			int mainCharaId = -1;
			ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(playerData.MainChara);
			if (serverCharacterState != null)
			{
				mainCharaId = serverCharacterState.Id;
			}
			int subCharaId = -1;
			ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(playerData.SubChara);
			if (serverCharacterState2 != null)
			{
				subCharaId = serverCharacterState2.Id;
			}
			loggedInServerInterface.RequestServerChangeCharacter(mainCharaId, subCharaId, base.gameObject);
		}
		this.UpdateRibbon();
	}

	public void ChangeSubChara(CharaType newCharaType)
	{
		PlayerData playerData = SaveDataManager.Instance.PlayerData;
		CharaType subChara = playerData.SubChara;
		if (subChara == newCharaType)
		{
			return;
		}
		CharaType mainChara = playerData.MainChara;
		if (mainChara == newCharaType)
		{
			CharaType subChara2 = playerData.SubChara;
			if (subChara2 == CharaType.UNKNOWN)
			{
				GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PlayerSet", "ui_Lbl_player_config").text;
				string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "change_chara_error").text;
				info.caption = text;
				info.message = text2;
				info.anchor_path = "Camera/menu_Anim/PlayerSet_2_UI/Anchor_5_MC";
				info.buttonType = GeneralWindow.ButtonType.Ok;
				info.isPlayErrorSe = true;
				GeneralWindow.Create(info);
				return;
			}
			playerData.SubChara = playerData.MainChara;
			playerData.MainChara = subChara2;
		}
		playerData.SubChara = newCharaType;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			int mainCharaId = -1;
			ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(playerData.MainChara);
			if (serverCharacterState != null)
			{
				mainCharaId = serverCharacterState.Id;
			}
			int subCharaId = -1;
			ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(playerData.SubChara);
			if (serverCharacterState2 != null)
			{
				subCharaId = serverCharacterState2.Id;
			}
			loggedInServerInterface.RequestServerChangeCharacter(mainCharaId, subCharaId, base.gameObject);
		}
		this.UpdateRibbon();
	}

	public void UnlockedChara(CharaType unlockedChara)
	{
		if (MenuPlayerSetUtil.GetPlayableCharaCount() == 2)
		{
			this.ChangeSubChara(unlockedChara);
		}
		else
		{
			int pageIndexFromCharaType = MenuPlayerSetUtil.GetPageIndexFromCharaType(unlockedChara);
			MenuPlayerSetCharaPage menuPlayerSetCharaPage = this.m_charaPage[pageIndexFromCharaType];
			if (menuPlayerSetCharaPage != null)
			{
				menuPlayerSetCharaPage.OnSelected();
			}
		}
		if (this.m_shortCutMenu != null)
		{
			this.m_shortCutMenu.UnclockCharacter(unlockedChara);
		}
		AchievementManager.RequestUpdate();
	}

	protected override void OnPlayStart()
	{
	}

	protected override void OnPlayEnd()
	{
	}

	protected override void OnUpdate(float deltaTime)
	{
		if (GeneralWindow.IsOkButtonPressed)
		{
			GeneralWindow.Close();
		}
	}

	private IEnumerator JumpCharaPageCoroutine(int pageIndex)
	{
		MenuPlayerSetContents._JumpCharaPageCoroutine_c__Iterator4A _JumpCharaPageCoroutine_c__Iterator4A = new MenuPlayerSetContents._JumpCharaPageCoroutine_c__Iterator4A();
		_JumpCharaPageCoroutine_c__Iterator4A.pageIndex = pageIndex;
		_JumpCharaPageCoroutine_c__Iterator4A.___pageIndex = pageIndex;
		_JumpCharaPageCoroutine_c__Iterator4A.__f__this = this;
		return _JumpCharaPageCoroutine_c__Iterator4A;
	}

	public void UpdateRibbon()
	{
		for (int i = 0; i < 29; i++)
		{
			MenuPlayerSetCharaPage menuPlayerSetCharaPage = this.m_charaPage[i];
			if (!(menuPlayerSetCharaPage == null))
			{
				menuPlayerSetCharaPage.UpdateRibbon();
			}
		}
	}

	private void ShortCutButtonClickedCallback(CharaType charaType)
	{
		if (this.m_scrollBar != null)
		{
			int pageIndexFromCharaType = MenuPlayerSetUtil.GetPageIndexFromCharaType(charaType);
			this.m_scrollBar.PageJump(pageIndexFromCharaType, false);
		}
	}

	private void ChangePageCallback(int prevPage, int currentPage)
	{
		int openedCharaCount = MenuPlayerSetUtil.GetOpenedCharaCount();
		int num = Mathf.Clamp(prevPage - 1, 0, openedCharaCount - 1);
		int num2 = Mathf.Clamp(prevPage + 1, 0, openedCharaCount - 1);
		MenuPlayerSetUtil.ActivateCharaPageObjects(this.m_charaPage[num].gameObject, false);
		MenuPlayerSetUtil.ActivateCharaPageObjects(this.m_charaPage[prevPage].gameObject, false);
		MenuPlayerSetUtil.ActivateCharaPageObjects(this.m_charaPage[num2].gameObject, false);
		int num3 = Mathf.Clamp(currentPage - 1, 0, openedCharaCount - 1);
		int num4 = Mathf.Clamp(currentPage + 1, 0, openedCharaCount - 1);
		MenuPlayerSetUtil.ActivateCharaPageObjects(this.m_charaPage[num3].gameObject, true);
		MenuPlayerSetUtil.ActivateCharaPageObjects(this.m_charaPage[currentPage].gameObject, true);
		MenuPlayerSetUtil.ActivateCharaPageObjects(this.m_charaPage[num4].gameObject, true);
		if (this.m_shortCutMenu != null)
		{
			CharaType charaTypeFromPageIndex = MenuPlayerSetUtil.GetCharaTypeFromPageIndex(currentPage);
			this.m_shortCutMenu.SetActiveCharacter(charaTypeFromPageIndex, false);
		}
		if (this.m_callback != null)
		{
			this.m_callback(currentPage);
		}
		SoundManager.SePlay("sys_page_skip", "SE");
	}
}
