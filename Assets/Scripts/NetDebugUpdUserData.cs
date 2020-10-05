using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugUpdUserData : NetBase
{
	private int _paramAddRank_k__BackingField;

	public int paramAddRank
	{
		get;
		set;
	}

	public NetDebugUpdUserData() : this(0)
	{
	}

	public NetDebugUpdUserData(int addRank)
	{
		this.paramAddRank = addRank;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/updUserData");
		this.SetParameter_AddRank();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_AddRank()
	{
		base.WriteActionParamValue("addRank", this.paramAddRank);
	}
}
