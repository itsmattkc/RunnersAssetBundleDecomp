using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetVariousParameter : NetBase
{
	private int _resultEnergyRecveryTime_k__BackingField;

	private int _resultEnergyRecoveryMax_k__BackingField;

	private int _resultOnePlayCmCount_k__BackingField;

	private int _resultOnePlayContinueCount_k__BackingField;

	private int _resultCmSkipCount_k__BackingField;

	private bool _resultIsPurchased_k__BackingField;

	public int resultEnergyRecveryTime
	{
		get;
		set;
	}

	public int resultEnergyRecoveryMax
	{
		get;
		set;
	}

	public int resultOnePlayCmCount
	{
		get;
		set;
	}

	public int resultOnePlayContinueCount
	{
		get;
		set;
	}

	public int resultCmSkipCount
	{
		get;
		set;
	}

	public bool resultIsPurchased
	{
		get;
		set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Login/getVariousParameter");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponseVariousParameter(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponseVariousParameter(JsonData jdata)
	{
		this.resultEnergyRecveryTime = NetUtil.GetJsonInt(jdata, "energyRecveryTime");
		this.resultEnergyRecoveryMax = NetUtil.GetJsonInt(jdata, "energyRecoveryMax");
		this.resultOnePlayCmCount = NetUtil.GetJsonInt(jdata, "onePlayCmCount");
		this.resultOnePlayContinueCount = NetUtil.GetJsonInt(jdata, "onePlayContinueCount");
		this.resultCmSkipCount = NetUtil.GetJsonInt(jdata, "cmSkipCount");
		this.resultIsPurchased = NetUtil.GetJsonFlag(jdata, "isPurchased");
	}
}
