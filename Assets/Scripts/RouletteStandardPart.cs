using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RouletteStandardPart : RoulettePartsBase
{
	private const string FADE_ANIM_INTRO = "ui_simple_load_intro_Anim2";

	private const string FADE_ANIM_OUTRO = "ui_simple_load_outro_Anim2";

	private const float FRONT_COLLIDER_DELAY = 0.25f;

	private const float EVENT_UI_UPDATE_TIME = 10f;

	[SerializeField]
	private Animation m_wordAnim;

	[SerializeField]
	private GameObject m_wordGet;

	[SerializeField]
	private GameObject m_wordRankup;

	[SerializeField]
	private GameObject m_wordJackpot;

	[SerializeField]
	private GameObject m_wordLavel;

	[SerializeField]
	private GameObject m_spEgg;

	[SerializeField]
	private List<GameObject> m_Eggs;

	[SerializeField]
	private GameObject m_backButton;

	[SerializeField]
	private GameObject m_oddsButton;

	[SerializeField]
	private List<GameObject> m_spinButtons;

	[SerializeField]
	private GameObject m_costBase;

	[SerializeField]
	private GameObject m_eventUI;

	[SerializeField]
	private GameObject m_frontCollider;

	[SerializeField]
	private Animation m_fadeAnime;

	private float m_frontColliderDelay;

	private int m_remainingNum;

	private int m_remainingOffset;

	private float m_animeTime;

	private ServerWheelOptionsData.SPIN_BUTTON m_spinBtn = ServerWheelOptionsData.SPIN_BUTTON.NONE;

	private bool m_spinBtnActive;

	private string m_spinErrorWindow;

	private UIImageButton m_backButtonImg;

	private List<int> m_spinCostList;

	private List<ServerItem> m_attentionItemList;

	private bool m_isJackpot;

	private GameObject m_spinMultiButton;

	private List<GameObject> m_costList;

	private List<Constants.Campaign.emType> m_campaign;

	private RouletteCategory m_currentCategory;

	private int m_eventUiCount;

	private float m_eventUiNextUpdate = -1f;

	protected override void UpdateParts()
	{
		if (this.m_backButtonImg != null)
		{
			if (base.isSpin && base.spinDecisionIndex == -1)
			{
				this.m_backButtonImg.gameObject.SetActive(true);
				if (this.m_parent.spinTime > 10f)
				{
					this.m_backButtonImg.isEnabled = true;
				}
			}
			else
			{
				this.m_backButtonImg.gameObject.SetActive(false);
			}
		}
		if (!string.IsNullOrEmpty(this.m_spinErrorWindow))
		{
			if (GeneralWindow.IsCreated(this.m_spinErrorWindow))
			{
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						ServerWheelOptionsData.SPIN_BUTTON spinBtn = this.m_spinBtn;
						if (spinBtn != ServerWheelOptionsData.SPIN_BUTTON.RING)
						{
							if (spinBtn == ServerWheelOptionsData.SPIN_BUTTON.RSRING)
							{
								HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP, false);
							}
						}
						else
						{
							HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.RING_TO_SHOP, false);
						}
					}
					GeneralWindow.Close();
					this.m_spinErrorWindow = null;
				}
			}
			else
			{
				GeneralWindow.Close();
				this.m_spinErrorWindow = null;
			}
		}
		if (RouletteManager.GetCurrentLoading() != null && RouletteManager.GetCurrentLoading().Count > 0)
		{
			if (this.m_frontCollider != null && !this.m_frontCollider.activeSelf)
			{
				this.m_frontCollider.SetActive(true);
			}
			this.m_frontColliderDelay = 0.25f;
		}
		else if (this.m_parent != null && (this.m_parent.isSpinGetWindow || this.m_parent.isWordAnime))
		{
			if (this.m_frontCollider != null && !this.m_frontCollider.activeSelf)
			{
				this.m_frontCollider.SetActive(true);
			}
			this.m_frontColliderDelay = 0.25f;
		}
		else if (base.isDelay)
		{
			if (this.m_frontCollider != null && !this.m_frontCollider.activeSelf)
			{
				this.m_frontCollider.SetActive(true);
			}
			if (this.m_frontColliderDelay < 0.0625f)
			{
				this.m_frontColliderDelay = 0.0625f;
			}
		}
		if (this.m_frontColliderDelay > 0f)
		{
			this.m_frontColliderDelay -= Time.deltaTime / Time.timeScale;
			if (this.m_frontColliderDelay <= 0f)
			{
				if (this.m_frontCollider != null)
				{
					this.m_frontCollider.SetActive(false);
				}
				this.m_frontColliderDelay = 0f;
			}
		}
		if (this.m_animeTime > 0f)
		{
			this.m_animeTime -= Time.deltaTime;
			if (this.m_animeTime <= 0f)
			{
				this.AnimationFinishCallback();
				this.m_animeTime = 0f;
				if (this.m_wordGet != null)
				{
					this.m_wordGet.SetActive(false);
				}
				if (this.m_wordJackpot != null)
				{
					this.m_wordJackpot.SetActive(false);
				}
				if (this.m_wordLavel != null)
				{
					this.m_wordLavel.SetActive(false);
				}
				if (this.m_wordRankup != null)
				{
					this.m_wordRankup.SetActive(false);
				}
				if (this.m_isJackpot)
				{
					this.m_isJackpot = false;
				}
			}
		}
		if (this.m_currentCategory != RouletteCategory.NONE && this.m_currentCategory != RouletteCategory.ITEM && this.m_attentionItemList == null)
		{
			if (RouletteManager.Instance != null && !RouletteManager.Instance.isCurrentPrizeLoading)
			{
				this.m_attentionItemList = base.wheel.GetAttentionItemList();
				if (this.m_attentionItemList != null)
				{
					this.m_eventUiNextUpdate = 0f;
					this.SetEventUI();
				}
			}
		}
		else if (this.m_eventUiNextUpdate > 0f)
		{
			this.m_eventUiNextUpdate -= Time.deltaTime;
			if (this.m_eventUiNextUpdate <= 0f)
			{
				this.m_eventUiNextUpdate = 0f;
				this.SetEventUI();
			}
		}
	}

	public override void UpdateEffectSetting()
	{
		this.m_isEffectLock = !base.parent.IsEffect(RouletteTop.ROULETTE_EFFECT_TYPE.BG_PARTICLE);
		bool enabled = base.parent.IsEffect(RouletteTop.ROULETTE_EFFECT_TYPE.SPIN);
		if (this.m_spinButtons != null && this.m_spinButtons.Count > 0)
		{
			int num = 0;
			foreach (GameObject current in this.m_spinButtons)
			{
				UIPlayAnimation[] components = current.GetComponents<UIPlayAnimation>();
				if (components != null)
				{
					UIPlayAnimation[] array = components;
					for (int i = 0; i < array.Length; i++)
					{
						UIPlayAnimation uIPlayAnimation = array[i];
						uIPlayAnimation.enabled = enabled;
					}
				}
				num++;
			}
		}
	}

	public override void Setup(RouletteTop parent)
	{
		base.Setup(parent);
		this.m_eventUiCount = 0;
		this.m_eventUiNextUpdate = -1f;
		this.m_isEffectLock = false;
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
			this.m_backButtonImg = this.m_backButton.GetComponent<UIImageButton>();
			if (this.m_backButtonImg != null)
			{
				this.m_backButtonImg.isEnabled = false;
			}
		}
		this.m_isJackpot = false;
		this.m_spinErrorWindow = null;
		this.m_frontColliderDelay = 0f;
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(false);
		}
		this.m_animeTime = 0f;
		if (this.m_attentionItemList != null)
		{
			this.m_attentionItemList.Clear();
			this.m_attentionItemList = null;
		}
		if (base.wheel != null)
		{
			base.wheel.ChangeSpinCost(0);
			this.m_attentionItemList = base.wheel.GetAttentionItemList();
			this.m_currentCategory = base.wheel.category;
			if (this.m_attentionItemList != null)
			{
				this.m_eventUiNextUpdate = 0f;
			}
		}
		if (this.m_wordGet != null)
		{
			this.m_wordGet.SetActive(false);
		}
		if (this.m_wordJackpot != null)
		{
			this.m_wordJackpot.SetActive(false);
		}
		if (this.m_wordLavel != null)
		{
			this.m_wordLavel.SetActive(false);
		}
		if (this.m_wordRankup != null)
		{
			this.m_wordRankup.SetActive(false);
		}
		this.SetEventUI();
		this.SetButton();
		this.SetEgg();
		this.UpdateEffectSetting();
	}

	public override void OnUpdateWheelData(ServerWheelOptionsData data)
	{
		base.OnUpdateWheelData(data);
		this.m_isJackpot = false;
		this.m_spinErrorWindow = null;
		this.m_frontColliderDelay = 0.125f;
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(true);
		}
		this.m_animeTime = 0f;
		if (this.m_attentionItemList != null)
		{
			this.m_attentionItemList.Clear();
			this.m_attentionItemList = null;
		}
		if (base.wheel != null)
		{
			this.m_attentionItemList = base.wheel.GetAttentionItemList();
			this.m_currentCategory = base.wheel.category;
		}
		this.SetEventUI();
		this.SetButton();
		this.SetEgg();
		this.UpdateEffectSetting();
	}

	private void SetEventAttention()
	{
		if (this.m_eventUI != null && this.m_attentionItemList != null && this.m_eventUiCount >= 0)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_eventUI, "add_space");
			if (gameObject != null)
			{
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "chao_set");
				GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "item_set");
				GameObject gameObject4 = GameObjectUtil.FindChildGameObject(gameObject, "player_set");
				ServerItem serverItem = this.m_attentionItemList[this.m_eventUiCount % this.m_attentionItemList.Count];
				ServerItem.IdType idType = serverItem.idType;
				if (idType != ServerItem.IdType.EQUIP_ITEM)
				{
					if (idType != ServerItem.IdType.CHARA)
					{
						if (idType != ServerItem.IdType.CHAO)
						{
							if (gameObject3 != null)
							{
								gameObject3.SetActive(true);
								UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_item");
								if (uISprite != null)
								{
									int id = (int)serverItem.id;
									uISprite.spriteName = "ui_cmn_icon_item_" + id;
								}
							}
							if (gameObject2 != null)
							{
								gameObject2.SetActive(false);
							}
							if (gameObject4 != null)
							{
								gameObject4.SetActive(false);
							}
						}
						else
						{
							if (gameObject2 != null)
							{
								gameObject2.SetActive(true);
								UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject2, "img_tex_chao");
								UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject2, "img_bord_bg");
								if (uITexture != null && ChaoTextureManager.Instance != null)
								{
									ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
									ChaoTextureManager.Instance.GetTexture(serverItem.chaoId, info);
								}
								if (uISprite2 != null)
								{
									if (serverItem.id >= ServerItem.Id.CHAO_BEGIN_SRARE)
									{
										uISprite2.spriteName = "ui_chao_set_bg_m_2";
									}
									else if (serverItem.id >= ServerItem.Id.CHAO_BEGIN_RARE)
									{
										uISprite2.spriteName = "ui_chao_set_bg_m_1";
									}
									else
									{
										uISprite2.spriteName = "ui_chao_set_bg_m_0";
									}
								}
							}
							if (gameObject3 != null)
							{
								gameObject3.SetActive(false);
							}
							if (gameObject4 != null)
							{
								gameObject4.SetActive(false);
							}
						}
					}
					else
					{
						if (gameObject4 != null)
						{
							gameObject4.SetActive(true);
							UITexture uITexture2 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject4, "img_tex_player");
							if (uITexture2 != null)
							{
								TextureRequestChara request = new TextureRequestChara(serverItem.charaType, uITexture2);
								TextureAsyncLoadManager.Instance.Request(request);
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
				}
				else
				{
					if (gameObject3 != null)
					{
						gameObject3.SetActive(true);
						UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_item");
						if (uISprite3 != null)
						{
							int num = serverItem.id - ServerItem.Id.INVINCIBLE;
							uISprite3.spriteName = "ui_cmn_icon_item_" + num;
						}
					}
					if (gameObject2 != null)
					{
						gameObject2.SetActive(false);
					}
					if (gameObject4 != null)
					{
						gameObject4.SetActive(false);
					}
				}
			}
		}
	}

	private void SetEventUI()
	{
		if (this.m_eventUI != null)
		{
			if (!RouletteUtility.isTutorial || this.m_parent.wheelData.category != RouletteCategory.PREMIUM)
			{
				if (EventUtility.IsEnableRouletteUI())
				{
					bool flag = this.m_attentionItemList != null;
					if (flag && this.m_eventUiNextUpdate >= 0f)
					{
						this.SetEventAttention();
						this.m_eventUiNextUpdate = 10f;
						this.m_eventUiCount++;
					}
					this.m_eventUI.SetActive(flag);
				}
				else
				{
					this.m_eventUI.SetActive(false);
				}
			}
			else
			{
				this.m_eventUI.SetActive(false);
			}
		}
	}

	private void SetEgg()
	{
		if (this.m_parent == null || this.m_parent.wheelData == null)
		{
			return;
		}
		int num = 0;
		bool eggSeting = this.m_parent.wheelData.GetEggSeting(out num);
		if (this.m_Eggs != null && this.m_Eggs.Count > 0)
		{
			bool flag = true;
			if (RouletteUtility.isTutorial && this.m_parent.wheelData.category == RouletteCategory.PREMIUM && this.m_parent.addSpecialEgg)
			{
				flag = false;
			}
			if (flag)
			{
				for (int i = 0; i < this.m_Eggs.Count; i++)
				{
					if (this.m_Eggs[i] != null)
					{
						this.m_Eggs[i].SetActive(num > i);
					}
				}
			}
		}
		if (this.m_spEgg != null)
		{
			this.m_spEgg.SetActive(eggSeting);
		}
	}

	private void UpdateButtonCount(int offset)
	{
		if (this.m_spinBtn == ServerWheelOptionsData.SPIN_BUTTON.FREE)
		{
			this.m_remainingOffset = offset;
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_spinButtons[(int)this.m_spinBtn], "img_free_counter_bg");
			if (this.m_remainingNum - this.m_remainingOffset < 0 && gameObject != null)
			{
				gameObject.SetActive(false);
			}
			else if (gameObject != null)
			{
				gameObject.SetActive(true);
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_number_00");
				UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_number_0");
				if (uISprite != null && uISprite2 != null)
				{
					if (this.m_remainingNum - this.m_remainingOffset >= 100)
					{
						uISprite.spriteName = "ui_roulette_free_counter_number_9";
						uISprite2.spriteName = "ui_roulette_free_counter_number_9";
					}
					else if (this.m_remainingNum - this.m_remainingOffset <= 0)
					{
						uISprite.spriteName = "ui_roulette_free_counter_number_0";
						uISprite2.spriteName = "ui_roulette_free_counter_number_0";
					}
					else
					{
						uISprite.spriteName = "ui_roulette_free_counter_number_" + (this.m_remainingNum - this.m_remainingOffset) / 10 % 10;
						uISprite2.spriteName = "ui_roulette_free_counter_number_" + (this.m_remainingNum - this.m_remainingOffset) % 10;
					}
				}
			}
		}
	}

	private void SetButton()
	{
		if (this.m_parent == null || this.m_parent.wheelData == null)
		{
			return;
		}
		this.m_campaign = this.m_parent.wheelData.GetCampaign();
		if (this.m_costBase != null && this.m_costList == null)
		{
			this.m_costList = new List<GameObject>();
			for (int i = 0; i < 5; i++)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_costBase, "roulette_cost_" + i);
				UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_costBase, "roulette_cost_" + i);
				if (!(gameObject != null))
				{
					break;
				}
				gameObject.SetActive(false);
				this.m_costList.Add(gameObject);
				if (uIButtonMessage != null)
				{
					uIButtonMessage.functionName = "OnClickSpinCost" + i;
				}
			}
		}
		this.SetCostItem(-1, 0);
		int num = 0;
		bool flag = false;
		this.m_spinBtn = this.m_parent.wheelData.GetSpinButtonSeting(out num, out flag);
		this.m_spinBtnActive = flag;
		flag = true;
		this.m_remainingOffset = 0;
		this.m_remainingNum = num;
		if (this.m_oddsButton != null)
		{
			this.m_oddsButton.SetActive(true);
		}
		this.SetButtonMulti();
		this.SetButtonSpin(num, flag);
		if (this.m_parent != null && this.m_parent.wheelData.category != RouletteCategory.ITEM)
		{
			if (RouletteUtility.isTutorial && this.m_parent.wheelData.category == RouletteCategory.PREMIUM)
			{
				GeneralUtil.SetRouletteBannerBtn(base.gameObject, "Btn_ad", base.gameObject, "OnClickBanner", this.m_parent.wheelData.category, false);
			}
			else
			{
				GeneralUtil.SetRouletteBannerBtn(base.gameObject, "Btn_ad", base.gameObject, "OnClickBanner", this.m_parent.wheelData.category, true);
			}
		}
		else
		{
			GeneralUtil.SetRouletteBannerBtn(base.gameObject, "Btn_ad", base.gameObject, "OnClickBanner", this.m_parent.wheelData.category, false);
		}
	}

	private void SetCostItem(int costItemId = -1, int offset = 0)
	{
		this.m_spinCostList = base.wheel.GetSpinCostItemIdList();
		if (this.m_spinCostList != null && this.m_spinCostList.Count > 0)
		{
			if (this.m_costList != null && this.m_costList.Count > 0)
			{
				for (int i = 0; i < this.m_costList.Count; i++)
				{
					GameObject gameObject = this.m_costList[i];
					if (gameObject != null)
					{
						if (i < this.m_spinCostList.Count && this.m_spinCostList[i] != 910000 && this.m_spinCostList[i] != 900000 && this.m_spinCostList[i] > 0)
						{
							gameObject.SetActive(true);
							UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_icon");
							UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_num");
							UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_num_sdw");
							if (uISprite != null && uILabel != null && uILabel != null)
							{
								uISprite.spriteName = "ui_cmn_icon_item_" + this.m_spinCostList[i];
								int num = base.wheel.GetSpinCostItemNum(this.m_spinCostList[i]);
								if (costItemId == this.m_spinCostList[i])
								{
									num += offset;
								}
								uILabel.text = HudUtility.GetFormatNumString<int>(num);
								uILabel2.text = HudUtility.GetFormatNumString<int>(num);
							}
						}
						else
						{
							gameObject.SetActive(false);
						}
					}
				}
			}
			else if (this.m_costList != null && this.m_costList.Count > 0)
			{
				for (int j = 0; j < this.m_costList.Count; j++)
				{
					this.m_costList[j].SetActive(false);
				}
			}
		}
		else if (this.m_costList != null && this.m_costList.Count > 0)
		{
			for (int k = 0; k < this.m_costList.Count; k++)
			{
				this.m_costList[k].SetActive(false);
			}
		}
	}

	private void SetButtonSpin(int count, bool btnAct)
	{
		if (this.m_spinButtons != null && this.m_spinButtons.Count > 0)
		{
			for (int i = 0; i < this.m_spinButtons.Count; i++)
			{
				if (this.m_spinButtons[i] != null)
				{
					if (this.m_spinBtn == (ServerWheelOptionsData.SPIN_BUTTON)i)
					{
						this.m_spinButtons[i].SetActive(true);
						UIPlayAnimation[] componentsInChildren = this.m_spinButtons[i].GetComponentsInChildren<UIPlayAnimation>();
						if (componentsInChildren != null && componentsInChildren.Length > 0)
						{
							UIPlayAnimation[] array = componentsInChildren;
							for (int j = 0; j < array.Length; j++)
							{
								UIPlayAnimation uIPlayAnimation = array[j];
								uIPlayAnimation.enabled = this.m_spinBtnActive;
							}
						}
						UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, this.m_spinButtons[i].name);
						if (uIImageButton != null)
						{
							uIImageButton.isEnabled = btnAct;
						}
						UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_spinButtons[i], "img_sale_icon");
						switch (this.m_spinBtn)
						{
						case ServerWheelOptionsData.SPIN_BUTTON.FREE:
						{
							GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_spinButtons[i], "img_free_counter_bg");
							if (this.m_remainingNum - this.m_remainingOffset < 0 && gameObject != null)
							{
								gameObject.SetActive(false);
							}
							else if (gameObject != null)
							{
								gameObject.SetActive(true);
								UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_number_00");
								UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_number_0");
								if (uISprite2 != null && uISprite3 != null)
								{
									if (this.m_remainingNum - this.m_remainingOffset >= 100)
									{
										uISprite2.spriteName = "ui_roulette_free_counter_number_9";
										uISprite3.spriteName = "ui_roulette_free_counter_number_9";
									}
									else if (this.m_remainingNum - this.m_remainingOffset <= 0)
									{
										uISprite2.spriteName = "ui_roulette_free_counter_number_0";
										uISprite3.spriteName = "ui_roulette_free_counter_number_0";
									}
									else
									{
										uISprite2.spriteName = "ui_roulette_free_counter_number_" + (this.m_remainingNum - this.m_remainingOffset) / 10 % 10;
										uISprite3.spriteName = "ui_roulette_free_counter_number_" + (this.m_remainingNum - this.m_remainingOffset) % 10;
									}
								}
							}
							if (uISprite != null)
							{
								if (this.m_campaign != null && this.m_campaign.Contains(Constants.Campaign.emType.FreeWheelSpinCount))
								{
									uISprite.gameObject.SetActive(true);
								}
								else
								{
									uISprite.gameObject.SetActive(false);
								}
							}
							break;
						}
						case ServerWheelOptionsData.SPIN_BUTTON.RING:
						case ServerWheelOptionsData.SPIN_BUTTON.RSRING:
						case ServerWheelOptionsData.SPIN_BUTTON.RAID:
						{
							UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_spinButtons[i], "Lbl_btn_" + i);
							UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_spinButtons[i], "Lbl_btn_" + i + "_sdw");
							if (uILabel != null && uILabel2 != null)
							{
								uILabel.text = HudUtility.GetFormatNumString<int>(count);
								uILabel2.text = HudUtility.GetFormatNumString<int>(count);
							}
							if (uISprite != null)
							{
								if (this.m_campaign != null && this.m_campaign.Contains(Constants.Campaign.emType.ChaoRouletteCost))
								{
									uISprite.gameObject.SetActive(true);
								}
								else
								{
									uISprite.gameObject.SetActive(false);
								}
							}
							break;
						}
						case ServerWheelOptionsData.SPIN_BUTTON.TICKET:
						{
							UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_spinButtons[i], "img_btn_" + i + "_icon");
							UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_spinButtons[i], "Lbl_btn_" + i);
							UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_spinButtons[i], "Lbl_btn_" + i + "_sdw");
							if (uISprite4 != null)
							{
								uISprite4.spriteName = base.wheel.GetRouletteTicketSprite();
							}
							if (uILabel3 != null && uILabel4 != null)
							{
								uILabel3.text = string.Empty + HudUtility.GetFormatNumString<int>(count);
								uILabel4.text = string.Empty + HudUtility.GetFormatNumString<int>(count);
							}
							if (uISprite != null)
							{
								uISprite.gameObject.SetActive(false);
							}
							break;
						}
						default:
							this.m_spinButtons[i].SetActive(false);
							break;
						}
					}
					else
					{
						this.m_spinButtons[i].SetActive(false);
					}
				}
			}
		}
	}

	private void SetButtonMulti()
	{
		if (this.m_spinMultiButton == null)
		{
			this.m_spinMultiButton = GameObjectUtil.FindChildGameObject(base.gameObject, "multiple_switch");
		}
		if (this.m_spinMultiButton != null)
		{
			bool flag = false;
			if (base.wheel != null && base.wheel.isGeneral && base.wheel.GetRouletteRank() == RouletteUtility.WheelRank.Normal && (!RouletteUtility.isTutorial || this.m_parent.wheelData.category != RouletteCategory.PREMIUM) && this.m_spinBtn != ServerWheelOptionsData.SPIN_BUTTON.FREE)
			{
				flag = true;
			}
			else if (base.wheel != null && base.wheel.category == RouletteCategory.PREMIUM && (!RouletteUtility.isTutorial || this.m_parent.wheelData.category != RouletteCategory.PREMIUM) && this.m_spinBtn != ServerWheelOptionsData.SPIN_BUTTON.FREE)
			{
				flag = true;
			}
			if (flag)
			{
				this.m_spinMultiButton.SetActive(true);
				UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_spinMultiButton.gameObject, "Tgl_multi_0");
				UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_spinMultiButton.gameObject, "Tgl_multi_1");
				UIButtonMessage uIButtonMessage3 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_spinMultiButton.gameObject, "Tgl_multi_2");
				if (uIButtonMessage == null)
				{
					uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_spinMultiButton.gameObject, "Tgl_single");
				}
				if (uIButtonMessage != null)
				{
					uIButtonMessage.functionName = "OnClickSpinMulti0";
					UIImageButton componentInChildren = uIButtonMessage.gameObject.GetComponentInChildren<UIImageButton>();
					if (componentInChildren != null)
					{
						componentInChildren.isEnabled = base.wheel.IsMulti(1);
					}
				}
				if (uIButtonMessage2 != null)
				{
					uIButtonMessage2.functionName = "OnClickSpinMulti1";
					UILabel componentInChildren2 = uIButtonMessage2.gameObject.GetComponentInChildren<UILabel>();
					UIImageButton componentInChildren3 = uIButtonMessage2.gameObject.GetComponentInChildren<UIImageButton>();
					if (componentInChildren2 != null)
					{
						componentInChildren2.text = string.Empty + 3;
					}
					if (componentInChildren3 != null)
					{
						componentInChildren3.isEnabled = base.wheel.IsMulti(3);
					}
				}
				if (uIButtonMessage3 != null)
				{
					uIButtonMessage3.functionName = "OnClickSpinMulti2";
					UILabel componentInChildren4 = uIButtonMessage3.gameObject.GetComponentInChildren<UILabel>();
					UIImageButton componentInChildren5 = uIButtonMessage3.gameObject.GetComponentInChildren<UIImageButton>();
					if (componentInChildren4 != null)
					{
						componentInChildren4.text = string.Empty + 5;
					}
					if (componentInChildren5 != null)
					{
						componentInChildren5.isEnabled = base.wheel.IsMulti(5);
					}
				}
				if (uIButtonMessage != null)
				{
					UIToggle componentInChildren6 = uIButtonMessage.gameObject.GetComponentInChildren<UIToggle>();
					if (componentInChildren6 != null)
					{
						if (base.wheel.multi == 1)
						{
							componentInChildren6.startsActive = true;
							componentInChildren6.SendMessage("Start");
						}
						else
						{
							componentInChildren6.startsActive = false;
						}
					}
				}
				if (uIButtonMessage2 != null)
				{
					UIToggle componentInChildren7 = uIButtonMessage2.gameObject.GetComponentInChildren<UIToggle>();
					if (componentInChildren7 != null)
					{
						if (base.wheel.multi == 3)
						{
							componentInChildren7.startsActive = true;
							componentInChildren7.SendMessage("Start");
						}
						else
						{
							componentInChildren7.startsActive = false;
						}
					}
				}
				if (uIButtonMessage3 != null)
				{
					UIToggle componentInChildren8 = uIButtonMessage3.gameObject.GetComponentInChildren<UIToggle>();
					if (componentInChildren8 != null)
					{
						if (base.wheel.multi == 5)
						{
							componentInChildren8.startsActive = true;
							componentInChildren8.SendMessage("Start");
						}
						else
						{
							componentInChildren8.startsActive = false;
						}
					}
				}
			}
			else
			{
				this.m_spinMultiButton.SetActive(false);
			}
		}
	}

	private void OnClickFront()
	{
		if (this.m_parent == null || base.isDelay)
		{
			return;
		}
		if (base.isSpin && base.spinDecisionIndex != -1)
		{
			this.m_parent.OnRouletteSpinSkip();
		}
	}

	private void OnClickOdds()
	{
		if (this.m_parent == null || base.isDelay || (RouletteUtility.isTutorial && this.m_parent.wheelData.category == RouletteCategory.PREMIUM))
		{
			return;
		}
		if (!RouletteManager.Instance.isCurrentPrizeLoading && !RouletteManager.IsPrizeLoading(this.m_parent.wheelData.category))
		{
			this.m_parent.SetDelayTime(0.2f);
			RouletteManager.RequestRoulettePrize(this.m_parent.wheelData.category, base.gameObject);
		}
	}

	private void OnClickBack()
	{
		if (RouletteManager.IsRouletteEnabled() && base.isSpin && base.spinDecisionIndex == -1)
		{
			RouletteManager.RouletteClose();
		}
	}

	private void OnClickSpin()
	{
		if (base.isSpin || this.m_parent == null || base.wheel == null || base.isDelay)
		{
			return;
		}
		if (!GeneralUtil.IsNetwork())
		{
			GeneralUtil.ShowNoCommunication("SpinNoCommunication");
			return;
		}
		if (this.m_spinBtnActive || (RouletteUtility.isTutorial && this.m_parent.wheelData.category == RouletteCategory.PREMIUM))
		{
			if (this.m_backButtonImg != null)
			{
				this.m_backButtonImg.isEnabled = false;
			}
			int spinCostItemId = base.wheel.GetSpinCostItemId();
			int spinCostItemCost = base.wheel.GetSpinCostItemCost(spinCostItemId);
			this.SetCostItem(spinCostItemId, spinCostItemCost * -1 * base.wheel.multi);
			this.m_isJackpot = false;
			base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.Spin, 0f);
			this.m_parent.OnRouletteSpinStart(this.m_parent.wheelData, base.wheel.multi);
		}
		else
		{
			base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.SpinError, 0f);
			if (this.m_spinBtn != ServerWheelOptionsData.SPIN_BUTTON.FREE)
			{
				this.m_spinErrorWindow = this.m_parent.wheelData.ShowSpinErrorWindow();
			}
			else
			{
				this.m_spinErrorWindow = this.m_parent.wheelData.ShowSpinErrorWindow();
			}
		}
	}

	private void OnClickSpin0()
	{
		this.OnClickSpin();
	}

	private void OnClickSpin1()
	{
		this.OnClickSpin();
	}

	private void OnClickSpin2()
	{
		this.OnClickSpin();
	}

	private void OnClickSpin3()
	{
		this.OnClickSpin();
	}

	private void OnClickSpin4()
	{
		this.OnClickSpin();
	}

	private void OnClickSpin5()
	{
		this.OnClickSpin();
	}

	private void OnClickSpinCost(int index)
	{
		if (base.wheel != null && this.m_spinCostList != null && this.m_spinCostList.Count > 1 && (!RouletteUtility.isTutorial || this.m_parent.wheelData.category != RouletteCategory.PREMIUM))
		{
			int spinCostItemId = base.wheel.GetSpinCostItemId();
			if (spinCostItemId != this.m_spinCostList[index] && base.wheel.ChangeSpinCost(index + 1))
			{
				base.wheel.ChangeMulti(base.wheel.multi);
				base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.Click, 0f);
				this.SetButton();
			}
		}
	}

	private void OnClickSpinCost0()
	{
		this.OnClickSpinCost(0);
	}

	private void OnClickSpinCost1()
	{
		this.OnClickSpinCost(1);
	}

	private void OnClickSpinCost2()
	{
		this.OnClickSpinCost(2);
	}

	private void OnClickSpinCost3()
	{
		this.OnClickSpinCost(3);
	}

	private void onClickInfoButton()
	{
		if (!RouletteManager.Instance.isCurrentPrizeLoading)
		{
			if (this.m_attentionItemList == null)
			{
				this.m_attentionItemList = base.wheel.GetAttentionItemList();
			}
			if (this.m_attentionItemList != null)
			{
				EventBestChaoWindow window = EventBestChaoWindow.GetWindow();
				if (window != null)
				{
					window.OpenWindow(this.m_attentionItemList);
				}
			}
		}
	}

	private void OnClickBanner()
	{
		if (this.m_parent != null && this.m_parent.wheelData != null)
		{
			this.m_parent.OnClickCurrentRouletteBanner();
		}
		global::Debug.Log("OnClickBanner !");
	}

	private void OnClickSpinMulti0()
	{
		if (base.wheel.ChangeMulti(1))
		{
			this.SetButton();
			base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.Click, 0f);
		}
	}

	private void OnClickSpinMulti1()
	{
		if (base.wheel.ChangeMulti(3))
		{
			this.SetButton();
			base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.Click, 0f);
		}
	}

	private void OnClickSpinMulti2()
	{
		if (base.wheel.ChangeMulti(5))
		{
			this.SetButton();
			base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.Click, 0f);
		}
	}

	private void AnimationFinishCallback()
	{
		if (this.m_parent != null)
		{
			this.m_parent.OnRouletteWordAnimeEnd();
		}
	}

	private void FadeAnimationFinishCallback()
	{
		if (this.m_parent != null)
		{
			this.m_parent.OnRouletteSpinEnd();
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_fadeAnime, "ui_simple_load_outro_Anim2", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.FadeOut), true);
		}
	}

	private void FadeOut()
	{
	}

	private void RequestRoulettePrize_Succeeded(ServerPrizeState prize)
	{
		if (this.m_parent == null)
		{
			return;
		}
		this.m_parent.OpenOdds(prize, null);
	}

	private void RequestRoulettePrize_Failed()
	{
		global::Debug.Log("RequestRoulettePrize_Failed !!!");
	}

	public override void OnSpinStart()
	{
		this.m_frontColliderDelay = 5f;
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(true);
		}
		if (this.m_spinBtn == ServerWheelOptionsData.SPIN_BUTTON.FREE || this.m_spinBtn == ServerWheelOptionsData.SPIN_BUTTON.TICKET)
		{
			this.UpdateButtonCount(1);
		}
	}

	public override void OnSpinSkip()
	{
		if (this.m_frontCollider != null)
		{
			this.m_frontColliderDelay = 5f;
			this.m_frontCollider.SetActive(true);
		}
	}

	public override void OnSpinDecision()
	{
		if (this.m_frontCollider != null)
		{
			this.m_frontColliderDelay = 5f;
			this.m_frontCollider.SetActive(true);
		}
	}

	public override void OnSpinDecisionMulti()
	{
		if (this.m_fadeAnime != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_fadeAnime, "ui_simple_load_intro_Anim2", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.FadeAnimationFinishCallback), true);
			base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.Multi, 0f);
		}
		else
		{
			this.m_parent.OnRouletteSpinEnd();
		}
	}

	public override void OnSpinEnd()
	{
		global::Debug.Log("RouletteStandardPart OnSpinEnd !!!!!");
		if (this.m_frontCollider != null)
		{
			this.m_frontColliderDelay = 0.5f;
			this.m_frontCollider.SetActive(true);
		}
		if (this.m_spinBtn == ServerWheelOptionsData.SPIN_BUTTON.FREE && this.m_parent.wheelData.isRemainingRefresh)
		{
			this.UpdateButtonCount(0);
		}
		this.m_isJackpot = false;
		if (this.m_wordAnim != null && this.m_parent != null)
		{
			RouletteManager instance = RouletteManager.Instance;
			ServerWheelOptionsData wheelData = this.m_parent.wheelData;
			bool flag = true;
			if (wheelData != null && instance != null)
			{
				ServerSpinResultGeneral result = instance.GetResult();
				ServerChaoSpinResult resultChao = instance.GetResultChao();
				if (this.m_wordGet != null)
				{
					this.m_wordGet.SetActive(false);
				}
				if (this.m_wordJackpot != null)
				{
					this.m_wordJackpot.SetActive(false);
				}
				if (this.m_wordLavel != null)
				{
					this.m_wordLavel.SetActive(false);
				}
				if (this.m_wordRankup != null)
				{
					this.m_wordRankup.SetActive(false);
				}
				if (result != null)
				{
					if (result != null)
					{
						if (result.ItemWon >= 0)
						{
							int num = 0;
							ServerItem cellItem = wheelData.GetCellItem(result.ItemWon, out num);
							if (cellItem.idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
							{
								if (wheelData.GetRouletteRank() != RouletteUtility.WheelRank.Super)
								{
									if (this.m_wordRankup != null)
									{
										this.m_wordRankup.SetActive(true);
									}
									base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetRankup, 0.3f);
									this.m_frontColliderDelay = 0.125f;
								}
								else
								{
									if (this.m_wordJackpot != null)
									{
										this.m_wordJackpot.SetActive(true);
										this.m_isJackpot = true;
									}
									base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetJackpot, 0.3f);
									this.m_frontColliderDelay = 0.125f;
								}
							}
							else
							{
								if (this.m_wordGet != null)
								{
									this.m_wordGet.SetActive(true);
								}
								bool flag2 = false;
								if (cellItem.idType == ServerItem.IdType.CHARA)
								{
									flag2 = true;
								}
								else if (cellItem.idType == ServerItem.IdType.CHAO && cellItem.chaoId >= 1000)
								{
									flag2 = true;
								}
								if (flag2)
								{
									base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetRare, 0.3f);
								}
								else
								{
									base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetItem, 0.3f);
								}
							}
						}
						else
						{
							flag = false;
						}
					}
				}
				else if (wheelData.wheelType == RouletteUtility.WheelType.Normal)
				{
					if (this.m_wordGet != null)
					{
						this.m_wordGet.SetActive(true);
					}
					bool flag3 = false;
					if (resultChao != null)
					{
						Dictionary<int, ServerItemState>.KeyCollection keys = resultChao.ItemState.Keys;
						foreach (int current in keys)
						{
							ServerItem item = resultChao.ItemState[current].GetItem();
							if (item.idType == ServerItem.IdType.CHARA)
							{
								flag3 = true;
								break;
							}
							if (item.idType == ServerItem.IdType.CHAO && item.chaoId >= 1000)
							{
								flag3 = true;
								break;
							}
						}
					}
					if (flag3)
					{
						base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetRare, 0.3f);
					}
					else
					{
						base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetItem, 0.3f);
					}
				}
				else
				{
					global::Debug.Log("RouletteStandardPart OnSpinEnd error?");
					if (wheelData.itemWonData.idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
					{
						if (wheelData.GetRouletteRank() != RouletteUtility.WheelRank.Super)
						{
							if (this.m_wordRankup != null)
							{
								this.m_wordRankup.SetActive(true);
							}
							base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetRankup, 0.3f);
						}
						else
						{
							if (this.m_wordJackpot != null)
							{
								this.m_wordJackpot.SetActive(true);
								this.m_isJackpot = true;
							}
							base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetJackpot, 0.3f);
						}
					}
					else
					{
						if (this.m_wordGet != null)
						{
							this.m_wordGet.SetActive(true);
						}
						base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.GetItem, 0.3f);
					}
				}
			}
			this.m_wordAnim.Stop("ui_menu_roulette_word_Anim");
			if (flag)
			{
				if (this.m_isJackpot)
				{
					this.m_animeTime = 3f;
				}
				else
				{
					this.m_animeTime = 1.1f;
				}
				ActiveAnimation.Play(this.m_wordAnim, "ui_menu_roulette_word_Anim", Direction.Forward);
			}
			else
			{
				this.m_animeTime = 0f;
				this.AnimationFinishCallback();
			}
		}
	}

	public override void OnSpinError()
	{
		this.m_frontColliderDelay = 0f;
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(false);
		}
	}

	public override void windowClose()
	{
		base.windowClose();
		if (this.m_frontCollider != null && !this.m_frontCollider.activeSelf)
		{
			this.m_frontCollider.SetActive(true);
		}
		this.m_frontColliderDelay = 0.25f;
	}

	public override void PartsSendMessage(string mesage)
	{
		if (!string.IsNullOrEmpty(mesage) && mesage.IndexOf("CostItemUpdate") != -1)
		{
			this.SetEgg();
			this.SetCostItem(-1, 0);
			this.SetButton();
		}
	}
}
