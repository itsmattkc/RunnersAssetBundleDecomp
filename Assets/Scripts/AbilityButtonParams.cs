using System;
using UnityEngine;

public class AbilityButtonParams
{
	private CharaType m_charaType;

	private AbilityType m_abilityType;

	private GameObject m_buttonObject;

	public CharaType Character
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

	public AbilityType Ability
	{
		get
		{
			return this.m_abilityType;
		}
		set
		{
			this.m_abilityType = value;
		}
	}
}
