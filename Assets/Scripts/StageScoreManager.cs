using GameScore;
using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/GameMode/Stage")]
public class StageScoreManager : MonoBehaviour
{
	public enum DataType
	{
		StageCount,
		BonusCount,
		BonusCount_MainChao,
		BonusCount_SubChao,
		BonusCount_ChaoCount,
		BonusCount_MainChara,
		BonusCount_SubChara,
		BonusCount_Campaign,
		BonusCount_Rank,
		FinalCount,
		Score,
		MileageBonusScore,
		NUM
	}

	public struct MaskedInt
	{
		private int m_valueUp;

		private int m_valueDown;

		private int m_mask;

		private int m_addNum;

		public void Set(int input)
		{
			if (this.m_mask == 0)
			{
				this.m_mask = UnityEngine.Random.Range(15, 2147483647);
				this.m_addNum = UnityEngine.Random.Range(15, 1024);
			}
			input += this.m_addNum;
			this.m_valueUp = (input & this.m_mask);
			this.m_valueDown = (input & ~this.m_mask);
		}

		public int Get()
		{
			int num = this.m_valueUp | this.m_valueDown;
			return num - this.m_addNum;
		}
	}

	public struct MaskedLong
	{
		private long m_valueUp;

		private long m_valueDown;

		private long m_mask;

		private long m_addNum;

		public void Set(long input)
		{
			if (this.m_mask == 0L)
			{
				this.m_mask = (long)UnityEngine.Random.Range(15, 2147483647);
				this.m_addNum = (long)UnityEngine.Random.Range(15, 1024);
			}
			input += this.m_addNum;
			this.m_valueUp = (input & this.m_mask);
			this.m_valueDown = (input & ~this.m_mask);
		}

		public long Get()
		{
			long num = this.m_valueUp | this.m_valueDown;
			return num - this.m_addNum;
		}
	}

	public class ResultData
	{
		private StageScoreManager.MaskedLong m_score;

		private StageScoreManager.MaskedLong m_animal;

		private StageScoreManager.MaskedLong m_ring;

		private StageScoreManager.MaskedLong m_red_ring;

		private StageScoreManager.MaskedLong m_distance;

		private StageScoreManager.MaskedLong m_sp_crystal;

		private StageScoreManager.MaskedLong m_raid_boss_ring;

		private StageScoreManager.MaskedLong m_raid_boss_reward;

		private StageScoreManager.MaskedLong m_final_score;

		public long score
		{
			get
			{
				return this.m_score.Get();
			}
			set
			{
				this.m_score.Set(value);
			}
		}

		public long animal
		{
			get
			{
				return this.m_animal.Get();
			}
			set
			{
				this.m_animal.Set(value);
			}
		}

		public long ring
		{
			get
			{
				return this.m_ring.Get();
			}
			set
			{
				this.m_ring.Set(value);
			}
		}

		public long red_ring
		{
			get
			{
				return this.m_red_ring.Get();
			}
			set
			{
				this.m_red_ring.Set(value);
			}
		}

		public long distance
		{
			get
			{
				return this.m_distance.Get();
			}
			set
			{
				this.m_distance.Set(value);
			}
		}

		public long sp_crystal
		{
			get
			{
				return this.m_sp_crystal.Get();
			}
			set
			{
				this.m_sp_crystal.Set(value);
			}
		}

		public long raid_boss_ring
		{
			get
			{
				return this.m_raid_boss_ring.Get();
			}
			set
			{
				this.m_raid_boss_ring.Set(value);
			}
		}

		public long raid_boss_reward
		{
			get
			{
				return this.m_raid_boss_reward.Get();
			}
			set
			{
				this.m_raid_boss_reward.Set(value);
			}
		}

		public long final_score
		{
			get
			{
				return this.m_final_score.Get();
			}
			set
			{
				this.m_final_score.Set(value);
			}
		}

		public ResultData()
		{
			this.m_animal = default(StageScoreManager.MaskedLong);
			this.m_ring = default(StageScoreManager.MaskedLong);
			this.m_red_ring = default(StageScoreManager.MaskedLong);
			this.m_sp_crystal = default(StageScoreManager.MaskedLong);
			this.m_raid_boss_ring = default(StageScoreManager.MaskedLong);
			this.m_raid_boss_reward = default(StageScoreManager.MaskedLong);
			this.m_score = default(StageScoreManager.MaskedLong);
			this.m_distance = default(StageScoreManager.MaskedLong);
			this.m_final_score = default(StageScoreManager.MaskedLong);
		}

