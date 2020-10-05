using LitJson;
using System;
using System.Collections.Generic;

public class NetServerGetEventRaidBossUserList : NetBase
{
	private int m_eventId;

	private long m_raidBossId;

	private List<ServerEventRaidBossUserState> m_raidBossUserStateList;

	private ServerEventRaidBossBonus m_raidBossBonus;

	private ServerEventRaidBossState m_raidBossState;

	public List<ServerEventRaidBossUserState> RaidBossUserStateList
	{
		get
		{
			return this.m_raidBossUserStateList;
		}
	}

	public ServerEventRaidBossBonus RaidBossBonus
	{
		get
		{
			return this.m_raidBossBonus;
		}
	}

	public ServerEventRaidBossState RaidBossState
	{
		get
		{
			return this.m_raidBossState;
		}
	}

	public NetServerGetEventRaidBossUserList(int eventId, long raidBossId)
	{
		this.m_eventId = eventId;
		this.m_raidBossId = raidBossId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/getEventRaidbossUserList");
		base.WriteActionParamValue("raidbossId", this.m_raidBossId);
		base.WriteActionParamValue("eventId", this.m_eventId);
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_raidBossUserStateList = NetUtil.AnalyzeEventRaidBossUserStateList(jdata);
		this.m_raidBossBonus = NetUtil.AnalyzeEventRaidBossBonus(jdata);
		this.m_raidBossState = NetUtil.AnalyzeRaidBossState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
