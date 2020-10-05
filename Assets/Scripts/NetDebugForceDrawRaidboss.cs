using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugForceDrawRaidboss : NetBase
{
	private ServerEventRaidBossState m_raidBossState;

	private int _paramEventId_k__BackingField;

	private long _paramScore_k__BackingField;

	public int paramEventId
	{
		get;
		set;
	}

	public long paramScore
	{
		get;
		set;
	}

	public ServerEventRaidBossState RaidBossState
	{
		get
		{
			return this.m_raidBossState;
		}
	}

	public NetDebugForceDrawRaidboss() : this(0, 0L)
	{
	}

	public NetDebugForceDrawRaidboss(int eventId, long score)
	{
		this.paramEventId = eventId;
		this.paramScore = score;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/forceDrawRaidboss");
		this.SetParameter_User();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_raidBossState = NetUtil.AnalyzeRaidBossState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_User()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
		base.WriteActionParamValue("score", this.paramScore);
	}
}
