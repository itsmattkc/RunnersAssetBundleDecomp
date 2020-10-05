using LitJson;
using System;

public class NetServerSetInviteHistory : NetBase
{
	public string facebookIdHash;

	public NetServerSetInviteHistory() : this(string.Empty)
	{
	}

	public NetServerSetInviteHistory(string facebookIdHash)
	{
		this.facebookIdHash = facebookIdHash;
	}

	protected override void DoRequest()
	{
		base.SetAction("Friend/setInviteHistory");
		this.SetParameter_Data();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Data()
	{
		base.WriteActionParamValue("facebookIdHash", this.facebookIdHash);
	}
}
