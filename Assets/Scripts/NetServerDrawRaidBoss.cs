using LitJson;
using System;

public class NetServerDrawRaidBoss : NetBase
{
	private int m_eventId;

	private long m_score;

	private ServerEventRaidBossState m_raidBossState;

	public ServerEventRaidBossState RaidBossState
	{
		get
		{
			return this.m_raidBossState;
		}
	}

	public NetServerDrawRaidBoss(int eventId, long score)
	{
		this.m_eventId = eventId;
		this.m_score = score;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/drawRaidboss");
		base.WriteActionParamValue("eventId", this.m_eventId);
		base.WriteActionParamValue("score", this.m_score);
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_raidBossState = NetUtil.AnalyzeRaidBossState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
