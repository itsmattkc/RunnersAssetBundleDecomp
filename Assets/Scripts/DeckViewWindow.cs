using AnimationOrTween;
using DataTable;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class DeckViewWindow : WindowBase
{
	private enum BTN_TYPE
	{
		UP,
		DOWN
	}

	private enum SELECT_TYPE
	{
		CHARA_MAIN,
		CHARA_SUB,
		CHAO_MAIN,
		CHAO_SUB,
		NUM,
		UNKNOWN = -1
	}

	private const float BTN_TIME_LIMIT = 0.5f;

	private const float BTN_DELAY_TIME = 0.5f;

	private GameObject m_parent;

	private GameObject m_windowRoot;

	private Animation m_windowAnimation;

	private BoxCollider m_bgCollider;

	private bool m_init;

	private bool m_change;

	private bool m_close;

	private int m_chaoMainId = -1;

	private int m_chaoSubId = -1;

	private float m_chaoSpIconTime;

	private int m_direction = 1;

	private float m_pressTime;

	private DeckViewWindow.SELECT_TYPE m_pressBtnType = DeckViewWindow.SELECT_TYPE.UNKNOWN;

	private float m_changeDelayCheckTime;

	private Dictionary<DeckViewWindow.SELECT_TYPE, List<UIImageButton>> m_changeBtnList;

	private int m_currentChaoSetStock;

	private List<bool> m_isSaveData;

	private CharaType m_playerMain = CharaType.UNKNOWN;

	private CharaType m_playerSub = CharaType.UNKNOWN;

	private List<CharaType> m_charaList;

	private List<DataTable.ChaoData> m_chaoList;

	private bool m_isLastInputTime;

	private Dictionary<DeckViewWindow.SELECT_TYPE, float> m_lastInputTime;

	private UISprite m_detailTextBg;

	private UILabel m_detailTextLabel;

	private ChaoSort m_currentChaoSort = ChaoSort.NONE;

	private int m_initChaoSetStock;

	private CharaType m_initPlayerMain = CharaType.UNKNOWN;

	private CharaType m_initPlayerSub = CharaType.UNKNOWN;

	private int m_initChaoMain = -1;

	private int m_initChaoSub = -1;

	private int m_reqChaoSetStock;

	private CharaType m_reqPlayerMain = CharaType.UNKNOWN;

	private CharaType m_reqPlayerSub = CharaType.UNKNOWN;

	private int m_reqChaoMain = -1;

	private int m_reqChaoSub = -1;

	private bool m_btnLock;

	private static DeckViewWindow s_instance;

	public static DeckViewWindow instance
	{
		get
		{
			return DeckViewWindow.s_instance;
		}
	}

	public static bool isActive
	{
		get
		{
			return DeckViewWindow.s_instance != null && DeckViewWindow.s_instance.gameObject.activeSelf;
		}
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	public void Init()
	{
		this.m_btnLock = false;
		this.m_parent = null;
		UIPanel uIPanel = GameObjectUtil.FindChildGameObjectComponent<UIPanel>(base.gameObject, "DeckViewWindow");
		if (uIPanel != null)
		{
			uIPanel.alpha = 0f;
		}
		if (this.m_bgCollider != null)
		{
			this.m_bgCollider.enabled = false;
		}
		if (this.m_windowRoot == null)
		{
			this.m_windowRoot = GameObjectUtil.FindChildGameObject(base.gameObject, "window");
		}
		if (this.m_windowRoot != null)
		{
			this.m_windowRoot.SetActive(false);
		}
		this.m_change = false;
		this.m_chaoMainId = -1;
		this.m_chaoSubId = -1;
		this.m_playerMain = CharaType.UNKNOWN;
		this.m_playerSub = CharaType.UNKNOWN;
		this.m_init = false;
		this.m_close = false;
		this.m_chaoSpIconTime = 0f;
		this.m_initChaoSetStock = 0;
		this.m_initPlayerMain = CharaType.UNKNOWN;
		this.m_initPlayerSub = CharaType.UNKNOWN;
		this.m_initChaoMain = -1;
		this.m_initChaoSub = -1;
		this.SetChangeBtn();
		this.ResetBtnDelayTime();
		this.ResetLastInputTime(DeckViewWindow.SELECT_TYPE.NUM);
	}

	public void SetChangeBtn()
	{
		if (this.m_changeBtnList == null)
		{
			this.m_changeBtnList = new Dictionary<DeckViewWindow.SELECT_TYPE, List<UIImageButton>>();
		}
		else if (this.m_changeBtnList.Count > 0)
		{
			this.m_changeBtnList.Clear();
		}
		if (this.m_changeBtnList != null)
		{
			GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "info_player");
			GameObject parent2 = GameObjectUtil.FindChildGameObject(base.gameObject, "info_chao");
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				DeckViewWindow.SELECT_TYPE key = (DeckViewWindow.SELECT_TYPE)i;
				List<UIImageButton> list = new List<UIImageButton>();
				switch (key)
				{
				case DeckViewWindow.SELECT_TYPE.CHARA_MAIN:
				{
					UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, "Btn_main_up");
					UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, "Btn_main_down");
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = true;
					}
					if (uIImageButton2 != null)
					{
						uIImageButton2.isEnabled = true;
					}
					list.Add(uIImageButton);
					list.Add(uIImageButton2);
					break;
				}
				case DeckViewWindow.SELECT_TYPE.CHARA_SUB:
				{
					UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, "Btn_sub_up");
					UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent, "Btn_sub_down");
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = true;
					}
					if (uIImageButton2 != null)
					{
						uIImageButton2.isEnabled = true;
					}
					list.Add(uIImageButton);
					list.Add(uIImageButton2);
					break;
				}
				case DeckViewWindow.SELECT_TYPE.CHAO_MAIN:
				{
					UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent2, "Btn_main_up");
					UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent2, "Btn_main_down");
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = true;
					}
					if (uIImageButton2 != null)
					{
						uIImageButton2.isEnabled = true;
					}
					list.Add(uIImageButton);
					list.Add(uIImageButton2);
					break;
				}
				case DeckViewWindow.SELECT_TYPE.CHAO_SUB:
				{
					UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent2, "Btn_sub_up");
					UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(parent2, "Btn_sub_down");
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = true;
					}
					if (uIImageButton2 != null)
					{
						uIImageButton2.isEnabled = true;
					}
					list.Add(uIImageButton);
					list.Add(uIImageButton2);
					break;
				}
				}
				if (list.Count > 0)
				{
					this.m_changeBtnList.Add(key, list);
				}
			}
		}
	}

	public void ResetBtnDelayTime()
	{
		this.SetAllChangeBtnEnabled(true);
		this.m_changeDelayCheckTime = 0f;
	}

	public void Setup(int mainChaoId, int subChaoId, GameObject parent)
	{
		this.m_btnLock = false;
		DeckViewWindow.s_instance = this;
		base.gameObject.SetActive(true);
		this.m_parent = parent;
		this.m_change = false;
		this.m_chaoMainId = mainChaoId;
		this.m_chaoSubId = subChaoId;
		this.m_init = true;
		this.m_close = false;
		this.m_chaoSpIconTime = 0f;
		this.m_windowAnimation = base.gameObject.GetComponentInChildren<Animation>();
		ActiveAnimation.Play(this.m_windowAnimation, Direction.Forward);
		this.SetChangeBtn();
		this.ResetBtnDelayTime();
		this.m_currentChaoSetStock = DeckUtil.GetDeckCurrentStockIndex();
		this.m_initChaoSetStock = this.m_currentChaoSetStock;
		this.m_isSaveData = new List<bool>();
		this.m_isSaveData.Add(true);
		this.m_isSaveData.Add(DeckUtil.IsChaoSetSave(1));
		this.m_isSaveData.Add(DeckUtil.IsChaoSetSave(2));
		this.ResetLastInputTime(DeckViewWindow.SELECT_TYPE.NUM);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_notice");
		if (gameObject != null)
		{
			this.m_detailTextBg = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_base_bg");
			this.m_detailTextLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_bonusnotice");
		}
		if (this.m_detailTextBg != null && this.m_detailTextLabel != null)
		{
			this.m_detailTextBg.alpha = 0f;
			this.m_detailTextLabel.alpha = 0f;
			TweenAlpha component = this.m_detailTextBg.GetComponent<TweenAlpha>();
			TweenAlpha component2 = this.m_detailTextLabel.GetComponent<TweenAlpha>();
			if (component != null && component2 != null)
			{
				component.enabled = false;
				component2.enabled = false;
			}
		}
		if (this.m_windowRoot == null)
		{
			this.m_windowRoot = GameObjectUtil.FindChildGameObject(base.gameObject, "window");
		}
		if (this.m_windowRoot != null)
		{
			this.m_windowRoot.SetActive(true);
		}
		this.m_bgCollider = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(base.gameObject, "blinder");
		if (this.m_bgCollider != null)
		{
			this.m_bgCollider.enabled = true;
		}
		UIPlayAnimation[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIPlayAnimation>();
		if (componentsInChildren != null && componentsInChildren.Length > 0)
		{
			UIPlayAnimation[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UIPlayAnimation uIPlayAnimation = array[i];
				uIPlayAnimation.enabled = true;
				if (uIPlayAnimation.onFinished == null || uIPlayAnimation.onFinished.Count == 0)
				{
					EventDelegate.Add(uIPlayAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), false);
				}
			}
		}
		SaveDataManager instance = SaveDataManager.Instance;
		this.m_initPlayerMain = instance.PlayerData.MainChara;
		this.m_initPlayerSub = instance.PlayerData.SubChara;
		this.m_initChaoMain = mainChaoId;
		this.m_initChaoSub = subChaoId;
		this.SetupChaoView();
		this.SetupPlayerView(instance);
		this.SetupBonusView();
		this.SetupTabView();
		this.m_charaList = null;
		if (instance != null)
		{
			ServerPlayerState playerState = ServerInterface.PlayerState;
			if (playerState != null)
			{
				int num = 29;
				for (int j = 0; j < num; j++)
				{
					ServerCharacterState serverCharacterState = playerState.CharacterState((CharaType)j);
					if (serverCharacterState != null && serverCharacterState.IsUnlocked)
					{
						CharaType charaType = (CharaType)j;
						if (this.m_charaList == null)
						{
							this.m_charaList = new List<CharaType>();
							this.m_charaList.Add(charaType);
						}
						else
						{
							this.m_charaList.Add(charaType);
						}
						global::Debug.Log("use chara:" + charaType);
					}
				}
			}
		}
		this.m_currentChaoSort = ChaoSort.NONE;
		this.m_chaoList = ChaoTable.GetPossessionChaoData();
	}

	private void SetupBonusView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_bonus");
		if (gameObject != null)
		{
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_bonus_0");
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_bonus_1");
			UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_bonus_2");
			UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_bonus_3");
			UILabel uILabel5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_bonus_4");
			this.SetBonus(ref uILabel, ref uILabel2, ref uILabel3, ref uILabel4, ref uILabel5);
		}
	}

	private void SetBonus(ref UILabel scoreBonus, ref UILabel ringBonus, ref UILabel animalBonus, ref UILabel distanceBonus, ref UILabel enemyBonus)
	{
		BonusParamContainer currentBonusData = BonusUtil.GetCurrentBonusData(this.m_playerMain, this.m_playerSub, this.m_chaoMainId, this.m_chaoSubId);
		if (currentBonusData != null)
		{
			int index = -1;
			Dictionary<BonusParam.BonusType, float> bonusInfo = currentBonusData.GetBonusInfo(index);
			this.SetupAbilityIcon(currentBonusData);
			this.SetupNoticeView(currentBonusData);
			if (bonusInfo != null)
			{
				if (bonusInfo.ContainsKey(BonusParam.BonusType.SCORE))
				{
					this.SetBonusParam(ref scoreBonus, bonusInfo[BonusParam.BonusType.SCORE]);
				}
				else
				{
					this.SetBonusParam(ref scoreBonus, 0f);
				}
				if (bonusInfo.ContainsKey(BonusParam.BonusType.RING))
				{
					this.SetBonusParam(ref ringBonus, bonusInfo[BonusParam.BonusType.RING]);
				}
				else
				{
					this.SetBonusParam(ref ringBonus, 0f);
				}
				if (bonusInfo.ContainsKey(BonusParam.BonusType.ANIMAL))
				{
					this.SetBonusParam(ref animalBonus, bonusInfo[BonusParam.BonusType.ANIMAL]);
				}
				else
				{
					this.SetBonusParam(ref animalBonus, 0f);
				}
				if (bonusInfo.ContainsKey(BonusParam.BonusType.DISTANCE))
				{
					this.SetBonusParam(ref distanceBonus, bonusInfo[BonusParam.BonusType.DISTANCE]);
				}
				else
				{
					this.SetBonusParam(ref distanceBonus, 0f);
				}
				if (bonusInfo.ContainsKey(BonusParam.BonusType.ENEMY_OBJBREAK))
				{
					this.SetBonusParam(ref enemyBonus, bonusInfo[BonusParam.BonusType.ENEMY_OBJBREAK]);
				}
				else
				{
					this.SetBonusParam(ref enemyBonus, 0f);
				}
			}
		}
	}

	private void SetBonusParam(ref UILabel bonusLabel, float param)
	{
		if (bonusLabel != null)
		{
			bonusLabel.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "clear_percent").text, new Dictionary<string, string>
			{
				{
					"{PARAM}",
					param.ToString()
				}
			});
		}
	}

	private void SetupTabView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Deck_tab");
		if (gameObject != null)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			list.Add("tab_1");
			list.Add("tab_2");
			list.Add("tab_3");
			list.Add("tab_4");
			list.Add("tab_5");
			list2.Add("OnClickTab1");
			list2.Add("OnClickTab2");
			list2.Add("OnClickTab3");
			list2.Add("OnClickTab4");
			list2.Add("OnClickTab5");
			GeneralUtil.SetToggleObject(base.gameObject, gameObject, list2, list, this.m_currentChaoSetStock, true);
		}
	}

	private void SetupChaoView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_chao");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "chao_main");
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "chao_sub");
			if (gameObject2 != null)
			{
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_chao_main_lv");
				UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_chao_main_name");
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject2, "img_chao_rank_bg_main");
				UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject2, "img_chao_type_main");
				UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject2, "img_chao_text_main");
				UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject2, "img_chao_main");
				this.SetChao(this.m_chaoMainId, ref uILabel2, ref uILabel, ref uISprite, ref uISprite2, ref uITexture, ref uISprite3);
			}
			if (gameObject3 != null)
			{
				UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject3, "Lbl_chao_sub_lv");
				UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject3, "Lbl_chao_sub_name");
				UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_chao_rank_bg_sub");
				UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_chao_type_sub");
				UITexture uITexture2 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject3, "img_chao_text_sub");
				UISprite uISprite6 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_chao_sub");
				this.SetChao(this.m_chaoSubId, ref uILabel4, ref uILabel3, ref uISprite4, ref uISprite5, ref uITexture2, ref uISprite6);
			}
		}
	}

	private void ResetupChaoView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_chao");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "chao_main");
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "chao_sub");
			if (gameObject2 != null)
			{
				UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject2, "img_chao_text_main");
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject2, "img_chao_main");
				this.ResetupChao(ref uITexture, ref uISprite);
			}
			if (gameObject3 != null)
			{
				UITexture uITexture2 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject3, "img_chao_text_sub");
				UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_chao_sub");
				this.ResetupChao(ref uITexture2, ref uISprite2);
			}
		}
	}

	private void SetupPlayerView(SaveDataManager saveMrg = null)
	{
		if (saveMrg == null)
		{
			saveMrg = SaveDataManager.Instance;
		}
		else
		{
			PlayerData playerData = saveMrg.PlayerData;
			this.m_playerMain = CharaType.UNKNOWN;
			this.m_playerSub = CharaType.UNKNOWN;
			if (playerData != null)
			{
				this.m_playerMain = playerData.MainChara;
				this.m_playerSub = playerData.SubChara;
			}
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_player");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "player_main");
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "player_sub");
			GameObject parent = GameObjectUtil.FindChildGameObject(gameObject, "base_main");
			GameObject parent2 = GameObjectUtil.FindChildGameObject(gameObject, "base_sub");
			if (gameObject2 != null)
			{
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_player_main_lv");
				UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_player_main_name");
				UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject2, "img_player_tex_main");
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject2, "img_player_main_genus");
				UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject2, "img_player_main_speacies");
				UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(parent, "base_star");
				UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_player_main_star_lv");
				this.SetPlayer(this.m_playerMain, ref uILabel2, ref uILabel, ref uITexture, ref uISprite2, ref uISprite, ref uISprite3, ref uILabel3);
			}
			if (gameObject3 != null)
			{
				UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject3, "Lbl_player_sub_lv");
				UILabel uILabel5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject3, "Lbl_player_sub_name");
				UITexture uITexture2 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject3, "img_player_tex_sub");
				UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_player_sub_genus");
				UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_player_sub_speacies");
				UISprite uISprite6 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(parent2, "base_star");
				UILabel uILabel6 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject3, "Lbl_player_sub_star_lv");
				this.SetPlayer(this.m_playerSub, ref uILabel5, ref uILabel4, ref uITexture2, ref uISprite5, ref uISprite4, ref uISprite6, ref uILabel6);
			}
			UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(gameObject, "Btn_change");
			if (uIImageButton != null)
			{
				if (this.m_playerMain == CharaType.UNKNOWN || this.m_playerSub == CharaType.UNKNOWN)
				{
					uIImageButton.gameObject.SetActive(false);
				}
				else
				{
					uIImageButton.gameObject.SetActive(true);
				}
			}
		}
	}

	private void SetupNoticeView(BonusParamContainer bonus)
	{
		string a = string.Empty;
		string text;
		if (bonus.IsDetailInfo(out text) && this.m_detailTextLabel != null)
		{
			a = this.m_detailTextLabel.text;
			this.m_detailTextLabel.text = text;
		}
		if (this.m_detailTextBg != null && this.m_detailTextLabel != null)
		{
			TweenAlpha component = this.m_detailTextBg.GetComponent<TweenAlpha>();
			TweenAlpha component2 = this.m_detailTextLabel.GetComponent<TweenAlpha>();
			if (!string.IsNullOrEmpty(text))
			{
				if (component != null && component2 != null)
				{
					if (a != text)
					{
						component.Reset();
						component2.Reset();
						this.m_detailTextBg.alpha = 0f;
						this.m_detailTextLabel.alpha = 0f;
						component.enabled = true;
						component2.enabled = true;
						component.Play();
						component2.Play();
					}
					else if (!component.enabled)
					{
						this.m_detailTextBg.alpha = 0f;
						this.m_detailTextLabel.alpha = 0f;
						component.enabled = true;
						component2.enabled = true;
						component.Play();
						component2.Play();
					}
				}
			}
			else
			{
				this.m_detailTextBg.alpha = 0f;
				this.m_detailTextLabel.alpha = 0f;
				if (component != null && component2 != null)
				{
					component.Reset();
					component2.Reset();
					component.enabled = false;
					component2.enabled = false;
				}
			}
		}
	}

	private void SetupAbilityIcon(BonusParamContainer bonus)
	{
		List<UISprite> list = new List<UISprite>();
		List<UISprite> list2 = new List<UISprite>();
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_player");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "player_main");
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "player_sub");
			if (gameObject2 != null)
			{
				GameObject gameObject4 = GameObjectUtil.FindChildGameObject(gameObject2, "ability");
				if (gameObject4 != null)
				{
					for (int i = 0; i < 8; i++)
					{
						UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject4, "img_ability_icon_" + i);
						if (!(uISprite != null))
						{
							break;
						}
						list.Add(uISprite);
					}
				}
			}
			if (gameObject3 != null)
			{
				GameObject gameObject5 = GameObjectUtil.FindChildGameObject(gameObject3, "ability");
				if (gameObject5 != null)
				{
					for (int j = 0; j < 8; j++)
					{
						UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject5, "img_ability_icon_" + j);
						if (!(uISprite2 != null))
						{
							break;
						}
						list2.Add(uISprite2);
					}
				}
			}
		}
		if (bonus != null)
		{
			if (list.Count > 0)
			{
				for (int k = 0; k < list.Count; k++)
				{
					list[k].enabled = false;
					list[k].gameObject.SetActive(false);
				}
			}
			if (list2.Count > 0)
			{
				for (int l = 0; l < list2.Count; l++)
				{
					list2[l].enabled = false;
					list2[l].gameObject.SetActive(false);
				}
			}
			BonusParam bonusParam = bonus.GetBonusParam(0);
			BonusParam bonusParam2 = bonus.GetBonusParam(1);
			if (bonusParam != null && bonusParam.GetBonusInfo(BonusParam.BonusTarget.CHARA, true) != null)
			{
				Dictionary<BonusParam.BonusType, float> bonusInfo = bonusParam.GetBonusInfo(BonusParam.BonusTarget.CHARA, true);
				this.SetAbilityIconSprite(ref list, bonusInfo, this.m_playerMain);
			}
			if (bonusParam2 != null && bonusParam2.GetBonusInfo(BonusParam.BonusTarget.CHARA, true) != null)
			{
				Dictionary<BonusParam.BonusType, float> bonusInfo2 = bonusParam2.GetBonusInfo(BonusParam.BonusTarget.CHARA, true);
				this.SetAbilityIconSprite(ref list2, bonusInfo2, this.m_playerSub);
			}
		}
		else
		{
			if (list.Count > 0)
			{
				for (int m = 0; m < list.Count; m++)
				{
					list[m].enabled = false;
				}
			}
			if (list2.Count > 0)
			{
				for (int n = 0; n < list2.Count; n++)
				{
					list2[n].enabled = false;
				}
			}
		}
	}

	private void SetAbilityIconSprite(ref List<UISprite> icons, Dictionary<BonusParam.BonusType, float> info, CharaType charaType)
	{
		if (info == null || icons == null)
		{
			return;
		}
		if (info.Count > 0)
		{
			int num = 0;
			Dictionary<BonusParam.BonusType, float>.KeyCollection keys = info.Keys;
			List<BonusParam.BonusType> list = new List<BonusParam.BonusType>();
			foreach (BonusParam.BonusType current in keys)
			{
				list.Add(current);
			}
			Dictionary<BonusParam.BonusType, bool> dictionary = BonusUtil.IsTeamBonus(charaType, list);
			foreach (BonusParam.BonusType current2 in keys)
			{
				if (info[current2] != 0f && dictionary != null && dictionary.ContainsKey(current2) && dictionary[current2])
				{
					string abilityIconSpriteName = BonusUtil.GetAbilityIconSpriteName(current2, info[current2]);
					if (!string.IsNullOrEmpty(abilityIconSpriteName))
					{
						icons[num].spriteName = BonusUtil.GetAbilityIconSpriteName(current2, info[current2]);
						icons[num].enabled = true;
						icons[num].gameObject.SetActive(true);
						num++;
					}
				}
			}
		}
	}

	private void SetPlayer(CharaType charaType, ref UILabel name, ref UILabel lv, ref UITexture chara, ref UISprite type, ref UISprite genus, ref UISprite star, ref UILabel starLv)
	{
		bool flag = false;
		if (charaType != CharaType.NUM && charaType != CharaType.UNKNOWN && HudCharacterPanelUtil.CheckValidChara(charaType))
		{
			chara.gameObject.SetActive(true);
			TextureRequestChara request = new TextureRequestChara(charaType, chara);
			TextureAsyncLoadManager.Instance.Request(request);
			HudCharacterPanelUtil.SetName(charaType, name.gameObject);
			HudCharacterPanelUtil.SetLevel(charaType, lv.gameObject);
			HudCharacterPanelUtil.SetCharaType(charaType, type.gameObject);
			HudCharacterPanelUtil.SetTeamType(charaType, genus.gameObject);
			if (star != null && starLv != null && ServerInterface.PlayerState != null)
			{
				ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(charaType);
				if (serverCharacterState != null)
				{
					star.gameObject.SetActive(true);
					starLv.text = string.Empty + serverCharacterState.star;
					starLv.gameObject.SetActive(true);
				}
				else
				{
					star.gameObject.SetActive(false);
					starLv.gameObject.SetActive(false);
				}
			}
			flag = true;
		}
		if (!flag)
		{
			if (lv != null)
			{
				lv.text = string.Empty;
			}
			if (name != null)
			{
				name.text = string.Empty;
			}
			if (chara != null)
			{
				chara.gameObject.SetActive(false);
			}
			if (type != null)
			{
				type.spriteName = string.Empty;
			}
			if (genus != null)
			{
				genus.spriteName = string.Empty;
			}
			if (star != null && starLv != null)
			{
				star.gameObject.SetActive(false);
				starLv.gameObject.SetActive(false);
			}
		}
	}

	private void SetChao(int id, ref UILabel name, ref UILabel lv, ref UISprite bg, ref UISprite type, ref UITexture tex, ref UISprite chao)
	{
		DataTable.ChaoData chaoData = ChaoTable.GetChaoData(id);
		if (chaoData != null)
		{
			if (lv != null)
			{
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_LevelNumber").text;
				lv.text = TextUtility.Replace(text, "{PARAM}", chaoData.level.ToString());
			}
			if (name != null)
			{
				name.text = chaoData.nameTwolines;
			}
			if (bg != null)
			{
				bg.spriteName = "ui_chao_set_bg_ll_" + (int)chaoData.rarity;
			}
			if (type != null)
			{
				switch (chaoData.charaAtribute)
				{
				case CharacterAttribute.SPEED:
					type.spriteName = "ui_chao_set_type_icon_speed";
					break;
				case CharacterAttribute.FLY:
					type.spriteName = "ui_chao_set_type_icon_fly";
					break;
				case CharacterAttribute.POWER:
					type.spriteName = "ui_chao_set_type_icon_power";
					break;
				default:
					type.spriteName = string.Empty;
					break;
				}
			}
			if (tex != null)
			{
				ChaoTextureManager instance = ChaoTextureManager.Instance;
				if (instance != null)
				{
					Texture loadedTexture = ChaoTextureManager.Instance.GetLoadedTexture(id);
					if (loadedTexture != null)
					{
						tex.mainTexture = loadedTexture;
					}
					else
					{
						ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(tex, null, true);
						ChaoTextureManager.Instance.GetTexture(id, info);
					}
					tex.alpha = 1f;
					chao.alpha = 0f;
					tex.enabled = true;
				}
			}
			else if (chao != null)
			{
				chao.enabled = true;
				chao.alpha = 1f;
			}
		}
		else
		{
			if (lv != null)
			{
				lv.text = string.Empty;
			}
			if (name != null)
			{
				name.text = string.Empty;
			}
			if (bg != null)
			{
				bg.spriteName = "ui_chao_set_ll_3";
			}
			if (type != null)
			{
				type.spriteName = string.Empty;
			}
			if (tex != null)
			{
				tex.alpha = 0f;
				tex.mainTexture = null;
			}
			if (chao != null)
			{
				chao.spriteName = string.Empty;
				chao.alpha = 0f;
			}
		}
	}

	private void ResetupChao(ref UITexture tex, ref UISprite chao)
	{
		if (chao != null && tex != null && chao.enabled && chao.alpha >= 0.1f && tex.alpha >= 0f && !string.IsNullOrEmpty(chao.spriteName))
		{
			string s = chao.spriteName.Replace("ui_tex_chao_", string.Empty);
			int num = int.Parse(s);
			if (num >= 0)
			{
				ChaoTextureManager instance = ChaoTextureManager.Instance;
				if (instance != null)
				{
					Texture loadedTexture = ChaoTextureManager.Instance.GetLoadedTexture(num);
					if (loadedTexture != null)
					{
						tex.mainTexture = loadedTexture;
					}
					else
					{
						ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(tex, null, true);
						ChaoTextureManager.Instance.GetTexture(num, info);
					}
					tex.alpha = 1f;
					chao.alpha = 0f;
					tex.enabled = true;
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_pressBtnType != DeckViewWindow.SELECT_TYPE.UNKNOWN)
		{
			this.m_pressTime += Time.deltaTime;
			if (this.m_pressTime > 0.5f)
			{
				switch (this.m_pressBtnType)
				{
				case DeckViewWindow.SELECT_TYPE.CHARA_MAIN:
					this.OnReleasePlayerMain();
					break;
				case DeckViewWindow.SELECT_TYPE.CHARA_SUB:
					this.OnReleasePlayerSub();
					break;
				case DeckViewWindow.SELECT_TYPE.CHAO_MAIN:
					this.OnReleaseChaoMain();
					break;
				case DeckViewWindow.SELECT_TYPE.CHAO_SUB:
					this.OnReleaseChaoSub();
					break;
				}
				this.m_pressTime = 0f;
				this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.UNKNOWN;
			}
		}
		if (this.m_chaoSpIconTime > 0f)
		{
			this.m_chaoSpIconTime += Time.deltaTime;
			if (this.m_chaoSpIconTime > 10f)
			{
				this.m_chaoSpIconTime = 0f;
				this.ResetupChaoView();
			}
		}
		float deltaTime = Time.deltaTime;
		this.UpdateChangeBtnDelay(deltaTime);
		this.UpdateLastInputTime(deltaTime);
	}

	private void ChangePlayer(DeckViewWindow.SELECT_TYPE select, CharaType type)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null && this.m_charaList != null && this.m_charaList.Count > 1 && this.m_charaList.Contains(type))
		{
			bool flag = false;
			if (select != DeckViewWindow.SELECT_TYPE.CHARA_MAIN)
			{
				if (select != DeckViewWindow.SELECT_TYPE.CHARA_SUB)
				{
					global::Debug.Log("ChangePlayer error select:" + select + " !!!!!!");
				}
				else if (type != this.m_playerSub)
				{
					if (type == this.m_playerMain)
					{
						this.m_playerMain = this.m_playerSub;
					}
					this.m_playerSub = type;
					flag = true;
				}
			}
			else if (type != this.m_playerMain)
			{
				if (type == this.m_playerSub)
				{
					this.m_playerSub = this.m_playerMain;
				}
				this.m_playerMain = type;
				flag = true;
			}
			if (flag)
			{
				this.m_change = true;
				this.SetupPlayerView(null);
				this.SetupBonusView();
				SoundManager.SePlay("sys_menu_decide", "SE");
			}
			else
			{
				SoundManager.SePlay("sys_window_close", "SE");
			}
		}
		else
		{
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void ChangePlayer(ref CharaType target, ref CharaType other, int param = 1)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null && this.m_charaList != null && this.m_charaList.Count > 1)
		{
			CharaType charaType = target;
			ServerPlayerState playerState = ServerInterface.PlayerState;
			if (playerState != null)
			{
				int count = this.m_charaList.Count;
				int num = 0;
				if (target != CharaType.UNKNOWN)
				{
					for (int i = 0; i < count; i++)
					{
						if (this.m_charaList[i] == target)
						{
							num = i;
							break;
						}
					}
					if (this.m_charaList[(num + param + count) % count] == other)
					{
						num = (num + param + count) % count;
					}
				}
				else
				{
					num = 0;
					if (this.m_charaList[(num + param + count) % count] == other)
					{
						num = 1;
					}
				}
				int index = (num + param + count) % count;
				ServerCharacterState serverCharacterState = playerState.CharacterState(this.m_charaList[index]);
				if (serverCharacterState != null && serverCharacterState.IsUnlocked)
				{
					target = this.m_charaList[index];
				}
			}
			if (target != charaType)
			{
				SoundManager.SePlay("sys_menu_decide", "SE");
				if (target == other)
				{
					other = charaType;
				}
				this.m_change = true;
				this.SetupPlayerView(null);
				this.SetupBonusView();
			}
			else
			{
				SoundManager.SePlay("sys_window_close", "SE");
			}
		}
		else
		{
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void OnPressPlayerMainUp()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = -1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHARA_MAIN;
		this.m_pressTime = 0f;
	}

	private void OnPressPlayerMainDown()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = 1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHARA_MAIN;
		this.m_pressTime = 0f;
	}

	private void OnReleasePlayerMain()
	{
		if (this.m_btnLock)
		{
			return;
		}
		if (this.m_init && this.m_pressBtnType == DeckViewWindow.SELECT_TYPE.CHARA_MAIN)
		{
			if (this.m_pressTime > 0.5f)
			{
				PlayerSetWindowUI.Create(this.m_playerMain, null, PlayerSetWindowUI.WINDOW_MODE.DEFAULT);
			}
			else
			{
				this.ResetLastInputTime(DeckViewWindow.SELECT_TYPE.CHARA_MAIN);
				this.SetChangeBtnDelay(DeckViewWindow.SELECT_TYPE.CHARA_MAIN);
				this.ChangePlayer(ref this.m_playerMain, ref this.m_playerSub, this.m_direction);
			}
		}
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.UNKNOWN;
	}

	private void OnPressPlayerSubUp()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = -1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHARA_SUB;
		this.m_pressTime = 0f;
	}

	private void OnPressPlayerSubDown()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = 1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHARA_SUB;
		this.m_pressTime = 0f;
	}

	private void OnReleasePlayerSub()
	{
		if (this.m_btnLock)
		{
			return;
		}
		if (this.m_init && this.m_pressBtnType == DeckViewWindow.SELECT_TYPE.CHARA_SUB)
		{
			if (this.m_pressTime > 0.5f)
			{
				PlayerSetWindowUI.Create(this.m_playerSub, null, PlayerSetWindowUI.WINDOW_MODE.DEFAULT);
			}
			else
			{
				this.ResetLastInputTime(DeckViewWindow.SELECT_TYPE.CHARA_SUB);
				this.SetChangeBtnDelay(DeckViewWindow.SELECT_TYPE.CHARA_SUB);
				this.ChangePlayer(ref this.m_playerSub, ref this.m_playerMain, this.m_direction);
			}
		}
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.UNKNOWN;
	}

	private void ChangeChaoSort(ChaoSort sort)
	{
		if (sort != ChaoSort.NUM && sort != ChaoSort.NONE)
		{
			this.m_currentChaoSort = sort;
			DataTable.ChaoData[] dataTable = ChaoTable.GetDataTable();
			ChaoDataSorting chaoDataSorting = new ChaoDataSorting(this.m_currentChaoSort);
			if (chaoDataSorting != null)
			{
				ChaoDataVisitorBase visitor = chaoDataSorting.visitor;
				if (visitor != null)
				{
					DataTable.ChaoData[] array = dataTable;
					for (int i = 0; i < array.Length; i++)
					{
						DataTable.ChaoData chaoData = array[i];
						chaoData.accept(ref visitor);
					}
					this.m_chaoList = chaoDataSorting.GetChaoList(false, DataTable.ChaoData.Rarity.NONE);
				}
			}
		}
	}

	private bool ChangeChao(DeckViewWindow.SELECT_TYPE select, int type)
	{
		bool flag = false;
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null && this.m_chaoList != null && this.m_chaoList.Count > 1)
		{
			if (select != DeckViewWindow.SELECT_TYPE.CHAO_MAIN)
			{
				if (select != DeckViewWindow.SELECT_TYPE.CHAO_SUB)
				{
					global::Debug.Log("ChangePlayer error select:" + select + " !!!!!!");
				}
				else if (type != this.m_chaoSubId)
				{
					if (type == this.m_chaoMainId)
					{
						this.m_chaoMainId = this.m_chaoSubId;
					}
					this.m_chaoSubId = type;
					flag = true;
				}
			}
			else if (type != this.m_chaoMainId)
			{
				if (type == this.m_chaoSubId)
				{
					this.m_chaoSubId = this.m_chaoMainId;
				}
				this.m_chaoMainId = type;
				flag = true;
			}
			if (flag)
			{
				this.m_change = true;
				this.SetupChaoView();
				this.SetupBonusView();
				SoundManager.SePlay("sys_menu_decide", "SE");
			}
			else
			{
				SoundManager.SePlay("sys_window_close", "SE");
			}
		}
		else
		{
			SoundManager.SePlay("sys_window_close", "SE");
		}
		return flag;
	}

	private bool ChangeChao(ref int target, ref int other, int param = 1)
	{
		bool result = false;
		bool flag = false;
		if (target >= 0 && this.m_chaoList != null && this.m_chaoList.Count > 1)
		{
			flag = true;
		}
		else if (target == -1 && this.m_chaoList != null && this.m_chaoList.Count > 0 && ((other >= 0 && this.m_chaoList.Count > 1) || (other < 0 && this.m_chaoList.Count > 0)))
		{
			flag = true;
		}
		if (flag)
		{
			int count = this.m_chaoList.Count;
			int num = -1;
			if (target >= 0)
			{
				for (int i = 0; i < count; i++)
				{
					if (target == this.m_chaoList[i].id)
					{
						num = i;
						break;
					}
				}
			}
			else
			{
				num = 0;
				int num2 = (num + param + count) % count;
				if (this.m_chaoList.Count > num2 && this.m_chaoList[num2] != null)
				{
					if (other >= 0)
					{
						int id = this.m_chaoList[num2].id;
						if (id == other)
						{
							num2 = (num + param + param + count) % count;
							if (this.m_chaoList.Count > num2 && this.m_chaoList[num2] != null)
							{
								num = 1;
							}
							else
							{
								num = -1;
							}
						}
					}
				}
				else
				{
					num = -1;
				}
			}
			if (num >= 0)
			{
				if (param != 0)
				{
					int num3 = target;
					int num4 = (num + param + count) % count;
					if (num4 >= 0 && num4 < this.m_chaoList.Count && this.m_chaoList[num4] != null)
					{
						if (this.m_chaoList[num4].id == other)
						{
							num4 = (num + param + param + count) % count;
							if (num4 < 0 || num4 >= this.m_chaoList.Count || this.m_chaoList[num4] == null)
							{
								return false;
							}
						}
						target = this.m_chaoList[num4].id;
						if (target != num3)
						{
							result = true;
						}
					}
				}
				else
				{
					result = true;
				}
			}
		}
		return result;
	}

	private void OnPressChaoMainUp()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = -1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHAO_MAIN;
		this.m_pressTime = 0f;
	}

	private void OnPressChaoMainDown()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = 1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHAO_MAIN;
		this.m_pressTime = 0f;
	}

	private void OnReleaseChaoMain()
	{
		if (this.m_btnLock)
		{
			return;
		}
		if (this.m_init && this.m_pressBtnType == DeckViewWindow.SELECT_TYPE.CHAO_MAIN)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.ResetLastInputTime(DeckViewWindow.SELECT_TYPE.CHAO_MAIN);
			if (this.m_pressTime > 0.5f)
			{
				ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
				if (window != null)
				{
					DataTable.ChaoData chaoData = ChaoTable.GetChaoData(this.m_chaoMainId);
					if (chaoData != null)
					{
						ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(chaoData);
						chaoInfo.level = chaoData.level;
						chaoInfo.detail = chaoData.GetDetailLevelPlusSP(chaoInfo.level);
						window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
					}
				}
			}
			else if (this.ChangeChao(ref this.m_chaoMainId, ref this.m_chaoSubId, this.m_direction))
			{
				this.m_change = true;
				this.SetupChaoView();
				this.SetupBonusView();
				this.SetChangeBtnDelay(DeckViewWindow.SELECT_TYPE.CHAO_MAIN);
			}
		}
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.UNKNOWN;
	}

	private void OnPressChaoSubUp()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = -1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHAO_SUB;
		this.m_pressTime = 0f;
	}

	private void OnPressChaoSubDown()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.m_direction = 1;
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.CHAO_SUB;
		this.m_pressTime = 0f;
	}

	private void OnReleaseChaoSub()
	{
		if (this.m_btnLock)
		{
			return;
		}
		if (this.m_init && this.m_pressBtnType == DeckViewWindow.SELECT_TYPE.CHAO_SUB)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.ResetLastInputTime(DeckViewWindow.SELECT_TYPE.CHAO_SUB);
			if (this.m_pressTime > 0.5f)
			{
				ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
				if (window != null)
				{
					DataTable.ChaoData chaoData = ChaoTable.GetChaoData(this.m_chaoSubId);
					if (chaoData != null)
					{
						ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(chaoData);
						chaoInfo.level = chaoData.level;
						chaoInfo.detail = chaoData.GetDetailLevelPlusSP(chaoInfo.level);
						window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
					}
				}
			}
			else if (this.ChangeChao(ref this.m_chaoSubId, ref this.m_chaoMainId, this.m_direction))
			{
				this.m_change = true;
				this.SetupChaoView();
				this.SetupBonusView();
				this.SetChangeBtnDelay(DeckViewWindow.SELECT_TYPE.CHAO_SUB);
			}
		}
		this.m_pressBtnType = DeckViewWindow.SELECT_TYPE.UNKNOWN;
	}

	private void OnClickChange()
	{
		if (this.m_btnLock)
		{
			return;
		}
		this.ResetLastInputTime(DeckViewWindow.SELECT_TYPE.NUM);
		if (this.m_playerMain != CharaType.UNKNOWN && this.m_playerSub != CharaType.UNKNOWN)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			CharaType playerMain = this.m_playerMain;
			CharaType playerSub = this.m_playerSub;
			this.m_playerMain = playerSub;
			this.m_playerSub = playerMain;
			this.m_change = true;
			this.SetupPlayerView(null);
			this.SetupBonusView();
		}
		else
		{
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void OnClickTab1()
	{
		if (this.m_currentChaoSetStock != 0 && !this.m_close)
		{
			this.DeckSetLoad(0);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab2()
	{
		if (this.m_currentChaoSetStock != 1 && !this.m_close)
		{
			this.DeckSetLoad(1);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab3()
	{
		if (this.m_currentChaoSetStock != 2 && !this.m_close)
		{
			this.DeckSetLoad(2);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab4()
	{
		if (this.m_currentChaoSetStock != 3 && !this.m_close)
		{
			this.DeckSetLoad(3);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab5()
	{
		if (this.m_currentChaoSetStock != 4 && !this.m_close)
		{
			this.DeckSetLoad(4);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickClose()
	{
		if (this.m_btnLock)
		{
			return;
		}
		if (this.m_init && !this.m_close)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_init = false;
			this.m_close = true;
			UIPlayAnimation[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIPlayAnimation>();
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				UIPlayAnimation[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					UIPlayAnimation uIPlayAnimation = array[i];
					uIPlayAnimation.enabled = false;
				}
			}
			this.ResetRequestData(false);
			if (this.m_change && this.m_initPlayerMain == this.m_playerMain && this.m_initPlayerSub == this.m_playerSub && this.m_initChaoMain == this.m_chaoMainId && this.m_initChaoSub == this.m_chaoSubId)
			{
				this.m_change = false;
				if (this.m_initChaoSetStock != this.m_currentChaoSetStock)
				{
					DeckUtil.SetDeckCurrentStockIndex(this.m_currentChaoSetStock);
					this.m_parent.SendMessage("OnMsgReset", SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this.m_change)
			{
				this.m_change = false;
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (!GeneralUtil.IsNetwork() || loggedInServerInterface == null)
				{
					DeckUtil.SetDeckCurrentStockIndex(this.m_initChaoSetStock);
					DeckUtil.DeckSetSave(this.m_initChaoSetStock, this.m_initPlayerMain, this.m_initPlayerSub, this.m_initChaoMain, this.m_initChaoSub);
					GeneralUtil.SetCharasetBtnIcon(this.m_initPlayerMain, this.m_initPlayerSub, this.m_initChaoMain, this.m_initChaoSub, this.m_parent, "Btn_charaset");
					GeneralUtil.ShowNoCommunication("ShowNoCommunication");
					ChaoTextureManager.Instance.RemoveChaoTextureForMainMenuEnd();
					if (this.m_parent != null)
					{
						this.m_parent.SendMessage("OnMsgDeckViewWindowNetworkError", SendMessageOptions.DontRequireReceiver);
					}
					this.CloseWindowAnim();
					return;
				}
				GeneralUtil.SetCharasetBtnIcon(this.m_playerMain, this.m_playerSub, this.m_chaoMainId, this.m_chaoSubId, this.m_parent, "Btn_charaset");
				this.SetRequestData();
				DeckUtil.DeckSetSave(this.m_currentChaoSetStock, this.m_playerMain, this.m_playerSub, this.m_chaoMainId, this.m_chaoSubId);
				DeckUtil.SetDeckCurrentStockIndex(this.m_currentChaoSetStock);
				this.PlayerDataUpdate();
				if (loggedInServerInterface != null && this.m_playerMain != this.m_playerSub)
				{
					if (this.m_initChaoMain != this.m_chaoMainId || this.m_initChaoSub != this.m_chaoSubId)
					{
						int id = (int)ServerItem.CreateFromChaoId(this.m_chaoMainId).id;
						int id2 = (int)ServerItem.CreateFromChaoId(this.m_chaoSubId).id;
						loggedInServerInterface.RequestServerEquipChao(id, id2, base.gameObject);
					}
					else
					{
						this.ServerEquipChao_Dummy();
					}
				}
				this.m_parent.SendMessage("OnMsgReset", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				if (this.m_parent != null)
				{
					this.m_parent.SendMessage("OnMsgDeckViewWindowNotChange", SendMessageOptions.DontRequireReceiver);
				}
				this.CloseWindowAnim();
			}
			if (this.m_parent != null)
			{
				this.m_parent.SendMessage("OnMsgDeckViewWindowEnd", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void OnClickSelect()
	{
		if (this.m_btnLock)
		{
			return;
		}
		if (this.m_init && !this.m_close)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_init = false;
			this.m_close = true;
			UIPlayAnimation[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIPlayAnimation>();
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				UIPlayAnimation[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					UIPlayAnimation uIPlayAnimation = array[i];
					uIPlayAnimation.enabled = false;
				}
			}
			this.ResetRequestData(false);
			if (this.m_change && this.m_initPlayerMain == this.m_playerMain && this.m_initPlayerSub == this.m_playerSub && this.m_initChaoMain == this.m_chaoMainId && this.m_initChaoSub == this.m_chaoSubId)
			{
				this.m_change = false;
				if (this.m_initChaoSetStock != this.m_currentChaoSetStock)
				{
					DeckUtil.SetDeckCurrentStockIndex(this.m_currentChaoSetStock);
				}
			}
			if (this.m_change)
			{
				this.m_change = false;
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (!GeneralUtil.IsNetwork() || loggedInServerInterface == null)
				{
					DeckUtil.SetDeckCurrentStockIndex(this.m_initChaoSetStock);
					DeckUtil.DeckSetSave(this.m_initChaoSetStock, this.m_initPlayerMain, this.m_initPlayerSub, this.m_initChaoMain, this.m_initChaoSub);
					GeneralUtil.ShowNoCommunication("ShowNoCommunication");
					ChaoTextureManager.Instance.RemoveChaoTextureForMainMenuEnd();
					HudMenuUtility.SendStartPlayerChaoPage();
					if (this.m_parent != null)
					{
						this.m_parent.SendMessage("OnMsgDeckViewWindowNetworkError", SendMessageOptions.DontRequireReceiver);
					}
					this.CloseWindowAnim();
					return;
				}
				this.SetRequestData();
				DeckUtil.DeckSetSave(this.m_currentChaoSetStock, this.m_playerMain, this.m_playerSub, this.m_chaoMainId, this.m_chaoSubId);
				DeckUtil.SetDeckCurrentStockIndex(this.m_currentChaoSetStock);
				this.PlayerDataUpdate();
				if (loggedInServerInterface != null && this.m_playerMain != this.m_playerSub)
				{
					if (this.m_initChaoMain != this.m_chaoMainId || this.m_initChaoSub != this.m_chaoSubId)
					{
						int id = (int)ServerItem.CreateFromChaoId(this.m_chaoMainId).id;
						int id2 = (int)ServerItem.CreateFromChaoId(this.m_chaoSubId).id;
						loggedInServerInterface.RequestServerEquipChao(id, id2, base.gameObject);
					}
					else
					{
						this.ServerEquipChao_Dummy();
					}
				}
			}
			else
			{
				if (this.m_parent != null)
				{
					this.m_parent.SendMessage("OnMsgDeckViewWindowNotChange", SendMessageOptions.DontRequireReceiver);
				}
				this.CloseWindowAnim();
			}
			if (this.m_parent != null)
			{
				this.m_parent.SendMessage("OnMsgDeckViewWindowEnd", SendMessageOptions.DontRequireReceiver);
			}
			HudMenuUtility.SendStartPlayerChaoPage();
		}
	}

	private void UpdateChangeBtnDelay(float delteTime)
	{
		if (this.m_changeDelayCheckTime > 0f)
		{
			this.m_changeDelayCheckTime -= delteTime;
			if (this.m_changeDelayCheckTime <= 0f)
			{
				this.m_changeDelayCheckTime = 0f;
				this.SetAllChangeBtnEnabled(true);
			}
		}
	}

	private void SetAllChangeBtnEnabled(bool enabled)
	{
		if (this.m_changeBtnList != null && this.m_changeBtnList.Count > 0)
		{
			Dictionary<DeckViewWindow.SELECT_TYPE, List<UIImageButton>>.KeyCollection keys = this.m_changeBtnList.Keys;
			foreach (DeckViewWindow.SELECT_TYPE current in keys)
			{
				if (this.m_changeBtnList[current] != null && this.m_changeBtnList[current].Count > 0)
				{
					foreach (UIImageButton current2 in this.m_changeBtnList[current])
					{
						if (current2 != null)
						{
							current2.isEnabled = enabled;
						}
					}
				}
			}
		}
	}

	private void UpdateLastInputTime(float delteTime)
	{
		if (!this.m_isLastInputTime)
		{
			return;
		}
		int num = 4;
		if (this.m_lastInputTime == null)
		{
			this.m_lastInputTime = new Dictionary<DeckViewWindow.SELECT_TYPE, float>();
			for (int i = 0; i < num; i++)
			{
				DeckViewWindow.SELECT_TYPE key = (DeckViewWindow.SELECT_TYPE)i;
				this.m_lastInputTime.Add(key, delteTime);
			}
		}
		else
		{
			int num2 = 0;
			for (int j = 0; j < num; j++)
			{
				DeckViewWindow.SELECT_TYPE sELECT_TYPE = (DeckViewWindow.SELECT_TYPE)j;
				if (this.m_lastInputTime.ContainsKey(sELECT_TYPE))
				{
					if (this.m_lastInputTime[sELECT_TYPE] >= 0f)
					{
						Dictionary<DeckViewWindow.SELECT_TYPE, float> lastInputTime;
						Dictionary<DeckViewWindow.SELECT_TYPE, float> expr_90 = lastInputTime = this.m_lastInputTime;
						DeckViewWindow.SELECT_TYPE sELECT_TYPE2;
						DeckViewWindow.SELECT_TYPE expr_95 = sELECT_TYPE2 = sELECT_TYPE;
						float num3 = lastInputTime[sELECT_TYPE2];
						expr_90[expr_95] = num3 + delteTime;
						if (this.m_lastInputTime[sELECT_TYPE] > 0.75f)
						{
							sELECT_TYPE2 = sELECT_TYPE;
							if (sELECT_TYPE2 != DeckViewWindow.SELECT_TYPE.CHAO_MAIN)
							{
								if (sELECT_TYPE2 != DeckViewWindow.SELECT_TYPE.CHAO_SUB)
								{
									this.m_lastInputTime[sELECT_TYPE] = -1f;
								}
								else
								{
									GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "info_chao");
									GameObject gameObject = GameObjectUtil.FindChildGameObject(parent, "chao_sub");
									if (gameObject != null && this.m_chaoSubId >= 0)
									{
										UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject, "img_chao_text_sub");
										if (uITexture != null)
										{
											if (GeneralUtil.CheckChaoTexture(uITexture, this.m_chaoSubId))
											{
												this.m_lastInputTime[sELECT_TYPE] = -1f;
											}
											else
											{
												Texture loadedTexture = ChaoTextureManager.Instance.GetLoadedTexture(this.m_chaoSubId);
												if (loadedTexture != null)
												{
													uITexture.mainTexture = loadedTexture;
												}
												else
												{
													ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
													ChaoTextureManager.Instance.GetTexture(this.m_chaoSubId, info);
												}
												uITexture.alpha = 1f;
												uITexture.enabled = true;
											}
										}
									}
									else
									{
										this.m_lastInputTime[sELECT_TYPE] = -1f;
									}
								}
							}
							else
							{
								GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "info_chao");
								GameObject gameObject = GameObjectUtil.FindChildGameObject(parent, "chao_main");
								if (gameObject != null && this.m_chaoMainId >= 0)
								{
									UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject, "img_chao_text_main");
									if (uITexture != null)
									{
										if (GeneralUtil.CheckChaoTexture(uITexture, this.m_chaoMainId))
										{
											this.m_lastInputTime[sELECT_TYPE] = -1f;
										}
										else
										{
											Texture loadedTexture2 = ChaoTextureManager.Instance.GetLoadedTexture(this.m_chaoMainId);
											if (loadedTexture2 != null)
											{
												uITexture.mainTexture = loadedTexture2;
											}
											else
											{
												ChaoTextureManager.CallbackInfo info2 = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
												ChaoTextureManager.Instance.GetTexture(this.m_chaoMainId, info2);
											}
											uITexture.alpha = 1f;
											uITexture.enabled = true;
										}
									}
								}
								else
								{
									this.m_lastInputTime[sELECT_TYPE] = -1f;
								}
							}
						}
					}
					else
					{
						num2++;
					}
				}
			}
			if (num2 >= num)
			{
				this.m_isLastInputTime = false;
			}
		}
	}

	private void ResetLastInputTime(DeckViewWindow.SELECT_TYPE targetType = DeckViewWindow.SELECT_TYPE.NUM)
	{
		this.m_isLastInputTime = true;
		if (this.m_lastInputTime == null)
		{
			this.m_lastInputTime = new Dictionary<DeckViewWindow.SELECT_TYPE, float>();
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				DeckViewWindow.SELECT_TYPE key = (DeckViewWindow.SELECT_TYPE)i;
				this.m_lastInputTime.Add(key, 0f);
			}
		}
		else if (targetType != DeckViewWindow.SELECT_TYPE.NUM)
		{
			if (this.m_lastInputTime.ContainsKey(targetType))
			{
				this.m_lastInputTime[targetType] = 0f;
			}
		}
		else
		{
			int num2 = 4;
			for (int j = 0; j < num2; j++)
			{
				DeckViewWindow.SELECT_TYPE key2 = (DeckViewWindow.SELECT_TYPE)j;
				if (this.m_lastInputTime.ContainsKey(key2))
				{
					this.m_lastInputTime[key2] = 0f;
				}
			}
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (!this.m_close)
		{
			if (msg != null)
			{
				msg.StaySequence();
			}
			if (PlayerSetWindowUI.isActive)
			{
				return;
			}
			if (ChaoSetWindowUI.isActive)
			{
				return;
			}
			if (this.m_btnLock)
			{
				return;
			}
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_ok");
			if (gameObject != null)
			{
				UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
				if (component != null)
				{
					component.SendMessage("OnClick");
				}
			}
		}
	}

	private void SetRequestData()
	{
		this.m_reqChaoSetStock = this.m_currentChaoSetStock;
		this.m_reqPlayerMain = this.m_playerMain;
		this.m_reqPlayerSub = this.m_playerSub;
		this.m_reqChaoMain = this.m_chaoMainId;
		this.m_reqChaoSub = this.m_chaoSubId;
	}

	private void PlayerDataUpdate()
	{
		if (this.m_reqChaoSetStock >= 0)
		{
			DeckUtil.SetDeckCurrentStockIndex(this.m_reqChaoSetStock);
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					SaveDataManager instance2 = SaveDataManager.Instance;
					instance2.PlayerData.MainChara = this.m_reqPlayerMain;
					instance2.PlayerData.SubChara = this.m_reqPlayerSub;
					instance2.PlayerData.MainChaoID = this.m_reqChaoMain;
					instance2.PlayerData.SubChaoID = this.m_reqChaoSub;
				}
			}
		}
	}

	private void ResetRequestData(bool isSavaDataUpdate)
	{
		if (isSavaDataUpdate && this.m_reqChaoSetStock >= 0)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					SaveDataManager instance2 = SaveDataManager.Instance;
					instance2.PlayerData.MainChara = this.m_reqPlayerMain;
					instance2.PlayerData.SubChara = this.m_reqPlayerSub;
					instance2.PlayerData.MainChaoID = this.m_reqChaoMain;
					instance2.PlayerData.SubChaoID = this.m_reqChaoSub;
					HudMenuUtility.SendMsgUpdateSaveDataDisplay();
				}
			}
		}
		this.m_reqChaoSetStock = -1;
		this.m_reqPlayerMain = CharaType.UNKNOWN;
		this.m_reqPlayerSub = CharaType.UNKNOWN;
		this.m_reqChaoMain = -1;
		this.m_reqChaoSub = -1;
	}

	private void CloseWindowAnim()
	{
		if (this.m_windowAnimation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_windowAnimation, Direction.Reverse);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_close)
		{
			this.OnFinished();
		}
	}

	private void ServerChangeCharacter_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.ResetRequestData(true);
		ItemSetMenu.UpdateBoostItemForCharacterDeck();
		if (this.m_parent != null)
		{
			this.m_parent.SendMessage("OnMsgDeckViewWindowChange", SendMessageOptions.DontRequireReceiver);
		}
		this.CloseWindowAnim();
	}

	private void ServerChangeCharacter_Dummy()
	{
		this.ResetRequestData(true);
		ItemSetMenu.UpdateBoostItemForCharacterDeck();
		if (this.m_parent != null)
		{
			this.m_parent.SendMessage("OnMsgDeckViewWindowChange", SendMessageOptions.DontRequireReceiver);
		}
		this.CloseWindowAnim();
	}

	private void ServerEquipChao_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		bool flag = false;
		if (loggedInServerInterface != null)
		{
			if (this.m_initPlayerMain != this.m_reqPlayerMain)
			{
				flag = true;
			}
			if (this.m_initPlayerSub != this.m_reqPlayerSub)
			{
				flag = true;
			}
			if (flag)
			{
				int mainCharaId = -1;
				int subCharaId = -1;
				ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(this.m_reqPlayerMain);
				ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(this.m_reqPlayerSub);
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
			if (this.m_initPlayerMain != this.m_reqPlayerMain)
			{
				flag = true;
			}
			if (this.m_initPlayerSub != this.m_reqPlayerSub)
			{
				flag = true;
			}
			if (flag)
			{
				int mainCharaId = -1;
				int subCharaId = -1;
				ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(this.m_reqPlayerMain);
				ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(this.m_reqPlayerSub);
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

	private void OnFinished()
	{
		if (this.m_bgCollider != null)
		{
			this.m_bgCollider.enabled = false;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "anime_blinder");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		if (this.m_windowRoot == null)
		{
			this.m_windowRoot = GameObjectUtil.FindChildGameObject(base.gameObject, "window");
		}
		if (this.m_windowRoot != null)
		{
			this.m_windowRoot.SetActive(false);
		}
		base.gameObject.SetActive(false);
	}

	public void DeckSetLoad(int stock)
	{
		DeckUtil.DeckSetSave(this.m_currentChaoSetStock, this.m_playerMain, this.m_playerSub, this.m_chaoMainId, this.m_chaoSubId);
		if (DeckUtil.DeckSetLoad(stock, ref this.m_playerMain, ref this.m_playerSub, ref this.m_chaoMainId, ref this.m_chaoSubId, null))
		{
			this.m_change = true;
			this.SetupChaoView();
			this.SetupBonusView();
			this.SetupPlayerView(null);
			this.m_currentChaoSetStock = stock;
			this.SetupTabView();
			DeckUtil.SetDeckCurrentStockIndex(this.m_currentChaoSetStock);
		}
	}

	public bool ChaoSetName(int stock, out string main, out string sub)
	{
		main = string.Empty;
		sub = string.Empty;
		if (stock < 0 || stock >= 3)
		{
			return false;
		}
		bool result = false;
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				int num = -1;
				int num2 = -1;
				systemdata.GetDeckData(stock, out num, out num2);
				if (num >= 0 || num2 >= 0)
				{
					if (num >= 0)
					{
						DataTable.ChaoData chaoData = ChaoTable.GetChaoData(num);
						main = chaoData.name;
					}
					if (num2 >= 0)
					{
						DataTable.ChaoData chaoData = ChaoTable.GetChaoData(num2);
						sub = chaoData.name;
					}
					result = true;
				}
			}
		}
		return result;
	}

	private void SetChangeBtnDelay(DeckViewWindow.SELECT_TYPE type)
	{
		this.SetAllChangeBtnEnabled(false);
		this.m_changeDelayCheckTime = 0.5f;
	}

	public static void Reset()
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "DeckViewWindow");
			if (gameObject != null)
			{
				DeckViewWindow deckViewWindow = GameObjectUtil.FindChildGameObjectComponent<DeckViewWindow>(gameObject.gameObject, "DeckViewWindow");
				if (deckViewWindow != null)
				{
					deckViewWindow.Init();
				}
			}
		}
	}

	public static DeckViewWindow Create(GameObject parent = null)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			PlayerData playerData = instance.PlayerData;
			if (playerData != null)
			{
				DeckViewWindow.Create(playerData.MainChaoID, playerData.SubChaoID, parent);
			}
		}
		return null;
	}

	public static DeckViewWindow Create(int mainChaoId, int subChaoId, GameObject parent = null)
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "DeckViewWindow");
			DeckViewWindow deckViewWindow = null;
			if (gameObject != null)
			{
				deckViewWindow = gameObject.GetComponent<DeckViewWindow>();
				if (deckViewWindow == null)
				{
					deckViewWindow = gameObject.AddComponent<DeckViewWindow>();
				}
				if (deckViewWindow != null)
				{
					deckViewWindow.Setup(mainChaoId, subChaoId, parent);
				}
			}
			return deckViewWindow;
		}
		return null;
	}
}
