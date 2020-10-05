using AnimationOrTween;
using DataTable;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class SpecialStageWindow : EventWindowBase
{
	private enum BUTTON_ACT
	{
		CLOSE,
		PLAY,
		INFO,
		RANK,
		ROULETTE,
		SHOP_RSRING,
		SHOP_RING,
		SHOP_CHALLENGE,
		NONE
	}

	private enum Mode
	{
		Idle,
		Wait,
		End
	}

	public const RankingUtil.RankingRankerType SPECILA_RANKING_RANKER_TYPE = RankingUtil.RankingRankerType.SP_ALL;

	public const int DRAW_RANKER_MAX = 10;

	private const string LBL_CAPTION = "Lbl_caption";

	private const string LBL_CAPTION_SH = "Lbl_caption_sh";

	private const string LBL_PAGE = "Lbl_page";

	private const string IMG_CHAO_BG = "img_chao_bg";

	private const string IMG_CHAO_TYPE = "img_type_icon";

	private const string IMG_CHAO_SPRITE = "sprite_chao";

	private const string TEX_CHAO = "Tex_chao";

	private const string GO_SNS = "facebook_top";

	private const string GO_PATTERN_0 = "pattern_0";

	private const string GO_PATTERN_1 = "pattern_1";

	private const string GO_PARTS = "ranking_cmn_parts";

	private const string GO_BTN_ALL = "Btn_all";

	private const string GO_BTN_FRIEND = "Btn_friend";

	private const string GO_BTN_BACK = "Btn_cmn_back";

	private const string GO_BTN_REWARD = "Btn_Reward";

	[Header("読み込み中表示"), SerializeField]
	private GameObject m_loading;

	[SerializeField]
	private UITexture m_bgTexture;

	private SpecialStageInfo m_spInfoData;

	private SpecialStageWindow.Mode m_mode;

	[SerializeField]
	private Animation m_animation;

	private static SocialInterface s_socialInterface;

	private bool m_close;

	private bool m_alertCollider;

	private SpecialStageWindow.BUTTON_ACT m_btnAct = SpecialStageWindow.BUTTON_ACT.NONE;

	private GameObject m_btnOption;

	private GameObject m_btnMore;

	private UIRectItemStorage m_myDataArea;

	private UIRectItemStorage m_topRankerArea;

	private UIRectItemStorageRanking m_listArea;

	private RankingUtil.RankingRankerType m_currentRankerType = RankingUtil.RankingRankerType.SP_ALL;

	private RankingUtil.RankingScoreType m_currentScoreType = RankingUtil.RankingScoreType.TOTAL_SCORE;

	private List<RankingUtil.Ranker> m_currentRankerList;

	private int m_page;

	private bool m_pageNext;

	private bool m_pagePrev;

	private float m_rankingInitloadingTime;

	private UIDraggablePanel m_mainListPanel;

	private bool m_facebookLock;

	private bool m_toggleLock;

	private bool m_opened;

	private EasySnsFeed m_easySnsFeed;

	private static SpecialStageWindow s_instance;

	public SpecialStageInfo infoData
	{
		get
		{
			return this.m_spInfoData;
		}
	}

	public static SpecialStageWindow Instance
	{
		get
		{
			return SpecialStageWindow.s_instance;
		}
	}

	public void SetLoadingObject()
	{
		if (this.m_loading != null)
		{
			this.m_loading.SetActive(true);
		}
	}

	public bool IsInitLoading()
	{
		return this.m_loading != null && this.m_loading.activeSelf;
	}

	public RankingUtil.Ranker GetCurrentRanker(int slot)
	{
		RankingUtil.Ranker result = null;
		if (this.m_currentRankerList != null && slot >= 0 && this.m_currentRankerList.Count > slot + 1)
		{
			result = this.m_currentRankerList[slot + 1];
		}
		return result;
	}

	protected override void SetObject()
	{
		if (this.m_isSetObject)
		{
			return;
		}
		base.gameObject.transform.localPosition = default(Vector3);
		if (this.m_animation == null)
		{
			this.m_animation = base.gameObject.GetComponent<Animation>();
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		list.Add("Lbl_caption");
		list.Add("Lbl_caption_sh");
		list.Add("Lbl_page");
		list2.Add("img_chao_bg");
		list2.Add("img_type_icon");
		list2.Add("sprite_chao");
		list3.Add("Tex_chao");
		list4.Add("facebook_top");
		list4.Add("pattern_0");
		list4.Add("pattern_1");
		list4.Add("ranking_cmn_parts");
		list4.Add("Btn_all");
		list4.Add("Btn_friend");
		list4.Add("Btn_cmn_back");
		list4.Add("Btn_Reward");
		this.m_objectLabels = ObjectUtility.GetObjectLabel(base.gameObject, list);
		this.m_objectSprites = ObjectUtility.GetObjectSprite(base.gameObject, list2);
		this.m_objectTextures = ObjectUtility.GetObjectTexture(base.gameObject, list3);
		this.m_objects = ObjectUtility.GetGameObject(base.gameObject, list4);
		if (this.m_objects != null)
		{
			if (this.m_objects.ContainsKey("pattern_0") && this.m_objects["pattern_0"] != null)
			{
				this.m_btnMore = GameObjectUtil.FindChildGameObject(this.m_objects["pattern_0"], "Btn_PageDown");
				this.m_myDataArea = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_objects["pattern_0"], "row_0");
				this.m_topRankerArea = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_objects["pattern_0"], "row_1");
			}
			if (this.m_objects.ContainsKey("pattern_1") && this.m_objects["pattern_1"] != null)
			{
				this.m_listArea = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorageRanking>(this.m_objects["pattern_1"], "slot");
				this.m_mainListPanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(this.m_objects["pattern_1"], "Panel_alpha_clip");
			}
			this.m_btnOption = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_option");
		}
		UIPlayAnimation uIPlayAnimation = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(base.gameObject, "blinder");
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "blinder");
		if (uIPlayAnimation != null && uIButtonMessage != null)
		{
			uIPlayAnimation.enabled = false;
			uIButtonMessage.enabled = false;
		}
		this.m_currentRankerType = RankingUtil.RankingRankerType.SP_ALL;
		this.m_currentScoreType = RankingManager.EndlessSpecialRankingScoreType;
		this.m_page = 0;
		this.m_isSetObject = true;
		this.m_facebookLock = true;
		if (RegionManager.Instance != null)
		{
			this.m_facebookLock = !RegionManager.Instance.IsUseSNS();
		}
		if (this.m_objects != null && this.m_objects.ContainsKey("Btn_friend") && this.m_objects["Btn_friend"] != null)
		{
			UIImageButton component = this.m_objects["Btn_friend"].GetComponent<UIImageButton>();
			if (component != null)
			{
				component.isEnabled = !this.m_facebookLock;
			}
		}
	}

	public void Setup(SpecialStageInfo info)
	{
		this.m_toggleLock = false;
		this.m_opened = false;
		this.SetAlertSimpleUI(true);
		this.SetObject();
		SpecialStageWindow.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		base.enabledAnchorObjects = true;
		this.m_mode = SpecialStageWindow.Mode.Idle;
		this.m_close = false;
		this.m_rankingInitloadingTime = 0f;
		Texture bGTexture = EventUtility.GetBGTexture();
		if (bGTexture != null && this.m_bgTexture != null)
		{
			this.m_bgTexture.mainTexture = bGTexture;
		}
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_eventmenu_intro", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			global::Debug.Log("Call hogehogehoge");
		}
		if (info != null)
		{
			this.m_spInfoData = info;
			this.m_objectLabels["Lbl_caption"].text = this.m_spInfoData.eventCaption;
			this.m_objectLabels["Lbl_caption_sh"].text = this.m_spInfoData.eventCaption;
			ChaoData rewardChao = this.m_spInfoData.GetRewardChao();
			if (rewardChao != null && this.m_objectSprites["img_chao_bg"] != null && this.m_objectSprites["img_type_icon"] != null)
			{
				switch (rewardChao.rarity)
				{
				case ChaoData.Rarity.NORMAL:
					this.m_objectSprites["img_chao_bg"].spriteName = "ui_tex_chao_bg_0";
					break;
				case ChaoData.Rarity.RARE:
					this.m_objectSprites["img_chao_bg"].spriteName = "ui_tex_chao_bg_1";
					break;
				case ChaoData.Rarity.SRARE:
					this.m_objectSprites["img_chao_bg"].spriteName = "ui_tex_chao_bg_2";
					break;
				default:
					this.m_objectSprites["img_chao_bg"].spriteName = "ui_tex_chao_bg_x";
					break;
				}
				switch (rewardChao.charaAtribute)
				{
				case CharacterAttribute.SPEED:
					this.m_objectSprites["img_type_icon"].spriteName = "ui_chao_set_type_icon_speed";
					break;
				case CharacterAttribute.FLY:
					this.m_objectSprites["img_type_icon"].spriteName = "ui_chao_set_type_icon_fly";
					break;
				case CharacterAttribute.POWER:
					this.m_objectSprites["img_type_icon"].spriteName = "ui_chao_set_type_icon_power";
					break;
				}
				if (this.m_objectTextures["Tex_chao"] != null)
				{
					ChaoTextureManager.CallbackInfo info2 = new ChaoTextureManager.CallbackInfo(this.m_objectTextures["Tex_chao"], null, true);
					ChaoTextureManager.Instance.GetTexture(rewardChao.id, info2);
				}
			}
			GeneralUtil.SetRouletteBtnIcon(base.gameObject, "Btn_roulette");
		}
		this.m_page = 0;
		this.m_currentRankerType = RankingUtil.RankingRankerType.SP_ALL;
		this.m_currentScoreType = RankingManager.EndlessSpecialRankingScoreType;
		global::Debug.Log("SpecialStageWindow  Setup!");
		this.ResetRankerList(this.m_page, this.m_currentRankerType);
		this.SetLoadingObject();
		RankingManager instance = SingletonGameObject<RankingManager>.Instance;
		if (instance != null && !instance.isLoading && instance.IsRankingTop(RankingUtil.RankingMode.ENDLESS, RankingManager.EndlessSpecialRankingScoreType, RankingUtil.RankingRankerType.SP_ALL))
		{
			this.SetRanking(RankingUtil.RankingRankerType.SP_ALL, RankingManager.EndlessSpecialRankingScoreType, -1);
		}
		BackKeyManager.AddWindowCallBack(base.gameObject);
		base.enabledAnchorObjects = true;
		this.m_mode = SpecialStageWindow.Mode.Wait;
		if (this.m_objects != null && this.m_objects.ContainsKey("Btn_all") && this.m_objects["Btn_all"] != null)
		{
			UIToggle uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject, "Btn_all");
			if (uIToggle != null && !uIToggle.value)
			{
				this.m_toggleLock = true;
				uIToggle.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
				this.m_toggleLock = false;
			}
		}
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		if (data != null && data.tex != null)
		{
			this.m_objectTextures["Tex_chao"].mainTexture = data.tex;
			this.m_objectTextures["Tex_chao"].alpha = 1f;
			if (this.m_objectSprites["sprite_chao"] != null)
			{
				this.m_objectSprites["sprite_chao"].alpha = 0f;
			}
		}
	}

	public bool SetRanking(RankingUtil.RankingRankerType type, RankingUtil.RankingScoreType score, int page)
	{
		if (page == -1 || this.m_currentRankerType != type || this.m_currentScoreType != score || this.m_page != page)
		{
			RankingManager instance = SingletonGameObject<RankingManager>.Instance;
			if (instance != null && !instance.isLoading)
			{
				if (page <= 0)
				{
					this.ResetRankerList(0, type);
					this.ResetRankerList(1, type);
				}
				this.m_page = page;
				this.m_currentRankerType = type;
				this.m_currentScoreType = score;
				if (this.m_page < 0)
				{
					this.m_page = 0;
				}
				if (SpecialStageWindow.s_socialInterface != null)
				{
					if (type == RankingUtil.RankingRankerType.SP_FRIEND && !SpecialStageWindow.s_socialInterface.IsLoggedIn)
					{
						if (this.m_objects.ContainsKey("facebook_top") && this.m_objects["facebook_top"] != null)
						{
							this.m_objects["facebook_top"].SetActive(true);
							this.ResetRankerList(0, type);
							return true;
						}
					}
					else if (type == RankingUtil.RankingRankerType.SP_FRIEND && SpecialStageWindow.s_socialInterface.IsLoggedIn)
					{
						if (this.m_objects.ContainsKey("facebook_top") && this.m_objects["facebook_top"] != null)
						{
							this.m_objects["facebook_top"].SetActive(false);
						}
						if (this.m_btnOption != null)
						{
							this.m_btnOption.SetActive(true);
						}
					}
					else
					{
						if (this.m_objects.ContainsKey("facebook_top") && this.m_objects["facebook_top"] != null)
						{
							this.m_objects["facebook_top"].SetActive(false);
						}
						if (this.m_btnOption != null)
						{
							this.m_btnOption.SetActive(false);
						}
					}
				}
				else if (type == RankingUtil.RankingRankerType.SP_FRIEND)
				{
					if (this.m_objects.ContainsKey("facebook_top") && this.m_objects["facebook_top"] != null)
					{
						this.m_objects["facebook_top"].SetActive(true);
						this.ResetRankerList(0, type);
						return true;
					}
				}
				else if (this.m_objects.ContainsKey("facebook_top") && this.m_objects["facebook_top"] != null)
				{
					this.m_objects["facebook_top"].SetActive(false);
				}
				if (this.m_btnMore != null)
				{
					this.m_btnMore.SetActive(false);
				}
				return instance.GetRanking(RankingUtil.RankingMode.ENDLESS, score, type, this.m_page, new RankingManager.CallbackRankingData(this.CallbackRanking));
			}
		}
		return true;
	}

	private void CallbackRanking(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		if (this.m_currentRankerType != type || this.m_currentScoreType != score)
		{
			return;
		}
		if (this.m_loading != null)
		{
			this.m_loading.SetActive(false);
		}
		this.m_pageNext = isNext;
		this.m_pagePrev = isPrev;
		this.SetRankerList(rankerList, type, page);
		if (this.m_mainListPanel != null && page <= 1)
		{
			if (type == RankingUtil.RankingRankerType.RIVAL)
			{
				GameObject myDataGameObject = this.m_listArea.GetMyDataGameObject();
				if (myDataGameObject != null)
				{
					float num = myDataGameObject.transform.localPosition.y * -1f - 367f;
					if (num < -1.25f)
					{
						num = -1.25f;
					}
					this.m_mainListPanel.transform.localPosition = new Vector3(this.m_mainListPanel.transform.localPosition.x, num, this.m_mainListPanel.transform.localPosition.z);
					this.m_mainListPanel.panel.clipRange = new Vector4(this.m_mainListPanel.panel.clipRange.x, -num, this.m_mainListPanel.panel.clipRange.z, this.m_mainListPanel.panel.clipRange.w);
					this.m_listArea.CheckItemDrawAll(true);
				}
				else
				{
					this.m_mainListPanel.Scroll(0f);
					this.m_mainListPanel.ResetPosition();
				}
			}
			else
			{
				this.m_mainListPanel.Scroll(0f);
				this.m_mainListPanel.ResetPosition();
			}
		}
		if (isNext && this.m_btnMore != null)
		{
			this.m_btnMore.SetActive(true);
		}
	}

	private void ResetRankerList(int page, RankingUtil.RankingRankerType type)
	{
		if (this.m_page > 1)
		{
			return;
		}
		if (page > 0 || type == RankingUtil.RankingRankerType.RIVAL)
		{
			if (this.m_objects.ContainsKey("pattern_0") && this.m_objects["pattern_0"] != null)
			{
				this.m_objects["pattern_0"].SetActive(false);
			}
			if (this.m_objects.ContainsKey("pattern_1") && this.m_objects["pattern_1"] != null)
			{
				this.m_objects["pattern_1"].SetActive(true);
			}
		}
		else
		{
			if (this.m_objects.ContainsKey("pattern_0") && this.m_objects["pattern_0"] != null)
			{
				this.m_objects["pattern_0"].SetActive(true);
			}
			if (this.m_objects.ContainsKey("pattern_1") && this.m_objects["pattern_1"] != null)
			{
				this.m_objects["pattern_1"].SetActive(false);
			}
		}
		if (this.m_listArea != null)
		{
			this.m_listArea.Reset();
		}
		if (this.m_myDataArea != null)
		{
			this.m_myDataArea.maxItemCount = (this.m_myDataArea.maxRows = 0);
			this.m_myDataArea.Restart();
		}
		if (this.m_topRankerArea != null)
		{
			this.m_topRankerArea.maxItemCount = (this.m_topRankerArea.maxRows = 0);
			this.m_topRankerArea.Restart();
		}
	}

	private void SetRankerList(List<RankingUtil.Ranker> rankers, RankingUtil.RankingRankerType type, int page)
	{
		if (page > 0 || type == RankingUtil.RankingRankerType.RIVAL)
		{
			this.m_currentRankerList = rankers;
		}
		if (page > 0 || type == RankingUtil.RankingRankerType.RIVAL)
		{
			if (this.m_objects.ContainsKey("pattern_0") && this.m_objects["pattern_0"] != null)
			{
				this.m_objects["pattern_0"].SetActive(false);
			}
			if (this.m_objects.ContainsKey("pattern_1") && this.m_objects["pattern_1"] != null)
			{
				this.m_objects["pattern_1"].SetActive(true);
			}
			if (this.m_listArea != null)
			{
				if (page < 1)
				{
					this.m_listArea.Reset();
					this.AddRectItemStorageRanking(this.m_listArea, rankers, type);
				}
				else
				{
					if (page == 1)
					{
						this.m_listArea.Reset();
					}
					this.AddRectItemStorageRanking(this.m_listArea, rankers, type);
				}
			}
			if (this.m_myDataArea != null)
			{
				this.m_myDataArea.maxItemCount = (this.m_myDataArea.maxRows = 0);
				this.m_myDataArea.Restart();
			}
			if (this.m_topRankerArea != null)
			{
				this.m_topRankerArea.maxItemCount = (this.m_topRankerArea.maxRows = 0);
				this.m_topRankerArea.Restart();
			}
		}
		else
		{
			if (this.m_objects.ContainsKey("pattern_0") && this.m_objects["pattern_0"] != null)
			{
				this.m_objects["pattern_0"].SetActive(true);
			}
			if (this.m_objects.ContainsKey("pattern_1") && this.m_objects["pattern_1"] != null)
			{
				this.m_objects["pattern_1"].SetActive(false);
			}
			if (this.m_listArea != null)
			{
				this.m_listArea.Reset();
			}
			if (this.m_myDataArea != null)
			{
				if (rankers != null && rankers.Count > 0)
				{
					if (rankers[0] != null)
					{
						this.m_myDataArea.maxItemCount = (this.m_myDataArea.maxRows = 1);
						this.UpdateRectItemStorage(this.m_myDataArea, rankers, 0);
					}
					else
					{
						this.m_myDataArea.maxItemCount = (this.m_myDataArea.maxRows = 0);
						this.m_myDataArea.Restart();
					}
				}
				else
				{
					this.m_myDataArea.maxItemCount = (this.m_myDataArea.maxRows = 0);
					this.m_myDataArea.Restart();
				}
			}
			if (this.m_topRankerArea != null && rankers != null)
			{
				if (rankers.Count - 1 >= RankingManager.GetRankingMax(type, page))
				{
					this.m_topRankerArea.maxItemCount = (this.m_topRankerArea.maxRows = 3);
				}
				else
				{
					this.m_topRankerArea.maxItemCount = (this.m_topRankerArea.maxRows = rankers.Count - 1);
				}
				this.UpdateRectItemStorage(this.m_topRankerArea, rankers, 1);
			}
		}
	}

	private void AddRectItemStorageRanking(UIRectItemStorageRanking ui_rankers, List<RankingUtil.Ranker> rankerList, RankingUtil.RankingRankerType type)
	{
		int childCount = ui_rankers.childCount;
		int num = rankerList.Count - childCount;
		if (this.m_pageNext)
		{
			num--;
		}
		ui_rankers.rankingType = type;
		if (ui_rankers.callback == null)
		{
			ui_rankers.callback = new UIRectItemStorageRanking.CallbackCreated(this.CallbackItemStorageRanking);
			ui_rankers.callbackTopOrLast = new UIRectItemStorageRanking.CallbackTopOrLast(this.CallbackItemStorageRankingTopOrLast);
		}
		ui_rankers.AddItem(num, 0.02f);
	}

	private bool CallbackItemStorageRankingTopOrLast(bool isTop)
	{
		bool result = false;
		RankingManager instance = SingletonGameObject<RankingManager>.Instance;
		if (instance != null && !instance.isLoading)
		{
			if (isTop)
			{
				if (this.m_pagePrev)
				{
					result = false;
				}
			}
			else if (this.m_pageNext)
			{
				result = instance.GetRankingScroll(RankingUtil.RankingMode.ENDLESS, true, new RankingManager.CallbackRankingData(this.CallbackRanking));
			}
		}
		return result;
	}

	private void CallbackItemStorageRanking(ui_ranking_scroll_dummy obj, UIRectItemStorageRanking storage)
	{
		if (obj != null && this.m_currentRankerList != null)
		{
			int num = obj.slot + 1;
			if (num > 0 && this.m_currentRankerList.Count > num)
			{
				RankingUtil.Ranker rankerData = this.m_currentRankerList[num];
				if (obj.myRankerData == null)
				{
					obj.myRankerData = this.m_currentRankerList[0];
				}
				obj.spWindow = this;
				obj.rankerData = rankerData;
				obj.rankerType = this.m_currentRankerType;
				obj.scoreType = this.m_currentScoreType;
				obj.SetActiveObject(storage.CheckItemDraw(obj.slot), 0f);
				obj.end = (obj.slot + 1 == this.m_currentRankerList.Count);
			}
			else
			{
				UnityEngine.Object.Destroy(obj.gameObject);
			}
		}
	}

	private void UpdateRectItemStorage(UIRectItemStorage ui_rankers, List<RankingUtil.Ranker> rankerList, int head = 1)
	{
		ui_rankers.Restart();
		ui_ranking_scroll[] componentsInChildren = ui_rankers.GetComponentsInChildren<ui_ranking_scroll>(true);
		for (int i = 0; i < ui_rankers.maxItemCount; i++)
		{
			if (i + head >= rankerList.Count)
			{
				break;
			}
			RankingUtil.Ranker ranker = rankerList[i + head];
			if (ranker != null)
			{
				componentsInChildren[i].UpdateView(this.m_currentScoreType, this.m_currentRankerType, ranker, i == ui_rankers.maxItemCount - 1);
				bool myRanker = false;
				if (rankerList[0] != null && ranker.id == rankerList[0].id)
				{
					myRanker = true;
				}
				componentsInChildren[i].SetMyRanker(myRanker);
			}
		}
	}

	public bool IsEnd()
	{
		return this.m_mode != SpecialStageWindow.Mode.Wait;
	}

	private void Update()
	{
		if (this.m_easySnsFeed != null)
		{
			EasySnsFeed.Result result = this.m_easySnsFeed.Update();
			if (result != EasySnsFeed.Result.COMPLETED)
			{
				if (result == EasySnsFeed.Result.FAILED)
				{
					this.m_easySnsFeed = null;
				}
			}
			else
			{
				this.m_easySnsFeed = null;
				this.m_currentRankerList = null;
				this.ResetRankerList(0, this.m_currentRankerType);
				global::Debug.Log("SetRanking m_easySnsFeed sp!");
				this.SetRanking(RankingUtil.RankingRankerType.SP_FRIEND, RankingManager.EndlessSpecialRankingScoreType, -1);
			}
		}
		if (base.enabledAnchorObjects)
		{
			if (this.m_loading != null && this.m_loading.activeSelf)
			{
				this.m_rankingInitloadingTime += Time.deltaTime;
				if (this.m_rankingInitloadingTime > 5f)
				{
					if (SingletonGameObject<RankingManager>.Instance != null)
					{
						SingletonGameObject<RankingManager>.Instance.Init(null, null);
					}
					this.m_rankingInitloadingTime = -20f;
				}
			}
		}
		else
		{
			this.m_rankingInitloadingTime = 0f;
		}
	}

	public void OnClickNoButton()
	{
		this.SetAlertSimpleUI(true);
		this.m_btnAct = SpecialStageWindow.BUTTON_ACT.CLOSE;
		this.m_close = true;
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_eventmenu_outro", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	public void OnClickNoBgButton()
	{
		this.SetAlertSimpleUI(true);
		this.m_btnAct = SpecialStageWindow.BUTTON_ACT.CLOSE;
		this.m_close = true;
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_eventmenu_outro", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	public void OnClickPlayButton()
	{
		this.SetAlertSimpleUI(true);
		this.m_btnAct = SpecialStageWindow.BUTTON_ACT.PLAY;
		this.m_close = true;
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	public void OnClickInfoButton()
	{
		this.m_btnAct = SpecialStageWindow.BUTTON_ACT.INFO;
		EventRewardWindow.Create(this.m_spInfoData);
	}

	public void OnClickRouletteButton()
	{
		this.SetAlertSimpleUI(true);
		this.m_btnAct = SpecialStageWindow.BUTTON_ACT.ROULETTE;
		this.m_close = true;
		SoundManager.SePlay("sys_menu_decide", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_roulette");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	public void OnClickSnsLogin()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/SpecialStageWindowUI/Anchor_5_MC");
	}

	public void OnClickAllToggle()
	{
		if (!this.m_toggleLock && this.m_currentRankerType != RankingUtil.RankingRankerType.SP_ALL && this.m_loading != null && !this.m_loading.activeSelf)
		{
			this.m_currentRankerList = null;
			this.ResetRankerList(0, this.m_currentRankerType);
			if (!this.SetRanking(RankingUtil.RankingRankerType.SP_ALL, RankingManager.EndlessSpecialRankingScoreType, 0) && this.m_loading != null)
			{
				this.m_loading.SetActive(true);
			}
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	public void OnClickFriendToggle()
	{
		if (!this.m_toggleLock && this.m_currentRankerType != RankingUtil.RankingRankerType.SP_FRIEND && this.m_loading != null && !this.m_loading.activeSelf)
		{
			this.m_currentRankerList = null;
			this.ResetRankerList(0, this.m_currentRankerType);
			if (!this.SetRanking(RankingUtil.RankingRankerType.SP_FRIEND, RankingManager.EndlessSpecialRankingScoreType, 0) && this.m_loading != null)
			{
				this.m_loading.SetActive(true);
			}
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	public void OnClickReward()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
		if (window != null && this.m_spInfoData != null && this.m_spInfoData.GetRewardChao() != null && this.m_spInfoData.GetRewardChao().id >= 0)
		{
			ChaoData chaoData = ChaoTable.GetChaoData(this.m_spInfoData.GetRewardChao().id);
			if (chaoData != null)
			{
				ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(chaoData);
				chaoInfo.level = ChaoTable.ChaoMaxLevel();
				chaoInfo.detail = chaoData.GetDetailsLevel(chaoInfo.level);
				if (chaoInfo.level == ChaoTable.ChaoMaxLevel())
				{
					chaoInfo.detail = chaoInfo.detail + "\n" + TextUtility.GetChaoText("Chao", "level_max");
				}
				window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
			}
		}
	}

	public void OnClickMoreButton()
	{
		RankingManager instance = SingletonGameObject<RankingManager>.Instance;
		if (instance != null && !instance.isLoading)
		{
			this.m_mainListPanel.transform.localPosition = new Vector3(this.m_mainListPanel.transform.localPosition.x, 0f, this.m_mainListPanel.transform.localPosition.z);
			this.m_mainListPanel.Scroll(0f);
			this.m_mainListPanel.ResetPosition();
			this.ResetRankerList(1, this.m_currentRankerType);
			this.SetRanking(this.m_currentRankerType, this.m_currentScoreType, 1);
		}
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickFriendOption()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		GameObject loadMenuChildObject = HudMenuUtility.GetLoadMenuChildObject("RankingFriendOptionWindow", true);
		if (loadMenuChildObject != null)
		{
			RankingFriendOptionWindow component = loadMenuChildObject.GetComponent<RankingFriendOptionWindow>();
			if (component != null)
			{
				component.StartCoroutine("SetUp");
			}
		}
	}

	private void OnClickFriendOptionOk()
	{
		if (SingletonGameObject<RankingManager>.Instance != null && EventManager.Instance != null && EventManager.Instance.Type == EventManager.EventType.SPECIAL_STAGE)
		{
			SingletonGameObject<RankingManager>.Instance.Reset(RankingUtil.RankingMode.ENDLESS, RankingUtil.RankingRankerType.SP_FRIEND);
			this.SetRanking(RankingUtil.RankingRankerType.SP_FRIEND, RankingManager.EndlessSpecialRankingScoreType, -1);
		}
	}

	public void OnClickEndButton(ButtonInfoTable.ButtonType btnType)
	{
		this.SetAlertSimpleUI(true);
		switch (btnType)
		{
		case ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP:
			this.m_btnAct = SpecialStageWindow.BUTTON_ACT.SHOP_RSRING;
			goto IL_7B;
		case ButtonInfoTable.ButtonType.RING_TO_SHOP:
			this.m_btnAct = SpecialStageWindow.BUTTON_ACT.SHOP_RING;
			goto IL_7B;
		case ButtonInfoTable.ButtonType.CHALLENGE_TO_SHOP:
			this.m_btnAct = SpecialStageWindow.BUTTON_ACT.SHOP_CHALLENGE;
			goto IL_7B;
		case ButtonInfoTable.ButtonType.RAIDENERGY_TO_SHOP:
		case ButtonInfoTable.ButtonType.EVENT_RAID:
		case ButtonInfoTable.ButtonType.EVENT_SPECIAL:
		case ButtonInfoTable.ButtonType.EVENT_COLLECT:
			IL_32:
			if (btnType != ButtonInfoTable.ButtonType.REWARDLIST_TO_CHAO_ROULETTE)
			{
				goto IL_7B;
			}
			this.m_btnAct = SpecialStageWindow.BUTTON_ACT.ROULETTE;
			goto IL_7B;
		case ButtonInfoTable.ButtonType.EVENT_BACK:
			this.m_btnAct = SpecialStageWindow.BUTTON_ACT.CLOSE;
			goto IL_7B;
		}
		goto IL_32;
		IL_7B:
		this.m_close = true;
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_eventmenu_outro", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	public void WindowAnimationFinishCallbackByPlay()
	{
		HudMenuUtility.SendVirtualNewItemSelectClicked(HudMenuUtility.ITEM_SELECT_MODE.EVENT_STAGE);
		base.enabledAnchorObjects = false;
		this.SetAlertSimpleUI(false);
		this.m_opened = false;
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_close)
		{
			this.m_mode = SpecialStageWindow.Mode.End;
			this.m_toggleLock = false;
			switch (this.m_btnAct)
			{
			case SpecialStageWindow.BUTTON_ACT.PLAY:
				HudMenuUtility.SendVirtualNewItemSelectClicked(HudMenuUtility.ITEM_SELECT_MODE.EVENT_STAGE);
				base.enabledAnchorObjects = false;
				goto IL_CD;
			case SpecialStageWindow.BUTTON_ACT.INFO:
				EventRewardWindow.Create(this.m_spInfoData);
				goto IL_CD;
			case SpecialStageWindow.BUTTON_ACT.ROULETTE:
				base.enabledAnchorObjects = false;
				HudMenuUtility.SendChaoRouletteButtonClicked();
				goto IL_CD;
			case SpecialStageWindow.BUTTON_ACT.SHOP_RSRING:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP, false);
				base.enabledAnchorObjects = false;
				goto IL_CD;
			case SpecialStageWindow.BUTTON_ACT.SHOP_RING:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.RING_TO_SHOP, false);
				base.enabledAnchorObjects = false;
				goto IL_CD;
			case SpecialStageWindow.BUTTON_ACT.SHOP_CHALLENGE:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHALLENGE_TO_SHOP, false);
				base.enabledAnchorObjects = false;
				goto IL_CD;
			}
			HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.EVENT_BACK, false);
			base.enabledAnchorObjects = false;
			IL_CD:
			this.SetAlertSimpleUI(false);
			BackKeyManager.RemoveWindowCallBack(base.gameObject);
			this.m_opened = false;
		}
		else
		{
			this.m_opened = true;
			this.SetAlertSimpleUI(false);
		}
	}

	private void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null && this.m_alertCollider)
		{
			msg.StaySequence();
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

	public static SpecialStageWindow Create(SpecialStageInfo info)
	{
		if (!(SpecialStageWindow.s_instance != null))
		{
			return null;
		}
		if (SpecialStageWindow.s_instance.gameObject.transform.parent != null && SpecialStageWindow.s_instance.gameObject.transform.parent.name != "Camera")
		{
			return null;
		}
		SpecialStageWindow.s_instance.gameObject.SetActive(true);
		SpecialStageWindow.s_instance.Setup(info);
		RankingManager instance = SingletonGameObject<RankingManager>.Instance;
		if (instance != null && !instance.isLoading && !instance.IsRankingTop(RankingUtil.RankingMode.ENDLESS, RankingManager.EndlessSpecialRankingScoreType, RankingUtil.RankingRankerType.SP_ALL))
		{
			SpecialStageWindow.RankingSetup(true);
		}
		return SpecialStageWindow.s_instance;
	}

	public static SpecialStageWindow RankingSetup(bool createRankingUsetList = false)
	{
		if (SpecialStageWindow.s_instance != null)
		{
			if (SpecialStageWindow.s_instance.enabledAnchorObjects)
			{
				if (createRankingUsetList)
				{
					SpecialStageWindow.s_instance.SetRanking(RankingUtil.RankingRankerType.SP_ALL, RankingManager.EndlessSpecialRankingScoreType, -1);
				}
				else
				{
					SpecialStageWindow.s_instance.SetLoadingObject();
				}
			}
			return SpecialStageWindow.s_instance;
		}
		return null;
	}

	public static bool IsOpend()
	{
		return SpecialStageWindow.s_instance != null && SpecialStageWindow.s_instance.m_opened;
	}

	public static void SetLoading()
	{
		if (SpecialStageWindow.s_instance != null)
		{
			SpecialStageWindow.s_instance.SetLoadingObject();
		}
	}

	public void UpdateSendChallengeOrg(RankingUtil.RankingRankerType type, string id)
	{
		if (this.m_currentRankerType == type && this.m_objects != null && this.m_objects.Count > 0)
		{
			if (this.m_objects.ContainsKey("pattern_0") && this.m_objects["pattern_0"] != null && this.m_objects["pattern_0"].activeSelf && this.m_topRankerArea != null)
			{
				ui_ranking_scroll[] componentsInChildren = this.m_topRankerArea.GetComponentsInChildren<ui_ranking_scroll>();
				if (componentsInChildren != null && componentsInChildren.Length > 0)
				{
					ui_ranking_scroll[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						ui_ranking_scroll ui_ranking_scroll = array[i];
						ui_ranking_scroll.UpdateSendChallenge(id);
					}
				}
			}
			else if (this.m_objects.ContainsKey("pattern_1") && this.m_objects["pattern_0"] != null && this.m_objects["pattern_1"].activeSelf && this.m_listArea != null)
			{
				ui_ranking_scroll[] componentsInChildren2 = this.m_listArea.GetComponentsInChildren<ui_ranking_scroll>();
				if (componentsInChildren2 != null && componentsInChildren2.Length > 0)
				{
					ui_ranking_scroll[] array2 = componentsInChildren2;
					for (int j = 0; j < array2.Length; j++)
					{
						ui_ranking_scroll ui_ranking_scroll2 = array2[j];
						ui_ranking_scroll2.UpdateSendChallenge(id);
					}
				}
			}
		}
	}

	public static void UpdateSendChallenge(RankingUtil.RankingRankerType type, string id)
	{
		if (SpecialStageWindow.s_instance != null)
		{
			SpecialStageWindow.s_instance.UpdateSendChallengeOrg(type, id);
		}
	}

	private void Awake()
	{
		this.SetInstance();
		base.enabledAnchorObjects = false;
	}

	private void OnDestroy()
	{
		if (SpecialStageWindow.s_instance == this)
		{
			SpecialStageWindow.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (SpecialStageWindow.s_instance == null)
		{
			SpecialStageWindow.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
