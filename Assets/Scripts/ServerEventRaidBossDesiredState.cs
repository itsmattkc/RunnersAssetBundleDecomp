using System;

public class ServerEventRaidBossDesiredState
{
	private string m_desireId;

	private string m_name;

	private int m_numRank;

	private int m_loginTime;

	private int m_charaId;

	private int m_charaLevel;

	private int m_subCharaId;

	private int m_subCharaLevel;

	private int m_mainChaoId;

	private int m_mainChaoLevel;

	private int m_subChaoId;

	private int m_subChaoLevel;

	private int m_language;

	private int m_league;

	private int m_numBeatedEnterprise;

	public string DesireId
	{
		get
		{
			return this.m_desireId;
		}
		set
		{
			this.m_desireId = value;
		}
	}

	public string UserName
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

	public int NumRank
	{
		get
		{
			return this.m_numRank;
		}
		set
		{
			this.m_numRank = value;
		}
	}

	public int LoginTime
	{
		get
		{
			return this.m_loginTime;
		}
		set
		{
			this.m_loginTime = value;
		}
	}

	public int CharaId
	{
		get
		{
			return this.m_charaId;
		}
		set
		{
			this.m_charaId = value;
		}
	}

	public int CharaLevel
	{
		get
		{
			return this.m_charaLevel;
		}
		set
		{
			this.m_charaLevel = value;
		}
	}

	public int SubCharaId
	{
		get
		{
			return this.m_subCharaId;
		}
		set
		{
			this.m_subCharaId = value;
		}
	}

	public int SubCharaLevel
	{
		get
		{
			return this.m_subCharaLevel;
		}
		set
		{
			this.m_subCharaLevel = value;
		}
	}

	public int MainChaoId
	{
		get
		{
			return this.m_mainChaoId;
		}
		set
		{
			this.m_mainChaoId = value;
		}
	}

	public int MainChaoLevel
	{
		get
		{
			return this.m_mainChaoLevel;
		}
		set
		{
			this.m_mainChaoLevel = value;
		}
	}

	public int SubChaoId
	{
		get
		{
			return this.m_subChaoId;
		}
		set
		{
			this.m_subChaoId = value;
		}
	}

	public int SubChaoLevel
	{
		get
		{
			return this.m_subChaoLevel;
		}
		set
		{
			this.m_subChaoLevel = value;
		}
	}

	public int Language
	{
		get
		{
			return this.m_language;
		}
		set
		{
			this.m_language = value;
		}
	}

	public int League
	{
		get
		{
			return this.m_league;
		}
		set
		{
			this.m_league = value;
		}
	}

	public int NumBeatedEnterprise
	{
		get
		{
			return this.m_numBeatedEnterprise;
		}
		set
		{
			this.m_numBeatedEnterprise = value;
		}
	}

	public ServerEventRaidBossDesiredState()
	{
		this.m_desireId = string.Empty;
		this.m_name = string.Empty;
		this.m_numRank = 0;
		this.m_loginTime = 0;
		this.m_charaId = 0;
		this.m_charaLevel = 0;
		this.m_subCharaId = 0;
		this.m_subCharaLevel = 0;
		this.m_mainChaoId = 0;
		this.m_mainChaoLevel = 0;
		this.m_subChaoId = 0;
		this.m_subChaoLevel = 0;
		this.m_language = 0;
		this.m_league = 0;
		this.m_numBeatedEnterprise = 0;
	}

	public void CopyTo(ServerEventRaidBossDesiredState to)
	{
		to.m_desireId = this.m_desireId;
		to.m_name = this.m_name;
		to.m_numRank = this.m_numRank;
		to.m_loginTime = this.m_loginTime;
		to.m_charaId = this.m_charaId;
		to.m_charaLevel = this.m_charaLevel;
		to.m_subCharaId = this.m_subCharaId;
		to.m_subCharaLevel = this.m_subCharaLevel;
		to.m_mainChaoId = this.m_mainChaoId;
		to.m_mainChaoLevel = this.m_mainChaoLevel;
		to.m_subChaoId = this.m_subChaoId;
		to.m_subChaoLevel = this.m_subChaoLevel;
		to.m_language = this.m_language;
		to.m_league = this.m_league;
		to.m_numBeatedEnterprise = this.m_numBeatedEnterprise;
	}
}
