using System;
using System.Collections.Generic;

public class ServerFreeItemState
{
	private List<ServerItemState> m_itemList;

	private bool m_isExpired;

	public List<ServerItemState> itemList
	{
		get
		{
			return this.m_itemList;
		}
	}

	public ServerFreeItemState()
	{
		this.m_itemList = new List<ServerItemState>();
	}

	public void AddItem(ServerItemState item)
	{
		this.m_itemList.Add(item);
	}

	public void SetExpiredFlag(bool flag)
	{
		this.m_isExpired = flag;
	}

	public bool IsExpired()
	{
		return this.m_isExpired;
	}

	public void ClearList()
	{
		this.m_itemList.Clear();
	}

	public void CopyTo(ServerFreeItemState dest)
	{
		foreach (ServerItemState current in this.m_itemList)
		{
			dest.AddItem(current);
		}
		dest.m_isExpired = this.m_isExpired;
	}
}
