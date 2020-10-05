using System;

public class ServerFriendEntry
{
	[Flags]
	public enum FriendStateFlags
	{
		None = 0,
		SentEnergy = 1,
		RequestedEnergy = 2,
		Invited = 4
	}

	public string m_mid;

	public string m_name;

	public string m_url;

	public ServerFriendEntry.FriendStateFlags m_stateFlags;

	public ServerFriendEntry()
	{
		this.m_mid = "0123456789abcdef";
		this.m_name = "0123456789abcdef";
		this.m_url = "0123456789abcdef";
		this.m_stateFlags = ServerFriendEntry.FriendStateFlags.None;
	}

	public void CopyTo(ServerFriendEntry to)
	{
		to.m_mid = this.m_mid;
		to.m_name = this.m_name;
		to.m_url = this.m_url;
		to.m_stateFlags = this.m_stateFlags;
	}

	public bool IsInvited()
	{
		return ServerFriendEntry.FriendStateFlags.Invited == this.m_stateFlags;
	}
}
