using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerAtomSerial : NetBase
{
	private string _campaign_k__BackingField;

	private string _serial_k__BackingField;

	private bool _new_user_k__BackingField;

	public string campaign
	{
		get;
		set;
	}

	public string serial
	{
		get;
		set;
	}

	public bool new_user
	{
		get;
		set;
	}

	public NetServerAtomSerial() : this(null, null, false)
	{
	}

	public NetServerAtomSerial(string campaign, string serial, bool new_user)
	{
		this.campaign = campaign;
		this.serial = serial;
		this.new_user = new_user;
	}

	protected override void DoRequest()
	{
		base.SetAction("Sgn/setSerialCode");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string setSerialCodeString = instance.GetSetSerialCodeString(this.campaign, this.serial, this.new_user);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(setSerialCodeString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Data()
	{
		base.WriteActionParamValue("campaignId", this.campaign);
		base.WriteActionParamValue("serialCode", this.serial);
		ushort num = 0;
		if (this.new_user)
		{
			num = 1;
		}
		base.WriteActionParamValue("newUser", num.ToString());
	}
}
