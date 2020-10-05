using LitJson;
using System;

public class NetManagementResetRankingStatus : NetBase
{
	protected override void DoRequest()
	{
		base.SetAction("Management/resetRankingStatus");
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
