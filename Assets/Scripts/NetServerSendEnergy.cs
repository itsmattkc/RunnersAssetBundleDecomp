using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerSendEnergy : NetBase
{
	private string _paramFriendId_k__BackingField;

	private int _resultExpireTime_k__BackingField;

	public string paramFriendId
	{
		get;
		set;
	}

	public int resultExpireTime
	{
		get;
		private set;
	}

	public NetServerSendEnergy()
	{
	}

	public NetServerSendEnergy(string friendId)
	{
		this.paramFriendId = friendId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Message/sendEnergy");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string sendEnergyString = instance.GetSendEnergyString(this.paramFriendId);
			base.WriteJsonString(sendEnergyString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_ExpireTime(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_FriendId()
	{
		base.WriteActionParamValue("friendId", this.paramFriendId);
	}

	private void GetResponse_ExpireTime(JsonData jdata)
	{
		this.resultExpireTime = NetUtil.GetJsonInt(jdata, "expireTime");
	}
}
