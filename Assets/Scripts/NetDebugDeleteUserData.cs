using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugDeleteUserData : NetBase
{
	private int _paramUserId_k__BackingField;

	public int paramUserId
	{
		get;
		set;
	}

	public NetDebugDeleteUserData() : this(0)
	{
	}

	public NetDebugDeleteUserData(int userId)
	{
		this.paramUserId = userId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/deleteUserData");
		this.SetParameter_User();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_User()
	{
		base.WriteActionParamValue("userId", this.paramUserId);
	}
}
