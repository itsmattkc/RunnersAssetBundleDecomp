using LitJson;
using System;
using System.Collections.Generic;

public class NetServerGetEventUserRaidBossList : NetBase
{
	private int m_eventId;

	private List<ServerEventRaidBossState> m_userRaidBossList;

	private ServerEventUserRaidBossState m_userRaidBossState;

	public List<ServerEventRaidBossState> UserRaidBossList
	{
		get
		{
			return this.m_userRaidBossList;
		}
	}

	public ServerEventUserRaidBossState UserRaidBossState
	{
		get
		{
			return this.m_userRaidBossState;
		}
	}

	public NetServerGetEventUserRaidBossList(int eventId)
	{
		this.m_eventId = eventId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/getEventUserRaidbossList");
		base.WriteActionParamValue("eventId", this.m_eventId);
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_userRaidBossList = NetUtil.AnalyzeRaidBossStateList(jdata);
		this.m_userRaidBossState = NetUtil.AnalyzeEventUserRaidBossState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
