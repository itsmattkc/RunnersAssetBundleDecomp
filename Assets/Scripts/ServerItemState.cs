using System;

public class ServerItemState
{
	public int m_itemId;

	public int m_num;

	public ServerItemState()
	{
		this.m_itemId = 0;
		this.m_num = 0;
	}

	public void CopyTo(ServerItemState to)
	{
		to.m_itemId = this.m_itemId;
		to.m_num = this.m_num;
	}

	public ServerItem GetItem()
	{
		if (this.m_itemId >= 0)
		{
			return new ServerItem((ServerItem.Id)this.m_itemId);
		}
		return default(ServerItem);
	}
}
