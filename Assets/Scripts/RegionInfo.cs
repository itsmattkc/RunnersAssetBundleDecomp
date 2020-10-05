using System;

public class RegionInfo
{
	private int m_countryId;

	private string m_countryCode;

	private string m_area;

	private string m_limit;

	public int CountryId
	{
		get
		{
			return this.m_countryId;
		}
		private set
		{
		}
	}

	public string CountryCode
	{
		get
		{
			return this.m_countryCode;
		}
		private set
		{
		}
	}

	public string Area
	{
		get
		{
			return this.m_area;
		}
		private set
		{
		}
	}

	public string Limit
	{
		get
		{
			return this.m_limit;
		}
		private set
		{
		}
	}

	public RegionInfo(int countryId, string countryCode, string area, string limit)
	{
		this.m_countryId = countryId;
		this.m_countryCode = countryCode;
		this.m_area = area;
		this.m_limit = limit;
	}
}
