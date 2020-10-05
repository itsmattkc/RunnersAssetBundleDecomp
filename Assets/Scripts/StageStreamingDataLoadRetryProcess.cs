using System;
using UnityEngine;

internal class StageStreamingDataLoadRetryProcess : ServerRetryProcess
{
	private GameModeStage m_gameModeStage;

	private int m_retryCount;

	public StageStreamingDataLoadRetryProcess(GameObject returnObject, GameModeStage gameModeStage) : base(returnObject)
	{
		this.m_gameModeStage = gameModeStage;
	}

	public override void Retry()
	{
		this.m_retryCount++;
		if (this.m_gameModeStage != null)
		{
			this.m_gameModeStage.RetryStreamingDataLoad(this.m_retryCount);
		}
	}
}
