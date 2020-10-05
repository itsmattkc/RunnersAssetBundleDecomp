using LitJson;
using System;

public class NetServerGetPrizeChaoWheelSpin : NetBase
{
	public int paramChaoWheelSpinType;

	public ServerPrizeState resultPrizeState;

	public NetServerGetPrizeChaoWheelSpin(int chaoWheelSpinType)
	{
		this.paramChaoWheelSpinType = chaoWheelSpinType;
	}

	protected override void DoRequest()
	{
		base.SetAction("Chao/getPrizeChaoWheelSpin");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getPrizeChaoWheelSpinString = instance.GetGetPrizeChaoWheelSpinString(this.paramChaoWheelSpinType);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getPrizeChaoWheelSpinString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PrizeChaoWheelSpin(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_ChaoWheelSpinType()
	{
		base.WriteActionParamValue("chaoWheelSpinType", this.paramChaoWheelSpinType);
	}

	private void GetResponse_PrizeChaoWheelSpin(JsonData jdata)
	{
		this.resultPrizeState = NetUtil.AnalyzePrizeChaoWheelSpin(jdata);
	}
}
