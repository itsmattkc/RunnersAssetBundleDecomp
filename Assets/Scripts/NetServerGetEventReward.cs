using LitJson;
using System;
using System.Collections.Generic;

public class NetServerGetEventReward : NetBase
{
	public int paramEventId;

	public List<ServerEventReward> resultEventRewardList;

	public NetServerGetEventReward(int eventId)
	{
		this.paramEventId = eventId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/getEventReward");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string eventRewardString = instance.GetEventRewardString(this.paramEventId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(eventRewardString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_EventRewardList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_EventId()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
	}

	private void GetResponse_EventRewardList(JsonData jdata)
	{
		this.resultEventRewardList = NetUtil.AnalyzeEventReward(jdata);
	}
}
