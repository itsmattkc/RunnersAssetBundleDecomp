using System;

public class ServerMileageMapState
{
	public int m_episode;

	public int m_chapter;

	public int m_point;

	public int m_numBossAttack;

	public long m_stageTotalScore;

	public long m_stageMaxScore;

	public DateTime m_chapterStartTime;

	public ServerMileageMapState()
	{
		this.m_episode = 1;
		this.m_chapter = 1;
		this.m_point = 0;
		this.m_numBossAttack = 0;
		this.m_stageTotalScore = 0L;
		this.m_stageMaxScore = 0L;
		this.m_chapterStartTime = DateTime.Now;
	}

	public void CopyTo(ServerMileageMapState to)
	{
		if (to != null)
		{
			to.m_episode = this.m_episode;
			to.m_chapter = this.m_chapter;
			to.m_point = this.m_point;
			to.m_numBossAttack = this.m_numBossAttack;
			to.m_stageTotalScore = this.m_stageTotalScore;
			to.m_stageMaxScore = this.m_stageMaxScore;
			to.m_chapterStartTime = this.m_chapterStartTime;
		}
	}
}
