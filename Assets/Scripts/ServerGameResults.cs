using System;

public class ServerGameResults
{
	public long m_score;

	public long m_numRings;

	public long m_numFailureRings;

	public long m_numRedStarRings;

	public long m_distance;

	public long m_dailyMissionValue;

	public bool m_dailyMissionComplete;

	public bool m_isSuspended;

	public long m_numAnimals;

	public int m_reachPoint;

	public bool m_clearChapter;

	public int m_numBossAttack;

	public long m_maxChapterScore;

	public bool m_chaoEggPresent;

	public bool m_isBossDestroyed;

	public int m_maxComboCount;

	public int? m_eventId;

	public long? m_eventValue;

	public ServerGameResults(bool isSuspended, bool tutorialStage, bool chaoEggPresent, bool bossStage, int oldNumBossAttack, int? eventId, long? eventValue)
	{
		this.Initialize(oldNumBossAttack);
		this.m_isSuspended = isSuspended;
		this.m_eventId = eventId;
		this.m_eventValue = eventValue;
		LevelInformation levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
		if (!this.m_isSuspended)
		{
			StageScoreManager instance = StageScoreManager.Instance;
			PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
			if (instance != null && playerInformation != null && levelInformation != null)
			{
				this.m_score = ((!bossStage) ? instance.FinalScore : 0L);
				this.m_numRings = instance.FinalCountData.ring;
				this.m_numFailureRings = (long)playerInformation.NumLostRings;
				this.m_numRedStarRings = instance.FinalCountData.red_ring;
				this.m_distance = ((!bossStage) ? instance.FinalCountData.distance : 0L);
				this.m_numAnimals = instance.FinalCountData.animal;
				this.m_reachPoint = 0;
				this.m_clearChapter = false;
				if (levelInformation.NowBoss)
				{
					this.m_numBossAttack = ((!levelInformation.BossDestroy) ? levelInformation.NumBossAttack : 0);
				}
				this.m_maxComboCount = StageComboManager.Instance.MaxComboCount;
			}
			this.m_chaoEggPresent = chaoEggPresent;
		}
		if (levelInformation != null)
		{
			this.m_isBossDestroyed = levelInformation.BossDestroy;
		}
		StageMissionManager stageMissionManager = GameObjectUtil.FindGameObjectComponent<StageMissionManager>("StageMissionManager");
		if (stageMissionManager != null)
		{
			this.m_dailyMissionComplete = stageMissionManager.Completed;
			if (SaveDataManager.Instance != null)
			{
				DailyMissionData dailyMission = SaveDataManager.Instance.PlayerData.DailyMission;
				this.m_dailyMissionValue = dailyMission.progress;
			}
		}
	}

	public void SetMapProgress(MileageMapState prevMapInfo, ref long[] pointScore, bool existBossInChapter)
	{
		if (EventManager.Instance != null && EventManager.Instance.IsSpecialStage())
		{
			this.m_clearChapter = false;
			this.m_maxChapterScore = 0L;
			this.m_reachPoint = 0;
			return;
		}
		this.m_maxChapterScore = pointScore[5];
		if (!this.m_isSuspended)
		{
			LevelInformation levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
			if (prevMapInfo != null)
			{
				long score = prevMapInfo.m_score;
				long num = score + this.m_score;
				for (int i = 5; i >= 0; i--)
				{
					if (num >= pointScore[i])
					{
						this.m_reachPoint = i;
						break;
					}
				}
				if (this.m_reachPoint >= 5 && !existBossInChapter)
				{
					this.m_clearChapter = true;
				}
			}
			if (levelInformation != null && levelInformation.NowBoss && levelInformation.BossDestroy)
			{
				this.m_clearChapter = true;
			}
		}
	}

	private void Initialize(int oldNumBossAttack)
	{
		this.m_score = 0L;
		this.m_numRings = 0L;
		this.m_numFailureRings = 0L;
		this.m_numRedStarRings = 0L;
		this.m_distance = 0L;
		this.m_dailyMissionValue = 0L;
		this.m_dailyMissionComplete = false;
		this.m_numAnimals = 0L;
		this.m_reachPoint = 0;
		this.m_clearChapter = false;
		this.m_numBossAttack = oldNumBossAttack;
		this.m_chaoEggPresent = false;
		this.m_isSuspended = false;
	}
}
