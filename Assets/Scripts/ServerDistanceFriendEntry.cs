using System;

public class ServerDistanceFriendEntry
{
	public string m_friendId;

	public int m_distance;

	public ServerDistanceFriendEntry()
	{
		this.m_friendId = string.Empty;
		this.m_distance = 0;
	}

	public void CopyTo(ServerDistanceFriendEntry to)
	{
		to.m_friendId = this.m_friendId;
		to.m_distance = this.m_distance;
	}
}
