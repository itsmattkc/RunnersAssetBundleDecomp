using System;

public class SocialUserData
{
	private string m_id;

	private string m_name;

	private string m_url;

	private bool m_isSilhouette;

	private SocialUserCustomData m_customData;

	public string Id
	{
		get
		{
			return this.m_id;
		}
		set
		{
			this.m_id = value;
		}
	}

	public string Name
	{
		get
		{
			return this.m_name;
		}
		set
		{
			this.m_name = value;
		}
	}

	public string Url
	{
		get
		{
			return this.m_url;
		}
		set
		{
			this.m_url = value;
		}
	}

	public bool IsSilhouette
	{
		get
		{
			return this.m_isSilhouette;
		}
		set
		{
			this.m_isSilhouette = value;
		}
	}

	public SocialUserCustomData CustomData
	{
		get
		{
			return this.m_customData;
		}
		set
		{
			this.m_customData = value;
		}
	}

	public SocialUserData()
	{
		this.m_id = string.Empty;
		this.m_name = string.Empty;
		this.m_url = string.Empty;
		this.m_customData = new SocialUserCustomData();
	}
}
