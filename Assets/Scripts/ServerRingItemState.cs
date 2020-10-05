using System;

public class ServerRingItemState
{
	public int m_itemId;

	public int m_cost;

	public ServerRingItemState()
	{
		this.m_itemId = 0;
		this.m_cost = 0;
	}

	public void Dump()
	{
		UnityEngine.Debug.Log(string.Format("itemId={0}, cost={1}", this.m_itemId, this.m_cost));
	}
}
