using System;
using System.Collections.Generic;

public class AbilityUpParamTable
{
	private Dictionary<AbilityType, AbilityUpParamList> m_table;

	public AbilityUpParamTable()
	{
		this.m_table = new Dictionary<AbilityType, AbilityUpParamList>();
	}

	public void AddList(AbilityType type, AbilityUpParamList list)
	{
		if (this.m_table == null)
		{
			return;
		}
		this.m_table.Add(type, list);
	}

	public AbilityUpParamList GetList(AbilityType type)
	{
		if (this.m_table == null)
		{
			return null;
		}
		if (!this.m_table.ContainsKey(type))
		{
			return null;
		}
		return this.m_table[type];
	}
}
