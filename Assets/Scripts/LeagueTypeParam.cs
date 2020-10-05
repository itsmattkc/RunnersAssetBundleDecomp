using System;

public class LeagueTypeParam
{
	public LeagueCategory m_category;

	public string m_categoryName;

	public LeagueTypeParam(LeagueCategory category, string categoryName)
	{
		this.m_category = category;
		this.m_categoryName = categoryName;
	}
}