		public long Sum()
		{
			long num = 0L;
			num += this.score;
			num += this.animal;
			num += this.ring;
			num += this.red_ring;
			return num + this.distance;
		}
	}

	private const float FloatToInt = 1000000f;

	private readonly int m_stockRingCoefficient = Data.ResultRing;

	private readonly int m_scoreCoefficient = 1;

	private readonly int m_animalCoefficient = Data.ResultAnimal;

	private readonly int m_totaldistanceCoefficient = Data.ResultDistance;

	[SerializeField]
	private int m_continueRing = 1000;

	[SerializeField]
	private int m_continueRaidBossRing = 500;

	private StageScoreManager.MaskedLong m_score = default(StageScoreManager.MaskedLong);

	private StageScoreManager.MaskedLong m_animal = default(StageScoreManager.MaskedLong);

	private StageScoreManager.MaskedLong m_ring = default(StageScoreManager.MaskedLong);

	private StageScoreManager.MaskedLong m_red_ring = default(StageScoreManager.MaskedLong);

	private StageScoreManager.MaskedLong m_special_crystal = default(StageScoreManager.MaskedLong);

	private StageScoreManager.MaskedLong m_raid_boss_ring = default(StageScoreManager.MaskedLong);

	private StageScoreManager.MaskedLong m_final_score = default(StageScoreManager.MaskedLong);

	private StageScoreManager.MaskedLong m_collectEventCount = default(StageScoreManager.MaskedLong);

	private StageScoreManager.ResultData[] m_results;

	private PlayerInformation m_information;

	private LevelInformation m_levelInformation;

	private bool m_bossStage;

	private bool m_quickMode;

	private bool m_isFinalScore;

	private StageScorePool m_scorePool;

	private float m_rank_rate;

	private long m_realtime_score_back;

	private long m_animal_back;

	private long m_event_score_back;

	private long m_realtime_score_old;

	private long m_animal_old;

	private long m_event_score_old;

	private static StageScoreManager instance;

	public static StageScoreManager Instance
	{
		get
		{
			return StageScoreManager.instance;
		}
	}

