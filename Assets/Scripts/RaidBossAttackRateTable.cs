using System;
using System.Runtime.CompilerServices;

public class RaidBossAttackRateTable : IComparable
{
	private float[] _attackRate_k__BackingField;

	public float[] attackRate
	{
		get;
		set;
	}

	public RaidBossAttackRateTable()
	{
	}

	public RaidBossAttackRateTable(float[] data)
	{
		this.attackRate = data;
	}

	public int CompareTo(object obj)
	{
		if (this == (RaidBossAttackRateTable)obj)
		{
			return 0;
		}
		return -1;
	}
}
