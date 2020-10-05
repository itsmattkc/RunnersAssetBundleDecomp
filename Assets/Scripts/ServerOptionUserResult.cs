using System;

public class ServerOptionUserResult
{
	public long m_totalSumHightScore;

	public long m_quickTotalSumHightScore;

	public long m_numTakeAllRings;

	public long m_numTakeAllRedRings;

	public int m_numChaoRoulette;

	public int m_numItemRoulette;

	public int m_numJackPot;

	public int m_numMaximumJackPotRings;

	public int m_numSupport;

	public void CopyTo(ServerOptionUserResult to)
	{
		if (to != null)
		{
			to.m_totalSumHightScore = this.m_totalSumHightScore;
			to.m_quickTotalSumHightScore = this.m_quickTotalSumHightScore;
			to.m_numTakeAllRings = this.m_numTakeAllRings;
			to.m_numTakeAllRedRings = this.m_numTakeAllRedRings;
			to.m_numChaoRoulette = this.m_numChaoRoulette;
			to.m_numItemRoulette = this.m_numItemRoulette;
			to.m_numJackPot = this.m_numJackPot;
			to.m_numMaximumJackPotRings = this.m_numMaximumJackPotRings;
			to.m_numSupport = this.m_numSupport;
		}
	}
}
