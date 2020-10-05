using System;

public class ServerNextVersionState
{
	public int m_buyRSRNum;

	public int m_freeRSRNum;

	public string m_userName;

	public string m_userId;

	public string m_url;

	public string m_eMsg;

	public string m_jMsg;

	public ServerNextVersionState()
	{
		this.m_buyRSRNum = 0;
		this.m_freeRSRNum = 0;
		this.m_userName = string.Empty;
		this.m_userId = string.Empty;
		this.m_url = string.Empty;
		this.m_eMsg = string.Empty;
		this.m_jMsg = string.Empty;
	}
}
