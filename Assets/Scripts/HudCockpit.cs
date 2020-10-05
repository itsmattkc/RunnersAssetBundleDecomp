using AnimationOrTween;
using App.Utility;
using Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Text;
using UnityEngine;

public class HudCockpit : MonoBehaviour
{
	private enum Anchor2TC
	{
		DEFAULT,
		BOSS,
		EVENTBOSS,
		SPSTAGE,
		NUM
	}

	private enum ScoreRank
	{
		RANK_01,
		RANK_02,
		RANK_03,
		RANK_04,
		RANK_05,
		RANK_06,
		RANK_07,
		RANK_08,
		NUM
	}

	[Serializable]
	private class ScoreColor
	{
		public int red;

		public int green;

		public int blue;

		public int score;

		public float Red
		{
			get
			{
				return (float)this.red / 255f;
			}
		}

		public float Green
		{
			get
			{
				return (float)this.green / 255f;
			}
		}

		public float Blue
		{
			get
			{
				return (float)this.blue / 255f;
			}
		}
	}

	private const float CHARA_CHANGE_DISABLE_TIME = 5f;

	private PlayerInformation m_playerInfo;

	private LevelInformation m_levelInformation;

	private StageScoreManager m_stageScoreManager;

	private UILabel m_stockRingLabel;

	private GameObject m_addStockRing;

	private GameObject m_addStockRingEff;

	private UILabel m_ringLabel;

	private TweenColor m_ringTweenColor;

	private Color m_ringDefaultColor;

	private UISprite[] m_itemSptires = new UISprite[3];

	private UILabel m_scoreLabel;

	private UILabel m_spCrystalLabel;

	private UILabel m_animalLabel;

	private long m_realTimeScore = -1L;

	private int m_ringCount = -1;

	private int m_stockRingCount = -1;

	private int m_animalCount = -1;

	private int m_crystalCount = -1;

	private int m_distance = -1;

	private GameObject m_charaChangeBtn;

	private GameObject m_itemBtn;

	private GameObject m_quickModeObj;

	private GameObject m_endlessObj;

	private GameObject m_endlessBossObj;

	private UILabel m_distanceLabel;

	private UILabel m_timer1Label;

	private UILabel m_timer2Label;

	private TweenColor m_timer1TweenColor;

	private TweenColor m_timer2TweenColor;

	private UISlider m_distanceSlider;

	private UISlider m_speedLSlider;

	private UISlider m_speedRSlider;

	private GameObject m_bossGauge;

	private UISlider m_bossLifeSlider;

	private UILabel m_bossLifeLabel;

	private UISlider m_deathDistance;

	private TweenColor m_deathDistanceTweenColor;

	private UIImageButton m_pauseButton;

	private UIPlayAnimation m_pauseButtonAnim;

	private GameObject m_colorPowerEffect;

	private Animation m_scoreAnim;

	private List<ItemType> m_displayItems = new List<ItemType>();

	private float m_charaChaneDisableTime;

	private bool m_enablePause;

	private bool m_enableItem;

	private bool m_itemPause;

	private bool m_bBossBattleDistance;

	private bool m_bBossBattleDistanceArea;

	private bool m_bBossStart;

	private float m_bossTime;

	private bool m_quickModeFlag;

	private bool m_countDownFlag;

	private bool m_changeFlag;

	private bool m_pauseFlag;

	private int m_pauseContinueCount;

	private float m_pauseContinueTimer;

	private bool m_backTitle;

	private bool m_backMainMenuCheck;

	private bool m_createWindow;

	private bool m_itemTutorial;

	private bool m_firtsTutorial;

	private bool m_init;

	private Bitset32 m_countDownSEflags;

	private readonly string[] Anchor2TC_tbl = new string[]
	{
		"pattern_0_default",
		"pattern_1_boss",
		"pattern_3_raid",
		"pattern_3_ev1"
	};

	private readonly string[] PauseWindowSetupLbl = new string[]
	{
		"pause_window/pause_Anim/window/Btn_back_mainmenu/Lbl_back",
		"pause_window/pause_Anim/window/Btn_back_mainmenu/Lbl_back/Lbl_back_sh",
		"pause_window/pause_Anim/window/Btn_continue/Lbl_continue",
		"pause_window/pause_Anim/window/Btn_continue/Lbl_continue/Lbl_continue_sh"
	};

	private HudCockpit.ScoreRank m_nextScoreRank;

	[SerializeField]
	private HudCockpit.ScoreColor[] m_scoreColors = new HudCockpit.ScoreColor[8];

	private Color m_scoreColor = new Color(1f, 1f, 1f, 1f);

	private void Start()
	{
		this.Initialize();
	}

