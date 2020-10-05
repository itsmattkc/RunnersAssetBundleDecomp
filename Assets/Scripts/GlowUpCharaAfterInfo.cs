using System;
using System.Collections.Generic;

public class GlowUpCharaAfterInfo
{
	private int m_level;

	private int m_levelUpCost;

	private int m_exp;

	private List<AbilityType> m_abilityList;

	private List<int> m_abilityListExp;

	public int level
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

	public int levelUpCost
	{
		get
		{
			return this.m_levelUpCost;
		}
		set
		{
			this.m_levelUpCost = value;
		}
	}

	public int exp
	{
		get
		{
			return this.m_exp;
		}
		set
		{
			this.m_exp = value;
		}
	}

	public List<AbilityType> abilityList
	{
		get
		{
			return this.m_abilityList;
		}
		set
		{
			this.m_abilityList = value;
		}
	}

	public List<int> abilityListExp
	{
		get
		{
			return this.m_abilityListExp;
		}
		set
		{
			this.m_abilityListExp = value;
		}
	}
}
