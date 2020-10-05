using System;

public class ServerPresentState
{
	public int m_itemId;

	public int m_numItem;

	public int m_additionalInfo1;

	public int m_additionalInfo2;

	public ServerPresentState()
	{
		this.m_itemId = 0;
		this.m_numItem = 0;
		this.m_additionalInfo1 = 0;
		this.m_additionalInfo2 = 0;
	}

	public void CopyTo(ServerPresentState to)
	{
		to.m_itemId = this.m_itemId;
		to.m_numItem = this.m_numItem;
		to.m_additionalInfo1 = this.m_additionalInfo1;
		to.m_additionalInfo2 = this.m_additionalInfo2;
	}
}
