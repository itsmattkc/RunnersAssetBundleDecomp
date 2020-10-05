using System;
using UnityEngine;

internal class StreamingDataKeyRetryProcess : ServerRetryProcess
{
	private GameModeTitle m_title;

	public StreamingDataKeyRetryProcess(GameObject returnObject, GameModeTitle title) : base(returnObject)
	{
		this.m_title = title;
	}

	public override void Retry()
	{
		if (this.m_title != null)
		{
			this.m_title.StreamingKeyDataRetry();
		}
	}
}
