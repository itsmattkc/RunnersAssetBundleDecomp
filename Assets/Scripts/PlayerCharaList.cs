using AnimationOrTween;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharaList : MonoBehaviour
{
	public enum SET_CHARA_MODE
	{
		MAIN,
		SUB,
		CHANGE
	}

	private const int CHARA_DRAW_MAX = 4;

	private const float CHANGE_BTN_DELAY = 1.5f;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIRectItemStorage m_storage;

	[SerializeField]
	private GameObject m_charaDeckObject;

	[SerializeField]
	private GameObject m_chaoDeckObject;

	[SerializeField]
	private List<GameObject> m_gameObjectList;

	private static bool s_isActive;

	private static PlayerCharaList s_instance;

	private ServerPlayerState.CHARA_SORT m_sortType;

	private int m_sortOffset;

	private int m_page;

	private int m_pageMax;

	private float m_changeDelay;

	private UIImageButton m_changeBtn;

	private List<ui_player_set_scroll> m_charaObjectList;

	private Dictionary<CharaType, ServerCharacterState> m_charaStateList;

	private int m_currentDeck;

	private List<DeckUtil.DeckSet> m_deckList;

	private bool m_isEnd;

	private DeckUtil.DeckSet m_oldDeckSet;

	private bool m_tutorial;

	private bool m_pickup;

	private int m_reqDeck;

	private CharaType m_reqCharaMain = CharaType.UNKNOWN;

	private CharaType m_reqCharaSub = CharaType.UNKNOWN;

	private int m_reqChaoMain = -1;

	private int m_reqChaoSub = -1;

	private ServerPlayerState.CHARA_SORT sortType
	{
		get
		{
			return this.m_sortType;
		}
	}

	private int sortOffset
	{
		get
		{
			return this.m_sortOffset;
		}
	}

	private int currentDeck
	{
		get
		{
			return this.m_currentDeck;
		}
	}

	public bool isTutorial
	{
		get
		{
			return this.m_tutorial;
		}
	}

	public void SetTutorialEnd()
	{
		BackKeyManager.InvalidFlag = false;
		this.m_tutorial = false;
	}

	private void Update()
	{
		if (this.m_changeDelay > 0f)
		{
			this.m_changeDelay -= Time.deltaTime;
			if (this.m_changeDelay <= 0f)
			{
				this.m_changeDelay = 0f;
				if (this.m_changeBtn != null)
				{
					if (this.CheckSetMode(PlayerCharaList.SET_CHARA_MODE.CHANGE, CharaType.UNKNOWN))
					{
						this.m_changeBtn.isEnabled = true;
					}
					else
					{
						this.m_changeBtn.isEnabled = false;
						this.m_changeDelay = -1f;
					}
				}
			}
		}
		if (!this.m_pickup && this.m_pageMax > 0 && GeneralWindow.IsCreated("ShowNoCommunicationPicupCharaList") && GeneralWindow.IsOkButtonPressed)
		{
			if (GeneralUtil.IsNetwork())
			{
				RouletteManager.Instance.RequestPicupCharaList(false);
				this.m_pickup = true;
			}
			else
			{
				GeneralUtil.ShowNoCommunication("ShowNoCommunicationPicupCharaList");
			}
		}
	}

	public void Setup()
	{
		base.gameObject.SetActive(true);
		this.m_pickup = false;
		this.m_tutorial = HudMenuUtility.IsTutorial_CharaLevelUp();
		if (this.m_tutorial)
		{
			BackKeyManager.InvalidFlag = true;
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.CHARASELECT_CHARA);
			HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.CHARA_LEVEL_UP_EXPLAINED);
		}
		if (RouletteManager.Instance != null && !RouletteManager.Instance.IsRequestPicupCharaList() && this.m_pageMax <= 0)
		{
			if (GeneralUtil.IsNetwork())
			{
				RouletteManager.Instance.RequestPicupCharaList(false);
				this.m_pickup = true;
			}
			else
			{
				GeneralUtil.ShowNoCommunication("ShowNoCommunicationPicupCharaList");
				this.m_pickup = false;
			}
		}
		this.m_currentDeck = DeckUtil.GetDeckCurrentStockIndex();
		this.m_deckList = DeckUtil.GetDeckList();
		global::Debug.Log(string.Concat(new object[]
		{
			"GetCurrentDeck ",
			this.m_deckList.Count,
			"   ",
			this.m_currentDeck
		}));
		this.m_isEnd = false;
		this.m_page = 0;
		this.m_pageMax = 0;
		this.m_changeDelay = 0f;
		this.SetParam(true);
		this.SetObject(true);
		if (this.m_animation != null)
		{
			ActiveAnimation.Play(this.m_animation, "ui_mm_player_set_2_intro_Anim", Direction.Forward);
		}
		if (this.m_gameObjectList != null && this.m_gameObjectList.Count > 0)
		{
			foreach (GameObject current in this.m_gameObjectList)
			{
				if (current != null)
				{
					current.SetActive(true);
				}
			}
		}
		this.SetSort(ServerPlayerState.CHARA_SORT.TEAM_ATTR);
	}

	public bool UpdateView(bool rest = false)
	{
		this.m_currentDeck = DeckUtil.GetDeckCurrentStockIndex();
		this.m_deckList = DeckUtil.GetDeckList();
		this.SetParam(rest);
		this.SetObject(rest);
		return true;
	}

	public bool CheckSetMode(PlayerCharaList.SET_CHARA_MODE setMode, CharaType setCharaType = CharaType.UNKNOWN)
	{
		bool result = false;
		int deckCurrentStockIndex = DeckUtil.GetDeckCurrentStockIndex();
		List<DeckUtil.DeckSet> deckList = DeckUtil.GetDeckList();
		DeckUtil.DeckSet deckSet = deckList[deckCurrentStockIndex];
		switch (setMode)
		{
		case PlayerCharaList.SET_CHARA_MODE.MAIN:
			if (deckSet.charaMain != setCharaType)
			{
				result = true;
			}
			break;
		case PlayerCharaList.SET_CHARA_MODE.SUB:
			if (deckSet.charaSub != setCharaType)
			{
				result = (deckSet.charaMain != setCharaType || deckSet.charaSub != CharaType.UNKNOWN);
			}
			break;
		case PlayerCharaList.SET_CHARA_MODE.CHANGE:
			if (deckSet.charaSub != CharaType.UNKNOWN && deckSet.charaMain != CharaType.UNKNOWN)
			{
				result = true;
			}
			break;
		}
		return result;
	}

	public bool SetChara(PlayerCharaList.SET_CHARA_MODE setMode, CharaType setCharaType = CharaType.UNKNOWN)
	{
		bool flag = false;
		if (GeneralUtil.IsNetwork())
		{
			if (setCharaType != CharaType.UNKNOWN || setMode == PlayerCharaList.SET_CHARA_MODE.CHANGE)
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					int deckCurrentStockIndex = DeckUtil.GetDeckCurrentStockIndex();
					List<DeckUtil.DeckSet> deckList = DeckUtil.GetDeckList();
					DeckUtil.DeckSet deckSet = deckList[deckCurrentStockIndex];
					int num = -1;
					int num2 = -1;
					int num3 = -1;
					int num4 = -1;
					int num5 = -1;
					ServerCharacterState serverCharacterState = null;
					ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(deckSet.charaMain);
					ServerCharacterState serverCharacterState3 = ServerInterface.PlayerState.CharacterState(deckSet.charaSub);
					if (setCharaType != CharaType.UNKNOWN)
					{
						serverCharacterState = ServerInterface.PlayerState.CharacterState(setCharaType);
					}
					if (serverCharacterState != null)
					{
						num = serverCharacterState.Id;
					}
					if (serverCharacterState2 != null)
					{
						num2 = serverCharacterState2.Id;
						num4 = num2;
					}
					if (serverCharacterState3 != null)
					{
						num3 = serverCharacterState3.Id;
						num5 = num3;
					}
					switch (setMode)
					{
					case PlayerCharaList.SET_CHARA_MODE.MAIN:
						if (num >= 0)
						{
							if (num3 == num)
							{
								num5 = num4;
							}
							num4 = num;
							flag = true;
						}
						break;
					case PlayerCharaList.SET_CHARA_MODE.SUB:
						if (num >= 0)
						{
							if (num2 == num)
							{
								num4 = num5;
							}
							num5 = num;
							flag = true;
						}
						break;
					case PlayerCharaList.SET_CHARA_MODE.CHANGE:
						if (num3 >= 0)
						{
							num4 = num3;
							num5 = num2;
							flag = true;
						}
						break;
					}
					if (flag)
					{
						loggedInServerInterface.RequestServerChangeCharacter(num4, num5, base.gameObject);
					}
				}
			}
		}
		else
		{
			GeneralUtil.ShowNoCommunication("ShowNoCommunication");
		}
		return flag;
	}

	public bool SetSort(ServerPlayerState.CHARA_SORT sort)
	{
		bool result = false;
		if (this.m_sortType != sort)
		{
			this.m_sortType = sort;
			this.m_sortOffset = 0;
			result = true;
		}
		this.SetParam(false);
		this.SetObject(false);
		return result;
	}

	public bool SetSortDebug(ServerPlayerState.CHARA_SORT sort)
	{
		bool result = false;
		if (this.m_sortType == sort)
		{
			this.m_sortOffset++;
		}
		else
		{
			this.m_sortType = sort;
			this.m_sortOffset = 0;
			result = true;
		}
		this.SetParam(false);
		this.SetObject(false);
		return result;
	}

	public bool SetDeck(int stock)
	{
		bool result = false;
		if (GeneralUtil.IsNetwork())
		{
			if (stock >= 0 && this.m_currentDeck != stock && this.m_deckList != null && this.m_deckList.Count > stock)
			{
				this.DeckSetLoad(stock);
			}
			else
			{
				global::Debug.Log(string.Concat(new object[]
				{
					"SetDeck error   currentDeck:",
					this.m_currentDeck,
					"  stock:",
					stock
				}));
			}
		}
		else
		{
			GeneralUtil.ShowNoCommunication("ShowNoCommunication");
		}
		return result;
	}

	private void DeckSetLoad(int stock)
	{
		this.m_oldDeckSet = this.GetCurrentDeck();
		this.m_reqCharaMain = this.m_deckList[stock].charaMain;
		this.m_reqCharaSub = this.m_deckList[stock].charaSub;
		this.m_reqChaoMain = this.m_deckList[stock].chaoMain;
		this.m_reqChaoSub = this.m_deckList[stock].chaoSub;
		this.m_reqDeck = stock;
		DeckUtil.SetDeckCurrentStockIndex(this.m_reqDeck);
		if (this.m_oldDeckSet.charaMain == this.m_reqCharaMain && this.m_oldDeckSet.charaSub == this.m_reqCharaSub && this.m_oldDeckSet.chaoMain == this.m_reqChaoMain && this.m_oldDeckSet.chaoSub == this.m_reqChaoSub)
		{
			this.m_oldDeckSet = null;
			this.m_reqDeck = -1;
			this.m_reqCharaMain = CharaType.UNKNOWN;
			this.m_reqCharaSub = CharaType.UNKNOWN;
			this.m_reqChaoMain = -1;
			this.m_reqChaoSub = -1;
			this.UpdateView(true);
		}
		else
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (this.m_oldDeckSet.chaoMain != this.m_reqChaoMain || this.m_oldDeckSet.chaoSub != this.m_reqChaoSub)
			{
				int id = (int)ServerItem.CreateFromChaoId(this.m_reqChaoMain).id;
				int id2 = (int)ServerItem.CreateFromChaoId(this.m_reqChaoSub).id;
				this.m_changeDelay = 0f;
				loggedInServerInterface.RequestServerEquipChao(id, id2, base.gameObject);
			}
			else
			{
				this.ServerEquipChao_Dummy();
			}
		}
	}

	private DeckUtil.DeckSet GetCurrentDeck()
	{
		DeckUtil.DeckSet result = null;
		if (this.m_deckList != null && this.m_deckList.Count > 0 && this.m_deckList.Count > this.m_currentDeck)
		{
			result = this.m_deckList[this.m_currentDeck];
		}
		return result;
	}

	private void SetParam(bool reset)
	{
		if (reset && this.m_charaObjectList != null)
		{
			this.m_charaObjectList.Clear();
		}
		if (this.m_charaStateList != null)
		{
			this.m_charaStateList.Clear();
		}
		ServerPlayerState playerState = ServerInterface.PlayerState;
		this.m_charaStateList = playerState.GetCharacterStateList(this.m_sortType, false, this.m_sortOffset);
		this.m_pageMax = Mathf.CeilToInt((float)this.m_charaStateList.Count / 4f);
		GeneralUtil.SetButtonFunc(base.gameObject, "player_set_grip_R", base.gameObject, "OnClickPageNext");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_player_set_grip_R", base.gameObject, "OnClickPageNext");
		GeneralUtil.SetButtonFunc(base.gameObject, "player_set_grip_L", base.gameObject, "OnClickPagePrev");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_player_set_grip_L", base.gameObject, "OnClickPagePrev");
	}

	private void SetObject(bool reset)
	{
		if (this.m_storage != null)
		{
			if (reset)
			{
				this.m_storage.maxItemCount = (this.m_storage.maxColumns = 0);
				this.m_storage.maxRows = 1;
				this.m_storage.Restart();
			}
			List<CharaType> list = new List<CharaType>();
			int num = 4 * this.m_page;
			int num2 = 0;
			if (this.m_charaStateList.Count > num)
			{
				num2 = this.m_charaStateList.Count - num;
				if (num2 > 4)
				{
					num2 = 4;
				}
			}
			this.m_storage.maxItemCount = (this.m_storage.maxColumns = num2);
			this.m_storage.Restart();
			this.m_charaObjectList = GameObjectUtil.FindChildGameObjectsComponents<ui_player_set_scroll>(this.m_storage.gameObject, "ui_player_set_scroll(Clone)");
			if (this.m_charaObjectList != null && this.m_charaObjectList.Count > 0)
			{
				ServerPlayerState playerState = ServerInterface.PlayerState;
				this.m_charaStateList = playerState.GetCharacterStateList(this.m_sortType, false, this.m_sortOffset);
				int num3 = 0;
				Dictionary<CharaType, ServerCharacterState>.KeyCollection keys = this.m_charaStateList.Keys;
				foreach (CharaType current in keys)
				{
					ServerCharacterState characterState = this.m_charaStateList[current];
					if (num3 >= num && num3 < num + num2 && this.m_charaObjectList.Count > num3 - num)
					{
						list.Add(current);
						this.m_charaObjectList[num3 - num].Setup(this, characterState);
					}
					num3++;
				}
			}
			if (list.Count > 0)
			{
				GeneralUtil.RemoveCharaTexture(list);
			}
		}
		DeckUtil.DeckSet currentDeck = this.GetCurrentDeck();
		if (this.m_charaDeckObject != null)
		{
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_charaDeckObject, "img_player_main");
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_charaDeckObject, "img_player_sub");
			UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_charaDeckObject, "img_decknum");
			if (uISprite != null)
			{
				uISprite.spriteName = "ui_tex_player_set_" + CharaTypeUtil.GetCharaSpriteNameSuffix(currentDeck.charaMain);
				uISprite.gameObject.SetActive(currentDeck.charaMain != CharaType.UNKNOWN);
			}
			if (uISprite2 != null)
			{
				uISprite2.spriteName = "ui_tex_player_set_" + CharaTypeUtil.GetCharaSpriteNameSuffix(currentDeck.charaSub);
				uISprite2.gameObject.SetActive(currentDeck.charaMain != CharaType.UNKNOWN);
			}
			if (uISprite3 != null)
			{
				uISprite3.spriteName = "ui_chao_set_deck_tab_" + (this.m_currentDeck + 1);
			}
			GeneralUtil.SetButtonFunc(this.m_charaDeckObject, "Btn_player_change", base.gameObject, "OnClickChange");
		}
		if (this.m_chaoDeckObject != null && reset)
		{
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_chaoDeckObject, "img_chao_main");
			UITexture uITexture2 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_chaoDeckObject, "img_chao_sub");
			if (ChaoTextureManager.Instance != null)
			{
				if (uITexture != null)
				{
					uITexture.gameObject.SetActive(currentDeck.chaoMain >= 0);
					if (currentDeck.chaoMain >= 0)
					{
						ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
						ChaoTextureManager.Instance.GetTexture(currentDeck.chaoMain, info);
					}
				}
				if (uITexture2 != null)
				{
					uITexture2.gameObject.SetActive(currentDeck.chaoSub >= 0);
					if (currentDeck.chaoSub >= 0)
					{
						ChaoTextureManager.CallbackInfo info2 = new ChaoTextureManager.CallbackInfo(uITexture2, null, true);
						ChaoTextureManager.Instance.GetTexture(currentDeck.chaoSub, info2);
					}
				}
			}
			else
			{
				if (uITexture != null)
				{
					uITexture.gameObject.SetActive(false);
				}
				if (uITexture2 != null)
				{
					uITexture2.gameObject.SetActive(false);
				}
			}
			GeneralUtil.SetButtonFunc(base.gameObject, this.m_chaoDeckObject.name, base.gameObject, "OnClickChao");
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_page");
		if (uILabel != null)
		{
			uILabel.text = this.m_page + 1 + "/" + this.m_pageMax;
		}
		if (this.m_changeDelay <= 0f && reset)
		{
			this.m_changeBtn = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_player_change");
			if (this.m_changeBtn != null)
			{
				if (this.CheckSetMode(PlayerCharaList.SET_CHARA_MODE.CHANGE, CharaType.UNKNOWN))
				{
					this.m_changeBtn.isEnabled = true;
					this.m_changeDelay = 1.5f;
				}
				else
				{
					this.m_changeBtn.isEnabled = false;
					this.m_changeDelay = -1f;
				}
			}
		}
		GeneralUtil.SetCharasetBtnIcon(base.gameObject, "Btn_charaset");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_charaset", base.gameObject, "OnClickDeck");
	}

	private void OnMsgDeckViewWindowChange()
	{
		this.UpdateView(true);
	}

	private void OnMsgDeckViewWindowNotChange()
	{
		this.UpdateView(true);
	}

	private void OnMsgDeckViewWindowNetworkError()
	{
		this.UpdateView(false);
	}

	private void OnClickBack()
	{
		if (base.gameObject.activeSelf && !this.m_isEnd)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_isEnd = true;
		}
	}

	private void OnClickDeck()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		DeckViewWindow.Create(base.gameObject);
	}

	private void OnClickChao()
	{
		HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHAO, true);
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickChange()
	{
		this.SetChara(PlayerCharaList.SET_CHARA_MODE.CHANGE, CharaType.UNKNOWN);
		if (this.m_changeBtn != null)
		{
			this.m_changeBtn.isEnabled = false;
			this.m_changeDelay = 1.5f;
		}
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickPageNext()
	{
		if (this.m_pageMax > 0)
		{
			this.m_page = (this.m_page + 1 + this.m_pageMax) % this.m_pageMax;
			this.SetObject(false);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickPagePrev()
	{
		if (this.m_pageMax > 0)
		{
			this.m_page = (this.m_page - 1 + this.m_pageMax) % this.m_pageMax;
			this.SetObject(false);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	public void OnClosedWindowAnim()
	{
		if (this.m_isEnd)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void OnClickBackButton()
	{
		if (base.gameObject.activeSelf && !this.m_isEnd)
		{
			this.m_isEnd = true;
			SoundManager.SePlay("sys_menu_decide", "SE");
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_mm_player_set_2_intro_Anim", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnClosedWindowAnim), true);
			}
		}
	}

	private void ServerChangeCharacter_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.m_oldDeckSet = null;
		this.m_reqDeck = -1;
		this.m_reqCharaMain = CharaType.UNKNOWN;
		this.m_reqCharaSub = CharaType.UNKNOWN;
		this.m_reqChaoMain = -1;
		this.m_reqChaoSub = -1;
		this.UpdateView(true);
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void ServerChangeCharacter_Dummy()
	{
		this.m_oldDeckSet = null;
		this.m_reqDeck = -1;
		this.m_reqCharaMain = CharaType.UNKNOWN;
		this.m_reqCharaSub = CharaType.UNKNOWN;
		this.m_reqChaoMain = -1;
		this.m_reqChaoSub = -1;
		this.UpdateView(true);
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void ServerEquipChao_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		bool flag = false;
		if (loggedInServerInterface != null)
		{
			if (this.m_oldDeckSet.charaMain != CharaType.UNKNOWN && this.m_oldDeckSet.charaMain != this.m_reqCharaMain)
			{
				flag = true;
			}
			if (this.m_oldDeckSet.charaSub != this.m_reqCharaSub)
			{
				flag = true;
			}
			if (flag)
			{
				int mainCharaId = -1;
				int subCharaId = -1;
				ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(this.m_reqCharaMain);
				ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(this.m_reqCharaSub);
				if (serverCharacterState != null)
				{
					mainCharaId = serverCharacterState.Id;
				}
				if (serverCharacterState2 != null)
				{
					subCharaId = serverCharacterState2.Id;
				}
				loggedInServerInterface.RequestServerChangeCharacter(mainCharaId, subCharaId, base.gameObject);
			}
			else
			{
				this.ServerChangeCharacter_Dummy();
			}
		}
	}

	private void ServerEquipChao_Dummy()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		bool flag = false;
		if (loggedInServerInterface != null)
		{
			if (this.m_oldDeckSet.charaMain != CharaType.UNKNOWN && this.m_oldDeckSet.charaMain != this.m_reqCharaMain)
			{
				flag = true;
			}
			if (this.m_oldDeckSet.charaSub != this.m_reqCharaSub)
			{
				flag = true;
			}
			if (flag)
			{
				int mainCharaId = -1;
				int subCharaId = -1;
				ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(this.m_reqCharaMain);
				ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(this.m_reqCharaSub);
				if (serverCharacterState != null)
				{
					mainCharaId = serverCharacterState.Id;
				}
				if (serverCharacterState2 != null)
				{
					subCharaId = serverCharacterState2.Id;
				}
				loggedInServerInterface.RequestServerChangeCharacter(mainCharaId, subCharaId, base.gameObject);
			}
			else
			{
				this.ServerChangeCharacter_Dummy();
			}
		}
	}

	public static void DebugShowPlayerCharaListGui()
	{
		if (PlayerCharaList.s_isActive && PlayerCharaList.s_instance != null && PlayerCharaList.s_instance.gameObject.activeSelf && !PlayerLvupWindow.isActive && !PlayerSetWindowUI.isActive && !DeckViewWindow.isActive && !GeneralWindow.Created)
		{
			Rect rect = new Rect(170f, -5f, 150f, 90f);
			Rect rect2 = SingletonGameObject<DebugGameObject>.Instance.CreateGuiRect(rect, DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM);
			GUI.Box(rect2, string.Concat(new object[]
			{
				"sort:",
				PlayerCharaList.s_instance.sortType,
				"  [",
				PlayerCharaList.s_instance.sortOffset,
				"]"
			}));
			int num = 3;
			float num2 = 0.25f;
			float num3 = (1f - num2) / (float)num;
			for (int i = 0; i < num; i++)
			{
				Rect rect3 = new Rect(0f, num2 + num3 * (float)i, 0.95f, num3 * 0.95f);
				Rect position = SingletonGameObject<DebugGameObject>.Instance.CreateGuiRectInRate(rect2, rect3, DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				ServerPlayerState.CHARA_SORT cHARA_SORT = (ServerPlayerState.CHARA_SORT)i;
				if (GUI.Button(position, string.Empty + cHARA_SORT))
				{
					PlayerCharaList.s_instance.SetSortDebug(cHARA_SORT);
				}
			}
		}
	}
}
