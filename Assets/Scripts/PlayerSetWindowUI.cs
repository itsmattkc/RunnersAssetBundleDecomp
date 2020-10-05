using AnimationOrTween;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class PlayerSetWindowUI : WindowBase
{
	public enum WINDOW_MODE
	{
		DEFAULT,
		BUY,
		SET
	}

	private enum BTN_TYPE
	{
		BUY_1,
		BUY_2,
		BUY_3,
		SET,
		NONE
	}

	private sealed class _OnFinished_c__Iterator4E : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal PlayerSetWindowUI __f__this;

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
				this._current = new WaitForSeconds(0.5f);
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.m_windMode = PlayerSetWindowUI.WINDOW_MODE.DEFAULT;
				PlayerSetWindowUI.s_isActive = false;
				this.__f__this.gameObject.SetActive(false);
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

	private const float BTN_DELAY_TIME = 2f;

	private static bool s_isActive;

	private ui_player_set_scroll m_parent;

	private bool m_init;

	private bool m_setup;

	private bool m_costError;

	private ButtonInfoTable.ButtonType m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;

	private ServerItem.Id m_buyId = ServerItem.Id.NONE;

	private PlayerSetWindowUI.WINDOW_MODE m_windMode;

	private PlayerSetWindowUI.BTN_TYPE m_btnType = PlayerSetWindowUI.BTN_TYPE.NONE;

	private CharaType m_charaType = CharaType.UNKNOWN;

	private ServerCharacterState m_charaState;

	private Dictionary<ServerItem.Id, int> m_buyCostList;

	private List<UIImageButton> m_btnObjectList;

	private float m_btnDelay;

	private Animation m_animation;

	private UIPlayAnimation m_playAnimation;

	private UIPanel m_panel;

	private int m_oldUnlockedCharacterNum;

	private UIButton m_btnClose;

	private List<GameObject> m_btnObjList;

	private static bool s_starTextDefaultInit;

	private static Color s_starTextDefault;

	public static bool isActive
	{
		get
		{
			return PlayerSetWindowUI.s_isActive;
		}
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	private void Update()
	{
		if (this.m_buyId != ServerItem.Id.NONE && GeneralWindow.IsCreated("BuyCharacter") && GeneralWindow.IsButtonPressed)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				if (GeneralUtil.IsNetwork())
				{
					if (this.m_buyCostList != null && this.m_buyCostList.ContainsKey(this.m_buyId))
					{
						ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
						if (loggedInServerInterface != null)
						{
							this.SetBtnObjectCollider(false);
							this.m_oldUnlockedCharacterNum = 1;
							ServerPlayerState playerState = ServerInterface.PlayerState;
							if (playerState != null)
							{
								this.m_oldUnlockedCharacterNum = playerState.unlockedCharacterNum;
							}
							ServerItem item = new ServerItem(this.m_buyId);
							loggedInServerInterface.RequestServerUnlockedCharacter(this.m_charaType, item, base.gameObject);
						}
					}
				}
				else
				{
					GeneralUtil.ShowNoCommunication("ShowNoCommunication");
				}
			}
			this.m_buyId = ServerItem.Id.NONE;
		}
		if (this.m_btnDelay > 0f)
		{
			this.m_btnDelay -= Time.deltaTime;
			if (this.m_btnDelay <= 0f)
			{
				this.SetBtnObjectEnabeld(true, 2f);
				this.m_btnDelay = 0f;
			}
		}
	}

	public void Init()
	{
		base.gameObject.SetActive(true);
		this.m_panel = GameObjectUtil.FindChildGameObjectComponent<UIPanel>(base.gameObject, "player_set_window");
		this.m_panel.alpha = 0f;
		this.m_animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "player_set_window");
		if (this.m_animation != null)
		{
			GameObject gameObject = this.m_animation.gameObject;
			gameObject.SetActive(false);
			this.m_animation.Stop();
		}
		this.m_playAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		this.m_playAnimation.target = this.m_animation;
		this.m_btnClose = GameObjectUtil.FindChildGameObjectComponent<UIButton>(base.gameObject, "Btn_window_close");
		this.m_btnObjList = new List<GameObject>();
		for (int i = 0; i < 7; i++)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_" + i);
			if (gameObject2 != null)
			{
				this.m_btnObjList.Add(gameObject2);
			}
			else if (i > 0)
			{
				break;
			}
		}
		UIButtonMessage uIButtonMessage = this.m_btnClose.gameObject.GetComponent<UIButtonMessage>();
		if (uIButtonMessage == null)
		{
			uIButtonMessage = this.m_btnClose.gameObject.AddComponent<UIButtonMessage>();
		}
		if (uIButtonMessage != null)
		{
			uIButtonMessage.trigger = UIButtonMessage.Trigger.OnClick;
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnClickBtn";
		}
		base.gameObject.SetActive(false);
		this.m_init = true;
	}

	private void OnClickBtn()
	{
		if (this.m_setup)
		{
			PlayerSetWindowUI.s_isActive = false;
			this.m_setup = false;
			this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
			this.m_buyId = ServerItem.Id.NONE;
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnAnimFinished), true);
			}
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void OnAnimFinished()
	{
		base.gameObject.SetActive(false);
		this.m_windMode = PlayerSetWindowUI.WINDOW_MODE.DEFAULT;
		PlayerSetWindowUI.s_isActive = false;
	}

	private IEnumerator OnFinished()
	{
		PlayerSetWindowUI._OnFinished_c__Iterator4E _OnFinished_c__Iterator4E = new PlayerSetWindowUI._OnFinished_c__Iterator4E();
		_OnFinished_c__Iterator4E.__f__this = this;
		return _OnFinished_c__Iterator4E;
	}

	private void ResetBtnObjectList()
	{
		if (this.m_btnObjectList != null)
		{
			this.m_btnObjectList.Clear();
		}
		else
		{
			this.m_btnObjectList = new List<UIImageButton>();
		}
	}

	private void AddBtnObjectList(UIImageButton btnObject)
	{
		if (this.m_btnObjectList == null)
		{
			this.m_btnObjectList = new List<UIImageButton>();
		}
		btnObject.isEnabled = (this.m_btnDelay <= 0f);
		this.m_btnObjectList.Add(btnObject);
	}

	private void SetBtnObjectCollider(bool enabeld)
	{
		if (this.m_btnObjectList != null && this.m_btnObjectList.Count > 0)
		{
			foreach (UIImageButton current in this.m_btnObjectList)
			{
				if (current != null)
				{
					BoxCollider component = current.gameObject.GetComponent<BoxCollider>();
					if (component != null)
					{
						component.isTrigger = !enabeld;
					}
				}
			}
		}
	}

	private void SetBtnObjectEnabeld(bool enabeld, float delay = 2f)
	{
		if (this.m_btnObjectList != null && this.m_btnObjectList.Count > 0)
		{
			foreach (UIImageButton current in this.m_btnObjectList)
			{
				if (current != null)
				{
					current.isEnabled = enabeld;
				}
			}
		}
		if (enabeld)
		{
			this.m_btnDelay = 0f;
			this.SetBtnObjectCollider(true);
		}
		else
		{
			this.m_btnDelay = delay;
		}
	}

	private void SetupCharaSetBtnView(GameObject parent, string mainObjName, string subObjName)
	{
		GeneralUtil.SetButtonFunc(parent, mainObjName, base.gameObject, "OnClickBtnMain");
		GeneralUtil.SetButtonFunc(parent, subObjName, base.gameObject, "OnClickBtnSub");
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, mainObjName);
		UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, subObjName);
		if (uIImageButton != null && uIImageButton2 != null)
		{
			if (this.m_parent != null && this.m_parent.parent != null)
			{
				uIImageButton.isEnabled = this.m_parent.parent.CheckSetMode(PlayerCharaList.SET_CHARA_MODE.MAIN, this.m_charaType);
				uIImageButton2.isEnabled = this.m_parent.parent.CheckSetMode(PlayerCharaList.SET_CHARA_MODE.SUB, this.m_charaType);
			}
			else
			{
				uIImageButton.isEnabled = true;
				uIImageButton2.isEnabled = true;
			}
		}
	}

	private void SetupRouletteBtnView(GameObject parent, string objName)
	{
		GeneralUtil.SetButtonFunc(parent, objName, base.gameObject, "OnClickBtnRoulette");
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, objName);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(parent, objName + "_1");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(parent, objName + "_2");
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(parent, "img_sale_icon");
		if (uIImageButton != null)
		{
			this.AddBtnObjectList(uIImageButton);
		}
		if (gameObject != null && gameObject2 != null)
		{
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
		}
		if (gameObject3 != null)
		{
			if (HudMenuUtility.IsSale(Constants.Campaign.emType.ChaoRouletteCost))
			{
				gameObject3.SetActive(true);
			}
			else if (HudMenuUtility.IsSale(Constants.Campaign.emType.PremiumRouletteOdds))
			{
				gameObject3.SetActive(true);
			}
			else
			{
				gameObject3.SetActive(false);
			}
		}
	}

	private void SetupBuyBtnView(GameObject parent, string objName, ServerItem.Id costItem, int costValue)
	{
		GeneralUtil.SetButtonFunc(parent, objName, base.gameObject, "OnClickBtn_" + costItem);
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, objName);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(parent, objName + "_1");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(parent, objName + "_2");
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(parent, "img_sale_icon");
		if (uIImageButton != null)
		{
			this.AddBtnObjectList(uIImageButton);
		}
		if (gameObject != null && gameObject2 != null)
		{
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_icon_ring");
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_cost");
			if (uISprite != null && uILabel != null)
			{
				if (costItem != ServerItem.Id.SPECIAL_EGG)
				{
					if (costItem != ServerItem.Id.RSRING)
					{
						if (costItem != ServerItem.Id.RING)
						{
							if (costItem != ServerItem.Id.RAIDRING)
							{
								uISprite.spriteName = string.Empty;
							}
							else
							{
								uISprite.spriteName = "ui_event_ring_icon";
							}
						}
						else
						{
							uISprite.spriteName = "ui_test_icon_ring00";
						}
					}
					else
					{
						uISprite.spriteName = "ui_test_icon_rsring";
					}
				}
				else
				{
					uISprite.spriteName = "ui_roulette_pager_icon_8";
				}
				uILabel.text = HudUtility.GetFormatNumString<int>(costValue);
			}
		}
		else
		{
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(parent, "img_icon_ring");
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_rs_cost_1");
			if (uILabel2 == null)
			{
				uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_rs_cost_2");
			}
			if (uISprite2 != null && uILabel2 != null)
			{
				if (costItem != ServerItem.Id.SPECIAL_EGG)
				{
					if (costItem != ServerItem.Id.RSRING)
					{
						if (costItem != ServerItem.Id.RING)
						{
							if (costItem != ServerItem.Id.RAIDRING)
							{
								uISprite2.spriteName = string.Empty;
							}
							else
							{
								uISprite2.spriteName = "ui_event_ring_icon";
							}
						}
						else
						{
							uISprite2.spriteName = "ui_test_icon_ring00";
						}
					}
					else
					{
						uISprite2.spriteName = "ui_test_icon_rsring";
					}
				}
				else
				{
					uISprite2.spriteName = "ui_roulette_pager_icon_8";
				}
				uILabel2.text = HudUtility.GetFormatNumString<int>(costValue);
			}
		}
		if (gameObject3 != null)
		{
			ServerItem serverItem = new ServerItem(this.m_charaType);
			int id = (int)serverItem.id;
			ServerCampaignData campaignInSession = ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.CharacterUpgradeCost, id);
			gameObject3.SetActive(campaignInSession != null);
		}
	}

	private void SetupBtn()
	{
		this.ResetBtnObjectList();
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			this.m_charaState = playerState.CharacterState(this.m_charaType);
		}
		else
		{
			this.m_charaState = null;
		}
		this.m_buyCostList = null;
		PlayerSetWindowUI.WINDOW_MODE windMode = this.m_windMode;
		if (windMode != PlayerSetWindowUI.WINDOW_MODE.BUY)
		{
			if (windMode != PlayerSetWindowUI.WINDOW_MODE.SET)
			{
				this.m_btnType = PlayerSetWindowUI.BTN_TYPE.NONE;
			}
			else
			{
				this.m_btnType = PlayerSetWindowUI.BTN_TYPE.SET;
			}
		}
		else
		{
			this.m_btnType = PlayerSetWindowUI.BTN_TYPE.BUY_1;
			if (this.m_charaState != null)
			{
				int num = 0;
				if (this.m_charaState.IsBuy)
				{
					this.m_buyCostList = this.m_charaState.GetBuyCostItemList();
					if (this.m_buyCostList != null)
					{
						num = this.m_buyCostList.Count;
					}
				}
				if (this.m_charaState.IsRoulette)
				{
					num++;
				}
				if (num == 1)
				{
					this.m_btnType = PlayerSetWindowUI.BTN_TYPE.BUY_1;
				}
				else if (num == 2)
				{
					this.m_btnType = PlayerSetWindowUI.BTN_TYPE.BUY_2;
				}
				else if (num == 3)
				{
					this.m_btnType = PlayerSetWindowUI.BTN_TYPE.BUY_3;
				}
				else
				{
					this.m_btnType = PlayerSetWindowUI.BTN_TYPE.NONE;
				}
			}
		}
		List<ServerItem.Id> list = null;
		if (this.m_buyCostList != null && this.m_buyCostList.Count > 0)
		{
			list = new List<ServerItem.Id>();
			Dictionary<ServerItem.Id, int>.KeyCollection keys = this.m_buyCostList.Keys;
			foreach (ServerItem.Id current in keys)
			{
				list.Add(current);
			}
		}
		int num2;
		switch (this.m_btnType)
		{
		case PlayerSetWindowUI.BTN_TYPE.BUY_1:
			num2 = 0;
			break;
		case PlayerSetWindowUI.BTN_TYPE.BUY_2:
			num2 = 1;
			break;
		case PlayerSetWindowUI.BTN_TYPE.BUY_3:
			num2 = 2;
			break;
		case PlayerSetWindowUI.BTN_TYPE.SET:
			num2 = 3;
			break;
		default:
			num2 = -1;
			break;
		}
		for (int i = 0; i < this.m_btnObjList.Count; i++)
		{
			if (i == num2)
			{
				this.m_btnObjList[i].SetActive(true);
				switch (this.m_btnType)
				{
				case PlayerSetWindowUI.BTN_TYPE.BUY_1:
					if (this.m_charaState.IsRoulette)
					{
						this.SetupRouletteBtnView(this.m_btnObjList[i], "Btn_c");
					}
					else
					{
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_c", list[0], this.m_buyCostList[list[0]]);
					}
					break;
				case PlayerSetWindowUI.BTN_TYPE.BUY_2:
					if (this.m_charaState.IsRoulette)
					{
						this.SetupRouletteBtnView(this.m_btnObjList[i], "Btn_l");
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_r", list[0], this.m_buyCostList[list[0]]);
					}
					else
					{
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_l", list[0], this.m_buyCostList[list[0]]);
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_r", list[1], this.m_buyCostList[list[1]]);
					}
					break;
				case PlayerSetWindowUI.BTN_TYPE.BUY_3:
					if (this.m_charaState.IsRoulette)
					{
						this.SetupRouletteBtnView(this.m_btnObjList[i], "Btn_l");
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_c", list[0], this.m_buyCostList[list[0]]);
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_r", list[1], this.m_buyCostList[list[1]]);
					}
					else
					{
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_l", list[0], this.m_buyCostList[list[0]]);
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_c", list[1], this.m_buyCostList[list[1]]);
						this.SetupBuyBtnView(this.m_btnObjList[i], "Btn_r", list[2], this.m_buyCostList[list[2]]);
					}
					break;
				case PlayerSetWindowUI.BTN_TYPE.SET:
					this.SetupCharaSetBtnView(this.m_btnObjList[i], "Btn_main", "Btn_sub");
					break;
				}
			}
			else
			{
				this.m_btnObjList[i].SetActive(false);
			}
		}
	}

	private void SetupObject()
	{
		if (this.m_panel != null)
		{
			this.m_panel.depth = 54;
			this.m_panel.alpha = 1f;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_icon_key");
		if (gameObject != null)
		{
			gameObject.SetActive(!this.m_charaState.IsUnlocked);
		}
		string commonText = TextUtility.GetCommonText("CharaName", CharaName.Name[(int)this.m_charaType]);
		string charaAttributeSpriteName = HudUtility.GetCharaAttributeSpriteName(this.m_charaType);
		string teamAttributeSpriteName = HudUtility.GetTeamAttributeSpriteName(this.m_charaType);
		string charaAttributeText = this.m_charaState.GetCharaAttributeText();
		int totalLevel = MenuPlayerSetUtil.GetTotalLevel(this.m_charaType);
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_player_speacies");
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_player_genus");
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_lv");
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_name");
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_attribute");
		UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_star_lv");
		UILabel uILabel5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbt_caption");
		if (uILabel5 != null)
		{
			switch (this.m_btnType)
			{
			case PlayerSetWindowUI.BTN_TYPE.BUY_1:
			case PlayerSetWindowUI.BTN_TYPE.BUY_2:
			case PlayerSetWindowUI.BTN_TYPE.BUY_3:
				uILabel5.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PlayerSet", "ui_Lbt_buy_caption").text;
				break;
			case PlayerSetWindowUI.BTN_TYPE.SET:
				uILabel5.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PlayerSet", "ui_Lbt_caption").text;
				break;
			case PlayerSetWindowUI.BTN_TYPE.NONE:
				uILabel5.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PlayerSet", "ui_Lbt_info_caption").text;
				break;
			}
		}
		if (uILabel4 != null)
		{
			uILabel4.text = this.m_charaState.star.ToString();
			if (!PlayerSetWindowUI.s_starTextDefaultInit)
			{
				PlayerSetWindowUI.s_starTextDefault = new Color(uILabel4.color.r, uILabel4.color.g, uILabel4.color.b, uILabel4.color.a);
				PlayerSetWindowUI.s_starTextDefaultInit = true;
			}
			if (this.m_charaState.star >= this.m_charaState.starMax)
			{
				uILabel4.color = new Color(0.9647059f, 0.454901963f, 0f);
			}
			else
			{
				uILabel4.color = PlayerSetWindowUI.s_starTextDefault;
			}
		}
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_player_tex");
		if (uITexture != null)
		{
			TextureRequestChara request = new TextureRequestChara(this.m_charaType, uITexture);
			TextureAsyncLoadManager.Instance.Request(request);
			if (this.m_charaState.IsUnlocked)
			{
				uITexture.color = new Color(1f, 1f, 1f);
			}
			else
			{
				uITexture.color = new Color(0f, 0f, 0f);
			}
		}
		uISprite.spriteName = charaAttributeSpriteName;
		uISprite2.spriteName = teamAttributeSpriteName;
		uILabel.text = TextUtility.GetTextLevel(string.Format("{0:000}", totalLevel));
		uILabel2.text = commonText;
		uILabel3.text = charaAttributeText;
	}

	public void Setup(CharaType charaType, ui_player_set_scroll parent = null, PlayerSetWindowUI.WINDOW_MODE mode = PlayerSetWindowUI.WINDOW_MODE.DEFAULT)
	{
		PlayerSetWindowUI.s_isActive = true;
		this.m_setup = true;
		this.m_costError = false;
		this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
		this.m_buyId = ServerItem.Id.NONE;
		if (!this.m_init)
		{
			this.Init();
		}
		this.m_parent = parent;
		this.m_charaType = charaType;
		this.m_windMode = mode;
		this.m_btnDelay = 0f;
		this.SetupBtn();
		if (this.m_charaState != null)
		{
			base.gameObject.SetActive(true);
			this.SetupObject();
			if (this.m_animation != null)
			{
				GameObject gameObject = this.m_animation.gameObject;
				gameObject.SetActive(true);
				this.m_playAnimation.Play(true);
			}
			UIDraggablePanel uIDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(base.gameObject, "ScrollView");
			if (uIDraggablePanel != null)
			{
				uIDraggablePanel.ResetPosition();
			}
			SoundManager.SePlay("sys_window_open", "SE");
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "eff_set1");
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "eff_set2");
		if (gameObject2 != null && gameObject3 != null)
		{
			gameObject2.SetActive(false);
			gameObject3.SetActive(false);
		}
		this.SetBtnObjectCollider(true);
	}

	public static PlayerSetWindowUI Create(CharaType charaType, ui_player_set_scroll parent = null, PlayerSetWindowUI.WINDOW_MODE mode = PlayerSetWindowUI.WINDOW_MODE.DEFAULT)
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "PlayerSetWindowUI");
			PlayerSetWindowUI playerSetWindowUI = null;
			if (gameObject != null)
			{
				playerSetWindowUI = gameObject.GetComponent<PlayerSetWindowUI>();
				if (playerSetWindowUI == null)
				{
					playerSetWindowUI = gameObject.AddComponent<PlayerSetWindowUI>();
				}
				if (playerSetWindowUI != null)
				{
					playerSetWindowUI.Setup(charaType, parent, mode);
				}
			}
			return playerSetWindowUI;
		}
		return null;
	}

	private void OnClickBtnRoulette()
	{
		PlayerSetWindowUI.s_isActive = false;
		this.m_setup = false;
		this.m_buyId = ServerItem.Id.NONE;
		ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
		if (activeAnimation != null)
		{
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnAnimFinished), true);
		}
		HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.ROULETTE, false);
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickBtn_RING()
	{
		if (this.m_buyCostList != null && this.m_buyCostList.Count > 0 && this.m_buyCostList.ContainsKey(ServerItem.Id.RING))
		{
			int num = this.m_buyCostList[ServerItem.Id.RING];
			if (this.SendBuyChara(ServerItem.Id.RING, num))
			{
				global::Debug.Log("OnClickBtn_RING value:" + num);
			}
		}
	}

	private void OnClickBtn_RSRING()
	{
		if (this.m_buyCostList != null && this.m_buyCostList.Count > 0 && this.m_buyCostList.ContainsKey(ServerItem.Id.RSRING))
		{
			int num = this.m_buyCostList[ServerItem.Id.RSRING];
			if (this.SendBuyChara(ServerItem.Id.RSRING, num))
			{
				global::Debug.Log("OnClickBtn_RSRING value:" + num);
			}
		}
	}

	private void OnClickBtnMain()
	{
		if (this.m_parent != null && this.m_parent.parent != null)
		{
			this.m_parent.parent.SetChara(PlayerCharaList.SET_CHARA_MODE.MAIN, this.m_charaType);
		}
		PlayerSetWindowUI.s_isActive = false;
		ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
		if (activeAnimation != null)
		{
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnAnimFinished), true);
		}
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickBtnSub()
	{
		if (this.m_parent != null && this.m_parent.parent != null)
		{
			this.m_parent.parent.SetChara(PlayerCharaList.SET_CHARA_MODE.SUB, this.m_charaType);
		}
		PlayerSetWindowUI.s_isActive = false;
		ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
		if (activeAnimation != null)
		{
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnAnimFinished), true);
		}
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private bool SendBuyChara(ServerItem.Id itemId, int cost)
	{
		bool result = false;
		if (GeneralUtil.IsNetwork())
		{
			if (this.m_charaState != null)
			{
				this.SetBtnObjectCollider(false);
				long itemCount = GeneralUtil.GetItemCount(itemId);
				if (itemCount >= (long)cost)
				{
					this.m_buyId = itemId;
					string text = string.Empty;
					string caption = string.Empty;
					if (itemId != ServerItem.Id.RSRING)
					{
						if (itemId == ServerItem.Id.RING)
						{
							if (this.m_charaState.IsUnlocked)
							{
								text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_unlock_ring_text_2");
							}
							else
							{
								text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_unlock_ring_text");
							}
						}
					}
					else if (this.m_charaState.IsUnlocked)
					{
						text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_unlock_rsring_text_2");
					}
					else
					{
						text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_unlock_rsring_text");
					}
					text = text.Replace("{RING_COST}", HudUtility.GetFormatNumString<int>(cost));
					if (this.m_charaState.IsUnlocked)
					{
						caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_unlock_caption_2");
					}
					else
					{
						caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_unlock_caption");
					}
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = "BuyCharacter",
						buttonType = GeneralWindow.ButtonType.YesNo,
						finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowBuyCharacterClosedCallback),
						caption = caption,
						message = text
					});
					result = true;
				}
				else
				{
					string message = string.Empty;
					string caption2 = string.Empty;
					string name = string.Empty;
					this.m_costError = false;
					GeneralWindow.ButtonType buttonType = GeneralWindow.ButtonType.ShopCancel;
					bool flag = ServerInterface.IsRSREnable();
					if (itemId != ServerItem.Id.RSRING)
					{
						if (itemId == ServerItem.Id.RING)
						{
							name = "SpinCostErrorRing";
							caption2 = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "gw_cost_caption");
							message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "gw_cost_text");
							this.m_costErrorType = ButtonInfoTable.ButtonType.RING_TO_SHOP;
						}
					}
					else
					{
						name = "SpinCostErrorRSRing";
						caption2 = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "gw_cost_caption");
						message = ((!flag) ? TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption_text_2") : TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption_text"));
						buttonType = ((!flag) ? GeneralWindow.ButtonType.Ok : GeneralWindow.ButtonType.ShopCancel);
						this.m_costErrorType = ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP;
					}
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = name,
						buttonType = buttonType,
						finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowClosedCallback),
						caption = caption2,
						message = message,
						isPlayErrorSe = true
					});
				}
			}
		}
		else
		{
			GeneralUtil.ShowNoCommunication("ShowNoCommunication");
		}
		return result;
	}

	private void GeneralWindowBuyCharacterClosedCallback()
	{
		if (!GeneralWindow.IsYesButtonPressed)
		{
			GeneralWindow.Close();
			this.SetBtnObjectEnabeld(true, 2f);
		}
	}

	private void GeneralWindowClosedCallback()
	{
		HudMenuUtility.SetConnectAlertSimpleUI(false);
		if (this.m_costErrorType != ButtonInfoTable.ButtonType.UNKNOWN)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				PlayerSetWindowUI.s_isActive = false;
				this.m_setup = false;
				this.m_buyId = ServerItem.Id.NONE;
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
				if (activeAnimation != null)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnAnimFinished), true);
				}
				HudMenuUtility.SendMenuButtonClicked(this.m_costErrorType, false);
				this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
			}
			else
			{
				this.SetBtnObjectEnabeld(true, 2f);
				this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
			}
		}
		GeneralWindow.Close();
	}

	private void ServerUnlockedCharacter_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "window");
		if (animation != null)
		{
			ActiveAnimation.Play(animation, "ui_menu_player_open_Anim", Direction.Forward);
		}
		if (this.m_parent != null)
		{
			this.m_parent.UpdateView();
		}
		this.SetupBtn();
		this.SetupObject();
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null && playerState.unlockedCharacterNum == 2 && this.m_oldUnlockedCharacterNum == 1 && this.m_parent != null && this.m_parent.parent != null)
		{
			this.m_parent.parent.SetChara(PlayerCharaList.SET_CHARA_MODE.SUB, this.m_charaType);
		}
		global::Debug.Log(string.Concat(new object[]
		{
			"ServerUnlockedCharacter_Succeeded oldUnlockedCharacterNum:",
			this.m_oldUnlockedCharacterNum,
			"  currentUnlockedCharacterNum:",
			playerState.unlockedCharacterNum
		}));
		SoundManager.SePlay("sys_buy", "SE");
		this.SetBtnObjectEnabeld(false, 2f);
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		if (!GeneralWindow.Created && !NetworkErrorWindow.Created)
		{
			PlayerSetWindowUI.s_isActive = false;
			this.m_setup = false;
			this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
			this.m_buyId = ServerItem.Id.NONE;
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnAnimFinished), true);
			}
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}
}
