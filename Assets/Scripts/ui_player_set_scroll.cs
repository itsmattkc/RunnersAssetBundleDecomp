using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ui_player_set_scroll : MonoBehaviour
{
	private enum BTN_MODE
	{
		GET,
		ADD,
		MAX,
		LOCK_EPISODE,
		LOCK
	}

	private PlayerCharaList m_parent;

	private int m_currentDeck;

	private List<int> m_selectList;

	private ui_player_set_scroll.BTN_MODE m_btnMode;

	private Dictionary<int, int> m_btnCost;

	private CharaType m_charaType = CharaType.UNKNOWN;

	private ServerCharacterState m_charaState;

	private static bool s_starTextDefaultInit;

	private static Color s_starTextDefault;

	public PlayerCharaList parent
	{
		get
		{
			return this.m_parent;
		}
	}

	public CharaType charaType
	{
		get
		{
			return this.m_charaType;
		}
	}

	public void Setup(PlayerCharaList parent, ServerCharacterState characterState)
	{
		this.m_parent = parent;
		this.m_charaType = characterState.charaType;
		this.m_charaState = characterState;
		this.m_currentDeck = DeckUtil.GetDeckCurrentStockIndex();
		if (this.m_charaType != CharaType.UNKNOWN && this.m_charaState != null)
		{
			this.SetParam();
			this.SetObject();
		}
	}

	public bool UpdateView()
	{
		this.m_currentDeck = DeckUtil.GetDeckCurrentStockIndex();
		if (this.m_charaType != CharaType.UNKNOWN)
		{
			ServerPlayerState playerState = ServerInterface.PlayerState;
			this.m_charaState = playerState.CharacterState(this.m_charaType);
			if (this.m_charaType != CharaType.UNKNOWN && this.m_charaState != null)
			{
				this.SetParam();
				this.SetObject();
			}
			return true;
		}
		return false;
	}

	private int GetSelect()
	{
		int result = 0;
		if (this.m_selectList != null && this.m_selectList.Count > 0 && this.m_selectList.Count > this.m_currentDeck)
		{
			result = this.m_selectList[this.m_currentDeck];
		}
		return result;
	}

	private void SetParam()
	{
		this.m_btnMode = ui_player_set_scroll.BTN_MODE.LOCK;
		if (this.m_btnCost != null)
		{
			this.m_btnCost.Clear();
		}
		else
		{
			this.m_btnCost = new Dictionary<int, int>();
		}
		if (this.m_charaState.IsUnlocked)
		{
			this.m_btnMode = ui_player_set_scroll.BTN_MODE.ADD;
			if (this.m_charaState.star >= this.m_charaState.starMax && this.m_charaState.starMax > 0)
			{
				this.m_btnMode = ui_player_set_scroll.BTN_MODE.MAX;
			}
		}
		else
		{
			this.m_btnMode = ui_player_set_scroll.BTN_MODE.GET;
			if (this.m_charaState.Condition == ServerCharacterState.LockCondition.MILEAGE_EPISODE)
			{
				this.m_btnMode = ui_player_set_scroll.BTN_MODE.LOCK_EPISODE;
			}
		}
		if (this.m_btnMode != ui_player_set_scroll.BTN_MODE.ADD || this.m_btnMode != ui_player_set_scroll.BTN_MODE.GET)
		{
			if (this.m_charaState.priceNumRings > 0)
			{
				this.m_btnCost.Add(910000, this.m_charaState.priceNumRings);
			}
			if (this.m_charaState.priceNumRedRings > 0)
			{
				this.m_btnCost.Add(900000, this.m_charaState.priceNumRedRings);
			}
			if (this.m_charaState.IsRoulette)
			{
				this.m_btnCost.Add(0, 0);
			}
		}
		if (this.m_selectList != null)
		{
			this.m_selectList.Clear();
		}
		else
		{
			this.m_selectList = new List<int>();
		}
		List<DeckUtil.DeckSet> deckList = DeckUtil.GetDeckList();
		if (deckList != null && deckList.Count > 0)
		{
			for (int i = 0; i < deckList.Count; i++)
			{
				int item = 0;
				if (deckList[i].charaMain == this.m_charaType)
				{
					item = 1;
				}
				else if (deckList[i].charaSub == this.m_charaType)
				{
					item = 2;
				}
				this.m_selectList.Add(item);
			}
		}
	}

	private void SetObject()
	{
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_lv");
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_name");
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_star");
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_player_tex");
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_player_genus");
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_player_speacies");
		UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_word_icon");
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_2_get");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "ability");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "pattern_lock");
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_1_lvUP");
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "pattern_0");
		GameObject gameObject5 = GameObjectUtil.FindChildGameObject(base.gameObject, "pattern_1");
		if (gameObject != null && gameObject2 != null && gameObject3 != null && gameObject4 != null && gameObject5 != null)
		{
			if (this.m_charaState.IsUnlocked)
			{
				gameObject2.SetActive(false);
				gameObject3.SetActive(true);
				gameObject4.SetActive(false);
				gameObject5.SetActive(true);
				if (!ui_player_set_scroll.s_starTextDefaultInit)
				{
					ui_player_set_scroll.s_starTextDefault = new Color(uILabel3.color.r, uILabel3.color.g, uILabel3.color.b, uILabel3.color.a);
					ui_player_set_scroll.s_starTextDefaultInit = true;
				}
				if (this.m_charaState.starMax > 0 && this.m_charaState.star >= this.m_charaState.starMax)
				{
					uILabel3.color = new Color(0.9647059f, 0.454901963f, 0f);
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = false;
					}
				}
				else
				{
					uILabel3.color = ui_player_set_scroll.s_starTextDefault;
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = true;
					}
				}
			}
			else
			{
				if (uIImageButton != null)
				{
					uIImageButton.isEnabled = true;
				}
				gameObject2.SetActive(true);
				gameObject3.SetActive(false);
				gameObject4.SetActive(true);
				gameObject5.SetActive(false);
			}
			Dictionary<BonusParam.BonusType, float> teamBonusList = this.m_charaState.GetTeamBonusList();
			if (teamBonusList != null && teamBonusList.Count > 0)
			{
				gameObject.SetActive(true);
				int count = teamBonusList.Count;
				GameObject gameObject6 = null;
				switch (count)
				{
				case 1:
					gameObject6 = GameObjectUtil.FindChildGameObject(gameObject, "1_item");
					break;
				case 2:
					gameObject6 = GameObjectUtil.FindChildGameObject(gameObject, "2_item");
					break;
				case 3:
					gameObject6 = GameObjectUtil.FindChildGameObject(gameObject, "4_item");
					if (gameObject6 != null)
					{
						GameObject gameObject7 = GameObjectUtil.FindChildGameObject(gameObject6, "cell_4");
						if (gameObject7 != null)
						{
							gameObject7.SetActive(false);
						}
					}
					break;
				case 4:
					gameObject6 = GameObjectUtil.FindChildGameObject(gameObject, "4_item");
					break;
				default:
					global::Debug.Log("ui_player_set_scroll SetObject error  abilityNum:" + count + " !!!!!!!");
					break;
				}
				if (gameObject6 != null)
				{
					for (int i = 1; i <= 5; i++)
					{
						if (i != count)
						{
							GameObject gameObject8 = GameObjectUtil.FindChildGameObject(gameObject, i + "_item");
							if (gameObject8 != null)
							{
								gameObject8.SetActive(false);
							}
						}
					}
					gameObject6.SetActive(true);
					int num = 1;
					Dictionary<BonusParam.BonusType, float>.KeyCollection keys = teamBonusList.Keys;
					foreach (BonusParam.BonusType current in keys)
					{
						GameObject gameObject7 = GameObjectUtil.FindChildGameObject(gameObject6, "cell_" + num);
						if (!(gameObject7 != null))
						{
							break;
						}
						gameObject7.SetActive(true);
						UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject7, "img_ability_icon_" + num);
						UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject7, "Lbl_ability_name_" + num);
						if (uISprite4 != null && uILabel4 != null)
						{
							float num2 = teamBonusList[current];
							uISprite4.spriteName = BonusUtil.GetAbilityIconSpriteName(current, num2);
							string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "bonus_percent").text;
							if (!string.IsNullOrEmpty(text))
							{
								if (current == BonusParam.BonusType.SPEED)
								{
									uILabel4.text = text.Replace("{BONUS}", (100f - num2).ToString());
								}
								else
								{
									if (current == BonusParam.BonusType.TOTAL_SCORE && Mathf.Abs(num2) <= 1f)
									{
										num2 *= 100f;
									}
									if (num2 >= 0f)
									{
										uILabel4.text = "+" + text.Replace("{BONUS}", num2.ToString());
									}
									else
									{
										uILabel4.text = text.Replace("{BONUS}", num2.ToString());
									}
								}
							}
						}
						num++;
					}
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		this.SetCharacter(this.m_charaType, ref uILabel2, ref uILabel, ref uILabel3, ref uITexture, ref uISprite2, ref uISprite, ref uISprite3);
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_0_info", base.gameObject, "OnClickChara");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_2_get", base.gameObject, "OnClickGet");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_1_lvUP", base.gameObject, "OnClickLvUp");
	}

	private void SetCharacter(CharaType charaType, ref UILabel name, ref UILabel lv, ref UILabel star, ref UITexture chara, ref UISprite type, ref UISprite genus, ref UISprite select)
	{
		bool flag = false;
		if (charaType != CharaType.NUM && charaType != CharaType.UNKNOWN && this.m_charaState != null && HudCharacterPanelUtil.CheckValidChara(charaType))
		{
			chara.gameObject.SetActive(true);
			if (this.m_charaState.IsUnlocked)
			{
				chara.color = new Color(1f, 1f, 1f);
				TextureRequestChara request = new TextureRequestChara(charaType, chara);
				TextureAsyncLoadManager.Instance.Request(request);
			}
			else
			{
				chara.color = new Color(0f, 0f, 0f);
				TextureRequestChara request2 = new TextureRequestChara(charaType, chara);
				TextureAsyncLoadManager.Instance.Request(request2);
			}
			HudCharacterPanelUtil.SetName(charaType, name.gameObject);
			HudCharacterPanelUtil.SetLevel(charaType, lv.gameObject);
			HudCharacterPanelUtil.SetCharaType(charaType, type.gameObject);
			HudCharacterPanelUtil.SetTeamType(charaType, genus.gameObject);
			if (select != null)
			{
				int select2 = this.GetSelect();
				if (select2 == 1)
				{
					select.spriteName = "ui_player_set_main";
				}
				else if (select2 == 2)
				{
					select.spriteName = "ui_player_set_sub";
				}
				else
				{
					select.spriteName = string.Empty;
				}
			}
			if (star != null)
			{
				star.text = this.m_charaState.star.ToString();
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
			if (star != null)
			{
				star.text = string.Empty;
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
			if (select != null)
			{
				select.spriteName = string.Empty;
			}
		}
	}

	private void OnClickChara()
	{
		if (this.m_parent != null && this.m_parent.isTutorial)
		{
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.CHARASELECT_MAIN);
			this.m_parent.SetTutorialEnd();
		}
		if (this.m_charaState != null && this.m_charaState.IsUnlocked)
		{
			PlayerSetWindowUI.Create(this.m_charaType, this, PlayerSetWindowUI.WINDOW_MODE.SET);
		}
	}

	private void OnClickGet()
	{
		if (this.m_parent != null && this.m_parent.isTutorial)
		{
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.CHARASELECT_MAIN);
			this.m_parent.SetTutorialEnd();
		}
		PlayerSetWindowUI.Create(this.m_charaType, this, PlayerSetWindowUI.WINDOW_MODE.BUY);
	}

	private void OnClickLvUp()
	{
		BackKeyManager.InvalidFlag = true;
		if (this.m_parent != null && this.m_parent.isTutorial)
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "chara_level_up_explan",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("MainMenu", "chara_level_up_explan_caption"),
				message = TextUtility.GetCommonText("MainMenu", "chara_level_up_explan"),
				finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowCharaLevelUpCloseCallback)
			});
		}
		PlayerLvupWindow.Open(this, this.m_charaType);
		global::Debug.Log("OnClickLvUp");
	}

	private void GeneralWindowCharaLevelUpCloseCallback()
	{
		TutorialCursor.StartTutorialCursor(TutorialCursor.Type.CHARASELECT_LEVEL_UP);
	}
}
