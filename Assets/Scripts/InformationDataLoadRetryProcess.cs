using System;
using UnityEngine;

internal class InformationDataLoadRetryProcess : ServerRetryProcess
{
	private TitleDataLoader m_loader;

	public InformationDataLoadRetryProcess(GameObject returnObject, TitleDataLoader loader) : base(returnObject)
	{
		this.m_loader = loader;
	}

	public override void Retry()
	{
		if (this.m_loader != null)
		{
			this.m_loader.RetryInformationDataLoad();
		}
	}
}
