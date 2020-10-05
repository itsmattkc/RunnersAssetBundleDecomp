using AnimationOrTween;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class PlayerLvupWindow : WindowBase
{
	private sealed class _SetupObject_c__Iterator4D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal bool init;

		internal ActiveAnimation _anim___0;

		internal UIPlayAnimation _btnClose___1;

		internal GameObject _saleObj___2;

		internal UILabel _labCaption___3;

		internal UILabel _labLv___4;

		internal UILabel _labCost___5;

		internal UITexture _texChara___6;

		internal UISlider _sliExp___7;

		internal UIImageButton _imgBtn___8;

		internal int _cnt___9;

		internal List<GameObject> _buttonList___10;

		internal List<AbilityType>.Enumerator __s_608___11;

		internal AbilityType _key___12;

		internal MenuPlayerSetAbilityButton _button___13;

		internal TextureRequestChara _textureRequest___14;

		internal int _charaId___15;

		internal ServerCampaignData _campaignData___16;

		internal int _totalLevel___17;

		internal int _levelUpCost___18;

		internal int _remainCost___19;

		internal float _expRatio___20;

		internal GameObject _barObj___21;

		internal int _PC;

		internal object _current;

		internal bool ___init;

		internal PlayerLvupWindow __f__this;

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
			{
				if (this.__f__this.m_mainPanel != null)
				{
					this.__f__this.m_mainPanel.alpha = 1f;
				}
				if (this.init && this.__f__this.m_animation != null)
				{
					this._anim___0 = ActiveAnimation.Play(this.__f__this.m_animation, "ui_cmn_window_Anim2", Direction.Forward);
					if (this._anim___0 != null)
					{
						EventDelegate.Add(this._anim___0.onFinished, new EventDelegate.Callback(this.__f__this.OnFinishedInAnim), true);
					}
				}
				GeneralUtil.SetButtonFunc(this.__f__this.gameObject, "Btn_close", this.__f__this.gameObject, "OnClickClose");
				GeneralUtil.SetButtonFunc(this.__f__this.gameObject, "Btn_lv_up", this.__f__this.gameObject, "OnClickLvUp");
				this._btnClose___1 = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(this.__f__this.gameObject, "Btn_close");
				if (this._btnClose___1 != null && !EventDelegate.IsValid(this._btnClose___1.onFinished))
				{
					EventDelegate.Add(this._btnClose___1.onFinished, new EventDelegate.Callback(this.__f__this.OnFinished), true);
				}
				this.__f__this.m_isEnd = false;
				this.__f__this.m_isClickClose = false;
				this._saleObj___2 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "img_icon_sale");
				this._labCaption___3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.__f__this.gameObject, "Lbl_caption");
				this._labLv___4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.__f__this.gameObject, "Lbl_player_lv");
				this._labCost___5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.__f__this.gameObject, "Lbl_price_number");
				this._texChara___6 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.__f__this.gameObject, "img_player_tex");
				this._sliExp___7 = GameObjectUtil.FindChildGameObjectComponent<UISlider>(this.__f__this.gameObject, "Pgb_exp");
				this._imgBtn___8 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.__f__this.gameObject, "Btn_lv_up");
				this.__f__this.m_storage = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.__f__this.gameObject, "slot");
				if (this.__f__this.m_storage != null && this.init)
				{
					if (this.__f__this.m_btnObjList == null)
					{
						this.__f__this.m_btnObjList = new Dictionary<AbilityType, MenuPlayerSetAbilityButton>();
					}
					else
					{
						this.__f__this.m_btnObjList.Clear();
					}
					this.__f__this.m_btnObjList = new Dictionary<AbilityType, MenuPlayerSetAbilityButton>();
					this._cnt___9 = 0;
					this._buttonList___10 = GameObjectUtil.FindChildGameObjects(this.__f__this.m_storage.gameObject, "ui_player_set_item_2_cell(Clone)");
					this.__s_608___11 = this.__f__this.m_abilityList.GetEnumerator();
					try
					{
						while (this.__s_608___11.MoveNext())
						{
							this._key___12 = this.__s_608___11.Current;
							if (this._buttonList___10.Count > this._cnt___9)
							{
								this._button___13 = this._buttonList___10[this._cnt___9].GetComponent<MenuPlayerSetAbilityButton>();
								if (this._button___13 == null)
								{
									this._button___13 = this._buttonList___10[this._cnt___9].AddComponent<MenuPlayerSetAbilityButton>();
								}
								if (this._button___13 != null)
								{
									this._button___13.Setup(this.__f__this.m_charaType, this._key___12);
									this.__f__this.m_btnObjList.Add(this._key___12, this._button___13);
								}
							}
							this._cnt___9++;
						}
					}
					finally
					{
						((IDisposable)this.__s_608___11).Dispose();
					}
				}
				if (this._texChara___6 != null)
				{
					this._textureRequest___14 = new TextureRequestChara(this.__f__this.m_charaType, this._texChara___6);
					TextureAsyncLoadManager.Instance.Request(this._textureRequest___14);
					if (this.__f__this.m_charaState.IsUnlocked)
					{
						this._texChara___6.color = new Color(1f, 1f, 1f);
					}
					else
					{
						this._texChara___6.color = new Color(0f, 0f, 0f);
					}
				}
				ServerItem serverItem = new ServerItem(this.__f__this.m_charaType);
				this._charaId___15 = (int)serverItem.id;
				this._campaignData___16 = ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.CharacterUpgradeCost, this._charaId___15);
				if (this._campaignData___16 != null)
				{
					if (this._saleObj___2 != null)
					{
						this._saleObj___2.SetActive(true);
					}
				}
				else if (this._saleObj___2 != null)
				{
					this._saleObj___2.SetActive(false);
				}
				if (this._labCaption___3 != null)
				{
					this._labCaption___3.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_level_up_caption").text;
				}
				if (this._labLv___4 != null)
				{
					this._totalLevel___17 = MenuPlayerSetUtil.GetTotalLevel(this.__f__this.m_charaType);
					this._labLv___4.text = TextUtility.GetTextLevel(string.Format("{0:000}", this._totalLevel___17));
				}
				if (this._labCost___5 != null)
				{
					this._levelUpCost___18 = MenuPlayerSetUtil.GetAbilityCost(this.__f__this.m_charaType);
					this._remainCost___19 = this._levelUpCost___18 - MenuPlayerSetUtil.GetCurrentExp(this.__f__this.m_charaType);
					this._remainCost___19 = Mathf.Max(0, this._remainCost___19);
					this._labCost___5.text = HudUtility.GetFormatNumString<int>(this._remainCost___19);
				}
				if (this._imgBtn___8 != null)
				{
					this._imgBtn___8.isEnabled = !MenuPlayerSetUtil.IsCharacterLevelMax(this.__f__this.m_charaType);
				}
				if (this.init)
				{
					SoundManager.SePlay("sys_window_open", "SE");
				}
				if (this._sliExp___7 != null)
				{
					this._expRatio___20 = MenuPlayerSetUtil.GetCurrentExpRatio(this.__f__this.m_charaType);
					this._sliExp___7.value = this._expRatio___20;
					this._barObj___21 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "img_bar_body");
					if (this._barObj___21 != null)
					{
						this._barObj___21.SetActive(this._sliExp___7.value > 0f);
					}
				}
				this._PC = -1;
				break;
			}
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

	private static bool s_isActive;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIPanel m_mainPanel;

	private bool m_isClickClose;

	private bool m_isEnd;

	private ui_player_set_scroll m_parent;

	private ServerCharacterState m_charaState;

	private CharaType m_charaType = CharaType.UNKNOWN;

	private AbilityType m_currentLevelUpAbility = AbilityType.NONE;

	private Dictionary<AbilityType, int> m_lvList;

	private Dictionary<AbilityType, float> m_paramList;

	private List<AbilityType> m_abilityList;

	private bool m_lock;

	private UIRectItemStorage m_storage;

	private Dictionary<AbilityType, MenuPlayerSetAbilityButton> m_btnObjList;

	public static bool isActive
	{
		get
		{
			return PlayerLvupWindow.s_isActive;
		}
	}

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	public static bool Open(ui_player_set_scroll parent, CharaType charaType)
	{
		bool result = false;
		GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
		if (menuAnimUIObject != null)
		{
			PlayerLvupWindow playerLvupWindow = GameObjectUtil.FindChildGameObjectComponent<PlayerLvupWindow>(menuAnimUIObject, "PlayerLvupWindowUI");
			if (playerLvupWindow == null)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(menuAnimUIObject, "PlayerLvupWindowUI");
				if (gameObject != null)
				{
					playerLvupWindow = gameObject.AddComponent<PlayerLvupWindow>();
				}
			}
			if (playerLvupWindow != null)
			{
				playerLvupWindow.Setup(parent, charaType);
				result = true;
			}
		}
		return result;
	}

	private void Setup(ui_player_set_scroll parent, CharaType charaType)
	{
		PlayerLvupWindow.s_isActive = true;
		base.gameObject.SetActive(true);
		this.m_charaType = charaType;
		this.m_parent = parent;
		this.m_lock = false;
		if (this.m_abilityList == null)
		{
			this.m_abilityList = new List<AbilityType>();
			this.m_abilityList.Add(AbilityType.INVINCIBLE);
			this.m_abilityList.Add(AbilityType.COMBO);
			this.m_abilityList.Add(AbilityType.MAGNET);
			this.m_abilityList.Add(AbilityType.TRAMPOLINE);
			this.m_abilityList.Add(AbilityType.ASTEROID);
			this.m_abilityList.Add(AbilityType.LASER);
			this.m_abilityList.Add(AbilityType.DRILL);
			this.m_abilityList.Add(AbilityType.DISTANCE_BONUS);
			this.m_abilityList.Add(AbilityType.RING_BONUS);
			this.m_abilityList.Add(AbilityType.ANIMAL);
		}
		this.SetParam();
		base.StartCoroutine(this.SetupObject(true));
	}

	private void SetParam()
	{
		ServerPlayerState playerState = ServerInterface.PlayerState;
		this.m_charaState = playerState.CharacterState(this.m_charaType);
		if (this.m_lvList == null)
		{
			this.m_lvList = new Dictionary<AbilityType, int>();
		}
		else
		{
			this.m_lvList.Clear();
		}
		if (this.m_paramList == null)
		{
			this.m_paramList = new Dictionary<AbilityType, float>();
		}
		else
		{
			this.m_paramList.Clear();
		}
		ImportAbilityTable instance = ImportAbilityTable.GetInstance();
		int num = 10;
		for (int i = 0; i < num; i++)
		{
			AbilityType abilityType = (AbilityType)i;
			ServerItem.Id id = ServerItem.ConvertAbilityId(abilityType);
			int num2 = this.m_charaState.AbilityLevel[id - ServerItem.Id.INVINCIBLE];
			float abilityPotential = instance.GetAbilityPotential(abilityType, num2);
			this.m_lvList.Add(abilityType, num2);
			this.m_paramList.Add(abilityType, abilityPotential);
		}
	}

	private IEnumerator SetupObject(bool init)
	{
		PlayerLvupWindow._SetupObject_c__Iterator4D _SetupObject_c__Iterator4D = new PlayerLvupWindow._SetupObject_c__Iterator4D();
		_SetupObject_c__Iterator4D.init = init;
		_SetupObject_c__Iterator4D.___init = init;
		_SetupObject_c__Iterator4D.__f__this = this;
		return _SetupObject_c__Iterator4D;
	}

	private void OnClickLvUp()
	{
		if (this.m_parent != null && this.m_parent.parent != null && this.m_parent.parent.isTutorial)
		{
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.CHARASELECT_LEVEL_UP);
			this.m_parent.parent.SetTutorialEnd();
		}
		if (GeneralUtil.IsNetwork())
		{
			this.SendLevelUp();
		}
		else
		{
			GeneralUtil.ShowNoCommunication("ShowNoCommunication");
		}
		global::Debug.Log("OnClickLvUp");
	}

	private bool SendLevelUp()
	{
		bool result = false;
		if (!this.m_lock && this.m_charaState != null && this.m_charaType != CharaType.UNKNOWN)
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				int abilityCost = MenuPlayerSetUtil.GetAbilityCost(this.m_charaType);
				int num = abilityCost - MenuPlayerSetUtil.GetCurrentExp(this.m_charaType);
				num = Mathf.Max(0, num);
				int num2 = num;
				bool flag = true;
				if (instance.ItemData.RingCount - (uint)num2 < 0u)
				{
					flag = false;
				}
				if (flag)
				{
					BackKeyManager.InvalidFlag = true;
					this.m_currentLevelUpAbility = MenuPlayerSetUtil.GetNextLevelUpAbility(this.m_charaType);
					if (ServerInterface.LoggedInServerInterface != null && this.m_currentLevelUpAbility != AbilityType.NONE)
					{
						ServerInterface component = GameObject.Find("ServerInterface").GetComponent<ServerInterface>();
						int abilityId = MenuPlayerSetUtil.TransformServerAbilityID(this.m_currentLevelUpAbility);
						component.RequestServerUpgradeCharacter(this.m_charaState.Id, abilityId, base.gameObject);
						this.m_lock = true;
						result = true;
					}
				}
				else
				{
					GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
					string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "insufficient_ring").text;
					string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemSet", "gw_buy_ring__error_text").text;
					info.caption = text;
					info.message = text2;
					info.buttonType = GeneralWindow.ButtonType.ShopCancel;
					info.isPlayErrorSe = true;
					info.finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowCloseCallback);
					GeneralWindow.Create(info);
				}
			}
		}
		return result;
	}

	private void OnClickClose()
	{
		if (!this.m_isClickClose)
		{
			if (this.m_parent != null && this.m_parent.parent != null && this.m_parent.parent.isTutorial)
			{
				TutorialCursor.EndTutorialCursor(TutorialCursor.Type.CHARASELECT_LEVEL_UP);
				this.m_parent.parent.SetTutorialEnd();
			}
			this.m_isClickClose = true;
			PlayerLvupWindow.s_isActive = false;
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim2", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
			}
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	public void OnFinished()
	{
		PlayerLvupWindow.s_isActive = false;
		this.m_isEnd = true;
		if (this.m_mainPanel != null)
		{
			this.m_mainPanel.alpha = 0f;
		}
		base.gameObject.SetActive(false);
		base.enabled = false;
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
		if (this.m_parent != null && this.m_parent.parent != null && this.m_parent.parent.isTutorial)
		{
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.CHARASELECT_LEVEL_UP);
			this.m_parent.parent.SetTutorialEnd();
		}
		if (!this.m_isClickClose && !GeneralWindow.Created && !NetworkErrorWindow.Created)
		{
			PlayerLvupWindow.s_isActive = false;
			this.m_isClickClose = true;
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim2", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
			}
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void OnFinishedInAnim()
	{
		if (this.m_parent == null)
		{
			return;
		}
		PlayerCharaList parent = this.m_parent.parent;
		if (parent == null)
		{
			return;
		}
		if (!parent.isTutorial)
		{
			BackKeyManager.InvalidFlag = false;
		}
	}

	private void ServerUpgradeCharacter_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.m_lock = false;
		BackKeyManager.InvalidFlag = false;
		SoundManager.SePlay("sys_buy", "SE");
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		StageAbilityManager instance = StageAbilityManager.Instance;
		if (instance != null)
		{
			instance.RecalcAbilityVaue();
		}
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			this.m_charaState = playerState.CharacterState(this.m_charaType);
		}
		if (this.m_currentLevelUpAbility != AbilityType.NONE)
		{
			MenuPlayerSetAbilityButton menuPlayerSetAbilityButton = this.m_btnObjList[this.m_currentLevelUpAbility];
			if (menuPlayerSetAbilityButton != null)
			{
				menuPlayerSetAbilityButton.LevelUp(new MenuPlayerSetAbilityButton.AnimEndCallback(this.LevelUpAnimationEndCallback));
			}
		}
		this.m_currentLevelUpAbility = AbilityType.NONE;
		this.SetParam();
		base.StartCoroutine(this.SetupObject(false));
		if (this.m_parent != null)
		{
			this.m_parent.UpdateView();
		}
	}

	public void GeneralWindowCloseCallback()
	{
		global::Debug.Log("GeneralWindowCloseCallback IsOkButtonPressed:" + GeneralWindow.IsOkButtonPressed);
		if (GeneralWindow.IsYesButtonPressed)
		{
			PlayerLvupWindow.s_isActive = false;
			this.m_isClickClose = true;
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim2", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
			}
			HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.RING_TO_SHOP, false);
			this.m_lock = false;
		}
		BackKeyManager.InvalidFlag = false;
	}

	private void LevelUpAnimationEndCallback()
	{
	}
}
