using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugUpdDailyMission : NetBase
{
	private int _paramMissionId_k__BackingField;

	public int paramMissionId
	{
		get;
		set;
	}

	public NetDebugUpdDailyMission() : this(0)
	{
	}

	public NetDebugUpdDailyMission(int missionId)
	{
		this.paramMissionId = missionId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/updDailyMission");
		this.SetParameter_DailyMission();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_DailyMission()
	{
		base.WriteActionParamValue("missionId", this.paramMissionId);
	}
}
