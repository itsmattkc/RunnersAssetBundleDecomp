using LitJson;
using System;

public class NetManagementMakeWeeklyRankingData : NetBase
{
	protected override void DoRequest()
	{
		base.SetAction("Management/makeWeeklyRankingData");
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
