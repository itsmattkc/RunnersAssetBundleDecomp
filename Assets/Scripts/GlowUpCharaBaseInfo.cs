using System;

public class GlowUpCharaBaseInfo
{
	private CharaType m_charaType;

	private int m_level;

	private int m_levelUpCost;

	private int m_currentExp;

	private bool m_isActive;

	public CharaType charaType
	{
		get
		{
			return this.m_charaType;
		}
		set
		{
			this.m_charaType = value;
		}
	}

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

	public int currentExp
	{
		get
		{
			return this.m_currentExp;
		}
		set
		{
			this.m_currentExp = value;
		}
	}

	public bool IsActive
	{
		get
		{
			return this.m_isActive;
		}
		set
		{
			this.m_isActive = value;
		}
	}
}
