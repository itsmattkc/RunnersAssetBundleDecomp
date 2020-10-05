using AnimationOrTween;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RaidBossWindow : EventWindowBase
{
	private enum BUTTON_ACT
	{
		CLOSE,
		BOSS_PLAY,
		BOSS_INFO,
		BOSS_REWARD,
		INFO,
		ROULETTE,
		CHALLENGE,
		SHOP_RSRING,
		SHOP_RING,
		SHOP_CHALLENGE,
		NONE
	}

	public enum WINDOW_OPEN_MODE
	{
		NONE,
		ADVENT_BOSS_NORMAL,
		ADVENT_BOSS_RARE,
		ADVENT_BOSS_S_RARE
	}

	public const float RELOAD_TIME = 5f;

	public const float WAIT_LIMIT_TIME = 300f;

	[SerializeField]
	private UIPanel mainPanel;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIDraggablePanel m_listPanel;

	[SerializeField]
	private UILabel m_crushCount;

	[SerializeField]
	private UILabel m_raidRingCount;

	[SerializeField]
	private RaidEnergyStorage m_energy;

	[SerializeField]
	private GameObject m_advent;

	[SerializeField]
	private UITexture m_bgTexture;

	[SerializeField]
	private GameObject eventEndObject;

	private RaidBossInfo m_rbInfoData;

	private bool m_isLoading;

	private float m_time;

	private bool m_opened;

	private bool m_close;

	private bool m_alertCollider;

	private RaidBossWindow.BUTTON_ACT m_btnAct = RaidBossWindow.BUTTON_ACT.NONE;

	private UIRectItemStorage m_storage;

	private UIButton[] m_btnReload;

	private bool m_isBossAttention;

	private bool m_isMyBoss;

	private RaidBossWindow.WINDOW_OPEN_MODE m_openMode;

	private Animation m_bossAnim;

	private static RaidBossWindow s_instance;

	public bool isLoading
	{
		get
		{
			return this.m_isLoading;
		}
	}

	public float time
	{
		get
		{
			return this.m_time;
		}
	}

	private static RaidBossWindow Instance
	{
		get
		{
			return RaidBossWindow.s_instance;
		}
	}

	private void Update()
	{
		this.m_time += Time.deltaTime;
		if (this.m_time >= 3.40282347E+38f)
		{
			this.m_time = 1000f;
		}
		this.CheckReloadBtn(this.m_time);
		if (GeneralWindow.IsCreated("RaidbossChallengeMissing"))
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				GeneralWindow.Close();
				this.OnClickEvChallenge();
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				GeneralWindow.Close();
			}
		}
	}

	private void CheckReloadBtn(float time)
	{
		if (time <= 5f && this.m_btnReload != null && this.m_btnReload.Length > 0)
		{
			UIButton[] btnReload = this.m_btnReload;
			for (int i = 0; i < btnReload.Length; i++)
			{
				UIButton uIButton = btnReload[i];
				uIButton.isEnabled = false;
			}
		}
		else if (time > 5f && this.m_btnReload != null && this.m_btnReload.Length > 0)
		{
			UIButton[] btnReload2 = this.m_btnReload;
			for (int j = 0; j < btnReload2.Length; j++)
			{
				UIButton uIButton2 = btnReload2[j];
				uIButton2.isEnabled = true;
			}
		}
	}

	private void UpdateList()
	{
		this.m_time = 0f;
		this.m_isMyBoss = false;
		if (this.m_energy != null)
		{
			this.m_energy.Init();
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_energy.gameObject, "img_sale_icon_challenge");
		if (gameObject != null)
		{
			bool flag = true;
			if (EventManager.Instance != null)
			{
				flag = EventManager.Instance.IsChallengeEvent();
			}
			if (HudMenuUtility.IsSale(Constants.Campaign.emType.PurchaseAddRaidEnergys) && flag)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		this.m_rbInfoData = null;
		if (EventManager.Instance != null)
		{
			this.m_rbInfoData = EventManager.Instance.RaidBossInfo;
		}
		if (this.m_rbInfoData != null)
		{
			if (this.m_crushCount != null)
			{
				this.m_crushCount.text = HudUtility.GetFormatNumString<long>(this.m_rbInfoData.totalDestroyCount);
			}
			if (this.m_raidRingCount != null)
			{
				this.m_raidRingCount.text = HudUtility.GetFormatNumString<long>(GeneralUtil.GetItemCount(ServerItem.Id.RAIDRING));
			}
			if (this.m_listPanel != null)
			{
				List<RaidBossData> raidData = this.m_rbInfoData.raidData;
				List<RaidBossData> list = new List<RaidBossData>();
				List<RaidBossData> list2 = new List<RaidBossData>();
				List<RaidBossData> list3 = new List<RaidBossData>();
				List<RaidBossData> list4 = new List<RaidBossData>();
				List<RaidBossData> list5 = new List<RaidBossData>();
				if (raidData != null)
				{
					for (int i = 0; i < raidData.Count; i++)
					{
						if (raidData[i] != null)
						{
							if (raidData[i].end && !raidData[i].IsDiscoverer())
							{
								if (!raidData[i].participation)
								{
									list5.Add(raidData[i]);
								}
								else if (raidData[i].clear)
								{
									list3.Add(raidData[i]);
								}
								else
								{
									list4.Add(raidData[i]);
								}
							}
							else if (raidData[i].IsDiscoverer())
							{
								list.Add(raidData[i]);
								this.m_isMyBoss = true;
							}
							else
							{
								list2.Add(raidData[i]);
							}
						}
					}
				}
				if (list2.Count > 0)
				{
					foreach (RaidBossData current in list2)
					{
						list.Add(current);
					}
				}
				if (list3.Count > 0)
				{
					foreach (RaidBossData current2 in list3)
					{
						list.Add(current2);
					}
				}
				if (list4.Count > 0)
				{
					foreach (RaidBossData current3 in list4)
					{
						list.Add(current3);
					}
				}
				if (list5.Count > 0)
				{
					ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
					int id = EventManager.Instance.Id;
					if (loggedInServerInterface != null)
					{
						foreach (RaidBossData current4 in list5)
						{
							loggedInServerInterface.RequestServerGetEventRaidBossUserList(id, current4.id, base.gameObject);
						}
					}
				}
				this.m_storage = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_listPanel.gameObject, "slot");
				if (this.m_storage != null)
				{
					this.m_storage.maxRows = list.Count;
					this.m_storage.Restart();
					if (list.Count > 0)
					{
						List<ui_event_raid_scroll> list6 = GameObjectUtil.FindChildGameObjectsComponents<ui_event_raid_scroll>(this.m_listPanel.gameObject, "ui_event_raid_scroll(Clone)");
						if (list6 != null && list6.Count > 0)
						{
							for (int j = 0; j < list6.Count; j++)
							{
								if (j >= list.Count)
								{
									break;
								}
								list6[j].UpdateView(list[j], this, true);
							}
						}
						this.m_listPanel.Scroll(1f);
						this.m_listPanel.ResetPosition();
					}
				}
				if (this.eventEndObject != null)
				{
					GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_charge_challenge");
					GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_roulette");
					if (!EventManager.Instance.IsChallengeEvent() && list.Count <= 0)
					{
						this.eventEndObject.SetActive(true);
						UIButtonMessage componentInChildren = this.eventEndObject.GetComponentInChildren<UIButtonMessage>();
						if (componentInChildren != null)
						{
							componentInChildren.target = base.gameObject;
							componentInChildren.functionName = "OnClickRouletteRaid";
						}
						UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.eventEndObject, "Lbl_expdate");
						if (uILabel != null)
						{
							DateTime eventCloseTime = EventManager.Instance.EventCloseTime;
							string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "event_finished_guidance_2").text;
							if (!string.IsNullOrEmpty(text))
							{
								uILabel.text = text.Replace("{DATE}", eventCloseTime.ToString());
							}
						}
						if (gameObject2 != null)
						{
							gameObject2.SetActive(false);
						}
						if (gameObject3 != null)
						{
							gameObject3.SetActive(false);
						}
					}
					else
					{
						this.eventEndObject.SetActive(false);
						if (gameObject2 != null)
						{
							gameObject2.SetActive(true);
						}
						if (gameObject3 != null)
						{
							gameObject3.SetActive(true);
						}
					}
				}
			}
		}
	}

	public void Setup(RaidBossInfo info, RaidBossWindow.WINDOW_OPEN_MODE mode)
	{
		RaidBossInfo.currentRaidData = null;
		this.m_isLoading = false;
		this.m_isBossAttention = false;
		this.m_isMyBoss = false;
		this.m_opened = false;
		this.m_openMode = mode;
		this.mainPanel.alpha = 1f;
		this.SetObject();
		this.SetAlertSimpleUI(true);
		HudMenuUtility.SendEnableShopButton(true);
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_eventmenu_intro", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			SoundManager.SePlay("sys_window_open", "SE");
		}
		Texture bGTexture = EventUtility.GetBGTexture();
		if (bGTexture != null && this.m_bgTexture != null)
		{
			this.m_bgTexture.mainTexture = bGTexture;
		}
		base.enabledAnchorObjects = true;
		info.callback = new RaidBossInfo.CallbackRaidBossInfoUpdate(this.CallbackInfoUpdate);
		this.UpdateList();
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.NONE;
		this.m_close = false;
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	protected override void SetObject()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_reload");
		if (gameObject != null)
		{
			this.m_btnReload = gameObject.GetComponents<UIButton>();
		}
		this.ShowBossAdvent(this.m_openMode != RaidBossWindow.WINDOW_OPEN_MODE.NONE);
		GeneralUtil.SetRouletteBtnIcon(base.gameObject, "Btn_roulette");
	}

	private void ShowBossAdvent(bool adventActive)
	{
		if (this.m_advent != null)
		{
			this.m_advent.SetActive(adventActive);
			if (adventActive)
			{
				RaidBossInfo raidBossInfo = EventManager.Instance.RaidBossInfo;
				RaidBossData raidBossData = null;
				if (raidBossInfo != null)
				{
					List<RaidBossData> raidData = raidBossInfo.raidData;
					if (raidData != null && raidData.Count > 0)
					{
						foreach (RaidBossData current in raidData)
						{
							if (current.IsDiscoverer())
							{
								raidBossData = current;
								break;
							}
						}
					}
				}
				this.m_bossAnim = GameObjectUtil.FindChildGameObjectComponent<Animation>(this.m_advent.gameObject, "bit_Anim");
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_advent.gameObject, "Lbl_boss_level");
				List<UISprite> list = null;
				for (int i = 0; i < 5; i++)
				{
					UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_advent.gameObject, "boss_icon" + i);
					if (!(uISprite != null))
					{
						break;
					}
					if (list == null)
					{
						list = new List<UISprite>();
					}
					list.Add(uISprite);
				}
				if (raidBossData != null)
				{
					if (uILabel != null)
					{
						string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_LevelNumber").text;
						uILabel.text = text.Replace("{PARAM}", raidBossData.lv.ToString());
					}
					if (list != null)
					{
						foreach (UISprite current2 in list)
						{
							current2.spriteName = "ui_event_raidboss_icon_silhouette_" + raidBossData.rarity;
							current2.color = new Color(1f, 1f, 1f, current2.alpha);
						}
					}
				}
				else
				{
					this.m_advent.SetActive(false);
				}
			}
		}
		if (this.m_openMode == RaidBossWindow.WINDOW_OPEN_MODE.NONE && adventActive)
		{
			this.StartRaidbossAttentionAnim();
		}
	}

	private void StartRaidbossAttentionAnim()
	{
		ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_bossAnim, "ui_EventResult_raidboss_attention_intro_Anim", Direction.Forward);
		EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.BossAnimationIntroCallback), true);
		SoundManager.SePlay("sys_boss_warning", "SE");
	}

	public void OnClickEvChallenge()
	{
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.CHALLENGE;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_cmn_back");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
		HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.RAIDENERGY_TO_SHOP, false);
	}

	public void OnClickNo()
	{
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.CLOSE;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_cmn_back");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	public void OnClickNoBg()
	{
	}

	public void OnClickReload()
	{
		if (GeneralUtil.IsNetwork())
		{
			if (this.m_time > 5f)
			{
				RaidBossInfo.currentRaidData = null;
				this.RequestServerGetEventUserRaidBossList();
				SoundManager.SePlay("sys_menu_decide", "SE");
			}
		}
		else
		{
			this.ShowNoCommunication();
		}
	}

	public void RequestServerGetEventUserRaidBossList()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			int eventId = 0;
			if (EventManager.Instance != null)
			{
				eventId = EventManager.Instance.Id;
			}
			this.m_isLoading = true;
			this.SetAlertSimpleUI(true);
			loggedInServerInterface.RequestServerGetEventUserRaidBossList(eventId, base.gameObject);
		}
		else
		{
			this.ServerGetEventUserRaidBossList_Succeeded(null);
		}
	}

	private void ServerGetEventUserRaidBossList_Succeeded(MsgGetEventUserRaidBossListSucceed msg)
	{
		this.m_isLoading = false;
		if (RaidBossInfo.currentRaidData == null)
		{
			this.SetAlertSimpleUI(false);
			this.listReload();
		}
		else if (RaidBossInfo.currentRaidData.end)
		{
			this.SetAlertSimpleUI(false);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "BossEnd",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_raid_boss_bye"),
				message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_raid_boss_bye2")
			});
			this.listReload();
		}
		else
		{
			this.SetAlertSimpleUI(true);
			this.m_btnAct = RaidBossWindow.BUTTON_ACT.BOSS_PLAY;
			this.m_close = true;
			SoundManager.SePlay("sys_window_close", "SE");
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_cmn_back");
			UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
			if (component != null)
			{
				EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
				component.Play(true);
			}
		}
	}

	private void ServerGetEventUserRaidBossList_Failed(MsgServerConnctFailed msg)
	{
		this.m_isLoading = false;
		this.SetAlertSimpleUI(false);
	}

	public void listReload()
	{
		this.m_time = 0f;
		this.UpdateList();
	}

	private void CallbackInfoUpdate(RaidBossInfo info)
	{
		this.UpdateList();
	}

	public void OnClickBossPlayButton(RaidBossData bossData)
	{
		if (this.m_energy != null && this.m_energy.energyCount <= 0u)
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "RaidbossChallengeMissing",
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "no_challenge_raid_count").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "no_challenge_raid_count_info").text,
				anchor_path = "Camera/RaidBossWindowUI/Anchor_5_MC",
				buttonType = GeneralWindow.ButtonType.ShopCancel
			});
			return;
		}
		this.SetAlertSimpleUI(true);
		RaidBossInfo.currentRaidData = bossData;
		TimeSpan timeLimit = bossData.GetTimeLimit();
		if (this.time >= 300f || timeLimit.Ticks <= 0L)
		{
			this.RequestServerGetEventUserRaidBossList();
		}
		this.m_energy.ReflectChallengeCount();
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.BOSS_PLAY;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_cmn_back");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	public void OnClickBossInfoButton(RaidBossData bossData)
	{
		RaidBossInfo.currentRaidData = bossData;
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.BOSS_INFO;
		SoundManager.SePlay("sys_menu_decide", "SE");
		RaidBossDamageRewardWindow.Create(bossData, this);
	}

	public void OnClickBossRewardButton(RaidBossData bossData)
	{
		RaidBossInfo.currentRaidData = bossData;
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.BOSS_REWARD;
		SoundManager.SePlay("sys_menu_decide", "SE");
		RaidBossDamageRewardWindow.Create(bossData, this);
	}

	public void OnClickBossInfoBackButton(RaidBossData bossData)
	{
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.NONE;
	}

	public void OnClickReward()
	{
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.INFO;
		if (EventManager.Instance != null)
		{
			EventRewardWindow.Create(EventManager.Instance.RaidBossInfo);
		}
	}

	public void OnClickRoulette()
	{
		this.SetAlertSimpleUI(true);
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.ROULETTE;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_roulette");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	public void OnClickRouletteRaid()
	{
		if (EventManager.Instance != null && EventManager.Instance.TypeInTime != EventManager.EventType.RAID_BOSS)
		{
			GeneralUtil.ShowEventEnd("ShowEventEnd");
			return;
		}
		this.SetAlertSimpleUI(true);
		RouletteUtility.rouletteDefault = RouletteCategory.RAID;
		this.m_btnAct = RaidBossWindow.BUTTON_ACT.ROULETTE;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_raid_roulette");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	public void OnClickAdventBg()
	{
		if (this.m_advent != null)
		{
			this.m_advent.SetActive(false);
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	public void OnClickEndButton(ButtonInfoTable.ButtonType btnType)
	{
		this.SetAlertSimpleUI(true);
		switch (btnType)
		{
		case ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP:
			this.m_btnAct = RaidBossWindow.BUTTON_ACT.SHOP_RSRING;
			goto IL_7C;
		case ButtonInfoTable.ButtonType.RING_TO_SHOP:
			this.m_btnAct = RaidBossWindow.BUTTON_ACT.SHOP_RING;
			goto IL_7C;
		case ButtonInfoTable.ButtonType.CHALLENGE_TO_SHOP:
			this.m_btnAct = RaidBossWindow.BUTTON_ACT.SHOP_CHALLENGE;
			goto IL_7C;
		case ButtonInfoTable.ButtonType.RAIDENERGY_TO_SHOP:
		case ButtonInfoTable.ButtonType.EVENT_RAID:
		case ButtonInfoTable.ButtonType.EVENT_SPECIAL:
		case ButtonInfoTable.ButtonType.EVENT_COLLECT:
			IL_32:
			if (btnType != ButtonInfoTable.ButtonType.REWARDLIST_TO_CHAO_ROULETTE)
			{
				goto IL_7C;
			}
			this.m_btnAct = RaidBossWindow.BUTTON_ACT.ROULETTE;
			goto IL_7C;
		case ButtonInfoTable.ButtonType.EVENT_BACK:
			this.m_btnAct = RaidBossWindow.BUTTON_ACT.CLOSE;
			goto IL_7C;
		}
		goto IL_32;
		IL_7C:
		this.m_close = true;
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_eventmenu_outro", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	public void OnClickBossAttention()
	{
		if (this.m_isBossAttention)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_bossAnim, "ui_EventResult_raidboss_attention_outro_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.BossAnimationFinishCallback), true);
			this.m_isBossAttention = false;
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null && this.m_alertCollider)
		{
			msg.StaySequence();
		}
	}

	private void BossAnimationIntroCallback()
	{
		this.m_isBossAttention = true;
	}

	private void BossAnimationFinishCallback()
	{
		this.m_advent.SetActive(false);
	}

	private bool IsActiveAdventData()
	{
		return this.m_advent != null && this.m_advent.activeSelf;
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_close)
		{
			switch (this.m_btnAct)
			{
			case RaidBossWindow.BUTTON_ACT.BOSS_PLAY:
				HudMenuUtility.SendVirtualNewItemSelectClicked(HudMenuUtility.ITEM_SELECT_MODE.EVENT_BOSS);
				base.enabledAnchorObjects = false;
				HudMenuUtility.SendEnableShopButton(true);
				break;
			case RaidBossWindow.BUTTON_ACT.BOSS_INFO:
				break;
			case RaidBossWindow.BUTTON_ACT.BOSS_REWARD:
				break;
			case RaidBossWindow.BUTTON_ACT.INFO:
				break;
			case RaidBossWindow.BUTTON_ACT.ROULETTE:
				base.enabledAnchorObjects = false;
				HudMenuUtility.SendEnableShopButton(true);
				HudMenuUtility.SendChaoRouletteButtonClicked();
				break;
			case RaidBossWindow.BUTTON_ACT.CHALLENGE:
				base.enabledAnchorObjects = false;
				break;
			case RaidBossWindow.BUTTON_ACT.SHOP_RSRING:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP, false);
				base.enabledAnchorObjects = false;
				break;
			case RaidBossWindow.BUTTON_ACT.SHOP_RING:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.RING_TO_SHOP, false);
				base.enabledAnchorObjects = false;
				break;
			case RaidBossWindow.BUTTON_ACT.SHOP_CHALLENGE:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHALLENGE_TO_SHOP, false);
				base.enabledAnchorObjects = false;
				break;
			default:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.EVENT_BACK, false);
				base.enabledAnchorObjects = false;
				HudMenuUtility.SendEnableShopButton(true);
				break;
			}
			this.SetAlertSimpleUI(false);
			BackKeyManager.RemoveWindowCallBack(base.gameObject);
			this.m_opened = false;
			this.m_close = false;
		}
		else
		{
			if (this.m_openMode != RaidBossWindow.WINDOW_OPEN_MODE.NONE)
			{
				this.StartRaidbossAttentionAnim();
			}
			this.m_opened = true;
			this.SetAlertSimpleUI(false);
		}
	}

	private void SetAlertSimpleUI(bool flag)
	{
		if (this.m_alertCollider)
		{
			if (!flag)
			{
				HudMenuUtility.SetConnectAlertSimpleUI(false);
				this.m_alertCollider = false;
			}
		}
		else if (flag)
		{
			HudMenuUtility.SetConnectAlertSimpleUI(true);
			this.m_alertCollider = true;
		}
	}

	public static bool IsEnabled()
	{
		bool result = false;
		if (RaidBossWindow.s_instance != null)
		{
			result = RaidBossWindow.s_instance.enabledAnchorObjects;
		}
		return result;
	}

	public static bool IsDataReload()
	{
		bool result = true;
		if (RaidBossWindow.s_instance != null)
		{
			result = RaidBossWindow.s_instance.IsReload();
		}
		return result;
	}

	private bool IsReload()
	{
		bool result = false;
		if (this.m_rbInfoData == null || this.m_time > 5f)
		{
			result = true;
		}
		return result;
	}

	public static bool IsOpend()
	{
		return RaidBossWindow.s_instance != null && RaidBossWindow.s_instance.m_opened;
	}

	public static bool IsOpenAdvent()
	{
		return RaidBossWindow.s_instance != null && RaidBossWindow.s_instance.IsActiveAdventData();
	}

	public static RaidBossWindow Create(RaidBossInfo info, RaidBossWindow.WINDOW_OPEN_MODE mode)
	{
		if (!(RaidBossWindow.s_instance != null))
		{
			return null;
		}
		if (RaidBossWindow.s_instance.gameObject.transform.parent != null && RaidBossWindow.s_instance.gameObject.transform.parent.name != "Camera")
		{
			return null;
		}
		RaidBossWindow.s_instance.gameObject.SetActive(true);
		RaidBossWindow.s_instance.Setup(info, mode);
		return RaidBossWindow.s_instance;
	}

	private static void RaidbossDiscoverSaveDataUpdate()
	{
		if (EventManager.Instance == null)
		{
			return;
		}
		if (EventManager.Instance.Type != EventManager.EventType.RAID_BOSS)
		{
			return;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null && EventManager.Instance.RaidBossInfo != null)
			{
				List<ServerEventRaidBossState> userRaidBossList = EventManager.Instance.UserRaidBossList;
				if (userRaidBossList != null)
				{
					bool flag = false;
					foreach (ServerEventRaidBossState current in userRaidBossList)
					{
						if (current.Encounter && systemdata.currentRaidDrawIndex != current.Id)
						{
							systemdata.currentRaidDrawIndex = current.Id;
							systemdata.raidEntryFlag = false;
							flag = true;
							break;
						}
					}
					if (flag)
					{
						instance.SaveSystemData();
					}
				}
			}
		}
	}

	public void ShowNoCommunication()
	{
		GeneralUtil.ShowNoCommunication("ShowNoCommunication");
	}

	private void Awake()
	{
		this.SetInstance();
		base.enabledAnchorObjects = false;
	}

	private void OnDestroy()
	{
		if (RaidBossWindow.s_instance == this)
		{
			RaidBossWindow.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (RaidBossWindow.s_instance == null)
		{
			RaidBossWindow.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