	public StageScoreManager.ResultData ScoreData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.Score);
		}
	}

	public StageScoreManager.ResultData MileageBonusScoreData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.MileageBonusScore);
		}
	}

	public StageScoreManager.ResultData CountData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.StageCount);
		}
	}

	public StageScoreManager.ResultData BonusCountData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount);
		}
	}

	public StageScoreManager.ResultData BonusCountMainChaoData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount_MainChao);
		}
	}

	public StageScoreManager.ResultData BonusCountSubChaoData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount_SubChao);
		}
	}

	public StageScoreManager.ResultData BonusCountChaoCountData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount_ChaoCount);
		}
	}

	public StageScoreManager.ResultData BonusCountMainCharaData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount_MainChara);
		}
	}

	public StageScoreManager.ResultData BonusCountSubCharaData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount_SubChara);
		}
	}

	public StageScoreManager.ResultData BonusCountCampaignData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount_Campaign);
		}
	}

	public StageScoreManager.ResultData BonusCountRankData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.BonusCount_Rank);
		}
	}

	public StageScoreManager.ResultData FinalCountData
	{
		get
		{
			return this.GetResultData(StageScoreManager.DataType.FinalCount);
		}
	}

	public long ResultChaoBonusTotal
	{
		get
		{
			long num = 0L;
			num += this.GetResultBonusScore(StageScoreManager.DataType.BonusCount_MainChao);
			num += this.GetResultBonusScore(StageScoreManager.DataType.BonusCount_SubChao);
			return num + this.GetResultBonusScore(StageScoreManager.DataType.BonusCount_ChaoCount);
		}
	}

	public long ResultCampaignBonusTotal
	{
		get
		{
			long num = 0L;
			return num + this.GetResultBonusScore(StageScoreManager.DataType.BonusCount_Campaign);
		}
	}

	public long ResultPlayerBonusTotal
	{
		get
		{
			long num = 0L;
			num += this.GetResultBonusScore(StageScoreManager.DataType.BonusCount_MainChara);
			num += this.GetResultBonusScore(StageScoreManager.DataType.BonusCount_SubChara);
			return num + this.GetResultBonusScore(StageScoreManager.DataType.BonusCount_Rank);
		}
	}

	public long FinalScore
	{
		get
		{
			return this.m_final_score.Get();
		}
	}

	public long Score
	{
		get
		{
			return this.m_score.Get();
		}
	}

	public long Animal
	{
		get
		{
			return this.m_animal.Get();
		}
	}

	public long Ring
	{
		get
		{
			return this.m_ring.Get();
		}
	}

	public long RedRing
	{
		get
		{
			return this.m_red_ring.Get();
		}
	}

	public long SpecialCrystal
	{
		get
		{
			return this.m_special_crystal.Get();
		}
	}

	public long RaidBossRing
	{
		get
		{
			return this.m_raid_boss_ring.Get();
		}
	}

	public long CollectEventCount
	{
		get
		{
			return this.m_collectEventCount.Get();
		}
	}

	public int ContinueRing
	{
		get
		{
			return this.m_continueRing;
		}
	}

	public int ContinueRaidBossRing
	{
		get
		{
			return this.m_continueRaidBossRing;
		}
	}

	public StageScorePool ScorePool
	{
		get
		{
			return this.m_scorePool;
		}
		private set
		{
		}
	}

	public StageScoreManager.ResultData GetResultData(StageScoreManager.DataType type)
	{
		if (type < StageScoreManager.DataType.NUM && this.m_results != null && type < (StageScoreManager.DataType)this.m_results.Length)
		{
			return this.m_results[(int)type];
		}
		return new StageScoreManager.ResultData();
	}

	public long GetResultBonusScore(StageScoreManager.DataType type)
	{
		long num = 0L;
		num += this.m_results[(int)type].score * (long)this.m_scoreCoefficient;
		num += this.m_results[(int)type].ring * (long)this.m_stockRingCoefficient;
		num += this.m_results[(int)type].animal * (long)this.m_animalCoefficient;
		return num + this.m_results[(int)type].distance * (long)this.m_totaldistanceCoefficient;
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		GameObject gameObject = GameObject.Find("PlayerInformation");
		if (gameObject != null)
		{
			this.m_information = gameObject.GetComponent<PlayerInformation>();
		}
		this.ResetScore();
		this.m_scorePool = new StageScorePool();
	}

	private void OnDestroy()
	{
		if (StageScoreManager.instance == this)
		{
			StageScoreManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (StageScoreManager.instance == null)
		{
			StageScoreManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Setup(bool bossStage)
	{
		this.m_bossStage = bossStage;
		if (StageModeManager.Instance != null)
		{
			this.m_quickMode = StageModeManager.Instance.IsQuickMode();
		}
		this.m_levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
		if (this.m_scorePool != null)
		{
			this.m_scorePool.CheckHalfWay();
		}
		CPlusPlusLink cPlusPlusLink = CPlusPlusLink.Instance;
		if (cPlusPlusLink != null)
		{
			cPlusPlusLink.ResetNativeResultScore();
		}
	}

	public void ResetScore(MsgResetScore msg)
	{
		this.ResetScore((long)msg.m_score, (long)msg.m_animal, (long)msg.m_ring, (long)msg.m_red_ring, (long)msg.m_final_score);
	}

	public void AddScore(long score)
	{
		this.m_score.Set(this.m_score.Get() + score);
	}

	public void AddAnimal(long addCount)
	{
		this.m_animal.Set(this.m_animal.Get() + addCount);
	}

	public void AddRedRing()
	{
		this.m_red_ring.Set(this.m_red_ring.Get() + 1L);
	}

	public void AddRecoveryRingCount(int addCount)
	{
		int transforDoubleRing = this.GetTransforDoubleRing(addCount);
		this.m_ring.Set(this.m_ring.Get() + (long)transforDoubleRing);
		ObjUtil.SendMessageScoreCheck(new StageScoreData(9, transforDoubleRing));
	}

	public void AddSpecialCrystal(long addCount)
	{
		this.m_special_crystal.Set(this.m_special_crystal.Get() + addCount);
	}

	public void AddScoreCheck(StageScoreData scoreData)
	{
		if (this.m_scorePool != null)
		{
			this.m_scorePool.AddScore(scoreData);
		}
	}

	private void ResetScore()
	{
		this.ResetScore(0L, 0L, 0L, 0L, 0L);
		this.m_special_crystal.Set(0L);
		this.m_raid_boss_ring.Set(0L);
		this.m_collectEventCount.Set(0L);
		this.m_realtime_score_back = 0L;
		this.m_animal_back = 0L;
		this.m_event_score_back = 0L;
		this.m_realtime_score_old = 0L;
		this.m_animal_old = 0L;
		this.m_event_score_old = 0L;
	}

	private void ResetScore(long score, long animal, long ring, long red_ring, long final_score)
	{
		this.m_score.Set(score);
		this.m_animal.Set(animal);
		this.m_ring.Set(ring);
		this.m_red_ring.Set(red_ring);
		this.m_final_score.Set(final_score);
	}

	private int GetTransforDoubleRing(int transferRingCount)
	{
		if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.TRANSFER_DOUBLE_RING))
		{
			ObjUtil.RequestStartAbilityToChao(ChaoAbility.TRANSFER_DOUBLE_RING, false);
			return StageAbilityManager.Instance.GetChaoAbliltyValue(ChaoAbility.TRANSFER_DOUBLE_RING, transferRingCount);
		}
		return transferRingCount;
	}

	public void TransferRingForContinue(int ring)
	{
		if (this.m_information != null)
		{
			int transforDoubleRing = this.GetTransforDoubleRing(this.m_information.NumRings);
			this.m_ring.Set(this.m_ring.Get() + (long)transforDoubleRing);
			ObjUtil.SendMessageScoreCheck(new StageScoreData(9, transforDoubleRing));
			this.m_information.SetNumRings(ring);
			GameObjectUtil.SendMessageToTagObjects("Player", "OnResetRingsForContinue", null, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void TransferRingForChaoAbility(int ring)
	{
		int transforDoubleRing = this.GetTransforDoubleRing(ring);
		this.m_ring.Set(this.m_ring.Get() + (long)transforDoubleRing);
		ObjUtil.SendMessageScoreCheck(new StageScoreData(9, transforDoubleRing));
	}

	public bool DefrayItemCostByRing(long costRing)
	{
		bool flag = false;
		if (this.m_ring.Get() > 0L)
		{
			if (this.m_ring.Get() > costRing)
			{
				this.m_ring.Set(this.m_ring.Get() - costRing);
			}
			else
			{
				this.m_ring.Set(0L);
			}
			flag = true;
		}
		else if (this.m_information != null)
		{
			long num = (long)this.m_information.NumRings;
			if (num > 0L)
			{
				if (num > costRing)
				{
					this.m_information.SetNumRings((int)(num - costRing));
				}
				else
				{
					this.m_information.SetNumRings(0);
				}
				flag = true;
			}
		}
		if (flag)
		{
			GameObjectUtil.SendMessageToTagObjects("Player", "OnDefrayRing", null, SendMessageOptions.DontRequireReceiver);
		}
		return flag;
	}

	public void TransferRing()
	{
		if (this.m_information != null && this.m_levelInformation != null)
		{
			int transforDoubleRing = this.GetTransforDoubleRing(this.m_information.NumRings);
			this.m_ring.Set(this.m_ring.Get() + (long)transforDoubleRing);
			ObjUtil.SendMessageScoreCheck(new StageScoreData(9, transforDoubleRing));
			bool flag = true;
			if (this.m_information.NumRings > 0 && !this.m_levelInformation.DestroyRingMode)
			{
				flag = false;
			}
			if (!flag)
			{
				this.m_information.SetNumRings(1);
			}
			else
			{
				this.m_information.SetNumRings(0);
			}
			GameObjectUtil.SendMessageToTagObjects("Player", "OnResetRingsForCheckPoint", new MsgPlayerTransferRing(flag), SendMessageOptions.DontRequireReceiver);
		}
	}

	public void TransferRingCountToRaidBossRingCount()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			this.m_raid_boss_ring = this.m_ring;
			this.m_ring.Set(0L);
		}
	}

	public void SendMessageFinalScoreBeforeResult()
	{
		if (!this.m_isFinalScore)
		{
			this.m_isFinalScore = true;
		}
		this.OnCalcFinalScore();
	}

	public void OnCalcFinalScore()
	{
		this.m_results = null;
		this.m_results = new StageScoreManager.ResultData[12];
		for (int i = 0; i < 12; i++)
		{
			this.m_results[i] = new StageScoreManager.ResultData();
		}
		float num = (!(this.m_information != null)) ? 0f : this.m_information.TotalDistance;
		int num2 = (int)num;
		this.SetStageCount(num2);
		this.SetBonusCountType(num2, StageScoreManager.DataType.BonusCount_MainChao);
		this.SetBonusCountType(num2, StageScoreManager.DataType.BonusCount_SubChao);
		this.SetBonusCountType(num2, StageScoreManager.DataType.BonusCount_ChaoCount);
		this.SetBonusCountType(num2, StageScoreManager.DataType.BonusCount_MainChara);
		this.SetBonusCountType(num2, StageScoreManager.DataType.BonusCount_SubChara);
		this.SetBonusCountRank();
		this.SetBonusCountCampaign();
		this.SetBonusCount();
		this.SetFinalCount(num2);
		this.SetScore();
		this.SetMileageBonusScore();
		this.SetCollectEventCount();
		this.SetFinalScore();
	}

	private void SetStageCount(int distance)
	{
		int num = 0;
		this.m_results[num].score = this.m_score.Get();
		this.m_results[num].animal = this.m_animal.Get();
		this.m_results[num].ring = this.m_ring.Get();
		this.m_results[num].red_ring = this.m_red_ring.Get();
		this.m_results[num].distance = (long)distance;
		this.m_results[num].sp_crystal = this.m_special_crystal.Get();
		this.m_results[num].raid_boss_ring = this.m_raid_boss_ring.Get();
	}

	private void SetBonusCountType(int distance, StageScoreManager.DataType type)
	{
		if (StageAbilityManager.Instance != null)
		{
			StageAbilityManager.BonusRate bonusRate;
			switch (type)
			{
			case StageScoreManager.DataType.BonusCount_MainChao:
				bonusRate = StageAbilityManager.Instance.MainChaoBonusValueRate;
				break;
			case StageScoreManager.DataType.BonusCount_SubChao:
				bonusRate = StageAbilityManager.Instance.SubChaoBonusValueRate;
				break;
			case StageScoreManager.DataType.BonusCount_ChaoCount:
				bonusRate = StageAbilityManager.Instance.CountChaoBonusValueRate;
				break;
			case StageScoreManager.DataType.BonusCount_MainChara:
				bonusRate = StageAbilityManager.Instance.MainCharaBonusValueRate;
				break;
			case StageScoreManager.DataType.BonusCount_SubChara:
				bonusRate = StageAbilityManager.Instance.SubCharaBonusValueRate;
				break;
			default:
				return;
			}
			double value = (double)this.m_score.Get() * (double)bonusRate.score;
			double value2 = (double)this.m_animal.Get() * (double)bonusRate.animal;
			double value3 = (double)this.m_ring.Get() * (double)bonusRate.ring;
			double value4 = (double)distance * (double)bonusRate.distance;
			if (this.m_isFinalScore)
			{
				ObjUtil.SendMessageScoreCheck(new StageScoreData(13, (int)(bonusRate.score * 1000000f)));
				ObjUtil.SendMessageScoreCheck(new StageScoreData(14, (int)(bonusRate.ring * 1000000f)));
				ObjUtil.SendMessageScoreCheck(new StageScoreData(15, (int)(bonusRate.animal * 1000000f)));
				ObjUtil.SendMessageScoreCheck(new StageScoreData(16, (int)(bonusRate.distance * 1000000f)));
			}
			this.m_results[(int)type].score = this.GetRoundUpValue(value);
			this.m_results[(int)type].animal = this.GetRoundUpValue(value2);
			this.m_results[(int)type].ring = this.GetRoundUpValue(value3);
			this.m_results[(int)type].red_ring = 0L;
			this.m_results[(int)type].distance = this.GetRoundUpValue(value4);
		}
	}

	private void SetBonusCountRank()
	{
		int num = 8;
		float num2 = 0f;
		if (this.m_quickMode)
		{
			this.m_results[num].score = 0L;
		}
		else
		{
			uint num3 = 1u;
			if (SaveDataManager.Instance != null)
			{
				num3 = SaveDataManager.Instance.PlayerData.DisplayRank;
				if (num3 < 1u)
				{
					num3 = 1u;
				}
			}
			num2 = num3 * 0.01f;
			this.m_results[num].score = this.GetRoundUpValue((double)((float)this.m_score.Get() * num2));
		}
		if (this.m_isFinalScore)
		{
			ObjUtil.SendMessageScoreCheck(new StageScoreData(12, (int)(num2 * 1000000f)));
		}
	}

	private void SetBonusCountCampaign()
	{
		if (StageAbilityManager.Instance != null)
		{
			float num = 0f;
			float campaignValueRate = StageAbilityManager.Instance.CampaignValueRate;
			if (campaignValueRate > 0f)
			{
				num = (float)this.m_ring.Get() * campaignValueRate;
			}
			int num2 = 7;
			this.m_results[num2].ring += this.GetRoundUpValue((double)num);
			if (this.m_isFinalScore)
			{
				ObjUtil.SendMessageScoreCheck(new StageScoreData(14, (int)(campaignValueRate * 1000000f)));
			}
		}
	}

	private void SetBonusCount()
	{
		if (StageAbilityManager.Instance != null)
		{
			int[] array = new int[]
			{
				2,
				3,
				4,
				5,
				6,
				7,
				8
			};
			int num = 1;
			for (int i = 0; i < array.Length; i++)
			{
				this.m_results[num].score += this.m_results[array[i]].score;
			}
			for (int j = 0; j < array.Length; j++)
			{
				this.m_results[num].animal += this.m_results[array[j]].animal;
			}
			for (int k = 0; k < array.Length; k++)
			{
				this.m_results[num].ring += this.m_results[array[k]].ring;
			}
			for (int l = 0; l < array.Length; l++)
			{
				this.m_results[num].distance += this.m_results[array[l]].distance;
			}
			for (int m = 0; m < array.Length; m++)
			{
				this.m_results[num].sp_crystal += this.m_results[array[m]].sp_crystal;
			}
			for (int n = 0; n < array.Length; n++)
			{
				this.m_results[num].raid_boss_ring += this.m_results[array[n]].raid_boss_ring;
			}
		}
	}

	private void SetFinalCount(int distance)
	{
		int num = 1;
		int num2 = 9;
		this.m_results[num2].score = this.m_score.Get() + this.m_results[num].score;
		this.m_results[num2].animal = this.m_animal.Get() + this.m_results[num].animal;
		this.m_results[num2].ring = this.m_ring.Get() + this.m_results[num].ring;
		this.m_results[num2].red_ring = this.m_red_ring.Get();
		this.m_results[num2].distance = (long)distance + this.m_results[num].distance;
		this.m_results[num2].sp_crystal = this.m_special_crystal.Get() + this.m_results[num].sp_crystal;
		this.m_results[num2].raid_boss_ring = this.m_raid_boss_ring.Get() + this.m_results[num].raid_boss_ring;
	}

	private void SetScore()
	{
		int num = 10;
		int num2 = 9;
		this.m_results[num].score = this.m_results[num2].score * (long)this.m_scoreCoefficient;
		this.m_results[num].ring = this.m_results[num2].ring * (long)this.m_stockRingCoefficient;
		this.m_results[num].animal = this.m_results[num2].animal * (long)this.m_animalCoefficient;
		this.m_results[num].distance = this.m_results[num2].distance * (long)this.m_totaldistanceCoefficient;
	}

	private void SetMileageBonusScore()
	{
		if (StageAbilityManager.Instance != null)
		{
			StageAbilityManager.BonusRate mileageBonusScoreRate = StageAbilityManager.Instance.MileageBonusScoreRate;
			int num = 10;
			int num2 = 11;
			float num3 = (float)this.m_results[num].score * mileageBonusScoreRate.score;
			float num4 = (float)this.m_results[num].animal * mileageBonusScoreRate.animal;
			float num5 = (float)this.m_results[num].ring * mileageBonusScoreRate.ring;
			float num6 = (float)this.m_results[num].distance * mileageBonusScoreRate.distance;
			this.m_results[num2].score = this.GetRoundUpValue((double)num3);
			this.m_results[num2].animal = this.GetRoundUpValue((double)num4);
			this.m_results[num2].ring = this.GetRoundUpValue((double)num5);
			this.m_results[num2].distance = this.GetRoundUpValue((double)num6);
			if (this.m_isFinalScore)
			{
				ObjUtil.SendMessageScoreCheck(new StageScoreData(13, (int)(mileageBonusScoreRate.score * 1000000f)));
				ObjUtil.SendMessageScoreCheck(new StageScoreData(15, (int)(mileageBonusScoreRate.animal * 1000000f)));
				ObjUtil.SendMessageScoreCheck(new StageScoreData(14, (int)(mileageBonusScoreRate.ring * 1000000f)));
				ObjUtil.SendMessageScoreCheck(new StageScoreData(16, (int)(mileageBonusScoreRate.distance * 1000000f)));
			}
		}
	}

	private void SetFinalScore()
	{
		if (this.m_bossStage)
		{
			this.m_final_score.Set(0L);
		}
		else
		{
			this.m_final_score.Set(this.m_results[10].Sum() + this.m_results[11].Sum());
			if (StageAbilityManager.Instance != null)
			{
				float final_score = StageAbilityManager.Instance.MileageBonusScoreRate.final_score;
				if (final_score > 0f)
				{
					this.m_results[11].final_score = this.GetRoundUpValue((double)((float)this.m_final_score.Get() * final_score));
					this.m_final_score.Set(this.m_final_score.Get() + (long)((int)this.m_results[11].final_score));
				}
			}
			this.m_final_score.Set(this.GetTeamAbliltyResultScore(this.m_final_score.Get(), 1));
		}
	}

	private void SetCollectEventCount()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsCollectEvent())
		{
			switch (EventManager.Instance.CollectType)
			{
			case EventManager.CollectEventType.GET_ANIMALS:
				this.m_collectEventCount.Set(this.GetResultData(StageScoreManager.DataType.FinalCount).animal);
				break;
			case EventManager.CollectEventType.GET_RING:
				this.m_collectEventCount.Set(this.GetResultData(StageScoreManager.DataType.FinalCount).ring);
				break;
			case EventManager.CollectEventType.RUN_DISTANCE:
				this.m_collectEventCount.Set(this.GetResultData(StageScoreManager.DataType.FinalCount).distance);
				break;
			}
		}
	}

	private long GetRoundUpValue(double value)
	{
		return (long)Math.Ceiling(value);
	}

	private long GetTeamAbliltyResultScore(long score, int coefficient)
	{
		StageAbilityManager stageAbilityManager = StageAbilityManager.Instance;
		if (stageAbilityManager != null)
		{
			return stageAbilityManager.GetTeamAbliltyResultScore(score, coefficient);
		}
		return score * (long)coefficient;
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
	}

	public void SetupScoreRate()
	{
		this.m_rank_rate = 0f;
		if (!this.m_quickMode && SaveDataManager.Instance != null)
		{
			uint num = SaveDataManager.Instance.PlayerData.DisplayRank;
			if (num < 1u)
			{
				num = 1u;
			}
			this.m_rank_rate = num * 0.01f;
		}
	}

	private StageAbilityManager.BonusRate GetBonusRate(StageScoreManager.DataType type)
	{
		StageAbilityManager.BonusRate result = default(StageAbilityManager.BonusRate);
		switch (type)
		{
		case StageScoreManager.DataType.BonusCount_MainChao:
			result = StageAbilityManager.Instance.MainChaoBonusValueRate;
			break;
		case StageScoreManager.DataType.BonusCount_SubChao:
			result = StageAbilityManager.Instance.SubChaoBonusValueRate;
			break;
		case StageScoreManager.DataType.BonusCount_ChaoCount:
			result = StageAbilityManager.Instance.CountChaoBonusValueRate;
			break;
		case StageScoreManager.DataType.BonusCount_MainChara:
			result = StageAbilityManager.Instance.MainCharaBonusValueRate;
			break;
		case StageScoreManager.DataType.BonusCount_SubChara:
			result = StageAbilityManager.Instance.SubCharaBonusValueRate;
			break;
		}
		return result;
	}

	public long GetRealtimeScore()
	{
		long num = this.m_score.Get();
		long num2 = this.m_animal.Get();
		long num3 = this.m_ring.Get();
		long num4 = (long)this.m_information.TotalDistance;
		long num5 = num + num2 + num3 + num4;
		if (this.m_realtime_score_back == num5)
		{
			return this.m_realtime_score_old;
		}
		this.m_realtime_score_back = num5;
		if (StageAbilityManager.Instance != null)
		{
			double num6 = (double)this.m_score.Get();
			num += this.GetRoundUpValue(num6 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChao).score);
			num += this.GetRoundUpValue(num6 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChao).score);
			num += this.GetRoundUpValue(num6 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_ChaoCount).score);
			num += this.GetRoundUpValue(num6 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChara).score);
			num += this.GetRoundUpValue(num6 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChara).score);
			num += this.GetRoundUpValue(num6 * (double)this.m_rank_rate);
			num *= (long)this.m_scoreCoefficient;
			double num7 = (double)this.m_animal.Get();
			num2 += this.GetRoundUpValue(num7 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChao).animal);
			num2 += this.GetRoundUpValue(num7 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChao).animal);
			num2 += this.GetRoundUpValue(num7 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChara).animal);
			num2 += this.GetRoundUpValue(num7 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChara).animal);
			num2 *= (long)this.m_animalCoefficient;
			double num8 = (double)this.m_ring.Get();
			num3 += this.GetRoundUpValue(num8 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChao).ring);
			num3 += this.GetRoundUpValue(num8 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChao).ring);
			num3 += this.GetRoundUpValue(num8 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChara).ring);
			num3 += this.GetRoundUpValue(num8 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChara).ring);
			num3 += this.GetRoundUpValue(num8 * (double)StageAbilityManager.Instance.CampaignValueRate);
			num3 *= (long)this.m_stockRingCoefficient;
			double num9 = (double)num4;
			num4 += this.GetRoundUpValue(num9 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChao).distance);
			num4 += this.GetRoundUpValue(num9 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChao).distance);
			num4 += this.GetRoundUpValue(num9 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChara).distance);
			num4 += this.GetRoundUpValue(num9 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChara).distance);
			num4 *= (long)this.m_totaldistanceCoefficient;
		}
		long num10 = 0L;
		num10 += num;
		num10 += num2;
		num10 += num3;
		num10 += num4;
		this.m_realtime_score_old = this.GetTeamAbliltyResultScore(num10, 1);
		return this.m_realtime_score_old;
	}

	public long GetRealtimeEventScore()
	{
		long num = this.m_special_crystal.Get();
		if (this.m_event_score_back == num)
		{
			return this.m_event_score_old;
		}
		this.m_realtime_score_back = num;
		if (StageAbilityManager.Instance != null)
		{
			double num2 = (double)this.m_special_crystal.Get();
			num += this.GetRoundUpValue(num2 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChao).sp_crystal);
			num += this.GetRoundUpValue(num2 * (double)this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChao).sp_crystal);
		}
		this.m_event_score_old = num;
		return num;
	}

	public long GetRealtimeEventAnimal()
	{
		long num = this.m_animal.Get();
		if (this.m_animal_back == num)
		{
			return this.m_animal_old;
		}
		this.m_animal_back = num;
		if (StageAbilityManager.Instance != null)
		{
			float num2 = (float)this.m_animal.Get();
			num += this.GetRoundUpValue((double)(num2 * this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChao).animal));
			num += this.GetRoundUpValue((double)(num2 * this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChao).animal));
			num += this.GetRoundUpValue((double)(num2 * this.GetBonusRate(StageScoreManager.DataType.BonusCount_MainChara).animal));
			num += this.GetRoundUpValue((double)(num2 * this.GetBonusRate(StageScoreManager.DataType.BonusCount_SubChara).animal));
		}
		this.m_animal_old = num;
		return num;
	}
}
