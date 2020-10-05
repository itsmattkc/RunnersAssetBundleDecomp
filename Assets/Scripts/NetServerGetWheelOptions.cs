using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetWheelOptions : NetBase
{
	private ServerWheelOptions _resultWheelOptions_k__BackingField;

	public ServerWheelOptions resultWheelOptions
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Spin/getWheelOptions");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_WheelOptions(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultWheelOptions = ServerInterface.WheelOptions;
		this.resultWheelOptions.RefreshFakeState();
	}

	private void GetResponse_WheelOptions(JsonData jdata)
	{
		this.resultWheelOptions = NetUtil.AnalyzeWheelOptionsJson(jdata, "wheelOptions");
	}
}
