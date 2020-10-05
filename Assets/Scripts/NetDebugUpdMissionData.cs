using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetDebugUpdMissionData : NetBase
{
	private bool[] mParamMissionComplete;

	private int _paramMissionSet_k__BackingField;

	public int paramMissionSet
	{
		get;
		set;
	}

	public bool[] paramMissionComplete
	{
		get
		{
			return this.mParamMissionComplete;
		}
		set
		{
			this.mParamMissionComplete = (value.Clone() as bool[]);
		}
	}

	public NetDebugUpdMissionData() : this(0, new bool[3])
	{
	}

	public NetDebugUpdMissionData(int missionSet, params bool[] missionComplete)
	{
		this.paramMissionSet = missionSet;
		this.paramMissionComplete = missionComplete;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/updMissionData");
		this.SetParameter_Mission();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Mission()
	{
		if (this.paramMissionComplete != null)
		{
			base.WriteActionParamValue("missionSet", this.paramMissionSet);
			List<object> list = new List<object>();
			for (int i = 0; i < this.paramMissionComplete.Length; i++)
			{
				list.Add((!this.paramMissionComplete[i]) ? 0 : 1);
			}
			base.WriteActionParamArray("missionsComplete", list);
			list.Clear();
		}
	}
}
