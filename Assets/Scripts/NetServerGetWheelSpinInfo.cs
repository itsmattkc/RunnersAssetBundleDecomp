using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetWheelSpinInfo : NetBase
{
	private List<ServerWheelSpinInfo> _resultWheelSpinInfos_k__BackingField;

	public List<ServerWheelSpinInfo> resultWheelSpinInfos
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Spin/getWheelSpinInfo");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_WheelSpinInfo(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_WheelSpinInfo(JsonData jdata)
	{
		this.resultWheelSpinInfos = NetUtil.AnalyzeWheelSpinInfo(jdata, "infoList");
	}
}
