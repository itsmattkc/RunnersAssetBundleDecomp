using App.Utility;
using System;
using UnityEngine;

public class LevelInformation : MonoBehaviour
{
	private enum Status
	{
		FEVER_BOSS,
		BOSS,
		BOSS_DESTROY,
		TUTORIAL,
		BOSS_STAGE,
		MISSED,
		REQEST_PAUSE,
		REQEST_CHARA_CHANGE,
		REQEST_EQUIP_ITEM
	}

	private float m_distanceToBossOnStart;

	private float m_distanceOnStage;

	private float m_distanceToBoss;

	private float m_bossEndTime;

	private int m_numBossAttack;

	private int m_numBossHpMax;

	private Bitset32 m_status;

	private int m_playerRank;

	private bool m_lightMode;

	private Rect m_window;

	public bool m_showLevelInfo;

	private int m_feverBossCount;

	private int m_moveTrapBooRand;

	private bool m_extreme;

	private bool m_invalidExtreme;

	public float DistanceToBoss
	{
		get
		{
			return this.m_distanceToBoss;
		}
	}

	public bool NowFeverBoss
	{
		get
		{
			return this.m_status.Test(0);
		}
		set
		{
			this.m_status.Set(0, value);
		}
	}

	public bool NowBoss
	{
		get
		{
			return this.m_status.Test(1);
		}
		set
		{
			this.m_status.Set(1, value);
		}
	}

	public bool BossDestroy
	{
		get
		{
			return this.m_status.Test(2);
		}
		set
		{
			this.m_status.Set(2, value);
		}
	}

	public bool NowTutorial
	{
		get
		{
			return this.m_status.Test(3);
		}
		set
		{
			this.m_status.Set(3, value);
		}
	}

	public bool BossStage
	{
		get
		{
			return this.m_status.Test(4);
		}
		set
		{
			this.m_status.Set(4, value);
		}
	}

	public bool Missed
	{
		get
		{
			return this.m_status.Test(5);
		}
		set
		{
			this.m_status.Set(5, value);
		}
	}

	public bool RequestPause
	{
		get
		{
			return this.m_status.Test(6);
		}
		set
		{
			this.m_status.Set(6, value);
		}
	}

	public bool RequestCharaChange
	{
		get
		{
			return this.m_status.Test(6);
		}
		set
		{
			this.m_status.Set(6, value);
		}
	}

	public bool RequestEqitpItem
	{
		get
		{
			return this.m_status.Test(8);
		}
		set
		{
			this.m_status.Set(8, value);
		}
	}

	public float DistanceToBossOnStart
	{
		get
		{
			return this.m_distanceToBossOnStart;
		}
		set
		{
			this.m_distanceToBossOnStart = value;
		}
	}

	public float DistanceOnStage
	{
		get
		{
			return this.m_distanceOnStage;
		}
		set
		{
			this.m_distanceOnStage = value;
		}
	}

	public float BossEndTime
	{
		get
		{
			return this.m_bossEndTime;
		}
		set
		{
			this.m_bossEndTime = value;
		}
	}

	public int NumBossAttack
	{
		get
		{
			return this.m_numBossAttack;
		}
		set
		{
			this.m_numBossAttack = value;
		}
	}

	public int NumBossHpMax
	{
		get
		{
			return this.m_numBossHpMax;
		}
		set
		{
			this.m_numBossHpMax = value;
		}
	}

	public int PlayerRank
	{
		get
		{
			return this.m_playerRank;
		}
		set
		{
			this.m_playerRank = value;
		}
	}

	public bool LightMode
	{
		get
		{
			return this.m_lightMode;
		}
		set
		{
			this.m_lightMode = value;
		}
	}

	public int FeverBossCount
	{
		get
		{
			return this.m_feverBossCount;
		}
		set
		{
			this.m_feverBossCount = value;
		}
	}

	public bool Extreme
	{
		get
		{
			return this.m_extreme;
		}
		set
		{
			this.m_extreme = value;
		}
	}

	public bool InvalidExtreme
	{
		get
		{
			return this.m_invalidExtreme;
		}
		set
		{
			this.m_invalidExtreme = value;
		}
	}

	public bool DestroyRingMode
	{
		get
		{
			return this.m_extreme && !this.m_invalidExtreme;
		}
	}

	public int MoveTrapBooRand
	{
		get
		{
			return this.m_moveTrapBooRand;
		}
		set
		{
			this.m_moveTrapBooRand = value;
		}
	}

	private void Start()
	{
		this.m_showLevelInfo = false;
	}

	private void Update()
	{
		if (this.NowFeverBoss || this.NowBoss)
		{
			this.m_distanceToBoss = 0f;
		}
		else
		{
			this.m_distanceToBoss = Mathf.Max(0f, this.m_distanceToBossOnStart - this.m_distanceOnStage);
		}
	}

	public void AddNumBossAttack(int count)
	{
		this.m_numBossAttack += count;
	}
}
