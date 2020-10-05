using System;

public class AbilityUpParam
{
	private float m_abilityPotential;

	private float m_abilityCost;

	public float Potential
	{
		get
		{
			return this.m_abilityPotential;
		}
		set
		{
			this.m_abilityPotential = value;
		}
	}

	public float Cost
	{
		get
		{
			return this.m_abilityCost;
		}
		set
		{
			this.m_abilityCost = value;
		}
	}

	public AbilityUpParam()
	{
		this.m_abilityPotential = 0f;
		this.m_abilityCost = 0f;
	}
}
