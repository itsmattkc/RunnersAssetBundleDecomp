using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugResetUserData : NetBase
{
	private int _paramUserId_k__BackingField;

	public int paramUserId
	{
		get;
		set;
	}

	public NetDebugResetUserData()
	{
	}

	public NetDebugResetUserData(int userId)
	{
		this.paramUserId = userId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/resetUserData");
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
