using System;

public class ServerRedStarItemState
{
	public int m_storeItemId;

	public int m_itemId;

	public int m_numItem;

	public int m_price;

	public string m_priceDisp;

	public string m_productId;

	public ServerRedStarItemState()
	{
		this.m_storeItemId = 0;
		this.m_itemId = 0;
		this.m_numItem = 0;
		this.m_price = 0;
		this.m_priceDisp = string.Empty;
		this.m_productId = string.Empty;
	}

	public void CopyTo(ServerRedStarItemState dest)
	{
		dest.m_storeItemId = this.m_storeItemId;
		dest.m_itemId = this.m_itemId;
		dest.m_numItem = this.m_numItem;
		dest.m_price = this.m_price;
		dest.m_priceDisp = this.m_priceDisp;
		dest.m_productId = this.m_productId;
	}

	public void Dump()
	{
		UnityEngine.Debug.Log(string.Format("storeItemId={0}, itemId={1}, numItem={2}, price={3}", new object[]
		{
			this.m_storeItemId,
			this.m_itemId,
			this.m_numItem,
			this.m_price
		}));
	}
}
