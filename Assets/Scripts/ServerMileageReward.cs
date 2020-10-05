using System;

public class ServerMileageReward
{
	public int m_episode;

	public int m_chapter;

	public int m_type;

	public int m_point;

	public int m_itemId;

	public int m_numItem;

	public int m_limitTime;

	public DateTime m_startTime;

	public ServerMileageReward()
	{
		this.m_episode = 1;
		this.m_chapter = 1;
		this.m_type = 1;
		this.m_point = 0;
		this.m_itemId = 0;
		this.m_numItem = 0;
		this.m_limitTime = 0;
	}

	public void CopyTo(ServerMileageReward to)
	{
		if (to != null)
		{
			to.m_episode = this.m_episode;
			to.m_chapter = this.m_chapter;
			to.m_type = this.m_type;
			to.m_point = this.m_point;
			to.m_itemId = this.m_itemId;
			to.m_numItem = this.m_numItem;
			to.m_limitTime = this.m_limitTime;
			to.m_startTime = this.m_startTime;
		}
	}
}
