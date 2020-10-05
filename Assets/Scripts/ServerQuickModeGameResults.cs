using System;

public class ServerQuickModeGameResults
{
	public long m_score;

	public long m_numRings;

	public long m_numFailureRings;

	public long m_numRedStarRings;

	public long m_distance;

	public long m_numAnimals;

	public int m_maxComboCount;

	public long m_dailyMissionValue;

	public bool m_dailyMissionComplete;

	public bool m_isSuspended;

	public ServerQuickModeGameResults(bool isSuspended)
	{
		this.Initialize();
		this.m_isSuspended = isSuspended;
		if (!this.m_isSuspended)
		{
			StageScoreManager instance = StageScoreManager.Instance;
			PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
			if (instance != null && playerInformation != null)
			{
				this.m_score = instance.FinalScore;
				this.m_numRings = instance.FinalCountData.ring;
				this.m_numFailureRings = (long)playerInformation.NumLostRings;
				this.m_numRedStarRings = instance.FinalCountData.red_ring;
				this.m_distance = instance.FinalCountData.distance;
				this.m_numAnimals = instance.FinalCountData.animal;
				this.m_maxComboCount = StageComboManager.Instance.MaxComboCount;
			}
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

	private void Initialize()
	{
		this.m_score = 0L;
		this.m_numRings = 0L;
		this.m_numFailureRings = 0L;
		this.m_numRedStarRings = 0L;
		this.m_distance = 0L;
		this.m_dailyMissionValue = 0L;
		this.m_numAnimals = 0L;
		this.m_dailyMissionComplete = false;
		this.m_isSuspended = false;
	}
}
