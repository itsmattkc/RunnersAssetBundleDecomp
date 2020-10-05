using LitJson;
using System;
using System.Collections.Generic;

public class NetServerGetEventList : NetBase
{
	public List<ServerEventEntry> resultEventList;

	protected override void DoRequest()
	{
		base.SetAction("Event/getEventList");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_EventList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_EventList(JsonData jdata)
	{
		this.resultEventList = NetUtil.AnalyzeEventList(jdata);
	}
}