	private void Initialize()
	{
		if (this.m_init)
		{
			return;
		}
		this.m_init = true;
		if (StageModeManager.Instance != null)
		{
			this.m_quickModeFlag = StageModeManager.Instance.IsQuickMode();
		}
		this.m_playerInfo = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
		this.m_levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
		this.m_stageScoreManager = StageScoreManager.Instance;
		this.m_charaChangeBtn = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_change");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "HUD_btn_item");
		if (gameObject != null)
		{
			this.m_itemBtn = GameObjectUtil.FindChildGameObject(gameObject, "Btn_item");
			if (this.m_itemBtn != null)
			{
				this.m_itemBtn.SetActive(false);
			}
			for (int i = 0; i < this.m_itemSptires.Length; i++)
			{
				this.m_itemSptires[i] = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "item_" + i);
				if (this.m_itemSptires[i] != null)
				{
					this.m_itemSptires[i].gameObject.SetActive(false);
				}
			}
		}
		GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "HUD_indicator");
		this.m_distanceLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_distance");
		this.m_distanceSlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(parent, "Pgb_distance");
		this.m_speedLSlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(parent, "Pgb_speed_L");
		this.m_speedRSlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(parent, "Pgb_speed_R");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(parent, "pattern_0_default");
		if (gameObject2 != null)
		{
			this.m_endlessObj = GameObjectUtil.FindChildGameObject(gameObject2, "mode_endless");
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(parent, "pattern_2_boss");
		if (gameObject3 != null)
		{
			this.m_endlessBossObj = GameObjectUtil.FindChildGameObject(gameObject3, "mode_endless");
		}
		this.m_quickModeObj = GameObjectUtil.FindChildGameObject(base.gameObject, "mode_quick_time");
		if (this.m_quickModeObj != null)
		{
			this.m_quickModeObj.SetActive(this.m_quickModeFlag);
		}
		if (this.m_quickModeFlag && this.m_quickModeObj != null)
		{
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_quickModeObj, "Lbl_time1");
			if (gameObject4 != null)
			{
				this.m_timer1Label = gameObject4.GetComponent<UILabel>();
				this.m_timer1TweenColor = gameObject4.GetComponent<TweenColor>();
			}
			GameObject gameObject5 = GameObjectUtil.FindChildGameObject(this.m_quickModeObj, "Lbl_time2");
			if (gameObject5 != null)
			{
				this.m_timer2Label = gameObject5.GetComponent<UILabel>();
				this.m_timer2TweenColor = gameObject5.GetComponent<TweenColor>();
			}
		}
		this.SetupBossParam(BossType.FEVER, 0, 0);
		GameObject parent2 = GameObjectUtil.FindChildGameObject(base.gameObject, "HUD_btn_pause");
		this.m_pauseButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent2, "Btn_pause");
		if (this.m_pauseButton != null)
		{
			this.m_pauseButtonAnim = this.m_pauseButton.GetComponent<UIPlayAnimation>();
			this.m_pauseButtonAnim.enabled = false;
			this.m_pauseButton.isEnabled = this.m_enablePause;
		}
		this.m_colorPowerEffect = GameObjectUtil.FindChildGameObject(base.gameObject, "HUD_ColorPower_effect");
		this.m_backTitle = false;
		this.ItemButton_SetEnabled(false);
		this.UpdateItemView();
	}

	private void SetupBossParam(BossType bossType, int hp, int hpMax)
	{
		GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "HUD_indicator");
		GameObject gameObject = null;
		string name;
		switch (bossType)
		{
		case BossType.FEVER:
			name = "pattern_2_boss";
			break;
		case BossType.MAP1:
		case BossType.MAP2:
		case BossType.MAP3:
			gameObject = GameObjectUtil.FindChildGameObject(parent, "pattern_0_default");
			name = "pattern_1_boss";
			break;
		default:
			gameObject = GameObjectUtil.FindChildGameObject(parent, "pattern_0_default");
			name = "pattern_3_raid";
			break;
		}
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		this.m_bossGauge = GameObjectUtil.FindChildGameObject(parent, name);
		if (this.m_bossGauge != null)
		{
			if (bossType != BossType.FEVER)
			{
				this.m_bossLifeSlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(this.m_bossGauge, "Pgb_boss_life");
				this.m_bossLifeLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_bossGauge, "Lbl_boss_life");
			}
			this.m_deathDistance = GameObjectUtil.FindChildGameObjectComponent<UISlider>(this.m_bossGauge, "Pgb_death_distance");
			this.m_deathDistanceTweenColor = GameObjectUtil.FindChildGameObjectComponent<TweenColor>(this.m_deathDistance.gameObject, "img_gauge_distance");
		}
	}

	private void Update()
	{
		if (this.m_createWindow && GeneralWindow.IsCreated("BackMainMenuCheckWindow"))
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				GeneralWindow.Close();
				GameObjectUtil.SendMessageFindGameObject("pause_window", "OnBackMainMenuAnimation", null, SendMessageOptions.DontRequireReceiver);
				GameObjectUtil.SendMessageToTagObjects("GameModeStage", "OnMsgNotifyEndPauseExitStage", new MsgNotifyEndPauseExitStage(), SendMessageOptions.DontRequireReceiver);
				this.m_backTitle = true;
				this.m_createWindow = false;
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				ObjUtil.SetHudStockRingEffectOff(false);
				GeneralWindow.Close();
				this.m_createWindow = false;
			}
		}
		if (this.m_pauseContinueCount > 0)
		{
			this.m_pauseContinueTimer -= RealTime.deltaTime;
			if (this.m_pauseContinueTimer <= 0f)
			{
				this.m_pauseContinueCount--;
				if (this.m_pauseContinueCount > 0)
				{
					SoundManager.SePlay("sys_pause", "SE");
					this.m_pauseContinueTimer = 1f;
				}
				else
				{
					SoundManager.SePlay("sys_go", "SE");
					this.m_pauseContinueTimer = 0f;
				}
			}
		}
		if (this.m_playerInfo != null)
		{
			if (this.m_ringLabel != null)
			{
				int numRings = this.m_playerInfo.NumRings;
				if (this.m_ringCount != numRings)
				{
					this.m_ringCount = numRings;
					this.m_ringLabel.text = HudUtility.GetFormatNumString<int>(this.m_ringCount);
				}
				if (this.m_ringTweenColor != null)
				{
					if (this.m_ringTweenColor.enabled)
					{
						if (numRings > 0)
						{
							this.m_ringTweenColor.enabled = false;
							this.m_ringLabel.color = this.m_ringDefaultColor;
						}
					}
					else if (numRings == 0)
					{
						this.m_ringTweenColor.enabled = true;
					}
				}
			}
			if (this.m_stockRingLabel != null && this.m_stageScoreManager != null)
			{
				int num = (int)this.m_stageScoreManager.Ring;
				if (this.m_stockRingCount != num)
				{
					this.m_stockRingCount = num;
					this.m_stockRingLabel.text = HudUtility.GetFormatNumString<int>(this.m_stockRingCount);
				}
			}
			if (this.m_quickModeFlag)
			{
				this.UpdateTimer();
			}
			if (this.m_distanceLabel != null)
			{
				int num2 = Mathf.FloorToInt(this.m_playerInfo.TotalDistance);
				if (this.m_distance != num2)
				{
					this.m_distance = num2;
					this.m_distanceLabel.text = HudUtility.GetFormatNumString<int>(this.m_distance);
				}
			}
			if (this.m_distanceSlider != null && this.m_levelInformation != null)
			{
				float num3 = (this.m_levelInformation.DistanceToBossOnStart != 0f) ? (this.m_levelInformation.DistanceOnStage / this.m_levelInformation.DistanceToBossOnStart) : 1f;
				if (num3 != this.m_distanceSlider.value)
				{
					this.m_distanceSlider.value = num3;
				}
			}
			float num4 = (1f + (float)this.m_playerInfo.SpeedLevel) / 3f;
			if (this.m_speedLSlider != null && this.m_speedLSlider.value != num4)
			{
				this.m_speedLSlider.value = num4;
			}
			if (this.m_speedRSlider != null && this.m_speedRSlider.value != num4)
			{
				this.m_speedRSlider.value = num4;
			}
		}
		if (this.m_stageScoreManager != null)
		{
			if (this.m_scoreLabel != null)
			{
				long realtimeScore = this.m_stageScoreManager.GetRealtimeScore();
				if (this.m_realTimeScore != realtimeScore)
				{
					this.m_realTimeScore = realtimeScore;
					this.m_scoreLabel.text = HudUtility.GetFormatNumString<long>(this.m_realTimeScore);
					if (this.m_nextScoreRank < HudCockpit.ScoreRank.NUM && (long)this.m_scoreColors[(int)this.m_nextScoreRank].score < this.m_realTimeScore)
					{
						this.m_scoreColor.r = this.m_scoreColors[(int)this.m_nextScoreRank].Red;
						this.m_scoreColor.g = this.m_scoreColors[(int)this.m_nextScoreRank].Green;
						this.m_scoreColor.b = this.m_scoreColors[(int)this.m_nextScoreRank].Blue;
						this.m_scoreLabel.color = this.m_scoreColor;
						this.m_nextScoreRank++;
						ActiveAnimation.Play(this.m_scoreAnim, "ui_gp_bit_score_Anim", Direction.Forward);
					}
				}
			}
			if (this.m_spCrystalLabel != null)
			{
				int num5 = (int)this.m_stageScoreManager.GetRealtimeEventScore();
				if (this.m_crystalCount != num5)
				{
					this.m_crystalCount = num5;
					this.m_spCrystalLabel.text = HudUtility.GetFormatNumString<int>(this.m_crystalCount);
				}
			}
			if (this.m_animalLabel != null)
			{
				int num6 = (int)this.m_stageScoreManager.GetRealtimeEventAnimal();
				if (this.m_animalCount != num6)
				{
					this.m_animalCount = num6;
					this.m_animalLabel.text = HudUtility.GetFormatNumString<int>(this.m_animalCount);
				}
			}
		}
		if (this.m_deathDistance != null && this.m_bossGauge != null && this.m_bossGauge.activeSelf && this.m_playerInfo != null && this.m_levelInformation != null)
		{
			if (!this.m_bBossStart)
			{
				if (this.m_deathDistance.value != 0f)
				{
					this.m_deathDistance.value = 0f;
					this.m_bossTime = this.m_levelInformation.BossEndTime;
				}
			}
			else
			{
				float num7 = 0f;
				this.m_bossTime -= Time.deltaTime;
				if (this.m_bossTime < 0f)
				{
					this.m_bossTime = 0f;
				}
				if (this.m_bossTime > 0f && this.m_levelInformation.BossEndTime > 0f)
				{
					num7 = this.m_bossTime / this.m_levelInformation.BossEndTime;
				}
				this.m_deathDistance.value = 1f - num7;
			}
			if (this.m_deathDistance.value > 0.8f && this.m_deathDistanceTweenColor != null && !this.m_deathDistanceTweenColor.enabled)
			{
				this.m_deathDistanceTweenColor.enabled = true;
			}
			if (this.m_deathDistance.value == 1f && this.m_bBossBattleDistance)
			{
				MsgBossDistanceEnd value = new MsgBossDistanceEnd(true);
				GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgBossDistanceEnd", value, SendMessageOptions.DontRequireReceiver);
				this.m_bBossBattleDistance = false;
				GameObjectUtil.SendMessageToTagObjects("GameModeStage", "OnBossTimeUp", null, SendMessageOptions.DontRequireReceiver);
			}
			if (this.m_bossTime < 3f && !this.m_bBossBattleDistanceArea)
			{
				MsgBossDistanceEnd value2 = new MsgBossDistanceEnd(false);
				GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgBossDistanceEnd", value2, SendMessageOptions.DontRequireReceiver);
				this.m_bBossBattleDistanceArea = true;
			}
		}
		if (this.m_charaChaneDisableTime > 0f)
		{
			if (this.m_charaChaneDisableTime < Time.deltaTime)
			{
				this.m_charaChaneDisableTime = 0f;
			}
			else
			{
				this.m_charaChaneDisableTime -= Time.deltaTime;
			}
			if (this.m_charaChaneDisableTime == 0f)
			{
				this.OnChangeCharaButton(new MsgChangeCharaButton(true, true));
			}
		}
	}

	private void UpdateItemView()
	{
		string arg = (!this.m_enableItem) ? "ui_cmn_icon_item_g_" : "ui_cmn_icon_item_";
		for (int i = 0; i < this.m_itemSptires.Length; i++)
		{
			ItemType itemType = (i >= this.m_displayItems.Count) ? ItemType.UNKNOWN : this.m_displayItems[i];
			if (this.m_itemSptires[i] != null)
			{
				if (itemType == ItemType.UNKNOWN)
				{
					this.m_itemSptires[i].gameObject.SetActive(false);
				}
				else
				{
					this.m_itemSptires[i].gameObject.SetActive(true);
					this.m_itemSptires[i].spriteName = arg + (int)itemType;
				}
			}
		}
		if (this.m_itemBtn != null)
		{
			this.m_itemBtn.SetActive(this.m_displayItems.Count > 0);
		}
	}

	private void UpdateTimer()
	{
		if (StageTimeManager.Instance != null)
		{
			float time = StageTimeManager.Instance.Time;
			int num = (int)time;
			int decimalNumber = (int)((time - (float)num) * 100f);
			if (this.m_timer1Label != null)
			{
				this.m_timer1Label.text = num.ToString("D2") + " .";
			}
			if (this.m_timer2Label != null)
			{
				this.m_timer2Label.text = decimalNumber.ToString("D2");
			}
			if (this.m_countDownFlag)
			{
				if (time > 10f)
				{
					this.m_countDownFlag = false;
					if (this.m_timer1TweenColor != null)
					{
						this.m_timer1TweenColor.enabled = false;
						if (this.m_timer1Label != null)
						{
							this.m_timer1Label.color = this.m_timer1TweenColor.from;
						}
					}
					if (this.m_timer2TweenColor != null)
					{
						this.m_timer2TweenColor.enabled = false;
						if (this.m_timer2Label != null)
						{
							this.m_timer2Label.color = this.m_timer2TweenColor.from;
						}
					}
				}
			}
			else if (time < 10f)
			{
				this.m_countDownFlag = true;
				this.m_countDownSEflags.Reset();
				if (this.m_timer1TweenColor != null)
				{
					this.m_timer1TweenColor.enabled = true;
				}
				if (this.m_timer2TweenColor != null)
				{
					this.m_timer2TweenColor.enabled = true;
				}
			}
			this.UpdateCountDownSE(num, decimalNumber);
		}
	}

	private void UpdateCountDownSE(int integerNumber, int decimalNumber)
	{
		if (this.m_countDownFlag)
		{
			if (integerNumber == 0 && decimalNumber == 0)
			{
				if (!this.m_countDownSEflags.Test(0))
				{
					ObjUtil.PlaySE("sys_quickmode_count_zero", "SE");
					this.m_countDownSEflags.Set(0);
				}
			}
			else
			{
				if (this.m_countDownSEflags.Test(0))
				{
					this.m_countDownSEflags.Reset(0);
				}
				for (int i = 0; i < 10; i++)
				{
					if (i < integerNumber)
					{
						if (this.m_countDownSEflags.Test(i + 1))
						{
							this.m_countDownSEflags.Reset(i + 1);
						}
					}
					else if (i == integerNumber && !this.m_countDownSEflags.Test(i + 1))
					{
						this.m_countDownSEflags.Set(i + 1);
						ObjUtil.PlaySE("sys_quickmode_count", "SE");
						break;
					}
				}
			}
		}
	}

	private void OnClickStartPause()
	{
		GameObjectUtil.SendMessageToTagObjects("GameModeStage", "OnMsgNotifyStartPause", new MsgNotifyStartPause(), SendMessageOptions.DontRequireReceiver);
	}

	private void OnClickEndPause()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		GC.Collect();
		Resources.UnloadUnusedAssets();
		GC.Collect();
		this.m_pauseContinueCount = 3;
		this.m_pauseContinueTimer = 1f;
	}

	private void OnFinishedContinueAnimation()
	{
		if (this.m_pauseFlag)
		{
			GameObjectUtil.SendMessageToTagObjects("GameModeStage", "OnMsgNotifyEndPause", new MsgNotifyEndPause(), SendMessageOptions.DontRequireReceiver);
			this.m_pauseFlag = false;
		}
	}

	private void OnClickEndPauseExitStage()
	{
		if (this.m_pauseContinueCount == 0 && this.m_pauseContinueTimer == 0f)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.CreateBackMainMenuCheckWindow();
		}
	}

	private void OnEnablePause(MsgEnablePause msg)
	{
		this.m_enablePause = msg.m_enable;
		if (this.m_pauseButton != null)
		{
			this.m_pauseButton.isEnabled = this.m_enablePause;
		}
	}

	private void OnClickChange()
	{
		SoundManager.SePlay("act_chara_change", "SE");
		if (this.m_charaChaneDisableTime == 0f)
		{
			GameObjectUtil.SendMessageToTagObjects("GameModeStage", "OnMsgChangeChara", new MsgChangeChara
			{
				m_changeByBtn = true
			}, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnChangeCharaSucceed(MsgChangeCharaSucceed msg)
	{
		this.m_charaChaneDisableTime = 5f;
		if (msg.m_disabled)
		{
			this.ChangeButton_SetActive(false);
		}
		else
		{
			this.m_changeFlag = true;
			this.ChangeButton_SetEnabled(false);
		}
	}

	private void OnChangeCharaEnable(MsgChangeCharaEnable msg)
	{
		this.Initialize();
		this.ChangeButton_SetActive(msg.value);
		this.ChangeButton_SetEnabled(false);
	}

	private void OnChangeCharaButton(MsgChangeCharaButton msg)
	{
		if (!msg.value)
		{
			if (!msg.pause)
			{
				this.m_changeFlag = false;
			}
			this.ChangeButton_SetEnabled(false);
		}
		else
		{
			if (!msg.pause)
			{
				this.m_changeFlag = true;
			}
			if (this.m_charaChaneDisableTime == 0f && this.m_changeFlag)
			{
				this.ChangeButton_SetEnabled(true);
			}
		}
	}

	private void ChangeButton_SetActive(bool isActive)
	{
		if (this.m_charaChangeBtn != null)
		{
			this.m_charaChangeBtn.SetActive(isActive);
		}
	}

	private void ChangeButton_SetEnabled(bool isEnabled)
	{
		if (this.m_charaChangeBtn != null)
		{
			UIImageButton component = this.m_charaChangeBtn.GetComponent<UIImageButton>();
			if (component != null)
			{
				component.isEnabled = isEnabled;
			}
		}
	}

	private void ItemButton_SetActive(bool isActive)
	{
		if (this.m_itemBtn != null)
		{
			this.m_itemBtn.SetActive(isActive);
		}
	}

	private void ItemButton_SetEnabled(bool isEnabled)
	{
		this.m_enableItem = isEnabled;
		if (this.m_itemBtn != null)
		{
			UIImageButton component = this.m_itemBtn.GetComponent<UIImageButton>();
			if (component != null)
			{
				component.isEnabled = isEnabled;
			}
		}
		this.UpdateItemView();
	}

	private void OnItemEnable(MsgItemButtonEnable msg)
	{
		if (msg.m_enable && !this.m_itemPause)
		{
			this.ItemButton_SetEnabled(true);
		}
		else
		{
			this.ItemButton_SetEnabled(false);
		}
	}

	private void OnStartTutorial()
	{
		this.m_itemTutorial = true;
		HudTutorial.StartTutorial(HudTutorial.Id.ITEM_BUTTON_1, BossType.NONE);
	}

	private void OnNextTutorial()
	{
		if (this.m_itemTutorial)
		{
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.STAGE_ITEM);
		}
	}

	private void OnSetEquippedItem(MsgSetEquippedItem msg)
	{
		ItemType[] itemType = msg.m_itemType;
		for (int i = 0; i < itemType.Length; i++)
		{
			ItemType itemType2 = itemType[i];
			if (itemType2 != ItemType.UNKNOWN)
			{
				this.m_displayItems.Add(itemType2);
			}
		}
		this.UpdateItemView();
	}

	private void OnSetPresentEquippedItem(MsgSetEquippedItem msg)
	{
		bool flag = this.m_displayItems.Count > 0;
		this.m_displayItems.Clear();
		ItemType[] itemType = msg.m_itemType;
		for (int i = 0; i < itemType.Length; i++)
		{
			ItemType itemType2 = itemType[i];
			if (itemType2 != ItemType.UNKNOWN)
			{
				this.m_displayItems.Add(itemType2);
			}
		}
		this.UpdateItemView();
		if (!flag && msg.m_enabled && !this.m_itemPause)
		{
			this.ItemButton_SetEnabled(true);
		}
	}

	private void OnChangeItem(MsgSetEquippedItem msg)
	{
		bool flag = this.m_displayItems.Count > 0;
		this.m_displayItems.Clear();
		ItemType[] itemType = msg.m_itemType;
		for (int i = 0; i < itemType.Length; i++)
		{
			ItemType itemType2 = itemType[i];
			if (itemType2 != ItemType.UNKNOWN)
			{
				this.m_displayItems.Add(itemType2);
			}
		}
		this.UpdateItemView();
	}

	private void OnUsedItem(MsgUsedItem msg)
	{
		this.m_displayItems.Remove(msg.m_itemType);
		this.UpdateItemView();
	}

	private void OnSetPause(MsgSetPause msg)
	{
		if (GeneralWindow.IsCreated("BackMainMenuCheckWindow") || this.m_backTitle)
		{
			return;
		}
		if (this.m_pauseFlag && msg.m_backKey)
		{
			if (this.m_pauseContinueCount == 0 && this.m_pauseContinueTimer == 0f)
			{
				GameObjectUtil.SendMessageFindGameObject("pause_window", "OnContinueAnimation", null, SendMessageOptions.DontRequireReceiver);
				this.OnClickEndPause();
				if (this.m_pauseButton != null)
				{
					NGUITools.SetActive(this.m_pauseButton.gameObject, true);
				}
			}
			return;
		}
		if (this.m_pauseButton != null && this.m_pauseButtonAnim != null)
		{
			Animation target = this.m_pauseButtonAnim.target;
			if (target != null)
			{
				this.m_pauseFlag = true;
				GameObjectUtil.SendMessageFindGameObject("pause_window", "OnMsgNotifyStartPause", null, SendMessageOptions.DontRequireReceiver);
				this.m_pauseButton.gameObject.SetActive(false);
				this.m_pauseContinueCount = 0;
				this.m_pauseContinueTimer = 0f;
				target.gameObject.SetActive(true);
				target.Rewind(pause_window.INANIM_NAME);
				ActiveAnimation.Play(target, pause_window.INANIM_NAME, Direction.Forward, true);
				if (msg.m_backMainMenu)
				{
					this.CreateBackMainMenuCheckWindow();
				}
			}
		}
	}

	private void HudBossHpGaugeOpen(MsgHudBossHpGaugeOpen msg)
	{
		this.SetupBossParam(msg.m_bossType, msg.m_hp, msg.m_hpMax);
		if (this.m_bossGauge != null)
		{
			this.m_bossGauge.SetActive(true);
		}
		this.SetBossHp(msg.m_hp, msg.m_hpMax);
		this.m_bBossBattleDistance = true;
		if (this.m_deathDistanceTweenColor != null)
		{
			this.m_deathDistanceTweenColor.enabled = false;
		}
	}

	private void HudBossGaugeStart(MsgHudBossGaugeStart msg)
	{
		this.m_bBossStart = true;
		this.m_bBossBattleDistanceArea = false;
	}

	private void OnBossEnd(MsgBossEnd msg)
	{
		if (this.m_bossGauge != null)
		{
			this.m_bossGauge.SetActive(false);
		}
		this.m_bBossStart = false;
		this.m_bBossBattleDistanceArea = false;
	}

	private void HudBossHpGaugeSet(MsgHudBossHpGaugeSet msg)
	{
		this.SetBossHp(msg.m_hp, msg.m_hpMax);
	}

	private void SetBossHp(int hp, int hpMax)
	{
		float value = 0f;
		if (hp > 0)
		{
			value = (float)hp / (float)hpMax;
		}
		if (this.m_bossLifeSlider != null)
		{
			this.m_bossLifeSlider.value = value;
		}
		if (this.m_bossLifeLabel != null)
		{
			this.m_bossLifeLabel.text = hp.ToString() + "/" + hpMax.ToString();
		}
	}

	private void OnMsgPrepareContinue(MsgPrepareContinue msg)
	{
		if (msg.m_bossStage && this.m_deathDistance != null && this.m_levelInformation != null)
		{
			this.m_deathDistance.value = 0f;
			this.m_bossTime = this.m_levelInformation.BossEndTime;
		}
	}

	private void OnPhantomActStart(MsgPhantomActStart msg)
	{
		if (this.m_colorPowerEffect != null)
		{
			this.m_colorPowerEffect.SetActive(true);
		}
	}

	private void OnPhantomActEnd(MsgPhantomActEnd msg)
	{
		if (this.m_colorPowerEffect != null)
		{
			this.m_colorPowerEffect.SetActive(false);
		}
	}

	private void OnAddStockRing(MsgAddStockRing msg)
	{
		if (this.m_addStockRing != null && msg.m_numAddRings > 0)
		{
			this.m_addStockRing.SetActive(false);
			this.m_addStockRing.SetActive(true);
			SoundManager.SePlay("act_ring_collect", "SE");
		}
	}

	private void OnSetup(MsgHudCockpitSetup msg)
	{
		this.m_backMainMenuCheck = msg.m_backMainMenuCheck;
		this.m_firtsTutorial = msg.m_firstTutorial;
		GameObject[] array = new GameObject[4];
		GameObject gameObject = base.gameObject;
		if (gameObject != null)
		{
			for (int i = 0; i < 4; i++)
			{
				Transform transform = gameObject.transform.FindChild("Anchor_2_TC/" + this.Anchor2TC_tbl[i]);
				if (transform != null)
				{
					array[i] = transform.gameObject;
					if (array[i] != null)
					{
						array[i].SetActive(false);
					}
				}
			}
		}
		GameObject gameObject2 = GameObject.Find("UI Root (2D)/Camera/Anchor_5_MC");
		if (gameObject2 != null)
		{
			for (int j = 0; j < this.PauseWindowSetupLbl.Length; j++)
			{
				Transform transform2 = gameObject2.transform.FindChild(this.PauseWindowSetupLbl[j]);
				if (transform2 != null)
				{
					HudUtility.SetupUILabelText(transform2.gameObject);
				}
			}
		}
		if (msg.m_bossType == BossType.MAP1 || msg.m_bossType == BossType.MAP2 || msg.m_bossType == BossType.MAP3)
		{
			GameObject gameObject3 = array[1];
			if (gameObject3 != null)
			{
				gameObject3.SetActive(true);
			}
			this.SetupRingObj(gameObject3);
		}
		else if (msg.m_bossType != BossType.NONE && msg.m_bossType != BossType.FEVER)
		{
			GameObject gameObject4 = array[2];
			if (gameObject4 != null)
			{
				gameObject4.SetActive(true);
			}
			this.SetupRingObj(gameObject4);
		}
		else if (msg.m_spCrystal)
		{
			GameObject gameObject5 = array[3];
			if (gameObject5 != null)
			{
				gameObject5.SetActive(true);
			}
			this.SetupRingObj(gameObject5);
			this.SetupScoreObj(gameObject5);
			this.m_spCrystalLabel = this.SetupEventObj(gameObject5, "HUD_event", "Lbl_event", "img_event_object", "ui_event_object_icon");
		}
		else if (msg.m_animal)
		{
			GameObject gameObject6 = array[3];
			if (gameObject6 != null)
			{
				gameObject6.SetActive(true);
			}
			this.SetupRingObj(gameObject6);
			this.SetupScoreObj(gameObject6);
			this.m_animalLabel = this.SetupEventObj(gameObject6, "HUD_event", "Lbl_event", "img_event_object", "ui_event_object_icon");
		}
		else
		{
			GameObject gameObject7 = array[0];
			if (gameObject7 != null)
			{
				gameObject7.SetActive(true);
			}
			this.SetupRingObj(gameObject7);
			this.SetupScoreObj(gameObject7);
		}
		if (this.m_stageScoreManager != null)
		{
			this.m_stageScoreManager.SetupScoreRate();
		}
		if (this.m_firtsTutorial)
		{
			GameObjectUtil.SendMessageFindGameObject("pause_window", "OnSetFirstTutorial", null, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void SetupScoreObj(GameObject patternObj)
	{
		if (patternObj != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(patternObj, "HUD_score");
			if (gameObject != null)
			{
				this.m_scoreLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_score");
				this.m_scoreAnim = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject, "Anim_score");
			}
		}
	}

	private UILabel SetupEventObj(GameObject patternObj, string objName, string lblName, string imgName, string texName)
	{
		UILabel result = null;
		if (patternObj != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(patternObj, objName);
			if (gameObject != null)
			{
				result = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, lblName);
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, imgName);
				if (uISprite != null)
				{
					uISprite.spriteName = texName;
				}
			}
		}
		return result;
	}

	private void SetupRingObj(GameObject patternObj)
	{
		if (patternObj != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(patternObj, "HUD_ring");
			if (gameObject != null)
			{
				this.m_stockRingLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_stock_ring");
				this.m_ringLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_ring");
				if (this.m_ringLabel != null)
				{
					this.m_ringDefaultColor = this.m_ringLabel.color;
				}
				this.m_ringTweenColor = GameObjectUtil.FindChildGameObjectComponent<TweenColor>(gameObject, "Lbl_ring");
				this.m_addStockRing = GameObjectUtil.FindChildGameObject(gameObject, "add");
				if (this.m_addStockRing != null)
				{
					this.m_addStockRingEff = GameObjectUtil.FindChildGameObject(this.m_addStockRing, "eff_switch");
					this.m_addStockRing.SetActive(false);
				}
			}
		}
	}

	private void CreateBackMainMenuCheckWindow()
	{
		string cellName = (!this.m_backMainMenuCheck) ? "ui_back_menu_text_option" : "ui_back_menu_text";
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "BackMainMenuCheckWindow",
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PauseWindow", "ui_back_menu_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PauseWindow", cellName).text,
			buttonType = GeneralWindow.ButtonType.YesNo
		});
		ObjUtil.SetHudStockRingEffectOff(true);
		this.m_createWindow = true;
	}

	private void OnPauseItemOnBoss()
	{
		this.m_itemPause = true;
		this.OnItemEnable(new MsgItemButtonEnable(false));
	}

	private void OnResumeItemOnBoss(bool phatomFlag)
	{
		this.m_itemPause = false;
		if (!phatomFlag)
		{
			this.OnItemEnable(new MsgItemButtonEnable(true));
		}
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
		if (this.m_addStockRing != null)
		{
			this.m_addStockRing.SetActive(false);
		}
	}

	private void OnMsgStockRingEffect(MsgHudStockRingEffect msg)
	{
		if (this.m_addStockRingEff != null)
		{
			if (msg.m_off)
			{
				this.m_addStockRingEff.transform.localPosition = new Vector3(1000f, 1000f, 0f);
			}
			else
			{
				this.m_addStockRingEff.transform.localPosition = Vector3.zero;
			}
		}
	}

	private void OnClickItemButton()
	{
		if (this.m_levelInformation != null)
		{
			this.m_levelInformation.RequestEqitpItem = true;
		}
		if (this.m_itemTutorial)
		{
			this.m_itemTutorial = false;
			TutorialCursor.DestroyTutorialCursor();
			MsgTutorialEnd value = new MsgTutorialEnd();
			GameObjectUtil.SendMessageToTagObjects("GameModeStage", "OnMsgTutorialItemButtonEnd", value, SendMessageOptions.DontRequireReceiver);
		}
		if (StageItemManager.Instance != null)
		{
			for (int i = 0; i < this.m_displayItems.Count; i++)
			{
				if (this.m_displayItems[i] != ItemType.UNKNOWN)
				{
					StageItemManager.Instance.OnRequestItemUse(new MsgAskEquipItemUsed(this.m_displayItems[i]));
					return;
				}
			}
		}
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
