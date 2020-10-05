using LitJson;
using System;

public class NetServerActRetry : NetBase
{
	protected override void DoRequest()
	{
		base.SetAction("Game/actRetry");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string actRetryString = instance.GetActRetryString();
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(actRetryString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
