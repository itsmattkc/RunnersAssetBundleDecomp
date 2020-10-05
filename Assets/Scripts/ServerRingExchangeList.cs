using System;

public class ServerRingExchangeList
{
	public int m_ringItemId;

	public int m_itemId;

	public int m_itemNum;

	public int m_price;

	public ServerRingExchangeList()
	{
		this.m_itemId = 0;
		this.m_itemNum = 0;
		this.m_price = 0;
	}

	public void Dump()
	{
		UnityEngine.Debug.Log(string.Format("itemId={0}, itemNum={1}, price={2}", this.m_itemId, this.m_itemNum, this.m_price));
	}

	public void CopyTo(ServerRingExchangeList dest)
	{
		dest.m_ringItemId = this.m_ringItemId;
		dest.m_itemId = this.m_itemId;
		dest.m_itemNum = this.m_itemNum;
		dest.m_price = this.m_price;
	}
}
