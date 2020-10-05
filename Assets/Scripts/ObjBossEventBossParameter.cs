using System;

public class ObjBossEventBossParameter : ObjBossParameter
{
	private float m_wispBoostRatio;

	private WispBoostLevel m_wispBoostLevel = WispBoostLevel.NONE;

	private int m_missilePos1;

	private int m_missilePos2;

	private float m_wispInterspace;

	private float m_bumperInterspace;

	private float m_wispSpeedMin;

	private float m_wispSpeedMax;

	private float m_wispSwingMin;

	private float m_wispSwingMax;

	private float m_wispAddXMin;

	private float m_wispAddXMax;

	private float m_missileWaitTime;

	private int m_missileCount;

	private float m_challengeValue;

	public float ChallengeValue
	{
		get
		{
			return this.m_challengeValue;
		}
	}

	public float BoostRatio
	{
		get
		{
			return this.m_wispBoostRatio;
		}
		set
		{
			this.m_wispBoostRatio = value;
		}
	}

	public float BoostRatioDown
	{
		get
		{
			return EventBossParamTable.GetItemData(EventBossParamTableItem.WispRatioDown);
		}
	}

	public float BoostRatioAdd
	{
		get
		{
			return EventBossParamTable.GetItemData(EventBossParamTableItem.WispRatio);
		}
	}

	public WispBoostLevel BoostLevel
	{
		get
		{
			return this.m_wispBoostLevel;
		}
		set
		{
			this.m_wispBoostLevel = value;
		}
	}

	public int MissilePos1
	{
		get
		{
			return this.m_missilePos1;
		}
		set
		{
			this.m_missilePos1 = value;
		}
	}

	public int MissilePos2
	{
		get
		{
			return this.m_missilePos2;
		}
		set
		{
			this.m_missilePos2 = value;
		}
	}

	public float WispInterspace
	{
		get
		{
			return this.m_wispInterspace;
		}
		set
		{
			this.m_wispInterspace = value;
		}
	}

	public float BumperInterspace
	{
		get
		{
			return this.m_bumperInterspace;
		}
		set
		{
			this.m_bumperInterspace = value;
		}
	}

	public float WispSpeedMin
	{
		get
		{
			return this.m_wispSpeedMin;
		}
		set
		{
			this.m_wispSpeedMin = value;
		}
	}

	public float WispSpeedMax
	{
		get
		{
			return this.m_wispSpeedMax;
		}
		set
		{
			this.m_wispSpeedMax = value;
		}
	}

	public float WispSwingMin
	{
		get
		{
			return this.m_wispSwingMin;
		}
		set
		{
			this.m_wispSwingMin = value;
		}
	}

	public float WispSwingMax
	{
		get
		{
			return this.m_wispSwingMax;
		}
		set
		{
			this.m_wispSwingMax = value;
		}
	}

	public float WispAddXMin
	{
		get
		{
			return this.m_wispAddXMin;
		}
		set
		{
			this.m_wispAddXMin = value;
		}
	}

	public float WispAddXMax
	{
		get
		{
			return this.m_wispAddXMax;
		}
		set
		{
			this.m_wispAddXMax = value;
		}
	}

	public float MissileWaitTime
	{
		get
		{
			return this.m_missileWaitTime;
		}
		set
		{
			this.m_missileWaitTime = value;
		}
	}

	public int MissileCount
	{
		get
		{
			return this.m_missileCount;
		}
		set
		{
			this.m_missileCount = value;
		}
	}

	protected override void OnSetup()
	{
		int num = 0;
		LevelInformation levelInformation = ObjUtil.GetLevelInformation();
		if (levelInformation != null)
		{
			levelInformation.NumBossHpMax = base.BossHPMax;
			num = base.BossHPMax - levelInformation.NumBossAttack;
		}
		if (num < 1)
		{
			num = 1;
		}
		base.BossHP = num;
		if (EventManager.Instance != null)
		{
			int useRaidbossChallengeCount = EventManager.Instance.UseRaidbossChallengeCount;
			this.m_challengeValue = EventManager.Instance.GetRaidAttackRate(useRaidbossChallengeCount);
		}
		else
		{
			this.m_challengeValue = 1f;
		}
	}

	private int GetBoostAttackParam(float attack, float challengeVal)
	{
		float num = attack * challengeVal;
		return (int)num;
	}

	public int GetBoostAttackParam(WispBoostLevel level)
	{
		int result = 0;
		switch (level)
		{
		case WispBoostLevel.LEVEL1:
			result = this.GetBoostAttackParam(EventBossParamTable.GetItemData(EventBossParamTableItem.BoostAttack1), this.m_challengeValue);
			break;
		case WispBoostLevel.LEVEL2:
			result = this.GetBoostAttackParam(EventBossParamTable.GetItemData(EventBossParamTableItem.BoostAttack2), this.m_challengeValue);
			break;
		case WispBoostLevel.LEVEL3:
			result = this.GetBoostAttackParam(EventBossParamTable.GetItemData(EventBossParamTableItem.BoostAttack3), this.m_challengeValue);
			break;
		}
		return result;
	}

	public float GetBoostSpeedParam(WispBoostLevel level)
	{
		float result = 0f;
		switch (level)
		{
		case WispBoostLevel.LEVEL1:
			result = EventBossParamTable.GetItemData(EventBossParamTableItem.BoostSpeed1);
			break;
		case WispBoostLevel.LEVEL2:
			result = EventBossParamTable.GetItemData(EventBossParamTableItem.BoostSpeed2);
			break;
		case WispBoostLevel.LEVEL3:
			result = EventBossParamTable.GetItemData(EventBossParamTableItem.BoostSpeed3);
			break;
		}
		return result;
	}
}
