using System;

public class ComboChaoAbilityData
{
	public ChaoAbility m_chaoAbility;

	public ComboChaoAbilityType m_type;

	public float m_timeMax;

	public ComboChaoAbilityData(ChaoAbility chaoAbility, ComboChaoAbilityType type, float timeMax)
	{
		this.m_chaoAbility = chaoAbility;
		this.m_type = type;
		this.m_timeMax = timeMax;
	}
}
