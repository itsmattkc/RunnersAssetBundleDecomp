using DataTable;
using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageAbilityManager : MonoBehaviour
{
	public struct BonusRate
	{
		public float score;

		public float animal;

		public float ring;

		public float red_ring;

		public float distance;

		public float sp_crystal;

		public float raid_boss_ring;

		public float final_score;

		public void Reset()
		{
			this.score = 0f;
			this.animal = 0f;
			this.ring = 0f;
			this.red_ring = 0f;
			this.distance = 0f;
			this.sp_crystal = 0f;
			this.raid_boss_ring = 0f;
			this.final_score = 0f;
		}
	}

	private class ChaoAbilityInfo
	{
		public class AbilityData
		{
			public ChaoAbility ability = ChaoAbility.UNKNOWN;

			public float bonus;

			public float normal;

			public float extra;
		}

		public List<StageAbilityManager.ChaoAbilityInfo.AbilityData> abilityDatas = new List<StageAbilityManager.ChaoAbilityInfo.AbilityData>();

		public CharacterAttribute attribute = CharacterAttribute.UNKNOWN;

		public void Init()
		{
			this.abilityDatas.Clear();
			this.attribute = CharacterAttribute.UNKNOWN;
		}

		public void AddAbility(ChaoAbility ability, float bonus, float normal, float extra)
		{
			StageAbilityManager.ChaoAbilityInfo.AbilityData abilityData = new StageAbilityManager.ChaoAbilityInfo.AbilityData();
			abilityData.ability = ability;
			abilityData.bonus = bonus;
			abilityData.normal = normal;
			abilityData.extra = extra;
			this.abilityDatas.Add(abilityData);
		}
	}

	private enum ResType
	{
		CHAO,
		CHARA,
		NUM
	}

	private const float CHARA_ABILITY_BONUS_VALUE = 20f;

	[Header("debugFlag にチェックを入れると、指定した値で設定できます"), SerializeField]
	private bool m_debugFlag;

	[SerializeField]
	private float m_debugCampaignBonusValue;

	private bool[] m_boostItemValidFlag = new bool[3];

	private StageAbilityManager.ChaoAbilityInfo m_mainChaoInfo = new StageAbilityManager.ChaoAbilityInfo();

	private StageAbilityManager.ChaoAbilityInfo m_subChaoInfo = new StageAbilityManager.ChaoAbilityInfo();

	private float[] m_mainCharaAbilityValue = new float[10];

	private float[] m_subCharaAbilityValue = new float[10];

	private float[] m_mainCharaOverlapBonus = new float[5];

	private float[] m_subCharaOverlapBonus = new float[5];

	private float[] m_mainTeamAbilityBonusValue = new float[6];

	private float[] m_subTeamAbilityBonusValue = new float[6];

	private TeamAttributeCategory m_mainTeamAttributeCategory = TeamAttributeCategory.NONE;

	private TeamAttributeCategory m_subTeamAttributeCategory = TeamAttributeCategory.NONE;

	private float m_boostBonusValue;

	private float m_campaignBonusValue;

	private StageAbilityManager.BonusRate m_count_chao_bonus_value_rate;

	private StageAbilityManager.BonusRate m_main_chao_bonus_value_rate;

	private StageAbilityManager.BonusRate m_sub_chao_bonus_value_rate;

	private StageAbilityManager.BonusRate m_main_chara_bonus_value_rate;

	private StageAbilityManager.BonusRate m_sub_chara_bonus_value_rate;

	private StageAbilityManager.BonusRate m_bonus_value_rate;

	private StageAbilityManager.BonusRate m_mileage_bonus_score_rate;

	private PlayerInformation m_playerInformation;

	private float m_chaoCountBonus;

	private int m_chaoCount;

	private int m_getMainChaoRecoveryRingCount;

	private int m_getSubChaoRecoveryRingCount;

	private bool m_initFlag;

	private static StageAbilityManager instance = null;

	private static string CHAODATA_SCENENAME = "ChaoDataTable";

	private static string CHAODATA_NAME = "ChaoTable";

	private static string CHARADATA_SCENENAME = "CharaAbilityDataTable";

	private static string CHARADATA_NAME = "ImportAbilityTable";

	private static readonly List<ResourceSceneLoader.ResourceInfo> m_loadInfo = new List<ResourceSceneLoader.ResourceInfo>
	{
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, StageAbilityManager.CHAODATA_SCENENAME, true, false, true, StageAbilityManager.CHAODATA_NAME, false),
		new ResourceSceneLoader.ResourceInfo(ResourceCategory.ETC, StageAbilityManager.CHARADATA_SCENENAME, true, false, true, StageAbilityManager.CHARADATA_NAME, false)
	};

	public static StageAbilityManager Instance
	{
		get
		{
			return StageAbilityManager.instance;
		}
	}

	public StageAbilityManager.BonusRate BonusValueRate
	{
		get
		{
			return this.m_bonus_value_rate;
		}
	}

	public StageAbilityManager.BonusRate MainChaoBonusValueRate
	{
		get
		{
			return this.m_main_chao_bonus_value_rate;
		}
	}

	public StageAbilityManager.BonusRate SubChaoBonusValueRate
	{
		get
		{
			return this.m_sub_chao_bonus_value_rate;
		}
	}

	public StageAbilityManager.BonusRate CountChaoBonusValueRate
	{
		get
		{
			return this.m_count_chao_bonus_value_rate;
		}
	}

	public StageAbilityManager.BonusRate MainCharaBonusValueRate
	{
		get
		{
			return this.m_main_chara_bonus_value_rate;
		}
	}

	public StageAbilityManager.BonusRate SubCharaBonusValueRate
	{
		get
		{
			return this.m_sub_chara_bonus_value_rate;
		}
	}

	public float CampaignValueRate
	{
		get
		{
			return this.m_campaignBonusValue;
		}
	}

	public StageAbilityManager.BonusRate MileageBonusScoreRate
	{
		get
		{
			return this.m_mileage_bonus_score_rate;
		}
	}

	public float SpecialCrystalRate
	{
		get
		{
			return this.m_bonus_value_rate.sp_crystal;
		}
	}

	public float RadBossRingRate
	{
		get
		{
			return this.m_bonus_value_rate.raid_boss_ring;
		}
	}

	public float[] CharaAbility
	{
		get
		{
			this.CheckPlayerInformation();
			if (!(this.m_playerInformation != null))
			{
				return this.m_mainCharaAbilityValue;
			}
			if (this.m_playerInformation.PlayingCharaType == PlayingCharacterType.MAIN)
			{
				return this.m_mainCharaAbilityValue;
			}
			return this.m_subCharaAbilityValue;
		}
	}

	public bool[] BoostItemValidFlag
	{
		get
		{
			return this.m_boostItemValidFlag;
		}
		set
		{
			this.m_boostItemValidFlag = value;
		}
	}

	public void RequestPlayChaoEffect(ChaoAbility ability)
	{
		this.PlayChaoEffect(ability);
	}

	public void RequestPlayChaoEffect(ChaoAbility ability, ChaoType chaoType)
	{
		this.PlayChaoEffect(ability, chaoType);
	}

	public void RequestPlayChaoEffect(ChaoAbility[] abilities)
	{
		for (int i = 0; i < abilities.Length; i++)
		{
			ChaoAbility ability = abilities[i];
			this.PlayChaoEffect(ability);
		}
	}

	public void RequestPlayChaoEffect(ChaoAbility[] abilities, ChaoType chaoType)
	{
		for (int i = 0; i < abilities.Length; i++)
		{
			ChaoAbility ability = abilities[i];
			this.PlayChaoEffect(ability, chaoType);
		}
	}

	public void RequestStopChaoEffect(ChaoAbility ability)
	{
		this.StopChaoEffect(ability);
	}

	public void RequestStopChaoEffect(ChaoAbility[] abilities)
	{
		for (int i = 0; i < abilities.Length; i++)
		{
			ChaoAbility ability = abilities[i];
			this.StopChaoEffect(ability);
		}
	}

	public bool HasChaoAbility(ChaoAbility ability)
	{
		return ability != ChaoAbility.UNKNOWN && ability < ChaoAbility.NUM && (this.HasChaoAbility(this.m_mainChaoInfo, ability) || this.HasChaoAbility(this.m_subChaoInfo, ability));
	}

	public bool HasChaoAbility(ChaoAbility ability, ChaoType type)
	{
		if (ability != ChaoAbility.UNKNOWN && ability < ChaoAbility.NUM)
		{
			if (type == ChaoType.MAIN)
			{
				return this.HasChaoAbility(this.m_mainChaoInfo, ability);
			}
			if (type == ChaoType.SUB)
			{
				return this.HasChaoAbility(this.m_subChaoInfo, ability);
			}
		}
		return false;
	}

	private bool HasChaoAbility(StageAbilityManager.ChaoAbilityInfo info, ChaoAbility ability)
	{
		foreach (StageAbilityManager.ChaoAbilityInfo.AbilityData current in info.abilityDatas)
		{
			if (current.ability == ability)
			{
				return true;
			}
		}
		return false;
	}

	public int GetChaoAbliltyValue(ChaoAbility ability, int src_value)
	{
		if (ability < ChaoAbility.NUM)
		{
			return (int)this.CalcPlusAbliltyBonusValue(ability, (float)src_value);
		}
		return 0;
	}

	public float GetChaoAbliltyValue(ChaoAbility ability, float src_value)
	{
		if (ability < ChaoAbility.NUM)
		{
			return this.CalcPlusAbliltyBonusValue(ability, src_value);
		}
		return 0f;
	}

	public float GetChaoAbilityValue(ChaoAbility ability)
	{
		float chaoAbilityValue = this.GetChaoAbilityValue(this.m_mainChaoInfo, ability);
		return chaoAbilityValue + this.GetChaoAbilityValue(this.m_subChaoInfo, ability);
	}

	public float GetChaoAbilityValue(ChaoAbility ability, ChaoType type)
	{
		if (type == ChaoType.MAIN)
		{
			return this.GetChaoAbilityValue(this.m_mainChaoInfo, ability);
		}
		if (type == ChaoType.SUB)
		{
			return this.GetChaoAbilityValue(this.m_subChaoInfo, ability);
		}
		return 0f;
	}

	private float GetChaoAbilityValue(StageAbilityManager.ChaoAbilityInfo info, ChaoAbility ability)
	{
		foreach (StageAbilityManager.ChaoAbilityInfo.AbilityData current in info.abilityDatas)
		{
			if (current.ability == ability)
			{
				return (!this.IsSameAttribute(info.attribute)) ? current.normal : current.bonus;
			}
		}
		return 0f;
	}

	public float GetChaoAbilityExtraValue(ChaoAbility ability)
	{
		return this.GetChaoAbilityExtraValue(ability, ChaoType.MAIN) + this.GetChaoAbilityExtraValue(ability, ChaoType.SUB);
	}

	public float GetChaoAbilityExtraValue(ChaoAbility ability, ChaoType type)
	{
		StageAbilityManager.ChaoAbilityInfo chaoAbilityInfo = null;
		if (type == ChaoType.MAIN)
		{
			chaoAbilityInfo = this.m_mainChaoInfo;
		}
		else if (type == ChaoType.SUB)
		{
			chaoAbilityInfo = this.m_subChaoInfo;
		}
		if (chaoAbilityInfo != null)
		{
			foreach (StageAbilityManager.ChaoAbilityInfo.AbilityData current in chaoAbilityInfo.abilityDatas)
			{
				if (current.ability == ability)
				{
					return current.extra;
				}
			}
		}
		return 0f;
	}

	public float GetCharacterOverlapBonusValue(OverlapBonusType bonusType)
	{
		if (OverlapBonusType.SCORE <= bonusType && bonusType < OverlapBonusType.NUM)
		{
			return this.m_mainCharaOverlapBonus[(int)bonusType] + this.m_subCharaOverlapBonus[(int)bonusType];
		}
		return 0f;
	}

	public float GetCharacterOverlapBonusValue(OverlapBonusType bonusType, bool mainChara)
	{
		if (OverlapBonusType.SCORE > bonusType || bonusType >= OverlapBonusType.NUM)
		{
			return 0f;
		}
		if (mainChara)
		{
			return this.m_mainCharaOverlapBonus[(int)bonusType];
		}
		return this.m_subCharaOverlapBonus[(int)bonusType];
	}

	public int GetChaoAndTeamAbliltyScoreValue(List<ChaoAbility> abilityList, TeamAttributeBonusType bonusType, int src_value)
	{
		float num = 0f;
		int count = abilityList.Count;
		for (int i = 0; i < count; i++)
		{
			num += this.GetChaoAbilityValue(this.m_mainChaoInfo, abilityList[i]);
			num += this.GetChaoAbilityValue(this.m_subChaoInfo, abilityList[i]);
		}
		num += this.GetTeamAbilityBonusValue(bonusType);
		return (int)this.GetPlusPercentBonusValue(num, (float)src_value);
	}

	public int GetChaoAndEnemyScoreValue(List<ChaoAbility> abilityList, int src_value)
	{
		float num = 0f;
		int count = abilityList.Count;
		for (int i = 0; i < count; i++)
		{
			num += this.GetChaoAbilityValue(this.m_mainChaoInfo, abilityList[i]);
			num += this.GetChaoAbilityValue(this.m_subChaoInfo, abilityList[i]);
		}
		num += this.GetTeamAbilityBonusValue(TeamAttributeBonusType.ENEMY);
		num += this.GetCharacterOverlapBonusValue(OverlapBonusType.ENEMY);
		return (int)this.GetPlusPercentBonusValue(num, (float)src_value);
	}

	private float GetTeamAbilityBonusValue(TeamAttributeBonusType bonusType)
	{
		if (bonusType < TeamAttributeBonusType.NUM && bonusType != TeamAttributeBonusType.NONE)
		{
			return this.m_mainTeamAbilityBonusValue[(int)bonusType] + this.m_subTeamAbilityBonusValue[(int)bonusType];
		}
		return 0f;
	}

	public long GetTeamAbliltyResultScore(long score, int coefficient)
	{
		float num = 1f;
		int num2 = 0;
		if (this.m_mainTeamAttributeCategory == TeamAttributeCategory.EASY_SPEED)
		{
			num2++;
		}
		if (this.m_subTeamAttributeCategory == TeamAttributeCategory.EASY_SPEED)
		{
			num2++;
		}
		if (num2 == 1)
		{
			num = 0.8f;
		}
		else if (num2 == 2)
		{
			num = 0.64f;
		}
		double num3 = (double)coefficient * (double)num;
		double num4 = (double)score * num3;
		long num5 = (long)num4;
		if ((double)num5 < num4)
		{
			num4 += 1.0;
		}
		return (long)num4;
	}

	public bool IsEasySpeed(PlayingCharacterType type)
	{
		if (type == PlayingCharacterType.MAIN)
		{
			if (this.m_mainTeamAttributeCategory == TeamAttributeCategory.EASY_SPEED)
			{
				return true;
			}
		}
		else if (type == PlayingCharacterType.SUB && this.m_subTeamAttributeCategory == TeamAttributeCategory.EASY_SPEED)
		{
			return true;
		}
		return false;
	}

	public float GetTeamAbliltyTimeScale(float timeScale)
	{
		float num = (this.m_mainTeamAbilityBonusValue[5] + this.m_subTeamAbilityBonusValue[5]) * 0.01f;
		return timeScale - num;
	}

	public float GetItemTimePlusAblityBonus(ItemType itemType)
	{
		return this.CalcItemPlusAbliltyBonusValue(itemType);
	}

	public float GetChaoCountBonusValue()
	{
		return this.m_chaoCountBonus;
	}

	public int GetChaoCount()
	{
		return this.m_chaoCount;
	}

	public void RecalcAbilityVaue()
	{
		this.InitParam();
		this.CalcChaoCountBonus();
		this.SetCharacterAbility();
		this.SetCharacterOverlapBonus();
		this.SetTeamAbility();
		this.SetChaoAbility();
		this.SetChaoBonusValueRate();
		this.SetCharacterBonusValueRate();
		this.SetPampaignBonusValueRate();
	}

	public GameObject GetLostRingChao()
	{
		GameObject result = null;
		if (this.HasChaoAbility(ChaoAbility.RECOVERY_RING))
		{
			bool flag = true;
			bool flag2 = false;
			int num = (int)this.GetChaoAbilityValue(this.m_mainChaoInfo, ChaoAbility.RECOVERY_RING);
			int num2 = (int)this.GetChaoAbilityValue(this.m_subChaoInfo, ChaoAbility.RECOVERY_RING);
			if (this.m_getMainChaoRecoveryRingCount < num)
			{
				flag2 = true;
			}
			else if (this.m_getSubChaoRecoveryRingCount < num2)
			{
				flag2 = true;
				flag = false;
			}
			string b = (!flag) ? "SubChao" : "MainChao";
			if (flag2)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Chao");
				GameObject[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					GameObject gameObject = array2[i];
					if (gameObject.name == b)
					{
						result = gameObject;
						break;
					}
				}
			}
		}
		return result;
	}

	public void SetLostRingCount(int ring)
	{
		bool flag = false;
		int num = 0;
		int num2 = (int)this.GetChaoAbilityValue(this.m_mainChaoInfo, ChaoAbility.RECOVERY_RING);
		int num3 = (int)this.GetChaoAbilityValue(this.m_subChaoInfo, ChaoAbility.RECOVERY_RING);
		if (this.m_getMainChaoRecoveryRingCount < num2)
		{
			int num4 = num2 - this.m_getMainChaoRecoveryRingCount;
			if (num4 < ring)
			{
				num = num4;
				ring -= num4;
			}
			else
			{
				num = ring;
				ring = 0;
			}
			this.m_getMainChaoRecoveryRingCount += num;
			flag = true;
		}
		if (ring > 0 && this.m_getSubChaoRecoveryRingCount < num3)
		{
			int num5 = num3 - this.m_getSubChaoRecoveryRingCount;
			if (num5 < ring)
			{
				num += num5;
				this.m_getSubChaoRecoveryRingCount += num5;
			}
			else
			{
				num += ring;
				this.m_getSubChaoRecoveryRingCount += ring;
			}
			flag = true;
		}
		if (flag)
		{
			MsgAddStockRing value = new MsgAddStockRing(num);
			GameObjectUtil.SendDelayedMessageFindGameObject("HudCockpit", "OnAddStockRing", value);
			if (StageScoreManager.Instance != null)
			{
				StageScoreManager.Instance.AddRecoveryRingCount(num);
			}
		}
	}

	private void Setup()
	{
		this.RecalcAbilityVaue();
		this.SetMileageBonusScoreRate();
	}

	protected void Awake()
	{
		base.tag = "Manager";
		this.SetInstance();
	}

	private void Start()
	{
		this.InitParam();
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (child != null)
			{
				GameObject gameObject = child.gameObject;
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
		for (int j = 0; j < 3; j++)
		{
			this.m_boostItemValidFlag[j] = false;
		}
	}

	private void OnDestroy()
	{
		if (StageAbilityManager.instance == this)
		{
			StageAbilityManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (StageAbilityManager.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			StageAbilityManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void InitParam()
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_mainTeamAbilityBonusValue[i] = 0f;
			this.m_subTeamAbilityBonusValue[i] = 0f;
		}
		for (int j = 0; j < 10; j++)
		{
			this.m_mainCharaAbilityValue[j] = 0f;
			this.m_subCharaAbilityValue[j] = 0f;
		}
		for (int k = 0; k < 5; k++)
		{
			this.m_mainCharaOverlapBonus[k] = 0f;
			this.m_subCharaOverlapBonus[k] = 0f;
		}
		this.m_mainTeamAttributeCategory = TeamAttributeCategory.NONE;
		this.m_subTeamAttributeCategory = TeamAttributeCategory.NONE;
		this.m_playerInformation = null;
		this.m_mainChaoInfo.Init();
		this.m_subChaoInfo.Init();
		this.m_chaoCountBonus = 0f;
		this.m_chaoCount = 0;
		this.m_count_chao_bonus_value_rate.Reset();
		this.m_main_chao_bonus_value_rate.Reset();
		this.m_sub_chao_bonus_value_rate.Reset();
		this.m_main_chara_bonus_value_rate.Reset();
		this.m_sub_chara_bonus_value_rate.Reset();
		this.m_bonus_value_rate.Reset();
		this.m_campaignBonusValue = 0f;
		this.m_boostBonusValue = 0f;
		if (!this.m_initFlag)
		{
			this.m_mileage_bonus_score_rate.Reset();
		}
		this.m_getMainChaoRecoveryRingCount = 0;
		this.m_getSubChaoRecoveryRingCount = 0;
		this.m_initFlag = true;
	}

	private void SetCharacterAbility()
	{
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager != null)
		{
			CharaType mainChara = saveDataManager.PlayerData.MainChara;
			if (this.CheckCharaType(mainChara))
			{
				this.SetCharaAbilityValue(ref this.m_mainCharaAbilityValue, saveDataManager.CharaData.AbilityArray[(int)mainChara]);
			}
			if (this.m_boostItemValidFlag[2])
			{
				CharaType subChara = saveDataManager.PlayerData.SubChara;
				if (this.CheckCharaType(subChara))
				{
					this.SetCharaAbilityValue(ref this.m_subCharaAbilityValue, saveDataManager.CharaData.AbilityArray[(int)subChara]);
				}
			}
		}
	}

	private void SetTeamAbility()
	{
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager != null && CharacterDataNameInfo.Instance != null)
		{
			CharaType mainChara = saveDataManager.PlayerData.MainChara;
			if (this.CheckCharaType(mainChara))
			{
				this.SetTeamAbilityBonusValue(ref this.m_mainTeamAbilityBonusValue, ref this.m_mainTeamAttributeCategory, CharacterDataNameInfo.Instance.GetDataByID(mainChara));
			}
			if (this.m_boostItemValidFlag[2])
			{
				CharaType subChara = saveDataManager.PlayerData.SubChara;
				if (this.CheckCharaType(subChara))
				{
					this.SetTeamAbilityBonusValue(ref this.m_subTeamAbilityBonusValue, ref this.m_subTeamAttributeCategory, CharacterDataNameInfo.Instance.GetDataByID(subChara));
				}
			}
		}
	}

	private void SetCharacterOverlapBonus()
	{
		if (ResourceManager.Instance != null)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ETC, "OverlapBonusTable");
			if (gameObject != null)
			{
				OverlapBonusTable component = gameObject.GetComponent<OverlapBonusTable>();
				if (component != null && SaveDataManager.Instance != null)
				{
					CharaType mainChara = SaveDataManager.Instance.PlayerData.MainChara;
					if (this.CheckCharaType(mainChara))
					{
						this.SetOverlapBonusValue(ref this.m_mainCharaOverlapBonus, mainChara, component);
					}
					if (this.m_boostItemValidFlag[2])
					{
						CharaType subChara = SaveDataManager.Instance.PlayerData.SubChara;
						if (this.CheckCharaType(subChara))
						{
							this.SetOverlapBonusValue(ref this.m_subCharaOverlapBonus, subChara, component);
						}
					}
				}
			}
		}
	}

	private void SetOverlapBonusValue(ref float[] overlapBonus, CharaType charaType, OverlapBonusTable overlapBonusTable)
	{
		if (ServerInterface.PlayerState != null && overlapBonusTable != null)
		{
			ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(charaType);
			if (serverCharacterState != null)
			{
				int star = serverCharacterState.star;
				if (overlapBonus.Length == 5)
				{
					for (int i = 0; i < 5; i++)
					{
						overlapBonus[i] = overlapBonusTable.GetStarBonusList(charaType, star, (OverlapBonusType)i);
					}
				}
			}
		}
	}

	private bool CheckCharaType(CharaType chara_type)
	{
		return CharaType.SONIC <= chara_type && chara_type < CharaType.NUM;
	}

	private void SetCharaAbilityValue(ref float[] ability_value, CharaAbility ability)
	{
		if (ability != null)
		{
			ImportAbilityTable importAbilityTable = ImportAbilityTable.GetInstance();
			if (importAbilityTable != null)
			{
				for (int i = 0; i < 10; i++)
				{
					ability_value[i] = importAbilityTable.GetAbilityPotential((AbilityType)i, (int)ability.Ability[i]);
				}
			}
		}
	}

	private void SetTeamAbilityBonusValue(ref float[] bonusValue, ref TeamAttributeCategory category, CharacterDataNameInfo.Info info)
	{
		for (int i = 0; i < 6; i++)
		{
			bonusValue[i] = 0f;
		}
		if (info != null)
		{
			category = info.m_teamAttributeCategory;
			if (info.m_mainAttributeBonus != TeamAttributeBonusType.NONE && info.m_mainAttributeBonus < TeamAttributeBonusType.NUM)
			{
				bonusValue[(int)info.m_mainAttributeBonus] = info.TeamAttributeValue;
			}
			if (info.m_subAttributeBonus != TeamAttributeBonusType.NONE && info.m_subAttributeBonus < TeamAttributeBonusType.NUM)
			{
				bonusValue[(int)info.m_subAttributeBonus] = info.TeamAttributeSubValue;
			}
		}
	}

	private void SetChaoAbility()
	{
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager != null)
		{
			bool mainChaoFlag = true;
			this.SetChaoAbilityData(saveDataManager.PlayerData.MainChaoID, mainChaoFlag);
			bool mainChaoFlag2 = false;
			this.SetChaoAbilityData(saveDataManager.PlayerData.SubChaoID, mainChaoFlag2);
		}
	}

	private void SetChaoAbilityData(int chaoId, bool mainChaoFlag)
	{
		ChaoData chaoData = ChaoTable.GetChaoData(chaoId);
		if (chaoData != null)
		{
			int level = chaoData.level;
			if (level >= 0)
			{
				int abilityNum = chaoData.abilityNum;
				for (int i = 0; i < abilityNum; i++)
				{
					chaoData.currentAbility = i;
					bool flag = false;
					int eventId = chaoData.eventId;
					if (eventId > 0)
					{
						if (EventManager.IsVaildEvent(eventId) && EventManager.Instance != null)
						{
							ChaoAbility chaoAbility = chaoData.chaoAbility;
							if (chaoAbility != ChaoAbility.SPECIAL_CRYSTAL_COUNT)
							{
								if (chaoAbility != ChaoAbility.RAID_BOSS_RING_COUNT)
								{
									if (chaoAbility == ChaoAbility.COMBO_ALL_SPECIAL_CRYSTAL || chaoAbility == ChaoAbility.SPECIAL_CRYSTAL_RATE)
									{
										goto IL_97;
									}
									if (chaoAbility != ChaoAbility.AGGRESSIVITY_UP_FOR_RAID_BOSS)
									{
										goto IL_C5;
									}
								}
								if (EventManager.Instance.IsRaidBossStage())
								{
									flag = true;
								}
								goto IL_C5;
							}
							IL_97:
							if (EventManager.Instance.IsSpecialStage())
							{
								flag = true;
							}
						}
						IL_C5:;
					}
					else
					{
						flag = true;
						if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
						{
							ChaoAbility chaoAbility = chaoData.chaoAbility;
							if (chaoAbility == ChaoAbility.RING_COUNT || chaoAbility == ChaoAbility.COMBO_SUPER_RING || chaoAbility == ChaoAbility.BOSS_SUPER_RING_RATE || chaoAbility == ChaoAbility.RECOVERY_RING)
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						float num = chaoData.bonusAbilityValue[level];
						float num2 = chaoData.abilityValue[level];
						float extraValue = chaoData.extraValue;
						if (chaoData.chaoAbility == ChaoAbility.RECOVERY_RING)
						{
							num = Mathf.Ceil(num);
							num2 = Mathf.Ceil(num2);
						}
						if (mainChaoFlag)
						{
							this.m_mainChaoInfo.AddAbility(chaoData.chaoAbility, num, num2, extraValue);
							this.m_mainChaoInfo.attribute = chaoData.charaAtribute;
						}
						else
						{
							this.m_subChaoInfo.AddAbility(chaoData.chaoAbility, num, num2, extraValue);
							this.m_subChaoInfo.attribute = chaoData.charaAtribute;
						}
					}
				}
				chaoData.currentAbility = 0;
			}
		}
	}

	private void SetBonusVale(ref StageAbilityManager.BonusRate bonusRate, StageAbilityManager.ChaoAbilityInfo info)
	{
		foreach (StageAbilityManager.ChaoAbilityInfo.AbilityData current in info.abilityDatas)
		{
			float num = 0f;
			switch (current.ability)
			{
			case ChaoAbility.ALL_BONUS_COUNT:
			case ChaoAbility.SCORE_COUNT:
			case ChaoAbility.RING_COUNT:
			case ChaoAbility.RED_RING_COUNT:
			case ChaoAbility.ANIMAL_COUNT:
			case ChaoAbility.RUNNIGN_DISTANCE:
			case ChaoAbility.SPECIAL_CRYSTAL_COUNT:
			case ChaoAbility.RAID_BOSS_RING_COUNT:
				num = ((!this.IsSameAttributeFromSaveData(info.attribute, true)) ? current.normal : current.bonus);
				num *= 0.01f;
				break;
			}
			switch (current.ability)
			{
			case ChaoAbility.ALL_BONUS_COUNT:
				bonusRate.score += num;
				bonusRate.ring += num;
				bonusRate.animal += num;
				bonusRate.distance += num;
				break;
			case ChaoAbility.SCORE_COUNT:
				bonusRate.score += num;
				break;
			case ChaoAbility.RING_COUNT:
				bonusRate.ring += num;
				break;
			case ChaoAbility.ANIMAL_COUNT:
				bonusRate.animal += num;
				break;
			case ChaoAbility.RUNNIGN_DISTANCE:
				bonusRate.distance += num;
				break;
			case ChaoAbility.SPECIAL_CRYSTAL_COUNT:
				bonusRate.sp_crystal += num;
				break;
			case ChaoAbility.RAID_BOSS_RING_COUNT:
				bonusRate.raid_boss_ring += num;
				break;
			}
		}
	}

	private bool IsSameAttribute(CharacterAttribute chaoAtribute)
	{
		this.CheckPlayerInformation();
		return this.m_playerInformation != null && this.m_playerInformation.PlayerAttribute == chaoAtribute;
	}

	private bool IsSameAttributeFromSaveData(CharacterAttribute attribute, bool subCharaCompare)
	{
		CharaType charaType = CharaType.UNKNOWN;
		CharaType charaType2 = CharaType.UNKNOWN;
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager != null)
		{
			charaType = saveDataManager.PlayerData.MainChara;
			if (this.m_boostItemValidFlag[2])
			{
				charaType2 = saveDataManager.PlayerData.SubChara;
			}
		}
		return this.IsSameCharaAbility(charaType, attribute) || (subCharaCompare && charaType2 != CharaType.UNKNOWN && this.IsSameCharaAbility(charaType2, attribute));
	}

	private bool IsSameCharaAbility(CharaType charaType, CharacterAttribute chaoAttribute)
	{
		CharacterAttribute characterAttribute = CharaTypeUtil.GetCharacterAttribute(charaType);
		return characterAttribute == chaoAttribute && chaoAttribute != CharacterAttribute.UNKNOWN;
	}

	private void SetChaoBonusValueRate()
	{
		this.m_count_chao_bonus_value_rate.score = this.m_chaoCountBonus * 0.01f;
		this.SetBonusVale(ref this.m_main_chao_bonus_value_rate, this.m_mainChaoInfo);
		this.SetBonusVale(ref this.m_sub_chao_bonus_value_rate, this.m_subChaoInfo);
		this.m_bonus_value_rate.score = this.m_bonus_value_rate.score + this.m_count_chao_bonus_value_rate.score;
		this.m_bonus_value_rate.score = this.m_bonus_value_rate.score + (this.m_main_chao_bonus_value_rate.score + this.m_sub_chao_bonus_value_rate.score);
		this.m_bonus_value_rate.animal = this.m_bonus_value_rate.animal + (this.m_main_chao_bonus_value_rate.animal + this.m_sub_chao_bonus_value_rate.animal);
		this.m_bonus_value_rate.ring = this.m_bonus_value_rate.ring + (this.m_main_chao_bonus_value_rate.ring + this.m_sub_chao_bonus_value_rate.ring);
		this.m_bonus_value_rate.red_ring = 0f;
		this.m_bonus_value_rate.distance = this.m_bonus_value_rate.distance + (this.m_main_chao_bonus_value_rate.distance + this.m_sub_chao_bonus_value_rate.distance);
		this.m_bonus_value_rate.sp_crystal = this.m_bonus_value_rate.sp_crystal + (this.m_main_chao_bonus_value_rate.sp_crystal + this.m_sub_chao_bonus_value_rate.sp_crystal);
		this.m_bonus_value_rate.raid_boss_ring = this.m_bonus_value_rate.raid_boss_ring + (this.m_main_chao_bonus_value_rate.raid_boss_ring + this.m_sub_chao_bonus_value_rate.raid_boss_ring);
	}

	private void SetCharacterBonusValueRate()
	{
		this.m_main_chara_bonus_value_rate.score = (this.m_mainTeamAbilityBonusValue[1] + this.m_mainCharaOverlapBonus[0]) * 0.01f;
		this.m_sub_chara_bonus_value_rate.score = (this.m_subTeamAbilityBonusValue[1] + this.m_subCharaOverlapBonus[0]) * 0.01f;
		this.m_main_chara_bonus_value_rate.animal = (this.m_mainCharaAbilityValue[6] + this.m_mainTeamAbilityBonusValue[3] + this.m_mainCharaOverlapBonus[2]) * 0.01f;
		this.m_sub_chara_bonus_value_rate.animal = (this.m_subCharaAbilityValue[6] + this.m_subTeamAbilityBonusValue[3] + this.m_subCharaOverlapBonus[2]) * 0.01f;
		this.m_main_chara_bonus_value_rate.ring = (this.m_mainCharaAbilityValue[3] + this.m_mainTeamAbilityBonusValue[2] + this.m_mainCharaOverlapBonus[1]) * 0.01f;
		this.m_sub_chara_bonus_value_rate.ring = (this.m_subCharaAbilityValue[3] + this.m_subTeamAbilityBonusValue[2] + this.m_subCharaOverlapBonus[1]) * 0.01f;
		this.m_main_chara_bonus_value_rate.distance = (this.m_mainCharaAbilityValue[4] + this.m_mainTeamAbilityBonusValue[0] + this.m_mainCharaOverlapBonus[3]) * 0.01f;
		this.m_sub_chara_bonus_value_rate.distance = (this.m_subCharaAbilityValue[4] + this.m_subTeamAbilityBonusValue[0] + this.m_subCharaOverlapBonus[3]) * 0.01f;
		if (this.m_boostItemValidFlag[0])
		{
			this.m_boostBonusValue = 1f;
			this.m_main_chara_bonus_value_rate.score = this.m_main_chara_bonus_value_rate.score + this.m_boostBonusValue;
			if (this.m_boostItemValidFlag[2])
			{
				SaveDataManager saveDataManager = SaveDataManager.Instance;
				if (saveDataManager != null && this.CheckCharaType(saveDataManager.PlayerData.SubChara))
				{
					this.m_sub_chara_bonus_value_rate.score = this.m_sub_chara_bonus_value_rate.score + this.m_boostBonusValue;
				}
			}
		}
		this.m_bonus_value_rate.distance = this.m_bonus_value_rate.distance + (this.m_main_chara_bonus_value_rate.distance + this.m_sub_chara_bonus_value_rate.distance);
		this.m_bonus_value_rate.score = this.m_bonus_value_rate.score + (this.m_main_chara_bonus_value_rate.score + this.m_sub_chara_bonus_value_rate.score);
		this.m_bonus_value_rate.animal = this.m_bonus_value_rate.animal + (this.m_main_chara_bonus_value_rate.animal + this.m_sub_chara_bonus_value_rate.animal);
		this.m_bonus_value_rate.ring = this.m_bonus_value_rate.ring + (this.m_main_chara_bonus_value_rate.ring + this.m_sub_chara_bonus_value_rate.ring);
	}

	private void SetPampaignBonusValueRate()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerCampaignData campaignInSession = ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.BankedRingBonus);
			if (campaignInSession != null)
			{
				float fContent = campaignInSession.fContent;
				this.m_campaignBonusValue = fContent;
			}
			else
			{
				this.m_campaignBonusValue = 0f;
			}
		}
	}

	private void SetMileageBonusScoreRate()
	{
		GameObject gameObject = GameObject.Find("MileageBonusInfo");
		if (gameObject != null)
		{
			MileageBonusInfo component = gameObject.GetComponent<MileageBonusInfo>();
			if (component != null)
			{
				this.m_mileage_bonus_score_rate.Reset();
				float num = component.BonusData.value - 1f;
				switch (component.BonusData.type)
				{
				case MileageBonus.SCORE:
					this.m_mileage_bonus_score_rate.score = num;
					break;
				case MileageBonus.ANIMAL:
					this.m_mileage_bonus_score_rate.animal = num;
					break;
				case MileageBonus.RING:
					this.m_mileage_bonus_score_rate.ring = num;
					break;
				case MileageBonus.DISTANCE:
					this.m_mileage_bonus_score_rate.distance = num;
					break;
				case MileageBonus.FINAL_SCORE:
					this.m_mileage_bonus_score_rate.final_score = num;
					break;
				}
			}
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			this.m_mileage_bonus_score_rate.Reset();
		}
	}

	private float GetPlusPercentBonusValue(float percent, float src_value)
	{
		return Mathf.Ceil(src_value + src_value * percent * 0.01f);
	}

	private float GetPlusPercentBonusTime(float percent, float src_value)
	{
		return src_value + src_value * percent * 0.01f;
	}

	private float CalcPlusAbliltyBonusValue(ChaoAbility ability, float src_value)
	{
		float num = src_value;
		switch (ability)
		{
		case ChaoAbility.COLOR_POWER_SCORE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.ASTEROID_SCORE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.DRILL_SCORE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.LASER_SCORE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.COLOR_POWER_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.ITEM_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.COMBO_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.TRAMPOLINE_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.MAGNET_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.ASTEROID_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.DRILL_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.LASER_TIME:
			num = this.GetPlusPercentBonusTime(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.BOSS_STAGE_TIME:
			num = this.GetChaoAbilityValue(ability) + src_value;
			return num;
		case ChaoAbility.COMBO_RECEPTION_TIME:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.BOSS_RED_RING_RATE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			num = Mathf.Clamp(num, 0f, 100f);
			return num;
		case ChaoAbility.BOSS_SUPER_RING_RATE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			num = Mathf.Clamp(num, 0f, 100f);
			return num;
		case ChaoAbility.RARE_ENEMY_UP:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			num = Mathf.Clamp(num, 0f, 100f);
			return num;
		case ChaoAbility.SPECIAL_CRYSTAL_RATE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			num = Mathf.Clamp(num, 0f, 100f);
			return num;
		case ChaoAbility.LAST_CHANCE:
			num = this.GetChaoAbilityValue(ChaoAbility.LAST_CHANCE);
			return num;
		case ChaoAbility.RECOVERY_RING:
			return num;
		case ChaoAbility.BOSS_ATTACK:
		case ChaoAbility.CHECK_POINT_COMBO_UP:
		case ChaoAbility.CHECK_POINT_ITEM_BOX:
		case ChaoAbility.SPIN_DASH_MAGNET:
		case ChaoAbility.JUMP_RAMP:
		case ChaoAbility.CANNON_MAGNET:
		case ChaoAbility.DASH_RING_MAGNET:
		case ChaoAbility.JUMP_RAMP_MAGNET:
		case ChaoAbility.JUMP_RAMP_TRICK_SUCCESS:
		case ChaoAbility.PURSUES_TO_BOSS_AFTER_ATTACK:
		case ChaoAbility.SPECIAL_ANIMAL:
		case ChaoAbility.SPECIAL_ANIMAL_PSO2:
		case ChaoAbility.DAMAGE_TRAMPOLINE:
		case ChaoAbility.RECOVERY_FROM_FAILURE:
		case ChaoAbility.ADD_COMBO_VALUE:
		case ChaoAbility.LOOP_COMBO_UP:
		case ChaoAbility.LOOP_MAGNET:
		case ChaoAbility.DAMAGE_DESTROY_ALL:
		case ChaoAbility.ITEM_REVIVE:
			IL_BD:
			switch (ability)
			{
			case ChaoAbility.ALL_BONUS_COUNT:
				return num;
			case ChaoAbility.SCORE_COUNT:
				return num;
			case ChaoAbility.RING_COUNT:
				return num;
			case ChaoAbility.RED_RING_COUNT:
				return num;
			case ChaoAbility.ANIMAL_COUNT:
				return num;
			case ChaoAbility.RUNNIGN_DISTANCE:
				return num;
			default:
				return num;
			}
			break;
		case ChaoAbility.MAP_BOSS_DAMAGE:
			num = src_value - this.GetChaoAbilityValue(ChaoAbility.MAP_BOSS_DAMAGE);
			if (num < 1f)
			{
				num = 1f;
			}
			return num;
		case ChaoAbility.ENEMY_SCORE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.MAGNET_RANGE:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.AGGRESSIVITY_UP_FOR_RAID_BOSS:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		case ChaoAbility.TRANSFER_DOUBLE_RING:
			num = this.GetPlusPercentBonusValue(this.GetChaoAbilityValue(ability), src_value);
			return num;
		}
		goto IL_BD;
	}

	private float CalcItemPlusAbliltyBonusValue(ItemType itemType)
	{
		float num = 0f;
		switch (itemType)
		{
		case ItemType.INVINCIBLE:
		{
			num = this.CharaAbility[9];
			float percent = this.GetChaoAbilityValue(ChaoAbility.ITEM_TIME);
			num = this.GetPlusPercentBonusTime(percent, num);
			break;
		}
		case ItemType.MAGNET:
		{
			num = this.CharaAbility[8];
			float percent = this.GetChaoAbilityValue(ChaoAbility.MAGNET_TIME) + this.GetChaoAbilityValue(ChaoAbility.ITEM_TIME);
			num = this.GetPlusPercentBonusTime(percent, num);
			break;
		}
		case ItemType.TRAMPOLINE:
		{
			num = this.CharaAbility[5];
			float percent = this.GetChaoAbilityValue(ChaoAbility.TRAMPOLINE_TIME) + this.GetChaoAbilityValue(ChaoAbility.ITEM_TIME);
			num = this.GetPlusPercentBonusTime(percent, num);
			break;
		}
		case ItemType.COMBO:
		{
			num = this.CharaAbility[7];
			float percent = this.GetChaoAbilityValue(ChaoAbility.COMBO_TIME) + this.GetChaoAbilityValue(ChaoAbility.ITEM_TIME);
			num = this.GetPlusPercentBonusTime(percent, num);
			break;
		}
		case ItemType.LASER:
		{
			num = this.CharaAbility[0];
			float percent = this.GetChaoAbilityValue(ChaoAbility.LASER_TIME) + this.GetChaoAbilityValue(ChaoAbility.COLOR_POWER_TIME);
			num = this.GetPlusPercentBonusTime(percent, num);
			break;
		}
		case ItemType.DRILL:
		{
			num = this.CharaAbility[1];
			float percent = this.GetChaoAbilityValue(ChaoAbility.DRILL_TIME) + this.GetChaoAbilityValue(ChaoAbility.COLOR_POWER_TIME);
			num = this.GetPlusPercentBonusTime(percent, num);
			break;
		}
		case ItemType.ASTEROID:
		{
			num = this.CharaAbility[2];
			float percent = this.GetChaoAbilityValue(ChaoAbility.ASTEROID_TIME) + this.GetChaoAbilityValue(ChaoAbility.COLOR_POWER_TIME);
			num = this.GetPlusPercentBonusTime(percent, num);
			break;
		}
		}
		return num;
	}

	private void CalcChaoCountBonus()
	{
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0.1f;
			float num5 = 0.3f;
			float num6 = 1f;
			List<ServerChaoState> chaoStates = playerState.ChaoStates;
			foreach (ServerChaoState current in chaoStates)
			{
				if (current != null && current.Status > ServerChaoState.ChaoStatus.NotOwned)
				{
					switch (current.Rarity)
					{
					case 0:
						num += num4 * (float)current.NumAcquired;
						break;
					case 1:
						num2 += num5 * (float)current.NumAcquired;
						break;
					case 2:
						num3 += num6 * (float)current.NumAcquired;
						break;
					}
					this.m_chaoCount += current.NumAcquired;
				}
			}
			float value = num + num2 + num3;
			this.m_chaoCountBonus = Mathf.Clamp(value, 0f, 200f);
		}
	}

	private ChaoEffect.TargetType GetChaoEffectTagetType(ChaoAbility ability)
	{
		ChaoEffect.TargetType targetType = ChaoEffect.TargetType.Unknown;
		if (ability != ChaoAbility.UNKNOWN && ability < ChaoAbility.NUM)
		{
			if (this.HasChaoAbility(this.m_mainChaoInfo, ability))
			{
				targetType = ChaoEffect.TargetType.MainChao;
			}
			if (this.HasChaoAbility(this.m_subChaoInfo, ability))
			{
				targetType = ((targetType != ChaoEffect.TargetType.MainChao) ? ChaoEffect.TargetType.SubChao : ChaoEffect.TargetType.BothChao);
				if (ability == ChaoAbility.RECOVERY_RING && targetType == ChaoEffect.TargetType.BothChao)
				{
					targetType = ChaoEffect.TargetType.MainChao;
				}
			}
		}
		return targetType;
	}

	private void PlayChaoEffect(ChaoAbility ability)
	{
		ChaoEffect.TargetType chaoEffectTagetType = this.GetChaoEffectTagetType(ability);
		if (chaoEffectTagetType != ChaoEffect.TargetType.Unknown)
		{
			ChaoEffect chaoEffect = ChaoEffect.Instance;
			if (chaoEffect != null)
			{
				chaoEffect.RequestPlayChaoEffect(chaoEffectTagetType, ability);
			}
		}
	}

	private void PlayChaoEffect(ChaoAbility ability, ChaoType chaoType)
	{
		ChaoEffect.TargetType chaoEffectTagetType = this.GetChaoEffectTagetType(ability);
		if (chaoType != ChaoType.MAIN)
		{
			if (chaoType == ChaoType.SUB)
			{
				if (chaoEffectTagetType == ChaoEffect.TargetType.SubChao || chaoEffectTagetType == ChaoEffect.TargetType.BothChao)
				{
					ChaoEffect chaoEffect = ChaoEffect.Instance;
					if (chaoEffect != null)
					{
						chaoEffect.RequestPlayChaoEffect(ChaoEffect.TargetType.SubChao, ability);
					}
				}
			}
		}
		else if (chaoEffectTagetType == ChaoEffect.TargetType.MainChao || chaoEffectTagetType == ChaoEffect.TargetType.BothChao)
		{
			ChaoEffect chaoEffect2 = ChaoEffect.Instance;
			if (chaoEffect2 != null)
			{
				chaoEffect2.RequestPlayChaoEffect(ChaoEffect.TargetType.MainChao, ability);
			}
		}
	}

	private void StopChaoEffect(ChaoAbility ability)
	{
		ChaoEffect.TargetType chaoEffectTagetType = this.GetChaoEffectTagetType(ability);
		if (chaoEffectTagetType != ChaoEffect.TargetType.Unknown)
		{
			ChaoEffect chaoEffect = ChaoEffect.Instance;
			if (chaoEffect != null)
			{
				chaoEffect.RequestStopChaoEffect(chaoEffectTagetType, ability);
			}
		}
	}

	private void CheckPlayerInformation()
	{
		if (this.m_playerInformation == null)
		{
			this.m_playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
		}
	}

	public static void LoadAbilityDataTable(ResourceSceneLoader loaderComponent)
	{
		if (loaderComponent != null)
		{
			if (!StageAbilityManager.IsExistDataObject(StageAbilityManager.CHAODATA_NAME))
			{
				loaderComponent.AddLoadAndResourceManager(StageAbilityManager.m_loadInfo[0]);
			}
			if (!StageAbilityManager.IsExistDataObject(StageAbilityManager.CHARADATA_NAME))
			{
				loaderComponent.AddLoadAndResourceManager(StageAbilityManager.m_loadInfo[1]);
			}
		}
	}

	public static void SetupAbilityDataTable()
	{
		StageAbilityManager stageAbilityManager = StageAbilityManager.Instance;
		if (stageAbilityManager != null)
		{
			stageAbilityManager.Setup();
		}
	}

	private static bool IsExistDataObject(string name)
	{
		GameObject x = GameObject.Find(name);
		return x != null;
	}
}
