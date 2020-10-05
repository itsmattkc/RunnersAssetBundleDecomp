using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetDebugUpdMileageData : NetBase
{
	private ServerMileageMapState _mileageMapState_k__BackingField;

	public ServerMileageMapState mileageMapState
	{
		get;
		set;
	}

	public NetDebugUpdMileageData() : this(null)
	{
	}

	public NetDebugUpdMileageData(ServerMileageMapState mileageMapState)
	{
		this.mileageMapState = mileageMapState;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/updMileageData");
		this.SetParameter_MileageMapState();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_MileageMapState()
	{
		long num = (long)NetUtil.GetCurrentUnixTime();
		int num2 = 0;
		int num3 = 0;
		base.WriteActionParamObject("mileageMapState", new Dictionary<string, object>
		{
			{
				"episode",
				this.mileageMapState.m_episode
			},
			{
				"chapter",
				this.mileageMapState.m_chapter
			},
			{
				"point",
				this.mileageMapState.m_point
			},
			{
				"mapDistance",
				num2
			},
			{
				"numBossAttack",
				this.mileageMapState.m_numBossAttack
			},
			{
				"stageDistance",
				num3
			},
			{
				"chapterStartTime",
				num
			},
			{
				"stageTotalScore",
				this.mileageMapState.m_stageTotalScore
			},
			{
				"stageMaxScore",
				this.mileageMapState.m_stageMaxScore
			}
		});
	}
}
