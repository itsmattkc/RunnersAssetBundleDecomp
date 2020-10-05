using System;

public class ServerMileageIncentive
{
	public enum Type
	{
		NONE,
		POINT,
		CHAPTER,
		EPISODE,
		FRIEND
	}

	public ServerMileageIncentive.Type m_type;

	public int m_itemId;

	public int m_num;

	public int m_pointId;

	public string m_friendId;

	public ServerMileageIncentive()
	{
		this.m_type = ServerMileageIncentive.Type.NONE;
		this.m_itemId = 0;
		this.m_num = 0;
		this.m_pointId = 0;
		this.m_friendId = null;
	}

	public void CopyTo(ServerMileageIncentive to)
	{
		if (to != null)
		{
			to.m_type = this.m_type;
			to.m_itemId = this.m_itemId;
			to.m_num = this.m_num;
			to.m_pointId = this.m_pointId;
			to.m_friendId = this.m_friendId;
		}
	}
}
