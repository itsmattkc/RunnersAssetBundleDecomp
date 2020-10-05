using System;

public class ServerEventRaidBossUserState
{
	private string m_wrestleId;

	private string m_name;

	private int m_grade;

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

	private int m_wrestleCount;

	private int m_wrestleDamage;

	private bool m_wrestleBeatFlg;

	public string WrestleId
	{
		get
		{
			return this.m_wrestleId;
		}
		set
		{
			this.m_wrestleId = value;
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

	public int Grade
	{
		get
		{
			return this.m_grade;
		}
		set
		{
			this.m_grade = value;
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

	public int WrestleCount
	{
		get
		{
			return this.m_wrestleCount;
		}
		set
		{
			this.m_wrestleCount = value;
		}
	}

	public int WrestleDamage
	{
		get
		{
			return this.m_wrestleDamage;
		}
		set
		{
			this.m_wrestleDamage = value;
		}
	}

	public bool WrestleBeatFlg
	{
		get
		{
			return this.m_wrestleBeatFlg;
		}
		set
		{
			this.m_wrestleBeatFlg = value;
		}
	}

	public ServerEventRaidBossUserState()
	{
		this.m_wrestleId = string.Empty;
		this.m_name = string.Empty;
		this.m_grade = 0;
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
		this.m_wrestleCount = 0;
		this.m_wrestleDamage = 0;
		this.m_wrestleBeatFlg = false;
	}

	public void CopyTo(ServerEventRaidBossUserState to)
	{
		to.m_wrestleId = this.m_wrestleId;
		to.m_name = this.m_name;
		to.m_grade = this.m_grade;
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
		to.m_wrestleCount = this.m_wrestleCount;
		to.m_wrestleDamage = this.m_wrestleDamage;
		to.m_wrestleBeatFlg = this.m_wrestleBeatFlg;
	}
}
