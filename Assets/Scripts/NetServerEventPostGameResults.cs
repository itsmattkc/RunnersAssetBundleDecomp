using LitJson;
using System;

public class NetServerEventPostGameResults : NetBase
{
	private int m_eventId = -1;

	private int m_numRaidBossRings;

	private ServerEventUserRaidBossState m_userRaidBossState;

	public ServerEventUserRaidBossState UserRaidBossState
	{
		get
		{
			return this.m_userRaidBossState;
		}
	}

	public NetServerEventPostGameResults(int eventId, int numRaidBossRings)
	{
		this.m_eventId = eventId;
		this.m_numRaidBossRings = numRaidBossRings;
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/eventPostGameResults");
		this.SetParameter_EventId();
		this.SetParameter_RaidBossRings();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_EventUserRaidBossState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_EventId()
	{
		base.WriteActionParamValue("eventId", this.m_eventId);
	}

	private void SetParameter_RaidBossRings()
	{
		base.WriteActionParamValue("numRaidbossRings", this.m_numRaidBossRings);
	}

	private void GetResponse_EventUserRaidBossState(JsonData jdata)
	{
		this.m_userRaidBossState = NetUtil.AnalyzeEventUserRaidBossState(jdata);
	}
}
