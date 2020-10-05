using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetChaoWheelOptions : NetBase
{
	private ServerChaoWheelOptions _resultChaoWheelOptions_k__BackingField;

	public ServerChaoWheelOptions resultChaoWheelOptions
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Chao/getChaoWheelOptions");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_ChaoWheelOptions(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_ChaoWheelOptions(JsonData jdata)
	{
		if (NetUtil.IsExist(jdata, "chaoWheelOptions"))
		{
			this.resultChaoWheelOptions = NetUtil.AnalyzeChaoWheelOptionsJson(jdata, "chaoWheelOptions");
		}
		else
		{
			this.resultChaoWheelOptions = NetUtil.AnalyzeChaoWheelOptionsJson(jdata, "wheelOptions");
		}
	}
}
