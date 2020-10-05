using App;
using System;

public class ServerLeaderboardEntry
{
	public string m_hspId;

	public long m_score;

	public long m_hiScore;

	public int m_userData;

	public string m_name;

	public string m_url;

	public bool m_energyFlg;

	public int m_grade;

	public int m_gradeChanged;

	public long m_expireTime;

	public int m_numRank;

	public long m_loginTime;

	public int m_charaId;

	public int m_charaLevel;

	public int m_subCharaId;

	public int m_subCharaLevel;

	public int m_mainChaoId;

	public int m_mainChaoLevel;

	public int m_subChaoId;

	public int m_subChaoLevel;

	public Env.Language m_language;

	public int m_leagueIndex;

	public ServerLeaderboardEntry()
	{
		this.m_hspId = "0123456789abcdef";
		this.m_score = 0L;
		this.m_hiScore = 0L;
		this.m_userData = 0;
		this.m_name = "0123456789abcdef";
		this.m_url = "0123456789abcdef";
		this.m_energyFlg = false;
		this.m_grade = 0;
		this.m_gradeChanged = 0;
		this.m_expireTime = 0L;
	}

	public void CopyTo(ServerLeaderboardEntry to)
	{
		to.m_gradeChanged = ((to.m_grade == 0) ? 0 : (to.m_grade - this.m_grade));
		to.m_hspId = this.m_hspId;
		to.m_score = this.m_score;
		to.m_hiScore = this.m_hiScore;
		to.m_userData = this.m_userData;
		to.m_name = this.m_name;
		to.m_url = this.m_url;
		to.m_energyFlg = this.m_energyFlg;
		to.m_grade = this.m_grade;
		to.m_expireTime = this.m_expireTime;
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
		to.m_leagueIndex = this.m_leagueIndex;
	}

	public ServerLeaderboardEntry Clone()
	{
		return new ServerLeaderboardEntry
		{
			m_gradeChanged = this.m_gradeChanged,
			m_hspId = this.m_hspId,
			m_score = this.m_score,
			m_hiScore = this.m_hiScore,
			m_userData = this.m_userData,
			m_name = this.m_name,
			m_url = this.m_url,
			m_energyFlg = this.m_energyFlg,
			m_grade = this.m_grade,
			m_expireTime = this.m_expireTime,
			m_numRank = this.m_numRank,
			m_loginTime = this.m_loginTime,
			m_charaId = this.m_charaId,
			m_charaLevel = this.m_charaLevel,
			m_subCharaId = this.m_subCharaId,
			m_subCharaLevel = this.m_subCharaLevel,
			m_mainChaoId = this.m_mainChaoId,
			m_mainChaoLevel = this.m_mainChaoLevel,
			m_subChaoId = this.m_subChaoId,
			m_subChaoLevel = this.m_subChaoLevel,
			m_language = this.m_language,
			m_leagueIndex = this.m_leagueIndex
		};
	}

	public bool IsSentEnergy()
	{
		return this.m_energyFlg;
	}
}
