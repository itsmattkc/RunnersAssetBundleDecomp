using System;

public class MileageMapState
{
	public int m_episode = 1;

	public int m_chapter = 1;

	public int m_point;

	public long m_score;

	public MileageMapState()
	{
	}

	public MileageMapState(ServerMileageMapState src)
	{
		this.Set(src);
	}

	public MileageMapState(MileageMapState src)
	{
		this.Set(src);
	}

	public void Set(ServerMileageMapState src)
	{
		if (src != null)
		{
			this.m_episode = src.m_episode;
			this.m_chapter = src.m_chapter;
			this.m_point = src.m_point;
			this.m_score = src.m_stageTotalScore;
		}
	}

	public void Set(MileageMapState src)
	{
		if (src != null)
		{
			this.m_episode = src.m_episode;
			this.m_chapter = src.m_chapter;
			this.m_point = src.m_point;
			this.m_score = src.m_score;
		}
	}
}
