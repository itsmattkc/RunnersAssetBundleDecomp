using System;

public class ServerLoginBonusState
{
	public int m_numLogin;

	public int m_numBonus;

	public DateTime m_lastBonusTime;

	public ServerLoginBonusState()
	{
		this.m_numLogin = 0;
		this.m_numBonus = 0;
		this.m_lastBonusTime = DateTime.Now;
	}

	public void CopyTo(ServerLoginBonusState dest)
	{
		dest.m_numLogin = this.m_numLogin;
		dest.m_numBonus = this.m_numBonus;
		dest.m_lastBonusTime = this.m_lastBonusTime;
	}
}
