using System;

public class ComboChaoParam
{
	public ChaoAbility m_chaoAbility;

	public ComboChaoAbilityType m_type;

	public float m_extraValue;

	public int m_comboNum;

	public int m_nextCombo;

	public int m_continuationNum;

	public bool m_movement;

	public ComboChaoParam(ChaoAbility chaoAbility, ComboChaoAbilityType type, float extra, int comboNum, int nextCombo)
	{
		this.m_chaoAbility = chaoAbility;
		this.m_type = type;
		this.m_extraValue = extra;
		this.m_comboNum = comboNum;
		this.m_nextCombo = nextCombo;
		this.m_continuationNum = 0;
		this.m_movement = false;
	}
}
