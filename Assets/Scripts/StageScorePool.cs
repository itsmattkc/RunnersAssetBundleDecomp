using System;

public class StageScorePool
{
	public static readonly int ArrayCount = 30000;

	public StageScoreData[] scoreDatas = new StageScoreData[StageScorePool.ArrayCount];

	private int m_objectIndex;

	private int m_lastDistance;

	public int StoredCount
	{
		get
		{
			return this.m_objectIndex;
		}
		private set
		{
		}
	}

	public void CheckHalfWay()
	{
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null && this.m_objectIndex != 0)
		{
			if (StageModeManager.Instance != null && StageModeManager.Instance.IsQuickMode())
			{
				ServerQuickModeGameResults gameResult = new ServerQuickModeGameResults(false);
				instance.CheckNativeHalfWayQuickModeResultScore(gameResult);
			}
			else
			{
				ServerGameResults gameResult2 = new ServerGameResults(false, false, false, false, 0, null, null);
				instance.CheckNativeHalfWayResultScore(gameResult2);
			}
		}
		this.m_objectIndex = 0;
	}

	public void AddScore(StageScoreData scoreData)
	{
		this.AddScore((ScoreType)scoreData.scoreType, scoreData.scoreValue);
	}

	public void AddScore(ScoreType type, int score)
	{
		if (this.m_objectIndex >= StageScorePool.ArrayCount)
		{
			UnityEngine.Debug.Log("StageScorePool arraySize over");
			return;
		}
		this.scoreDatas[this.m_objectIndex].scoreType = (byte)type;
		if (type == ScoreType.distance)
		{
			int scoreValue = score - this.m_lastDistance;
			this.scoreDatas[this.m_objectIndex].scoreValue = scoreValue;
			this.m_lastDistance = score;
		}
		else
		{
			this.scoreDatas[this.m_objectIndex].scoreValue = score;
		}
		this.m_objectIndex++;
	}

	public void DebugLog()
	{
		UnityEngine.Debug.Log("StageScorePool.CurrentDataSize = " + this.m_objectIndex.ToString());
	}
}
