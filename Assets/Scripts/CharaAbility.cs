using System;

public class CharaAbility
{
	private uint[] m_ability_level = new uint[10];

	public uint[] Ability
	{
		get
		{
			return this.m_ability_level;
		}
		set
		{
			this.m_ability_level = value;
		}
	}

	public CharaAbility()
	{
		for (uint num = 0u; num < 10u; num += 1u)
		{
			this.m_ability_level[(int)((UIntPtr)num)] = 0u;
		}
	}

	public uint GetTotalLevel()
	{
		uint num = 0u;
		for (uint num2 = 0u; num2 < 10u; num2 += 1u)
		{
			num += this.m_ability_level[(int)((UIntPtr)num2)];
		}
		return num;
	}
}
