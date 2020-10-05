using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjBossParameter : MonoBehaviour
{
	private const float ADDSPEED_DISTANCE = 0.2f;

	private static Vector3 SHOT_ROT_BASE = new Vector3(-1f, 0f, 0f);

	private int m_bossType;

	private BossCharaType m_bossCharaType;

	private float m_speed;

	private float m_min_speed;

	private float m_player_speed;

	private float m_add_speed;

	private float m_add_speed_ratio = 1f;

	private Vector3 m_start_pos = Vector3.zero;

	private int m_ring = 20;

	private int m_super_ring;

	private int m_red_star_ring;

	private int m_bronze_timer;

	private int m_silver_timer;

	private int m_gold_timer;

	private int m_hp;

	private int m_hp_max;

	private int m_distance;

	private float m_step_move_y;

	private bool m_data_setup;

	private int m_level;

	private int m_attackPower = 1;

	private float m_down_speed;

	private float m_attackInterspaceMin;

	private float m_attackInterspaceMax;

	private float m_defaultPlayerDistance;

	private int m_tbl_id;

	private int m_attack_tbl_id;

	private int m_trapRand;

	private float m_boundParamMin;

	private float m_boundParamMax;

	private int m_boundMaxRand;

	private float m_shotSpeed;

	private float m_attackSpeed;

	private float m_attackSpeedMin;

	private float m_attackSpeedMax;

	private float m_bumperFirstSpeed;

	private float m_bumperOutOfcontrol;

	private float m_bumperSpeedup;

	private float m_ballSpeed = 8f;

	private int m_bumperRand;

	private bool m_attackBallFlag;

	private int m_attackTrapCount;

	private int m_attackTrapCountMax;

	private float m_missileSpeed;

	private float m_missileInterspace;

	private float m_rotSpeed;

	private bool m_afterAttack;

	private List<Map3AttackData> m_map3AttackDataList;

	private static readonly Vector3[] BOM_TYPE_A = new Vector3[]
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(0f, 1f, 0f)
	};

	private static readonly Vector3[] BOM_TYPE_B = new Vector3[]
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(1f, 0f, 0f)
	};

	public Vector3 ShotRotBase
	{
		get
		{
			return ObjBossParameter.SHOT_ROT_BASE;
		}
	}

	public int TypeBoss
	{
		get
		{
			return this.m_bossType;
		}
		set
		{
			this.m_bossType = value;
			this.m_bossCharaType = BossTypeUtil.GetBossCharaType((BossType)this.m_bossType);
		}
	}

	public BossCharaType CharaTypeBoss
	{
		get
		{
			return this.m_bossCharaType;
		}
	}

	public float Speed
	{
		get
		{
			return this.m_speed;
		}
		set
		{
			this.m_speed = value;
		}
	}

	public float MinSpeed
	{
		get
		{
			return this.m_min_speed;
		}
		set
		{
			this.m_min_speed = value;
		}
	}

	public float PlayerSpeed
	{
		get
		{
			return this.m_player_speed;
		}
	}

	public float AddSpeed
	{
		get
		{
			return this.m_add_speed;
		}
	}

	public float AddSpeedRatio
	{
		get
		{
			return this.m_add_speed_ratio;
		}
	}

	public float AddSpeedDistance
	{
		get
		{
			return this.AddSpeed * 0.2f;
		}
	}

	public Vector3 StartPos
	{
		get
		{
			return this.m_start_pos;
		}
	}

	public int RingCount
	{
		get
		{
			return this.m_ring;
		}
	}

	public int SuperRingRatio
	{
		get
		{
			return this.m_super_ring;
		}
	}

	public int RedStarRingRatio
	{
		get
		{
			return this.m_red_star_ring;
		}
	}

	public int BronzeTimerRatio
	{
		get
		{
			return this.m_bronze_timer;
		}
	}

	public int SilverTimerRatio
	{
		get
		{
			return this.m_silver_timer;
		}
	}

	public int GoldTimerRatio
	{
		get
		{
			return this.m_gold_timer;
		}
	}

	public int BossHP
	{
		get
		{
			return this.m_hp;
		}
		set
		{
			this.m_hp = value;
		}
	}

	public int BossHPMax
	{
		get
		{
			return this.m_hp_max;
		}
		set
		{
			this.m_hp_max = value;
		}
	}

	public int BossDistance
	{
		get
		{
			return this.m_distance;
		}
		set
		{
			if (this.TypeBoss == 0)
			{
				this.m_distance = ObjUtil.GetChaoAbliltyValue(ChaoAbility.BOSS_STAGE_TIME, value);
			}
			else
			{
				this.m_distance = value;
			}
		}
	}

	public float StepMoveY
	{
		get
		{
			return this.m_step_move_y;
		}
		set
		{
			this.m_step_move_y = value;
		}
	}

	public int BossLevel
	{
		get
		{
			return this.m_level;
		}
		set
		{
			this.m_level = value;
		}
	}

	public int BossAttackPower
	{
		get
		{
			return this.m_attackPower;
		}
		set
		{
			this.m_attackPower = value;
		}
	}

	public float DownSpeed
	{
		get
		{
			return this.m_down_speed;
		}
		set
		{
			this.m_down_speed = value;
		}
	}

	public float AttackInterspaceMin
	{
		get
		{
			return this.m_attackInterspaceMin;
		}
		set
		{
			this.m_attackInterspaceMin = value;
		}
	}

	public float AttackInterspaceMax
	{
		get
		{
			return this.m_attackInterspaceMax;
		}
		set
		{
			this.m_attackInterspaceMax = value;
		}
	}

	public float DefaultPlayerDistance
	{
		get
		{
			return this.m_defaultPlayerDistance;
		}
		set
		{
			this.m_defaultPlayerDistance = value;
		}
	}

	public int TableID
	{
		get
		{
			return this.m_tbl_id;
		}
		set
		{
			this.m_tbl_id = value;
		}
	}

	public int AttackTableID
	{
		get
		{
			return this.m_attack_tbl_id;
		}
		set
		{
			this.m_attack_tbl_id = value;
		}
	}

	public int TrapRand
	{
		get
		{
			return this.m_trapRand;
		}
		set
		{
			this.m_trapRand = value;
		}
	}

	public float BoundParamMin
	{
		get
		{
			return this.m_boundParamMin;
		}
		set
		{
			this.m_boundParamMin = value;
		}
	}

	public float BoundParamMax
	{
		get
		{
			return this.m_boundParamMax;
		}
		set
		{
			this.m_boundParamMax = value;
		}
	}

	public int BoundMaxRand
	{
		get
		{
			return this.m_boundMaxRand;
		}
		set
		{
			this.m_boundMaxRand = value;
		}
	}

	public float ShotSpeed
	{
		get
		{
			return this.m_shotSpeed;
		}
		set
		{
			this.m_shotSpeed = value;
		}
	}

	public float AttackSpeed
	{
		get
		{
			return this.m_attackSpeed;
		}
		set
		{
			this.m_attackSpeed = value;
		}
	}

	public float AttackSpeedMin
	{
		get
		{
			return this.m_attackSpeedMin;
		}
		set
		{
			this.m_attackSpeedMin = value;
		}
	}

	public float AttackSpeedMax
	{
		get
		{
			return this.m_attackSpeedMax;
		}
		set
		{
			this.m_attackSpeedMax = value;
		}
	}

	public float BumperFirstSpeed
	{
		get
		{
			return this.m_bumperFirstSpeed;
		}
		set
		{
			this.m_bumperFirstSpeed = value;
		}
	}

	public float BumperOutOfcontrol
	{
		get
		{
			return this.m_bumperOutOfcontrol;
		}
		set
		{
			this.m_bumperOutOfcontrol = value;
		}
	}

	public float BumperSpeedup
	{
		get
		{
			return this.m_bumperSpeedup;
		}
		set
		{
			this.m_bumperSpeedup = value;
		}
	}

	public float BallSpeed
	{
		get
		{
			return this.m_ballSpeed;
		}
		set
		{
			this.m_ballSpeed = value;
		}
	}

	public int BumperRand
	{
		get
		{
			return this.m_bumperRand;
		}
		set
		{
			this.m_bumperRand = value;
		}
	}

	public bool AttackBallFlag
	{
		get
		{
			return this.m_attackBallFlag;
		}
		set
		{
			this.m_attackBallFlag = value;
		}
	}

	public int AttackTrapCount
	{
		get
		{
			return this.m_attackTrapCount;
		}
		set
		{
			this.m_attackTrapCount = value;
		}
	}

	public int AttackTrapCountMax
	{
		get
		{
			return this.m_attackTrapCountMax;
		}
		set
		{
			this.m_attackTrapCountMax = value;
		}
	}

	public float MissileSpeed
	{
		get
		{
			return this.m_missileSpeed;
		}
		set
		{
			this.m_missileSpeed = value;
		}
	}

	public float MissileInterspace
	{
		get
		{
			return this.m_missileInterspace;
		}
		set
		{
			this.m_missileInterspace = value;
		}
	}

	public float RotSpeed
	{
		get
		{
			return this.m_rotSpeed;
		}
		set
		{
			this.m_rotSpeed = value;
		}
	}

	public bool AfterAttack
	{
		get
		{
			return this.m_afterAttack;
		}
		set
		{
			this.m_afterAttack = value;
		}
	}

	public void Setup()
	{
		this.m_speed = 0f;
		this.m_min_speed = 0f;
		this.m_player_speed = ObjUtil.GetPlayerDefaultSpeed();
		this.m_add_speed = ObjUtil.GetPlayerAddSpeed();
		this.m_add_speed_ratio = ObjUtil.GetPlayerAddSpeedRatio();
		this.m_start_pos = base.transform.position;
		this.m_hp = this.BossHPMax;
		this.OnSetup();
		this.SetupBossTable();
	}

	protected virtual void OnSetup()
	{
	}

	public Map3AttackData GetMap3AttackData()
	{
		if (this.m_map3AttackDataList != null && this.m_map3AttackDataList.Count > 0)
		{
			int num = UnityEngine.Random.Range(0, this.m_map3AttackDataList.Count);
			if (num < this.m_map3AttackDataList.Count)
			{
				return this.m_map3AttackDataList[num];
			}
		}
		return null;
	}

	public Vector3 GetMap3BomTblA(BossAttackType type)
	{
		if ((ulong)type < (ulong)((long)ObjBossParameter.BOM_TYPE_A.Length))
		{
			return ObjBossParameter.BOM_TYPE_A[(int)type];
		}
		return ObjBossParameter.BOM_TYPE_A[0];
	}

	public Vector3 GetMap3BomTblB(BossAttackType type)
	{
		if ((ulong)type < (ulong)((long)ObjBossParameter.BOM_TYPE_B.Length))
		{
			return ObjBossParameter.BOM_TYPE_B[(int)type];
		}
		return ObjBossParameter.BOM_TYPE_B[0];
	}

	public void SetupBossTable()
	{
		if (this.m_data_setup)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("GameModeStage");
		if (gameObject != null)
		{
			GameModeStage component = gameObject.GetComponent<GameModeStage>();
			if (component != null)
			{
				BossTable bossTable = component.GetBossTable();
				BossMap3Table bossMap3Table = component.GetBossMap3Table();
				if (bossTable != null && bossTable.IsSetupEnd() && bossMap3Table != null && bossMap3Table.IsSetupEnd() && this.m_map3AttackDataList == null)
				{
					this.m_super_ring = bossTable.GetItemData(this.TableID, BossTableItem.SuperRing);
					this.m_red_star_ring = bossTable.GetItemData(this.TableID, BossTableItem.RedStarRing);
					this.m_bronze_timer = bossTable.GetItemData(this.TableID, BossTableItem.BronzeWatch);
					this.m_silver_timer = bossTable.GetItemData(this.TableID, BossTableItem.SilverWatch);
					this.m_gold_timer = bossTable.GetItemData(this.TableID, BossTableItem.GoldWatch);
					this.m_map3AttackDataList = new List<Map3AttackData>();
					for (int i = 0; i < 16; i++)
					{
						Map3AttackData map3AttackData = bossMap3Table.GetMap3AttackData(this.AttackTableID, i);
						if (map3AttackData.GetAttackCount() == 0)
						{
							break;
						}
						this.m_map3AttackDataList.Add(map3AttackData);
					}
					this.m_data_setup = true;
				}
			}
		}
	}
}
