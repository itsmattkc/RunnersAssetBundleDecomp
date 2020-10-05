using AnimationOrTween;
using System;
using Text;
using UnityEngine;

public abstract class GameResultScores : MonoBehaviour
{
	protected enum Category
	{
		CHAO,
		CAMPAIGN,
		CHARA,
		NUM,
		NONE
	}

	private enum EventSignal
	{
		PLAY_START = 100,
		SKIP,
		ALL_SKIP
	}

	private enum EventUpdateState
	{
		Idle,
		Start,
		Wait
	}

	private bool m_debugInfo;

	protected StageScoreManager m_scoreManager;

	protected GameObject m_resultRoot;

	private ResultObjParam[] ResultObjParamTable = new ResultObjParam[]
	{
		new ResultObjParam("Lbl_score", "Lbl_chao_score", "img_chao_score", "Lbl_player_score", "img_player_score", string.Empty, "Lbl_chaototal_score", "Lbl_player_rank_score1"),
		new ResultObjParam("Lbl_ring", "Lbl_chao_ring", "img_chao_ring", "Lbl_player_ring", "img_player_ring", "Lbl_campaign_ring", string.Empty, string.Empty),
		new ResultObjParam("Lbl_rsring", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
		new ResultObjParam("Lbl_animal", "Lbl_chao_animal", "img_chao_animal", "Lbl_player_animal", "img_player_animal", string.Empty, string.Empty, string.Empty),
		new ResultObjParam("Lbl_distance", "Lbl_chao_distance", "img_chao_distance", "Lbl_player_distance", "img_player_distance", string.Empty, string.Empty, string.Empty),
		new ResultObjParam("Lbl_totalscore", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
		new ResultObjParam(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
	};

	private string[] BonusCategoryData = new string[]
	{
		"chao_bonus",
		"campaign_bonus",
		"player_bonus"
	};

	private string[] BonusCategoryButtonLabel = new string[]
	{
		"ui_Lbl_word_chao_bonus",
		"ui_Lbl_word_campaign_bonus",
		"ui_Lbl_word_player_bonus",
		"ui_Lbl_word_bonus_details_button"
	};

	private TinyFsmBehavior m_fsm;

	private GameResultScores.Category m_category;

	private GameResultScoreInterporate[] m_scores = new GameResultScoreInterporate[7];

	private BonusEventScore[] m_bonusEventScores = new BonusEventScore[3];

	private BonusEventScore m_chaoCountBonusEventScores = new BonusEventScore();

	private BonusEventScore m_RankBonusEventScores = new BonusEventScore();

	private BonusEventInfo[] m_bonusEventInfos = new BonusEventInfo[3];

	private Animation m_bonusEventAnim;

	private GameObject m_eventRoot;

	private GameObject m_resultObj;

	private bool m_finished;

	private bool m_skip;

	private bool m_allSkip;

	protected bool m_isReplay;

	private float m_timer;

	private GameResultScores.EventUpdateState m_eventUpdateState;

	private GameResultScores.Category m_addScore;

	private UILabel m_DetailButtonLabel;

	private UILabel m_DetailButtonLabel_Sh;

	protected bool m_isBossResult;

	private bool m_isQuickMode;

	public bool IsEnd
	{
		get
		{
			return this.m_finished;
		}
	}

	private void Start()
	{
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm == null)
		{
			return;
		}
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
		this.m_finished = false;
		this.m_isReplay = false;
	}

	public void Setup(GameObject resultObj, GameObject resultRoot, GameObject eventRoot)
	{
		this.DebugScoreLog();
		this.m_finished = false;
		this.m_isBossResult = false;
		if (resultObj == null)
		{
			return;
		}
		this.m_resultObj = resultObj;
		this.m_scoreManager = StageScoreManager.Instance;
		if (this.m_scoreManager == null)
		{
			return;
		}
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		StageAbilityManager instance2 = StageAbilityManager.Instance;
		if (instance2 == null)
		{
			return;
		}
		if (resultRoot == null)
		{
			return;
		}
		this.m_resultRoot = resultRoot;
		if (eventRoot == null)
		{
			return;
		}
		this.m_eventRoot = eventRoot;
		if (StageModeManager.Instance != null)
		{
			this.m_isQuickMode = StageModeManager.Instance.IsQuickMode();
		}
		this.m_bonusEventAnim = this.m_eventRoot.GetComponent<Animation>();
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_resultRoot, "window_result");
		if (gameObject == null)
		{
			return;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_resultRoot, "window_bonus");
		if (gameObject2 == null)
		{
			return;
		}
		gameObject2.SetActive(true);
		this.m_DetailButtonLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_resultRoot, "Lbl_word_bonus_details");
		this.m_DetailButtonLabel_Sh = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_resultRoot, "Lbl_word_bonus_details_sh");
		for (int i = 0; i < 3; i++)
		{
			this.m_bonusEventScores[i] = new BonusEventScore();
			this.m_bonusEventScores[i].obj = GameObjectUtil.FindChildGameObject(gameObject2, this.BonusCategoryData[i]);
			if (this.m_bonusEventScores[i].obj != null)
			{
				this.m_bonusEventScores[i].obj.SetActive(false);
			}
		}
		this.m_chaoCountBonusEventScores.obj = GameObjectUtil.FindChildGameObject(gameObject2, "chaototal_bonus");
		if (this.m_chaoCountBonusEventScores.obj != null)
		{
			this.m_chaoCountBonusEventScores.obj.SetActive(false);
		}
		this.m_RankBonusEventScores.obj = GameObjectUtil.FindChildGameObject(gameObject2, "player_bonus");
		if (this.m_RankBonusEventScores.obj != null)
		{
			this.m_RankBonusEventScores.obj.SetActive(false);
		}
		for (int j = 0; j < 7; j++)
		{
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, this.ResultObjParamTable[j].scoreLabel);
			if (uILabel != null)
			{
				this.m_scores[j] = new GameResultScoreInterporate();
				this.m_scores[j].Setup(uILabel);
				this.m_scores[j].SetLabelStartValue(0L);
			}
			int num = 0;
			GameObject obj = this.m_bonusEventScores[num].obj;
			if (obj != null)
			{
				this.m_bonusEventScores[num].bonusData[j] = new BonusData();
				this.m_bonusEventScores[num].bonusData[j].labelScore1 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(obj, this.ResultObjParamTable[j].chaoBonusLabel + "1");
				this.m_bonusEventScores[num].bonusData[j].uiTexture1 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(obj, this.ResultObjParamTable[j].chaoBonusTexture + "1");
				this.m_bonusEventScores[num].bonusData[j].labelScore2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(obj, this.ResultObjParamTable[j].chaoBonusLabel + "2");
				this.m_bonusEventScores[num].bonusData[j].uiTexture2 = GameObjectUtil.FindChildGameObjectComponent<UITexture>(obj, this.ResultObjParamTable[j].chaoBonusTexture + "2");
				int num2 = (!this.IsBonusRate(instance2.MainChaoBonusValueRate, (ScoreDataType)j)) ? (-1) : instance.PlayerData.MainChaoID;
				if (num2 >= 0)
				{
					this.SetActiveBonusEventScore1(num, j, true);
					HudUtility.SetChaoTexture(this.m_bonusEventScores[num].bonusData[j].uiTexture1, num2, true);
				}
				else
				{
					this.SetActiveBonusEventScore1(num, j, false);
				}
				int num3 = (!this.IsBonusRate(instance2.SubChaoBonusValueRate, (ScoreDataType)j)) ? (-1) : instance.PlayerData.SubChaoID;
				if (num3 >= 0)
				{
					this.SetActiveBonusEventScore2(num, j, true);
					HudUtility.SetChaoTexture(this.m_bonusEventScores[num].bonusData[j].uiTexture2, num3, true);
				}
				else
				{
					this.SetActiveBonusEventScore2(num, j, false);
				}
			}
			GameObject obj2 = this.m_chaoCountBonusEventScores.obj;
			if (obj2 != null)
			{
				this.m_chaoCountBonusEventScores.bonusData[j] = new BonusData();
				this.m_chaoCountBonusEventScores.bonusData[j].labelScore1 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(obj2, this.ResultObjParamTable[j].chaoCountBonusLabel);
				this.SetActiveBonusEventScore(this.m_chaoCountBonusEventScores.bonusData[j].labelScore1, this.IsBonusRate(instance2.CountChaoBonusValueRate, (ScoreDataType)j));
			}
			int num4 = 1;
			GameObject obj3 = this.m_bonusEventScores[num4].obj;
			if (obj3 != null)
			{
				this.m_bonusEventScores[num4].bonusData[j] = new BonusData();
				this.m_bonusEventScores[num4].bonusData[j].labelScore1 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(obj3, this.ResultObjParamTable[j].campaignBonusLabel);
				this.SetActiveBonusEventScore(this.m_bonusEventScores[num4].bonusData[j].labelScore1, this.IsBonusRate(GameResultUtility.GetCampaignBonusRate(instance2), (ScoreDataType)j));
			}
			int num5 = 2;
			GameObject obj4 = this.m_bonusEventScores[num5].obj;
			if (obj4 != null)
			{
				this.m_bonusEventScores[num5].bonusData[j] = new BonusData();
				this.m_bonusEventScores[num5].bonusData[j].labelScore1 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(obj4, this.ResultObjParamTable[j].charaBonusLabel + "1");
				this.m_bonusEventScores[num5].bonusData[j].uiSprite1 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(obj4, this.ResultObjParamTable[j].charaBonusTexture + "1");
				this.m_bonusEventScores[num5].bonusData[j].labelScore2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(obj4, this.ResultObjParamTable[j].charaBonusLabel + "2");
				this.m_bonusEventScores[num5].bonusData[j].uiSprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(obj4, this.ResultObjParamTable[j].charaBonusTexture + "2");
				CharaType charaType = (!this.IsBonusRate(instance2.MainCharaBonusValueRate, (ScoreDataType)j)) ? CharaType.UNKNOWN : instance.PlayerData.MainChara;
				if (charaType != CharaType.UNKNOWN)
				{
					this.SetActiveBonusEventScore1(num5, j, true);
					GameResultUtility.SetCharaTexture(this.m_bonusEventScores[num5].bonusData[j].uiSprite1, charaType);
				}
				else
				{
					this.SetActiveBonusEventScore1(num5, j, false);
				}
				CharaType charaType2 = (!this.IsBonusRate(instance2.SubCharaBonusValueRate, (ScoreDataType)j)) ? CharaType.UNKNOWN : instance.PlayerData.SubChara;
				if (charaType2 != CharaType.UNKNOWN)
				{
					this.SetActiveBonusEventScore2(num5, j, true);
					GameResultUtility.SetCharaTexture(this.m_bonusEventScores[num5].bonusData[j].uiSprite2, charaType2);
				}
				else
				{
					this.SetActiveBonusEventScore2(num5, j, false);
				}
				GameObject obj5 = this.m_RankBonusEventScores.obj;
				if (obj5 != null)
				{
					this.m_RankBonusEventScores.bonusData[j] = new BonusData();
					this.m_RankBonusEventScores.bonusData[j].labelScore1 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(obj5, this.ResultObjParamTable[j].charaBonusRankScore);
					this.SetActiveBonusEventScore(this.m_RankBonusEventScores.bonusData[j].labelScore1, true);
					if (this.m_isQuickMode && this.m_RankBonusEventScores.bonusData[j].labelScore1 != null)
					{
						this.m_RankBonusEventScores.bonusData[j].labelScore1.gameObject.SetActive(false);
						GameObject gameObject3 = GameObjectUtil.FindChildGameObject(obj5, "player_rank_bonus_title");
						if (gameObject3 != null)
						{
							gameObject3.SetActive(false);
						}
					}
				}
			}
		}
		this.SetResultScore(this.m_scoreManager.CountData);
		for (int k = 0; k < 3; k++)
		{
			this.m_bonusEventInfos[k] = new BonusEventInfo();
		}
		if (this.m_bonusEventAnim != null)
		{
			int num6 = 0;
			this.m_bonusEventInfos[num6].obj = GameObjectUtil.FindChildGameObject(this.m_eventRoot, "chaobonus");
			this.m_bonusEventInfos[num6].animClipName = "ui_result_word_chaobonus_Anim";
			this.m_bonusEventInfos[num6].viewTime = this.m_bonusEventAnim[this.m_bonusEventInfos[num6].animClipName].length * 0.3f;
			this.m_bonusEventInfos[num6].valueText = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_bonusEventInfos[num6].obj, "Lbl_num_chaobonus_score");
			StageScoreManager.ResultData bonusCountMainChaoData = this.m_scoreManager.BonusCountMainChaoData;
			StageScoreManager.ResultData bonusCountSubChaoData = this.m_scoreManager.BonusCountSubChaoData;
			StageScoreManager.ResultData bonusCountChaoCountData = this.m_scoreManager.BonusCountChaoCountData;
			if (this.IsBonus(bonusCountMainChaoData, bonusCountSubChaoData, bonusCountChaoCountData))
			{
				this.SetupBonus(bonusCountMainChaoData, bonusCountSubChaoData, ref this.m_bonusEventScores[num6].bonusData);
				this.SetupBonus(bonusCountChaoCountData, null, ref this.m_chaoCountBonusEventScores.bonusData);
			}
			else
			{
				this.m_bonusEventInfos[num6].obj = null;
			}
			int num7 = 1;
			this.m_bonusEventInfos[num7].obj = GameObjectUtil.FindChildGameObject(this.m_eventRoot, "campaignbonus");
			this.m_bonusEventInfos[num7].animClipName = "ui_result_word_campaignbonus_Anim";
			this.m_bonusEventInfos[num7].viewTime = this.m_bonusEventAnim[this.m_bonusEventInfos[num7].animClipName].length * 0.3f;
			this.m_bonusEventInfos[num7].valueText = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_bonusEventInfos[num7].obj, "Lbl_num_campaignbonus_score");
			StageScoreManager.ResultData bonusCountCampaignData = this.m_scoreManager.BonusCountCampaignData;
			if (this.IsBonus(bonusCountCampaignData, null, null))
			{
				this.SetupBonus(bonusCountCampaignData, null, ref this.m_bonusEventScores[num7].bonusData);
			}
			else
			{
				this.m_bonusEventInfos[num7].obj = null;
			}
			int num8 = 2;
			this.m_bonusEventInfos[num8].obj = GameObjectUtil.FindChildGameObject(this.m_eventRoot, "playerbonus");
			this.m_bonusEventInfos[num8].animClipName = "ui_result_word_playerbonus_Anim";
			this.m_bonusEventInfos[num8].viewTime = this.m_bonusEventAnim[this.m_bonusEventInfos[num8].animClipName].length * 0.3f;
			this.m_bonusEventInfos[num8].valueText = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_bonusEventInfos[num8].obj, "Lbl_num_playerbonus_score");
			StageScoreManager.ResultData bonusCountMainCharaData = this.m_scoreManager.BonusCountMainCharaData;
			StageScoreManager.ResultData bonusCountSubCharaData = this.m_scoreManager.BonusCountSubCharaData;
			StageScoreManager.ResultData bonusCountRankData = this.m_scoreManager.BonusCountRankData;
			if (this.IsBonus(bonusCountMainCharaData, bonusCountSubCharaData, bonusCountRankData))
			{
				this.SetupBonus(bonusCountMainCharaData, bonusCountSubCharaData, ref this.m_bonusEventScores[num8].bonusData);
				this.SetupBonus(bonusCountRankData, null, ref this.m_RankBonusEventScores.bonusData);
			}
			else
			{
				this.m_bonusEventInfos[num8].obj = null;
			}
		}
		this.m_addScore = GameResultScores.Category.NONE;
		this.OnSetup(resultRoot);
	}

	public bool IsCampaignBonus()
	{
		return this.m_bonusEventInfos[1].obj != null;
	}

	private void SetResultScore(StageScoreManager.ResultData resultDataScore)
	{
		if (this.m_scores[0] != null)
		{
			this.m_scores[0].AddScore(resultDataScore.score);
		}
		if (this.m_scores[1] != null)
		{
			this.m_scores[1].AddScore(resultDataScore.ring);
		}
		if (this.m_scores[2] != null)
		{
			this.m_scores[2].AddScore(resultDataScore.red_ring);
		}
		if (this.m_scores[3] != null)
		{
			this.m_scores[3].AddScore(resultDataScore.animal);
		}
		if (this.m_scores[4] != null)
		{
			this.m_scores[4].AddScore(resultDataScore.distance);
		}
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage() && this.m_scores[6] != null)
		{
			this.m_scores[6].AddScore(resultDataScore.raid_boss_ring);
		}
	}

	private void SetActiveBonusEventScore1(int category, int scoreDataType, bool on)
	{
		if (this.m_bonusEventScores[category].bonusData[scoreDataType].labelScore1 != null)
		{
			this.m_bonusEventScores[category].bonusData[scoreDataType].labelScore1.gameObject.SetActive(on);
		}
		if (this.m_bonusEventScores[category].bonusData[scoreDataType].uiSprite1 != null)
		{
			this.m_bonusEventScores[category].bonusData[scoreDataType].uiSprite1.gameObject.SetActive(on);
		}
		if (this.m_bonusEventScores[category].bonusData[scoreDataType].uiTexture1 != null)
		{
			this.m_bonusEventScores[category].bonusData[scoreDataType].uiTexture1.gameObject.SetActive(on);
		}
	}

	private void SetActiveBonusEventScore2(int category, int scoreDataType, bool on)
	{
		if (this.m_bonusEventScores[category].bonusData[scoreDataType].labelScore2 != null)
		{
			this.m_bonusEventScores[category].bonusData[scoreDataType].labelScore2.gameObject.SetActive(on);
		}
		if (this.m_bonusEventScores[category].bonusData[scoreDataType].uiSprite2 != null)
		{
			this.m_bonusEventScores[category].bonusData[scoreDataType].uiSprite2.gameObject.SetActive(on);
		}
		if (this.m_bonusEventScores[category].bonusData[scoreDataType].uiTexture2 != null)
		{
			this.m_bonusEventScores[category].bonusData[scoreDataType].uiTexture2.gameObject.SetActive(on);
		}
	}

	private void SetActiveBonusEventScore(UILabel uiLavel, bool on)
	{
		if (uiLavel != null)
		{
			uiLavel.gameObject.SetActive(on);
		}
	}

	private void SetupBonus(StageScoreManager.ResultData resultData1, StageScoreManager.ResultData resultData2, ref BonusData[] bonusScore)
	{
		if (bonusScore == null)
		{
			return;
		}
		if (resultData1 != null)
		{
			int num = 0;
			if (bonusScore[num] != null && bonusScore[num].labelScore1 != null)
			{
				bonusScore[num].labelScore1.text = GameResultUtility.GetScoreFormat(resultData1.score);
			}
			int num2 = 1;
			if (bonusScore[num2] != null && bonusScore[num2].labelScore1 != null)
			{
				bonusScore[num2].labelScore1.text = GameResultUtility.GetScoreFormat(resultData1.ring);
			}
			int num3 = 3;
			if (bonusScore[num3] != null && bonusScore[num3].labelScore1 != null)
			{
				bonusScore[num3].labelScore1.text = GameResultUtility.GetScoreFormat(resultData1.animal);
			}
			int num4 = 4;
			if (bonusScore[num4] != null && bonusScore[num4].labelScore1 != null)
			{
				bonusScore[num4].labelScore1.text = GameResultUtility.GetScoreFormat(resultData1.distance);
			}
			if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
			{
				int num5 = 6;
				if (bonusScore[num5] != null && bonusScore[num5].labelScore1 != null)
				{
					bonusScore[num5].labelScore1.text = GameResultUtility.GetScoreFormat(resultData1.raid_boss_ring);
				}
			}
		}
		if (resultData2 != null)
		{
			int num6 = 0;
			if (bonusScore[num6] != null && bonusScore[num6].labelScore2 != null)
			{
				bonusScore[num6].labelScore2.text = GameResultUtility.GetScoreFormat(resultData2.score);
			}
			int num7 = 1;
			if (bonusScore[num7] != null && bonusScore[num7].labelScore2 != null)
			{
				bonusScore[num7].labelScore2.text = GameResultUtility.GetScoreFormat(resultData2.ring);
			}
			int num8 = 3;
			if (bonusScore[num8] != null && bonusScore[num8].labelScore2 != null)
			{
				bonusScore[num8].labelScore2.text = GameResultUtility.GetScoreFormat(resultData2.animal);
			}
			int num9 = 4;
			if (bonusScore[num9] != null && bonusScore[num9].labelScore2 != null)
			{
				bonusScore[num9].labelScore2.text = GameResultUtility.GetScoreFormat(resultData2.distance);
			}
			if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
			{
				int num10 = 6;
				if (bonusScore[num10] != null && bonusScore[num10].labelScore2 != null)
				{
					bonusScore[num10].labelScore2.text = GameResultUtility.GetScoreFormat(resultData2.raid_boss_ring);
				}
			}
		}
	}

	private bool IsBonusRate(StageAbilityManager.BonusRate bonusRate, ScoreDataType scoreDataType)
	{
		switch (scoreDataType)
		{
		case ScoreDataType.SCORE:
			if (bonusRate.score > 0f)
			{
				return true;
			}
			break;
		case ScoreDataType.RING:
			if (bonusRate.ring > 0f)
			{
				return true;
			}
			break;
		case ScoreDataType.REDSTAR_RING:
			if (bonusRate.red_ring > 0f)
			{
				return true;
			}
			break;
		case ScoreDataType.ANIMAL:
			if (bonusRate.animal > 0f)
			{
				return true;
			}
			break;
		case ScoreDataType.DISTANCE:
			if (bonusRate.distance > 0f)
			{
				return true;
			}
			break;
		case ScoreDataType.RAIDBOSS_RING:
			if (bonusRate.raid_boss_ring > 0f)
			{
				return true;
			}
			break;
		}
		return false;
	}

	protected void SetBonusEventScoreActive(GameResultScores.Category category)
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_bonusEventScores[i].obj != null)
			{
				this.m_bonusEventScores[i].obj.SetActive(category == (GameResultScores.Category)i);
			}
		}
		if (this.m_chaoCountBonusEventScores.obj != null)
		{
			this.m_chaoCountBonusEventScores.obj.SetActive(GameResultScores.Category.CHAO == category);
		}
		if (this.m_RankBonusEventScores.obj != null)
		{
			this.m_RankBonusEventScores.obj.SetActive(GameResultScores.Category.CHARA == category);
		}
		this.AddScore(category);
		if (this.m_scores[5] != null)
		{
			long bonusTotalScore = this.GetBonusTotalScore(category);
			this.m_scores[5].SetLabelStartValue(bonusTotalScore);
		}
	}

	private long GetBonusTotalScore(GameResultScores.Category category)
	{
		long result = 0L;
		switch (category)
		{
		case GameResultScores.Category.CHAO:
			result = this.m_scoreManager.ResultChaoBonusTotal;
			break;
		case GameResultScores.Category.CAMPAIGN:
			result = this.m_scoreManager.ResultCampaignBonusTotal;
			break;
		case GameResultScores.Category.CHARA:
			result = this.m_scoreManager.ResultPlayerBonusTotal;
			break;
		}
		return result;
	}

	private void AddScore(GameResultScores.Category category)
	{
		switch (category)
		{
		case GameResultScores.Category.CHAO:
			if (this.m_addScore == GameResultScores.Category.NONE)
			{
				this.SetResultScore(this.m_scoreManager.BonusCountMainChaoData);
				this.SetResultScore(this.m_scoreManager.BonusCountSubChaoData);
				this.SetResultScore(this.m_scoreManager.BonusCountChaoCountData);
				this.m_addScore = GameResultScores.Category.CHAO;
			}
			break;
		case GameResultScores.Category.CAMPAIGN:
			if (this.m_addScore == GameResultScores.Category.CHAO)
			{
				this.SetResultScore(this.m_scoreManager.BonusCountCampaignData);
				this.m_addScore = GameResultScores.Category.CAMPAIGN;
			}
			break;
		case GameResultScores.Category.CHARA:
			if (this.m_addScore == GameResultScores.Category.CAMPAIGN)
			{
				this.SetResultScore(this.m_scoreManager.BonusCountMainCharaData);
				this.SetResultScore(this.m_scoreManager.BonusCountSubCharaData);
				this.SetResultScore(this.m_scoreManager.BonusCountRankData);
				this.m_addScore = GameResultScores.Category.CHARA;
			}
			break;
		}
	}

	public bool IsBonusEvent()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_bonusEventInfos[i].obj != null)
			{
				return true;
			}
		}
		return false;
	}

	public void PlayStart()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	public void PlaySkip()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(101);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	public void SetActiveDetailsButton(bool on)
	{
		this.OnSetActiveDetailsButton(on);
	}

	public void AllSkip()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(102);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	public void OnFinishScore()
	{
		this.OnFinish();
	}

	private void OnStartBonusScore(GameResultScores.Category category)
	{
		GameObject obj = this.m_bonusEventInfos[(int)category].obj;
		if (this.m_isReplay)
		{
			if (obj != null)
			{
				obj.SetActive(false);
			}
			this.m_eventUpdateState = GameResultScores.EventUpdateState.Idle;
			GameResultScores.Category nextCategory = this.GetNextCategory(category);
			this.ReplaceDitailButtonLabel(nextCategory);
			if (this.m_bonusEventInfos[(int)category].obj != null)
			{
				this.SetBonusEventScoreActive(category);
			}
			else
			{
				this.AddScore(category);
			}
		}
		else if (obj != null && this.m_bonusEventAnim != null)
		{
			obj.SetActive(true);
			this.m_bonusEventAnim.Rewind(this.m_bonusEventInfos[(int)category].animClipName);
			if (this.m_isBossResult)
			{
				this.m_bonusEventInfos[(int)category].valueText.text = string.Empty;
			}
			else
			{
				this.m_bonusEventInfos[(int)category].valueText.text = this.GetBonusTotalScore(category).ToString();
			}
			ActiveAnimation.Play(this.m_bonusEventAnim, this.m_bonusEventInfos[(int)category].animClipName, Direction.Forward, false);
			SoundManager.SePlay("sys_bonus", "SE");
			this.m_eventUpdateState = GameResultScores.EventUpdateState.Start;
		}
		else
		{
			this.m_eventUpdateState = GameResultScores.EventUpdateState.Idle;
		}
	}

	private GameResultScores.Category GetNextCategory(GameResultScores.Category nowCategory)
	{
		GameResultScores.Category category = GameResultScores.Category.CHAO;
		if (nowCategory == GameResultScores.Category.CHAO)
		{
			category = GameResultScores.Category.CAMPAIGN;
		}
		else if (nowCategory == GameResultScores.Category.CAMPAIGN)
		{
			category = GameResultScores.Category.CHARA;
		}
		else if (nowCategory == GameResultScores.Category.CHARA)
		{
			category = GameResultScores.Category.NUM;
		}
		if (category != GameResultScores.Category.NUM && this.m_bonusEventInfos[(int)category].obj == null)
		{
			category = this.GetNextCategory(category);
		}
		return category;
	}

	private void ReplaceDitailButtonLabel(GameResultScores.Category category)
	{
		if (this.m_DetailButtonLabel != null)
		{
			this.m_DetailButtonLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Result", this.BonusCategoryButtonLabel[(int)category]).text;
		}
		if (this.m_DetailButtonLabel_Sh != null)
		{
			this.m_DetailButtonLabel_Sh.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Result", this.BonusCategoryButtonLabel[(int)category]).text;
		}
	}

	private void OnUpdateBonusScore(GameResultScores.Category category)
	{
		GameObject obj = this.m_bonusEventInfos[(int)category].obj;
		if (obj != null && this.m_bonusEventAnim != null)
		{
			GameResultScores.EventUpdateState eventUpdateState = this.m_eventUpdateState;
			if (eventUpdateState != GameResultScores.EventUpdateState.Start)
			{
				if (eventUpdateState == GameResultScores.EventUpdateState.Wait)
				{
					this.m_timer -= Time.deltaTime;
					if (this.m_timer < 0f)
					{
						this.m_eventUpdateState = GameResultScores.EventUpdateState.Idle;
					}
				}
			}
			else
			{
				float time = this.m_bonusEventAnim[this.m_bonusEventInfos[(int)category].animClipName].time;
				if (time > this.m_bonusEventInfos[(int)category].viewTime)
				{
					this.SetBonusEventScoreActive(category);
					this.m_timer = 2f;
					this.m_eventUpdateState = GameResultScores.EventUpdateState.Wait;
				}
			}
		}
	}

	private void OnSkipBonusScore(GameResultScores.Category category)
	{
		this.AddScore(category);
	}

	private void OnEndBonusScore(GameResultScores.Category category)
	{
		GameObject obj = this.m_bonusEventInfos[(int)category].obj;
		if (obj != null && this.m_bonusEventAnim != null)
		{
			this.SetBonusEventScoreActive(category);
			this.m_bonusEventAnim.Play();
			float length = this.m_bonusEventAnim[this.m_bonusEventInfos[(int)category].animClipName].length;
			this.m_bonusEventAnim[this.m_bonusEventInfos[(int)category].animClipName].time = length;
			this.m_bonusEventAnim.Sample();
			this.m_bonusEventAnim.Stop();
			if (this.m_isReplay)
			{
				obj.SetActive(false);
			}
		}
		else
		{
			this.AddScore(category);
		}
	}

	private bool IsEndBonusScore(GameResultScores.Category category)
	{
		return this.m_eventUpdateState == GameResultScores.EventUpdateState.Idle && (!(this.m_bonusEventInfos[(int)category].obj != null) || !this.m_isReplay);
	}

	protected void SetEnableNextButton(bool enabled)
	{
		if (this.m_resultObj != null)
		{
			this.m_resultObj.SendMessage("OnSetEnableNextButton", enabled);
		}
	}

	protected void SetEnableDetailsButton(bool enabled)
	{
		if (this.m_resultObj != null)
		{
			this.m_resultObj.SendMessage("OnSetEnableDetailsButton", enabled);
		}
	}

	protected virtual void OnSetActiveDetailsButton(bool on)
	{
	}

	protected abstract bool IsBonus(StageScoreManager.ResultData data1, StageScoreManager.ResultData data2, StageScoreManager.ResultData data3);

	protected abstract void OnSetup(GameObject resultRoot);

	protected abstract void OnFinish();

	protected virtual void OnStartMileageBonusScore()
	{
	}

	protected virtual void OnUpdateMileageBonusScore()
	{
	}

	protected virtual void OnEndMileageBonusScore()
	{
	}

	protected virtual void OnSkipMileageBonusScore()
	{
	}

	protected virtual bool IsEndMileageBonusScore()
	{
		return true;
	}

	protected virtual void OnStartFinished()
	{
	}

	protected virtual void OnUpdateFinished()
	{
	}

	protected virtual void OnEndFinished()
	{
	}

	protected virtual void OnSkipFinished()
	{
	}

	protected virtual bool IsEndFinished()
	{
		return true;
	}

	protected virtual void OnStartBeginning()
	{
	}

	protected virtual void OnUpdateBeginning()
	{
	}

	protected virtual void OnEndBeginning()
	{
	}

	protected virtual void OnSkipBeginning()
	{
	}

	protected virtual bool IsEndBeginning()
	{
		return true;
	}

	protected virtual void OnScoreInAnimation(EventDelegate.Callback callback)
	{
	}

	protected virtual void OnScoreOutAnimation(EventDelegate.Callback callback)
	{
	}

	public void PlayScoreInAnimation(EventDelegate.Callback callback)
	{
		this.OnScoreInAnimation(callback);
	}

	public void PlayScoreOutAnimation(EventDelegate.Callback callback)
	{
		this.OnScoreOutAnimation(callback);
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_skip = false;
			this.m_allSkip = false;
			this.m_category = GameResultScores.Category.CHAO;
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateBeginning)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateBeginning(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.OnStartBeginning();
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 101 && signal != 102)
			{
				return TinyFsmState.End();
			}
			this.OnSkipBeginning();
			return TinyFsmState.End();
		case 4:
			this.OnUpdateBeginning();
			if (this.IsEndBeginning())
			{
				this.OnEndBeginning();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateBonusUpdate)));
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateBonusUpdate(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_skip = false;
			this.OnStartBonusScore(this.m_category);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal == 101)
			{
				this.OnSkipBonusScore(this.m_category);
				this.m_skip = true;
				return TinyFsmState.End();
			}
			if (signal != 102)
			{
				return TinyFsmState.End();
			}
			this.OnSkipBonusScore(this.m_category);
			this.m_allSkip = true;
			return TinyFsmState.End();
		case 4:
			this.OnUpdateBonusScore(this.m_category);
			if (this.IsEndBonusScore(this.m_category) || this.m_skip || this.m_allSkip)
			{
				this.OnEndBonusScore(this.m_category);
				if (this.m_allSkip)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateMileageBonusUpdate)));
				}
				else if (this.m_category == GameResultScores.Category.CHAO)
				{
					this.m_category = GameResultScores.Category.CAMPAIGN;
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateBonusUpdate)));
				}
				else if (this.m_category == GameResultScores.Category.CAMPAIGN)
				{
					this.m_category = GameResultScores.Category.CHARA;
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateBonusUpdate)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateMileageBonusUpdate)));
				}
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateMileageBonusUpdate(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.AddScore(GameResultScores.Category.CHAO);
			this.AddScore(GameResultScores.Category.CAMPAIGN);
			this.AddScore(GameResultScores.Category.CHARA);
			this.m_skip = false;
			this.OnStartMileageBonusScore();
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 101 && signal != 102)
			{
				return TinyFsmState.End();
			}
			this.OnSkipMileageBonusScore();
			this.m_skip = true;
			this.m_allSkip = true;
			return TinyFsmState.End();
		case 4:
			this.OnUpdateMileageBonusScore();
			if (this.IsEndMileageBonusScore() || this.m_skip || this.m_allSkip)
			{
				if (this.m_allSkip)
				{
					this.OnSkipMileageBonusScore();
				}
				this.OnEndMileageBonusScore();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFinished)));
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateFinished(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.OnStartFinished();
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 101 && signal != 102)
			{
				return TinyFsmState.End();
			}
			if (!this.m_finished)
			{
				this.OnSkipFinished();
			}
			return TinyFsmState.End();
		case 4:
			if (!this.m_finished)
			{
				this.OnUpdateFinished();
				if (this.IsEndFinished())
				{
					this.OnEndFinished();
					this.m_finished = true;
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
					this.m_isReplay = true;
					GameResultScores.Category nextCategory = this.GetNextCategory(GameResultScores.Category.NUM);
					this.ReplaceDitailButtonLabel(nextCategory);
				}
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private void DebugScoreLog()
	{
	}

	private void DebugScoreLogResultData(string msg, StageScoreManager.ResultData resultData)
	{
	}
}
