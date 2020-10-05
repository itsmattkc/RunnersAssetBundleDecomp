using System;
using UnityEngine;

public class ServerEventGameResults
{
	public long m_numRings;

	public long m_numFailureRings;

	public long m_numRedStarRings;

	public bool m_isSuspended;

	public long m_dailyMissionValue;

	public bool m_dailyMissionComplete;

	public int m_eventId;

	public long m_eventValue;

	public long m_raidBossId;

	public int m_raidBossDamage;

	public bool m_isRaidBossBeat;

	public ServerEventGameResults(bool isSuspended, int eventId, long eventValue, long raidBossId)
	{
		this.m_isSuspended = isSuspended;
		this.m_numRings = 0L;
		this.m_numFailureRings = 0L;
		this.m_numRedStarRings = 0L;
		this.m_dailyMissionValue = 0L;
		this.m_dailyMissionComplete = false;
		this.m_eventId = eventId;
		this.m_eventValue = eventValue;
		this.m_raidBossId = raidBossId;
		this.m_raidBossDamage = 0;
		this.m_isRaidBossBeat = false;
		if (!this.m_isSuspended)
		{
			StageScoreManager instance = StageScoreManager.Instance;
			if (instance != null)
			{
				this.m_numRedStarRings = instance.FinalCountData.red_ring;
			}
			LevelInformation levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
			if (levelInformation != null)
			{
				int num = 0;
				RaidBossData currentRaidData = RaidBossInfo.currentRaidData;
				if (currentRaidData != null)
				{
					num = (int)(currentRaidData.hpMax - currentRaidData.hp);
				}
				this.m_raidBossDamage = Mathf.Max(levelInformation.NumBossAttack - num, 0);
				this.m_isRaidBossBeat = levelInformation.BossDestroy;
			}
		}
	}
}
