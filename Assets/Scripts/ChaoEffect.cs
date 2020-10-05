using DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChaoEffect : MonoBehaviour
{
	public enum TargetType
	{
		MainChao,
		SubChao,
		BothChao,
		Unknown
	}

	public class DataInfo
	{
		public string m_normal;

		public string m_rare;

		public string m_srare;

		public float m_time;

		public bool m_loop;

		public DataInfo()
		{
			this.m_normal = null;
			this.m_rare = null;
			this.m_srare = null;
			this.m_time = 0f;
			this.m_loop = false;
		}

		public DataInfo(string normal, string rare, string srare, float time, bool loop)
		{
			this.m_normal = normal;
			this.m_rare = rare;
			this.m_srare = srare;
			this.m_time = time;
			this.m_loop = loop;
		}
	}

	public class LoopEffetData
	{
		public GameObject m_obj;

		public ChaoAbility m_ability;

		public ChaoType m_chaoType;

		public LoopEffetData(GameObject obj, ChaoAbility ability, ChaoType chaoType)
		{
			this.m_obj = obj;
			this.m_ability = ability;
			this.m_chaoType = chaoType;
		}
	}

	public readonly ChaoEffect.DataInfo[] m_effectTable = new ChaoEffect.DataInfo[]
	{
		new ChaoEffect.DataInfo("ef_ch_bonus_all_01", null, "ef_ch_bonus_all_sr01", -1f, false),
		new ChaoEffect.DataInfo("ef_ch_bonus_score_01", "ef_ch_bonus_score_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_bonus_ring_01", "ef_ch_bonus_ring_r01", "ef_ch_bonus_ring_sr01", -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo("ef_ch_bonus_animal_01", "ef_ch_bonus_animal_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_bonus_run_01", "ef_ch_bonus_run_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_sp_score_spitem_r01", "ef_ch_sp_score_spitem_sr01", -1f, false),
		new ChaoEffect.DataInfo("ef_ch_raid_up_ring_c01", "ef_ch_raid_up_ring_r01", "ef_ch_raid_up_ring_sr01", -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_crystal_s_r01", "ef_ch_combo_crystal_s_sr01", -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_crystal_s_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_crystal_l_r01", "ef_ch_combo_crystal_l_sr01", -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_crystal_l_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_enemy_g_r01", "ef_ch_combo_enemy_g_sr01", -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_animal_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_bomber_bullet_r01", null, 6f, true),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_ring10_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_combo_combo_up_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_sp_combo_crystal_sp_c01", "ef_ch_sp_combo_crystal_sp_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_combo_brk_trap_c01", null, null, -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(null, null, "ef_ch_score_cpower_sr01", -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_score_asteroid_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_score_drill_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_score_laser_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_time_cpower_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_time_item_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_time_combo_c01", "ef_ch_time_combo_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_time_trampoline_c01", "ef_ch_time_trampoline_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_time_magnet_c01", "ef_ch_time_magnet_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_time_asteroid_01", null, null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_time_drill_01", null, null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_time_laser_01", null, null, -1f, false),
		new ChaoEffect.DataInfo(null, null, "ef_ch_magic_atk_st_sr01", -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(null, null, "ef_ch_beam_atk_st_sr01", -1f, false),
		new ChaoEffect.DataInfo(null, null, "ef_ch_beam_atk_st_sr02", -1f, false),
		new ChaoEffect.DataInfo("ef_ch_up_rareenemy_c01", "ef_ch_up_rareenemy_sr01", "ef_ch_up_rareenemy_sr01", -1f, false),
		new ChaoEffect.DataInfo("ef_ch_sp_up_spitem_c01", "ef_ch_sp_up_spitem_r01", "ef_ch_sp_up_spitem_sr01", -1f, false),
		new ChaoEffect.DataInfo(null, null, "ef_ch_lastchance_sr01", 1f, true),
		new ChaoEffect.DataInfo("ef_ch_ring_absorb_c01", "ef_ch_ring_absorb_r01", "ef_ch_ring_absorb_sr01", -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(null, null, "ef_ch_magic_atk_st_sr01", -1f, false),
		new ChaoEffect.DataInfo("ef_ch_check_combo_01", "ef_ch_check_combo_r01", null, -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo("ef_ch_random_magnet_01", null, null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_random_jump_01", null, null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_bonus_rsr_01", null, "ef_ch_bonus_rsr_sr01", -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_up_magnet_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_raid_up_atk_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_canon_magnet_c01", "ef_ch_canon_magnet_r01", null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_dashring_magnet_c01", null, null, -1f, false),
		new ChaoEffect.DataInfo("ef_ch_jumpboard_magnet_c01", "ef_ch_jumpboard_magnet_r01", null, -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(null, "ef_ch_change_rareanimal_r01", null, -1f, false),
		new ChaoEffect.DataInfo(null, "ef_ch_change_rappy_r01", null, -1f, false),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo(),
		new ChaoEffect.DataInfo()
	};

	private List<ChaoEffect.LoopEffetData> m_loopDataList = new List<ChaoEffect.LoopEffetData>();

	private readonly string[] ChaoTypeName = new string[]
	{
		"MainChao",
		"SubChao"
	};

	private static ChaoEffect instance;

	public static ChaoEffect Instance
	{
		get
		{
			return ChaoEffect.instance;
		}
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (ChaoEffect.instance == this)
		{
			ChaoEffect.instance = null;
		}
	}

	private void SetInstance()
	{
		if (ChaoEffect.instance == null)
		{
			ChaoEffect.instance = this;
		}
		else if (this != ChaoEffect.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private ChaoData.Rarity GetRarity(ChaoType chaotype)
	{
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager != null)
		{
			int id = (chaotype != ChaoType.MAIN) ? saveDataManager.PlayerData.SubChaoID : saveDataManager.PlayerData.MainChaoID;
			ChaoData chaoData = ChaoTable.GetChaoData(id);
			if (chaoData != null)
			{
				return chaoData.rarity;
			}
		}
		return ChaoData.Rarity.NORMAL;
	}

	private string GetEffectName(ChaoData.Rarity rarity, ChaoAbility ability)
	{
		if (ability == ChaoAbility.UNKNOWN || ability >= ChaoAbility.NUM)
		{
			return null;
		}
		if (rarity == ChaoData.Rarity.NORMAL)
		{
			return this.m_effectTable[(int)ability].m_normal;
		}
		if (rarity == ChaoData.Rarity.RARE)
		{
			return this.m_effectTable[(int)ability].m_rare;
		}
		return this.m_effectTable[(int)ability].m_srare;
	}

	private float GetEffectPlayTime(ChaoAbility ability)
	{
		if (ability != ChaoAbility.UNKNOWN && ability < ChaoAbility.NUM)
		{
			return this.m_effectTable[(int)ability].m_time;
		}
		return 0f;
	}

	private bool GetEffectPlayLoop(ChaoAbility ability)
	{
		return ability != ChaoAbility.UNKNOWN && ability < ChaoAbility.NUM && this.m_effectTable[(int)ability].m_loop;
	}

	private void PlayEffect(GameObject chaoObj, ChaoAbility ability, ChaoType chaoType)
	{
		if (chaoObj != null)
		{
			string effectName = this.GetEffectName(this.GetRarity(chaoType), ability);
			if (effectName != null)
			{
				if (this.GetEffectPlayLoop(ability))
				{
					GameObject gameObject = ObjUtil.PlayChaoEffect(chaoObj, effectName, -1f);
					if (gameObject != null)
					{
						ChaoEffect.LoopEffetData item = new ChaoEffect.LoopEffetData(gameObject, ability, chaoType);
						this.m_loopDataList.Add(item);
					}
				}
				else
				{
					float effectPlayTime = this.GetEffectPlayTime(ability);
					ObjUtil.PlayChaoEffect(chaoObj, effectName, effectPlayTime);
				}
			}
		}
	}

	private void StopEffect(ChaoAbility ability, ChaoType chaoType)
	{
		foreach (ChaoEffect.LoopEffetData current in this.m_loopDataList)
		{
			if (current.m_ability == ability && current.m_chaoType == chaoType)
			{
				UnityEngine.Object.Destroy(current.m_obj);
				this.m_loopDataList.Remove(current);
				break;
			}
		}
	}

	private void PlayChaoSE()
	{
		SoundManager.SePlay("act_chao_effect", "SE");
	}

	public void RequestPlayChaoEffect(ChaoEffect.TargetType target, ChaoAbility ability)
	{
		if (ability != ChaoAbility.UNKNOWN && ability < ChaoAbility.NUM)
		{
			switch (target)
			{
			case ChaoEffect.TargetType.MainChao:
				this.PlayEffect(this.GetChaoObject(ChaoType.MAIN), ability, ChaoType.MAIN);
				break;
			case ChaoEffect.TargetType.SubChao:
				this.PlayEffect(this.GetChaoObject(ChaoType.SUB), ability, ChaoType.SUB);
				break;
			case ChaoEffect.TargetType.BothChao:
				this.PlayEffect(this.GetChaoObject(ChaoType.MAIN), ability, ChaoType.MAIN);
				this.PlayEffect(this.GetChaoObject(ChaoType.SUB), ability, ChaoType.SUB);
				break;
			}
			this.PlayChaoSE();
		}
	}

	public void RequestStopChaoEffect(ChaoEffect.TargetType target, ChaoAbility ability)
	{
		if (ability != ChaoAbility.UNKNOWN && ability < ChaoAbility.NUM)
		{
			if (target == ChaoEffect.TargetType.MainChao)
			{
				this.StopEffect(ability, ChaoType.MAIN);
			}
			else if (target == ChaoEffect.TargetType.SubChao)
			{
				this.StopEffect(ability, ChaoType.SUB);
			}
			else if (target == ChaoEffect.TargetType.BothChao)
			{
				this.StopEffect(ability, ChaoType.MAIN);
				this.StopEffect(ability, ChaoType.SUB);
			}
		}
	}

	private GameObject GetChaoObject(ChaoType chaotype)
	{
		Transform parent = base.transform.parent;
		if (!(parent != null))
		{
			return null;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(parent.gameObject, this.ChaoTypeName[(int)chaotype]);
		if (gameObject != null && gameObject.activeSelf)
		{
			return gameObject;
		}
		return null;
	}
}
