using Message;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class ItemSetMenu : MonoBehaviour
{
	private enum TutorialMode
	{
		Idle,
		AppolloStartWait,
		Window1,
		Laser,
		AppolloEndWait,
		Play,
		Window2,
		Window3,
		SubChara
	}

	private enum ButtonType
	{
		NORMAL_STAGE,
		CHALLENGE_BOSS,
		SP_STAGE,
		RAID_BOSS_ATTACK,
		NUM
	}

	private sealed class _OnStart_c__Iterator32 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _anchor9___0;

		internal int _index___1;

		internal string _buttonParentName___2;

		internal GameObject _buttonParentObject___3;

		internal GameObject _ui_button___4;

		internal UIButtonMessage _button_msg___5;

		internal GameObject _ui_bet_button___6;

		internal UIButtonMessage _button_bet_msg___7;

		internal UIPlayAnimation[] _anims___8;

		internal UIPlayAnimation[] __s_540___9;

		internal int __s_541___10;

		internal UIPlayAnimation _anim___11;

		internal List<GameObject>.Enumerator __s_542___12;

		internal GameObject _o___13;

		internal int _PC;

		internal object _current;

		internal ItemSetMenu __f__this;

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
				if (ServerInterface.FreeItemState != null)
				{
					ServerInterface.FreeItemState.SetExpiredFlag(true);
				}
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._anchor9___0 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "Anchor_9_BR");
				if (this._anchor9___0 != null)
				{
					this._index___1 = 0;
					while (this._index___1 < 4)
					{
						this._buttonParentName___2 = "pattern_" + this._index___1.ToString();
						this._buttonParentObject___3 = GameObjectUtil.FindChildGameObject(this._anchor9___0, this._buttonParentName___2);
						if (!(this._buttonParentObject___3 == null))
						{
							this._ui_button___4 = GameObjectUtil.FindChildGameObject(this._buttonParentObject___3, "Btn_play");
							if (!(this._ui_button___4 == null))
							{
								this._button_msg___5 = this._ui_button___4.GetComponent<UIButtonMessage>();
								if (this._button_msg___5 == null)
								{
									this._ui_button___4.AddComponent<UIButtonMessage>();
									this._button_msg___5 = this._ui_button___4.GetComponent<UIButtonMessage>();
								}
								if (this._button_msg___5 != null)
								{
									this._button_msg___5.enabled = true;
									this._button_msg___5.trigger = UIButtonMessage.Trigger.OnClick;
									this._button_msg___5.target = this.__f__this.gameObject;
									this._button_msg___5.functionName = "OnPlayButtonClicked";
								}
								if (this._index___1 == 3)
								{
									this._ui_bet_button___6 = GameObjectUtil.FindChildGameObject(this._buttonParentObject___3, "Btn_bet");
									if (this._ui_bet_button___6 != null)
									{
										this._button_bet_msg___7 = this._ui_bet_button___6.GetComponent<UIButtonMessage>();
										if (this._button_bet_msg___7 == null)
										{
											this._ui_bet_button___6.AddComponent<UIButtonMessage>();
											this._button_bet_msg___7 = this._ui_bet_button___6.GetComponent<UIButtonMessage>();
										}
										if (this._button_bet_msg___7 != null)
										{
											this._button_bet_msg___7.enabled = true;
											this._button_bet_msg___7.trigger = UIButtonMessage.Trigger.OnClick;
											this._button_bet_msg___7.target = this.__f__this.gameObject;
											this._button_bet_msg___7.functionName = "OnBetButtonClicked";
										}
									}
								}
								this._anims___8 = this._ui_button___4.GetComponents<UIPlayAnimation>();
								if (this._anims___8 != null)
								{
									this.__s_540___9 = this._anims___8;
									this.__s_541___10 = 0;
									while (this.__s_541___10 < this.__s_540___9.Length)
									{
										this._anim___11 = this.__s_540___9[this.__s_541___10];
										if (!(this._anim___11 == null))
										{
											this._anim___11.target = null;
										}
										this.__s_541___10++;
									}
								}
							}
						}
						this._index___1++;
					}
				}
				this.__f__this.m_itemSet = GameObjectUtil.FindChildGameObjectComponent<ItemSet>(this.__f__this.gameObject, "item_set_contents");
				this.__f__this.m_instantItemSet = GameObjectUtil.FindChildGameObjectComponent<InstantItemSet>(this.__f__this.gameObject, "item_boost");
				this.__f__this.m_ringManagement = this.__f__this.gameObject.GetComponent<ItemSetRingManagement>();
				this.__f__this.m_hideGameObjects.Add(GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "item_boost"));
				this.__f__this.m_hideGameObjects.Add(GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "item_set_contents_maintenance"));
				this.__f__this.m_hideGameObjects.Add(GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "item_info"));
				this._current = null;
				this._PC = 2;
				return true;
			case 2u:
				this.__s_542___12 = this.__f__this.m_hideGameObjects.GetEnumerator();
				try
				{
					while (this.__s_542___12.MoveNext())
					{
						this._o___13 = this.__s_542___12.Current;
						if (!(this._o___13 == null))
						{
							this._o___13.SetActive(false);
						}
					}
				}
				finally
				{
					((IDisposable)this.__s_542___12).Dispose();
				}
				this._current = null;
				this._PC = 3;
				return true;
			case 3u:
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

	private const uint MAX_RAID_ATTACK_CHARGE = 3u;

	private ItemSet m_itemSet;

	private InstantItemSet m_instantItemSet;

	private ItemSetRingManagement m_ringManagement;

	private List<GameObject> m_hideGameObjects = new List<GameObject>();

	private MsgMenuItemSetStart m_msg;

	private float m_timer = 3f;

	private ItemSetMenu.TutorialMode m_tutorialMode;

	private float m_targetFrameTime = 0.0166666675f;

	private float m_timeCounter = 0.25f;

	private bool m_isRaidMenu;

	private uint m_raidChargeCount = 1u;

	private uint m_raidChargeCountMax = 3u;

	private UILabel m_ChargeText;

	private UILabel m_AttackRateText;

	private UILabel m_bossTime;

	private UILabel m_ChallengeCountLabel;

	private GameObject m_playBetObject0;

	private GameObject m_playBetObject1;

	private GameObject m_playBetMaxSign;

	private UISlider m_bossLifeBar;

	private RaidBossData m_currentRaidBossData;

	private SendApollo m_sendApollo;

	private bool m_isRaidbossTimeOver;

	private bool m_isUpdateFreeItems;

	private bool m_UpdateCharaSettingFlag;

	private bool m_isEndSetup;

	public bool IsEndSetup
	{
		get
		{
			return this.m_isEndSetup;
		}
	}

	private void Start()
	{
		base.StartCoroutine(this.OnStart());
	}

	private void OnEnable()
	{
		DeckViewWindow.Reset();
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_7_BL");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "Btn_charaset");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
				UIButtonMessage uIButtonMessage = gameObject2.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = gameObject2.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "CharaSetButtonClickedCallback";
				GeneralUtil.SetCharasetBtnIcon(base.gameObject, "Btn_charaset");
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "Btn_cmn_back");
			if (gameObject3 != null)
			{
				gameObject3.SetActive(true);
			}
		}
		bool flag = false;
		if (StageModeManager.Instance != null)
		{
			flag = StageModeManager.Instance.IsQuickMode();
		}
		ItemSetMenu.ButtonType buttonType = ItemSetMenu.ButtonType.NORMAL_STAGE;
		this.m_isRaidMenu = false;
		this.m_isRaidbossTimeOver = false;
		if (!flag)
		{
			if (EventManager.Instance.IsSpecialStage())
			{
				buttonType = ItemSetMenu.ButtonType.SP_STAGE;
			}
			else if (EventManager.Instance.IsRaidBossStage())
			{
				buttonType = ItemSetMenu.ButtonType.RAID_BOSS_ATTACK;
				this.m_currentRaidBossData = RaidBossInfo.currentRaidData;
				this.m_isRaidMenu = true;
				this.m_timeCounter = 0.25f;
				this.m_targetFrameTime = 1f / (float)Application.targetFrameRate;
			}
		}
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_9_BR");
		if (gameObject4 != null)
		{
			for (int i = 0; i < 4; i++)
			{
				string name = "pattern_" + i.ToString();
				GameObject gameObject5 = GameObjectUtil.FindChildGameObject(gameObject4, name);
				if (!(gameObject5 == null))
				{
					gameObject5.SetActive(false);
				}
			}
			string arg_1AE_0 = "pattern_";
			int num = (int)buttonType;
			string name2 = arg_1AE_0 + num.ToString();
			GameObject gameObject6 = GameObjectUtil.FindChildGameObject(gameObject4, name2);
			if (gameObject6 != null)
			{
				gameObject6.SetActive(true);
				if (EventManager.Instance.EventStage)
				{
					UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject6, "img_stage_tex");
					if (uITexture != null)
					{
						uITexture.mainTexture = EventUtility.GetBGTexture();
						BoxCollider component = uITexture.gameObject.GetComponent<BoxCollider>();
						if (component != null)
						{
							component.enabled = false;
						}
					}
				}
				else if (flag && EventManager.Instance.Type == EventManager.EventType.QUICK)
				{
					int num2 = 1;
					if (StageModeManager.Instance != null)
					{
						num2 = StageModeManager.Instance.QuickStageIndex;
					}
					UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject6, "img_next_map_cache");
					if (uISprite != null)
					{
						uISprite.gameObject.SetActive(true);
						uISprite.spriteName = "ui_mm_map_thumb_w" + num2.ToString("D2") + "a";
					}
					GameObject gameObject7 = GameObjectUtil.FindChildGameObject(gameObject6, "img_next_map");
					if (gameObject7 != null && gameObject7.activeSelf)
					{
						gameObject7.SetActive(false);
					}
					for (int j = 0; j < 3; j++)
					{
						string name3 = "img_icon_type_" + (j + 1).ToString();
						UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject6, name3);
						if (uISprite2 != null)
						{
							uISprite2.enabled = true;
							uISprite2.gameObject.SetActive(true);
							if (j == 0)
							{
								uISprite2.spriteName = "ui_chao_set_type_icon_power";
							}
							else if (j == 1)
							{
								uISprite2.spriteName = "ui_chao_set_type_icon_fly";
							}
							else if (j == 2)
							{
								uISprite2.spriteName = "ui_chao_set_type_icon_speed";
							}
						}
					}
				}
				else if (buttonType == ItemSetMenu.ButtonType.NORMAL_STAGE)
				{
					GameObject gameObject8 = GameObjectUtil.FindChildGameObject(gameObject6, "img_next_map_cache");
					if (gameObject8 != null && gameObject8.activeSelf)
					{
						gameObject8.SetActive(false);
					}
					int stageIndex = 1;
					if (MileageMapDataManager.Instance != null && StageModeManager.Instance != null)
					{
						if (!flag)
						{
							stageIndex = MileageMapDataManager.Instance.MileageStageIndex;
						}
						else
						{
							stageIndex = StageModeManager.Instance.QuickStageIndex;
						}
					}
					UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject6, "img_next_map");
					if (uISprite3 != null)
					{
						uISprite3.gameObject.SetActive(true);
						uISprite3.spriteName = "ui_mm_map_thumb_w" + stageIndex.ToString("00") + "a";
					}
					CharacterAttribute[] characterAttribute = MileageMapUtility.GetCharacterAttribute(stageIndex);
					if (characterAttribute != null)
					{
						for (int k = 0; k < 3; k++)
						{
							string name4 = "img_icon_type_" + (k + 1).ToString();
							UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject6, name4);
							if (uISprite4 != null)
							{
								if (k < characterAttribute.Length)
								{
									switch (characterAttribute[k])
									{
									case CharacterAttribute.SPEED:
										uISprite4.enabled = true;
										uISprite4.gameObject.SetActive(true);
										uISprite4.spriteName = "ui_chao_set_type_icon_speed";
										break;
									case CharacterAttribute.FLY:
										uISprite4.enabled = true;
										uISprite4.gameObject.SetActive(true);
										uISprite4.spriteName = "ui_chao_set_type_icon_fly";
										break;
									case CharacterAttribute.POWER:
										uISprite4.enabled = true;
										uISprite4.gameObject.SetActive(true);
										uISprite4.spriteName = "ui_chao_set_type_icon_power";
										break;
									default:
										uISprite4.gameObject.SetActive(false);
										uISprite4.enabled = false;
										break;
									}
								}
								else
								{
									uISprite4.gameObject.SetActive(false);
									uISprite4.enabled = false;
								}
							}
						}
					}
				}
			}
			if (this.m_isRaidMenu)
			{
				string arg_591_0 = "pattern_";
				int num3 = (int)buttonType;
				string name5 = arg_591_0 + num3.ToString();
				GameObject parent = GameObjectUtil.FindChildGameObject(gameObject4, name5);
				this.m_raidChargeCount = 1u;
				this.m_raidChargeCountMax = 1u;
				this.m_ChallengeCountLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_EVchallenge");
				this.m_ChargeText = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_bet_number");
				this.m_AttackRateText = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_strike_power");
				this.m_playBetObject0 = GameObjectUtil.FindChildGameObject(parent, "bet_0");
				if (this.m_playBetObject0 != null)
				{
					this.m_playBetObject0.SetActive(true);
				}
				this.m_playBetObject1 = GameObjectUtil.FindChildGameObject(parent, "bet_1");
				if (this.m_playBetObject1 != null)
				{
					this.m_playBetObject1.SetActive(false);
				}
				this.m_playBetMaxSign = GameObjectUtil.FindChildGameObject(parent, "max_sign");
				if (this.m_playBetMaxSign != null)
				{
					this.m_playBetMaxSign.SetActive(false);
				}
				this.UpdateRaidbossChallangeCountView();
				if (this.m_currentRaidBossData != null)
				{
					UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_boss_name");
					if (uILabel != null)
					{
						uILabel.text = this.m_currentRaidBossData.name;
					}
					UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_boss_name_sh");
					if (uILabel2 != null)
					{
						uILabel2.text = this.m_currentRaidBossData.name;
					}
					UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_boss_lv");
					if (uILabel3 != null)
					{
						uILabel3.text = "Lv." + this.m_currentRaidBossData.lv.ToString();
					}
					GameObject gameObject9 = GameObjectUtil.FindChildGameObject(parent, "img_boss_icon");
					if (gameObject9 != null)
					{
						UISprite component2 = gameObject9.GetComponent<UISprite>();
						if (component2 != null)
						{
							component2.spriteName = "ui_gp_gauge_boss_icon_raid_" + this.m_currentRaidBossData.rarity;
						}
						UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject9, "img_boss_icon_bg");
						if (uISprite5 != null)
						{
							uISprite5.spriteName = "ui_event_raidboss_window_bosslevel_" + this.m_currentRaidBossData.rarity;
						}
					}
					UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_boss_life");
					if (uILabel4 != null)
					{
						uILabel4.text = this.m_currentRaidBossData.hp.ToString() + "/" + this.m_currentRaidBossData.hpMax.ToString();
					}
					this.m_bossTime = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_boss_time");
					if (this.m_bossTime != null)
					{
						this.m_bossTime.text = this.m_currentRaidBossData.GetTimeLimitString(true);
					}
					this.m_bossLifeBar = GameObjectUtil.FindChildGameObjectComponent<UISlider>(parent, "Pgb_BossLife");
					if (this.m_bossLifeBar != null)
					{
						this.m_bossLifeBar.value = this.m_currentRaidBossData.GetHpRate();
						this.m_bossLifeBar.numberOfSteps = 1;
						this.m_bossLifeBar.ForceUpdate();
					}
					UISprite uISprite6 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(parent, "img_boss_bg");
					if (uISprite6 != null)
					{
						if (this.m_currentRaidBossData.IsDiscoverer())
						{
							uISprite6.spriteName = "ui_event_raidboss_window_boss_bar_1";
						}
						else
						{
							uISprite6.spriteName = "ui_event_raidboss_window_boss_bar_0";
						}
					}
				}
			}
			else
			{
				GameObject gameObject10 = GameObjectUtil.FindChildGameObject(gameObject4, "pattern_0");
				if (gameObject10 != null)
				{
					string name6 = "img_word_play";
					GameObject gameObject11 = GameObjectUtil.FindChildGameObject(gameObject10, name6);
					if (gameObject11 != null)
					{
						UISprite component3 = gameObject11.GetComponent<UISprite>();
						if (component3 != null)
						{
							if (MileageMapUtility.IsBossStage() && !flag)
							{
								component3.spriteName = "ui_mm_btn_word_play_boss";
							}
							else
							{
								component3.spriteName = "ui_mm_btn_word_play";
							}
						}
					}
				}
			}
		}
	}

	private void UpdateRaidbossChallangeCountView()
	{
		if (this.m_isRaidMenu)
		{
			this.m_raidChargeCountMax = (uint)EventManager.Instance.RaidbossChallengeCount;
			if (this.m_ChallengeCountLabel != null)
			{
				this.m_ChallengeCountLabel.text = "/" + this.m_raidChargeCountMax.ToString();
			}
			if (this.m_raidChargeCountMax > 3u)
			{
				this.m_raidChargeCountMax = 3u;
			}
			if (this.m_ChargeText != null)
			{
				this.m_ChargeText.text = "x" + this.m_raidChargeCount.ToString();
			}
		}
	}

	private IEnumerator OnStart()
	{
		ItemSetMenu._OnStart_c__Iterator32 _OnStart_c__Iterator = new ItemSetMenu._OnStart_c__Iterator32();
		_OnStart_c__Iterator.__f__this = this;
		return _OnStart_c__Iterator;
	}

	private void Update()
	{
		switch (this.m_tutorialMode)
		{
		case ItemSetMenu.TutorialMode.AppolloStartWait:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
				this.CreateTutorialWindow(0);
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Window1;
			}
			break;
		case ItemSetMenu.TutorialMode.Window1:
			if (GeneralWindow.IsCreated("ItemTutorial") && GeneralWindow.IsButtonPressed)
			{
				HudMenuUtility.SetConnectAlertSimpleUI(false);
				GeneralWindow.Close();
				TutorialCursor.StartTutorialCursor(TutorialCursor.Type.ITEMSELECT_LASER);
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Laser;
			}
			break;
		case ItemSetMenu.TutorialMode.Laser:
			if (this.IsEndTutorialLaser())
			{
				TutorialCursor.StartTutorialCursor(TutorialCursor.Type.MAINMENU_PLAY);
				this.SetEnablePlayButton(true);
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Play;
			}
			break;
		case ItemSetMenu.TutorialMode.AppolloEndWait:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Window2;
				this.CreateTutorialWindow(1);
			}
			break;
		case ItemSetMenu.TutorialMode.Window2:
			if (GeneralWindow.IsCreated("ItemTutorial2") && GeneralWindow.IsButtonPressed)
			{
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.PLAY_ITEM, false);
				GeneralWindow.Close();
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Idle;
			}
			break;
		case ItemSetMenu.TutorialMode.Window3:
			if (GeneralWindow.IsCreated("ItemTutorial3") && GeneralWindow.IsButtonPressed)
			{
				HudMenuUtility.SetConnectAlertSimpleUI(false);
				GeneralWindow.Close();
				TutorialCursor.StartTutorialCursor(TutorialCursor.Type.ITEMSELECT_SUBCHARA);
				this.m_timer = 3f;
				this.m_tutorialMode = ItemSetMenu.TutorialMode.SubChara;
			}
			break;
		case ItemSetMenu.TutorialMode.SubChara:
			this.m_timer -= Time.deltaTime;
			if (TutorialCursor.IsTouchScreen() || this.m_timer < 0f)
			{
				TutorialCursor.DestroyTutorialCursor();
				HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.SUB_CHARA_ITEM_EXPLAINED);
				this.SetEnablePlayButton(true);
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Idle;
			}
			break;
		}
		if (this.m_isRaidMenu)
		{
			if (!this.m_isRaidbossTimeOver)
			{
				this.m_timeCounter -= this.m_targetFrameTime;
				if (this.m_timeCounter <= 0f)
				{
					if (this.m_bossTime != null && this.m_currentRaidBossData != null)
					{
						this.m_bossTime.text = this.m_currentRaidBossData.GetTimeLimitString(true);
					}
					this.m_timeCounter = 0.25f;
				}
			}
			else if (GeneralWindow.IsCreated("RaidbossTimeOver") && GeneralWindow.IsButtonPressed)
			{
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.ITEM_BACK, false);
				GeneralWindow.Close();
			}
		}
		if (GeneralWindow.IsCreated("RingMissing"))
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.RING_TO_SHOP, false);
				this.OnMsgMenuBack();
				GeneralWindow.Close();
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				GeneralWindow.Close();
			}
		}
	}

	private void ServerGetRingExchangeList_Succeeded(MsgGetRingExchangeListSucceed msg)
	{
		this.Setup();
	}

	private void GetFreeItemList()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		this.m_isUpdateFreeItems = false;
		if (loggedInServerInterface != null)
		{
			ServerFreeItemState freeItemState = ServerInterface.FreeItemState;
			if (freeItemState.IsExpired())
			{
				loggedInServerInterface.RequestServerGetFreeItemList(base.gameObject);
				this.m_isUpdateFreeItems = true;
			}
		}
	}

	private void ServerGetFreeItemList_Succeeded(MsgGetFreeItemListSucceed msg)
	{
		if (this.m_instantItemSet != null)
		{
			this.m_instantItemSet.UpdateFreeItemList(msg.m_freeItemState);
			this.m_instantItemSet.SetupBoostedItem();
		}
		if (this.m_itemSet != null)
		{
			this.m_itemSet.UpdateFreeItemList(msg.m_freeItemState);
			this.m_itemSet.SetupEquipItem();
		}
	}

	private void OnMsgMenuItemSetStart(MsgMenuItemSetStart msg)
	{
		this.ServerGetRingExchangeList_Succeeded(null);
		switch (msg.m_setMode)
		{
		case MsgMenuItemSetStart.SetMode.NORMAL:
			this.m_tutorialMode = ItemSetMenu.TutorialMode.Idle;
			this.CheckFreeItemAndEquip();
			break;
		case MsgMenuItemSetStart.SetMode.TUTORIAL:
		{
			BackKeyManager.TutorialFlag = true;
			HudMenuUtility.SetConnectAlertSimpleUI(true);
			this.SetEnablePlayButton(false);
			this.ClearEquipedItemForTutorial();
			string[] value = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP4, ref value);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_START, value);
			this.m_tutorialMode = ItemSetMenu.TutorialMode.AppolloStartWait;
			break;
		}
		case MsgMenuItemSetStart.SetMode.TUTORIAL_SUBCHARA:
		{
			uint num = 0u;
			if (SaveDataManager.Instance != null)
			{
				num = SaveDataManager.Instance.PlayerData.ChallengeCount;
			}
			if (num == 0u)
			{
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Idle;
			}
			else
			{
				HudMenuUtility.SetConnectAlertSimpleUI(true);
				this.SetEnablePlayButton(false);
				this.m_tutorialMode = ItemSetMenu.TutorialMode.Window3;
				this.CreateTutorialWindow(2);
			}
			this.CheckFreeItemAndEquip();
			break;
		}
		}
		this.m_msg = msg;
		if (this.m_isRaidMenu)
		{
			this.UpdateRaidbossChallangeCountView();
		}
		this.OnEnable();
	}

	private void CheckFreeItemAndEquip()
	{
		this.GetFreeItemList();
		if (!this.m_isUpdateFreeItems)
		{
			ServerFreeItemState freeItemState = ServerInterface.FreeItemState;
			if (freeItemState != null)
			{
				if (this.m_instantItemSet != null)
				{
					this.m_instantItemSet.UpdateFreeItemList(freeItemState);
				}
				if (this.m_itemSet != null)
				{
					this.m_itemSet.UpdateFreeItemList(freeItemState);
				}
			}
			if (this.m_instantItemSet != null)
			{
				this.m_instantItemSet.SetupBoostedItem();
			}
			if (this.m_itemSet != null)
			{
				this.m_itemSet.SetupEquipItem();
			}
		}
	}

	private void ClearEquipedItemForTutorial()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			PlayerData playerData = instance.PlayerData;
			if (playerData != null)
			{
				playerData.BoostedItem[0] = false;
				playerData.BoostedItem[2] = false;
				playerData.BoostedItem[1] = false;
				playerData.EquippedItem[0] = ItemType.UNKNOWN;
				playerData.EquippedItem[1] = ItemType.UNKNOWN;
				playerData.EquippedItem[2] = ItemType.UNKNOWN;
				if (this.m_instantItemSet != null)
				{
					this.m_instantItemSet.SetupBoostedItem();
				}
				if (this.m_itemSet != null)
				{
					this.m_itemSet.SetupEquipItem();
				}
			}
		}
	}

	private void OnMsgMenuBack()
	{
		this.m_instantItemSet.ResetCheckMark();
		this.m_itemSet.ResetCheckMark();
		if (this.m_UpdateCharaSettingFlag)
		{
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			this.m_UpdateCharaSettingFlag = false;
		}
	}

	private void OnPlayButtonClicked(GameObject obj)
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_msg != null && this.m_msg.m_setMode == MsgMenuItemSetStart.SetMode.TUTORIAL)
		{
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.MAINMENU_PLAY);
			this.m_tutorialMode = ItemSetMenu.TutorialMode.AppolloEndWait;
			string[] value = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP4, ref value);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value);
		}
		else if (this.m_ringManagement != null)
		{
			if (this.m_ringManagement.GetDisplayRingCount() >= 0)
			{
				if (this.m_isRaidMenu)
				{
					if (this.m_currentRaidBossData.IsLimit())
					{
						GeneralWindow.Create(new GeneralWindow.CInfo
						{
							name = "RaidbossTimeOver",
							buttonType = GeneralWindow.ButtonType.Ok,
							caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_raid_boss_bye"),
							message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_raid_boss_bye2"),
							anchor_path = "Camera/menu_Anim/ItemSet_3_UI/Anchor_5_MC"
						});
						this.m_isRaidbossTimeOver = true;
					}
					else
					{
						uint raidbossChallengeCount = (uint)EventManager.Instance.RaidbossChallengeCount;
						if (this.m_raidChargeCount <= raidbossChallengeCount)
						{
							EventManager.Instance.UseRaidbossChallengeCount = (int)this.m_raidChargeCount;
							HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.PLAY_ITEM, false);
						}
					}
				}
				else
				{
					HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.PLAY_ITEM, false);
				}
			}
			else
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "RingMissing",
					caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "gw_cost_caption").text,
					message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "gw_cost_text").text,
					anchor_path = "Camera/menu_Anim/ItemSet_3_UI/Anchor_5_MC",
					buttonType = GeneralWindow.ButtonType.ShopCancel
				});
			}
		}
	}

	private void OnBetButtonClicked(GameObject obj)
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		uint raidChargeCount = this.m_raidChargeCount;
		if (this.m_raidChargeCountMax <= 1u)
		{
			this.m_raidChargeCount = 1u;
		}
		else if (this.m_raidChargeCount < this.m_raidChargeCountMax)
		{
			this.m_raidChargeCount += 1u;
		}
		else
		{
			this.m_raidChargeCount = 1u;
		}
		if (raidChargeCount != this.m_raidChargeCount)
		{
			this.m_ChargeText.text = "x" + this.m_raidChargeCount.ToString();
			if (this.m_raidChargeCount == 3u)
			{
				this.m_playBetObject0.SetActive(false);
				this.m_playBetObject1.SetActive(true);
				this.m_playBetMaxSign.SetActive(true);
				this.SetRaidAttackRate();
			}
			else if (this.m_raidChargeCount > 1u)
			{
				this.m_playBetObject0.SetActive(false);
				this.m_playBetObject1.SetActive(true);
				this.m_playBetMaxSign.SetActive(false);
				this.SetRaidAttackRate();
			}
			else
			{
				this.m_playBetObject0.SetActive(true);
				this.m_playBetObject1.SetActive(false);
				this.m_playBetMaxSign.SetActive(false);
			}
		}
	}

	private void SetRaidAttackRate()
	{
		if (this.m_raidChargeCount > 0u)
		{
			float raidAttackRate = EventManager.Instance.GetRaidAttackRate((int)this.m_raidChargeCount);
			if (this.m_AttackRateText != null)
			{
				this.m_AttackRateText.text = "x" + raidAttackRate.ToString();
			}
		}
	}

	private void CreateTutorialWindow(int index)
	{
		string str = string.Empty;
		if (index > 0)
		{
			str = (index + 1).ToString();
		}
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "ItemTutorial" + str,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ShopItem", "tutorial_comment_caption" + str).text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ShopItem", "tutorial_comment_text" + str).text,
			anchor_path = "Camera/menu_Anim/ItemSet_3_UI/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.Ok
		});
	}

	private void Setup()
	{
		foreach (GameObject current in this.m_hideGameObjects)
		{
			if (!(current == null))
			{
				current.SetActive(true);
			}
		}
		if (this.m_instantItemSet != null)
		{
			this.m_instantItemSet.Setup();
		}
		if (this.m_itemSet != null)
		{
			this.m_itemSet.Setup();
		}
		this.m_UpdateCharaSettingFlag = false;
		this.m_isEndSetup = true;
	}

	private bool IsEndTutorialLaser()
	{
		if (this.m_itemSet != null)
		{
			ItemType[] item = this.m_itemSet.GetItem();
			if (item != null)
			{
				ItemType[] array = item;
				for (int i = 0; i < array.Length; i++)
				{
					ItemType itemType = array[i];
					if (itemType == ItemType.LASER)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private void CharaSetButtonClickedCallback()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		DeckViewWindow.Create(base.gameObject);
	}

	private void OnMsgReset()
	{
		if (StageAbilityManager.Instance != null)
		{
			StageAbilityManager.Instance.RecalcAbilityVaue();
			this.m_UpdateCharaSettingFlag = true;
		}
		if (this.m_itemSet != null)
		{
			this.m_itemSet.UpdateView();
		}
	}

	private void SetEnablePlayButton(bool enabledFlag)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_7_BL");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "Btn_cmn_back");
			if (gameObject2 != null)
			{
				BoxCollider component = gameObject2.GetComponent<BoxCollider>();
				if (component != null)
				{
					component.isTrigger = !enabledFlag;
				}
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "Btn_charaset");
			if (gameObject3 != null)
			{
				BoxCollider component2 = gameObject3.GetComponent<BoxCollider>();
				if (component2 != null)
				{
					component2.isTrigger = !enabledFlag;
				}
			}
		}
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_9_BR");
		if (gameObject4 != null)
		{
			GameObject x = GameObjectUtil.FindChildGameObject(gameObject4, "pattern_0");
			if (x != null)
			{
				GameObject gameObject5 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_play");
				if (gameObject5 != null)
				{
					BoxCollider component3 = gameObject5.GetComponent<BoxCollider>();
					if (component3 != null)
					{
						component3.isTrigger = !enabledFlag;
					}
				}
			}
		}
	}

	public void UpdateBoostButton()
	{
		if (this.m_instantItemSet != null)
		{
			this.m_instantItemSet.ResetCheckMark();
			this.m_instantItemSet.Setup();
			this.m_instantItemSet.SetupBoostedItem();
		}
	}

	public static void UpdateBoostItemForCharacterDeck()
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			ItemSetMenu itemSetMenu = GameObjectUtil.FindChildGameObjectComponent<ItemSetMenu>(cameraUIObject, "ItemSet_3_UI");
			if (itemSetMenu != null)
			{
				itemSetMenu.UpdateBoostButton();
			}
		}
	}
}
