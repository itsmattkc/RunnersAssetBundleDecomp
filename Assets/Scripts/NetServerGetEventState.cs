using LitJson;
using System;

public class NetServerGetEventState : NetBase
{
	public int paramEventId;

	public ServerEventState resultEventState;

	public NetServerGetEventState(int eventId)
	{
		this.paramEventId = eventId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/getEventState");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getEventStateString = instance.GetGetEventStateString(this.paramEventId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getEventStateString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_EventState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_EventId()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
	}

	private void GetResponse_EventState(JsonData jdata)
	{
		this.resultEventState = NetUtil.AnalyzeEventState(jdata);
	}
}
