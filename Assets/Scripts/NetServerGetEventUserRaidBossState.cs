using LitJson;
using System;

public class NetServerGetEventUserRaidBossState : NetBase
{
	private int m_eventId;

	private ServerEventUserRaidBossState m_userRaidBossState;

	public ServerEventUserRaidBossState UserRaidBossState
	{
		get
		{
			return this.m_userRaidBossState;
		}
	}

	public NetServerGetEventUserRaidBossState(int eventId)
	{
		this.m_eventId = eventId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/getEventUserRaidboss");
		base.WriteActionParamValue("eventId", this.m_eventId);
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_userRaidBossState = NetUtil.AnalyzeEventUserRaidBossState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
