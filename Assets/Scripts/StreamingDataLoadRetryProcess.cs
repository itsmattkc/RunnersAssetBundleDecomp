using System;
using UnityEngine;

internal class StreamingDataLoadRetryProcess : ServerRetryProcess
{
	private TitleDataLoader m_loader;

	private int m_retryCount;

	public StreamingDataLoadRetryProcess(int retriedCount, GameObject returnObject, TitleDataLoader loader) : base(returnObject)
	{
		this.m_retryCount = retriedCount;
		this.m_loader = loader;
	}

	public override void Retry()
	{
		this.m_retryCount++;
		if (this.m_loader != null)
		{
			this.m_loader.RetryStreamingDataLoad(this.m_retryCount);
		}
	}
}
