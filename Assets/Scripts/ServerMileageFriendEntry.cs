using System;

public class ServerMileageFriendEntry : ServerFriendEntry
{
	public string m_friendId;

	public ServerMileageMapState m_mapState;

	public ServerMileageFriendEntry()
	{
		this.m_friendId = "0123456789abcdef";
		this.m_mapState = new ServerMileageMapState();
	}

	public void CopyTo(ServerMileageFriendEntry to)
	{
		if (to != null)
		{
			base.CopyTo(to);
			to.m_friendId = this.m_friendId;
			this.m_mapState.CopyTo(to.m_mapState);
		}
	}
}
