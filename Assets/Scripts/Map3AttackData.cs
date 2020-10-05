using System;

public class Map3AttackData
{
	public BossAttackType m_type;

	public int m_randA;

	public BossAttackPos m_posA;

	public int m_randB;

	public BossAttackPos m_posB;

	public Map3AttackData()
	{
		this.m_type = BossAttackType.NONE;
		this.m_randA = 0;
		this.m_posA = BossAttackPos.NONE;
		this.m_randB = 0;
		this.m_posB = BossAttackPos.NONE;
	}

	public Map3AttackData(BossAttackType type, int randA, BossAttackPos posA, int randB, BossAttackPos posB)
	{
		this.m_type = type;
		this.m_randA = randA;
		this.m_posA = posA;
		this.m_randB = randB;
		this.m_posB = posB;
	}

	public int GetAttackCount()
	{
		return BossMap3Table.GetBossAttackCount(this.m_type);
	}
}
