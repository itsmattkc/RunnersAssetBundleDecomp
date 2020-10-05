using LitJson;
using System;

public class NetDebugResetGameData : NetBase
{
	protected override void DoRequest()
	{
		base.SetAction("Debug/resetGameData");
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
